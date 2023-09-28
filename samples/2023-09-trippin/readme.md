# TripPin Exercise

![Hero image](hero.png)

## Background

TripPin is a sample REST service provided by Microsoft (see [https://www.odata.org/odata-services/](https://www.odata.org/odata-services/)). We are going to use it for this Angular sample. TripPin maintains a list of people and their travels (=trips).

Note that the service follows the *OData* standard. However, in this exercise, we do not care for the specifics of OData. We use TripPin like any other REST API that we have used so far.

## Functional Requirements

1. Person list
    1. Create a route for finding a person (see sample request *Search for people* in [requests.http](./requests.http)).
    2. Display the people in a list. Use a CSS grid to format the list.
    3. The list must contain a link with which the user can navigate to the trips of the selected person (see sample request *Search for trips of a single user* in [requests.http](./requests.http)).
2. Trips
    1. Display the trips of the selected person in a list. Use a CSS grid to format the list.
    2. Offer the possibility (separate route or form on the same page as the list of trips) to add a trip (see sample request *Create a trip for a user* in [requests.http](./requests.http)). Format the form with a CSS grid.

## Non-Functional Requirements

1. Use the latest version of Angular.
2. Use Angular Signals.
3. Use Standalone Components.
4. Do all data access and business logic in services, **not** in components.
5. If you cannot remember how to access web APIs, rewatch the videos from last year, read the [Angular documentation](https://angular.io/docs), use a search engine, or ask your favorite AI.
6. The application does not need to be particularly pretty. However, it must be functional, clean, and usable.
