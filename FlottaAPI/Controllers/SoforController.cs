using FlottaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {
        //dependency injection tervezési minta
        private readonly FlottaContext _context;
        public SoforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_context.Sofors.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _context.Sofors.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                Sofor? talalat = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (talalat == null)
                {
                    return NotFound($"Nincs megfelelő azonosítójú sofőr. {id}");
                }
                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult PostSofor(Sofor uj)
        {
            try
            {
                uj.Id = 0;
                _context.Sofors.Add(uj);
                _context.SaveChanges();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpPost("PostAsync")]
        public async Task<IActionResult> PostAsync(Sofor uj)
        {
            try
            {
                uj.Id = 0;
                _context.Sofors.Add(uj);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpPut("Put/{id}")]
        public IActionResult Put(int id, Sofor modositott)
        {
            try
            {
                var toUpdate = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    return NotFound($"Rossz sofőr azonosító. {id}");
                }
                modositott.Id = id;
                _context.Entry(toUpdate).CurrentValues.SetValues(modositott);
                _context.SaveChanges();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpPut("PutAsync/{id}")]
        public async Task<IActionResult> PutAsync(int id, Sofor modositott)
        {
            try
            {
                var toUpdate = await _context.Sofors.FirstOrDefaultAsync(x => x.Id == id);
                if (toUpdate == null)
                {
                    return NotFound($"Rossz sofőr azonosító. {id}");
                }
                modositott.Id = id;
                _context.Entry(toUpdate).CurrentValues.SetValues(modositott);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteSofor(int id)
        {
            try
            {
                var toDelete = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (toDelete == null)
                {
                    return NotFound($"Rossz sofőr azonosító. {id}");
                }
                _context.Sofors.Remove(toDelete);
                _context.SaveChanges();
                return Ok("Sikeres törlés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong " + ex.Message);
            }
        }

        //adott sofőr milyen járművet vezetett?

        [HttpGet("SoforJarmu/{id}")]

        public IActionResult GetSoforJarmu(int id) {
            try
            {
                Sofor vane = _context.Sofors.FirstOrDefault(y => y.Id == id);
                if(vane == null)
                    return NotFound("Nincs ilyen sofőr");
                    var result = _context.Kikuldottjarmus.Where(j  => j.SoforNavigation.Id == id).Select(j => j.Gepjarmu).ToList();
                return Ok(result);
            }
            catch { 
            return BadRequest("Hiba!" + ex.Message);
            }
        }
    }
}
