using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SplitListWebApi.Repositories.Interfaces
{
    public interface IRepository<TSource>
        where TSource : class
    {
        IQueryable<TSource> GetAll();
        TSource GetById(double id);
        TSource Create(TSource entity);
        TSource Update(TSource entity);
        void Delete(TSource entity);
    }
}