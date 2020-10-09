using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestPond.BusinessLayer.Models.NUnit
{
    // Testun > Testsuite(multiple nested levels > Testcase

    [XmlRoot(ElementName = "test-suite")]
    public class NUnitTestsuite
    {
        [XmlElement(ElementName = "test-case")]
        public List<NUnitTestCase> Testcase { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "fullname")]
        public string Fullname { get; set; }

        [XmlAttribute(AttributeName = "classname")]
        public string Classname { get; set; }

        [XmlAttribute(AttributeName = "runstate")]
        public string Runstate { get; set; }

        [XmlAttribute(AttributeName = "testcasecount")]
        public string Testcasecount { get; set; }

        [XmlAttribute(AttributeName = "result")]
        public string Result { get; set; }

        [XmlAttribute(AttributeName = "start-time")]
        public string Starttime { get; set; }

        [XmlAttribute(AttributeName = "end-time")]
        public string Endtime { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }

        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }

        [XmlAttribute(AttributeName = "passed")]
        public string Passed { get; set; }

        [XmlAttribute(AttributeName = "failed")]
        public string Failed { get; set; }

        [XmlAttribute(AttributeName = "warnings")]
        public string Warnings { get; set; }

        [XmlAttribute(AttributeName = "inconclusive")]
        public string Inconclusive { get; set; }

        [XmlAttribute(AttributeName = "skipped")]
        public string Skipped { get; set; }

        [XmlAttribute(AttributeName = "asserts")]
        public string Asserts { get; set; }

        [XmlElement(ElementName = "reason")]
        public Reason Reason { get; set; }

        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }

        [XmlAttribute(AttributeName = "site")]
        public string Site { get; set; }

        [XmlElement(ElementName = "test-suite")]
        public NUnitTestsuite TestSuite { get; set; }

        [XmlElement(ElementName = "failure")]
        public Failure Failure { get; set; }

        [XmlElement(ElementName = "settings")]
        public Settings Settings { get; set; }
    }

    [XmlRoot(ElementName = "environment")]
    public class Environment
    {
        [XmlAttribute(AttributeName = "framework-version")]
        public string Frameworkversion { get; set; }

        [XmlAttribute(AttributeName = "clr-version")]
        public string Clrversion { get; set; }

        [XmlAttribute(AttributeName = "os-version")]
        public string Osversion { get; set; }

        [XmlAttribute(AttributeName = "platform")]
        public string Platform { get; set; }

        [XmlAttribute(AttributeName = "cwd")]
        public string Cwd { get; set; }

        [XmlAttribute(AttributeName = "machine-name")]
        public string Machinename { get; set; }

        [XmlAttribute(AttributeName = "user")]
        public string User { get; set; }

        [XmlAttribute(AttributeName = "user-domain")]
        public string Userdomain { get; set; }

        [XmlAttribute(AttributeName = "culture")]
        public string Culture { get; set; }

        [XmlAttribute(AttributeName = "uiculture")]
        public string Uiculture { get; set; }

        [XmlAttribute(AttributeName = "os-architecture")]
        public string Osarchitecture { get; set; }
    }

    [XmlRoot(ElementName = "setting")]
    public class Setting
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "settings")]
    public class Settings
    {
        [XmlElement(ElementName = "setting")]
        public List<Setting> SettingList { get; set; }
    }
}
