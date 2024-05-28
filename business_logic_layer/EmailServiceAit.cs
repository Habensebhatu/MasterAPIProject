using System;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using business_logic_layer.ViewModel;
using MimeKit;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace business_logic_layer
{

    public class EmailServiceAit : IEmailServiceAit
    {
        private readonly EmailSettingsAitmaten _emailSettings;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;


        public EmailServiceAit(IOptions<EmailSettingsAitmaten> aitmatenOptions, IWebHostEnvironment env, IConfiguration configuration)
        {
            _emailSettings = aitmatenOptions.Value;
            _configuration = configuration;
            _environment = env;

        }

       
        public async Task SendEmailAsyncAit(mailRequestModelAit mailRequest)
        {
            var emailTemplatePathAit = _configuration["TemplatePaths:EmailTemplateAit"];
            var emailTemplatePath = Path.Combine(_environment.WebRootPath, emailTemplatePathAit);
            var emailTemplate = await File.ReadAllTextAsync(emailTemplatePath);

            // Vervang placeholders in de template
            emailTemplate = emailTemplate.Replace("{OrderDate}", mailRequest.OrderDate.ToString("dd-MM-yyyy"));
            emailTemplate = emailTemplate.Replace("{recipientName}", mailRequest.recipientName);
            emailTemplate = emailTemplate.Replace("{adres}", mailRequest.adres);
            emailTemplate = emailTemplate.Replace("{city}", mailRequest.city);
            emailTemplate = emailTemplate.Replace("{OrderNummer}", mailRequest.OrderNummer.ToString());
            emailTemplate = emailTemplate.Replace("{postalCode}", mailRequest.postalCode);

            // Bereken en bouw de bestelitems op
            decimal AmountTotal = 0;
            StringBuilder orderItemsBuilder = new StringBuilder();

            foreach (var item in mailRequest.OrderItems)
            {
                decimal price = item.Price;
                decimal total = item.Quantity * price;
                AmountTotal += total;

                orderItemsBuilder.AppendLine($@"
            <tr>
                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>
                    <img src='{item.ImageUrl}' alt='Product Image' width='50px' height='50px' />
                </td>
                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{item.ProductName}</td>
                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>{item.Quantity}</td>
                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>€{price.ToString("0.00")}</td>
                <td style='padding: 8px; border-bottom: 1px solid #e0e0e0;'>€{total.ToString("0.00")}</td>
            </tr>");
            }

            string orderItemsHtml = orderItemsBuilder.ToString();
            emailTemplate = emailTemplate.Replace("{orderItemsHtml}", orderItemsHtml);
            emailTemplate = emailTemplate.Replace("{formattedAmountTotal}", AmountTotal.ToString("0.00"));

            // Stuur bevestiging naar klant
            var confirmationEmail = new MimeMessage();
            confirmationEmail.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            confirmationEmail.To.Add(MailboxAddress.Parse(mailRequest.Email));
            confirmationEmail.Subject = "Bevestiging van uw bestelling";
            confirmationEmail.Body = new TextPart("html") { Text = emailTemplate };

            await SendMimeMessageAsync(confirmationEmail);

            // Stuur melding naar verkoper
            var adminNotificationEmail = new MimeMessage();
            adminNotificationEmail.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            adminNotificationEmail.To.Add(MailboxAddress.Parse(_emailSettings.SenderEmail)); // Voeg je eigen e-mailadres toe
            adminNotificationEmail.Subject = $"Nieuwe bestelling ontvangen: {mailRequest.OrderNummer}";
            adminNotificationEmail.Body = new TextPart("html") { Text = emailTemplate };

            await SendMimeMessageAsync(adminNotificationEmail);
        }

        


        private async Task SendMimeMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderName, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}



