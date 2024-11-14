Contract Monthly Claim System

Description

The Contract Monthly Claim System is an application designed to manage and automate the claim submission and approval process for contract workers, specifically for lecturers, coordinators, and managers. It allows users to submit claims, manage their submissions, approve or reject claims, and track the overall status of claims in the system. The system is built using C# ASP.NET MVC/Core, jQuery, Entity Framework, and SQL Server.

Features

Lecturer Functionality:

Submit claims for services provided.

View claim status and details.

Submit grades for assigned tasks.


Coordinator Functionality:

Review claims submitted by lecturers.

Handle lecturer assignments and tasks.

Submit claims for review and approval.


Manager Functionality:

Approve or reject claims submitted by lecturers and coordinators.

Monitor claim statuses and ensure timely processing.


General Features:

User authentication with role-based access control (Lecturer, Coordinator, Manager).

A user-friendly interface with a dynamic dashboard for managing claims.

Integration with a SQL Server database for data storage.



Technologies Used

Frontend:

HTML, CSS, JavaScript (jQuery)


Backend:

C# ASP.NET MVC/Core

Entity Framework


Database:

SQL Server (LecturerClaimsDB)


Version Control:

Git, GitHub



Setup Instructions

Prerequisites

Install Visual Studio (Community/Professional/Enterprise Edition) with the ASP.NET and web development workload.

Install SQL Server (or use a cloud-based database like Azure SQL).

Install Git for version control.

Make sure you have a GitHub account to push the repository.


Set up the Database

1. Open SQL Server Management Studio (SSMS) and connect to your local server or cloud SQL Server instance.


2. Create a new database named LecturerClaimsDB.


3. Run the SQL scripts from the Database folder to set up the necessary tables and schema.


4. Update the connection string in your project to match your database settings.



Build and Run the Project

1. Open the solution in Visual Studio.


2. Build the solution by clicking Build > Build Solution from the menu.


3. Run the application by pressing F5 or clicking the Start button in Visual Studio.


4. The application will open in your default web browser.


Usage

1. Log In:

Use the provided credentials for Lecturers, Coordinators, or Managers to access the system.

Roles are assigned based on the database user.



2. Submit Claims:

Lecturers and Coordinators can submit claims for payment by filling out the claim form.

Claims will be reviewed by Coordinators and Managers.



3. Manage Claims:

Users with the Coordinator role can review, update, or assign tasks to Lecturers.

Managers can approve or reject claims.



4. View Submitted Claims:

All users can view their claim history and current status.




Contribution

We welcome contributions to the Contract Monthly Claim System! To contribute:

1. Fork the repository.


2. Create a new branch.


3. Make your changes and commit them.

4. Push your changes to your forked repository .


5. Create a pull request from your forked repository.



License

This project is licensed under the MIT License - see the LICENSE file for details.



