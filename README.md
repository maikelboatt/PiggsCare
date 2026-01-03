# PiggsCare - Pig Farm Management System

A production-grade desktop livestock management system built with WPF for comprehensive pig farm operations. Features breeding cycle tracking, health management, synchronization protocols, and automated reminder systems.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-MVVM-0078D4?style=flat-square)](https://github.com/dotnet/wpf)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2025-CC2927?style=flat-square)](https://www.microsoft.com/sql-server)
[![Dapper](https://img.shields.io/badge/Dapper-Micro--ORM-orange?style=flat-square)](https://github.com/DapperLib/Dapper)

## Overview

PiggsCare manages complete pig farm operations including animal records, breeding cycles, health tracking, synchronization protocols, farrowing events, weaning schedules, and removal tracking. Built with agricultural precision using N-tier architecture, MVVM pattern, and immutable domain models.

**Key Highlights:**
- Clean N-tier architecture with clear separation of concerns
- Complete breeding lifecycle management (synchronization → insemination → pregnancy → farrowing → weaning)
- Automated reminder system for critical farm operations
- Health record tracking with diagnosis and treatment history
- Synchronization event management with protocol documentation
- Individual animal tracking with breed, certificate, and back fat index
- Real-time notifications for operation success/failure
- Expected farrowing date calculations

---

## Features

### Animal Management
- Individual animal records with unique identification numbers
- Breed tracking (Large White, Duroc, etc.)
- Birth date and certificate number documentation
- Gender classification
- Back fat index monitoring for breeding quality
- Search and filtering capabilities
- Comprehensive animal lifecycle tracking

### Breeding Cycle Management
- **Synchronization Events**: Estrus synchronization with protocol documentation
  - Start and end date tracking
  - Batch number assignment
  - Protocol specification (e.g., "Boar exposure")
  - Comments and notes
- **Insemination Events**: Artificial insemination (AI) tracking
  - AI date recording
  - Automatic expected farrowing date calculation
  - Linked to synchronization events
  - Individual sow tracking
- **Pregnancy Monitoring**: Pregnancy scan management
  - Scan date recording
  - Scan results documentation
  - Breeding event correlation
- **Farrowing Events**: Birth record tracking
  - Farrowing date documentation
  - Litter size recording
  - Born alive count
  - Born dead count
  - Mummified piglet tracking
- **Weaning Events**: Weaning process management
  - Weaning date tracking
  - Number of piglets weaned
  - Gender distribution (males/females)
  - Average weaning weight calculation

### Health Management
- Comprehensive health record system
- Diagnosis documentation
- Treatment tracking
- Outcome recording
- Date-based health history
- Individual animal health profiles

### Automated Reminder System
- **Synchronization Reminders**: Notifications for estrus synchronization schedules
- **AI Reminders**: Automated alerts when sows are ready for artificial insemination
- **Customizable Scheduling**: Optional reminder acceptance after synchronization
- **Multi-animal Alerts**: Batch notifications for multiple animals
- **Date-based Triggers**: Scheduled notifications based on breeding cycles

### Real-time Notifications
- Operation success confirmations
- Failure alerts with error details
- New animal creation notifications
- Synchronization event updates
- Insemination event confirmations
- Health record additions
- Removal event tracking

### Removal Management
- Removal event documentation
- Date tracking
- Reason for removal recording (culling, sale, death, etc.)
- Complete animal exit audit trail

---

## Architecture

### Tech Stack

- **.NET 9.0** - Latest framework features and performance improvements
- **WPF + MVVM** - Separation of UI and business logic
- **C# Classes** - Domain models with primary constructor syntax
- **Dapper** - High-performance micro-ORM
- **SQL Server 2025** - Enterprise-grade database
- **Async/Await** - Non-blocking operations throughout

### N-Tier Structure

```
┌─────────────────────────────────┐
│  Presentation (WPF + MVVM)      │  Views, ViewModels, Commands
├─────────────────────────────────┤
│  Application Services           │  Business Logic, Reminders, Notifications
├─────────────────────────────────┤
│  Domain Models                  │  Entities, Business Rules, Calculations
├─────────────────────────────────┤
│  Data Access (Dapper)           │  Repositories, Async Operations
├─────────────────────────────────┤
│  SQL Server 2025                │  Relational Database
└─────────────────────────────────┘
```

---

## Domain Models

### Animal
```csharp
public class Animal(
    int animalId,
    int name,
    string breed,
    DateOnly birthDate,
    int certificateNumber,
    string gender,
    float backFatIndex)
{
    public int AnimalId { get; init; } = animalId;
    public int Name { get; init; } = name;
    public string Breed { get; init; } = breed;
    public DateOnly BirthDate { get; init; } = birthDate;
    public int CertificateNumber { get; init; } = certificateNumber;
    public string Gender { get; init; } = gender;
    public float BackFatIndex { get; init; } = backFatIndex;
}
```

**Key Features:**
- Unique identification system
- Breed classification (Large White, Duroc, etc.)
- Certificate number for pedigree tracking
- Back fat index for breeding quality assessment
- Gender classification for herd management

### SynchronizationEvent
```csharp
public class SynchronizationEvent(
    int synchronizationEventId,
    DateOnly startDate,
    DateOnly endDate,
    string batchNumber,
    string synchronizationProtocol,
    string comments)
{
    public int SynchronizationEventId { get; init; } = synchronizationEventId;
    public DateOnly StartDate { get; init; } = startDate;
    public DateOnly EndDate { get; set; } = endDate;
    public string BatchNumber { get; set; } = batchNumber;
    public string SynchronizationProtocol { get; set; } = synchronizationProtocol;
    public string Comments { get; set; } = comments;
}
```

**Key Features:**
- Estrus synchronization scheduling
- Batch management for group synchronization
- Protocol documentation (boar exposure, hormone treatments, etc.)
- Start and end date tracking
- Comments for operational notes

### InseminationEvent
```csharp
public class InseminationEvent(
    int breedingEventId,
    int animalId,
    DateOnly aiDate,
    DateOnly expectedFarrowDate,
    int? synchronizationEventId)
{
    public int BreedingEventId { get; init; } = breedingEventId;
    public int AnimalId { get; init; } = animalId;
    public DateOnly AiDate { get; init; } = aiDate;
    public DateOnly ExpectedFarrowDate { get; init; } = expectedFarrowDate;
    public int? SynchronizationEventId { get; init; } = synchronizationEventId;
}
```

**Key Features:**
- AI date documentation
- Expected farrowing date calculation (~114 days gestation)
- Optional linkage to synchronization events
- Individual sow tracking

### InseminationEventWithAnimal
```csharp
public class InseminationEventWithAnimal(
    int breedingEventId,
    int animalId,
    DateOnly aiDate,
    DateOnly expectedFarrowDate,
    int? synchronizationEventId,
    string? name) : InseminationEvent(
        breedingEventId, animalId, aiDate, 
        expectedFarrowDate, synchronizationEventId)
{
    public string? Name { get; init; } = name;
}
```

**Key Features:**
- Extends InseminationEvent with animal name
- Optimized for display and reporting
- Eliminates need for additional joins

### PregnancyScan
```csharp
public class PregnancyScan(
    int scanId,
    int breedingEventId,
    DateOnly scanDate,
    string scanResults)
{
    public int ScanId { get; init; } = scanId;
    public int BreedingEventId { get; init; } = breedingEventId;
    public DateOnly ScanDate { get; set; } = scanDate;
    public string ScanResults { get; set; } = scanResults;
}
```

**Key Features:**
- Pregnancy verification tracking
- Scan date documentation
- Results recording (positive, negative, uncertain)
- Breeding event correlation

### FarrowEvent
```csharp
public class FarrowEvent(
    int farrowingEventId,
    int breedingEventId,
    DateOnly farrowDate,
    int litterSize,
    int bornAlive,
    int bornDead,
    int mummified)
{
    public int FarrowingEventId { get; init; } = farrowingEventId;
    public int BreedingEventId { get; init; } = breedingEventId;
    public DateOnly FarrowDate { get; set; } = farrowDate;
    public int LitterSize { get; set; } = litterSize;
    public int BornAlive { get; set; } = bornAlive;
    public int BornDead { get; set; } = bornDead;
    public int Mummified { get; set; } = mummified;
}
```

**Key Features:**
- Complete farrowing outcome documentation
- Litter size tracking
- Viability statistics (alive/dead/mummified)
- Breeding event linkage for complete lifecycle tracking

### WeaningEvent
```csharp
public class WeaningEvent(
    int weaningEventId,
    int farrowingEventId,
    DateOnly weaningDate,
    int numberWeaned,
    int malesWeaned,
    int femalesWeaned,
    float averageWeaningWeight)
{
    public int WeaningEventId { get; init; } = weaningEventId;
    public int FarrowingEventId { get; init; } = farrowingEventId;
    public DateOnly WeaningDate { get; set; } = weaningDate;
    public int NumberWeaned { get; set; } = numberWeaned;
    public int MalesWeaned { get; set; } = malesWeaned;
    public int FemalesWeaned { get; set; } = femalesWeaned;
    public float AverageWeaningWeight { get; set; } = averageWeaningWeight;
}
```

**Key Features:**
- Weaning date tracking
- Gender distribution statistics
- Average weight calculation for performance assessment
- Farrowing event linkage

### HealthRecord
```csharp
public class HealthRecord(
    int healthRecordId,
    int animalId,
    DateOnly recordDate,
    string diagnosis,
    string treatment,
    string outcome)
{
    public int HealthRecordId { get; init; } = healthRecordId;
    public int AnimalId { get; init; } = animalId;
    public DateOnly RecordDate { get; init; } = recordDate;
    public string Diagnosis { get; init; } = diagnosis;
    public string Treatment { get; init; } = treatment;
    public string Outcome { get; init; } = outcome;
}
```

**Key Features:**
- Complete medical history per animal
- Diagnosis documentation
- Treatment tracking
- Outcome recording for effectiveness analysis

### RemovalEvent
```csharp
public class RemovalEvent(
    int removalEventId,
    int animalId,
    DateOnly removalDate,
    string reasonForRemoval)
{
    public int RemovalEventId { get; init; } = removalEventId;
    public int AnimalId { get; init; } = animalId;
    public DateOnly RemovalDate { get; init; } = removalDate;
    public string ReasonForRemoval { get; init; } = reasonForRemoval;
}
```

**Key Features:**
- Removal date documentation
- Reason tracking (culling, sale, death, etc.)
- Complete exit audit trail

### ScheduledNotification
```csharp
public class ScheduledNotification(
    int notificationId,
    string message,
    DateOnly scheduledDate,
    List<int> animalIds,
    int synchronizationId)
{
    public int Id { get; set; } = notificationId;
    public string Message { get; set; } = message;
    public DateOnly ScheduledDate { get; set; } = scheduledDate;
    public List<int> AnimalIds { get; set; } = animalIds;
    public int SynchronizationId { get; set; } = synchronizationId;
}
```

**Key Features:**
- Multi-animal reminder support
- Scheduled date-based alerts
- Synchronization event linkage
- Customizable reminder messages

---

## Database Schema

### Key Tables

**Animals** - Core livestock information with breed, certificate, and back fat tracking
**SynchronizationEvents** - Estrus synchronization scheduling and protocol documentation
**InseminationEvents** - AI tracking with expected farrowing dates
**PregnancyScans** - Pregnancy verification results
**FarrowingEvents** - Birth records with detailed litter statistics
**WeaningEvents** - Weaning outcomes with weight and gender data
**HealthRecords** - Medical history with diagnosis and treatment
**RemovalEvents** - Animal exit tracking
**ScheduledNotifications** - Automated reminder system

### Key Relationships

```sql
-- Breeding lifecycle chain
SynchronizationEvents → InseminationEvents → PregnancyScans → FarrowingEvents → WeaningEvents

-- Animal health tracking
Animals → HealthRecords (One-to-Many)

-- Animal removal tracking
Animals → RemovalEvents (One-to-One)

-- Reminder system
SynchronizationEvents → ScheduledNotifications (One-to-Many)
InseminationEvents → Animals (Many-to-One)
```

### Key Constraints

```sql
-- Animal validation
CONSTRAINT CHK_Animals_BackFatIndex CHECK (BackFatIndex >= 0)
CONSTRAINT CHK_Animals_Gender CHECK (Gender IN ('Male', 'Female'))

-- Farrowing validation
CONSTRAINT CHK_FarrowingEvents_Statistics 
    CHECK (LitterSize = BornAlive + BornDead + Mummified)
CONSTRAINT CHK_FarrowingEvents_Counts 
    CHECK (BornAlive >= 0 AND BornDead >= 0 AND Mummified >= 0)

-- Weaning validation
CONSTRAINT CHK_WeaningEvents_Gender 
    CHECK (NumberWeaned = MalesWeaned + FemalesWeaned)
CONSTRAINT CHK_WeaningEvents_Weight CHECK (AverageWeaningWeight > 0)

-- Date validation
CONSTRAINT CHK_InseminationEvents_ExpectedFarrowDate 
    CHECK (ExpectedFarrowDate > AiDate)
CONSTRAINT CHK_WeaningEvents_WeaningDate 
    CHECK (WeaningDate > (SELECT FarrowDate FROM FarrowingEvents 
                          WHERE FarrowingEventId = FarrowingEvents.FarrowingEventId))
```

---

## Performance Features

### Async Operations
All database operations are asynchronous for responsive UI:
```csharp
await animalRepository.GetByIdAsync(animalId);
await inseminationService.RecordAIAsync(inseminationEvent);
await notificationService.ScheduleReminderAsync(notification);
await farrowingService.RecordBirthAsync(farrowEvent);
```

### Optimized Workflows
- Cached animal and breed reference data
- Batch synchronization event processing
- Efficient reminder scheduling system
- Parameterized queries for SQL plan reuse

### Reminder System Architecture
- Background service for scheduled notification checks
- Date-based trigger evaluation
- Multi-animal batch notifications
- Automatic reminder dismissal after action completion

---

## Getting Started

### Prerequisites
- Windows 10/11
- .NET 9.0 SDK
- SQL Server 2019+ (2025 recommended)

### Installation

1. **Clone repository**
   ```bash
   git clone https://github.com/maikelboatt/piggscare.git
   cd piggscare
   ```

2. **Setup database**
   ```bash
   sqlcmd -S your_server -i PiggsCare.Database/Schema/CreateDatabase.sql
   sqlcmd -S your_server -d PiggsCareDB -i PiggsCare.Database/Schema/CreateTables.sql
   ```

3. **Configure connection**
   
   Update `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "PiggsCareDatabase": "Server=localhost;Database=PiggsCareDB;Integrated Security=true;TrustServerCertificate=true"
     }
   }
   ```

4. **Run**
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project PiggsCare.UI
   ```

Default credentials: `admin` / `admin123` (change immediately)

---

## Project Structure

```
PiggsCare/
├── PiggsCare.UI/                 # WPF Views & ViewModels
│   ├── Views/                    # Animal, Breeding, Health Views
│   ├── Components/               # Reusable UI Components
│   ├── Forms/                    # Input Forms & Dialogs
│   └── Resources/                # Styles, Themes, Icons
├── PiggsCare.Application/        # Business Services & Validation
│   ├── Services/                 # Breeding, Health, Reminder Services
│   └── Notifications/            # Notification System
├── PiggsCare.Domain/             # Domain Models
│   └── Models/                   # All domain entities
├── PiggsCare.DataAccess/         # Dapper Repositories
├── PiggsCare.Infrastructure/     # Logging, Config, Background Services
└── PiggsCare.Database/           # SQL Scripts & Migrations
```

---

## Key Design Patterns

**MVVM** - Complete UI/logic separation  
**Repository Pattern** - Abstracted data access  
**Primary Constructors** - Concise model initialization  
**Immutable Properties** - Thread-safe init-only setters  
**Inheritance** - InseminationEventWithAnimal extends InseminationEvent  
**Background Services** - Reminder scheduling system  
**Event Aggregation** - Real-time notification broadcasting  
**Async/Await** - Non-blocking I/O throughout

---

## Example Usage

### Record Animal
```csharp
var animal = new Animal(
    animalId: 0,
    name: 101,
    breed: "Large White",
    birthDate: DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)),
    certificateNumber: 14,
    gender: "Female",
    backFatIndex: 4.6f
);

await animalService.RegisterAnimalAsync(animal);
// Triggers notification: "Animal 101 successfully registered"
```

### Create Synchronization Event with Reminder
```csharp
var syncEvent = new SynchronizationEvent(
    synchronizationEventId: 0,
    startDate: DateOnly.FromDateTime(DateTime.Now),
    endDate: DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
    batchNumber: "Batch-405",
    synchronizationProtocol: "Boar exposure",
    comments: "Successful"
);

await syncService.CreateSynchronizationAsync(syncEvent);

// User prompt: "Do you want to be reminded for AI?"
// If accepted, schedules reminder for AI date
var reminder = new ScheduledNotification(
    notificationId: 0,
    message: "Animals ready for insemination in Batch-405",
    scheduledDate: DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
    animalIds: new List<int> { 101, 102, 108 },
    synchronizationId: syncEvent.SynchronizationEventId
);

await notificationService.ScheduleReminderAsync(reminder);
```

### Record Insemination
```csharp
var insemination = new InseminationEvent(
    breedingEventId: 0,
    animalId: 101,
    aiDate: DateOnly.FromDateTime(DateTime.Now),
    expectedFarrowDate: DateOnly.FromDateTime(DateTime.Now.AddDays(114)), // Gestation period
    synchronizationEventId: 1
);

await breedingService.RecordInseminationAsync(insemination);
// Triggers notification: "Insemination recorded. Expected farrowing: [date]"
```

### Track Farrowing
```csharp
var farrowing = new FarrowEvent(
    farrowingEventId: 0,
    breedingEventId: 1,
    farrowDate: DateOnly.FromDateTime(DateTime.Now),
    litterSize: 12,
    bornAlive: 10,
    bornDead: 1,
    mummified: 1
);

await farrowingService.RecordFarrowingAsync(farrowing);
// Automatic validation: LitterSize = BornAlive + BornDead + Mummified
// Triggers notification: "Farrowing recorded for animal 101: 10 alive, 1 dead, 1 mummified"
```

### Record Weaning
```csharp
var weaning = new WeaningEvent(
    weaningEventId: 0,
    farrowingEventId: 1,
    weaningDate: DateOnly.FromDateTime(DateTime.Now.AddDays(21)),
    numberWeaned: 9,
    malesWeaned: 5,
    femalesWeaned: 4,
    averageWeaningWeight: 5.8f
);

await weaningService.RecordWeaningAsync(weaning);
// Triggers notification: "Weaning completed: 9 piglets (5M, 4F) avg weight 5.8kg"
```

### Add Health Record
```csharp
var healthRecord = new HealthRecord(
    healthRecordId: 0,
    animalId: 101,
    recordDate: DateOnly.FromDateTime(DateTime.Now),
    diagnosis: "Respiratory infection",
    treatment: "Antibiotics - 5 day course",
    outcome: "Recovered"
);

await healthService.AddHealthRecordAsync(healthRecord);
// Triggers notification: "Health record added for animal 101"
```

---

## Reminder & Notification System

### Automated Reminders
1. **Synchronization to AI**: After creating a synchronization event, users can opt-in for AI reminders
2. **Scheduled Triggers**: Background service checks scheduled notifications daily
3. **Batch Notifications**: Single reminder can alert for multiple animals
4. **Dismissal**: Reminders automatically dismiss after associated action is completed

### Real-time Notifications
- **Success Confirmations**: Green notifications for successful operations
- **Error Alerts**: Red notifications with error details
- **Information Updates**: Blue notifications for status changes
- **Persistent Log**: All notifications stored for audit trail

### Notification Types
```csharp
// Operation success
"Animal 102 successfully registered"
"Synchronization event Batch-405 created"
"Insemination recorded for animal 101"

// Scheduled reminders
"Reminder: 3 animals ready for AI in Batch-405"
"Upcoming farrowing: Animal 101 expected to farrow in 3 days"

// Operation failures
"Failed to register animal: Invalid certificate number"
"Synchronization creation failed: Batch number already exists"
```

---

## Analytics Features

### Farm Performance Metrics
- Average litter size per sow
- Born alive percentage
- Weaning success rate
- Average weaning weight trends
- Farrowing interval analysis
- Breeding efficiency statistics

### Health Tracking
- Disease incidence rates
- Treatment effectiveness
- Recovery time analysis
- Individual animal health history

### Breeding Performance
- Conception rates per synchronization batch
- Pregnancy confirmation rates
- Farrowing success rates
- Piglet survival rates

---

## Security

- **Role-based access control** - Farm Manager, Livestock Officer, Veterinarian roles
- **Password hashing** - Secure credential storage
- **Audit logging** - Complete operational history
- **Parameterized queries** - SQL injection protection
- **Data backup** - Scheduled database backups
- **User activity tracking** - Who did what and when

---

## Testing

```bash
dotnet test                                    # All tests
dotnet test --filter Category=Unit             # Unit tests only
dotnet test --filter Category=Integration      # Integration tests
dotnet test /p:CollectCoverage=true            # With coverage
```

**Coverage Areas:**
- Domain model validation and equality
- Breeding lifecycle workflow
- Reminder scheduling logic
- Notification delivery system
- Repository async operations
- Business rule enforcement (farrowing statistics, weaning validation)

---

## Roadmap

- [ ] Mobile app for field data entry
- [ ] Feed consumption tracking
- [ ] Growth rate monitoring with weight tracking
- [ ] Financial module (feed costs, revenue from sales)
- [ ] Genetic lineage tracking and pedigree visualization
- [ ] Automated report generation (monthly, quarterly, annual)
- [ ] Integration with electronic identification (RFID tags)
- [ ] Export capabilities (CSV, Excel, PDF)
- [ ] Multi-farm management support
- [ ] Weather correlation analysis for breeding success
- [ ] Predictive analytics for optimal breeding times
- [ ] Vaccination schedule management

---

## Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/NewFeature`)
3. Follow domain model patterns (primary constructors, init properties)
4. Add comprehensive tests for breeding workflows
5. Commit changes (`git commit -m 'Add NewFeature'`)
6. Push to branch (`git push origin feature/NewFeature`)
7. Open Pull Request

---

## License

MIT License - see [LICENSE](LICENSE)

---

## Author

**Michael Boateng**  
GitHub: [@maikelboatt](https://github.com/maikelboatt)  
Email: boattmaikel@gmail.com

---

Built with WPF, .NET 9, Dapper, and SQL Server 2025

⭐ Star this repo if you find it useful!

---

## Acknowledgments

Special thanks to pig farm managers and livestock officers who provided invaluable feedback on breeding workflows, synchronization protocols, and operational requirements. This system was built to improve farm efficiency, animal welfare, and data-driven decision making in modern swine production.
