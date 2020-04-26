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
            /*using (var transaction = db.Database.BeginTransaction())
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
            }*/
            var entry = dbFunc(source);
            db.SaveChanges();
            return entry.Entity;
        }

        public static T GetFromDatabase<T>(SplitListContext db, Expression<Func<T, bool>> predicate)
            where T : class, IModel
        {
            return db.Set<T>().Where(predicate).FirstOrDefault();
        }
    }
}