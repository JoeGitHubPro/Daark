using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Daark.Data;
using Daark.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Daark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DaarkRealEstatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DaarkRealEstatesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/allUsers")]
        public async Task<IActionResult> AllUsers()
        {
            var result = await _context.Users.Select(a=> new {a.UserId ,a.FirstName ,a.LastName,a.PhoneNumber}).ToListAsync();

            return Ok(result);
        }

        // GET: api/DaarkRealEstates
        [HttpGet]
        public async Task<IActionResult> GetDaarkRealEstates()
        {
            if (_context.DaarkRealEstates == null)
                return NotFound();


            var result = await _context.DaarkRealEstates.Include(a => a.Portals).Select(a =>
            new
            {
                a.Id,
                a.Date,
                a.User.FirstName,
                a.User.LastName,
                a.User.UserName,

                Poartals = new
                {
                    Bayut = new { a.Portals.Bayut.Dubai, a.Portals.Bayut.Other },
                    PropertyFinder = new { a.Portals.PropertyFinder.Dubai, a.Portals.PropertyFinder.Rak },
                    a.Portals.Semsar
                },

                a.Calls,
                a.FollowUp,

                Leads = new { a.Leads.Bayut, a.Leads.PropertyFinder, a.Leads.Semsar },

                a.Meeting,
                a.Deal,
                a.LeedsInSheet,
                a.ThingsIDidToday
                

            }).ToListAsync();
            return Ok(result);
        }
        // GET: api/DaarkRealEstates
        [HttpGet("GetReportUserTeam")]
        public async Task<IActionResult> GetDaarkRealEstatesByTeam(string team)
        {
            if (_context.DaarkRealEstates == null)
                return NotFound();
            var Team = await _context.Teams.SingleOrDefaultAsync(a => a.Name == team);
            List<string> usersTeams = await _context.UserTeams.Where(a => a.TeamId == Team.Id).Select(a => a.UserId).ToListAsync();

            var result = await _context.DaarkRealEstates.Include(a => a.Portals).Where(e => usersTeams.Contains(e.UserId)).Select(a =>
            new
            {
                a.Id,
                a.Date,
                a.User.FirstName,
                a.User.LastName,
                a.User.UserName,

                Poartals = new
                {
                    Bayut = new { a.Portals.Bayut.Dubai, a.Portals.Bayut.Other },
                    PropertyFinder = new { a.Portals.PropertyFinder.Dubai, a.Portals.PropertyFinder.Rak },
                    a.Portals.Semsar
                },

                a.Calls,
                a.FollowUp,

                Leads = new { a.Leads.Bayut, a.Leads.PropertyFinder, a.Leads.Semsar },

                a.Meeting,
                a.Deal,
                a.LeedsInSheet,
                a.ThingsIDidToday

            }).ToListAsync();
            return Ok(result);
        }

        // GET: api/DaarkRealEstates
        [HttpGet("GetReportUser")]

        public async Task<IActionResult> GetDaarkRealEstate()
        {
            if (_context.DaarkRealEstates == null)
            {
                return NotFound();
            }
            var result = await _context.DaarkRealEstates.Where(a => a.UserId == GetUserIdFromToken()).Select(a =>
            new
            {
                a.Id,
                a.Date,
                a.User.FirstName,
                a.User.LastName,
                a.User.UserName,

                Poartals = new
                {
                    Bayut = new { a.Portals.Bayut.Dubai, a.Portals.Bayut.Other },
                    PropertyFinder = new { a.Portals.PropertyFinder.Dubai, a.Portals.PropertyFinder.Rak },
                    a.Portals.Semsar
                },

                a.Calls,
                a.FollowUp,

                Leads = new { a.Leads.Bayut, a.Leads.PropertyFinder, a.Leads.Semsar },

                a.Meeting,
                a.Deal,
                a.LeedsInSheet,
                a.ThingsIDidToday

            }).ToListAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        // GET: api/DaarkRealEstates
        [HttpGet("GetReportsByDate")]

        public async Task<IActionResult> GetDaarkRealEstateDate(DateTime date)
        {
            if (_context.DaarkRealEstates == null)
            {
                return NotFound();
            }
            var result = await _context.DaarkRealEstates.Where(a => a.Date == date).Select(a =>
            new
            {
                a.Id,
                a.Date,
                a.User.FirstName,
                a.User.LastName,
                a.User.UserName,

                Poartals = new
                {
                    Bayut = new { a.Portals.Bayut.Dubai, a.Portals.Bayut.Other },
                    PropertyFinder = new { a.Portals.PropertyFinder.Dubai, a.Portals.PropertyFinder.Rak },
                    a.Portals.Semsar
                },

                a.Calls,
                a.FollowUp,

                Leads = new { a.Leads.Bayut, a.Leads.PropertyFinder, a.Leads.Semsar },

                a.Meeting,
                a.Deal,
                a.LeedsInSheet,
                a.ThingsIDidToday

            }).ToListAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/DaarkRealEstates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDaarkRealEstate(int id, DaarkRealEstate daarkRealEstate)
        {
            if (id != daarkRealEstate.Id)
            {
                return BadRequest();
            }

            _context.Entry(daarkRealEstate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DaarkRealEstateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DaarkRealEstates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostDaarkRealEstate(ReportDto? reportDto)
        {
            if (_context.DaarkRealEstates == null)
            {
                return Problem("Entity set 'AppDbContext.DaarkRealEstates'  is null.");
            }


            Portal portal = new Portal()
            {
                Bayut = new Bayut() { Dubai = reportDto.PortalBayutDubai, Other = reportDto.PortalBayutOther },
                PropertyFinder = new PropertyFinder() { Dubai = reportDto.PortalPropertyFinderDubai, Rak = reportDto.PortalPropertyFinderRak },
                Semsar = reportDto.PortalSemsar
            };
            _context.Portals.Add(portal);
            await _context.SaveChangesAsync();

            Lead lead = new Lead()
            {
                Bayut = reportDto.LeadBayut,
                PropertyFinder = reportDto.LeadPropertyFinder,
                Semsar = reportDto.LeadSemsar
            };
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            DaarkRealEstate daarkRealEstate = new DaarkRealEstate()
            {

                UserId = GetUserIdFromToken(),
                LeadsId = lead.Id,
                PortalsId = portal.Id,
                Date = DateTime.UtcNow,
                Calls = reportDto.Calls,
                Deal = reportDto.Deal,
                FollowUp = reportDto.FollowUp,
                Meeting = reportDto.Meeting,
                LeedsInSheet = reportDto.LeedsInSheet,
                ThingsIDidToday = reportDto.ThingsIDidToday

            };

            _context.DaarkRealEstates.Add(daarkRealEstate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DaarkRealEstateExists(daarkRealEstate.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // DELETE: api/DaarkRealEstates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDaarkRealEstate(int id)
        {
            if (_context.DaarkRealEstates == null)
            {
                return NotFound();
            }
            var daarkRealEstate = await _context.DaarkRealEstates.FindAsync(id);
            if (daarkRealEstate == null)
            {
                return NotFound();
            }

            _context.DaarkRealEstates.Remove(daarkRealEstate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private string? GetUserIdFromToken()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = _context.Users.Where(a => a.UserName == userName).Select(a => a.Id).SingleOrDefault();
            return userId;
        }
        private bool DaarkRealEstateExists(int id)
        {
            return (_context.DaarkRealEstates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
