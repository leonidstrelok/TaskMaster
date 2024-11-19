using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskMasterAPI.Models;

namespace TaskMasterAPI.DAL.Context.Configurations;

public class HistoryTaskConfiguration : IEntityTypeConfiguration<HistoryTask>
{
    public void Configure(EntityTypeBuilder<HistoryTask> builder)
    {
    }
}