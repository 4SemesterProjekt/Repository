using System;
using ApiFormat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Controllers.Utilities
{
    public static class ModelUtilities
    {
        public static T Save<T>(this T source, IRepository<T> repo) where T : class, IModel => repo.Update(source).Entity == null ? repo.Create(source) : source;

        public static T Add<T>(this T source, IRepository<T> repo) where T : class, IModel => repo.Create(source);

        public static void Delete<T>(this T source, IRepository<T> repo, double id) where T : class, IModel => repo.Delete(id);

        public static T Get<T>(this T source, IRepository<T> repo, double id) where T : class, IModel => repo.GetById(id);

        
    }
}