namespace Syllabus.Web.Models
{
    public class AppSettings
    {
        // Canvas Redshift
        public string CanvasRedshiftApiUrl { get; set; }

        // Canvas API
        public string CanvasApiAuthToken { get; set; }

        // Canvas OAuth
        public string CanvasOAuthBaseUrl { get; set; }
        public string CanvasOAuthAuthorizationEndpointUrl { get; set; }
        public string CanvasOAuthTokenEndpointUrl { get; set; }
        public string CanvasOAuthClientId { get; set; }
        public string CanvasOAuthClientSecret { get; set; }
    }
}