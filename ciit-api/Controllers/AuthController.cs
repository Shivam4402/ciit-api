using ciit_api.DTOs.Auth;
using ciit_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ciit_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly CiitstudContext _context;

        public AuthController(IConfiguration config, CiitstudContext context)
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            Console.WriteLine($"Incoming: {dto.UserName}");

            var user = await _context.AspNetUsers
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null)
            {
                Console.WriteLine("User not found");
                return Unauthorized("Invalid credentials");
            }

            // Create password hasher
            var passwordHasher = new PasswordHasher<AspNetUser>();

            // Verify hashed password
            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                Console.WriteLine("Password mismatch");
                return Unauthorized("Invalid credentials");
            }

            Console.WriteLine("Login success");

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }


        [AllowAnonymous]
        [HttpPost("student-login")]
        public async Task<IActionResult> StudentLogin([FromBody] StudentLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("UserName and Password are required");

            var student = await _context.TblstudentDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.Flag == 0 &&
                    (s.PermanentIdentificationNumber == dto.UserName || s.EmailAddress == dto.UserName));

            if (student == null)
                return Unauthorized("Invalid credentials");

            if (!string.Equals(student.Password, dto.Password, StringComparison.Ordinal))
                return Unauthorized("Invalid credentials");

            var token = GenerateStudentJwtToken(student);

            return Ok(new
            {
                Token = token,
                Student = new
                {
                    student.StudentId,
                    student.StudentName,
                    student.LastName,
                    student.EmailAddress,
                    student.MobileNumber,
                    student.ProfilePhoto,
                    student.StudentCode,
                    student.BranchId
                }
            });
        }

        private string GenerateJwtToken(AspNetUser user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.UserName ?? "")
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateStudentJwtToken(TblstudentDetail student)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
                new Claim(ClaimTypes.Email, student.EmailAddress),
                new Claim("actor", "student"),
                new Claim(ClaimTypes.Name, $"{student.StudentName ?? ""} {student.LastName ?? ""}".Trim())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
