using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Password).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Role).IsRequired();
        builder.HasMany(e => e.Teams).WithMany(e => e.Users);
    }
}