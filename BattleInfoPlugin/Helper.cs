using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin
{
    public static class Helper
    {
        public static IObservable<SvData<T>> Observe<T>(this KanColleProxy proxy, string path)
        {
            return proxy.ApiSessionSource
                .Where(x => new Uri(x.HttpClient.Request.Url).PathAndQuery == path)
                .TryParse<T>();
        }
    }
}
