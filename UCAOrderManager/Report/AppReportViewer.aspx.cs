using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace UCAOrderManager.Report
{
    public partial class AppReportViewer : System.Web.UI.Page
    {
        public virtual string ReportName { get { return null; } }

        public virtual string ReportDisplayName { get { return null; } }

        public virtual List<UCAReports.ReportCommon.SubReportDetail> SubReportsName { get { return null; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && ReportViewer1 != null)
            {
                GenerateReport();
            }
        }

        void GenerateReport()
        {
            if (String.IsNullOrWhiteSpace(ReportName)) return;
            List<UCAReports.ReportCommon.ReportDataSource> reportDataSource = GetReportDataSource();

            //ReportViewer1.Reset();
            ReportViewer1.PageCountMode = PageCountMode.Actual;
            ReportViewer1.LocalReport.DisplayName = ReportDisplayName;
            ReportViewer1.ShowBackButton = true;
            ReportViewer1.ShowFindControls = true;
            ReportViewer1.ShowPageNavigationControls = true;
            ReportViewer1.ShowPrintButton = true;
            ReportViewer1.ShowRefreshButton = true;
            ReportViewer1.ShowZoomControl = true;
            ReportViewer1.ZoomMode = ZoomMode.FullPage;
            

            ReportViewer1.LocalReport.LoadReportDefinition(UCAReports.ReportCommon.GetReportStream(ReportName));

            // Loading Sub reports 
            if (SubReportsName != null && SubReportsName.Count > 0)
            {
                foreach (UCAReports.ReportCommon.SubReportDetail subRep in SubReportsName)
                {
                    ReportViewer1.LocalReport.LoadSubreportDefinition(subRep.ReportName, UCAReports.ReportCommon.GetReportStream(subRep.ReportPath));
                }
            }

            ReportViewer1.LocalReport.DataSources.Clear();
            if (reportDataSource != null)
            {
                foreach (UCAReports.ReportCommon.ReportDataSource rds in reportDataSource)
                {
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(rds.DataSetName, rds.DataSource));
                }
            }

            ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
            ReportViewer1.LocalReport.Refresh();
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e)
        {
            UCAReports.ReportCommon.SubReportDetail SubRep = SubReportsName.FirstOrDefault(r => r.ReportName == e.ReportPath);
            if (SubRep != null)
            {
                foreach (UCAReports.ReportCommon.ReportDataSource RepDS in SubRep.reportDataSource)
                {
                    e.DataSources.Add(new ReportDataSource(RepDS.DataSetName, RepDS.DataSource));
                }
            }
        }

        public virtual List<UCAReports.ReportCommon.ReportDataSource> GetReportDataSource() { return null; }

        protected void btnPrintReport_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ReportName)) return;
            List<UCAReports.ReportCommon.ReportDataSource> reportDataSource = GetReportDataSource();

            LocalReport LR = new LocalReport();
            LR.LoadReportDefinition(UCAReports.ReportCommon.GetReportStream(ReportName));

            // Loading Sub reports 
            if (SubReportsName != null && SubReportsName.Count > 0)
            {
                foreach (UCAReports.ReportCommon.SubReportDetail subRep in SubReportsName)
                {
                    LR.LoadSubreportDefinition(subRep.ReportName, UCAReports.ReportCommon.GetReportStream(subRep.ReportPath));
                }
            }

            LR.DataSources.Clear();
            if (reportDataSource != null)
            {
                foreach (UCAReports.ReportCommon.ReportDataSource rds in reportDataSource)
                {
                    LR.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(rds.DataSetName, rds.DataSource));
                }
            }

            LR.SubreportProcessing += LocalReport_SubreportProcessing;
            LR.Refresh();

            DirectPrint DP = new DirectPrint();
            DP.Print(LR);
        }
    }

    public class ReportParemeter
    {
        public ReportParemeter()
        {
            Exception = null;
            Result = eReportGenerationResult.NotGeneratedYet;
        }

        public Exception Exception { get; set; }
        public eReportGenerationResult Result { get; set; }
        public string ValidationError { get; set; }

        public enum eReportGenerationResult
        {
            NotGeneratedYet = 0,
            GeneratedSucessfuly = 1,
            ErrorWhileGenerating = 2,
            ValidationError = 3
        }

        public string ReportHeadingLine1 { get; set; }
        public string ReportHeadingLine2 { get; set; }

        public ReportSource MainReport { get; set; }

        public List<ReportSource> SubReports { get; set; }
    }

    public class ReportSource
    {
        public string ReportName { get; set; }
        public List<ReportDataSource> ReportDataSources { get; set; }
    }

    public class DirectPrint : IDisposable
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;

        private ReportParemeter ReportSourceParameter { get; set; }

        public bool IsLandscape { get; set; }

        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        /// Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.27in</PageWidth>
                <PageHeight>11.69in</PageHeight>
                <MarginTop>0.5in</MarginTop>
                <MarginLeft>0.25in</MarginLeft>
                <MarginRight>0.00in</MarginRight>
                <MarginBottom>0.5in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        /// Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void SendToPrinter()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();

            PrintDialog dialogue = new PrintDialog();
 
            DialogResult dr = dialogue.ShowDialog();
            if( dr == DialogResult.OK)
            {
                printDoc.PrinterSettings = dialogue.PrinterSettings;
            }
            dialogue.Dispose();
            
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        /// Create a local report for Report.rdlc, load the data, export the report to an .emf file, and print it.
        public void Print(LocalReport report)
        {
            try
            {
                Export(report);
                SendToPrinter();
            }
            catch (Exception)
            {
                //MessageBox.Show("Error occured while printing. Please check your printer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void PrintReport(params object[] QueryParas)
        {
            LocalReport Report = new LocalReport();
            Report.SubreportProcessing += LocalReport_SubreportProcessing;

            ReportParemeter RepParas = new ReportParemeter();
            GenerateReportDataSource(RepParas, QueryParas);
            GenerateReport(Report, RepParas);

            Print(Report);
        }

        protected virtual void GenerateReportDataSource(ReportParemeter Paras, params object[] QueryParas)
        {

        }
        private void GenerateReport(LocalReport lr, ReportParemeter Paras)
        {
            lr.LoadReportDefinition(GetReportStream(Paras.MainReport.ReportName));
            //lr.DataSources.Add(new ReportDataSource("dsTemplateNomal", ReportHeader));
            //--
            ReportSourceParameter = Paras;
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "MainReport")
            {
                foreach (ReportDataSource RepDS in ReportSourceParameter.MainReport.ReportDataSources)
                {
                    e.DataSources.Add(RepDS);
                }
            }
            else
            {
                ReportSource SubRep = ReportSourceParameter.SubReports.FirstOrDefault(r => r.ReportName == e.ReportPath);
                if (SubRep != null)
                {
                    foreach (ReportDataSource RepDS in SubRep.ReportDataSources)
                    {
                        e.DataSources.Add(RepDS);
                    }
                }
            }
        }

        public static TextReader GetReportStream(string reportResourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return new StreamReader(assembly.GetManifestResourceStream(reportResourceName));
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}