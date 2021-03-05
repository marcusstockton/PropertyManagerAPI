using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/{tenantId}")]
    [ApiController]
    public class NotesController : ControllerBase // NOT SURE I NEED THIS CONTROLLER AS NOTES WILL BE ON THE TENANT...
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, Note note)
        {
            if (ModelState.IsValid)
            {
                if (id != note.Id)
                {
                    return BadRequest();
                }

                _context.Entry(note).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(id))
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
            return BadRequest(ModelState);
        }

        // POST: api/Notes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return note;
        }

        private bool NoteExists(Guid id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}