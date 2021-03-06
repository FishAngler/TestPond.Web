﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestPond.BusinessLayer.Models;
using TestPond.BusinessLayer.Models.DTO;
using TestPond.BusinessLayer.Models.NUnit;
using TestPond.BusinessLayer.Repositories;
using TestPond.BusinessLayer.Services.NUnitDeserialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace TestPond.BusinessLayer.Services.CollectionRun
{
    public class CollectionRunService
    {
        private readonly ILogger<CollectionRunService> _logger;
        private readonly INUnitXMLDeserializer _deserializer;
        private readonly IHostEnvironment _env;
        private readonly TestPondRepository _repo;
        private readonly IMapper _mapper;

        public CollectionRunService(TestPondRepository repo, INUnitXMLDeserializer deserializer, IMapper mapper,
            IHostEnvironment env, ILogger<CollectionRunService> logger)
        {
            _deserializer = deserializer;
            _repo = repo;
            _mapper = mapper;
            _env = env;
            _logger = logger;
        }

        #region MobileBuild
        public async Task<List<MobileBuild>> GetMobileBuilds()
        {
            return await _repo.GetMobileBuilds();
        }
        #endregion

        #region DeviceTestSuiteCollectionRun 
        public async Task<DeviceTestSuiteCollectionRun> GetCollectionRun(Guid id)
        {
            return await _repo.GetCollectionRun(id);
        }
        public async Task<List<DeviceTestSuiteCollectionRun>> GetCollectionRuns()
        {
            return await _repo.GetCollectionRuns();
        }

        //TODO: Move DTO Conversion and NUnit Deserialization into a separate class
        public void AddCollectionRun(NUnitXMLCollectionRunDTO dto)
        {
            var collectionRun = ConvertfromCollectionDTO(dto);

            _repo.AddCollectionRun(collectionRun);

            _logger.LogInformation($"{nameof(CollectionRunService)}: Collection Run SAVED at {DateTime.Now.ToLocalTime()}!");
        }
        #endregion

        #region TestCaseAttachments
        public async Task SaveTestCaseAttachments(IFormFileCollection screenshots)
        {
            string screenshotsDir = Path.Combine(_env.ContentRootPath, "wwwroot", "Uploads", "Screenshots");
            SaveFilesToDirectory(screenshots, screenshotsDir);
            await AddAttachmentsToTestCaseExecutions(screenshots);
        }

        private async Task AddAttachmentsToTestCaseExecutions(IFormFileCollection screenshots)
        {
            var imgFilePathParts = screenshots.FirstOrDefault().FileName.Split("--");

            var collRunId = Guid.Parse(imgFilePathParts.First());
            var collRun = await GetCollectionRun(collRunId);

            foreach (var img in screenshots)
            {
                var parts = img.FileName.Split("--");

                var singleDeviceRunId = Guid.Parse(parts.ElementAt(1));
                var testCaseName = parts.ElementAt(2).Replace(new FileInfo(img.FileName).Extension, "").Replace("-", ".");                

                _repo.AddTestCaseAttachment(collRun, img.FileName, singleDeviceRunId, testCaseName);
            }
        }

        private static void SaveFilesToDirectory(IFormFileCollection screenshots, string destinationDirectory)
        {
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            if (screenshots.Count() > 0)
            {
                foreach (var file in screenshots)
                {
                    //Will overwrite any existing file with the same FileName
                    string newFile = Path.Combine(destinationDirectory, $"{file.FileName}");

                    using (FileStream fs = File.Create(newFile))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
        }
        #endregion

        #region SingleDeviceRun
        public async Task<SingleDeviceTestSuiteRun> GetSingleDeviceTestSuiteRun(Guid id)
        {
            var result = await _repo.GetSingleDeviceTestSuiteRun(id);
            return result;
        }
        #endregion

        #region TestCaseSummary
        //public Task<IEnumerable<IGrouping<string, TestCaseExecution>>> GetTestCaseSummary(Guid id)
        //{
        //    var res = _repo.GetTestCaseSummary(id);
        //    return res;
        //}

        public Task<IEnumerable<IGrouping<string, TestCase>>> GetTestCaseSummary(Guid id)
        {
            var res = _repo.GetTestCaseSummary(id);
            return res;
        }
        #endregion

        #region TestCaseExecution
        public async Task<TestCaseExecution> GetTestCaseExecution(int id)
        {
            var result = await _repo.GetTestCaseExecution(id);
            return result;
        }
        #endregion

        #region DTOConversion
        private DeviceTestSuiteCollectionRun ConvertfromCollectionDTO(NUnitXMLCollectionRunDTO collectionRunDTO)
        {
            //var TestSelectionFilter = _mapper.Map<Models.Filter>(collectionRunDTO.TestSelectionFilter);
            var coll = new DeviceTestSuiteCollectionRun()
            {
                Id = collectionRunDTO.Id,
                Date = collectionRunDTO.Date,
                MobileAppBuild = collectionRunDTO.MobileAppBuild,
                TestSelectionQuery = collectionRunDTO.TestSelectionQuery,
            };

            List<SingleDeviceTestSuiteRun> testRuns = new List<SingleDeviceTestSuiteRun>();

            foreach (var testRunDTO in collectionRunDTO.SingleDeviceTestRuns)
            {
                var testRun = ConvertFromSingleDeviceDTO(testRunDTO, coll);
                testRuns.Add(testRun);
            }

            coll.SingleDeviceTestSuiteRuns = testRuns;

            return coll;
        }

        private SingleDeviceTestSuiteRun ConvertFromSingleDeviceDTO(NUnitDeviceTestRunDTO testRunDTO, DeviceTestSuiteCollectionRun deviceTestSuiteCollectionRun)
        {
            var base64 = testRunDTO.NUnitXmlBase64;
            string nunitXml = EncodingUtilities.DecodeStringFromBase64(base64);

            NUnitReportSummary nUnitReportSummary = _deserializer.GetNunitReportSummmary(nunitXml);

            var singleRun = new SingleDeviceTestSuiteRun()
            {
                Id = testRunDTO.Id,
                MobileDevice = testRunDTO.MobileDevice,
                TestRunReportRaw = nunitXml,

                DeviceTestSuiteCollectionRun = deviceTestSuiteCollectionRun,
            };

            //TODO: Add Deserialized NUnit Data from "NUnitTestRun testrun" 
            //NUnitTestResult = testrun
            //Use simple, scaled-down Test Case / Test Case Execution models for now:
            singleRun.TestCaseExecutions = GetTestCaseDataFromDTO(singleRun, nUnitReportSummary);

            return singleRun;
        }
        private List<TestCaseExecution> GetTestCaseDataFromDTO(SingleDeviceTestSuiteRun testRun, NUnitReportSummary nUnitReportSummary)
        {
            var testcases = new List<Models.TestCase>();
            var testExecutions = new List<Models.TestCaseExecution>();

            //TODO: implement attachments
            var attachments = new List<Models.TestCase_Attachment>();

            //Loop though NUnit TestCases and create TestPond Models of Test Case and Execution
            foreach (var tc in nUnitReportSummary.Testcases)
            {
                var testPondTestCase = new Models.TestCase()
                {
                    Id = tc.Fullname
                };

                Enum.TryParse(tc.Result, out TestResult result);

                var testPondTestCaseExec = new Models.TestCaseExecution()
                {
                    TestCaseId = testPondTestCase.Id,
                    ClassName = tc.Classname,
                    TestCase = testPondTestCase,
                    SingleDeviceTestSuiteRunId = testRun.Id,
                    SingleDeviceTestSuiteRun = testRun,
                    Result = result,
                    ConsoleOutput = tc.Output,
                    FailureMessage = tc.Failure?.Message,
                    FailureStackTrace = tc.Failure?.Stacktrace
                };

                testcases.Add(testPondTestCase);
                testExecutions.Add(testPondTestCaseExec);
            }

            return testExecutions;
        }
        #endregion

        private string saveBase64Image(string base64Image)
        {
            //Depending on if you want the byte array or a memory stream, you can use the below. 
            var imageDataByteArray = Convert.FromBase64String(base64Image);

            //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
            var memoryImageDataStream = new MemoryStream(imageDataByteArray);

            //TODO Use a subfolder based on the Collection Run / DeviceRun....
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "Screenshots");
            var destFilePath = Path.Combine(uploadsFolder, $"Screenshot{DateTime.Now.Ticks}.png");

            var destStream = new FileStream(destFilePath, FileMode.CreateNew);

            memoryImageDataStream.WriteTo(destStream);

            destStream.Close();
            destStream.Dispose();

            memoryImageDataStream.Close();
            memoryImageDataStream.Dispose();

            var uploadContents = _env.ContentRootFileProvider.GetDirectoryContents(Path.Combine("wwwroot", "Screenshots"));

            foreach (var file in uploadContents)
            {
                Console.WriteLine(file.Name + " " + file.PhysicalPath);
            }

            return destFilePath;
        }
    }
}

