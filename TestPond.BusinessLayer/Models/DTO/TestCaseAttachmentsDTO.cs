using System;
using Microsoft.AspNetCore.Http;

namespace TestPond.BusinessLayer.Models.DTO
{
    public class TestCaseAttachmentsDTO
    {
        public IFormFileCollection TestFailureScreenshots { get; set; }
    }
}
