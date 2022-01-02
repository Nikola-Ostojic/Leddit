using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DAL.Repositories
{
    /// <summary>
    /// Basic CRUD functionalities to avoid duplication across repositories
    /// If modification is needed, override it in derived repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext DatabaseContext;
        protected readonly DbSet<T> _entities;

        protected BaseRepository(ApplicationDbContext context)
        {
            DatabaseContext = context;
            _entities = context.Set<T>();
        }

        public virtual Task<T> FindSingleByCondition(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentException("Provide the condition for the query");
            }


            return _entities.AsQueryable().FirstOrDefaultAsync(condition);
        }
        public virtual async Task<Pageable<T>> FindByConditionPageable(Expression<Func<T, bool>> filter, int page, int itemsPerPage, Func<IQueryable<T>, IQueryable<T>> inclusionFn = null, Func<IQueryable<T>, IQueryable<T>> sortingFn = null)
        {
            var query = _entities.AsQueryable();

            if (inclusionFn != null)
            {
                query = inclusionFn(query);
            }

            // if filter is provided add the filter to the query
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (sortingFn != null)
            {
                query = sortingFn(query);
            }
            

            var totalItems = query.Count();

            if (itemsPerPage != 0)
            {
                query = query
                       .Skip((page - 1) * itemsPerPage)
                       .Take(itemsPerPage);
            }

            

            return new Pageable<T>
            {
                Data = await query.ToListAsync(),
                TotalItems = totalItems,
                TotalPages = page == 0 ? 1 : CalculateTotalPages(totalItems, itemsPerPage),
                Page = page == 0 ? 1 : page
            };
        }

        public virtual Task<T> Get(int id) => _entities.FirstOrDefaultAsync(s => s.Id == id);
        public virtual Task<List<T>> GetAll() => _entities.ToListAsync();

        public virtual async Task<T> Create(T entity)
        {
            var result = await _entities.AddAsync(entity);
            await DatabaseContext.SaveChangesAsync();
            return result.Entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            if (!_entities.AsNoTracking().Any(e => e.Id == entity.Id))
                return null;

            _entities.Update(entity);
            await DatabaseContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(int id)
        {
            if (!_entities.AsNoTracking().Any(e => e.Id == id))
                return;

            _entities.Remove(_entities.Find(id));
            await DatabaseContext.SaveChangesAsync();
        }

        public virtual async Task DeleteByCondition(Expression<Func<T, bool>> filter)
        {
            var items = _entities.AsQueryable().Where(filter);

            _entities.RemoveRange(items);
            await DatabaseContext.SaveChangesAsync();
        }

        private int CalculateTotalPages(int totalItems, int itemsPerPage)
        {
            var totalPages = totalItems / itemsPerPage;

            if (totalItems % itemsPerPage != 0)
                totalPages++;

            return totalPages == 0 ? 1 : totalPages;
        }
    }
}
