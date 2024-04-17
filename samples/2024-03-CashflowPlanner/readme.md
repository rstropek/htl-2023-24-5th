# Cashflow Planner

## Introduction

This project involves developing a financial planning tool for enterprise customers, focusing on managing cashflows in multiple currencies. Your main task is to enable the input of cashflows in various currencies and integrate functionality to import exchange rates from a CSV file. These rates are crucial for converting all cashflows to the Euro, the base currency.

The application must also feature a planning calendar that presents the cashflows for each month of a selected year, aiding in financial planning and analysis.

The goal is to create a tool that simplifies the financial management process for businesses operating with multiple currencies, providing a clear overview of financial activities throughout the year.

## Starter Code

The starter code already contains the data model in [_Model.cs_](./CashflowPlanner.Data/Model.cs).
It also contains the data context in [ApplicationDataContext.cs_](./CashflowPlanner.Data/ApplicationDataContext.cs). You just have to create the migrations and update the database. Note that the data context automatically
creates reference data (currencies and cashflow categories) to make testing of the application easier for you.

The UI design for the WPF app was already done. You can find the resulting XAML code in [_CashflowPlanner_](./CashflowPlanner/).

## Level 1 - Exchange Rates (Mandatory)

When the program starts (`LoadData` method in [_MainWindowViewModel.cs_](./CashflowPlanner/MainWindowViewModel.cs)), read the exchange rates from the database. Display the exchange rate in the data grid in the _Exchange Rates_ tab.

If the user clicks the _Import_ button, the program must import the exchange rate from [_ExchangeRates.csv_](./CashflowPlanner.Data/ExchangeRates.csv) into the database. Importing means that all existing exchange rates are deleted and new ones are added. The entire operation must be done in a single database transaction.

## Level 2 - Cashflows (Mandatory)

When the program starts, read the cashflows from the database. Display the cashflows in the data grid in the _Cashflows_ tab.

The _Year/Month_ combo box must contain all years and months between 2023 and 2026 in the format _YYYY/MM_. Values for the categories and the currencies combo boxes must be read from the database when the program starts.

If the user clicks on a cashflow, copy the data of the selected row into the form at the bottom of the tab.

If the user clicks the _Reset_ button, clear all fields in the form.

If the user clicks the _Delete_ button, delete the selected record. The _Delete_ button must be disabled if no record is selected. After deleting the record, reread the cashflow list from the database.

If the user clicks the _Save_ button, add the new record to the database and reread the cashflow list from the database. The _Save_ button must be disabled if one of the fields in the form is empty.

## Level 3 - Cashflow Calendar (Optional)

When the program starts, calculate the cashflow calendar based on the cashflows and exchange rates in the database:

* Read all Categories, and order them by `InOut` and then by `Id`.
* Fill the cashflow calendar with cashflows from the database. All cashflows must be converted to Euro using the exchange rates from the database. The cashflow calendar must show the cashflows for each month of the selected year (zero if there are no cashflows for a month).
* Add an intermediate total for all income categories, an additional intermediate total for all expense categories, and a final total for all cashflows (see also `Level` property in `PlannerLine`). Note that the starter code contains sample data that illustrates the desired output.

By default, the selected calendar year is 2024. The user can switch years with the _next_ and _previous_ buttons.

If the user imports exchange rates, the cashflow calendar must be recalculated. If the user adds, or deletes a cashflow, the cashflow calendar must be recalculated.

## Level 4 - Unit Tests (Optional)

Write at least 5 meaningful unit tests for the following business logic:

* Convert a given currency amount from a currency into Euro and vice versa.
* Calculate the cashflow calendar for a given year.

## Level 5 - Single DB Operation (Optional)

Ensure that no two database operations can be triggered by the user at the same time. Examples:

* If the user clicks the _Import_ button in _Exchange Rates_, the _Save_ button in _Cashflows_ must be disabled.
* If the user clicks the _Save_ button, the buttons for switching the year in the _Cashflow Calendar_ must be disabled as clicking it would trigger reading cashflows from the database.

## Non-Functional Requirements

* The program must be implemented in C# and WPF.
* The program must use the MVVM pattern.
* The program must use Entity Framework Core for data access.
* Think about where you put the data access logic and the business logic. Feel free to restructure the code (e.g. adding classes, moving classes or records, adding libraries etc.) Document your decision in the code using comments.
