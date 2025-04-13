using System.Net;
using System.Net.Mail;
using codeSparkRequestAPI.Interfaces;
using codeSparkRequestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace codeSparkRequestAPI.Services;

public class sendEmailService : IsendEmailInteface
{   
    private readonly emailSettings _emailSettings;
    public sendEmailService(IOptions<emailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    public EmailResponse sendEmail(requestEmail emailRequest)
    {
        try
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                EnableSsl = true,
            };
            var responseBody = $@"
                <p>Hello {emailRequest.fullName},</p>
                <p>Thanks for reaching out to us. We will get back to you soon.</p>
                <p>Regards,<br/>Team from codeSpark</p>
                <img src='https://www.vestirsourcing.com/codeSparkLogo.png' 
     alt='Company Logo' 
     style='max-width: auto; height: 60px;' />
            ";
            
            var clientMailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SmtpUsername), 
                Subject = "Thanks from CodeSpark",
                Body = responseBody,
                IsBodyHtml = true,
            };

            clientMailMessage.To.Add(emailRequest.emailAddress);
            smtpClient.Send(clientMailMessage);
            var body = $@"
                <p>Hello Team,</p>
                <p>You have received a new request from: <strong>{emailRequest.fullName}</strong></p>
                <p>Email: {emailRequest.emailAddress}<br/>
                Phone: {emailRequest.phoneNumber}</p>
                <p>Organization Name: {emailRequest.OrganizationName}<br/>
                Designation: {emailRequest.designation}</p>
                <p><strong>Message:</strong><br/>{emailRequest.message}</p>
                <p>Regards,<br/>WebPage Request Portal</p>
                <br/>
                <img src='https://www.vestirsourcing.com/codeSparkLogo.png' 
     alt='Company Logo' 
     style='max-width: auto; height: 60px;' />
            ";
            var requestMailMessage = new MailMessage{
                From = new MailAddress(_emailSettings.SmtpUsername),
                Subject = "New request from "+ emailRequest.fullName +" !",
                Body = body,
                IsBodyHtml = true
            };
            requestMailMessage.To.Add(_emailSettings.SmtpUsername);
            smtpClient.Send(requestMailMessage);
            return new EmailResponse { Success = true, Message = "Email sent successfully!" };
        }
        catch (Exception ex)
        {
            return new EmailResponse { Success = false, Message = $"Email send failed: {ex.Message}" };
        }
    } 
}
