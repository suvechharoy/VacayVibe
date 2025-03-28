using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacayVibe.Web.Models.DTO;

namespace VacayVibe.Web.Models.VM;

public class VillaNumberUpdateVM
{
    public VillaNumberUpdateVM()
    {
        VillaNumber = new VillaNumberUpdateDTO();
    }
    public VillaNumberUpdateDTO VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}