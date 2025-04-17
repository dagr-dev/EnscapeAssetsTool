using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;

namespace ReadRhinoFile.Helpers
{
    /// <summary>
    /// Helper class for sending email notifications about plugin usage.
    /// </summary>
    public static class EmailSender
    {

        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587;
        private static readonly string senderEmail = "";
        private static readonly string appPassword = "";
        private static readonly string recipentEmail = "dagr@henninglarsen.com";
        private static readonly string subject = "AssetSync: Success";

        /// <summary>
        /// Sends an email notification with information about plugin usage.
        /// </summary>
        /// <param name="computerName">The name of the computer where the plugin was used.</param>
        /// <param name="instanceNum">The number of imported instances by the plugin.</param>
        public static void SendEmail(string computerName, int instanceNum)
        {
            string body = $"Plugin successfully used by: {computerName}" +
                $"\nAmount of imported instances: {instanceNum}";

                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage(senderEmail, recipentEmail))
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        smtpClient.Send(mailMessage);
                    }
                }

            //catch (Exception ex)
            //{
            //    TaskDialog.Show("Error", $"Error sending email: {ex.Message}. Contact with dagr@henninglarsen.com");
            //}
        }
    }
}
