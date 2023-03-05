using luftborn.Ground;
using luftborn.Service.Features.Common.Interfaces;
using luftborn.Service.Features.Common.Interfaces.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace luftborn.Presistance.Infrastructure
{

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IAuditRepository _auditRepository;

        public DbConnectionFactory(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }
        public IDbConnection GetDbConnection()
        {
            return new WrappedDbConnection(new SqlConnection(Configurations.luftbornConnectionString), _auditRepository);
        }
    }

}
