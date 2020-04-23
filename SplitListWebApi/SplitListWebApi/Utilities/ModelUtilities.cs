using System;
using System.Linq.Expressions;
using ApiFormat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Utilities
{
    public static class ModelUtilities
    {
        public static T Save<T>(this T source, IRepository<T> repo) where T : class, IModel => repo.Update(source) == null ? repo.Create(source) : source;

        public static T Add<T>(this T source, IRepository<T> repo) where T : class, IModel => repo.Create(source);

        public static void Delete<T>(this T source, IRepository<T> repo) where T : class, IModel => repo.Delete(source);

        public static T GetById<T>(this T source, IRepository<T> repo, Expression<Func<T, bool>> predicate) where T : class, IModel => repo.GetBy(predicate);
    }
}