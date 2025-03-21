using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacayVibe.Web.Models.DTO;

namespace VacayVibe.Web.Models.VM;

public class VillaNumberCreateVM
{
    public VillaNumberCreateVM()
    {
        VillaNumber = new VillaNumberCreateDTO();
    }
    public VillaNumberCreateDTO VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}