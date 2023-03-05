using Microsoft.EntityFrameworkCore;
using luftborn.Data.AuditEntites;
using luftborn.Service.Features.Common.Interfaces;

namespace luftborn.Presistance
{
    public class luftbornAuditDbContext : DbContext, IluftbornAuditDbContext
    {
        public luftbornAuditDbContext(DbContextOptions<luftbornAuditDbContext> options) : base(options)
        {

        }
        public DbSet<EntityAuditLog> EntityAuditLogs { get; set; }
        public DbSet<ProcedureAuditLog> ProcedureAuditLogs { get; set; }
    }
}
