using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacayVibe.Web.Models.DTO;

namespace VacayVibe.Web.Models.VM;

public class VillaNumberDeleteVM
{
    public VillaNumberDeleteVM()
    {
        VillaNumber = new VillaNumberDTO();
    }
    public VillaNumberDTO VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}