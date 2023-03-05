using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using luftborn.Presistance.Infrastructure;
using luftborn.Service.Features.Common.Interfaces.Repositories;

namespace luftborn.Presistance
{
    public class luftbornEntitiesFactory : DesignTimeDbContextFactoryBase<luftbornEntities>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditRepository _auditRepository;

        public luftbornEntitiesFactory(IHttpContextAccessor httpContextAccessor,
            IAuditRepository auditRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditRepository = auditRepository;
        }
        public luftbornEntitiesFactory()
        {

        }
        protected override luftbornEntities CreateNewInstance(DbContextOptions<luftbornEntities> options)
        {
            return new luftbornEntities(options, _auditRepository);
        }
    }
}