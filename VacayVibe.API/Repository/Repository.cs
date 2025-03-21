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
        //_context.VillaNumbers.Include(u => u.Villa).ToList();
        this.dbSet = _context.Set<T>();
    }
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return await query.ToListAsync();
    }

    //"Villa, VillaSpecial"
    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
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

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
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