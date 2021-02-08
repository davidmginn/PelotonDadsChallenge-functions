using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FileHelpers;
using Microsoft.Extensions.Options;
using PelotonDadsChallenge.Configuration;
using PelotonDadsChallenge.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PelotonDadsChallenge.Services
{
    public class SendGridService : ISendGridService
    {
        private SendGridOptions _sendGridOptions;

        public SendGridService(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }

        public async Task EmailChallengeResults(IEnumerable<PelotonDadChallengeResult> results)
        {
            var engine = new FileHelperEngine<PelotonDadChallengeResult>();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            engine.WriteStream(writer, results);
            stream.Position = 0;

            var reader = new StreamReader(stream);

            var csv = await reader.ReadToEndAsync();

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(csv);
            var content = System.Convert.ToBase64String(plainTextBytes);

            var client = new SendGridClient(_sendGridOptions.ApiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridOptions.FromEmail),
                Subject = "Peloton Dads Challenge Report",
                PlainTextContent = "Sample Challenge Report ",
                HtmlContent = "<strong>Sample Challenge Report</strong>"
            };
            msg.AddTo(new EmailAddress(_sendGridOptions.ToEmail, null));
            msg.AddAttachment("pelotonDadsChallengeResults.csv", content, "text/csv", "attachment", "banner");

            var response = await client.SendEmailAsync(msg);
        }
    }
}
