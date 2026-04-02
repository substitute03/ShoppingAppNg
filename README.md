# ShoppingApp Take-Home - Technical Notes

## Prerequisites

- .NET SDK 10.0+
- Node.js 20+ and npm

## API (.NET)

1. Open a terminal in:

   `ShoppingAppApi/ShoppingAppApi`

2. Run:

   ```bash
   dotnet restore
   dotnet run --launch-profile http
   ```

   **Alternatively**: From Visual Studio, open ShoppingAppApi.slnx, set the debug dropdown to the `http` profile, then run the ShoppingAppApi project.

3. API will run at:

   http://localhost:5254

## Frontend (Angular)

1. Open a terminal in:

   `ShoppingAppNg`

2. Run:

   ```bash
   npm install
   npm start
   ```

3. Frontend will run at:

   http://localhost:4200

## Notes

- This demo is configured to run the API over HTTP.

## General Design

- The solution is separated into a .NET API and an Angular frontend, with clear separation between presentation, business logic, and data access responsibilities.
- The API follows a controller + service + repository structure to keep concerns isolated and make behavior easier to extend and easier to test by using dependency injection.
- The API uses custom errors which get caught by the middleware so appropriate HTTP error codes are returned and stack trace is preserved.
- Controllers use appropriate HTTP verbs and status codes, and routes are designed around resource-oriented API endpoints.
  - I would normally prefer a soft delete using PUT but I am actually deleting a resource here so the delete product endpoint is an HTTP delete.
- Order and product identifiers use GUIDs for simplicity in this in-memory demo and to avoid manual integer ID allocation logic.

## Technical Decisions and Justification

- The API is run over HTTP (not HTTPS) for demo convenience and local development simplicity.
- A toggle on the home screen is used for flagging the user as an admin for the purpose of simulating authorisation in this demo for simplicity. In a production app this would likely be handled by roles/claimns on a JWT.
    - Additionally, there are no UI elements hidden based on the user role for demo purposes, but this would be done in a production app.
- A toggle on the order page is used for mocking the payment flow and triggering a successful or failed payment.
- Angular Signals are used for UI state updates because Zone.js did not reliably detect state changes in this setup.
- Product create/delete operations in the UI use optimistic updates for a responsive experience, with rollback if API calls fail.
- A template-driven form is used for simple product creation, while a reactive form is used for order creation because it is more complex and dynamic.

## Edge Cases and Constraints Handling

- Idempotency tokens are sent from the client for order creation to prevent duplicate submissions.
- A “New Order” action resets the idempotency token. In production, this reset would likely occur after a order creation for example.
- The UI intentionally keeps delete buttons visible for non-admin users in this demo so a 403 Forbidden response can be demonstrated clearly.
- In-memory concurrent dictionaries are used to persist products and orders across requests since there is no database.
- Current deletion behavior physically removes entries from dictionaries; in production, soft deletes (for example, IsDeleted) would be preferred.

## Safety, Maintainability, and Future-Proofing

- Dependency injection and a service/repository design reduce coupling and make future changes lower-risk and busniess logic easier to test.
- API behavior is designed to fail safely with explicit error responses.
- The current “Created Order” and “Retrieved Order” display sections intentionally share the same layout and are strong candidates for extraction into a reusable OrderDetails component.
- Unit tests are not included in this demo implementation but adding automated tests would be a priority in a production-ready version.
