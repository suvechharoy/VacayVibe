using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VacayVibe.API.Data;
using VacayVibe.API.Models;
using VacayVibe.API.Models.DTO;
using VacayVibe.API.Repository.IRepository;

namespace VacayVibe.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private string secretKey;
    
    public UserRepository(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        secretKey = config.GetValue<string>("APISettings:Secret");
    }

    public bool IsUniqueUser(string username)
    {
        var user = _context.LocalUsers.FirstOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return true;
        }
        return false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = _context.LocalUsers.FirstOrDefault(u=> u.UserName.ToLower()==loginRequestDTO.UserName.ToLower() 
                                                         && u.Password == loginRequestDTO.Password );
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Token = "",
                User = null
            };
        }
        
        //if user is found, generate JWT token 
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey); //access the secret key & encode in bytes. Convert the secret from string to byte array
        var tokenDescriptor = new SecurityTokenDescriptor //most important-contains everything like all the claims, expiration, signing credentials. Claims identify username, role etc.
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDTO responseDTO = new LoginResponseDTO()
        {
            User = user,
            Token = tokenHandler.WriteToken(token)
        };
        return responseDTO;
    }

    public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
    {
        LocalUser newUser = new LocalUser()
        {
            UserName = registerationRequestDTO.UserName,
            Name = registerationRequestDTO.Name,
            Password = registerationRequestDTO.Password,
            Role = registerationRequestDTO.Role
        };
        _context.LocalUsers.Add(newUser);
        await _context.SaveChangesAsync();
        newUser.Password = "";
        return newUser;
    }
}