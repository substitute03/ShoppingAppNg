namespace ShoppingAppApi.Exceptions;

public sealed class PaymentFailedException : ApiException
{
  public PaymentFailedException(
    string clientMessage = "Payment failed",
    string? errorCode = null)
    : base(
      statusCode: 500,
      title: "Internal Server Error",
      clientMessage: clientMessage,
      errorCode: errorCode)
  {
  }
}

