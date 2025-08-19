using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoilerMonitoringAPI.Data;
using BoilerMonitoringAPI.Models;
using BoilerMonitoringAPI.DTOs;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerMonitoringAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomesController : Controller
    {
        private readonly BoilerMonitoringAPIContext _context;

        public HomesController(BoilerMonitoringAPIContext context)
        {
            _context = context;
        }
        string isnull = "Entity set 'DatabaseContext.Homes'  is null.";



        [HttpPost("AddHome")]
        public async Task<ActionResult<HomesDTOID>> AddBoiler(HomesDTO HomeDTO)
        {
            if (_context.Boilers == null)
            {
                return Problem(isnull);
            }
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

        [HttpPost("AddBoiler")]
        public async Task<ActionResult<BoilerDTOID>> AddBoiler(BoilerDTO BoilerDTO)
        {
            if (_context.Boilers == null)
            {
                return Problem(isnull);
            }
            try
            {
                Boilers boiler = BoilerDTO.Adapt<Boilers>();
                Home home = await _context.Homes.FindAsync(BoilerDTO.HomeID);
                _context.Boilers.Add(boiler);
                home.Boilers.Add(boiler);
                await _context.SaveChangesAsync();
                BoilerDTOID boilerID = BoilerDTO.Adapt<BoilerDTOID>();
                return CreatedAtAction("GetBoiler", new { id = boiler.BoilerID }, boilerID);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetBoiler")]
        public async Task<ActionResult<BoilerDTOID>> GetBoiler(Guid BoilerID)
        {
            if (_context.Homes == null)
            {
                return Problem(isnull);
            }
            Boilers boiler = await _context.Boilers.FindAsync(BoilerID);
            BoilerDTOID boilerDTOID = boiler.Adapt<BoilerDTOID>();
            return boilerDTOID;
        }

        private bool HomesExists(Guid id)
        {
            return _context.Homes.Any(e => e.HomeID == id);
        }
    }
}
