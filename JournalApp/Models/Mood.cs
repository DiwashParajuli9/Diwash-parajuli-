using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models;

public class Mood
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public MoodCategory Category { get; set; }
    
    public string Emoji { get; set; } = string.Empty;
}
