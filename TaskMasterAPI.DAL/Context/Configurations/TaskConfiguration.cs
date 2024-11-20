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
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь между Task и Author через AuthorId
        builder
            .HasOne(t => t.Author)
            .WithMany() // Без обратной навигации
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь между Task и Tester через TesterId
        builder
            .HasOne(t => t.Tester)
            .WithMany() // Без обратной навигации
            .HasForeignKey(t => t.TesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore("ClientId1");
    }
}