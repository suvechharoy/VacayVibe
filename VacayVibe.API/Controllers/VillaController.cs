using System.Net;
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
    protected APIResponse _response;
    public VillaController(IVillaRepository villaRepository, IMapper mapper)
    {
        _villaRepository = villaRepository;
        _mapper = mapper;
        this._response = new APIResponse();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villaList = await _villaRepository.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpGet("{id}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villa = await _villaRepository.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response); 
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        try
        {
            if (await _villaRepository.GetAsync(x => x.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages","Villa already exists!");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //converting villaDTO to villa
            Villa villa = _mapper.Map<Villa>(createDTO);
            await _villaRepository.CreateAsync(villa);
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new {id=villa.Id}, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response; 
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villa = await _villaRepository.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            await _villaRepository.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response; 
    }

    [HttpPut("{id}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || updateDTO.Id != id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            //converting villaDTO to villa
            Villa model = _mapper.Map<Villa>(updateDTO);
            await _villaRepository.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
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
        if (villa == null)
        {
            return BadRequest();
        }
        patchDTO.ApplyTo(villaDTO, ModelState);
        Villa model = _mapper.Map<Villa>(villaDTO);
        await _villaRepository.UpdateAsync(model);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}