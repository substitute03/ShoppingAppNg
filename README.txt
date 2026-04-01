ShoppingApp Take-Home - Technical Notes

General Design
- The solution is separated into a .NET API and an Angular frontend, with clear separation between presentation, business logic, and data access responsibilities.
- The API follows a controller + service + repository structure to keep concerns isolated and make behavior easier to extend and easier to test by using dependency injection.
- Controllers use appropriate HTTP verbs and status codes, and routes are designed around resource-oriented API endpoints.
- Order and product identifiers use GUIDs for simplicity in this in-memory demo and to avoid manual integer ID allocation logic.

Technical Decisions and Justification
- The API is run over HTTP (not HTTPS) for demo convenience and local development simplicity.
- Angular Signals are used for UI state updates because Zone.js did not reliably detect state changes in this setup.
- Product create/delete operations in the UI use optimistic updates for a responsive experience, with rollback if API calls fail.
- A template-driven form is used for simple product creation, while a reactive form is used for order creation because it is more complex and dynamic.

Edge Cases and Constraints Handling
- Idempotency tokens are sent from the client for order creation to prevent duplicate submissions.
- A “New Order” action resets the idempotency token. In production, this reset would typically occur after a successful submission flow (for example, post-redirect-success).
- The UI intentionally keeps delete buttons visible for non-admin users in this demo so a 403 Forbidden response can be demonstrated clearly.
- In-memory concurrent dictionaries are used to persist products and orders across requests since there is no database.
- Current deletion behavior physically removes entries from dictionaries; in production, soft deletes (for example, IsDeleted) would be preferred.

Safety, Maintainability, and Future-Proofing
- Dependency injection and a service/repository design reduce coupling and make future changes lower-risk and busniess logic easier to test.
- API behavior is designed to fail safely with explicit error responses.
- The current “Created Order” and “Retrieved Order” display sections intentionally share the same layout and are strong candidates for extraction into a reusable OrderDetails component.

Notes
- This implementation is intentionally lightweight for a take-home demo while still showing design choices that scale toward production patterns.