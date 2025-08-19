using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WindowMonitorAPI.Models;
using WindowMonitorAPI.DTOs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WindowMonitorAPI.Data;

namespace WindowMonitorAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomesController : Controller
    {
        private readonly DatabaseContext _context;

        public HomesController(DatabaseContext context)
        {
            _context = context;
        }
        string isnull = "Entity set 'DatabaseContext.Homes'  is null.";



        [HttpPost("AddHome")]
        public async Task<ActionResult<HomesDTOID>> AddBoiler(HomesDTO HomeDTO)
        {
            Home home = HomeDTO.Adapt<Home>();

            home.Users.Add(await _context.Users.FindAsync(HomeDTO.UserID));
            _context.Homes.Add(home);
            await _context.SaveChangesAsync();
            HomesDTOID homeID = HomeDTO.Adapt<HomesDTOID>();
            return CreatedAtAction("GetHome", new { id = home.HomeID }, homeID);
        }

        [HttpGet("GetHome")]
        public async Task<ActionResult<HomesDTOID>> GetHome(Guid HomeID)
        {
            if (_context.Homes == null)
            {
                return Problem(isnull);
            }
            Home homes = await _context.Homes.FindAsync(HomeID);
            HomesDTOID HomeDTO = homes.Adapt<HomesDTOID>();
            return HomeDTO;
        }

        private bool HomesExists(Guid id)
        {
            return _context.Homes.Any(e => e.HomeID == id);
        }
    }
}
