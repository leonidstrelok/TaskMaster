using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.DAL.Context.Configurations;

public class CommentTaskConfiguration : IEntityTypeConfiguration<CommentTask>
{
    public void Configure(EntityTypeBuilder<CommentTask> builder)
    {
        builder.HasMany(p => p.Tasks)
            .WithMany(p => p.Comments)
            .UsingEntity(p => p.ToTable("CommentsTasks"));
    }
}