using luftborn.Ground;
using luftborn.Service.Features.Common.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace luftborn.Presistance.Infrastructure
{

    public class AuditDbConnectionFactory : IAuditDbConnectionFactory
    {
        public IDbConnection GetAuditDbConnection()
        {
            return new SqlConnection(Configurations.luftbornAuditConnectionString);
        }
    }

}
