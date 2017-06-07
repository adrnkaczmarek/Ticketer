using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Ticketer.Database;
using Ticketer.Database.Enums;

namespace Ticketer.Migrations
{
    [DbContext(typeof(TicketContext))]
    [Migration("20170606234828_ExternalResponseSender")]
    partial class ExternalResponseSender
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Ticketer.Database.AutomatedResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Created");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.Property<string>("Match")
                        .IsRequired();

                    b.Property<int>("MaxPriority");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("AutomatedResponses");
                });

            modelBuilder.Entity("Ticketer.Database.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Ticketer.Database.ExternalClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("ExternalClients");
                });

            modelBuilder.Entity("Ticketer.Database.ExternalTicketResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("SenderId");

                    b.Property<int>("TicketId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("TicketId");

                    b.ToTable("ExternalTicketResponse");
                });

            modelBuilder.Entity("Ticketer.Database.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Ticketer.Database.GroupAutomatedResponse", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("AutomatedResponseId");

                    b.HasKey("GroupId", "AutomatedResponseId");

                    b.HasIndex("AutomatedResponseId");

                    b.ToTable("GroupAutomatedResponse");
                });

            modelBuilder.Entity("Ticketer.Database.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Website")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("Ticketer.Database.SourceRouting", b =>
                {
                    b.Property<int>("SourceId");

                    b.Property<int>("GroupId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("CreatedById")
                        .IsRequired();

                    b.HasKey("SourceId", "GroupId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("GroupId");

                    b.ToTable("SourceRoutings");
                });

            modelBuilder.Entity("Ticketer.Database.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignedGroupId");

                    b.Property<string>("AssignedId");

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("Date");

                    b.Property<string>("Description");

                    b.Property<int>("Priority");

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AssignedGroupId");

                    b.HasIndex("AssignedId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Ticketer.Database.TicketResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("SenderId");

                    b.Property<int>("TicketId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketResponse");
                });

            modelBuilder.Entity("Ticketer.Database.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<int>("GroupId");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Ticketer.Database.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Ticketer.Database.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ticketer.Database.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.AutomatedResponse", b =>
                {
                    b.HasOne("Ticketer.Database.User", "CreatedBy")
                        .WithMany("CreatedAutomatedResponses")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.ExternalTicketResponse", b =>
                {
                    b.HasOne("Ticketer.Database.ExternalClient", "Sender")
                        .WithMany("TicketResponses")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ticketer.Database.Ticket", "Ticket")
                        .WithMany("ExternalTicketResponses")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.Group", b =>
                {
                    b.HasOne("Ticketer.Database.Company", "Company")
                        .WithMany("Groups")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.GroupAutomatedResponse", b =>
                {
                    b.HasOne("Ticketer.Database.AutomatedResponse", "AutomatedResponse")
                        .WithMany("GroupAutomatedResponses")
                        .HasForeignKey("AutomatedResponseId");

                    b.HasOne("Ticketer.Database.Group", "Group")
                        .WithMany("GroupAutomatedResponses")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("Ticketer.Database.Source", b =>
                {
                    b.HasOne("Ticketer.Database.Company", "Company")
                        .WithMany("Sources")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.SourceRouting", b =>
                {
                    b.HasOne("Ticketer.Database.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ticketer.Database.Group", "Group")
                        .WithMany("SourceRoutings")
                        .HasForeignKey("GroupId");

                    b.HasOne("Ticketer.Database.Source", "Source")
                        .WithMany("SourceRoutings")
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("Ticketer.Database.Ticket", b =>
                {
                    b.HasOne("Ticketer.Database.Group", "AssignedGroup")
                        .WithMany()
                        .HasForeignKey("AssignedGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ticketer.Database.User", "Assigned")
                        .WithMany()
                        .HasForeignKey("AssignedId");

                    b.HasOne("Ticketer.Database.Company", "Company")
                        .WithMany("Tickets")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("Ticketer.Database.TicketResponse", b =>
                {
                    b.HasOne("Ticketer.Database.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.HasOne("Ticketer.Database.Ticket", "Ticket")
                        .WithMany("TicketResponses")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ticketer.Database.User", b =>
                {
                    b.HasOne("Ticketer.Database.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
