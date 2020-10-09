 using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TestPond.BusinessLayer.Models.NUnit
{
    [XmlRoot(ElementName = "test-case")]
    public class NUnitTestCase
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "fullname")]
        public string Fullname { get; set; }

        [XmlAttribute(AttributeName = "classname")]
        public string Classname { get; set; }

        [XmlAttribute(AttributeName = "methodname")]
        public string Methodname { get; set; }

        [XmlAttribute(AttributeName = "start-time")]
        public string Starttime { get; set; }

        [XmlAttribute(AttributeName = "end-time")]
        public string Endtime { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }

        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlElement(ElementName = "properties")]
        public Properties Properties { get; set; }

        [XmlAttribute(AttributeName = "runstate")]
        public string Runstate { get; set; }

        [XmlElement(ElementName = "output")]
        public string Output { get; set; }

        [XmlAttribute(AttributeName = "result")]
        public string Result { get; set; }

        [XmlAttribute(AttributeName = "seed")]
        public string Seed { get; set; }

        [XmlAttribute(AttributeName = "asserts")]
        public string Asserts { get; set; }

        [XmlElement(ElementName = "assertions")]
        public Assertions Assertions { get; set; }

        [XmlElement(ElementName = "failure")]
        public Failure Failure { get; set; }

        [XmlElement(ElementName = "reason")]
        public Reason Reason { get; set; }

        [XmlElement(ElementName = "attachments")]
        public Attachments Attachments { get; set; }
    }

    [XmlRoot(ElementName = "properties")]
    public class Properties
    {
        [XmlElement(ElementName = "property")]
        public List<Property> Property { get; set; }
    }

    [XmlRoot(ElementName = "property")]
    public class Property
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "failure")]
    public class Failure
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }

        [XmlElement(ElementName = "stack-trace")]
        public string Stacktrace { get; set; }
    }

    [XmlRoot(ElementName = "reason")]
    public class Reason
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }
    }

    [XmlRoot(ElementName = "assertion")]
    public class Assertion
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }

        [XmlElement(ElementName = "stack-trace")]
        public string Stacktrace { get; set; }

        [XmlAttribute(AttributeName = "result")]
        public string Result { get; set; }
    }

    [XmlRoot(ElementName = "assertions")]
    public class Assertions
    {
        [XmlElement(ElementName = "assertion")]
        public Assertion Assertion { get; set; }
    }

    [XmlRoot(ElementName = "attachments")]
    public class Attachments
    {
        [XmlElement(ElementName = "attachment")]
        public List<Attachment> AttachmentList { get; set; }
    }

    [XmlRoot(ElementName = "attachment")]
    public class Attachment
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "filePath")]
        public string FilePath { get; set; }
    }
}
