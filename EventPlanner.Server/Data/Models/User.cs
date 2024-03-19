using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Server.Data.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public List<Event>? Events { get; set; }
}