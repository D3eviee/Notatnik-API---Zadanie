using System.ComponentModel.DataAnnotations;
namespace NotesApi.DTOs;

public record CreateNoteDto([Required] string Content);
public record UpdateNoteDto([Required] string Content);

public record NoteResponseDto(int Id, string Content);