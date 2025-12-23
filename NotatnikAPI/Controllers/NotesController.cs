using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.DTOs;
using NotesApi.Models;

namespace NotesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notes")] 
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (idClaim == null)
            {
                idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return int.Parse(idClaim!);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteResponseDto>>> GetAll()
        {
            var userId = GetUserId();

            var notes = await _context.Notes
                .Where(n => n.UserId == userId)
                .Select(n => new NoteResponseDto(n.Id, n.Content))
                .ToListAsync();

            return Ok(notes);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseDto>> GetOne(int id)
        {
            var userId = GetUserId();

            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null) return NotFound();

            return Ok(new NoteResponseDto(note.Id, note.Content));
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> Create(CreateNoteDto dto)
        {
            var userId = GetUserId();

            var note = new Note
            {
                Content = dto.Content,
                UserId = userId
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = note.Id }, new NoteResponseDto(note.Id, note.Content));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateNoteDto dto)
        {
            var userId = GetUserId();

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null) return NotFound();
            if (note.UserId != userId) return StatusCode(403);

            note.Content = dto.Content;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null) return NotFound();
            if (note.UserId != userId) return StatusCode(403);

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}