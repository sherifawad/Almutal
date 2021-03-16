using DataBase.Models;
using DataBase.Services;
using DataBase.Services.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using AppContext = DataBase.Services.AppContext;

namespace Almutal.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseContext _databaseContext;
        private Hashtable _repositories;
        public UnitOfWork(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        private void Dispose(bool dispose)
        {
            if (dispose)
                _databaseContext.Dispose();
        }

        public Task CommitAsync()
        {
            return _databaseContext.SaveChangesAsync();
        }

        public IRepository<Guid, T> Repository<T>() where T : BaseModel
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(new Type[] { typeof(Guid), typeof(T) }), _databaseContext);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<Guid, T>)_repositories[type];

        }
    }
}
