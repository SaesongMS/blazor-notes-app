using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly NotesService _notesService;

    public NotesController(NotesService notesService)
    {
        _notesService = notesService;
    }

    [HttpGet]
    public async Task<List<Note>> Get() =>
        await _notesService.GetAsync();
    
    [HttpGet("{id:length(24)}", Name = "GetNote")]
    public async Task<ActionResult<Note>> Get(string id)
    {
        var note = await _notesService.GetAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        return note;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Note note)
    {
        await _notesService.CreateAsync(note);

        return CreatedAtRoute("GetNote", new { id = note.Id }, note);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Note noteIn)
    {
        var note = await _notesService.GetAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        await _notesService.UpdateAsync(id, noteIn);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var note = await _notesService.GetAsync(id);

        if (note == null)
        {
            return NotFound();
        }

        await _notesService.RemoveAsync(note.Id!);

        return NoContent();
    }
}