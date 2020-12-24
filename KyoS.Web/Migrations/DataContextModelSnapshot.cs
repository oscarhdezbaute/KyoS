﻿// <auto-generated />
using System;
using KyoS.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KyoS.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KyoS.Web.Data.Entities.ActivityEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("ThemeId");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ClassificationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Classifications");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClinicId");

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<int>("Gender");

                    b.Property<int?>("GroupId");

                    b.Property<string>("MedicalID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("GroupId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Clinic_Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClinicId");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("ThemeId");

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("ThemeId");

                    b.ToTable("Clinics_Themes");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ClinicEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LogoPath");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.DailySessionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Day");

                    b.Property<int?>("GroupId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("DailySessions");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.DiagnosisEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("MTPId");

                    b.HasKey("Id");

                    b.HasIndex("MTPId");

                    b.ToTable("Diagnoses");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.DiagnosisTempEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("DiagnosesTemp");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.FacilitatorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClinicId");

                    b.Property<string>("Codigo");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Facilitators");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.GoalEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AreaOfFocus")
                        .IsRequired();

                    b.Property<int?>("MTPId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.HasIndex("MTPId");

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.GroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Am");

                    b.Property<int?>("FacilitatorId");

                    b.Property<bool>("Pm");

                    b.HasKey("Id");

                    b.HasIndex("FacilitatorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.MTPEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AdmisionDate");

                    b.Property<int?>("ClientId");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Frecuency");

                    b.Property<string>("InitialDischargeCriteria");

                    b.Property<string>("LevelCare");

                    b.Property<DateTime>("MTPDevelopedDate");

                    b.Property<string>("Modality");

                    b.Property<int?>("NumberOfMonths");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("MTPs");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Note_Classification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClassificationId");

                    b.Property<int?>("NoteId");

                    b.HasKey("Id");

                    b.HasIndex("ClassificationId");

                    b.HasIndex("NoteId");

                    b.ToTable("Notes_Classifications");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Note_DailySession_Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId");

                    b.Property<int?>("DailySessionId");

                    b.Property<int?>("NoteId");

                    b.Property<int?>("PlanId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DailySessionId");

                    b.HasIndex("NoteId");

                    b.HasIndex("PlanId");

                    b.ToTable("Notes_DailySessions_Clients");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.NoteEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ActivityId");

                    b.Property<string>("AnswerClient")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("AnswerFacilitator")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Objetive_Classification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClassificationId");

                    b.Property<int?>("ObjetiveId");

                    b.HasKey("Id");

                    b.HasIndex("ClassificationId");

                    b.HasIndex("ObjetiveId");

                    b.ToTable("Objetives_Classifications");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ObjetiveEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOpened");

                    b.Property<DateTime>("DateResolved");

                    b.Property<DateTime>("DateTarget");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int?>("GoalId");

                    b.Property<string>("Intervention")
                        .IsRequired();

                    b.Property<string>("Objetive")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GoalId");

                    b.ToTable("Objetives");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Plan_Classification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClassificationId");

                    b.Property<int?>("PlanId");

                    b.HasKey("Id");

                    b.HasIndex("ClassificationId");

                    b.HasIndex("PlanId");

                    b.ToTable("Plans_Classifications");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.PlanEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.SupervisorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClinicId");

                    b.Property<string>("Code");

                    b.Property<string>("Firm");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ThemeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address")
                        .HasMaxLength(100);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PicturePath");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
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
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ActivityEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ThemeEntity", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ClientEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClinicEntity", "Clinic")
                        .WithMany("Clients")
                        .HasForeignKey("ClinicId");

                    b.HasOne("KyoS.Web.Data.Entities.GroupEntity", "Group")
                        .WithMany("Clients")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Clinic_Theme", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClinicEntity", "Clinic")
                        .WithMany("Clinics_Themes")
                        .HasForeignKey("ClinicId");

                    b.HasOne("KyoS.Web.Data.Entities.ThemeEntity", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.DailySessionEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.GroupEntity", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.DiagnosisEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.MTPEntity", "MTP")
                        .WithMany("Diagnosis")
                        .HasForeignKey("MTPId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.FacilitatorEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClinicEntity", "Clinic")
                        .WithMany("Facilitators")
                        .HasForeignKey("ClinicId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.GoalEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.MTPEntity", "MTP")
                        .WithMany("Goals")
                        .HasForeignKey("MTPId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.GroupEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.FacilitatorEntity", "Facilitator")
                        .WithMany("Groups")
                        .HasForeignKey("FacilitatorId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.MTPEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClientEntity", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Note_Classification", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClassificationEntity", "Classification")
                        .WithMany("NotesClassification")
                        .HasForeignKey("ClassificationId");

                    b.HasOne("KyoS.Web.Data.Entities.NoteEntity", "Note")
                        .WithMany("Classifications")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Note_DailySession_Client", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClientEntity", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("KyoS.Web.Data.Entities.DailySessionEntity", "DailySession")
                        .WithMany()
                        .HasForeignKey("DailySessionId");

                    b.HasOne("KyoS.Web.Data.Entities.NoteEntity", "Note")
                        .WithMany()
                        .HasForeignKey("NoteId");

                    b.HasOne("KyoS.Web.Data.Entities.PlanEntity", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.NoteEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ActivityEntity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId");
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Objetive_Classification", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClassificationEntity", "Classification")
                        .WithMany("ObjetivesClassification")
                        .HasForeignKey("ClassificationId");

                    b.HasOne("KyoS.Web.Data.Entities.ObjetiveEntity", "Objetive")
                        .WithMany("Classifications")
                        .HasForeignKey("ObjetiveId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.ObjetiveEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.GoalEntity", "Goal")
                        .WithMany("Objetives")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.Plan_Classification", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClassificationEntity", "Classification")
                        .WithMany()
                        .HasForeignKey("ClassificationId");

                    b.HasOne("KyoS.Web.Data.Entities.PlanEntity", "Plan")
                        .WithMany("Classifications")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KyoS.Web.Data.Entities.SupervisorEntity", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.ClinicEntity", "Clinic")
                        .WithMany("Supervisors")
                        .HasForeignKey("ClinicId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KyoS.Web.Data.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KyoS.Web.Data.Entities.UserEntity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
