using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Contracts;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}