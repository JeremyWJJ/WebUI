using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpdCommon.Utils.Extensions
{
    public static class Data
    {
        #region GerateToSQL

        private static Dictionary<string, object> GenerateColumns<T>(T src) where T : class, new()
        {
            var type = typeof(T);
            var colDic = new Dictionary<string, object>();
            var properties = type.GetProperties();
            foreach (var item in properties)
            {
                var attributes = item.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (attributes is null) continue;
                var value = item.GetValue(src, null);

                try
                {
                    if (!string.IsNullOrEmpty(attributes.TypeName))
                        colDic.Add(attributes.Name, Convert.ChangeType(value ?? default, Type.GetType(attributes.TypeName)));
                    else
                        colDic.Add(attributes.Name, Convert.ChangeType(value ?? default, item.PropertyType.GetType()));
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException($"{item.Name}在指定了类型后,转换类型失败,请确保所有数据均具备转换条件." + ex.Message);
                }
            }

            return colDic;
        }

        public static (string SQL, object[] Params) GerateToInsertSQL<T>(this T src) where T : class, new()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            string tableName = attr?.Name ?? type.Name;

            var colDic = GenerateColumns(src);

            var sql = new StringBuilder();
            sql.Append($" insert into {tableName}({string.Join(",", colDic.Select(x => x.Key))}) values ({string.Join(",", colDic.Select(x => $"@{x.Key}"))})");

            return (sql.ToString(), colDic.Select(x => x.Value).ToArray());
        }

        /// <summary>
        /// 通过属性名直接生成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="colNameUnderline">是否将属性中的驼峰转换为下划线</param>
        /// <returns></returns>
        public static (string SQL, object[] Params) GerateToInsertSQL<T>(this T src, bool colNameUnderline)
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            string tableName = attr?.Name ?? type.Name;

            var colDic = new Dictionary<string, object>();
            foreach (var item in type.GetProperties())
            {
                colDic.Add(colNameUnderline ? StringHelper.UnderscoreName(item.Name) : item.Name, item.GetValue(src));
            }

            var sql = new StringBuilder();
            sql.Append($" insert into {tableName}({string.Join(",", colDic.Select(x => x.Key))}) values ({string.Join(",", colDic.Select(x => $"@{x.Key}"))})");

            return (sql.ToString(), colDic.Select(x => x.Value).ToArray());
        }

        #endregion GerateToSQL
    }
}