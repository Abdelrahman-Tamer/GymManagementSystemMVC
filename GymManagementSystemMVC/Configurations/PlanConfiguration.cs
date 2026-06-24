using GymManagementSystemMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystemMVC.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .HasPrecision(10, 2);

            builder.Property(x => x.CreatedAT)
                .HasColumnName("CreateAt")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedAT)
                .HasColumnName("UpdateAt");

            builder.Property(x => x.IsActive)
                .HasColumnName("isActive");

            builder.ToTable("Plans", tb =>
            {
                tb.HasCheckConstraint("PlanDurationCheck", "DurationInDays BETWEEN 1 AND 365");
            });
        }
    }
}
