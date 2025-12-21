using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace JournalApp.Models;

public class JournalDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Mood> Moods { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public string DbPath { get; }
    
    public JournalDbContext()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Combine(folder, "journal.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
        modelBuilder.Entity<JournalEntry>()
            .HasOne(e => e.PrimaryMood)
            .WithMany()
            .HasForeignKey(e => e.PrimaryMoodId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<JournalEntry>()
            .HasOne(e => e.SecondaryMood1)
            .WithMany()
            .HasForeignKey(e => e.SecondaryMood1Id)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<JournalEntry>()
            .HasOne(e => e.SecondaryMood2)
            .WithMany()
            .HasForeignKey(e => e.SecondaryMood2Id)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Seed moods data
        SeedMoods(modelBuilder);
        
        // Seed predefined tags
        SeedTags(modelBuilder);
    }
    
    private void SeedMoods(ModelBuilder modelBuilder)
    {
        var moods = new List<Mood>
        {
            // Positive moods
            new Mood { Id = 1, Name = "Happy", Category = MoodCategory.Positive, Emoji = "😊" },
            new Mood { Id = 2, Name = "Excited", Category = MoodCategory.Positive, Emoji = "🤗" },
            new Mood { Id = 3, Name = "Relaxed", Category = MoodCategory.Positive, Emoji = "😌" },
            new Mood { Id = 4, Name = "Grateful", Category = MoodCategory.Positive, Emoji = "🙏" },
            new Mood { Id = 5, Name = "Confident", Category = MoodCategory.Positive, Emoji = "💪" },
            
            // Neutral moods
            new Mood { Id = 6, Name = "Calm", Category = MoodCategory.Neutral, Emoji = "😐" },
            new Mood { Id = 7, Name = "Thoughtful", Category = MoodCategory.Neutral, Emoji = "🤔" },
            new Mood { Id = 8, Name = "Curious", Category = MoodCategory.Neutral, Emoji = "🧐" },
            new Mood { Id = 9, Name = "Nostalgic", Category = MoodCategory.Neutral, Emoji = "😌" },
            new Mood { Id = 10, Name = "Bored", Category = MoodCategory.Neutral, Emoji = "😑" },
            
            // Negative moods
            new Mood { Id = 11, Name = "Sad", Category = MoodCategory.Negative, Emoji = "😔" },
            new Mood { Id = 12, Name = "Angry", Category = MoodCategory.Negative, Emoji = "😠" },
            new Mood { Id = 13, Name = "Stressed", Category = MoodCategory.Negative, Emoji = "😰" },
            new Mood { Id = 14, Name = "Lonely", Category = MoodCategory.Negative, Emoji = "😢" },
            new Mood { Id = 15, Name = "Anxious", Category = MoodCategory.Negative, Emoji = "😟" }
        };
        
        modelBuilder.Entity<Mood>().HasData(moods);
    }
    
    private void SeedTags(ModelBuilder modelBuilder)
    {
        var predefinedTags = new[]
        {
            "Work", "Career", "Studies", "Family", "Friends", "Relationships",
            "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies", "Travel",
            "Nature", "Finance", "Spirituality", "Birthday", "Holiday", "Vacation",
            "Celebration", "Exercise", "Reading", "Writing", "Cooking", "Meditation",
            "Yoga", "Music", "Shopping", "Parenting", "Projects", "Planning", "Reflection"
        };
        
        var tags = new List<Tag>();
        for (int i = 0; i < predefinedTags.Length; i++)
        {
            tags.Add(new Tag
            {
                Id = i + 1,
                Name = predefinedTags[i],
                IsCustom = false
            });
        }
        
        modelBuilder.Entity<Tag>().HasData(tags);
    }
}
