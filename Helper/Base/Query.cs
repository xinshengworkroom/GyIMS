using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace GyIMS.Helper
{
    public class DbContextFactory
    {
        /// <summary>
        /// 创建EF上下文对象,已存在就直接取,不存在就创建,保证线程内是唯一
        /// </summary>
        public static DbContext Create()
        {
            DbContext dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)
            {
                dbContext = new DBContext();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }
    }

    public class Query<T>: IQuery<T> where T : class, new()
    {
        private DbContext dbContext = DbContextFactory.Create();

        public virtual int Add(T t)
        {
            dbContext.Set<T>().Add(t);
            //dbContext.Entry<T>(t).State = EntityState.Added;
            dbContext.Configuration.ValidateOnSaveEnabled = false;
            int rows= dbContext.SaveChanges();
            dbContext.Configuration.ValidateOnSaveEnabled = true;
            return rows;
        }
        public virtual int Delete(object primarykey)
        {
            var entity = dbContext.Set<T>().Find(primarykey);
            dbContext.Set<T>().Remove(entity);
            return dbContext.SaveChanges();
           
        }
        public virtual int Update(T t)
        {
            if (dbContext.Entry(t).State == EntityState.Detached)
            {
                HandleDetached(dbContext,t);
            }
            dbContext.Set<T>().Attach(t);
            dbContext.Entry<T>(t).State = EntityState.Modified;      
            return dbContext.SaveChanges();
        }


        public int ExecuteSql(string sql, params SqlParameter[] parameters)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return dbContext.Set<T>().Where(whereLambda);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="type">Models</typeparam>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="OrderByLambda">条件</param>
        /// <param name="WhereLambda">查询条件</param>
        /// <returns></returns>
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
        Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            //是否升序
            if (isAsc)
            {
                return dbContext.Set<T>().Where(WhereLambda).OrderBy(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return dbContext.Set<T>().Where(WhereLambda).OrderByDescending(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }


        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
     Expression<Func<T, type>> OrderByLambda)
        {
            //是否升序
            if (isAsc)
            {
                return dbContext.Set<T>().OrderBy(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return dbContext.Set<T>().OrderByDescending(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }


        private bool HandleDetached(DbContext dbContext,T entity)
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var entitySet = objectContext.CreateObjectSet<T>();
            var entityKey = objectContext.CreateEntityKey(entitySet.EntitySet.Name, entity);
            object foundSet;
            bool exists = objectContext.TryGetObjectByKey(entityKey, out foundSet);
            if (exists)
            {
                objectContext.Detach(foundSet); //从上下文中移除
            }
            return exists;


        }
    }
}
