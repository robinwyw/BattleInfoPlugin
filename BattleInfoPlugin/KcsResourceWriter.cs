using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using BattleInfoPlugin.Models.Repositories;
using BattleInfoPlugin.Models.Settings;
using BattleInfoPlugin.Properties;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using Titanium.Web.Proxy.EventArguments;

namespace BattleInfoPlugin
{
    class KcsResourceWriter
    {
        private int currentMapAreaId;
        private int currentMapInfoNo;
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, string>> resourceUrlMapping;

        public KcsResourceWriter()
        {
            this.resourceUrlMapping = PluginSettings.Paths.ResourceUrlMappingFileName.Deserialize<ConcurrentDictionary<int, ConcurrentDictionary<int, string>>>()
                                    .ValueOrNew();

            var proxy = KanColleClient.Current.Proxy;
            proxy.ApiSessionSource
                .Where(s => new Uri(s.HttpClient.Request.Url).PathAndQuery.StartsWith("/kcs/resources/swf/map"))
                .Subscribe(s => this.HttpGetMapResource(s));
            proxy.api_req_map_start
                .TryParse<kcsapi_map_start>()
                .Subscribe(x => this.ReqMapStart(x.Data));
        }

        private void HttpGetMapResource(SessionEventArgs s)
        {
            var filePath = new Uri(s.HttpClient.Request.Url).PathAndQuery.Split('?').First();
            s.SaveResponseBody(PluginSettings.Paths.CacheDirPath + filePath);

            Debug.WriteLine($"{this.currentMapAreaId}-{this.currentMapInfoNo}:{filePath}");

            this.resourceUrlMapping
                .GetOrAdd(this.currentMapAreaId, new ConcurrentDictionary<int, string>())
                .AddOrUpdate(this.currentMapInfoNo, filePath, (_, __) => filePath);
            this.resourceUrlMapping.Serialize(PluginSettings.Paths.ResourceUrlMappingFileName);
        }

        private void ReqMapStart(kcsapi_map_start data)
        {
            this.currentMapAreaId = data.api_maparea_id;
            this.currentMapInfoNo = data.api_mapinfo_no;
        }
    }

    static class KcsResourceWriterExtensions
    {
        private static readonly object lockObj = new object();

        public static void SaveResponseBody(this SessionEventArgs session, string filePath)
        {
            lock (lockObj)
            {
                var dir = Directory.GetParent(filePath);
                if (!dir.Exists) dir.Create();
                File.WriteAllBytes(filePath, session.HttpClient.Response?.Body);
            }
        }
    }
}
