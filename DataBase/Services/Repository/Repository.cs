using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Services.Repository
{
    public class Repository<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly IDatabaseContext _context;
        private readonly DbSet<T> dbSet;

        protected IDatabaseContext Context => _context;

        public Repository(IDatabaseContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task<bool> AddItemAsync(T item, bool save = false) 
        {
            await dbSet.AddAsync(item);
            if (save)
                await _context.SaveChangesAsync();

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(T item, bool save = false) 
        {
            if (item == null) throw new ArgumentNullException("entity");

            if (Context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);

            if (save)
                await _context.SaveChangesAsync();

            return await Task.FromResult(true);

        }


        public async Task<T> FindAsync(TKey id) 
        {
            return await dbSet.FindAsync(id);

        }

        public async Task<List<T>> GetAllAsync(bool forceRefresh = false) 
        {
            return await dbSet.ToListAsync();
        }

        public async Task<bool> UpdateItemAsync(T item, bool save = false) 
        {

            dbSet.Update(item);

            if (save)
                await _context.SaveChangesAsync();

            return await Task.FromResult(true);
        }


        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = "", bool track = true)
        {
            IQueryable<T> query = null;
            if (track)
                query = dbSet;
            else
                query = dbSet.AsNoTracking();

            if (!string.IsNullOrEmpty(includeProperties) && !string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }



            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> filter = null, string includeProperties = "",
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool track = true) 
        {
            IQueryable<T> query = null;
            if (track)
                query = dbSet;
            else
                query = dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties) && !string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public Task<TValue> GetMax<TValue>(Expression<Func<T, TValue>> filter = null)
        {

            var GetMax = dbSet.Max(filter);
            return Task.FromResult(GetMax);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter = null, string includeProperties = "",
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties) && !string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            return query;

        }

        private IQueryable<T> EvaluateInclude(IQueryable<T> current, Expression<Func<T, object>> item) 
        {
            if (item.Body is MethodCallExpression)
            {
                var arguments = ((MethodCallExpression)item.Body).Arguments;
                if (arguments.Count > 1)
                {
                    var navigationPath = string.Empty;
                    for (var i = 0; i < arguments.Count; i++)
                    {
                        var arg = arguments[i];
                        var path = arg.ToString().Substring(arg.ToString().IndexOf('.') + 1);

                        navigationPath += (i > 0 ? "." : string.Empty) + path;
                    }
                    return current.Include(navigationPath);
                }
            }

            return current.Include(item);
        }

        public bool IsTracked(T item) 
        {
            return Context.Entry(item).State == EntityState.Added
                                              || Context.Entry(item).State == EntityState.Modified
                                              || Context.Entry(item).State == EntityState.Deleted;
        }

        public async Task<bool> InsertOrUpdate(TKey id, T item, bool save = false) 
        {
            var itemInDB = await dbSet.FindAsync(id);
            if (itemInDB == null)
            {
                await dbSet.AddAsync(item);
            }
            else
            {
                Context.Entry(itemInDB).CurrentValues.SetValues(item);
            }

            if (save)
                await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
    }
}
