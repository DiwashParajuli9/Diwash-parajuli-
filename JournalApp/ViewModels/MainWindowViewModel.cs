using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JournalApp.Models;
using JournalApp.Services;

namespace JournalApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly JournalService _journalService;
    
    [ObservableProperty]
    private string _currentDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
    
    [ObservableProperty]
    private string _entryTitle = string.Empty;
    
    [ObservableProperty]
    private string _entryContent = string.Empty;
    
    [ObservableProperty]
    private string _statusMessage = string.Empty;
    
    [ObservableProperty]
    private ObservableCollection<Mood> _moods = new();
    
    [ObservableProperty]
    private Mood? _selectedPrimaryMood;
    
    [ObservableProperty]
    private Mood? _selectedSecondaryMood1;
    
    [ObservableProperty]
    private Mood? _selectedSecondaryMood2;
    
    [ObservableProperty]
    private ObservableCollection<Tag> _availableTags = new();
    
    [ObservableProperty]
    private ObservableCollection<Tag> _selectedTags = new();
    
    [ObservableProperty]
    private ObservableCollection<JournalEntry> _allEntries = new();
    
    [ObservableProperty]
    private ObservableCollection<JournalEntry> _monthEntries = new();
    
    [ObservableProperty]
    private int _currentMonth = DateTime.Today.Month;
    
    [ObservableProperty]
    private int _currentYear = DateTime.Today.Year;
    
    public string CurrentMonthYear => new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM yyyy");
    
    public string WordCountText => $"Word count: {CalculateWordCount()}";
    
    public MainWindowViewModel()
    {
        _journalService = new JournalService();
        LoadDataAsync();
    }
    
    private async void LoadDataAsync()
    {
        try
        {
            // Load moods
            var moods = await _journalService.GetAllMoodsAsync();
            Moods = new ObservableCollection<Mood>(moods);
            
            // Load tags
            var tags = await _journalService.GetAllTagsAsync();
            AvailableTags = new ObservableCollection<Tag>(tags);
            
            // Load all entries for the list view
            var entries = await _journalService.GetAllEntriesAsync();
            AllEntries = new ObservableCollection<JournalEntry>(entries.OrderByDescending(e => e.Date).Take(20));
            
            // Load entries for current month (calendar view)
            LoadMonthEntries();
            
            // Load today's entry if it exists
            var todayEntry = await _journalService.GetEntryByDateAsync(DateTime.Now);
            if (todayEntry != null)
            {
                EntryTitle = todayEntry.Title;
                EntryContent = todayEntry.Content;
                SelectedPrimaryMood = Moods.FirstOrDefault(m => m.Id == todayEntry.PrimaryMoodId);
                SelectedSecondaryMood1 = todayEntry.SecondaryMood1Id.HasValue 
                    ? Moods.FirstOrDefault(m => m.Id == todayEntry.SecondaryMood1Id.Value) 
                    : null;
                SelectedSecondaryMood2 = todayEntry.SecondaryMood2Id.HasValue 
                    ? Moods.FirstOrDefault(m => m.Id == todayEntry.SecondaryMood2Id.Value) 
                    : null;
                
                // Load selected tags
                SelectedTags.Clear();
                foreach (var tag in todayEntry.Tags)
                {
                    SelectedTags.Add(tag);
                }
                
                StatusMessage = "Loaded today's entry";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading data: {ex.Message}";
        }
    }
    
    private void LoadMonthEntries()
    {
        var entriesForMonth = AllEntries
            .Where(e => e.Date.Month == CurrentMonth && e.Date.Year == CurrentYear)
            .OrderByDescending(e => e.Date)
            .ToList();
        MonthEntries = new ObservableCollection<JournalEntry>(entriesForMonth);
        OnPropertyChanged(nameof(CurrentMonthYear));
    }
    
    [RelayCommand]
    private async Task SaveEntry()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EntryContent))
            {
                StatusMessage = "Please write something in your entry!";
                return;
            }
            
            if (SelectedPrimaryMood == null)
            {
                StatusMessage = "Please select your primary mood!";
                return;
            }
            
            var entry = new JournalEntry
            {
                Date = DateTime.Now,
                Title = EntryTitle,
                Content = EntryContent,
                PrimaryMoodId = SelectedPrimaryMood.Id,
                SecondaryMood1Id = SelectedSecondaryMood1?.Id,
                SecondaryMood2Id = SelectedSecondaryMood2?.Id
            };
            
            // Add selected tags
            foreach (var tag in SelectedTags)
            {
                entry.Tags.Add(tag);
            }
            
            await _journalService.CreateOrUpdateEntryAsync(entry);
            StatusMessage = "Entry saved successfully! ✓";
            
            // Clear status after a delay
            const int statusMessageDelayMs = 3000;
            await Task.Delay(statusMessageDelayMs);
            StatusMessage = string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving entry: {ex.Message}";
        }
    }
    
    [RelayCommand]
    private void Clear()
    {
        EntryTitle = string.Empty;
        EntryContent = string.Empty;
        SelectedPrimaryMood = null;
        SelectedSecondaryMood1 = null;
        SelectedSecondaryMood2 = null;
        SelectedTags.Clear();
        StatusMessage = "Form cleared";
    }
    
    [RelayCommand]
    private void AddTag(Tag tag)
    {
        if (!SelectedTags.Contains(tag))
        {
            SelectedTags.Add(tag);
        }
    }
    
    [RelayCommand]
    private void RemoveTag(Tag tag)
    {
        SelectedTags.Remove(tag);
    }
    
    [RelayCommand]
    private void PreviousMonth()
    {
        if (CurrentMonth == 1)
        {
            CurrentMonth = 12;
            CurrentYear--;
        }
        else
        {
            CurrentMonth--;
        }
        LoadMonthEntries();
    }
    
    [RelayCommand]
    private void NextMonth()
    {
        if (CurrentMonth == 12)
        {
            CurrentMonth = 1;
            CurrentYear++;
        }
        else
        {
            CurrentMonth++;
        }
        LoadMonthEntries();
    }
    
    private int CalculateWordCount()
    {
        if (string.IsNullOrWhiteSpace(EntryContent))
            return 0;
        
        return EntryContent.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
    
    partial void OnEntryContentChanged(string value)
    {
        OnPropertyChanged(nameof(WordCountText));
    }
}

