using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestPond.BusinessLayer.Models;

namespace TestPond.BusinessLayer.Models
{
    public static class ModelBuildExtensions
    {
        //public static void Seed(this ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TestPond.BusinessLayer.Models.SingleDeviceTestSuiteRun>().HasData(
        //        new SingleDeviceTestSuiteRun
        //        {
        //            Id = 1,
        //            MobileDeviceName = "Galaxy S8",
        //            TestRunReportRaw = "<xml></xml>"
        //        },
        //        new SingleDeviceTestSuiteRun
        //        {
        //            Id = 2,
        //            MobileDeviceName = "iPhone 8 Plus",
        //            TestRunReportRaw = "<xml></xml>"
        //        });
        //}

        public static void Seed(this ModelBuilder modelBuilder)
        {
            //    modelBuilder.Entity<TestPond.BusinessLayer.Models.DeviceTestSuiteCollectionRun>().HasData(
            //        new DeviceTestSuiteCollectionRun()
            //        {
            //            Id = 1,
            //            MobileAppBuild = "Android Beta 3.0",
            //            TestSelectionQuery = "Cat == Regression",
            //            SingleDeviceTestSuiteRuns = new List<SingleDeviceTestSuiteRun>()
            //            {   new SingleDeviceTestSuiteRun
            //                {
            //                    Id = 1,
            //                    MobileDeviceName = "Galaxy S8",
            //                    TestRunReportRaw = "<xml></xml>"
            //                },
            //                new SingleDeviceTestSuiteRun
            //                {
            //                    Id = 2,
            //                    MobileDeviceName = "iPhone8Plus",
            //                    TestRunReportRaw = "<xml></xml>"
            //                }
            //            }
            //        }
            //        );

            modelBuilder.Entity<DeviceTestSuiteCollectionRun>(b =>
            {
                b.HasData(new DeviceTestSuiteCollectionRun()
                {
                    Id = Guid.NewGuid(),
                    MobileAppBuild = new MobileBuild()
                    {
                        //Id = 5,
                        BuildDefinitionId = 10,
                        BuildDate = DateTime.Now.AddDays(-5),
                        BuildNumber = "3.0.0",
                        BuildDefinitionName = "Android Beta",
                        Environment = BuildEnvironment.Beta,
                        Platform = Platform.Android,
                        SourceBranch = "Staging"
                    },
                    TestSelectionQuery = "Cat == Regression",
                });

                b.OwnsMany(e => e.SingleDeviceTestSuiteRuns).HasData(
                    new SingleDeviceTestSuiteRun
                    {
                        Id = Guid.NewGuid(),
                        //MobileDeviceName = "Galaxy S8",
                        MobileDevice = new MobileDevice()
                        {
                            //Id = 101,
                            Id = "iPhone 8Plus",
                            Manufacturer = "Apples"
                        },
                        TestRunReportRaw = "<xml></xml>"
                    }
                    );
            });
        }

        /*
        new SingleDeviceTestSuiteRun
                {
                    Id = 1,
                    MobileDeviceName = "Galaxy S8",
                    TestRunReportRaw = "<xml></xml>"
                },
                new SingleDeviceTestSuiteRun
                {
                    Id = 2,
                    MobileDeviceName = "iPhone 8 Plus",
                    TestRunReportRaw = "<xml></xml>"
                });
        */

        private static DeviceTestSuiteCollectionRun getColl()
        {
            return new DeviceTestSuiteCollectionRun()
            {
                Id = Guid.NewGuid(),
                MobileAppBuild = new MobileBuild()
                {
                    Id = 5,
                    BuildDefinitionId = 10,
                    BuildDate = DateTime.Now.AddDays(-5),
                    BuildNumber = "3.0.0",
                    BuildDefinitionName = "Android Beta",
                    Environment = BuildEnvironment.Beta,
                    Platform = Platform.Android,
                    SourceBranch = "Staging"
                },
                TestSelectionQuery = "Cat == Regression",
                SingleDeviceTestSuiteRuns = new List<SingleDeviceTestSuiteRun>()
                    {   new SingleDeviceTestSuiteRun
                        {
                            Id = Guid.NewGuid(),
                            //MobileDeviceName = "Galaxy S8",
                             MobileDevice = new MobileDevice()
                            {
                                //Id = 101,
                                Id = "iPhone 8Plus",
                                Manufacturer = "Apples"
                            },
                            TestRunReportRaw = "<xml></xml>"
                        },
                        new SingleDeviceTestSuiteRun
                        {
                            Id = Guid.NewGuid(),
                            //MobileDeviceName = "iPhone8Plus",
                             MobileDevice = new MobileDevice()
                            {
                                //Id = 102,
                                Id = "iPhone 8Plus",
                                Manufacturer = "Apples"
                            },
                            TestRunReportRaw = "<xml></xml>"
                        }
                    }
            };
        }

        private static DeviceTestSuiteCollectionRun getColl2()
        {
            return new DeviceTestSuiteCollectionRun()
            {
                Id = Guid.NewGuid(),
                MobileAppBuild = new MobileBuild()
                {
                    Id = 5,
                    BuildDefinitionId = 10,
                    BuildDate = DateTime.Now.AddDays(-5),
                    BuildNumber = "3.0.0",
                    BuildDefinitionName = "Android Beta",
                    Environment = BuildEnvironment.Beta,
                    Platform = Platform.Android,
                    SourceBranch = "Staging"
                },
                TestSelectionQuery = "Cat == Regression",
            };
        }
    }
}
