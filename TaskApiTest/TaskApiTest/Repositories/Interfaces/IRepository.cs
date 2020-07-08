using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskApiTest.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(Guid id, CancellationToken token);
        Task Add(T newItem, CancellationToken token);
        Task Update(T updatedItem, CancellationToken token);
    }
}