using System;
using ApiFormat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Controllers.Utilities
{
    public static class DTOUtilities
    {
        public static T Save<T>(this T source, IGenericRepository<T> repo) where T : class => repo.Update(source).Entity == null ? repo.Create(source) : source;

        public static T Add<T>(this T source, IGenericRepository<T> repo) where T : class => repo.Create(source);

        public static void Delete<T>(this T source, IGenericRepository<T> repo, double id) where T : class => repo.Delete(id);

        public static T Get<T>(this T source, IGenericRepository<T> repo, double id) where T : class => repo.GetById(id);
    }
}