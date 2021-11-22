using Newtonsoft.Json;
using SpirionUITests.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpirionUITests.Fixtures
{
    public class ReportFixture
    {
        public ReportFixture()
        {
            ReportDetails = GetReportDetails();
        }

        public ReportModel ReportDetails { get; set; }
        private ReportModel GetReportDetails()
        {
            string json = File.ReadAllText("Report//ReportConfig.json");
            var report = JsonConvert.DeserializeObject<ReportConfig>(json);
            return report.ReportModel;
        }
    }
}

