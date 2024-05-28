using business_logic_layer.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;



namespace business_logic_layer
{
    public class ContactUsBLL : IEmailContactUs
    {
		
        private readonly emailSettingsModel _emailSettings;
        private readonly EmailSettingsAitmaten _aitmatenEmailSettings;

        public ContactUsBLL(IOptions<emailSettingsModel> sofamimarketOptions, IOptions<EmailSettingsAitmaten> aitmatenOptions)
        {
            _emailSettings = sofamimarketOptions.Value;
            _aitmatenEmailSettings = aitmatenOptions.Value;

        }

        

        public async Task SendEmailAsync(MailContactUS mailRequest, string connectionString)
        {
            // Send confirmation text to customers
            var emailSettings = connectionString == "Aitmaten" ? _aitmatenEmailSettings : _emailSettings;
            string closingGreeting = connectionString == "Aitmaten" ? "Met vriendelijke groet,<br><br>Aitmaten" :
                              connectionString == "SofaniMarket" ? "Met vriendelijke groet,<br><br>Sofani market" :
                              "Met vriendelijke groet"; // 
            var confirmationEmail = new MimeMessage();
            confirmationEmail.Sender = MailboxAddress.Parse(emailSettings.SenderEmail);
            confirmationEmail.To.Add(MailboxAddress.Parse(mailRequest.Email));
            confirmationEmail.Subject = "Bevestiging van uw offerteaanvraag";
            var confirmationBuilder = new BodyBuilder();
            confirmationBuilder.HtmlBody = $"Geachte {mailRequest.Name},<br><br>" +
                 $"Hartelijk dank voor uw bericht via ons 'contact ons' formulier." +
                 $" Wij waarderen het dat u contact met ons heeft opgenomen en we zullen uw bericht zo spoedig mogelijk in behandeling nemen." +
                 $"<br><br>Met deze e-mail willen wij u graag bevestigen dat wij uw bericht hebben ontvangen" +
                 $" en ons team zal hier zo spoedig mogelijk op reageren." +
                $"<br><br>{closingGreeting}";

            confirmationEmail.Body = confirmationBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings.SenderName, emailSettings.SmtpPassword);
                await client.SendAsync(confirmationEmail);
                await client.DisconnectAsync(true);
            }

            // Send request quote information to you
            var quoteEmail = new MimeMessage();
            quoteEmail.Sender = MailboxAddress.Parse(emailSettings.SenderEmail);
            quoteEmail.To.Add(MailboxAddress.Parse(emailSettings.SenderEmail)); // Replace with your email address
            quoteEmail.Subject = mailRequest.Name;
            var quoteBuilder = new BodyBuilder();
            quoteBuilder.HtmlBody = $"Bericht van {mailRequest.Name}<br><br>" +
                 $"Email: {mailRequest.Email}<br><br>" +
                 $"Telefoon: {mailRequest.Telephone}<br><br>" +
                 $"Berichtinhoud: {mailRequest.Body}";
            quoteEmail.Body = quoteBuilder.ToMessageBody();

            using (var quoteClient = new SmtpClient())
            {
                await quoteClient.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await quoteClient.AuthenticateAsync(emailSettings.SenderName, emailSettings.SmtpPassword);
                await quoteClient.SendAsync(quoteEmail);
                await quoteClient.DisconnectAsync(true);
            }
        }
    

}
}

