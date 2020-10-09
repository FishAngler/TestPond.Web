using System;
using System.Xml.Serialization;

namespace TestPond.BusinessLayer.Models.NUnit
{
    public class NUnitTestRun
    {
        [XmlElement(ElementName = "command-line")]
        public string Commandline { get; set; }

        [XmlElement(ElementName = "filter")]
        public Filter Filter { get; set; }

        [XmlElement(ElementName = "test-suite")]
        public NUnitTestsuite Testsuite { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "testcasecount")]
        public int Testcasecount { get; set; }

        [XmlAttribute(AttributeName = "result")]
        public string Result { get; set; }

        [XmlAttribute(AttributeName = "total")]
        public int Total { get; set; }

        [XmlAttribute(AttributeName = "passed")]
        public int Passed { get; set; }

        [XmlAttribute(AttributeName = "failed")]
        public int Failed { get; set; }

        [XmlAttribute(AttributeName = "inconclusive")]
        public int Inconclusive { get; set; }

        [XmlAttribute(AttributeName = "skipped")]
        public int Skipped { get; set; }

        [XmlAttribute(AttributeName = "asserts")]
        public string Asserts { get; set; }

        [XmlAttribute(AttributeName = "engine-version")]
        public string Engineversion { get; set; }

        [XmlAttribute(AttributeName = "clr-version")]
        public string Clrversion { get; set; }

        [XmlAttribute(AttributeName = "start-time")]
        public string Starttime { get; set; }

        [XmlAttribute(AttributeName = "end-time")]
        public string Endtime { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public decimal Duration { get; set; }
    }

    public class Filter
    {
        [XmlElement(ElementName = "test")]
        public string Test { get; set; }

        [XmlElement(ElementName = "method")]
        public string Method { get; set; }

        [XmlElement(ElementName = "cat")]
        public string Category { get; set; }

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
}
