using luftborn.Data.AuditEntites;
using System.Collections.Generic;
using System.Data.Common;

namespace luftborn.Service.Features.Common.Interfaces.Repositories
{
    public interface IAuditRepository
    {
        void LogEntityChanges(List<EntityAuditLog> auditLogs);

        void LogDbCommandDetails(DbCommand command);
    }
}
