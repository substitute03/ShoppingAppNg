namespace ShoppingAppApi.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public Guid IdempotencyToken { get; init; }
    }
}
