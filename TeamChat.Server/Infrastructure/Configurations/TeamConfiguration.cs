using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(255).IsRequired();
        builder.HasMany(e => e.Groups).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Users).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}