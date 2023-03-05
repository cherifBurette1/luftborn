using System;

namespace luftborn.Data.AuditEntites.Interfaces
{
    public interface IAuditLog
    {
        public int Id { get; set; }
        public DateTime AuditDate { get; set; }
        //user data
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RemoteUserIp { get; set; }
        public string LocalUserIp { get; set; }
        public string UserAgent { get; set; }
        public string BrowserName { get; set; }
        public string OsName { get; set; }
        public string HostName { get; set; }
    }
}
