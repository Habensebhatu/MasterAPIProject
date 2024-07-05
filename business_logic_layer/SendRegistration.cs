using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace business_logic_layer
{
    public class SendRegistration : ISendRegistration
    {

        private readonly EmailSettingsAitmaten _aitmatenEmailSettings;
      
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public SendRegistration(IOptions<EmailSettingsAitmaten> aitmatenOptions, IWebHostEnvironment env, IConfiguration configuration)
        {

            _aitmatenEmailSettings = aitmatenOptions.Value;
            _configuration = configuration;
            _environment = env;
        }

        public async Task SendRegistrationPendingEmail(UserRegistrationModel user)
        {

            
            var registrationTemplatePath = _configuration["TemplatePaths:RegistrationTemplate"];
            var emailTemplatePath = Path.Combine(_environment.WebRootPath, registrationTemplatePath);
            Console.WriteLine($"emailTemplatePath: {emailTemplatePath}");

            if (!File.Exists(emailTemplatePath))
            {
                throw new FileNotFoundException($"Het templatebestand werd niet gevonden op pad: {emailTemplatePath}");
            }

            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);
            emailTemplate = emailTemplate.Replace("{Name}", user.FirstName);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_aitmatenEmailSettings.SenderEmail);
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Uw registratie is in afwachting van goedkeuring";
            var builder = new BodyBuilder { HtmlBody = emailTemplate };
            email.Body = builder.ToMessageBody();
            await SendMimeMessageAsync(email);
            // Maak en verzend een administratieve notificatie e-mail naar jezelf
            var adminNotificationTemplatePath = _configuration["TemplatePaths:AdminNotificationTemplate"];
            var adminMaiTemplatePath = Path.Combine(_environment.WebRootPath, adminNotificationTemplatePath);
            var adminMailRequestTemplte = await File.ReadAllTextAsync(adminMaiTemplatePath);
            adminMailRequestTemplte = adminMailRequestTemplte.Replace("{Name}", user.FirstName);
            adminMailRequestTemplte = adminMailRequestTemplte.Replace("{BedrijfsNaam}", user.BedrijfsNaam);
            adminMailRequestTemplte = adminMailRequestTemplte.Replace("{KvkNummer}", user.KvkNummer);
            adminMailRequestTemplte = adminMailRequestTemplte.Replace("{Email}", user.Email);
            var adminMailRequest = new MimeMessage();
            adminMailRequest.Sender = MailboxAddress.Parse(_aitmatenEmailSettings.SenderEmail);
            adminMailRequest.To.Add(MailboxAddress.Parse(_aitmatenEmailSettings.SenderEmail));
            adminMailRequest.Subject = $"Nieuwe Gebruikersregistratie";
            var adminMailRequestbuilder = new BodyBuilder { HtmlBody = adminMailRequestTemplte };
            adminMailRequest.Body = adminMailRequestbuilder.ToMessageBody();

            await SendMimeMessageAsync(adminMailRequest);




        }

        public async Task SendAccountActivatedEmail(UserRegistrationModel user)
        {
            var accountActivatedTemplatePath = _configuration["TemplatePaths:AccountActivationTemplate"];
            var emailTemplatePath = Path.Combine(_environment.WebRootPath, accountActivatedTemplatePath);
            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);
            emailTemplate = emailTemplate.Replace("{Name}", $"{user.FirstName} {user.LastName}");
            emailTemplate = emailTemplate.Replace("{BedrijfsNaam}", user.BedrijfsNaam);

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_aitmatenEmailSettings.SenderEmail);
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Uw Account Is Geactiveerd";
            var builder = new BodyBuilder { HtmlBody = emailTemplate };
            email.Body = builder.ToMessageBody();

            await SendMimeMessageAsync(email);
        }


        private async Task SendMimeMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_aitmatenEmailSettings.SmtpServer, _aitmatenEmailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_aitmatenEmailSettings.SenderName, _aitmatenEmailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}



