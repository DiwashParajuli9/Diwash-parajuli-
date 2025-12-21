using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models;

public class JournalEntry
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    // Mood tracking
    [Required]
    public int PrimaryMoodId { get; set; }
    public Mood? PrimaryMood { get; set; }
    
    public int? SecondaryMood1Id { get; set; }
    public Mood? SecondaryMood1 { get; set; }
    
    public int? SecondaryMood2Id { get; set; }
    public Mood? SecondaryMood2 { get; set; }
    
    // Category (optional)
    public string Category { get; set; } = string.Empty;
    
    // Tags
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    // Calculated property
    public int WordCount => string.IsNullOrWhiteSpace(Content) ? 0 : Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
}
