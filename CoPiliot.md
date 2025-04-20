# CFP Compass

**CFP Compass** is a call-for-speakers aggregator that helps community speakers discover open CFPs (Call For Papers). Its purpose is to ease the process of finding speaking opportunities by compiling and maintaining an up-to-date listing of open calls for speakers.

Initially, the project leverages .NET, C#, and Azure SQL to generate static markdown files in a GitHub repository. A nightly job filters out expired CFPs, keeping the listings current. In the future, CFP Compass will evolve into a dynamic solution hosted on Azure Static Web Apps with advanced search capabilities powered by Azure AI Search. This project is also the first step toward integrating with Hermes, a comprehensive speaking engagement management system.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture Overview](#architecture-overview)
- [Getting Started](#getting-started)
- [Future Roadmap](#future-roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **Aggregated CFP Listings:** Compiles open call-for-speaker opportunities for community speakers.
- **Static Markdown Generation:** Generates markdown files automatically from the backend data.
- **Nightly Updates:** Periodic jobs remove expired CFPs and update listings.
- **Scalable Architecture:** Built with .NET and Azure services with room for future expansion.
- **Integration with Hermes:** Forms the foundational piece for the more extensive Hermes speaking engagement management system.

## Tech Stack

- **Backend:** .NET, C#
- **Database:** Azure SQL
- **Deployment (Initial):** Static markdown files in GitHub
- **Future Enhancements:** Azure Static Web Apps, Azure Functions, Azure AI Search for intelligent filtering

## Architecture Overview

The current system is composed of the following components:

- **Azure SQL Database:** Stores detailed information about each CFP, including dates, locations, and benefits.
- **Backend Service:** Developed in NET/C# to query the database and generate markdown files based on open CFP listings.
- **GitHub Repository:** Serves as the initial hosting platform for the generated static content.
- **Nightly Update Job:** Automates refreshing the CFP listings by filtering out expired calls.

This setup lays the groundwork for a seamless transition to a fully dynamic web application with enriched search and user experience.