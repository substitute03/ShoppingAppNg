namespace ShoppingAppApi.Exceptions;

public abstract class ApiException : Exception
{
  public int StatusCode { get; }
  public string Title { get; }
  public string ClientMessage { get; }
  public string? ErrorCode { get; }

  protected ApiException(
    int statusCode,
    string title,
    string clientMessage,
    string? errorCode = null) : base(clientMessage)
  {
    StatusCode = statusCode;
    Title = title;
    ClientMessage = clientMessage;
    ErrorCode = errorCode;
  }
}

