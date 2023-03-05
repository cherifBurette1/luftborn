using luftborn.Data.AuditEntites.Interfaces;
using System;

namespace luftborn.Data.AuditEntites
{
    public class ProcedureAuditLog : IAuditLog
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

        //procedure data
        public string ProcedureName { get; set; }
        public string Parameters { get; set; }
    }
}
