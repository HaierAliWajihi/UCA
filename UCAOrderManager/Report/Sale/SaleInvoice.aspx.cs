using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UCAReports.SaleInvoice;

namespace UCAOrderManager.Report.Sale
{
    public partial class SaleInvoice : AppReportViewer
    {
        int SaleInvoiceID { get; set; }

        string ReportDisplayName_;
        public override string ReportDisplayName
        {
            get
            {
                return ReportDisplayName_;
            }
        }

        public SaleInvoice()
        {
            ReportDisplayName_ = "Full Invoice";
        }

        protected override void OnLoad(EventArgs e)
        {
            string v = Request.QueryString["ID"];
            if (!String.IsNullOrWhiteSpace(v))
            {
                int intv = 0;
                int.TryParse(v, out intv);
                SaleInvoiceID = intv;
            }
            base.OnLoad(e);
        }

        public override string ReportName
        {
            get
            {
                return "UCAReports.SaleInvoice.rptSaleInvoice.rdlc";
            }
        }

        public override List<UCAReports.ReportCommon.ReportDataSource> GetReportDataSource()
        {
            DAL.SaleInvoice.SaleInvoiceDAL DALObj = new DAL.SaleInvoice.SaleInvoiceDAL();
            List<SaleInvoiceReportModel> ds = DALObj.GetSaleInvoiceReportHeader(SaleInvoiceID);

            if(ds != null && ds.Count > 0)
            {
                ReportDisplayName_ = "FullInvoice" + ds[0].InvoiceNo.ToString("000#");
            }

            return new List<UCAReports.ReportCommon.ReportDataSource>() { 
                new UCAReports.ReportCommon.ReportDataSource() { DataSetName = "dsSaleInvoice", DataSource = ds }
                //new UCAReports.ReportCommon.ReportDataSource() 
                //{ 
                //    DataSetName = "dsSaleInvoiceFooter", 
                //    DataSource = new List<UCAReports.SaleInvoice.SaveInvoiceFooter>(){ new UCAReports.SaleInvoice.SaveInvoiceFooter(){ SaleInvoiceID = SaleInvoiceID} 
                //} 
                //}
            };
        }

        public override List<UCAReports.ReportCommon.SubReportDetail> SubReportsName
        {
            get
            {
                DAL.SaleInvoice.SaleInvoiceDAL DALObj = new DAL.SaleInvoice.SaleInvoiceDAL();
                List<SaleInvoiceProducDetailReportModel> ds = DALObj.GetSaleInvoiceReportProductDetail(SaleInvoiceID);

                if (ds != null)
                {
                    FillFooterAdjustmentRecord(ds);
                }


                return new List<UCAReports.ReportCommon.SubReportDetail>()
                {
                  new UCAReports.ReportCommon.SubReportDetail()
                  {
                      ReportName = "SubReportProductDetail", 
                      ReportPath = "UCAReports.SaleInvoice.rptSaleInvoiceProductDetail.rdlc",
                      reportDataSource = new List<UCAReports.ReportCommon.ReportDataSource>()
                      {
                          new UCAReports.ReportCommon.ReportDataSource()
                          {
                              DataSetName = "dsSaleInvoiceProductDetail",
                              DataSource = ds
                          }
                      }
                  }
                  //,
                  //new UCAReports.ReportCommon.SubReportDetail()
                  //{
                  //    ReportName = "SubreportFooter", 
                  //    ReportPath = "UCAReports.SaleInvoice.rptSaleInvoiceFooter.rdlc",
                  //    reportDataSource = new List<UCAReports.ReportCommon.ReportDataSource>()
                  //    {
                  //        new UCAReports.ReportCommon.ReportDataSource()
                  //        {
                  //            DataSetName = "dsSaleInvoiceFooter",
                  //            DataSource = new List<UCAReports.SaleInvoice.SaveInvoiceFooter>(){ new UCAReports.SaleInvoice.SaveInvoiceFooter(){ SaleInvoiceID = SaleInvoiceID}}
                  //        }
                  //    }
                  //},
                };
            }
        }

        public static void FillFooterAdjustmentRecord(List<SaleInvoiceProducDetailReportModel> ds)
        {
            if (ds == null) return;

            int RecordCount = ds.Count;
            const int MaxCountWithHeader = 45;
            const int MaxCountWithoutHeader = 61;

            int CountToAdd = 0;

            if(RecordCount <= MaxCountWithHeader)
            {
                CountToAdd = MaxCountWithHeader - RecordCount;
            }
            else if(RecordCount <= MaxCountWithoutHeader)
            {
                CountToAdd = MaxCountWithoutHeader - (RecordCount % MaxCountWithHeader);
            }
            else
            {
                int res = (RecordCount % MaxCountWithoutHeader);
                if(res <= MaxCountWithHeader)
                {
                    CountToAdd = MaxCountWithHeader - res;
                }
                else
                {
                    CountToAdd = (MaxCountWithoutHeader - res) + MaxCountWithHeader;
                }
            }

            for(int ri = 0; ri < CountToAdd; ri++)
            {
                ds.Add(new SaleInvoiceProducDetailReportModel());
            }
        }
    }
}