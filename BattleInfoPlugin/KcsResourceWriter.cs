using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using BattleInfoPlugin.Models.Settings;
using Nekoxy;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using System.Collections.Generic;
using BattleInfoPlugin.Properties;
using BattleInfoPlugin.Models.Repositories;
using System.Collections.Concurrent;
using System.Diagnostics;

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
                                    ?? new ConcurrentDictionary<int, ConcurrentDictionary<int, string>>();

            var proxy = KanColleClient.Current.Proxy;
            proxy.SessionSource
                .Where(s => s.Request.PathAndQuery.StartsWith("/kcs/resources/swf/map"))
                .Subscribe(s => this.HttpGetMapResource(s));
            proxy.api_req_map_start
                .TryParse<kcsapi_map_start>()
                .Subscribe(x => this.ReqMapStart(x.Data));
        }

        private void HttpGetMapResource(Session s)
        {
            var filePath = s.Request.PathAndQuery.Split('?').First();
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

        public static void SaveResponseBody(this Session session, string filePath)
        {
            lock (lockObj)
            {
                var dir = Directory.GetParent(filePath);
                if (!dir.Exists) dir.Create();
                File.WriteAllBytes(filePath, session.Response.Body);
            }
        }
    }
}
