using VacayVibe.API.Data;
using VacayVibe.API.Models;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Repository;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext _context;
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _context.VillaNumbers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}