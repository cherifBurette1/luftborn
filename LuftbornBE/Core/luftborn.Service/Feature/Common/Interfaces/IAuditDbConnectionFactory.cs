using System.Data;

namespace luftborn.Service.Features.Common.Interfaces
{
    public interface IAuditDbConnectionFactory
    {
        IDbConnection GetAuditDbConnection();
    }
}
