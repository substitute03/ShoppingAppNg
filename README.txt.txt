In addition to checking ProductIds and OrderIds, I used idempotency tokens from the client to prevent duplicates being created.
Used concurrent dictionaries to persist orders and products between requests for the purpose of this demo.
Would normally just flag orders and products as IsDeleted = true in the database, but we are just adding and removing from a dictionary for the purpose of this demo.