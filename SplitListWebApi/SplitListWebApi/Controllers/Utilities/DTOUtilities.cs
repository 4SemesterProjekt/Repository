using ApiFormat;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Controllers.Utilities
{
    public static class DTOUtilities
    {
        public static IDTO Save(this IDTO source, IRepository repo) => repo.Update(source) != null ? source : repo.Add(source);

        public static IDTO Add(this IDTO source, IRepository repo) => repo.Add(source);

        public static void Delete(this IDTO source, IRepository repo) => repo.Delete(source);

        public static IDTO Get(this IDTO source, IRepository repo) => repo.Find(source.Id);
    }
}