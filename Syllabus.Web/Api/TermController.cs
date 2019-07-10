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
    public class TermController : ControllerBase
    {
        private readonly HttpClient canvasRedshiftClient;
        private readonly AppSettings appSettings;

        public TermController(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
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
    }
}