using System.ComponentModel.DataAnnotations;
namespace NotesApi.DTOs;

public record RegisterDto([Required, EmailAddress] string Email, [Required] string Password);
public record LoginDto([Required, EmailAddress] string Email, [Required] string Password);