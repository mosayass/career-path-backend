using System.Threading.Tasks;

namespace CareerPath.Identity.Core.Contracts;

public interface IEmailNotificationService
{
    // Abstracts away Mailtrap and SmtpClient completely
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
}