Run the API using http rather than https for this demo.
Zone.js wasn't detecting changes so using signals for change detection
I would have hidden the delete product buttons for non-admins but, for the purpose of this demo, they remain visible so you can see an HTTP 403 being returned
In addition to checking ProductIds and OrderIds, I sent idempotency tokens from the client to prevent duplicates being created.
Reset the idempotency token by clicking "New order". In a live app this would likely be reset when being redirected to a success page or similar after form submission.
Used concurrent dictionaries to persist orders and products between requests for the purpose of this demo.
Would normally just flag orders and products as IsDeleted = true in the database, but we are just adding and removing from a dictionary for the purpose of this demo.
Using guids for Order and Products Ids for simplicity so I don't need to increment an integer ID when adding new orders/products.
Used a template driven forms for adding a product as it's very simple but used a reactive form for creating an order as it is more complex.
Used optimistic saves and deletions which are rolled back in the UI if there is an error whilst saving or deleting.
Using appropriate HTTP status codes and HTTP verbs in the controllers.
The created order and retrived order displays on the orders screen are identical in layout and would be ideal candidates for creating a shared "OrderDetails" component.