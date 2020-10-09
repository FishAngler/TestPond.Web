using System;
using System.Collections.Generic;
using TestPond.BusinessLayer.Models;

namespace TestPond.BusinessLayer.Repositories
{
    public static class RepositorySeeder
    {
        internal static DeviceTestSuiteCollectionRun GetCollectionSeedData()
        {
            return new DeviceTestSuiteCollectionRun()
            {
                Date = DateTime.Now,
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
                            MobileDevice = new MobileDevice()
                            {
                                Id = "6h456jhj4hg5",
                                Manufacturer = "Samsung",
                                Name = "Galaxy S21"
                            },
                            TestRunReportRaw = "<xml></xml>",
                            TestCaseExecutions = new List<TestCaseExecution>()
                            {
                                new TestCaseExecution()
                                {
                                   //TestCaseId = "FishAngler.UI.Tests.TestSuite.Communities.UserGroups(Android)",
                                   TestCase = new TestCase()
                                   {
                                       Id = "FishAngler.UI.Tests.TestSuite.Communities.UserGroups(Android)"
                                   },
                                   Result = TestResult.Passed,
                                }
                            }
                        },
                        new SingleDeviceTestSuiteRun
                        {
                            Id = Guid.NewGuid(),
                            MobileDevice = new MobileDevice()
                            {
                                Id = "dkj34j4h45",
                                Manufacturer = "Apples",
                                Name = "qa-iphone8Plus"
                            },
                            TestRunReportRaw = "<xml></xml>",
                            TestCaseExecutions = new List<TestCaseExecution>()
                            {
                                new TestCaseExecution()
                                {
                                   //TestCaseId = "FishAngler.UI.Tests.TestSuite.Communities.Dude(Android)",
                                   TestCase = new TestCase()
                                   {
                                       Id = "FishAngler.UI.Tests.TestSuite.Communities.Dude(Android)"
                                   },
                                   Result = TestResult.Failed
                                }
                            }
                        }
                    }
            };
        }

        internal static DeviceTestSuiteCollectionRun GetCollectionSeedDataWithoutTestCases()
        {
            return new DeviceTestSuiteCollectionRun()
            {
                Date = DateTime.Now,
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
                            MobileDevice = new MobileDevice()
                            {
                                Id = "Galaxy S8",
                                Manufacturer = "Samsung"
                            },
                            TestRunReportRaw = "<xml></xml>"
                        },
                        new SingleDeviceTestSuiteRun
                        {
                            Id = Guid.NewGuid(),
                            MobileDevice = new MobileDevice()
                            {
                                Id = "iPhone 8Plus",
                                Manufacturer = "Apples"
                            },
                            TestRunReportRaw = "<xml></xml>"
                        }
                    }
            };
        }
    }
}
