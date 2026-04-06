namespace Ghtk.Api.AuthenticationHandler
{
    public class RemoteAuthenticationHandler
    {
        private readonly string _remoteAuthenticationServiceUrl;
        private readonly HttpClient _httpClient = new HttpClient();    

        public RemoteAuthenticationHandler(string remoteUrl)
        {
            _remoteAuthenticationServiceUrl = remoteUrl;
        }

        public bool Validate(string clientSource)
        {
            if (string.IsNullOrEmpty(clientSource))
            {
                return false;
            }

            var requestUrl = $"{_remoteAuthenticationServiceUrl}/api/clientSource/{clientSource}";  
            var response = _httpClient.GetAsync(requestUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
