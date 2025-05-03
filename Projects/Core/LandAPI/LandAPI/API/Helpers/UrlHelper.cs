namespace LandAPI.API
{
    public static class UrlHelper
    {
        public static string InjectUrls(string url, HttpRequest request)
        {
            string scheme = request.Scheme;
            string host = request.Host.Host;
            string port = (request.Host.Port ?? (scheme == "https" ? 443 : 80)).ToString();

            return url
                .Replace("<scheme>", scheme)
                .Replace("<current-server-ip>", host)
                .Replace("<port>", port);
        }
    }
}
