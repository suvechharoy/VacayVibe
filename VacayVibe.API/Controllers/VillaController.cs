using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacayVibe.API.Data;
using VacayVibe.API.Models;
using VacayVibe.API.Models.DTO;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    public VillaController(IVillaRepository villaRepository, IMapper mapper)
    {
        _villaRepository = villaRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        IEnumerable<Villa> villaList = await _villaRepository.GetAllAsync();
        return Ok(_mapper.Map<List<VillaDTO>>(villaList));
    }
    
    [HttpGet("{id}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = await _villaRepository.GetAsync(x => x.Id == id);
        
        if (villa == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<VillaDTO>(villa));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        if (await _villaRepository.GetAsync(x => x.Name.ToLower() == createDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("Custom Error","Villa already exists!");
            return BadRequest(ModelState);
        }
        if (createDTO == null)
        {
            return BadRequest(createDTO);
        }

        /*if (villaDTO.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }*/
        
        //converting villaDTO to villa
        Villa model = _mapper.Map<Villa>(createDTO);
        /*Villa model = new()
        {
            Amenity = createDTO.Amenity,
            Details = createDTO.Details,
            Name = createDTO.Name,
            ImageUrl = createDTO.ImageUrl,
            Occupancy = createDTO.Occupancy,
            Rate = createDTO.Rate,
            Sqft = createDTO.Sqft,
        };*/
        await _villaRepository.CreateAsync(model);
        
        return CreatedAtRoute("GetVilla", new {id=model.Id}, model);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }
        var villa = await _villaRepository.GetAsync(x => x.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        await _villaRepository.RemoveAsync(villa);
        return NoContent();
    }

    [HttpPut("{id}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
    {
        if (updateDTO == null || updateDTO.Id != id)
        {
            return BadRequest();
        }
        /*var villa = _context.Villas.FirstOrDefault(x => x.Id == id);
        villa.Name = villaDTO.Name;
        villa.Occupancy = villaDTO.Occupancy;
        villa.Sqft = villaDTO.Sqft;*/

        //converting villaDTO to villa
        Villa model = _mapper.Map<Villa>(updateDTO);
        /*Villa model = new()
        {
            Amenity = updateDTO.Amenity,
            Details = updateDTO.Details,
            Id = updateDTO.Id,
            Name = updateDTO.Name,
            ImageUrl = updateDTO.ImageUrl,
            Occupancy = updateDTO.Occupancy,
            Rate = updateDTO.Rate,
            Sqft = updateDTO.Sqft,
        };*/
        await _villaRepository.UpdateAsync(model);
        return NoContent();
    }

    [HttpPatch("{id}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }
        var villa = await _villaRepository.GetAsync(x => x.Id == id, tracked: false);
        
        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
        /*VillaUpdateDTO villaDTO = new()
        {
            Amenity = villa.Amenity,
            Details = villa.Details,
            Id = villa.Id,
            Name = villa.Name,
            ImageUrl = villa.ImageUrl,
            Occupancy = villa.Occupancy,
            Rate = villa.Rate,
            Sqft = villa.Sqft,
        };*/
        
        if (villa == null)
        {
            return BadRequest();
        }
        
        patchDTO.ApplyTo(villaDTO, ModelState);
        Villa model = _mapper.Map<Villa>(villaDTO);

        /*Villa model = new Villa()
        {
            Amenity = villa.Amenity,
            Details = villa.Details,
            Id = villa.Id,
            Name = villa.Name,
            ImageUrl = villa.ImageUrl,
            Occupancy = villa.Occupancy,
            Rate = villa.Rate,
            Sqft = villa.Sqft,
        };*/
        
        await _villaRepository.UpdateAsync(model);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}