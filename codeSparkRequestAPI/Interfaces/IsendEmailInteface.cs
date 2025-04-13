namespace codeSparkRequestAPI.Interfaces;
using codeSparkRequestAPI.Models;
using Microsoft.AspNetCore.Mvc;

public interface IsendEmailInteface
{
    public EmailResponse sendEmail(requestEmail a);
}
