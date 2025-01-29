using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        IdentityRole[] roles = [
            new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "organizer", NormalizedName = "ORGANIZER" },
            new IdentityRole { Name = "user", NormalizedName = "USER" }
        ];

        builder.HasData(roles);
    }
}
