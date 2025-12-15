using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin
{
    public static class JsonLogger
    {
        /// <summary>
        /// 将对象序列化为 JSON 字符串（使用 DataContractJsonSerializer）
        /// </summary>
        public static string SerializeForLog<T>(T obj)
        {
            if (obj == null)
                return "null";

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                // 如果序列化失败，可以返回异常信息
                return $"<Failed to serialize object: {ex.Message}>";
            }
        }
    }
}
