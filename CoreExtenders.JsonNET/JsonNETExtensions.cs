using CoreExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CoreExtenders
{
    public static class JsonNETExt
    {
        public static string AsJsonDump(this object instance)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                };
                return JsonConvert.SerializeObject(instance, settings);
            }
            catch
            {
                return null;
            }
        }

        public static string AsXmlDump(this object instance, string root = "Root")
        {
            try
            {
                var jDump = instance.AsJsonDump();
                XNode doc = JsonConvert.DeserializeXNode(jDump, root);
                return doc.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static List<CompareObject> Compare(this object old, object @new, bool ignoreCase = false)
        {
            if (old == null || @new == null)
                throw new ArgumentNullException("At least, one of the parameters is null.");
            if (!old.IsSameType(@new))
                throw new Exception("Two types should be same.");

            var result = new List<CompareObject>();
            var isList = old.GetType().IsEnumerable();
            if (isList)
                throw new Exception("Your object should'n be enumerable.");
            var obj1 = JsonConvert.SerializeObject(old, Formatting.Indented);
            var obj2 = JsonConvert.SerializeObject(@new, Formatting.Indented);
            var theOld = JObject.Parse(obj1).GetObjectKeyValue();
            var theNew = JObject.Parse(obj2).GetObjectKeyValue();
            foreach (var o in theOld)
            {
                var k = o.Key;
                var v = o.Value;

                var status = ignoreCase ? theNew[k].ToLowerInvariant() == v.ToLowerInvariant() : theNew[k] == v;
                if (status)
                {
                    result.Add(new CompareObject()
                    {
                        Id = k,
                        Old = v,
                        New = theNew[k],
                        IsChanged = false
                    });
                }
                else
                {
                    result.Add(new CompareObject()
                    {
                        Id = k,
                        Old = v,
                        New = theNew[k],
                        IsChanged = true,
                    });
                }
            }

            return result;
        }

        public static List<JsonData> GetJsonData(this object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            var jObject = JObject.Parse(str);
            var jData = jObject.Descendants()
                        .Where(t => t.Type != JTokenType.Property && !t.HasValues)
                        .Select(t => new JsonData() { Key = t.Path, Value = t.ToString() }).ToList();
            return jData;
        }

        public static Dictionary<string, string> GetObjectKeyValue(this JObject jObject)
        {
            return jObject.Descendants()
                    .Where(t => t.Type != JTokenType.Property && !t.HasValues)
                    .ToDictionary(t => t.Path, t => t.ToString());
        }

        public static Dictionary<string, string> GetObjectKeyValue(this JArray jArray)
        {
            return jArray.Descendants()
                    .Where(t => t.Type != JTokenType.Property && !t.HasValues)
                    .ToDictionary(t => t.Path, t => t.ToString());
        }

        public static string JsonCompareData(this object old, object @new, bool ignoreCase = false)
        {
            return JsonConvert.SerializeObject(Compare(old, @new, ignoreCase), Formatting.Indented);
        }

        public static string JsonDiffData(this object old, object @new, bool ignoreCase = false)
        {
            var newList = Compare(old, @new, ignoreCase).Where(x => x.IsChanged);
            return JsonConvert.SerializeObject(newList, Formatting.Indented);

        }

        public static string JsonSameData(this object old, object @new, bool ignoreCase = false)
        {
            var newList = Compare(old, @new, ignoreCase).Where(x => !x.IsChanged);
            return JsonConvert.SerializeObject(newList, Formatting.Indented);
        }
    }
}
