namespace GatewayApi.Helpers
{
    public static class ReverseProxyConfigHelper
    {
        public static List<string> GetPublicRoutes(IConfiguration configuration)
        {
            var publicRoutesSection = configuration.GetSection("ReverseProxy:PublicRoutes");
            var publicPaths = new List<string>();

            foreach (var route in publicRoutesSection.GetChildren())
            {
                // Lire le path depuis la clé "Match:Path"
                var path = route.GetSection("Match:Path").Value;
                if (!string.IsNullOrEmpty(path))
                {
                    publicPaths.Add(path);
                }
            }

            return publicPaths;
        }
    }
}
