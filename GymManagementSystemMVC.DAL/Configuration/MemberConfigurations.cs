using GymManagementSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystemMVC.DAL.Configuration
    {
    public class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
        {
        public new void Configure( EntityTypeBuilder<Member> builder )
            {
            builder.Property(x => x.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.HealthRecord)
                .WithOne(x => x.Member)
                .HasForeignKey<HealthRecord>(x => x.MemberId);

            base.Configure(builder);
            }
        }
    }
