using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestPond.BusinessLayer.Models.NUnit;

namespace TestPond.BusinessLayer.Services.NUnitDeserialization
{
    public interface INUnitXMLDeserializer
    {
        NUnitTestRun GetTestRun(string xml);
        NUnitReportSummary GetNunitReportSummmary(string nunitXml);
    }

    public class NUnitXMLDeserializer : INUnitXMLDeserializer
    {
        public NUnitReportSummary GetNunitReportSummmary(string xml)
        {
            NUnitReportSummary reportModel = new NUnitReportSummary()
            {
                Testrun = GetTestRun(xml),
                Testcases = GetTestCaseList(xml),
                TestCasesGroupedByClassName = GetTestCasesByOrderedByClassName(xml)
            };

            return reportModel;
        }

        public NUnitTestRun GetTestRun(string xml)
        {
            NUnitTestRun testrun = null;

            XmlRootAttribute xroot = new XmlRootAttribute();
            xroot.ElementName = "test-run";
            xroot.IsNullable = true;

            testrun = (FromXml(xml, typeof(NUnitTestRun), xroot) as NUnitTestRun);
            return testrun;
        }

        //Creates an object from an XML string.
        public static object FromXml(string xml, System.Type ObjType, XmlRootAttribute xmlRoot)
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(ObjType, xmlRoot);

            StringReader stringReader;
            stringReader = new StringReader(xml);

            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);

            object obj;
            obj = serializer.Deserialize(xmlReader);

            xmlReader.Close();
            stringReader.Close();

            return obj;
        }

        //What if XML File does not have any Test Cases?
        public List<NUnitTestCase> GetTestCaseList(string xml)
        {
            var xmlDoc = XDocument.Parse(xml);
            List<XElement> testCaseElements = xmlDoc.Descendants("test-case").ToList();

            List<NUnitTestCase> testcases = new List<NUnitTestCase>();

            foreach (XElement xmlTestCaseElement in testCaseElements)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NUnitTestCase));
                testcases.Add((NUnitTestCase)serializer.Deserialize(xmlTestCaseElement.CreateReader()));
            }

            return testcases;
        }

        //What if XML File does not have any Class Names to Group By?
        public IEnumerable<IGrouping<string, NUnitTestCase>> GetTestCasesByOrderedByClassName(string xml )
        {
            List<NUnitTestCase> testcases = GetTestCaseList(xml);

            IEnumerable<IGrouping<string, NUnitTestCase>> testCasesGroupedByClassName
                = testcases.GroupBy(x => x.Classname);

            return testCasesGroupedByClassName;
        }
    }
}
