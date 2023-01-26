using SampleShop.Core.Models;
using System.Linq;

namespace SampleShop.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Commit();
        void Create(T t);
        void Delete(string id);
        T Get(string id);
        IQueryable<T> GetAll();
        void Update(T t);
    }
}