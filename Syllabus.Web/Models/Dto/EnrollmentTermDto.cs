using System;

namespace Syllabus.Web.Models.Dto
{
    public class EnrollmentTermDto
    {
        public string Id { get; set; }
        public long CanvasId { get; set; }
        public long RootAccountId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SisSourceId { get; set; }
    }
}