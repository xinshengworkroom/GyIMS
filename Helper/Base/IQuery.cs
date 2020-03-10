using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GyIMS.Helper
{
    /// <summary>
    /// 定义一个通用查询接口接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial interface IQuery<T> where T : class, new()
    {
        void Dispose();
        int Add(T t);
        int Delete(object primarykey);
        int Update(T t);
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda);
        /// <summary>
        /// 一个业务中有可能涉及到对多张表的操作,那么可以将操作的数据,打上相应的标记,最后调用该方法,将数据一次性提交到数据库中,避免了多次链接数据库。
        /// </summary>
        //bool SaveChanges();

        IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda);

        int ExecuteSql(string sql, params SqlParameter[] parameters);
    }
}
