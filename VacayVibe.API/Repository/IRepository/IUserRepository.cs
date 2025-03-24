using Microsoft.AspNetCore.Mvc;
using VacayVibe.API.Models;
using VacayVibe.API.Models.DTO;

namespace VacayVibe.API.Repository.IRepository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO); 
    Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
}