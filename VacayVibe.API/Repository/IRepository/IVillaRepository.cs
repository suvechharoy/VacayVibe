using System.Linq.Expressions;
using VacayVibe.API.Models;

namespace VacayVibe.API.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}