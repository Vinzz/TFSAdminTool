using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace TFSAdministrationTool
{
    class NotificationService
    {
        public Dictionary<string, List<NotificationLine>> Notifications { get; set; }

        public NotificationService()
        {
            Notifications = new Dictionary<string, List<NotificationLine>>();
        }

        public List<MailMessage> PrepareNotifications()
        {
            List<MailMessage> ans = new List<MailMessage>();

            foreach(string email in Notifications.Keys)
            {
                if(!string.IsNullOrEmpty(email))
                { 
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = Properties.Resources.Notifi    cationBody.StartsWith("<html");
                    mail.To.Add(email);

#if DEBUG
                    mail.Bcc.Add("vincent.tollu@orange.com");
#endif
                    mail.Subject = Properties.Resources.NotificationSubject;

                    StringBuilder bodyList = new StringBuilder();

                    foreach(NotificationLine line in Notifications[email])
                    {
                        bodyList.AppendLine(
                            string.Format(Properties.Resources.NotificationLine, line.Server, line.Project, line.Action, line.Role, line.AppTier));
                    }

                    mail.Body = Properties.Resources.NotificationBody.Replace("{0}", bodyList.ToString());

                    ans.Add(mail);
                }
            }

            return ans;
        }
    }
}
