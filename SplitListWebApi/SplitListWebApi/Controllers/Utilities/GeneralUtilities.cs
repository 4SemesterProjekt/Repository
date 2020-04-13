using System;
using System.Diagnostics.CodeAnalysis;
using ApiFormat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SplitListWebApi.Areas.Identity.Data;

namespace SplitListWebApi.Controllers.Utilities
{
    public static class GeneralUtilities
    {
        //Encapsulate Db-funcitions into transactions.
        public static void BeginTransaction<T>(this T source, Func<T, EntityEntry<T>> dbFunc, SplitListContext db) where T : class
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    dbFunc(source);
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