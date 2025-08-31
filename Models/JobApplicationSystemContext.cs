using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication2.Models;

public partial class JobApplicationSystemContext : IdentityDbContext<ApplicationUser>
{
    public JobApplicationSystemContext()
    {
    }

    public JobApplicationSystemContext(DbContextOptions<JobApplicationSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<EducationLevel> EducationLevels { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobLocation> JobLocations { get; set; }

    public virtual DbSet<JobSeeker> JobSeekers { get; set; }

    public virtual DbSet<JobSeekerSkill> JobSeekerSkills { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SavedJob> SavedJobs { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=MINA005;Database=JobApplicationSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // This is needed for Identity tables
        
        // Configure relationship between ApplicationUser and JobSeeker
        modelBuilder.Entity<JobSeeker>()
            .HasOne(js => js.User)
            .WithOne(u => u.JobSeeker)
            .HasForeignKey<JobSeeker>(js => js.UserId);
            
        // Configure relationship between ApplicationUser and Employee
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithOne(u => u.Employee)
            .HasForeignKey<Employee>(e => e.UserId);
            
        // Rename IdentityRole table to avoid conflicts with our custom Role table
        modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
        
        // Use new keyword to resolve conflict with Identity's Roles property
        modelBuilder.Entity<Role>().ToTable("AppRoles");
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07F64AD973");

            entity.HasIndex(e => e.JobId, "IX_Applications_JobId");

            entity.HasIndex(e => e.JobSeekerId, "IX_Applications_JobSeekerId");

            entity.HasIndex(e => e.StatusId, "IX_Applications_StatusId");

            entity.Property(e => e.AppliedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PreviousCompany).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Job).WithMany(p => p.Applications)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__Applicati__JobId__03F0984C");

            entity.HasOne(d => d.JobSeeker).WithMany(p => p.Applications)
                .HasForeignKey(d => d.JobSeekerId)
                .HasConstraintName("FK__Applicati__JobSe__05D8E0BE");

            entity.HasOne(d => d.Status).WithMany(p => p.Applications)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Applicati__Statu__04E4BC85");
        });

        modelBuilder.Entity<ApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07FA75BE17");

            entity.HasIndex(e => e.StatusName, "UK_ApplicationStatuses_StatusName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Branches__3214EC07669CCDC0");

            entity.HasIndex(e => e.CompanyId, "IX_Branches_CompanyId");

            entity.Property(e => e.BranchLocation).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Branches__Compan__5CD6CB2B");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Companie__3214EC07D00C2DD8");

            entity.HasIndex(e => e.IndustryId, "IX_Companies_IndustryId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.HeadquartersLocation).HasMaxLength(200);
            entity.Property(e => e.LogoUrl).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Website).HasMaxLength(200);

            entity.HasOne(d => d.Industry).WithMany(p => p.Companies)
                .HasForeignKey(d => d.IndustryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Companies__Indus__5812160E");
        });

        modelBuilder.Entity<EducationLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC072F1503BF");

            entity.HasIndex(e => e.LevelName, "UK_EducationLevels_LevelName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LevelName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07F84F985E");

            entity.HasIndex(e => e.CompanyId, "IX_Employees_CompanyId");
            entity.HasIndex(e => e.RoleId, "IX_Employees_RoleId");
            entity.HasIndex(e => e.UserId, "IX_Employees_UserId");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PositionTitle).HasMaxLength(100);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(300);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasMaxLength(450); // Match the ApplicationUser Id length

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Employees__Compa__6383C8BA");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__RoleI__628FA481");
        });

        modelBuilder.Entity<EmploymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employme__3214EC074A7EA0EE");

            entity.HasIndex(e => e.TypeName, "UK_EmploymentTypes_TypeName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TypeName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Industri__3214EC07EDD7BE5F");

            entity.HasIndex(e => e.IndustryType, "UK_Industries_IndustryType").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IndustryType).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs__3214EC07133CAC2F");

            entity.HasIndex(e => e.CompanyId, "IX_Jobs_CompanyId");

            entity.HasIndex(e => e.EmploymentTypeId, "IX_Jobs_EmploymentTypeId");

            entity.HasIndex(e => e.JobStatusId, "IX_Jobs_JobStatusId");

            entity.HasIndex(e => e.LocationId, "IX_Jobs_LocationId");

            entity.HasIndex(e => e.PostedByEmployeeId, "IX_Jobs_PostedByEmployeeId");

            entity.HasIndex(e => e.PostedDate, "IX_Jobs_PostedDate").IsDescending();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.JobTitle).HasMaxLength(100);
            entity.Property(e => e.PostedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SalaryFrom).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SalaryTo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Company).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Jobs__CompanyId__73BA3083");

            entity.HasOne(d => d.EmploymentType).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.EmploymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__Employment__76969D2E");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__JobStatusI__75A278F5");

            entity.HasOne(d => d.Location).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jobs__LocationId__778AC167");

            entity.HasOne(d => d.PostedByEmployee).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.PostedByEmployeeId)
                .HasConstraintName("FK__Jobs__PostedByEm__74AE54BC");
        });

        modelBuilder.Entity<JobLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobLocat__3214EC0739F50B72");

            entity.HasIndex(e => e.LocationName, "UK_JobLocations_LocationName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LocationName).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<JobSeeker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobSeeke__3214EC07DBB76B89");

            entity.HasIndex(e => e.EducationLevelId, "IX_JobSeekers_EducationLevelId");
            entity.HasIndex(e => e.UserId, "IX_JobSeekers_UserId");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LinkedInUrl).HasMaxLength(300);
            entity.Property(e => e.PortfolioUrl).HasMaxLength(300);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(300);
            entity.Property(e => e.ResumeUrl).HasMaxLength(300);
            entity.Property(e => e.UniversityName).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasMaxLength(450); // Match the ApplicationUser Id length

            entity.HasOne(d => d.EducationLevel).WithMany(p => p.JobSeekers)
                .HasForeignKey(d => d.EducationLevelId)
                .HasConstraintName("FK__JobSeeker__Educa__68487DD7");
        });

        modelBuilder.Entity<JobSeekerSkill>(entity =>
        {
            entity.HasKey(e => new { e.JobSeekerId, e.SkillId }).HasName("PK__JobSeeke__E4EB33B4261818DB");

            entity.HasIndex(e => e.JobSeekerId, "IX_JobSeekerSkills_JobSeekerId");

            entity.HasIndex(e => e.SkillId, "IX_JobSeekerSkills_SkillId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.JobSeeker).WithMany(p => p.JobSeekerSkills)
                .HasForeignKey(d => d.JobSeekerId)
                .HasConstraintName("FK__JobSeeker__JobSe__6D0D32F4");

            entity.HasOne(d => d.Skill).WithMany(p => p.JobSeekerSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK__JobSeeker__Skill__6E01572D");
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobStatu__3214EC072AD4AF93");

            entity.HasIndex(e => e.StatusName, "UK_JobStatuses_StatusName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0783825276");

            entity.HasIndex(e => e.RoleName, "UK_Roles_RoleName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<SavedJob>(entity =>
        {
            entity.HasKey(e => new { e.JobSeekerId, e.JobId }).HasName("PK__SavedJob__B94753A0309EB6C7");

            entity.HasIndex(e => e.JobId, "IX_SavedJobs_JobId");

            entity.HasIndex(e => e.JobSeekerId, "IX_SavedJobs_JobSeekerId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SavedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Job).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__SavedJobs__JobId__7E37BEF6");

            entity.HasOne(d => d.JobSeeker).WithMany(p => p.SavedJobs)
                .HasForeignKey(d => d.JobSeekerId)
                .HasConstraintName("FK__SavedJobs__JobSe__7D439ABD");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Skills__3214EC07D606DCE4");

            entity.HasIndex(e => e.SkillName, "UK_Skills_SkillName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SkillName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
