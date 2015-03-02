using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSAdministrationTool.Properties;
using TFSAdministrationTool.Proxy.Common;

namespace TFSAdministrationTool
{
    class NotificationLine
    {
        public string SendToAdress { get; set; }

        public string SendToMail { get; set; }

        public string Action { get; set; }

        public string Role { get; set; }

        public string Project { get; set; }

        public string Server { get; set; }

        public SystemTier AppTier { get; set; }

        public NotificationLine(PendingChange pendingChange)
        {
            SendToAdress = pendingChange.Email;
            SendToMail = pendingChange.DisplayName;
            Action = (pendingChange.ChangeType == TFSAdministrationTool.Proxy.Common.ChangeType.Add) ? Resources.PendingChangeActionAdd : Resources.PendingChangeActionDelete;
            Role = pendingChange.Role;
            Project = pendingChange.TeamProject;
            Server = pendingChange.Server;
            AppTier = pendingChange.Tier;
        }
    }
}
