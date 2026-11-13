# Cybersecurity Awareness Bot

## Project Overview
The Cybersecurity Awareness Bot is a C# WPF desktop application designed to educate users about cybersecurity and help them develop safer online habits.

## Features

### 1. Chatbot
- Answers cybersecurity questions.
- Provides information about passwords, phishing, scams, malware, privacy and Wi-Fi security.
- Recognises different user messages.
- Includes sentiment detection and basic conversation memory.

### 2. Security Task Assistant
- Add cybersecurity tasks.
- Set reminder dates.
- Mark tasks as completed.
- Delete tasks.
- Track task progress.

### 3. MySQL Database
- Stores tasks in a MySQL database.
- Loads saved tasks when the application opens.
- Updates and deletes database records.

### 4. Cybersecurity Quiz
- Contains 10 cybersecurity questions.
- Supports multiple-choice and true/false questions.
- Provides immediate feedback and explanations.
- Displays the final score.

### 5. Activity Log
- Records important user activities.
- Displays the 10 most recent activities with timestamps.

### 6. Natural Language Recognition
- Recognises commands to open the Task Assistant, Quiz and Activity Log.

## Technologies Used
- C#
- WPF and XAML
- .NET
- MySQL
- MySql.Data
- Visual Studio

## Installation Requirements
- Visual Studio with the .NET desktop development workload.
- MySQL Server.
- MySQL Workbench.
- MySql.Data NuGet package.

## Database Setup

Create the database and tables using MySQL Workbench:

```sql
CREATE DATABASE IF NOT EXISTS CybersecurityDB;
USE CybersecurityDB;

CREATE TABLE IF NOT EXISTS Tasks
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(150) NOT NULL,
    Description TEXT,
    ReminderDate DATETIME NULL,
    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS ActivityLogs
(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(255) NOT NULL,
    ActivityTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

## How to Run
1. Open the project solution in Visual Studio.
2. Install the required NuGet packages.
3. Configure your local MySQL connection.
4. Ensure the MySQL Server service is running.
5. Build the solution.
6. Run the application.

**Security note:** Do not upload your database password or private connection details to GitHub. Keep them in local configuration excluded from version control.

## Author
Jephte Mukonkole

## Project Purpose
This project demonstrates the development of a cybersecurity education application that combines chatbot interaction, task management, database integration, quizzes and activity tracking.