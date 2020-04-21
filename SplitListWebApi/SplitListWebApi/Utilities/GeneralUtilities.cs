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
        public static T WriteToDatabase<T>(this T source, Func<T, EntityEntry<T>> dbFunc, SplitListContext db) where T : class, IModel
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var entry = dbFunc(source);
                    db.SaveChanges();
                    transaction.Commit();
                    db.Entry(source).State = EntityState.Detached;
                    return entry.Entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public static T GetFromDatabase<T>(this int id, SplitListContext db)
            where T : class, IModel
        {
            var query = db.Set<T>().Where(i => i.ModelId == id).AsQueryable().AsNoTracking();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    query = db.Model
                        .FindEntityType(typeof(T))
                        .GetNavigations()
                        .Aggregate(query, (current, property) => current.Include(property.Name));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return query.FirstOrDefault();
        }
    }
}