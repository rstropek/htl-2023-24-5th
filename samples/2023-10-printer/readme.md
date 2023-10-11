# Printer Management

## Introduction

3D printing becomes more and more popular. In our school, we have multiple 3D printers that students can use. However, a 3D print can easily take hours to complete. Therefore, it is important to have a system that can manage the printers and the print jobs. Your job is to create a system through which students can reserve a printer.

## Backend

You can find a prototype of the DB backend in the folder [_server_](./server). Note that this is _not_ production code. This API is kept simple on purpose.

You find documented sample API requests in [_requests.http_](./server/requests.http).

Here is how you can run the backend locally (execute commands inside the _server_ folder):

```bash
npm install
npx tsc
node ./dist/app.js
```

## Requirements

* Implement a route with which students can search through existing reservations. They can enter a filter string (see also [_requests.http_](./server/requests.http)).
  * Use `FormControl` and an RxJS pipeline to implement the search.
* Display the list of reservations using a CSS grid.
* Implement a route with which students can create a new reservation.
  * Use Angular's reactive forms to implement the form for creating a new reservation.
  * Handle errors that happen when storing a reservation gracefully (e.g. unique constraint violation).
  * Printer and student must be selectable in a dropdown. The list of printers and students must be loaded from the backend.
  * Try to use a date picker control for the date (`<input type="datetime-local" ...>`).
