using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SpdCommon
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class LocalMethod
    {
        #region util

        /// <summary>
        /// 深克隆一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T src)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                T ret = default(T);
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, src);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本)
                ret = (T)bf.Deserialize(ms);
                return ret;
            }
        }

        /// <summary>
        /// 将datatable 序列化成为一个动态对象，不会有智能提示，所有属性均和datatable一致
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<dynamic> SerializeToDynamicObject(this System.Data.DataTable dataTable)
        {
            if (dataTable == null)
                return null;

            // 如果数据表字段有下滑线会去掉
            var columns = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c =>
                c.ColumnName
            );

            if (dataTable == null || dataTable.Rows.Count == 0)
                return null;

            var ret = new List<dynamic>();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                dynamic obj = new System.Dynamic.ExpandoObject();
                var dic = obj as IDictionary<string, Object>;
                foreach (var colName in columns)
                {
                    dic.Add(colName, row[colName]);
                }
                ret.Add(obj);
            }

            return ret;
        }

        /// <summary>
        /// 将DataTable序列化成对象，如果DataTable中和对象中存在相同的字段名，就会赋值，否则不会赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="ignoreTableUnderline">是否忽略数据表中的下划线</param>
        /// <returns></returns>
        public static IList<T> SerializeToObject<T>(this System.Data.DataTable dataTable, bool ignoreTableUnderline = true) where T : new()
        {
            if (dataTable == null)
                return null;

            var type = typeof(T);
            var props = type.GetProperties();
            // 如果数据表字段有下滑线会去掉
            var columns = dataTable.Columns.Cast<System.Data.DataColumn>().Select(c =>
                new
                {
                    ProptyName = ignoreTableUnderline ?
                        c.ColumnName.Replace("_", "").ToLower() :
                        c.ColumnName.ToLower(),
                    c.ColumnName
                }
            );
            if (columns.Count() == 0)
                return new List<T>();

            if (dataTable == null || dataTable.Rows.Count == 0)
                return new List<T>();

            var getDefaultValue = new Func<Type, object>(propType =>
            {
                // 如果是可空类型，直接返回null
                if (Nullable.GetUnderlyingType(propType) != null)
                    return null;
                // 如果时非可空类型 直接获取默认值
                else
                {
                    // 如果不是值类型，直接返回null
                    return propType.IsValueType ? Activator.CreateInstance(propType) : null;
                }
            });

            var convertTypeToProperty = new Func<Type, object, object>((prop, value) =>
            {
                try
                {
                    // 当datatable中字段的值为空时，需要根据实体类的属性类型来初始化默认值
                    if (value == null || value == DBNull.Value)
                    {
                        return getDefaultValue(prop);
                    }
                    // 当datatable中字段的值不为空时，将其转换为属性类型，如果失败，则抛出异常。
                    else
                    {
                        var t = Nullable.GetUnderlyingType(prop) ?? prop;
                        return Convert.ChangeType(value, t);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            var ret = new List<T>();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    var c = columns.Where(x => x.ProptyName.ToLower() == prop.Name.ToLower()).FirstOrDefault();
                    // 如果对象属性在DataTable中存在相应的列，就赋值
                    if (c != null)
                    {
                        try
                        {
                            // 类型相同直接赋值
                            if (row[c.ColumnName].GetType().FullName == prop.PropertyType.FullName)
                                prop.SetValue(obj, row[c.ColumnName], null);
                            // 类型不同，将table的类型转换为属性的类型，转换失败时赋值为当前类型的默认值
                            else
                                prop.SetValue(obj, convertTypeToProperty(prop.PropertyType, row[c.ColumnName]), null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                ret.Add(obj);
            }

            return ret;
        }

        /// <summary>
        /// 动态给对象中的属性赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">对象实例</param>
        /// <param name="arrtName">属性名称</param>
        /// <param name="value">属性值，会根据属性值的属性去执行转换</param>
        /// <returns></returns>
        public static T DynamicAssignmentAttr<T>(T instance, string arrtName, object value)
        {
            var props = instance.GetType().GetProperties();
            int pos = props.ToList().FindIndex(it => it.Name == arrtName);
            switch (props[pos].PropertyType.FullName)
            {
                // double类型
                case "System.Double":
                    props[pos].SetValue(instance, Convert.ToDouble(value), null);
                    break;
                // int类型
                case "System.Int32":
                    props[pos].SetValue(instance, Convert.ToInt32(value), null);
                    break;
                // 字符串
                default:
                    props[pos].SetValue(instance, value.ToString(), null);
                    break;
            }
            return instance;
        }

        /// <summary>
        /// 将大驼峰格式调整为下划线
        /// </summary>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static string ChangeAttrName(string attrName)
        {
            if (attrName.Length <= 1) return attrName;
            StringBuilder sb = new StringBuilder();
            sb.Append(attrName.ToLower()[0]);
            for (int i = 1; i < attrName.Length; i++)
            {
                if ('A' <= attrName[i] && attrName[i] <= 'Z')
                {
                    sb.Append($"_{attrName[i]}".ToLower());
                }
                else
                {
                    sb.Append(attrName[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Map实现 通过指定函数会返回一个新的集合
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> src, Func<A, B> func)
        {
            var bs = new List<B>();
            foreach (A item in src)
            {
                bs.Add(func(item));
            }
            return bs.AsEnumerable();
        }

        /// <summary>
        /// Map实现 通过指定函数会返回一个新的集合
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> src, Func<A, int, B> func)
        {
            var bs = new List<B>();
            var index = 0;
            foreach (A item in src)
            {
                bs.Add(func(item, index++));
            }
            return bs.AsEnumerable();
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator">结果初始值</param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, B> func, B accumulator)
        {
            var b = accumulator;
            foreach (A item in src)
            {
                b = func(b, item);
            }
            return b;
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator"></param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, int, B> func, B accumulator)
        {
            var b = accumulator;
            var index = 0;
            foreach (A item in src)
            {
                b = func(b, item, index++);
            }
            return b;
        }

        /// <summary>
        /// Reduce实现
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="accumulator">结果初始值</param>
        /// <returns></returns>
        public static B Reduce<A, B>(this IEnumerable<A> src, Func<B, A, int, IEnumerable<A>, B> func, B accumulator)
        {
            var b = accumulator;
            var index = 0;
            foreach (A item in src)
            {
                b = func(b, item, index++, src);
            }
            return b;
        }

        #endregion util
    }
}