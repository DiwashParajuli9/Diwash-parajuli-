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

public partial class CalendarViewModel : ViewModelBase
{
    private readonly JournalService _journalService;
    
    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;
    
    [ObservableProperty]
    private JournalEntry? _selectedEntry;
    
    [ObservableProperty]
    private List<DateTime> _datesWithEntries = new();
    
    [ObservableProperty]
    private string _statusMessage = string.Empty;
    
    [ObservableProperty]
    private int _currentMonth;
    
    [ObservableProperty]
    private int _currentYear;
    
    public string CurrentMonthYear => new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM yyyy");
    
    public CalendarViewModel()
    {
        _journalService = new JournalService();
        CurrentMonth = DateTime.Today.Month;
        CurrentYear = DateTime.Today.Year;
        LoadEntriesForMonthAsync();
    }
    
    private async void LoadEntriesForMonthAsync()
    {
        try
        {
            var allEntries = await _journalService.GetAllEntriesAsync();
            DatesWithEntries = allEntries
                .Where(e => e.Date.Month == CurrentMonth && e.Date.Year == CurrentYear)
                .Select(e => e.Date.Date)
                .ToList();
            
            OnPropertyChanged(nameof(CurrentMonthYear));
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading entries: {ex.Message}";
        }
    }
    
    [RelayCommand]
    private async Task SelectDate(DateTime date)
    {
        SelectedDate = date;
        
        try
        {
            SelectedEntry = await _journalService.GetEntryByDateAsync(date);
            if (SelectedEntry == null)
            {
                StatusMessage = $"No entry for {date:MMMM dd, yyyy}";
            }
            else
            {
                StatusMessage = $"Loaded entry for {date:MMMM dd, yyyy}";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading entry: {ex.Message}";
        }
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
        LoadEntriesForMonthAsync();
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
        LoadEntriesForMonthAsync();
    }
    
    public bool HasEntryOnDate(DateTime date)
    {
        return DatesWithEntries.Any(d => d.Date == date.Date);
    }
}
