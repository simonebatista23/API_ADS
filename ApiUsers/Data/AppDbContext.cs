using ApiUsers.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Dept> Depts { get; set; }
        public DbSet<Profile> UserProfiles { get; set; }
        public DbSet<Statususer> UserStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StatusTickets> StatusTickets { get; set; }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketsTransction> TicketsTransctions { get; set; }

        public DbSet<StatusTickets> Statustickets { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dept>().ToTable("depts");
            modelBuilder.Entity<Profile>().ToTable("profiles");
            modelBuilder.Entity<Statususer>().ToTable("statususers");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Category>().ToTable("category");
            modelBuilder.Entity<StatusTickets>().ToTable("statusticket");
            modelBuilder.Entity<Tickets>().ToTable("tickets");
            modelBuilder.Entity<TicketsTransction>().ToTable("ticketstransction");

            modelBuilder.Entity<User>()
                .HasOne(u => u.IdDeptNavigation)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.IdDept)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Users_Dept");

            modelBuilder.Entity<User>()
                .HasOne(u => u.IdProfileNavigation)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.IdProfile)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Users_Profile");

            modelBuilder.Entity<User>()
                .HasOne(u => u.IdStatusNavigation)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.IdStatus)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Users_Statususer");

            modelBuilder.Entity<Category>()
                 .HasOne(c => c.IdDeptNavigation)
                 .WithMany(c => c.Category)
                 .HasForeignKey(c => c.IdDept)
                 .OnDelete(DeleteBehavior.SetNull)
                 .HasConstraintName("FK_Category_Dept");

            modelBuilder.Entity<Tickets>()
                      .HasOne(t => t.IdStatusTicketsNavigation)
                      .WithMany(t => t.Tickets)
                      .HasForeignKey(t => t.Id_status)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Status_StatusTicket");

            modelBuilder.Entity<Tickets>()
                  .HasOne(t => t.IdCategoryNavigation)
                  .WithMany(t => t.Tickets)
                  .HasForeignKey(t => t.Id_category)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_Category_Category");

            modelBuilder.Entity<Tickets>()
                  .HasOne(t => t.IdUserNavigation)
                  .WithMany(t => t.Tickets)
                  .HasForeignKey(t => t.Id_user)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_user_UserTicket");

            modelBuilder.Entity<Tickets>()
                  .HasOne(t => t.IdDeptNavigation)
                  .WithMany(t => t.Tickets)
                  .HasForeignKey(t => t.Id_dept_target)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_Dept_DeptTicket");

            modelBuilder.Entity<TicketsTransction>()
                   .HasOne(t => t.UserSource)
                   .WithMany(t => t.TicketsSource)
                   .HasForeignKey(t => t.Id_user_source)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_UserSource_TicketTransction");

            modelBuilder.Entity<TicketsTransction>()
                   .HasOne(t => t.UserTarget)
                   .WithMany(t => t.TicketsTarget)
                   .HasForeignKey(t => t.Id_user_target)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_UserTarget_TicketTransction");

            modelBuilder.Entity<TicketsTransction>()
                   .HasOne(t => t.TicketNavigation)
                   .WithMany(t => t.TicketsTransctions)
                   .HasForeignKey(t => t.Id_ticket)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Ticket_TicketTransction");


        }
    }
}
