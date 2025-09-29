namespace EMS.Web.Models
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; }="https://localhost:7007";
        public int TimeOutSeconds { get; set; }=30;
        public int RetryCount { get; set; } = 3;
    }
}
