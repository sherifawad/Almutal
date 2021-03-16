using DataBase.Models;
using DataBase.Services.Repository;
using System;
using System.Threading.Tasks;

namespace Almutal.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<Guid, T> Repository<T>() where T : BaseModel;
    }
}