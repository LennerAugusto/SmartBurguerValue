﻿using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using System;

namespace SmartBurguerValueAPI.Repository.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        protected AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            entity.DateCreated = entity.DateCreated == new DateTime() ? DateTime.Now : entity.DateCreated;
            entity.DateUpdated = DateTime.Now;
            _context.Set<T>().Add(entity);
        }
        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync(bool? isActive = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (isActive != null)
            {
                query = query.Where(x => EF.Property<bool>(x, "IsActive") == isActive);
            }

            return await query.ToListAsync();
        }


        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(T entity)
        {
            entity.DateUpdated = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
        }

        public async Task SaveAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
        }
    }
}
