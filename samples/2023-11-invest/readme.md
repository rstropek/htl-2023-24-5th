# School Budgeting App
   
## Objective

This exercise is designed to assess your knowledge about _Entity Framework Core_ with _ASP.NET Core_ by constructing an API for a _school budgeting app_. The goal is to design a system that allows users to manage investment requests and view the budget allocation status.  
   
## API Functionality

Required API endpoints:

1. **Add Investment Requests**: Users must be capable of adding new investment requests. Each request must include the following attributes (choose meaningful data types and field lengths for each attribute):
   - Description: A brief description of the investment request.
   - Requestor ID: The user making the request. This should reference a _user_ table. Insert users manually through Azure Data Studio.  
   - EshopLink: A link to the offer in an e-shop such as Amazon.
   - Reoccuring: This indicates whether the investment is reoccuring (i.e. has to be made every year).
   - Costs: The cost of the investment in EUR.
   - Accepted: This indicates whether the investment is pending (`null`), accepted (`true`), or declined (`false`).
   
2. **Update and Delete Investment Requests**: Users should have the ability to update existing investment requests as well as delete them when necessary.  
   
3. **Investment Requests Query Functionality**: Users should be able to retrieve a list of all investment requests. The API should provide options for filtering this list based on the user and/or the status of the investment (accepted or declined).  
   
4. **Budget Statistics Functionality**: The API should include an endpoint that accepts the total budget of the school for the next 12 months as an input parameter. This endpoint should calculate the total costs of accepted investment requests for the next 12 months, and return this total cost value along with the percentage of the total budget already allocated to these investments.  
