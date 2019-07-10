namespace Syllabus.Web.Models
{
    public class HomeViewModel
    {
        public string SearchTerm { get; set; }
        public string Department { get; set; }
        public bool Authorized { get; internal set; }
    }
}