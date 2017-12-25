using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Remont.Tests.Context
{
    public class RemontTestContext : DbContext, IRemontTestContext
    {
        public RemontTestContext()
            : base("TestConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Skill> skillsPrvt { get; set; }
        public DbQuery<Skill> Skills
        {
            get
            {
                return Set<Skill>().AsNoTracking();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                        .HasOptional(e => e.Worker).WithRequired(e => e.UserProfile);

            modelBuilder.Entity<Worker>().HasMany(w => w.Skills).WithMany(w => w.Workers).Map(m => m.ToTable("WorkerSkills"));

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            var skill1 = new Skill() { Code = "Live" };
            var skill2 = new Skill() { Code = "VeryLive" };
            skillsPrvt.Add(skill1);
            skillsPrvt.Add(skill2);
            SaveChanges();
        }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

        public virtual Worker Worker { get; set; }
    }

    [Table("Workers")]
    public class Worker
    {
        [Key]
        public int WorkerId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual IList<Skill> Skills { get; set; }
    }

    [Table("Skill")]
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }

        [Required]
        public string Code { get; set; }

        public virtual IList<Worker> Workers { get; set; }
    }
}