using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacayVibe.API.Data;
using VacayVibe.API.Models;
using VacayVibe.API.Models.DTO;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class VillaNumberController : ControllerBase
{
    private readonly IVillaNumberRepository _villaNumberRepository;
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    protected APIResponse _response;
    public VillaNumberController(IVillaNumberRepository villaNumberRepository,
        IVillaRepository villaRepository, IMapper mapper)
    {
        _villaNumberRepository = villaNumberRepository;
        _villaRepository = villaRepository;
        _mapper = mapper;
        this._response = new APIResponse();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepository.GetAllAsync(includeProperties:"Villa");
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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
    
    [HttpGet("{id}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
    {
        try
        {
            if (await _villaNumberRepository.GetAsync(x => x.VillaNo == createDTO.VillaNo) != null)
            {
                ModelState.AddModelError("ErrorMessages","Villa Number already exists!");
                return BadRequest(ModelState);
            }
            if (await _villaRepository.GetAsync(u => u.Id == createDTO.VillaId) == null)
            {
                ModelState.AddModelError("ErrorMessages","Villa ID is invalid!");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //converting villaNumberDTO to villaNumber
            VillaNumber villa = _mapper.Map<VillaNumber>(createDTO);
            await _villaNumberRepository.CreateAsync(villa);
            _response.Result = _mapper.Map<VillaNumberDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVillaNumber", new {id=villa.VillaNo}, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response; 
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            await _villaNumberRepository.RemoveAsync(villaNumber);
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
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || updateDTO.VillaNo != id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (await _villaRepository.GetAsync(u => u.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("ErrorMessages","Villa ID is invalid!");
                return BadRequest(ModelState);
            }
            //converting villaNumberDTO to villaNumber
            VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);
            await _villaNumberRepository.UpdateAsync(model);
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

    [HttpPatch("{id}", Name = "UpdatePartialVillaNumber")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVillaNummber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }
        var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id, tracked: false);
        VillaNumberUpdateDTO villaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);
        if (villaNumber == null)
        {
            return BadRequest();
        }
        patchDTO.ApplyTo(villaNumberUpdateDTO, ModelState);
        VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
        await _villaNumberRepository.UpdateAsync(model);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}