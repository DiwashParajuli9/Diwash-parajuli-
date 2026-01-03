using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JournalApp.Models;
using JournalApp.Services;

namespace JournalApp.ViewModels;

public partial class JournalListViewModel : ViewModelBase
{
    private readonly JournalService _journalService;
    
    [ObservableProperty]
    private ObservableCollection<JournalEntry> _entries = new();
    
    [ObservableProperty]
    private JournalEntry? _selectedEntry;
    
    [ObservableProperty]
    private int _currentPage = 1;
    
    [ObservableProperty]
    private int _pageSize = 10;
    
    [ObservableProperty]
    private int _totalEntries;
    
    [ObservableProperty]
    private string _statusMessage = string.Empty;
    
    public int TotalPages => (int)Math.Ceiling((double)TotalEntries / PageSize);
    
    public bool CanGoPrevious => CurrentPage > 1;
    
    public bool CanGoNext => CurrentPage < TotalPages;
    
    public JournalListViewModel()
    {
        _journalService = new JournalService();
        LoadEntriesAsync();
    }
    
    private async void LoadEntriesAsync()
    {
        try
        {
            var allEntries = await _journalService.GetAllEntriesAsync();
            TotalEntries = allEntries.Count;
            
            var pagedEntries = allEntries
                .OrderByDescending(e => e.Date)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
            Entries = new ObservableCollection<JournalEntry>(pagedEntries);
            
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading entries: {ex.Message}";
        }
    }
    
    [RelayCommand]
    private void PreviousPage()
    {
        if (CanGoPrevious)
        {
            CurrentPage--;
            LoadEntriesAsync();
        }
    }
    
    [RelayCommand]
    private void NextPage()
    {
        if (CanGoNext)
        {
            CurrentPage++;
            LoadEntriesAsync();
        }
    }
    
    [RelayCommand]
    private async Task DeleteEntry(JournalEntry entry)
    {
        try
        {
            if (entry != null)
            {
                await _journalService.DeleteEntryAsync(entry.Id);
                StatusMessage = "Entry deleted successfully";
                LoadEntriesAsync();
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting entry: {ex.Message}";
        }
    }
}
