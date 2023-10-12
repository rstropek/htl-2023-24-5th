import express, { Request, Response } from "express";
import bodyParser from "body-parser";
import sqlite3 from "sqlite3";
import { StatusCodes } from "http-status-codes";

// Initialize express and SQLite database
const app = express();
const db = new sqlite3.Database("./data.db");

app.use(bodyParser.json());

// Setup database tables
db.serialize(() => {
  db.run("PRAGMA foreign_keys = ON;");
  db.run(
    "CREATE TABLE IF NOT EXISTS printers (inventoryNumber TEXT PRIMARY KEY, model TEXT, year INTEGER)"
  );
  db.run(
    "CREATE TABLE IF NOT EXISTS students (studentID TEXT PRIMARY KEY, firstName TEXT, lastName TEXT)"
  );
  db.run(`CREATE TABLE IF NOT EXISTS reservations (
        reservationID INTEGER PRIMARY KEY AUTOINCREMENT, 
        studentID TEXT, 
        inventoryNumber TEXT, 
        fromDateTime TEXT, 
        durationHours INTEGER,
        FOREIGN KEY (studentID) REFERENCES students(studentID),
        FOREIGN KEY (inventoryNumber) REFERENCES printers(inventoryNumber),
        UNIQUE(studentID, inventoryNumber, fromDateTime)
    )`);
});

app.get("/printers", (req: Request, res: Response) => {
  db.all("SELECT * FROM printers", [], (err, rows) => {
    if (err) {
      return res
        .status(StatusCodes.INTERNAL_SERVER_ERROR)
        .json({ error: err.message });
    }
    return res.json(rows);
  });
});

// CRUD for Printers
app.post("/printers", (req: Request, res: Response) => {
  const { inventoryNumber, model, year } = req.body;
  db.run(
    "INSERT INTO printers (inventoryNumber, model, year) VALUES (?, ?, ?)",
    [inventoryNumber, model, year],
    function (err) {
      if (err) {
        return res
          .status(StatusCodes.INTERNAL_SERVER_ERROR)
          .json({ error: err.message });
      }
      return res.status(StatusCodes.CREATED).json({ id: this.lastID });
    }
  );
});

app.get("/students", (req: Request, res: Response) => {
  db.all("SELECT * FROM students", [], (err, rows) => {
    if (err) {
      return res
        .status(StatusCodes.INTERNAL_SERVER_ERROR)
        .json({ error: err.message });
    }
    return res.json(rows);
  });
});

// CRUD for Students
app.post("/students", (req: Request, res: Response) => {
  const { studentID, firstName, lastName } = req.body;
  db.run(
    "INSERT INTO students (studentID, firstName, lastName) VALUES (?, ?, ?)",
    [studentID, firstName, lastName],
    function (err) {
      if (err) {
        return res
          .status(StatusCodes.INTERNAL_SERVER_ERROR)
          .json({ error: err.message });
      }
      return res.status(StatusCodes.CREATED).json({ id: this.lastID });
    }
  );
});

// CRUD for Reservations
app.post("/reservations", (req: Request, res: Response) => {
  const { studentID, inventoryNumber, fromDateTime, durationHours } = req.body;

  // Validate fromDateTime
  if (isNaN(Date.parse(fromDateTime))) {
    return res
      .status(StatusCodes.BAD_REQUEST)
      .json({ error: "Invalid fromDateTime, must be an ISO 8601 string" });
  }

  db.run(
    "INSERT INTO reservations (studentID, inventoryNumber, fromDateTime, durationHours) VALUES (?, ?, ?, ?)",
    [studentID, inventoryNumber, fromDateTime, durationHours],
    function (err) {
      if (err) {
        return res
          .status(StatusCodes.INTERNAL_SERVER_ERROR)
          .json({ error: err.message });
      }
      return res.json({ id: this.lastID });
    }
  );
});

// List Reservations
app.get("/reservations", (req: Request, res: Response) => {
  const name: string = (req.query.name as string) || "";
  const past: boolean = req.query.past === "true";

  let query = `SELECT * FROM reservations
      JOIN printers ON reservations.inventoryNumber = printers.inventoryNumber
      JOIN students ON reservations.studentID = students.studentID
      WHERE (printers.model LIKE $name OR students.firstName LIKE $name OR students.lastName LIKE $name)`;
  if (!past) {
    query += " AND reservations.fromDateTime >= date('now')";
  }
  console.log(query);

  db.all(query, { $name: "%" + name + "%" }, (err, rows) => {
    if (err) {
      return res
        .status(StatusCodes.INTERNAL_SERVER_ERROR)
        .json({ error: err.message });
    }
    return res.json(rows);
  });
});

app.post("/purge", (req: Request, res: Response) => {
  db.serialize(() => {
    db.run("DELETE FROM reservations;", (error) => {
      if (error) {
        return res
          .status(StatusCodes.INTERNAL_SERVER_ERROR)
          .json({ error: error.message });
      }
      db.run("DELETE FROM students;", (error) => {
        if (error) {
          return res
            .status(StatusCodes.INTERNAL_SERVER_ERROR)
            .json({ error: error.message });
        }
        db.run("DELETE FROM printers;", (error) => {
          if (error) {
            return res
              .status(StatusCodes.INTERNAL_SERVER_ERROR)
              .json({ error: error.message });
          }
          res
            .status(StatusCodes.NO_CONTENT)
            .json({ message: "All data deleted successfully" });
        });
      });
    });
  });
});

// Setup server
const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Server is running at http://localhost:${PORT}`);
});
