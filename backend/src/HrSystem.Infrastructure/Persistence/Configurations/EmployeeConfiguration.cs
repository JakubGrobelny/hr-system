using HrSystem.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrSystem.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).HasMaxLength(Employee.NameMaxLength).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(Employee.NameMaxLength).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(Employee.EmailMaxLength).IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => new { e.LastName, e.FirstName });

        builder.Ignore(e => e.FullName);
    }
}
