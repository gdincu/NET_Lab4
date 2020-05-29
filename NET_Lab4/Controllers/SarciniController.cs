﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using TaskManager.Domain.Models;

namespace Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SarciniController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SarciniController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sarcini
        /// <summary>
        /// Returns a set of items.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sarcina>>> GetSarcini(DateTimeOffset? from = null, DateTimeOffset? to = null)
        {
            //Filters results by deadline
            IQueryable<Sarcina> result = _context.Sarcini;
            if (from != null && to != null)
                result = result.Where(f => from <= f.Deadline && f.Deadline <= to);
            if (from != null)
                result = result.Where(f => from <= f.Deadline);
            else if (to != null)
                result = result.Where(f => to <= f.Deadline);

            return await result.Include(e => e.Comments).ToListAsync();

        }

        // GET: api/Sarcini/5
        /// <summary>
        /// Returns a specific item.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>  
        [HttpGet("{id}")]
        public async Task<ActionResult<Sarcina>> GetSarcina(int id)
        {
            var sarcina = _context.Sarcini;

            if (sarcina == null)
            {
                return NotFound();
            }

            return await sarcina.Include(e => e.Comments).SingleOrDefaultAsync(e => e.Id == id);
        }

        // PUT: api/Sarcini/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Updates a specific item.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSarcina(int id, Sarcina sarcina)
        {
            if (id != sarcina.Id)
            {
                return BadRequest();
            }

            //Ex.1 - Task 3
            if (sarcina.Stare.Equals(Stare.Closed))
            {
                sarcina.ClosedAt = DateTime.Now;
            }
            else
            {
                sarcina.ClosedAt = default;
            }

            _context.Entry(sarcina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SarcinaExists(id))
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

        // POST: api/Sarcini
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Adds an item.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Sarcina>> PostSarcina(Sarcina sarcina)
        {
            _context.Sarcini.Add(sarcina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSarcina", new { id = sarcina.Id }, sarcina);
        }

        // DELETE: api/Sarcini/5
        /// <summary>
        /// Deletes a specific item.
        /// </summary>
        [HttpDelete("{id}")]
        
        public async Task<ActionResult<Sarcina>> DeleteSarcina(int id)
        {
            var sarcina = await _context.Sarcini.FindAsync(id);
            if (sarcina == null)
            {
                return NotFound();
            }

            _context.Sarcini.Remove(sarcina);
            await _context.SaveChangesAsync();

            return sarcina;
        }

        private bool SarcinaExists(int id)
        {
            return _context.Sarcini.Any(e => e.Id == id);
        }
    }
}
