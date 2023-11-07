# Bus Plan

## Introduction

In this exercise, you have to implement simplified timetable and price management for a public bus network.

## Server API

The server API has already been created. You can find it in [_server_](./server). Here is how you can start it locally:

```bash
cd server
npm install
npm start
```

Note that the bus plan is stored in [_plan.json_](./server/plan.json). Time values are stored as floating point values (e.g. 6am = 6, 10:30am = 10.5, 6:15pm = 18.25).

Also note that the `travelTime` is the number of minutes from the first station of the bus line (i.e. it takes line 180 18 minutes to get from Main Square to Shopping Mall).

## Minimal Requirements

### Get Personalized Bus Plan

Create a route in your Angular application where a customer can

* enter a bus line,
* enter station name,
* enter a starting hour (0-23), and
* enter a starting minute (0-59).

When the users clicks on a _Get Timetable_ button, your application has to call the `GET {{host}}/api/bus/:busLine` endpoint (see also [_requests.http_](./server/requests.http)). The response of this endpoint is a list of all stops of the given bus line starting with the given station. The `departure` value is a floating point value (e.g. 6am = 6, 10:30am = 10.5, 6:15pm = 18.25). Display the response in a nicely formatted table.

### Get Price

Create a route in your Angular application where a customer can fill out a form with the following data:

* Enter a bus line (e.g. 180).
* Enter a starting station (e.g. Main Square).
* Enter an ending station (e.g. Shopping Mall).
* _Optionally_ enter a rebate code (_student_ or _senior_).

Call the `POST {{host}}/api/price` endpoint (see also [_requests.http_](./server/requests.http)) with the entered data. The response of this endpoint is the ticket type (_mini_, _midi_, or _maxi_) and the price. Display the response in a nicely formatted table.

### Non-Functional Requirements

* Use latest version of Angular
* Use standalone components
* Use a CSS Grid
* Use _Reactive Forms_ (e.g. `FormGroup`, `FormControl`)
* You do not need to handle API errors (e.g. invalid bus line).
* Interaction with server API should be done in a separate Angular service.

## Enhanced Requirements

### Get Personalized Bus Plan

* Make the bus lines a dropdown where the user can select a line from a list of all lines. You get a list of lines using `GET {{host}}/api/lines` (see also [_requests.http_](./server/requests.http)).
* Make the station name a dropdown where the user can select a station from a list of all stations. You get a list of stations using `GET {{host}}/api/stops` (see also [_requests.http_](./server/requests.http)).
* Make the starting hour a dropdown (0-23).
* Make the starting minute a dropdown (0-59, 5 minute increments).
* Do not hard-code hours and minutes in HTML. Instead, calculate the values in your TypeScript code and use a `Signal`.

### Get Price

* Make the lines a dropdown where the user can select a line from a list of all lines. You get a list of lines using `GET {{host}}/api/lines` (see also [_requests.http_](./server/requests.http)).
* Make the station names dropdowns where the user can select a station from a list of all stations. You get a list of stations using `GET {{host}}/api/stops` (see also [_requests.http_](./server/requests.http)).
* Make the rebate code a dropdown (empty, _student_, _senior_).
* Offer a hyperlink from the personalized bus plan to the price page. If the user clicks on the hyperlink, the bus line and starting station are pre-filled in the form.

### Non-Functional Requirements

* Make the base URL of the server API easily configurable (dependency injection).
