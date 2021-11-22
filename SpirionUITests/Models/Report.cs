using System;
using System.Collections.Generic;
using System.Text;

namespace SpirionUITests.Models
{
    public class ReportModel
    {
        public string ReportTitle { get; set; }
        public string ReportPath { get; set; }
        public string ReportName { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveType { get; set; }
    }

    public class ReportConfig
    {
        public ReportModel ReportModel { get; set; }
    }




}
