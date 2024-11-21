using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.DAL.Context.Configurations;

public class CommandConfiguration : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder.HasMany(p => p.Clients)
            .WithMany(p => p.Commands)
            .UsingEntity(p => p.ToTable("CommandClients"));
    }
}