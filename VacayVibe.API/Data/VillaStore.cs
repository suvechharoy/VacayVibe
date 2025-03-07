using VacayVibe.API.Models.DTO;

namespace VacayVibe.API.Data;

public static class VillaStore
{
    public static List<VillaDTO> villaList = new List<VillaDTO>()
    {
        new VillaDTO { Id = 1, Name = "Beach View", Occupancy = 2, Sqft = 100},
        new VillaDTO { Id = 2, Name = "Pool View", Occupancy = 3, Sqft = 300 },
        new VillaDTO { Id = 3, Name = "Park View", Occupancy = 5, Sqft = 600}
    };
}