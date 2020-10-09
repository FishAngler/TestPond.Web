using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestPond.BusinessLayer.Models
{
    //Not being used 
    public class GenericCollectionRunDTO
    {
        public MobileBuild BuildSummary { get; set; }
        public DateTime StartTime { get; set; }
        public string UnitTestFramework { get; set; }
        public List<GenericDeviceRunDTO> DeviceRuns;
    }

    public class GenericDeviceRunDTO
    {
        public long Date { get; set; }
        public string MobileAppBuild { get; set; }
        public string DeviceName { get; set; }
        public string TestSelector { get; set; } //NUnitFilter
        public string TestResultFile { get; set; } //NUnitXml
        public List<GenericTestCaseDTO> TestCaseDTOs { get; set; }
    }

    public class GenericTestCaseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //TODO: Do I need a date here also? (I have one for the DeviceRun)
        public long Date { get; set; }
        public string Fullname { get; set; }
        public string Result { get; set; }
        public string Output { get; set; }
        public GenericFailure Failure { get; set; }
        public GenericReason Reason { get; set; }
        public GenericAttachments Attachments { get; set; }
    }

    public class GenericFailure
    {
        public string Message { get; set; }
        public string Stacktrace { get; set; }
    }

    public class GenericReason
    {
        public string Message { get; set; }
    }

    public class GenericAttachments
    {
        public List<GenericAttachment> AttachmentList { get; set; }
    }

    public class GenericAttachment
    {
        public string Description { get; set; }
        public string FilePath { get; set; }
    }
}
