using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SplitListWebApi.Repositories.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(double id);
        TEntity Create(TEntity entity);

        EntityEntry<TEntity> Update(TEntity entity);

        void Delete(double id);
    }
}