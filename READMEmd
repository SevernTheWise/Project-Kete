# Project Kete — Full Specification
**NZ Civil Defence Emergency Preparedness Tracker**  
*A C# Console Application*

---

## Overview

Project Kete is a console application that helps a Dunedin household track their Civil Defence emergency kit. It is seeded with NZ Civil Defence's actual 72-hour kit recommendations, adjusts required quantities based on household size, calculates a readiness score, and flags expired or expiring items. Data persists between sessions via CSV files.

The project should be built in Visual Studio 2022, targeting .NET 8.0, with top-level statements disabled. It must include a separate MSTest project.

---

## Solution Structure

```
ProjectKete/               ← Solution folder
│
├── Kete/                  ← Main project
│   ├── Program.cs
│   ├── Models/
│   │   ├── EmergencyItem.cs
│   │   └── HouseholdProfile.cs
│   ├── Services/
│   │   ├── InventoryManager.cs
│   │   ├── ReadinessCalculator.cs
│   │   ├── FileHandler.cs
│   │   └── ReportGenerator.cs
│   └── Data/
│       └── SeedData.cs
│
└── Kete.Tests/            ← MSTest project
    ├── EmergencyItemTests.cs
    ├── ReadinessCalculatorTests.cs
    ├── HouseholdProfileTests.cs
    ├── InventoryManagerTests.cs
    └── FileHandlerTests.cs
```

---

## Data Design

### Categories
Use these exact strings as your category values throughout the application:

- `"Water"`
- `"Food"`
- `"First Aid"`
- `"Light & Power"`
- `"Communication"`
- `"Warmth & Clothing"`
- `"Sanitation"`
- `"Documents & Cash"`
- `"Tools"`
- `"Pet Supplies"`

### Units
Items have a unit of measurement. Use strings:  
`"litres"`, `"days supply"`, `"items"`, `"sets"`, `"kg"`, `"doses"`

---

## Class Specifications

---

### `EmergencyItem` — `Models/EmergencyItem.cs`

Represents a single item in the emergency kit.

**Fields (all public):**

| Field | Type | Description |
|---|---|---|
| `Name` | `string` | Name of the item e.g. "Bottled Water" |
| `Category` | `string` | One of the ten categories listed above |
| `RecommendedQtyPerPerson` | `double` | How much is needed per person per 72hrs |
| `Unit` | `string` | The unit of measurement |
| `OwnedQuantity` | `double` | How much the household currently has |
| `IsNonExpiring` | `bool` | True if the item does not expire (e.g. a torch) |
| `ExpiryDate` | `string` | Expiry date as `"dd/MM/yyyy"`, or `"N/A"` if non-expiring |
| `Notes` | `string` | Optional freeform notes, empty string if none |

> **Note on dates:** Store ExpiryDate as a string internally. You will need to convert it to a `DateTime` when doing comparisons. Look up `DateTime.ParseExact()` and `DateTime.TryParseExact()` — you will need both. This is intentional: learning to handle date strings is a real-world skill.

**Constructor:**

One constructor accepting all fields in the order listed above.

**Methods:**

`IsExpired()`  
Returns `bool`. Returns `false` if `IsNonExpiring` is true. Otherwise parses `ExpiryDate` and returns `true` if that date is before today's date.

`IsExpiringSoon(int daysThreshold)`  
Returns `bool`. Returns `false` if `IsNonExpiring` is true. Otherwise returns `true` if the item expires within `daysThreshold` days from today. An already-expired item should also return `true` here.

`GetShortSummary()`  
Returns `string`. Returns a single formatted line suitable for console display. Include: name, category, owned quantity with unit, and expiry status. Design the format yourself — it should be readable at a glance.

`GetDetailedSummary()`  
Returns `string`. Returns a multi-line string with all fields displayed with labels. Used when viewing a single item in detail.

---

### `HouseholdProfile` — `Models/HouseholdProfile.cs`

Represents the household being assessed.

**Fields (all public):**

| Field | Type | Description |
|---|---|---|
| `HouseholdName` | `string` | e.g. "The Smith Household" |
| `Adults` | `int` | Number of adults |
| `Children` | `int` | Number of children (counts as 1 person for quantities) |
| `Pets` | `int` | Number of pets |
| `HasMedicalNeeds` | `bool` | True if anyone has specific medical requirements |
| `MedicalNotes` | `string` | Description of medical needs, empty string if none |

**Constructor:**

One constructor accepting all fields in the order listed above.

**Methods:**

`GetTotalPeople()`  
Returns `int`. Returns adults + children. Does not include pets.

`GetRequiredQuantity(EmergencyItem item)`  
Returns `double`. Returns `item.RecommendedQtyPerPerson * GetTotalPeople()`. This is how much of that item the household should have.

`GetSummary()`  
Returns `string`. Returns a formatted multi-line string displaying all household details including total people count.

---

### `InventoryManager` — `Services/InventoryManager.cs`

Manages the collection of emergency items. This is the core service class.

**Fields:**

| Field | Type | Description |
|---|---|---|
| `Items` | `EmergencyItem[]` | The array of all items |
| `Profile` | `HouseholdProfile` | The household profile |
| `_count` | `int` (private) | How many items are currently stored |

> **On array sizing:** Declare your array with a fixed maximum size (suggest 100). Use `_count` to track how many slots are actually filled. Do not use `List<T>` — manage the array manually. This is deliberate: you will understand memory and indices better for it.

**Constructor:**

Accepts a `HouseholdProfile` and initialises the items array and count.

**Methods:**

`AddItem(EmergencyItem item)`  
Returns `void`. Adds the item to the next available slot. If the array is full, display a message and return without adding.

`RemoveItem(int index)`  
Returns `bool`. Removes the item at the given index by shifting all subsequent items left by one position and decrementing `_count`. Returns `true` if successful, `false` if index is out of range.

`GetItem(int index)`  
Returns `EmergencyItem` or `null`. Returns the item at the given index, or `null` if index is out of range.

`GetCount()`  
Returns `int`. Returns `_count`.

`FindByName(string name)`  
Returns `int`. Searches for an item whose name contains the search string (case-insensitive). Returns the index of the first match, or -1 if not found.

`FindAllByCategory(string category)`  
Returns `int[]`. Returns an array of indices of all items in the given category. Returns an empty array if none found.

`GetExpiredItems()`  
Returns `int[]`. Returns indices of all items where `IsExpired()` is true.

`GetExpiringSoonItems(int daysThreshold)`  
Returns `int[]`. Returns indices of all items where `IsExpiringSoon(daysThreshold)` is true and `IsExpired()` is false. (Expired items are handled separately.)

`SortByName()`  
Returns `void`. Sorts `Items` (up to `_count`) alphabetically by name using a bubble sort. Use `.CompareTo()` for string comparison. Do not use any built-in sort methods.

`SortByExpiry()`  
Returns `void`. Sorts items by expiry date, ascending. Non-expiring items go to the end. Use bubble sort. You will need to parse the date strings to compare them.

`SortByCategory()`  
Returns `void`. Sorts items by category name alphabetically. Use bubble sort.

`UpdateItem(int index, EmergencyItem updatedItem)`  
Returns `bool`. Replaces the item at the given index with `updatedItem`. Returns `true` if successful, `false` if index is out of range.

---

### `ReadinessCalculator` — `Services/ReadinessCalculator.cs`

Calculates the household's readiness score. This class should have no fields — all methods are static.

**Methods (all static):**

`CalculateScore(EmergencyItem[] items, int count, HouseholdProfile profile)`  
Returns `double`. 

The score is calculated as follows:
- For each item, calculate `profile.GetRequiredQuantity(item)` — the amount needed
- If the household owns at least the required quantity, the item is "fully covered"
- If they own more than zero but less than required, the item is "partially covered" — count it as `OwnedQuantity / RequiredQuantity` (a fraction between 0 and 1)
- If they own zero, the item contributes 0
- Non-expiring items that are "fully covered" contribute 1 regardless of expiry
- Expiring items that are expired contribute 0, even if OwnedQuantity >= RequiredQuantity
- Sum all contributions, divide by `count`, multiply by 100
- Return the result as a double between 0 and 100
- If `count` is 0, return 0

`GetReadinessLevel(double score)`  
Returns `string`. Maps the score to a Civil Defence readiness level:
- 0–24: `"Critical — not prepared"`
- 25–49: `"Low — significant gaps"`
- 50–74: `"Moderate — some key items missing"`
- 75–89: `"Good — nearly ready"`
- 90–99: `"High — minor improvements needed"`
- 100: `"Fully Prepared"`

`GetReadinessColour(double score)`  
Returns `ConsoleColor`. Maps score to a console colour for display:
- 0–24: `ConsoleColor.Red`
- 25–49: `ConsoleColor.DarkYellow`
- 50–74: `ConsoleColor.Yellow`
- 75–89: `ConsoleColor.Cyan`
- 90–100: `ConsoleColor.Green`

---

### `FileHandler` — `Services/FileHandler.cs`

Handles all file reading and writing. All methods are static.

**File format:**

Two files are used:

`kete_profile.txt` — One line per field, in this exact order:
```
HouseholdName
Adults
Children
Pets
HasMedicalNeeds
MedicalNotes
```

`kete_items.csv` — One item per line, comma-separated, in this exact order:
```
Name,Category,RecommendedQtyPerPerson,Unit,OwnedQuantity,IsNonExpiring,ExpiryDate,Notes
```

> Both files are saved in the same directory as the executable. Notes and MedicalNotes fields may contain commas — wrap them in double quotes when writing, handle that when reading.

**Methods (all static):**

`SaveProfile(HouseholdProfile profile, string filePath)`  
Returns `void`. Writes the profile to `kete_profile.txt` using `StreamWriter`.

`LoadProfile(string filePath)`  
Returns `HouseholdProfile` or `null`. Reads the profile from file. Returns `null` if the file does not exist or cannot be parsed. Use try/catch — this is one of the places you are expected to handle exceptions.

`SaveItems(EmergencyItem[] items, int count, string filePath)`  
Returns `void`. Writes all items (up to `count`) to `kete_items.csv`.

`LoadItems(string filePath)`  
Returns `EmergencyItem[]`. Reads all items from file. Returns an empty array if the file does not exist. Use try/catch.

> **Note:** You will need `using System.IO;` at the top of this file. Look up `StreamWriter` and `StreamReader` — you have used them in your lab book.

---

### `ReportGenerator` — `Services/ReportGenerator.cs`

Generates a plain-text report that could be printed. All methods are static.

**Methods:**

`GenerateReport(EmergencyItem[] items, int count, HouseholdProfile profile)`  
Returns `string`. Produces a full formatted report as a single string containing:
- A header with the household name and date generated
- Household summary (from `profile.GetSummary()`)
- Readiness score and level
- A section listing all fully-covered items
- A section listing all partially-covered items with the shortfall quantity
- A section listing all items with zero owned
- A section listing expired items
- A section listing items expiring within 30 days
- A footer with a Civil Defence reminder message (write something appropriate — look up the real getready.govt.nz guidance for inspiration)

`SaveReportToFile(string report, string filePath)`  
Returns `bool`. Writes the report string to the given file path. Returns `true` on success, `false` on failure. Use try/catch.

`PrintReportToConsole(string report)`  
Returns `void`. Writes the report to the console. Because the report can be long, pause every 20 lines and ask the user to press Enter to continue.

---

### `SeedData` — `Data/SeedData.cs`

Provides the default NZ Civil Defence item list. All methods are static.

**Methods:**

`GetDefaultItems()`  
Returns `EmergencyItem[]`. Returns an array containing the following pre-built items. These are based on NZ Civil Defence's actual getready.govt.nz guidance.

| Name | Category | Qty/Person | Unit | IsNonExpiring |
|---|---|---|---|---|
| Bottled Water | Water | 3.0 | litres | false |
| Water Purification Tablets | Water | 1.0 | items | false |
| Non-Perishable Food | Food | 3.0 | days supply | false |
| Manual Can Opener | Food | 1.0 | items | true |
| First Aid Kit | First Aid | 1.0 | items | false |
| Prescription Medications | First Aid | 3.0 | doses | false |
| Torch | Light & Power | 1.0 | items | true |
| Spare Batteries | Light & Power | 2.0 | items | false |
| Candles | Light & Power | 3.0 | items | false |
| Waterproof Matches or Lighter | Light & Power | 1.0 | items | true |
| Battery or Wind-up Radio | Communication | 1.0 | items | true |
| Charged Portable Battery Bank | Communication | 1.0 | items | false |
| Cash (Small Denominations) | Documents & Cash | 1.0 | items | true |
| Copies of Important Documents | Documents & Cash | 1.0 | sets | true |
| Warm Clothing | Warmth & Clothing | 1.0 | sets | true |
| Sturdy Footwear | Warmth & Clothing | 1.0 | sets | true |
| Rain Gear | Warmth & Clothing | 1.0 | sets | true |
| Sleeping Bag or Warm Blanket | Warmth & Clothing | 1.0 | items | true |
| Toilet Paper | Sanitation | 1.0 | items | false |
| Hand Sanitiser | Sanitation | 1.0 | items | false |
| Work Gloves | Tools | 1.0 | sets | true |
| Basic Tool Kit | Tools | 1.0 | items | true |
| Whistle | Tools | 1.0 | items | true |
| Pet Food | Pet Supplies | 3.0 | days supply | false |
| Pet Water | Pet Supplies | 1.5 | litres | false |

For all items, set `OwnedQuantity` to `0`, `ExpiryDate` to `"N/A"`, and `Notes` to `""`.

Pet items (`"Pet Supplies"`) should only be included if `profile.Pets > 0`. However, `GetDefaultItems()` does not take a profile parameter — handle this filtering in `Program.cs` when seeding.

---

### `Program.cs`

The entry point. Contains the main menu loop and all user interaction. No logic beyond input handling, calling service methods, and displaying results belongs here.

**Startup behaviour:**

1. Display a welcome banner with the project name and a brief description
2. Attempt to load `kete_profile.txt` and `kete_items.csv`
3. If no profile file exists, run first-time setup:
   - Prompt the user for all household profile fields
   - Ask if they want to start with the NZ Civil Defence recommended item list (Y/N)
   - If yes, seed with `SeedData.GetDefaultItems()` (filtering pet items based on profile)
   - Save both files
4. If files exist, load and confirm: display household name and item count

**Main menu:**

```
=== PROJECT KETE ===
[Household: The Smith Household | Items: 25 | Score: 42%]

 1. View all items
 2. View items by category
 3. Add item
 4. Edit item
 5. Remove item
 6. View expired / expiring items
 7. View readiness score
 8. Sort items
 9. Generate report
10. Manage household profile
 0. Save and exit
```

The status line (household name, item count, score) must update each time the menu is displayed.

**Menu option detail:**

**1 — View all items**  
Display all items using `GetShortSummary()`. Number each item from 1. Use `Console.ForegroundColor` to colour expired items red and expiring-soon items yellow. Pause if more than 20 items.

**2 — View items by category**  
Show the list of categories, let the user pick one, display matching items.

**3 — Add item**  
Prompt the user for each field. Validate that numeric fields are actually numbers (use try/catch or `double.TryParse()`). Validate that category is one of the ten valid options. Add to inventory.

**4 — Edit item**  
Ask for item number (from View all). Display current values. For each field, display the current value and let the user press Enter to keep it or type a new value. Save the updated item.

**5 — Remove item**  
Ask for item number. Confirm before removing (Y/N). Remove and display confirmation.

**6 — View expired / expiring items**  
Show two sections: expired items (red), and items expiring within 30 days (yellow). If neither exists, display a positive message.

**7 — View readiness score**  
Display the score as a percentage, the readiness level string, and a breakdown by category showing how many items in each category are fully covered vs total. Use `GetReadinessColour()` to colour the score.

**8 — Sort items**  
Submenu:
```
 1. Sort by name (A-Z)
 2. Sort by expiry date (soonest first)
 3. Sort by category (A-Z)
```

**9 — Generate report**  
Generate the report. Ask the user if they want to: (a) print to screen, (b) save to file, or (c) both.

**10 — Manage household profile**  
Display current profile. Allow editing the same way as editing an item. Note: changing adult/child count will affect required quantities and score.

**0 — Save and exit**  
Save both files. Display a Civil Defence reminder message. Exit.

---

## MSTest — Test Specifications

### `EmergencyItemTests.cs`

- `IsExpired_ReturnsFalse_WhenNonExpiring()`
- `IsExpired_ReturnsFalse_WhenExpiryDateInFuture()`
- `IsExpired_ReturnsTrue_WhenExpiryDateInPast()`
- `IsExpiringSoon_ReturnsFalse_WhenNonExpiring()`
- `IsExpiringSoon_ReturnsTrue_WhenWithinThreshold()`
- `IsExpiringSoon_ReturnsFalse_WhenOutsideThreshold()`
- `IsExpiringSoon_ReturnsTrue_WhenAlreadyExpired()`

### `HouseholdProfileTests.cs`

- `GetTotalPeople_ReturnsAdultsPlusChildren()`
- `GetTotalPeople_DoesNotIncludePets()`
- `GetRequiredQuantity_ReturnsCorrectAmount()`
- `GetRequiredQuantity_ScalesWithHouseholdSize()`

### `ReadinessCalculatorTests.cs`

- `CalculateScore_ReturnsZero_WhenNoItems()`
- `CalculateScore_Returns100_WhenAllItemsFullyCovered()`
- `CalculateScore_Returns0_WhenNoItemsOwned()`
- `CalculateScore_ReturnsCorrectPartialScore()`
- `CalculateScore_CountsExpiredItemsAsZero()`
- `GetReadinessLevel_ReturnsCorrectLevel_ForEachRange()`

### `InventoryManagerTests.cs`

- `AddItem_IncreasesCount()`
- `RemoveItem_DecreasesCount()`
- `RemoveItem_ReturnsFalse_WhenIndexOutOfRange()`
- `FindByName_ReturnsCorrectIndex()`
- `FindByName_ReturnsMinus1_WhenNotFound()`
- `FindByName_IsCaseInsensitive()`
- `FindAllByCategory_ReturnsCorrectIndices()`
- `SortByName_SortsAlphabetically()`
- `UpdateItem_ReplacesCorrectItem()`

### `FileHandlerTests.cs`

- `SaveAndLoadProfile_RoundTrip_PreservesAllFields()`
- `SaveAndLoadItems_RoundTrip_PreservesAllFields()`
- `LoadProfile_ReturnsNull_WhenFileDoesNotExist()`
- `LoadItems_ReturnsEmptyArray_WhenFileDoesNotExist()`

---

## Implementation Notes

**Things you have not done before that you will need to look up:**

- `DateTime.ParseExact()` and `DateTime.TryParseExact()` — for converting date strings
- `Console.ForegroundColor` and `Console.ResetColor()` — for coloured output
- `double.TryParse()` — for safe numeric input parsing
- `string.Contains()` with `StringComparison.OrdinalIgnoreCase` — for case-insensitive search
- `try/catch` blocks — for exception handling in FileHandler
- `Array.Copy()` — you may need this in InventoryManager when removing items

**Things to keep in mind:**

- `_count` is not the same as `Items.Length`. Never loop to `Items.Length` — always loop to `_count`
- The menu index shown to the user (starting at 1) is always one more than the array index (starting at 0). Off-by-one errors here are the most common bug in this project
- Save files every time a change is made, not just on exit — so a crash does not lose data. You can call save at the end of each menu action
- Test your `FileHandler` tests with temporary file paths, not your real data files

**Stretch goals (only if you finish everything else):**

- Colour-code the menu status line based on readiness score
- Add an `ImportFromCsv()` method to `FileHandler` that accepts a different CSV format for bulk importing items
- Track when items were last checked/verified and flag items not checked in over 6 months
- Add a "What to do NOW" emergency action guide that displays when score is below 50

---

## README Requirements

Your GitHub README for this project must include:

1. What the project is and why it exists (mention Dunedin, Alpine Fault, Civil Defence)
2. How to clone and run it
3. A screenshot of the main menu
4. A screenshot of the readiness score display
5. What you learned building it
6. A note acknowledging NZ Civil Defence / getready.govt.nz as the source of the item recommendations

---

*Specification version 1.0 — Project Kete — June 2026*
