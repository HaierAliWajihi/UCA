using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UCAReports.SaleInvoice;

namespace UCAOrderManager.Report.Sale
{
    public partial class SaleInvoiceShipping : AppReportViewer
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

        public SaleInvoiceShipping()
        {
            ReportDisplayName_ = "Shipping Invoice";
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
                return "UCAReports.SaleInvoice.rptSaleInvoiceShipping.rdlc";
            }
        }

        public override List<UCAReports.ReportCommon.ReportDataSource> GetReportDataSource()
        {
            DAL.SaleInvoice.SaleInvoiceDAL DALObj = new DAL.SaleInvoice.SaleInvoiceDAL();
            List<SaleInvoiceReportModel> ds = DALObj.GetSaleInvoiceReportHeader(SaleInvoiceID);

            if (ds != null && ds.Count > 0)
            {
                ReportDisplayName_ = "ShippingInvoice" + ds[0].InvoiceNo.ToString("000#");
            }

            return new List<UCAReports.ReportCommon.ReportDataSource>() { new UCAReports.ReportCommon.ReportDataSource() { DataSetName = "dsSaleInvoice", DataSource = ds } };
        }

        public override List<UCAReports.ReportCommon.SubReportDetail> SubReportsName
        {
            get
            {
                DAL.SaleInvoice.SaleInvoiceDAL DALObj = new DAL.SaleInvoice.SaleInvoiceDAL();
                List<SaleInvoiceProducDetailReportModel> ds = DALObj.GetSaleInvoiceReportProductDetail(SaleInvoiceID);
                SaleInvoice.FillFooterAdjustmentRecord(ds);

                return new List<UCAReports.ReportCommon.SubReportDetail>()
                {
                  new UCAReports.ReportCommon.SubReportDetail()
                  {
                      ReportName = "SubReportProductDetail", 
                      ReportPath = "UCAReports.SaleInvoice.rptSaleInvoiceShippingProductDetail.rdlc",
                      reportDataSource = new List<UCAReports.ReportCommon.ReportDataSource>()
                      {
                          new UCAReports.ReportCommon.ReportDataSource()
                          {
                              DataSetName = "dsSaleInvoiceProductDetail",
                              DataSource = ds
                          }
                      }
                  }
                };
            }
        }
    }
}