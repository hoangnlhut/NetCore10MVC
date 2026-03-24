namespace OidcServer.Models
{
    public class AuthenticationResponseCallbackModel
    {
        public required string CallbackUri { get; set; }
        public required CodeFlowResponseModel CodeFlowResponse { get; set; }
        public string? State { get; set; }
    }
}
