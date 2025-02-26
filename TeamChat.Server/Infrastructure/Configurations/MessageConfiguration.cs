using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamChat.Server.Domain;

namespace TeamChat.Server.Infrastructure.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Text).HasMaxLength(255).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.HasOne(e => e.User).WithMany().OnDelete(DeleteBehavior.Cascade);
    }
}