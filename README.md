
## UserNova - User Management Web App

UserNova is a full-stack web application built with ASP.NET Core MVC and SQL Server. It provides a complete, secure, and professional solution for user registration, authentication, and management. This project was developed as a comprehensive solution for Task #4 of the iTransition traineeship program, demonstrating a strong focus on data integrity, application security, and responsive UI/UX design.

The application features a polished, branded, and fully responsive user interface that works seamlessly on both desktop and mobile devices.

## Live Demo


## Features

**Secure Authentication:** A custom-built, secure registration and login system using the BCrypt hashing algorithm for password protection.

**Robust Data Integrity:** Guarantees email uniqueness at the database level via a UNIQUE index, with graceful error handling in the UI.

**Full-Stack User Management:** Authenticated users can view, block, unblock, and delete all users in the system, including themselves.

**Advanced Security:**

A per-request authorization filter immediately logs out and redirects any user who has been blocked or deleted.

Anti-Forgery Tokens are used on all POST actions to prevent CSRF attacks.

Login errors are generic to prevent username enumeration.

**Professional & Responsive UI:**

A polished, two-panel, brand-centric design for the authentication pages.

The user management dashboard features a table that intelligently transforms into a "card" layout on mobile devices, eliminating horizontal scrolling.

**Modern UX:**

Toolbar-based actions with multi-select checkboxes for efficient user management.

Informative tooltips for icon-based controls.

Auto-dismissing status messages that provide clear feedback on user actions.

## Technology Stack

**Backend:** C# / .NET 8 (ASP.NET Core MVC)

**Database:** SQL Server

**ORM:** Entity Framework Core 8

**Frontend:** HTML5, CSS3, JavaScript (ES6)

**UI Framework:** Bootstrap 5

**Security:**

BCrypt.Net-Next for secure password hashing.

ASP.NET Core Session Management & Anti-Forgery Tokens.

## Core Requirements Implemented

This project successfully implements all key requirements of the task, including:

**1. Database-Level _UNIQUE_ Index:** The Users table has a unique index on the Email column, enforced by the database.

**2. Professional UI:** The application features a clean, business-oriented design with a clear distinction between the toolbar and the data table, adhering to the provided visual guidelines.

**3. Data Sorting:** The user table is sorted by the last login time by default.

**4. Checkbox Multi-Select:** A "Select All" checkbox and individual row checkboxes are implemented for all toolbar actions.

**5. Per-Request Authorization:** A custom action filter validates the user's status on every authenticated request, ensuring immediate redirection if they are blocked or deleted.

## Getting Started
**Prerequisites**

.NET 8 SDK

SQL Server (SQL Server Express or the Developer Edition is recommended for local development).

**Installation & Setup**

**1. Clone the Repository:**

git clone (https://github.com/Rizve1503/UserManagement.WebApp)
cd UserManagement


**2. Configure the Database Connection:**

Open the appsettings.Development.json file.

Find the ConnectionStrings section.

Update the DefaultConnection value to point to your local SQL Server instance. A typical LocalDB string is:
"Server=(localdb)\\mssqllocaldb;Database=UserNovaDB;Trusted_Connection=True;MultipleActiveResultSets=true"

**3. Apply Database Migrations:**

Open a terminal in the UserManagement.WebApp project directory.

Run the following command to create the database and apply the schema, including the UNIQUE index:

dotnet ef database update

**4. Run the Application:**

Run the project from Visual Studio or use the .NET CLI:

dotnet run

The application will be available at https://localhost:XXXX.
