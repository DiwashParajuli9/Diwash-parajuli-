# My Daily Journal Application

## Project Overview

**Purpose:** This is a secure, feature-rich desktop journal application designed to modernize personal journaling with a digital solution.

**Scope:** The application allows users to create, update, and delete daily journal entries with rich mood tracking, tagging system, and data persistence using SQLite database.

**Objectives:**
- Provide a simple and intuitive interface for daily journaling
- Enable mood tracking with primary and secondary moods
- Support data persistence with local SQLite database
- Implement CRUD operations for journal entries
- Follow MVVM architectural pattern for maintainability

## Technology Stack

### Framework
- **Avalonia UI 11.3.10** - Cross-platform MVVM desktop framework for .NET
  - Chosen because: Cross-platform support (Windows, Linux, macOS), modern XAML-based UI, MVVM architecture support
  - Alternative to MAUI which requires specific platform support

### Database
- **SQLite with Entity Framework Core 9.0.0**
  - Provides local data persistence
  - Lightweight and file-based
  - No server setup required

### External Libraries
1. **Avalonia** (v11.3.10) - UI framework
2. **Avalonia.Desktop** (v11.3.10) - Desktop platform support
3. **Avalonia.Themes.Fluent** (v11.3.10) - Modern UI theme
4. **CommunityToolkit.Mvvm** (v8.2.1) - MVVM helpers (ObservableProperty, RelayCommand)
5. **Microsoft.EntityFrameworkCore.Sqlite** (v9.0.0) - SQLite database provider
6. **Microsoft.EntityFrameworkCore.Design** (v9.0.0) - EF Core design-time tools

## Features Implemented (Milestone 1)

### ✅ Feature 1: Journal Entry Management

**Description:** Users can create, update, and delete daily journal entries. The system ensures only one entry per day with automatic timestamp management.

**Implementation Details:**
- Created `JournalEntry` model with fields: Id, Date, Title, Content, CreatedAt, UpdatedAt
- Implemented `JournalService` with methods:
  - `CreateOrUpdateEntryAsync()` - Creates new or updates existing entry
  - `GetEntryByDateAsync()` - Retrieves entry for a specific date
  - `DeleteEntryAsync()` - Deletes an entry
  - `GetAllEntriesAsync()` - Retrieves all entries
- UI provides input fields for title and content
- Automatic word count calculation
- Save and Clear buttons for entry management

**Database Schema:**
```
JournalEntries Table:
- Id (Primary Key)
- Date (DateTime, indexed)
- Title (String, optional)
- Content (String, required)
- CreatedAt (DateTime, auto-generated)
- UpdatedAt (DateTime, auto-updated)
- PrimaryMoodId (Foreign Key)
- SecondaryMood1Id (Foreign Key, optional)
- SecondaryMood2Id (Foreign Key, optional)
- Category (String)
```

### ✅ Feature 2: Mood Tracking
**Status:** Completed (Primary Mood)

**Description:** Users can track their emotional state by selecting a primary mood from predefined categories (Positive, Neutral, Negative).

**Implementation Details:**
- Created `Mood` model with properties: Id, Name, Category, Emoji
- Created `MoodCategory` enum: Positive, Neutral, Negative
- Seeded 15 predefined moods in database:
  - **Positive:** Happy 😊, Excited 🤗, Relaxed 😌, Grateful 🙏, Confident 💪
  - **Neutral:** Calm 😐, Thoughtful 🤔, Curious 🧐, Nostalgic 😌, Bored 😑
  - **Negative:** Sad 😔, Angry 😠, Stressed 😰, Lonely 😢, Anxious 😟
- UI provides ComboBox with emoji and mood name for easy selection
- Primary mood is required for each entry

**Database Schema:**
```
Moods Table:
- Id (Primary Key)
- Name (String)
- Category (Enum: Positive/Neutral/Negative)
- Emoji (String)
```

## Features Implemented (Milestone 2)

### ✅ Feature 3: Rich Text/Markdown Writing
**Status:** Completed

**Description:** Support for rich text and markdown formatting in journal entries.

**Implementation Details:**
- Added `Markdown.Avalonia` package for markdown rendering support
- Multi-line TextBox with text wrapping for content entry
- Word count calculation updates in real-time
- AcceptsReturn enabled for multi-line input
- Users can write with markdown syntax (**, *, lists, etc.)

### ✅ Feature 4: Secondary Mood Selection
**Status:** Completed

**Description:** Users can select up to 2 additional secondary moods to capture complex emotional states.

**Implementation Details:**
- Two additional ComboBox controls for secondary moods
- Optional selection (not required like primary mood)
- All three moods (primary + 2 secondary) saved to database
- Loaded when viewing existing entries
- Foreign key relationships established in database
- UI displays emoji and mood name for easy selection

### ✅ Feature 5: Tagging System
**Status:** Completed

**Description:** Users can categorize entries with custom or predefined tags.

**Implementation Details:**
- Display of 31 predefined tags in clickable chips
- Click to add tag to current entry
- Selected tags shown with "×" remove button
- Chip-style UI with rounded corners and colors
- Tags stored with many-to-many relationship
- Tags loaded and displayed when viewing entries
- ScrollViewer for tag selection area

**Predefined Tags:**
Work, Career, Studies, Family, Friends, Relationships, Health, Fitness, Personal Growth, Self-care, Hobbies, Travel, Nature, Finance, Spirituality, Birthday, Holiday, Vacation, Celebration, Exercise, Reading, Writing, Cooking, Meditation, Yoga, Music, Shopping, Parenting, Projects, Planning, Reflection

### ✅ Feature 6: Calendar Navigation
**Status:** Completed

**Description:** Navigate journal entries through a calendar-based interface.

**Implementation Details:**
- Month/Year navigation with Previous/Next buttons
- Current month display (e.g., "January 2026")
- List of entries for selected month
- Entries shown with date, mood emoji, and title
- Month navigation updates the entry list dynamically
- Tab-based UI for easy access

### ✅ Feature 7: Paginated Journal View
**Status:** Completed

**Description:** View journal entries in a paginated list format.

**Implementation Details:**
- Separate "My Entries" tab for browsing entries
- Card-based layout for each entry
- Display of:
  - Entry date (formatted as "MMM dd, yyyy")
  - Title (optional, shown in italic)
  - Content preview (2 lines max with ellipsis)
  - Primary mood with emoji and name
  - Word count
- Shows most recent 20 entries
- ScrollViewer for browsing through list
- Clean, organized card design with borders and padding

## UI Improvements (Milestone 2)

### Tab-Based Navigation
- **New Entry Tab (✍)**: Create/edit today's journal entry
- **My Entries Tab (📚)**: Browse all journal entries in list view
- **Calendar Tab (📅)**: Navigate entries by month

### Enhanced Form
- Larger window size (900×700)
- Better organized sections
- Clear visual hierarchy
- Status messages for user feedback

### Tag Selection Interface
- Chip-style tag display
- Color-coded selected vs available tags
- Easy add/remove functionality
- ScrollViewer for many tags

## Data/Entity Modelling

### Entity Relationship Diagram

```
┌─────────────────┐
│  JournalEntry   │
├─────────────────┤
│ Id (PK)         │
│ Date            │
│ Title           │
│ Content         │
│ CreatedAt       │
│ UpdatedAt       │
│ PrimaryMoodId   │──────┐
│ SecondaryMood1Id│──┐   │
│ SecondaryMood2Id│──┼───┼───┐
│ Category        │  │   │   │
└─────────────────┘  │   │   │
                     │   │   │
        ┌────────────┘   │   │
        │  ┌─────────────┘   │
        │  │  ┌──────────────┘
        ▼  ▼  ▼
    ┌──────────┐
    │   Mood   │
    ├──────────┤
    │ Id (PK)  │
    │ Name     │
    │ Category │
    │ Emoji    │
    └──────────┘

┌─────────────────┐        ┌──────────┐
│  JournalEntry   │───────<│   Tag    │
└─────────────────┘ Many   ├──────────┤
                    to     │ Id (PK)  │
                    Many   │ Name     │
                           │ IsCustom │
                           └──────────┘
```

### Key Entities

1. **JournalEntry**
   - Represents a single daily journal entry
   - Has relationships to Mood entities
   - Can have multiple tags (many-to-many)

2. **Mood**
   - Represents an emotional state
   - Categorized as Positive, Neutral, or Negative
   - Has emoji representation

3. **Tag**
   - Categorizes journal entries
   - Can be custom or predefined
   - Many-to-many relationship with JournalEntry

4. **MoodCategory (Enum)**
   - Positive, Neutral, Negative

## Project Structure

```
JournalApp/
├── Data/
│   └── JournalDbContext.cs       # Database context with seed data
├── Models/
│   ├── JournalEntry.cs           # Journal entry entity
│   ├── Mood.cs                   # Mood entity
│   ├── MoodCategory.cs           # Mood category enum
│   └── Tag.cs                    # Tag entity
├── Services/
│   └── JournalService.cs         # Business logic for journal operations
├── ViewModels/
│   ├── MainWindowViewModel.cs    # Main window view model with calendar
│   ├── JournalListViewModel.cs   # Journal list pagination view model
│   ├── CalendarViewModel.cs      # Calendar navigation view model
│   └── ViewModelBase.cs          # Base view model class
├── Views/
│   ├── MainWindow.axaml          # Main window with tabs
│   └── MainWindow.axaml.cs       # Main window code-behind
├── Migrations/
│   └── [EF Core migration files]
├── App.axaml                     # Application XAML
├── Program.cs                    # Application entry point
└── JournalApp.csproj             # Project file
```

## UI Design (Wireframe)

### Updated Main Window (Tab-Based Interface)
```
┌────────────────────────────────────────────────────┐
│  [✍ New Entry] [📚 My Entries] [📅 Calendar]      │
├────────────────────────────────────────────────────┤
│              My Daily Journal                      │
│                                                    │
│           Today: Saturday, January 3, 2026         │
│                                                    │
│  Title (Optional):                                │
│  ┌──────────────────────────────────────────────┐ │
│  │ Enter a title for today's entry...           │ │
│  └──────────────────────────────────────────────┘ │
│                                                    │
│  How are you feeling? (Primary Mood)*             │
│  ┌──────────────────────────────────────────────┐ │
│  │ 😊 Happy                              ▼     │ │
│  └──────────────────────────────────────────────┘ │
│                                                    │
│  Secondary Moods (Optional - up to 2):            │
│  ┌───────────────┐ ┌───────────────┐            │
│  │ Secondary 1 ▼│ │ Secondary 2 ▼│            │
│  └───────────────┘ └───────────────┘            │
│                                                    │
│  Tags (Optional):                                 │
│  Selected: [Work ×] [Health ×]                   │
│  Available: [Family] [Friends] [Travel]...       │
│                                                    │
│  What's on your mind?*                            │
│  ┌──────────────────────────────────────────────┐ │
│  │ Write about your day...                      │ │
│  │                                              │ │
│  └──────────────────────────────────────────────┘ │
│                             Word count: 0         │
│                                                    │
│        [Save Entry]      [Clear]                  │
│                                                    │
│              Entry saved successfully! ✓          │
└────────────────────────────────────────────────────┘
```

### Original Main Window (Journal Entry Form)
```
┌──────────────────────────────────────────────────┐
│              My Daily Journal                    │
├──────────────────────────────────────────────────┤
│                                                  │
│           Today: Saturday, December 21, 2025     │
│                                                  │
│  Title (Optional):                              │
│  ┌────────────────────────────────────────────┐ │
│  │ Enter a title for today's entry...         │ │
│  └────────────────────────────────────────────┘ │
│                                                  │
│  How are you feeling? (Primary Mood)*           │
│  ┌────────────────────────────────────────────┐ │
│  │ 😊 Happy                              ▼   │ │
│  └────────────────────────────────────────────┘ │
│                                                  │
│  What's on your mind?*                          │
│  ┌────────────────────────────────────────────┐ │
│  │ Write about your day...                    │ │
│  │                                            │ │
│  │                                            │ │
│  │                                            │ │
│  └────────────────────────────────────────────┘ │
│                             Word count: 0        │
│                                                  │
│        [Save Entry]      [Clear]                │
│                                                  │
│              Entry saved successfully! ✓         │
└──────────────────────────────────────────────────┘
```

## Database Schema

### Tables

1. **JournalEntries**
   - Stores all journal entries
   - One entry per day constraint enforced at application level
   - Tracks creation and update timestamps

2. **Moods**
   - Predefined mood options
   - Seeded with 15 moods across 3 categories

3. **Tags**
   - Seeded with 31 predefined tags
   - Supports custom tags (to be implemented)

## How to Build and Run

### Prerequisites
- .NET 9.0 SDK or higher
- Any operating system (Windows, Linux, macOS)

### Build Instructions
```bash
cd JournalApp
dotnet restore
dotnet build
```

### Run Application
```bash
dotnet run
```

### Database Setup
The database is automatically created on first run using Entity Framework Core migrations.

Database location: `%LocalAppData%/journal.db` (Windows) or `~/.local/share/journal.db` (Linux/macOS)

## Development Practices

### Architecture Pattern
- **MVVM (Model-View-ViewModel)** pattern for separation of concerns
- Models in `Models/` folder
- Views in `Views/` folder (AXAML)
- ViewModels in `ViewModels/` folder

### Version Control
- Git repository initialized
- Regular commits with meaningful messages
- `.gitignore` configured to exclude:
  - `bin/` and `obj/` directories
  - `.vs/` and `.vscode/` folders
  - User-specific files

### Code Quality
- Following C# naming conventions
- Using nullable reference types
- Async/await for database operations
- Dependency injection ready architecture

## Next Steps (Future Milestones)

### Milestone 2 (Week 11) - ✅ COMPLETED
- ✅ Secondary mood selection (up to 2 additional moods)
- ✅ Tag system implementation
- ✅ Rich Text/Markdown writing support
- ✅ Calendar navigation
- ✅ Paginated journal view

### Milestone 3 (Week 13) - PLANNED
- Search and filter functionality
- Streak tracking (daily, longest, missed days)
- Dashboard with analytics (mood distribution, word count trends)
- Export to PDF by date range
- Security (password/PIN protection)
- Theme customization (light/dark mode)
- Full calendar grid with date highlighting

## Project Status

**Current Milestone:** Milestone 2 Complete ✅  
**Features Implemented:** 7 out of 12 total features (58%)  
**Milestones Completed:** 2 out of 3 (67%)  

### Milestone Summary
- **Milestone 1:** 2 features ✅
  1. Journal Entry Management
  2. Primary Mood Tracking
  
- **Milestone 2:** 5 features ✅
  3. Rich Text/Markdown Writing
  4. Secondary Mood Selection
  5. Tagging System
  6. Calendar Navigation
  7. Paginated Journal View

- **Milestone 3:** 5 features (planned)
  8. Search & Filter
  9. Streak Tracking
  10. Dashboard Analytics
  11. Security & Privacy
  12. Export to PDF

## Individual Contribution

**Developer:** Diwash Parajuli
**Type:** Individual Project
**Contribution:** 100% - All design, development, and documentation

## References

- Avalonia UI Documentation: https://docs.avaloniaui.net/
- Entity Framework Core Documentation: https://learn.microsoft.com/en-us/ef/core/
- SQLite Documentation: https://www.sqlite.org/docs.html
- CommunityToolkit.Mvvm Documentation: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/
- Markdown.Avalonia: https://github.com/whistyun/Markdown.Avalonia

---

**Project Status:** Milestone 2 Complete ✅  
**Last Updated:** January 3, 2026
