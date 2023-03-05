using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using luftborn.Data.Entities;
namespace luftborn.Presistance.EntitiesMapping
{
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(i => i.Id).IsClustered(false);
        }
    }
}
