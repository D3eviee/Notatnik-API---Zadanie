using System.ComponentModel.DataAnnotations;
namespace NotesApi.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string PasswordHash { get; set; }
    public List<Note> Notes { get; set; } = new();
}