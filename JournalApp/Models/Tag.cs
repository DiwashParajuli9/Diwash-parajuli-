using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models;

public class Tag
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public bool IsCustom { get; set; }
    
    // Navigation property
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
