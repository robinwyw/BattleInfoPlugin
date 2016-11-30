using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Repositories
{
    static class Extensions
    {
        private class SimpleEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _comparator;

            public SimpleEqualityComparer(Func<T, T, bool> comparator)
            {
                this._comparator = comparator;
            }

            public bool Equals(T x, T y)
            {
                return this._comparator(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparator)
        {
            return first.SequenceEqual(second, new SimpleEqualityComparer<T>(comparator));
        }

        public static bool EqualsValue<T>(this T[] array1, T[] array2)
        {
            if (array1 == array2) return true;
            if (array1 == null || array2 == null) return false;
            if (array1.Length != array2.Length) return false;
            return array1.SequenceEqual(array2, (x, y) => EqualsValue((dynamic)x, (dynamic)y));
        }
        public static bool EqualsValue<T>(this HashSet<T> set1, HashSet<T> set2)
        {
            if (set1 == set2) return true;
            if (set1 == null || set2 == null) return false;
            return set1.SetEquals(set2);
        }

        public static bool EqualsValue<T>(T obj1, T obj2)
        {
            return obj1.Equals(obj2);
        }

        public static int GetValuesHashCode<T>(this IEnumerable<T> ie, Func<T, int> valuesHashCodeFunc = null)
        {
            return ie?.Aggregate(new StringBuilder(), (b, v) => b.Append(valuesHashCodeFunc?.Invoke(v) ?? v?.ToString().GetHashCode() ?? 0))
            .ToString().GetHashCode()
            ?? 0;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null) return default(TValue);
            TValue p;
            return dictionary.TryGetValue(key, out p) ? p : default(TValue);
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> dic1,
            IDictionary<TKey, TValue> dic2,
            Func<TValue, TValue, TValue> updateValueFactory)
        {
            if (dic1 == null && dic2 == null) return new Dictionary<TKey, TValue>();

            if (dic2 == null || dic2.Count == 0) return dic1.ToDictionary(x => x.Key, x => x.Value);
            if (dic1 == null || dic1.Count == 0) return dic2.ToDictionary(x => x.Key, x => x.Value);

            var merged = new ConcurrentDictionary<TKey, TValue>(dic1);
            foreach (var newKvp in dic2)
            {
                merged.AddOrUpdate(newKvp.Key, newKvp.Value, (k, v) => updateValueFactory(v, newKvp.Value));
            }
            return merged.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> dic1,
            IDictionary<TKey, TValue> dic2,
            bool isSelectNewValue = true)
        {
            return dic1.Merge(dic2, (v1, v2) => isSelectNewValue ? v2 : v1);
        }

        public static HashSet<T> Merge<T>(this HashSet<T> h1, HashSet<T> h2)
        {
            h1.UnionWith(h2);
            return h1;
        }

        public static List<TElement> Merge<TElement, TKey>(this List<TElement> c1, List<TElement> c2, Func<TElement, TKey> keySelector)
        {
            var e = c2
                .Where(x => !c1.Any(y => keySelector(x).Equals(keySelector(y))))
                .ToArray();
            c1.AddRange(e);
            return c1;
        }

        public static HashSet<T> Merge<T>(this IEnumerable<HashSet<T>> sets)
        {
            var result = new HashSet<T>();
            foreach (var set in sets)
            {
                result.UnionWith(set);
            }
            return result;
        }

        private static readonly object serializeLoadLock = new object();
        public static void Serialize<T>(this T target, string path)
        {
            Debug.WriteLine("Start Serialize");
            var serializer = new DataContractJsonSerializer(typeof(T));
            lock (serializeLoadLock)
            {
                using (var stream = Stream.Synchronized(new FileStream(path, FileMode.Create, FileAccess.Write)))
                {
                    serializer.WriteObject(stream, target);
                }
            }
            Debug.WriteLine("End  Serialize");
        }
        public static T Deserialize<T>(this string path)
        {
            Debug.WriteLine("Start Deserialize");
            var serializer = new DataContractJsonSerializer(typeof(T));
            lock (serializeLoadLock)
            {
                if (!File.Exists(path)) return default(T);
                using (var stream = Stream.Synchronized(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    Debug.WriteLine("End  Deserialize");
                    return (T)serializer.ReadObject(stream);
                }
            }
        }

        public static string ToAbsolutePath(this string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            return path;
        }

        public static T ValueOrNew<T>(this T source)
            where T : class, new()
        {
            return source ?? new T();
        }

        public static T ValueOrDefault<T>(T? source)
            where T : struct
        {
            return source ?? default(T);
        }

        public static TValue GetOrAddNew<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
            where TValue : new()
        {
            TValue result;
            if (!source.TryGetValue(key, out result))
            {
                result = new TValue();
                source[key] = result;
            }
            return result;
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> FilterKeys<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IEnumerable<TKey> keys)
        {
            return keys
                .Where(source.ContainsKey)
                .Select(k => new KeyValuePair<TKey, TValue>(k, source[k]));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue[]>> FilterValue<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue[]>> source,
            TValue[] value)
        {
            return source.Where(pair => EqualsValue(pair.Value, value));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> FilterValue<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source,
            TValue value)
        {
            return source.Where(pair => EqualsValue(pair.Value, value));
        }

        public static TKey[] Filter<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IEnumerable<TKey> keys, TValue value)
        {
            return source
                .FilterKeys(keys)
                .FilterValue(value)
                .Select(pair => pair.Key)
                .ToArray();
        }

        public static TKey[] Filter<TKey, TValue>(
            this IDictionary<TKey, TValue[]> source,
            IEnumerable<TKey> keys, TValue[] value)
        {
            return source
                .FilterKeys(keys)
                .FilterValue(value)
                .Select(pair => pair.Key)
                .ToArray();
        }

        public static bool Update<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue newValue)
        {
            TValue oldValue;
            // not contains or not equals
            if (!source.TryGetValue(key, out oldValue) || !newValue.Equals(oldValue))
            {
                source[key] = newValue;
                return true;
            }
            return false;
        }
    }
}
