using System.Collections.Generic;

namespace Common.Utils
{
    public static class MySqlDBExtensions
    {
        public static (IList<T> Data, PageResult Page) GetDataWithPage<T>(this MySqlDB mySqlDB, int currPage, int pageSize, string pageBy, bool isAsc, string sql, params object[] args) where T : new()
        {
            var ret = mySqlDB.GetDataTable(currPage, pageSize, pageBy, isAsc, sql, args);
            if (!ret.IsSuccessed) return default;

            if (ret.Table == null || ret.Table.Rows.Count == 0) return default;

            try
            {
                return (ret.Table.SerializeToObject<T>(), new PageResult(ret.CurrentPage, ret.PageSize, ret.TotalRecords));
            }
            catch
            {
                return default;
            }
        }

        public static IList<T> GetData<T>(this MySqlDB mySqlDB, string sql, params object[] args) where T : new()
        {
            var ret = mySqlDB.GetDataTable(sql, args);
            if (!ret.IsSuccessed) return default;

            if (ret.Table == null || ret.Table.Rows.Count == 0) return default;

            try
            {
                return ret.Table.SerializeToObject<T>();
            }
            catch
            {
                return default;
            }
        }
    }
}