using JournalApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JournalApp.Services;

public class JournalService
{
    private readonly JournalDbContext _context;
    
    public JournalService()
    {
        _context = new JournalDbContext();
        _context.Database.EnsureCreated();
    }
    
    public async Task<List<JournalEntry>> GetAllEntriesAsync()
    {
        return await _context.JournalEntries
            .Include(e => e.PrimaryMood)
            .Include(e => e.SecondaryMood1)
            .Include(e => e.SecondaryMood2)
            .Include(e => e.Tags)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
    
    public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
        
        return await _context.JournalEntries
            .Include(e => e.PrimaryMood)
            .Include(e => e.SecondaryMood1)
            .Include(e => e.SecondaryMood2)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Date >= startOfDay && e.Date <= endOfDay);
    }
    
    public async Task<JournalEntry> CreateOrUpdateEntryAsync(JournalEntry entry)
    {
        var existingEntry = await GetEntryByDateAsync(entry.Date);
        
        if (existingEntry != null)
        {
            // Update existing entry
            existingEntry.Title = entry.Title;
            existingEntry.Content = entry.Content;
            existingEntry.PrimaryMoodId = entry.PrimaryMoodId;
            existingEntry.SecondaryMood1Id = entry.SecondaryMood1Id;
            existingEntry.SecondaryMood2Id = entry.SecondaryMood2Id;
            existingEntry.Category = entry.Category;
            existingEntry.UpdatedAt = DateTime.Now;
            
            // Update tags
            existingEntry.Tags.Clear();
            foreach (var tag in entry.Tags)
            {
                var existingTag = await _context.Tags.FindAsync(tag.Id);
                if (existingTag != null)
                {
                    existingEntry.Tags.Add(existingTag);
                }
            }
            
            await _context.SaveChangesAsync();
            return existingEntry;
        }
        else
        {
            // Create new entry
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;
            
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }
    }
    
    public async Task<bool> DeleteEntryAsync(int entryId)
    {
        var entry = await _context.JournalEntries.FindAsync(entryId);
        if (entry != null)
        {
            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    
    public async Task<List<Mood>> GetAllMoodsAsync()
    {
        return await _context.Moods.ToListAsync();
    }
    
    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }
}
