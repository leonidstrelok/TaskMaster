using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.DAL.Context.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasMany(p => p.Comments)
            .WithMany(p => p.Tasks)
            .UsingEntity(p => p.ToTable("TasksComments"));

        builder.HasOne(t => t.Client)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ClientId);

        builder.Ignore("ClientId1");
    }
}