using System;
using System.Collections.Generic;
using TestPond.BusinessLayer.Models.NUnit;

namespace TestPond.BusinessLayer.Models.DTO
{
    public class NUnitXMLCollectionRunDTO
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public MobileBuild MobileAppBuild { get; set; }

        public string TestSelectionQuery { get; set; }

        public Models.NUnit.Filter TestSelectionFilter{ get; set; }

        public List<NUnitDeviceTestRunDTO> SingleDeviceTestRuns { get; set; }
    }

    public class NUnitDeviceTestRunDTO
    {
        public Guid Id { get; set; }

        public MobileDevice MobileDevice { get; set; }

        public string NUnitXmlBase64 { get; set; }
    }
}
