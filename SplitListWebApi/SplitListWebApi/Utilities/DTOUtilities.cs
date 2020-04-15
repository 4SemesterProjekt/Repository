using ApiFormat;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Utilities
{
    public static class DTOUtilities
    {
        public static T Save<T>(this T source, IRepository<T> repo) where T : class, IDTO => repo.Update(source) == null ? repo.Create(source) : source;

        public static T Add<T>(this T source, IRepository<T> repo) where T : class, IDTO => repo.Create(source);

        public static void Delete<T>(this T source, IRepository<T> repo) where T : class, IDTO => repo.Delete(source);

        public static T GetById<T>(this T source, IRepository<T> repo, int id) where T : class, IDTO => repo.GetById(id);
    }
}