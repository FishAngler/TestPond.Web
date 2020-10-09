using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPond.BusinessLayer.Models.NUnit
{
    public class NUnitReportSummary
    {
        // Entire from NUnit XML Report object
        public NUnitTestRun Testrun;

        public IEnumerable<NUnitTestCase> Testcases;

        // Actual Test Cases "grouped by class name"
        // - which "flattens" out the multiple, unnecessary "deeply nested" Test Suite layers in XML Tree
        public IEnumerable<IGrouping<string, NUnitTestCase>> TestCasesGroupedByClassName;
    }
}
