# Reminder Service

## About the project

This project is a Simple web API developed in .NET core Web API. This project works like a CRON job in which we have a service which sends the reminders or messages to users based on the time stored as per the database using Twilio SMS gateway.

I have used an Open source package "Hangfire" for scheduling the background jobs for sending the reminders.

It contains a service which executes every after 24 hours and schedules the jobs for next 24hours for sending the reminders.

## Installation of Database

I have used Entity framework core for Database operations. Its based on code first approach in which I have created the migrations. You need minimal configuration for setting up the Database on your machine.

You need to create the Database in MS Sql server and note down the Database connection string.

We have a configuration file `appsetting.json` file where you need to set up your Database connection string.

```bash
  "ConnectionStrings": {
    "DefaultConnection": "ConnectionString>"
  },
```

Once Connection string is done, In Visual Studio, open Package manager console, run the below command to create the tables.

```bash
 update-database
```

## Screenshots

![App Screenshot](https://res.cloudinary.com/codemites/image/upload/v1665302237/sample.jpg)

## APIs

When you run the project, all the APIs will be listed down on swagger.

- api/v1/system-register

  This API is for registering a user inside the database

- api/Reminder/save-Reminder

  This API is for saving the reminder in the database.

- api/Reminder/schedule-reminder

  This API is for activating the service which runs 24 hourly to schedule the jobs of sending the reminders

## Tech Stack

.NET Core 3.0 Web API

**Database:** MS Sql Server
