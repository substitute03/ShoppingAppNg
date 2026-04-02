namespace ShoppingAppApi.Exceptions;

public sealed class ConflictException : ApiException
{
  public ConflictException(string clientMessage, string? errorCode = null)
    : base(
      statusCode: Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict,
      title: "Conflict",
      clientMessage: clientMessage,
      errorCode: errorCode)
  {
  }
}

