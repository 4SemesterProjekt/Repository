using System;
using ApiFormat;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SplitListWebApi.Areas.Identity.Data;

namespace SplitListWebApi.Utilities
{
    public static class GeneralUtilities
    {
        //Encapsulate Db-funcitions into transactions.
        public static void BeginTransaction<T>(this T source, Func<T, EntityEntry<T>> dbFunc, SplitListContext db) where T : class, IModel
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
                    transaction.Rollback();
                }
            }
        }
    }
}