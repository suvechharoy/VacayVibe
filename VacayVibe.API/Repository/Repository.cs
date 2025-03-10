using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VacayVibe.API.Data;
using VacayVibe.API.Repository.IRepository;
using VacayVibe.API.Models;

namespace VacayVibe.API.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        this.dbSet = _context.Set<T>();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
    {
        IQueryable<T> query = dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await SaveAsync();
    }
    
    public async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}