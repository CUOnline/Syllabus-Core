using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syllabus.Web.Models;
using Syllabus.Web.Models.Dto;

namespace Syllabus.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly HttpClient canvasRedshiftClient;
        private readonly AppSettings appSettings;

        public SearchController(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            this.canvasRedshiftClient = httpClientFactory.CreateClient(HttpClientNames.CanvasRedshiftClient);
            this.appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var enrollmentTerms = JsonConvert.DeserializeObject<List<EnrollmentTermDto>>(await canvasRedshiftClient.GetStringAsync("EnrollmentTerm"));
            return new JsonResult(enrollmentTerms.Where(x => x.EndDate != null).OrderByDescending(x => x.EndDate));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CourseSearchDto dto)
        {
            try
            {
                var apiUrl = $"Courses/GetCoursesMatchingCriteria?query={dto.SearchTerm}&department={dto.Department}&selectedTermId={dto.SelectedTerm}";
                var result = await canvasRedshiftClient.GetStringAsync(apiUrl);

                var courses = JsonConvert.DeserializeObject<List<CourseResultDto>>(result);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}