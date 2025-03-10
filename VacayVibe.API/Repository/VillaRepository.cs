using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VacayVibe.API.Data;
using VacayVibe.API.Models;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Repository;

public class VillaRepository : Repository<Villa>, IVillaRepository
{
    private readonly ApplicationDbContext _context;

    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Villas.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}