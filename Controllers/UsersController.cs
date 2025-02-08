using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

    public UsersController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        Env.Load();
    }  
    
    //Get all Users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var allUsers = _context.Users.ToList();
        return Ok(allUsers);
    }
    
    //Get user by Id
    [Authorize]
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return Ok(user);
    }
    
    //Add User
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserDto user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var userEntity = new User()
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
        
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return Ok(userEntity);
    }
    
    //Log in
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginDto userDto)
    {
        var jwtKey = Env.GetString("JWT_SECRET");
        var user = _context.Users.FirstOrDefault(x => x.Email == userDto.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Incorrect email or password" });
        }
        
        bool isMatch = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);
        
        if (!isMatch) { return Unauthorized(new { message = "Incorrect password" });}

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtConfig:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email.ToString()),
            new Claim("Name", user.Name.ToString()),
        };
        
        // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["JwtConfig:Issuer"],
            _configuration["JwtConfig:Audience"],
            claims,
            signingCredentials: creds
        );
        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = tokenValue, user = new { userId = user.Id, user.Name, user.Email } });

    }
    
    //Delete User
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
        
    //Update user
    [HttpPatch]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto user)
    {
        var userEntity = await _context.Users.FindAsync(id);
        userEntity.Name = user.Name;
        userEntity.Email = user.Email;
        userEntity.Password = user.Password;
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
        return Ok(userEntity);
    }
}
}    

