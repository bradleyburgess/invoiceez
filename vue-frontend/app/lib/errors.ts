export class NetworkError extends Error {
  constructor(
    message = "Server unreachable. Please check your network connection."
  ) {
    super(message);
    this.name = "NetworkUnreachable";
  }
}
