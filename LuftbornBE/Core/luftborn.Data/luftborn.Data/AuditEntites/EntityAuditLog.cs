using luftborn.Data.AuditEntites.Interfaces;
using luftborn.Ground;
using System;

namespace luftborn.Data.AuditEntites
{
    public class EntityAuditLog : IAuditLog
    {
        //IAuditLog data
        public int Id { get; set; }
        public DateTime AuditDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RemoteUserIp { get; set; }
        public string LocalUserIp { get; set; }
        public string UserAgent { get; set; }
        public string BrowserName { get; set; }
        public string OsName { get; set; }
        public string HostName { get; set; }

        //table data
        public AuditActionTypes ActionType { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string ModifiedColumns { get; set; }
    }
}
