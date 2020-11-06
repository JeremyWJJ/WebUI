using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Utils
{
    public class ConvertHelper
    {
        public static KeyValuePair<string, object>[] ConvertObjToKeyPair(object obj)
        {
            var type = obj.GetType();
            var list = new List<KeyValuePair<string, object>>();
            foreach (PropertyInfo item in type.GetProperties())
            {
                list.Add(new KeyValuePair<string, object>(item.Name, item.GetValue(obj, null)));
            }
            return list.ToArray();
        }

        public static object[] ConvertObjToKeyPairObject(object obj)
        {
            return ConvertObjToKeyPair(obj).Cast<object>().ToArray();
        }
    }
}