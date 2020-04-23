using System;
using System.Linq;
using System.Linq.Expressions;
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
                    //Forhold os til den specifikke case -tjek fx. om User eksisterer
                    var entry = dbFunc(source);
                    db.SaveChanges();
                    transaction.Commit();
                    return entry.Entity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public static T GetFromDatabase<T>(SplitListContext db, Expression<Func<T, bool>> predicate)
            where T : class, IModel
        {
            var query = db.Set<T>().Where(predicate).AsQueryable().AsNoTracking();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //query = db.Model
                    //    .FindEntityType(typeof(T))
                    //    .GetNavigations()
                    //    .Aggregate(query, (current, property) => current.Include(property.Name));

                    foreach (var property in db.Model.FindEntityType(typeof(T)).GetNavigations())
                    {
                        query = query.Include(property.Name);
                    }

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