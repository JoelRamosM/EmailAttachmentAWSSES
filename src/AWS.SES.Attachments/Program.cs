using System;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;

namespace Test.SES.Anexos
{

    /// <summary>
    /// First you must to install the following Nuget Packages: MimeKitLite, AWSSDK.Core and AWSSDK.SimpleEmail
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Press any key to send mail!");
            Console.ReadKey();
            //Getting AWS credentials from App.config
            //note: see the app.config to get a example
            var credentials = new EnvironmentAWSCredentials();

            //Client SES instantiated
            var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.USEast1);
            var mimeMessage = new MimeMessage();

            //Add sender e-mail address
            //Note: this e-mail address must to be allowed and checked by AWS SES
            mimeMessage.From.Add(new MailboxAddress("Test Sender", "teste.sender@test.com.br"));

            //Add  e-mail address destiny
            mimeMessage.To.Add(new MailboxAddress("Joel", "joel.teste@gmail.com"));
            mimeMessage.Subject = "Test";
            //Getting attachment stream
            var fileBytes = File.ReadAllBytes(@"C:\anyfile.pdf");

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Testing the body message";

            //You must to inform the mime-type of the attachment and his name
            bodyBuilder.Attachments.Add("AnyAttachment.pdf", fileBytes, new ContentType("application", "pdf"));
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            //Map MimeMessage to MemoryStream, that is what SenRawEmailRequest accepts
            var rawMessage = new MemoryStream();
            mimeMessage.WriteTo(rawMessage);

            client.SendRawEmail(new SendRawEmailRequest(new RawMessage(rawMessage)));
            Console.WriteLine("Email Sended");

            Console.ReadKey();
        }
    }


}
