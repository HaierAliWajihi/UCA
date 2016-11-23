using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UCAReports
{
    public static class ReportCommon
    {
        public static TextReader GetReportStream(string reportResourceName)
        {
            if (String.IsNullOrWhiteSpace(reportResourceName)) return null;
            Assembly assembly = Assembly.GetExecutingAssembly();
            return new StreamReader(assembly.GetManifestResourceStream(reportResourceName));
        }

        public class ReportDataSource
        {
            public string DataSetName { get; set; }

            public object DataSource { get; set; }
        }
        public class SubReportDetail
        {
            public string ReportName { get; set; }

            public string ReportPath { get; set; }

            public List<UCAReports.ReportCommon.ReportDataSource> reportDataSource { get; set; }
        }
    }
}
