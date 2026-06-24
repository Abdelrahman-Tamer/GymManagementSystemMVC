using GymManagementSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.DAL.Configuration
    {
    public class MemberShipConfigurations : IEntityTypeConfiguration<MemberShip>
        {
        public void Configure( EntityTypeBuilder<MemberShip> builder )
            {
            builder.HasKey(m => m.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.Plan)
                .WithMany(p => p.PlanMembers)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(M => M.Member)
                .WithMany(m => m.MemberPlans)
                .HasForeignKey(m => m.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
