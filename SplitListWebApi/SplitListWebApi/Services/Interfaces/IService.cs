using System.Linq;
using ApiFormat;

namespace SplitListWebApi.Services.Interfaces
{
    public interface IService<TSource, T>
        where TSource : class, IDTO
    {
        TSource Create(TSource dto);
        TSource Update(TSource dto);
        void Delete(TSource dto);
    }
}