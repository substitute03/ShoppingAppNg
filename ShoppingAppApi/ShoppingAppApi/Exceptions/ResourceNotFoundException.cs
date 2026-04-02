namespace ShoppingAppApi.Exceptions;

public sealed class ResourceNotFoundException : ApiException
{
  public ResourceNotFoundException(string clientMessage, string? errorCode = null)
    : base(
      statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound,
      title: "Not Found",
      clientMessage: clientMessage,
      errorCode: errorCode)
  {
  }
}

