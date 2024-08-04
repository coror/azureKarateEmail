using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Erin.Coralic
{
    public class SendEmail
    {
        private readonly ILogger<SendEmail> _logger;

        public SendEmail(ILogger<SendEmail> logger)
        {
            _logger = logger;
        }

        [Function("SendEmail")]
        [SendGridOutput(ApiKey = "AzureWebJobsSendGridApiKey")]
        public SendGridMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
        [FromBody] FormInput formInput)
        {
            SendGridMessage msg = new SendGridMessage();
            var fromEmailAddress = new EmailAddress("erco.sp22@gmail.com", "Karate Institute");
            EmailAddress[] toEmailAddresses = {
                new EmailAddress("erco.sp22@gmail.com"),
                new EmailAddress(formInput.Email)
            };
            var subject = "Nova Vpisnica";
            var PlainTextContent = $@"
                <table border='1'>
                    <tr><th>Property</th><th>Value</th></tr>
                    <tr><td>Ime</td><td>{formInput.Ime}</td></tr>
                    <tr><td>Priimek</td><td>{formInput.Priimek}</td></tr>
                    <tr><td>NaslovBivalisca</td><td>{formInput.NaslovBivalisca}</td></tr>
                    <tr><td>PostnaSt</td><td>{formInput.PostnaSt}</td></tr>
                    <tr><td>Kraj</td><td>{formInput.Kraj}</td></tr>
                    <tr><td>DatumRoj</td><td>{formInput.DatumRoj}</td></tr>
                    <tr><td>Email</td><td>{formInput.Email}</td></tr>
                    <tr><td>Telefon</td><td>{formInput.Telefon}</td></tr>
                    <tr><td>Tecaj</td><td>{formInput.Tecaj}</td></tr>
                    <tr><td>Obisk</td><td>{formInput.Obisk}</td></tr>
                </table>";
            msg.SetFrom(fromEmailAddress);
            foreach (EmailAddress e in toEmailAddresses)
            {
                msg.AddTo(e);
            }
            msg.SetSubject(subject);
            msg.AddContent("text/html", PlainTextContent);
            return msg;
        }
    }
}
