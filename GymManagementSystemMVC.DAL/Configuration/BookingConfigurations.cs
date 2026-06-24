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
    public class BookingConfigurations : IEntityTypeConfiguration<Booking>
        {
            public void Configure(EntityTypeBuilder<Booking> builder )
            {
            builder.Ignore(b => b.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            #region Session
            builder.HasOne(x => x.Session)
                .WithMany(x => x.SessionMembers)
                .HasForeignKey(x => x.SessionId);

            builder.HasOne(x => x.Member)
                .WithMany(x => x.MemberSessions)
                .HasForeignKey(x => x.MemberId);
            builder.HasKey(x => new { x.MemberId, x.SessionId });
            #endregion
            }
        }
    }
