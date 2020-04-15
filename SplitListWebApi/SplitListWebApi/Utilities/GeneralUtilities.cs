using System;
using System.Linq;
using ApiFormat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SplitListWebApi.Areas.Identity.Data;

namespace SplitListWebApi.Utilities
{
    public static class GeneralUtilities
    {
        //Encapsulate Db-funcitions into transactions.
        public static void WriteToDatabase<T>(this T source, Func<T, EntityEntry<T>> dbFunc, SplitListContext db) where T : class, IModel
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    dbFunc(source);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    source = null;
                    transaction.Rollback();
                }
            }
        }

        public static TSource GetFromDatabase<TSource, TEntity>(this double id, SplitListContext db, IMapper mapper)
            where TEntity : class, IModel
            where TSource : class, IDTO
        {
            var query = db.Set<TEntity>().Where(i => Math.Abs(i.Id - id) < 0.1).AsQueryable();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    query = db.Model
                        .FindEntityType(typeof(TEntity))
                        .GetNavigations()
                        .Aggregate(query, (current, property) => current.Include(property.Name));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            var dto = mapper.Map<TSource>(query.SingleOrDefault());
            return dto;
        }
    }
}