using System;

namespace Syllabus.Web.Models.Dto
{
    public class CourseResultDto
    {
        public long Id { get; set; }
        public long CanvasId { get; set; }
        public long? EnrollmentTermId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string SisSourceId { get; set; }
        public long? AccountId { get; set; }
        public string SyllabusBody { get; set; }
        public string WorkflowState { get; set; }
    }
}