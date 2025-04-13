using System.Net;
using System.Net.Mail;
using codeSparkRequestAPI.Interfaces;
using codeSparkRequestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class requestController : ControllerBase
    {
        private readonly IsendEmailInteface _sendEmailService;
        public requestController(IsendEmailInteface sendEmail)
        {
            _sendEmailService = sendEmail;
        }
        [HttpPost("send-email")]
        public IActionResult SendEmail([FromBody] requestEmail emailRequest)
        {
            var result = _sendEmailService.sendEmail(emailRequest);
            if (result.Success)
                return Ok(result.Message);
            else
                return StatusCode(500, result.Message);
        }
    }
}
