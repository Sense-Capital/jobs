using System.Security.Cryptography;

namespace TicTacToeAPI.Services.Interfaces
{
    public interface IRepository<T, TId>
    {
        IList<T> GetAll();
        T GetById(TId id);
        int Create(T item);
        int Update(TId id, T item);
        int Delete(TId id);
    }
}
