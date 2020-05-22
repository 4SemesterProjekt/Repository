using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiFormat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SplitListWebApi.Areas.Identity.Data;

namespace SplitListWebApi.Utilities
{
    public static class GeneralUtilities
    {
        /// <summary>
        /// Modifies database with function dbFunc, and wraps it in a transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dbFunc"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        //Encapsulate Db-funcitions into transactions.
        public static T WriteToDatabase<T>(this T source, Func<T, EntityEntry<T>> dbFunc, SplitListContext db) where T : class, IModel
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var entry = dbFunc(source);
                    db.SaveChanges();
                    transaction.Commit();
                    return entry.Entity;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        /// <summary>
        /// Retrieves TResult (bound to selector) from database using a predicate. Can be modified to order the output, and include navigational properties.
        /// This function was taken from stackoverflow: https://stackoverflow.com/questions/46374252/how-to-write-repository-method-for-theninclude-in-ef-core-2
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="db"></param>
        /// <param name="selector"></param>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking"></param>
        /// <returns>List of TResult </returns>
        public static List<TResult> GetFromDatabase<TResult, TEntity>(
            SplitListContext db,
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        where TEntity : class
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).ToList();
            }

            return query.Select(selector).ToList();
        }
    }
}