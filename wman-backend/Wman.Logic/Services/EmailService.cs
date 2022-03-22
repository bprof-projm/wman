using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.Services
{
    public class EmailService : IEmailService
    {
        private IOptions<SmtpSettings> config;
        public int MyProperty { get; set; }
        public EmailService(IOptions<SmtpSettings> config)
        {
            this.config = config;
        }
        public async Task AssigedToWorkEvent(WorkEvent we, WmanUser user)
        {
            string htmlContent = File.ReadAllText("./Assets/emailhtml.html");
            htmlContent = htmlContent.Replace("NameDynamicValue", $"Dear Mr/Mrs.{user.LastName}");
            htmlContent = htmlContent.Replace("SubjectDynamicValue", $"You have been assigned to {we.JobDescription} event!");
            htmlContent = htmlContent.Replace("JobDescriptionDynamicValue", we.JobDescription);
            htmlContent = htmlContent.Replace("EstimatedStartDateDynamicValue", we.EstimatedStartDate.ToString("yyyy.MM.dd, HH:mm"));
            htmlContent = htmlContent.Replace("EstimatedFinishDateDynamicValue", we.EstimatedFinishDate.ToString("yyyy.MM.dd, HH:mm"));
            htmlContent = htmlContent.Replace("AddressDynamicValue", we.Address.Floordoor == null ?
                we.Address.ZIPCode +", "+ we.Address.City + "<br>" + we.Address.Street + " " +  we.Address.BuildingNumber +
                "" :
                we.Address.ZIPCode + ", " + we.Address.City + "<br>" + we.Address.Street +" "+ we.Address.BuildingNumber + $" {we.Address.Floordoor}");
            await SendEmail(user.Email, $"You have been assigned to {we.JobDescription} event!", htmlContent);
            
        }
        public async Task WorkEventUpdated(WorkEvent we, WmanUser user)
        {
            string htmlContent = File.ReadAllText("./Assets/emailhtml.html");
            htmlContent = htmlContent.Replace("NameDynamicValue", $"Dear Mr/Mrs.{user.LastName}");
            htmlContent = htmlContent.Replace("SubjectDynamicValue", $"The {we.JobDescription} event has been modified!");
            htmlContent = htmlContent.Replace("JobDescriptionDynamicValue", we.JobDescription);
            htmlContent = htmlContent.Replace("EstimatedStartDateDynamicValue", we.EstimatedStartDate.ToString("yyyy.MM.dd, HH:mm"));
            htmlContent = htmlContent.Replace("EstimatedFinishDateDynamicValue", we.EstimatedFinishDate.ToString("yyyy.MM.dd, HH:mm"));
            htmlContent = htmlContent.Replace("AddressDynamicValue", we.Address.Floordoor == null ?
                we.Address.ZIPCode + ", " + we.Address.City + "<br>" + we.Address.Street + " " + we.Address.BuildingNumber +
                "" :
                we.Address.ZIPCode + ", " + we.Address.City + "<br>" + we.Address.Street +" " + we.Address.BuildingNumber + $" {we.Address.Floordoor}");
            await SendEmail(user.Email, $"The {we.JobDescription} event has been modified!", htmlContent);
        }
        public async Task SendXls(WmanUser user, string path)
        {
            string htmlContent = File.ReadAllText("./Assets/managerEmail.html");
            htmlContent = htmlContent.Replace("NameDynamicValue", $"Dear Mr/Mrs.{user.LastName}");
            htmlContent = htmlContent.Replace("SubjectDynamicValue", $"XLS generated at: xxx");
            htmlContent = htmlContent.Replace("MessageDynamicValue", $"Please find the generated statistics attached below");

            await SendEmailWithAttachment(user.Email, $"Manager statistics", htmlContent, path);
        }
        private async Task SendEmailWithAttachment(string toAddress, string subject, string htmlContent, string filePath) 
        {
            SmtpClient smtpClient = new SmtpClient(config.Value.SmtpHost, config.Value.SmtpPort);

            smtpClient.Credentials = new System.Net.NetworkCredential(config.Value.SmtpAddress, config.Value.SmtpSecret);
            // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                Attachment data = new Attachment(filePath, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(filePath);
                disposition.ModificationDate = File.GetLastWriteTime(filePath);
                disposition.ReadDate = File.GetLastAccessTime(filePath);
                mail.Attachments.Add(data);
            }

            //Setting From , To and CC
            mail.From = new MailAddress(config.Value.SmtpAddress);
            mail.To.Add(new MailAddress(toAddress));
            //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));
            mail.Body = htmlContent;
            mail.Subject = subject;

            mail.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mail);
        }
        private async Task SendEmail(string toAddress, string subject, string htmlContent)
        {
            await this.SendEmailWithAttachment(toAddress, subject, htmlContent, null);
        }
    }
}
