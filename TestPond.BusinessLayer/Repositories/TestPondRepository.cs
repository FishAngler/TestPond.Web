using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Models;

namespace TestPond.BusinessLayer.Repositories
{
    public class TestPondRepository
    {
        private readonly TestPondContext _ctx;
        private readonly ILogger<TestPondContext> _logger;

        public TestPondRepository(TestPondContext ctx, ILogger<TestPondContext> logger)
        {
            _ctx = ctx;
            _logger = logger;

            //SEED Database (WRONG WAY)
            ctx.Database.EnsureCreated();

            SeedDataIfEmpty();

            var testCases = ctx.TestCases.ToList();
            var collRuns = ctx.DeviceTestSuiteCollectionRuns.ToList();
        }

        private void SeedDataIfEmpty()
        {
            if (!_ctx.DeviceTestSuiteCollectionRuns.Any())
            {
                _ctx.Add<TestCase>(new TestCase()
                { Id = "FishAngler.UI.Tests.TestSuite.Communities.FooBar(Android)" });

                _ctx.Add<DeviceTestSuiteCollectionRun>(RepositorySeeder.GetCollectionSeedData());
                _ctx.SaveChanges();
            }
        }

        #region MobileBuild
        public async Task<List<MobileBuild>> GetMobileBuilds()
        {
            return await _ctx.MobileBuilds
                .Include(x => x.DeviceTestSuiteCollectionRuns)
                .ThenInclude(x => x.SingleDeviceTestSuiteRuns)
                .ToListAsync();
        }
        #endregion

        #region MobileDevice
        async Task<MobileDevice> GetMobileDevice(string id)
        {
            return await _ctx.MobileDevices.SingleOrDefaultAsync(x => x.Id == id);
        }
        async Task<List<MobileDevice>> GetMobileDevices()
        {
            return await _ctx.MobileDevices.ToListAsync();
        }
        #endregion

        #region DeviceTestSuiteCollectionRun
        public async Task<DeviceTestSuiteCollectionRun> GetCollectionRun(Guid id)
        {
            var coll = await _ctx.DeviceTestSuiteCollectionRuns
                .Include(a => a.MobileAppBuild)
                .Include(a => a.SingleDeviceTestSuiteRuns).ThenInclude(x => x.MobileDevice)
                .Include(a => a.SingleDeviceTestSuiteRuns).ThenInclude(x => x.TestCaseExecutions).ThenInclude(x => x.TestCase)
                .Include(a => a.SingleDeviceTestSuiteRuns).ThenInclude(x => x.TestCaseExecutions).ThenInclude(x => x.Attachments)
                .SingleOrDefaultAsync(x => x.Id == id);

            return coll;
        }

        public async Task<List<DeviceTestSuiteCollectionRun>> GetCollectionRuns()
        {
            var coll = await _ctx.DeviceTestSuiteCollectionRuns
                .Include(a => a.MobileAppBuild)
                .Include(a => a.SingleDeviceTestSuiteRuns).ThenInclude(x => x.TestCaseExecutions)
                .Include(a => a.SingleDeviceTestSuiteRuns).ThenInclude(x => x.MobileDevice)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return coll;
        }

        public void AddCollectionRun(DeviceTestSuiteCollectionRun collectionRun)
        {
            //MobileBuild(New / Existing) - ID always comes from client - must check for match
            var existingMobileBuild = _ctx.MobileBuilds
               .Where(x => x.Id == collectionRun.MobileAppBuild.Id)
               .Include(x => x.DeviceTestSuiteCollectionRuns)
               .SingleOrDefault();

            if (existingMobileBuild != null)
            {
                _ctx.MobileBuilds.Attach(existingMobileBuild);
            }
            else
            {
                var mobileEntry = _ctx.Entry(collectionRun.MobileAppBuild).State = EntityState.Added;
            }

            //DeviceCollectionRun(Always new)
            _ctx.Entry(collectionRun).State = EntityState.Added;

            foreach (var singleRun in collectionRun.SingleDeviceTestSuiteRuns)
            {
                //SingleDeviceTestRuns (Always new)
                _ctx.Entry(singleRun).State = EntityState.Added;

                var foo = singleRun.MobileDevice;

                //Mobile Device (New/Existing) - ID always from client - must check for match
                var existingMobileDevice = _ctx.MobileDevices
                 .Where(x => x.Id == singleRun.MobileDevice.Id)
                 .SingleOrDefault();

                if (existingMobileDevice != null)
                {
                    var state = _ctx.Entry(existingMobileDevice).State;
                    _ctx.MobileDevices.Attach(existingMobileDevice);
                }
                else
                {
                    var mobileEntry = _ctx.Entry(singleRun.MobileDevice).State = EntityState.Added;
                }

                //Test Case Executions and Test Cases
                foreach (var tcExecution in singleRun.TestCaseExecutions)
                {
                    //Test Case Execution (Always new) - nothing to do here?
                    _ctx.Entry(tcExecution).State = EntityState.Added;

                    //Handle any existing Test Cases
                    string existingId = tcExecution.TestCaseId;

                    var existingTestCase = _ctx.TestCases
                        .Where(x => x.Id == existingId)
                        .SingleOrDefault();

                    if (existingTestCase != null)
                    {
                        var state = _ctx.Entry(existingTestCase).State;
                        _ctx.TestCases.Attach(existingTestCase);
                    }
                    else
                    {
                        var mobileEntry = _ctx.Entry(tcExecution.TestCase).State = EntityState.Added;
                        _ctx.SaveChanges();
                    }
                }
            }

            _ctx.SaveChanges();

            PrintCounts();
        }
        #endregion

        #region SingleDeviceRun
        public async Task<SingleDeviceTestSuiteRun> GetSingleDeviceTestSuiteRun(Guid id)
        {
            var result = await _ctx.SingleDeviceTestSuiteRuns
                .Include(x => x.DeviceTestSuiteCollectionRun).ThenInclude(x => x.MobileAppBuild)
                .Include(a => a.MobileDevice)
                .Include(a => a.TestCaseExecutions).ThenInclude(x => x.TestCase)
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }
        #endregion

        #region TestCases
        public async Task<List<TestCase>> GetTestCases()
        {
            var testCases = await _ctx.TestCases.ToListAsync();
            return testCases;
        }

        private IEnumerable<TestCase> InvertTestCaseExecutions(List<TestCaseExecution> tcExecs)
        {
            var testCases = new Collection<TestCase>();

            foreach (var exec in tcExecs)
            {
                var testCase = exec.TestCase;
                testCases.Add(testCase);
            }

            var distinctTestCases = testCases.Distinct();

            foreach (var tc in distinctTestCases)
            {
                foreach (var exec in tcExecs)
                {
                    if (exec.TestCaseId == tc.Id)
                    {
                        if(tc.TestCaseRuns == null)
                        {
                            tc.TestCaseRuns = new List<TestCaseExecution>();
                        }
                        tc.TestCaseRuns.Add(exec);
                    }
                }          
            }

            return distinctTestCases;
        }

        public async Task<IEnumerable<IGrouping<string, TestCase>>> GetTestCaseSummary(Guid id)
        {
            var testExecutions = _ctx.DeviceTestSuiteCollectionRuns
                .Include(x => x.SingleDeviceTestSuiteRuns).ThenInclude(x => x.TestCaseExecutions).ThenInclude(x => x.TestCase)
                .Where(x => x.Id == id)
                .SingleOrDefault()

                .SingleDeviceTestSuiteRuns

                .SelectMany(x => x.TestCaseExecutions);
            //This is where I would lose the TestCaseRuns:
            //.Select(x => x.TestCase);

            var testCases = InvertTestCaseExecutions(testExecutions.ToList());

            var testCasesGroupedByExecutionClassName = testCases
                .GroupBy(x => x.TestCaseRuns.FirstOrDefault().ClassName)
                .OrderBy(x => x.Key);

            return testCasesGroupedByExecutionClassName;
        }

        public async Task<TestCaseExecution> GetTestCaseExecution(int id)
        {
            var result = await _ctx.TestCaseExecutions
                .Include(x => x.TestCase)
                .Include(x => x.SingleDeviceTestSuiteRun)
                .ThenInclude(x => x.MobileDevice)
                .Include(x => x.SingleDeviceTestSuiteRun)
                .ThenInclude(x => x.DeviceTestSuiteCollectionRun)
                .ThenInclude(x => x.MobileAppBuild)
                .Include(x => x.Attachments)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }

        #endregion

        public void AddTestCaseAttachment(DeviceTestSuiteCollectionRun collRun, string fileName, Guid singleDeviceRunId, string testCaseName)
        {
            var matchingTestCaseExec = collRun.SingleDeviceTestSuiteRuns
                                .Where(x => x.Id == singleDeviceRunId)
                                .Single().TestCaseExecutions
                                .Where(x => x.TestCase.Id == testCaseName).Single();

            var attachment = new TestCase_Attachment() { Description = "TestFailureScreenshot", FilePath = fileName };

            matchingTestCaseExec.Attachments.Add(attachment);
            _ctx.SaveChanges();
        }

        private void PrintCounts()
        {
            //SANITY CHECK
            var builds = _ctx.MobileBuilds.ToList();

            var existingbuildCount = builds.Count();
            var existingCollRunsCount = _ctx.DeviceTestSuiteCollectionRuns.ToList().Count();
            var existingSingleRunCount = _ctx.SingleDeviceTestSuiteRuns.ToList().Count();
            var existingDevicesCount = _ctx.MobileDevices.ToList().Count();

            _logger.LogInformation($"Builds {existingbuildCount} - Collection Runs {existingCollRunsCount} - Device Runs {existingSingleRunCount} - Devices {existingDevicesCount}");
        }
    }
}
