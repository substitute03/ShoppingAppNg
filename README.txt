Run the API using http rather than https for this demo.
Zone.js wasn't detecting changes so using signals for change detection
I would have hidden the delete product buttons for non-admins but, for the purpose of this demo, they remain visible so you can see an HTTP 403 being returned
In addition to checking ProductIds and OrderIds, I used idempotency tokens from the client to prevent duplicates being created.
Used concurrent dictionaries to persist orders and products between requests for the purpose of this demo.
Would normally just flag orders and products as IsDeleted = true in the database, but we are just adding and removing from a dictionary for the purpose of this demo.
Using guids for Order and Products Ids for simplicity so I don't need to increment an integer ID when adding new orders/products.