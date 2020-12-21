using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TestPond.BusinessLayer.Models
{
    public class TestPondContext : DbContext
    {
        public DbSet<MobileBuild> MobileBuilds { get; set; }

        public DbSet<DeviceTestSuiteCollectionRun> DeviceTestSuiteCollectionRuns { get; set; }

        public DbSet<SingleDeviceTestSuiteRun> SingleDeviceTestSuiteRuns { get; set; }
        public DbSet<MobileDevice> MobileDevices{ get; set; }

        public DbSet<TestCaseExecution> TestCaseExecutions { get; set; }
        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<TestCase_Attachment> TestCase_Attachments { get; set; }

        public TestPondContext(DbContextOptions<TestPondContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO:Remove this
            //optionsBuilder.UseSqlite("Filename=TestPond.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Seed();
        }
    }

    public class MobileBuild 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public Platform Platform { get; set; }
        public BuildEnvironment Environment { get; set; }
        public int BuildDefinitionId { get; set; }
        public string BuildDefinitionName { get; set; }
        public string BuildNumber { get; set; }
        public DateTime BuildDate { get; set; }
        public string SourceBranch { get; set; }
        public string FullName { get; }

        public virtual List<DeviceTestSuiteCollectionRun> DeviceTestSuiteCollectionRuns{ get; set; }

        public override string ToString()
        {
            return $"{Id} - {Platform} - {Environment} - {BuildNumber} - {BuildDate.ToString()} - {SourceBranch}";
        }
    }

    public class Filter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Test { get; set; }
        public string Method { get; set; }
        public string Category { get; set; }

        public virtual List<DeviceTestSuiteCollectionRun> DeviceTestSuiteCollectionRuns { get; set; }

        public KeyValuePair<string, string> Summary() 
        {
            return new KeyValuePair<string, string>("foo", "bar");
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Test))
            {
                return $"Test - {Test}";
            }

            else if (!string.IsNullOrEmpty(Method))
            {
                return $"Method - {Method}";
            }

            else if (!string.IsNullOrEmpty(Category))
            {
                return $"Category - {Category}";
            }

            else
            {
                return "Unknown NUnit Filter Type";
            }
        }
    }

    [Table("DeviceTestSuiteCollectionRun")]
    public class DeviceTestSuiteCollectionRun
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        //TODO: Implement Platform
        public Platform Platform { get; set; }

        public int MobileAppBuildId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(MobileAppBuildId))]
        public MobileBuild MobileAppBuild { get; set; }

        public string TestSelectionQuery { get; set; }

        public ICollection<SingleDeviceTestSuiteRun> SingleDeviceTestSuiteRuns { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class MobileDevice
    {
        [Key]
        public string Id { get; set; }

        public Platform Platform { get; set; }

        public string Manufacturer { get; set; }

        public string ModelNumber { get; set; }

        //Could be the Unique Name for iOS or Model Number for Android
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<SingleDeviceTestSuiteRun> SingleDeviceTestSuiteRuns { get; set; }
    }

    public class SingleDeviceTestSuiteRun
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public virtual string MobileDeviceId  { get; set; }

        [ForeignKey(nameof(MobileDeviceId))]
        public virtual MobileDevice MobileDevice { get; set; }

        public string TestRunReportRaw { get; set; }

        public Guid DeviceTestSuiteCollectionRunId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DeviceTestSuiteCollectionRunId))]
        public virtual DeviceTestSuiteCollectionRun DeviceTestSuiteCollectionRun { get; set; }

        public List<TestCaseExecution> TestCaseExecutions { get; set; }
    }

    public class TestCase 
    {
        [Key]
        //TODO: Implement TestCase IDs in UI Test Assembly
        // Just use the fully-qualified name for now (not the best practice)
        public string Id { get; set; }

        public string Description { get; set; }

        public List<TestCaseExecution> TestCaseRuns;  
    }

    public class TestCaseComparer : IEqualityComparer<TestCase>
    {
        public bool Equals(TestCase x, TestCase y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(TestCase obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class TestCaseExecution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ClassName { get; set; }

        public Guid SingleDeviceTestSuiteRunId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(SingleDeviceTestSuiteRunId))]
        public virtual SingleDeviceTestSuiteRun SingleDeviceTestSuiteRun { get; set; }

        public string TestCaseId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(TestCaseId))]
        public virtual TestCase TestCase{ get; set; }

        public TestResult Result { get; set; }

        public string ConsoleOutput { get; set; }

        public string FailureMessage { get; set; }

        public string FailureStackTrace { get; set; }

        public List<TestCase_Attachment> Attachments { get; set; }

        public List<TestCase_Property> Properties { get; set; }
    }

    public class TestCase_Attachment
    {
        //TODO: Decide if this should be a GUID also
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TestCaseExecutionId { get; set; } 

        [JsonIgnore]
        [ForeignKey(nameof(TestCaseExecutionId))]
        public virtual TestCaseExecution TestCaseExecution { get; set; }

        public string Description { get; set; }

        public string FilePath { get; set; }
    }

    public class TestCase_Property
    {
        //TODO: Decide if this should be a GUID also
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TestCaseExecutionId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(TestCaseExecutionId))]
        public virtual TestCaseExecution TestCaseExecution { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }

    public enum TestResult
    {
        Passed,
        Failed,
        Skipped
    }
}
