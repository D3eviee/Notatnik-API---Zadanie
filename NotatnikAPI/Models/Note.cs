using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotesApi.Models;

public class Note
{
    public int Id { get; set; }

    [Required]
    public required string Content { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;
}