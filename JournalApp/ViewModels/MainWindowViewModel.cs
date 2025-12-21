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
            
            // Load today's entry if it exists
            var todayEntry = await _journalService.GetEntryByDateAsync(DateTime.Now);
            if (todayEntry != null)
            {
                EntryTitle = todayEntry.Title;
                EntryContent = todayEntry.Content;
                SelectedPrimaryMood = Moods.FirstOrDefault(m => m.Id == todayEntry.PrimaryMoodId);
                StatusMessage = "Loaded today's entry";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading data: {ex.Message}";
        }
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
                PrimaryMoodId = SelectedPrimaryMood.Id
            };
            
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
        StatusMessage = "Form cleared";
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

