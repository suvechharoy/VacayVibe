using System.ComponentModel.DataAnnotations;

namespace VacayVibe.API.Models.DTO;

public class VillaCreateDTO
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    public string Details { get; set; } 
    [Required]
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string ImageUrl { get; set; }
    public string Amenity { get; set; }
}