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

namespace VacayVibe.API.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
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
    
    //simple demonstration of how versioning works. Say this is another Get action method with more complex functionalities i.e., version 2 of the endpoint.
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
}