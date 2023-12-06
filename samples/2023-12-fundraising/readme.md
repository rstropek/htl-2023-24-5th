# Fundraising

## Introduction

Traditionally, non-profit organizations like *freiwillige Feuerwehr* raise money by going from door to door asking for donations. Obviously, the fundraising teams try to avoid visiting a single household multiple times.

You do volunteer work in such a non-profit organization and you want to build a simple web api with which the fundraising teams can coordinate their visits.

## Functional Requirements

### Campaigns

*As a fundraising team manager, I want to maintain a list of fundraising campaigns (e.g. _Weihnachtssammlung 2021_).*

* Create a campaign
* Delete a campaign (only possible if no related data is in the DB for the given campaign)
* Update the name of a campaign

### Check Household

*As a fundraiser, I want to check whether a team already visited a certain household and met someone there.*

Input:

* (Mandatory) Campaign
* (Optional) Part of town name
  * Example: *Leo* when looking for *Leonding*
* (Optional) Part of street name
  * Example: *Limes* when looking for *Limesstraße*
* Flag whether all visits should be shown or only unsuccessful ones (i.e. visits where nobody was at home).

Output:

* List of all visits fitting the input filter criteria (see above).
* Each result record has to contain the following properties:
  * Town name
  * Street name
  * House number
  * Family name
  * Flag whether someone was met or nobody was at home

### Enter Visits

*As a fundraiser, I want to enter a visit.*

Input:

* (Mandatory) Campaign
* (Mandatory) Town name (max. length 150 chars)
* (Mandatory Street name (max. length 150 chars)
* (Mandatory) House number
  * Starts with a number >= 1
  * Can optionally contain a single letter after the number
* (Optional) Family name (max. length 150 chars)
* Flag whether someone was met or nobody was at home

### Enter Successful Re-visit

*As a fundraiser, I want to be able to mark a household that was successfully re-visited.*

A fundraiser will activate this API if she re-visited the household and met somebody there. Note that this API can only be called for households that were already visited before and where nobody was at home.

## Non-Functional Requirements

* Derive the data model from the functional requirements.
* Design the API based on the functional requirements. Use the principles of RESTful APIs.
* Use .NET 8 to implement the backend web API.
* Use Entity Framework Core 8 to implement the database access.
* Use SQL Server (LocalDB or Docker Container) as your database.

## Out-of-Scope

* Authentication and authorization
