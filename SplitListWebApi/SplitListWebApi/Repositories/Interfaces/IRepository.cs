using ApiFormat;

namespace SplitListWebApi.Repositories.Interfaces
{
    public interface IRepository
    {
        IDTO Add(IDTO obj);
        IDTO Update(IDTO obj);
        IDTO Find(double Id);
        void Delete(IDTO obj);
    }
}