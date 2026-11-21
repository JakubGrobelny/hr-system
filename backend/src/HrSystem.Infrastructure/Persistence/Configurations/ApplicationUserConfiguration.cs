using HrSystem.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrSystem.Infrastructure.Persistence.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // One account per employee; an employee cannot be deleted while an account references it.
        builder.HasOne(u => u.Employee)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
