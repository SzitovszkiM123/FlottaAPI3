using FlottaAPI.DTOs;
using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FlottaContext _context;

        public UserController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var users = _context.Users.Select(u => new
                {
                    u.Id, u.Name, u.LoginName, u.Status, u.Active,
                    u.Address, u.Email, u.Phone, u.Image, u.Permission
                }).ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong happend " + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _context.Users.Where(u => u.Id == id).Select(u => new
                {
                    u.Id, u.Name, u.LoginName, u.Status, u.Active,
                    u.Address, u.Email, u.Phone, u.Image, u.Permission
                }).FirstOrDefault();
                if (user == null)
                {
                    return NotFound("Missing user");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong happend " + ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult Post(UserDTO userDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userDTO.Password))
                {
                    return BadRequest("Missing password");
                }
                if (_context.Users.Any(u => u.LoginName == userDTO.LoginName || u.Email == userDTO.Email))
                {
                    return BadRequest("LoginName or Email already exists");
                }
                User user = new User
                {
                    Name = userDTO.Name,
                    LoginName = userDTO.LoginName,
                    Status = userDTO.Status,
                    Active = userDTO.Active,
                    Address = userDTO.Address,
                    Email = userDTO.Email,
                    Hash = Argon2Sharp.Argon2PhcFormat.HashToPhcStringWithAutoSalt(userDTO.Password),
                    Phone = userDTO.Phone,
                    Image = userDTO.Image,
                    Permission = userDTO.Permission
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok("User created");
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong happend " + ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public IActionResult Put(int id, UserDTO userDTO)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound("Missing user");
                }
                if (_context.Users.Any(u => u.Id != id && (u.LoginName == userDTO.LoginName || u.Email == userDTO.Email)))
                {
                    return BadRequest("LoginName or Email already exists");
                }
                user.Name = userDTO.Name;
                user.LoginName = userDTO.LoginName;
                user.Status = userDTO.Status;
                user.Active = userDTO.Active;
                user.Address = userDTO.Address;
                user.Email = userDTO.Email;
                user.Phone = userDTO.Phone;
                user.Image = userDTO.Image;
                user.Permission = userDTO.Permission;
                if (!string.IsNullOrWhiteSpace(userDTO.Password))
                {
                    user.Hash = Argon2Sharp.Argon2PhcFormat.HashToPhcStringWithAutoSalt(userDTO.Password);
                }
                _context.SaveChanges();
                return Ok("User updated");
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong happend " + ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound("Missing user");
                }
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong happend " + ex.Message);
            }
        }
    }
}
