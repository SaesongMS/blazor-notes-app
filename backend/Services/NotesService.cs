using backend.Data;
using backend.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace backend.Services;

public class NotesService
{
    private readonly IMongoCollection<Note> _notes;

    public NotesService(
        IOptions<AppNoteDbSettings> settings
    )
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);

        _notes = database.GetCollection<Note>(settings.Value.NotesCollectionName);
    }

    public async Task<List<Note>> GetAsync() =>
        await _notes.Find(note => true).ToListAsync();

    public async Task<Note> GetAsync(string id) =>
        await _notes.Find<Note>(note => note.Id == id).FirstOrDefaultAsync();

    public async Task<Note> CreateAsync(Note note)
    {
        await _notes.InsertOneAsync(note);
        return note;
    }

    public async Task UpdateAsync(string id, Note noteIn) =>
        await _notes.ReplaceOneAsync(note => note.Id == id, noteIn);

    public async Task RemoveAsync(Note noteIn) =>
       await _notes.DeleteOneAsync(note => note.Id == noteIn.Id!);

    public async Task RemoveAsync(string id) =>
       await _notes.DeleteOneAsync(note => note.Id == id);
}