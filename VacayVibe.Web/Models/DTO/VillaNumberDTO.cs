using System.ComponentModel.DataAnnotations;

namespace VacayVibe.Web.Models.DTO;

public class VillaNumberDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
    public VillaDTO Villa { get; set; } //navigation property
}