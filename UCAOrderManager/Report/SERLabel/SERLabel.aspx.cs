using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UCAReports.SERLabel;

namespace UCAOrderManager.Report.SERLabel
{
    public partial class SERLabel : Report.AppReportViewer
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

        public SERLabel()
        {
            ReportDisplayName_ = "SER Label";
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
                return "UCAReports.SERLabel.SERLabel.rdlc";
            }
        }

        public override List<UCAReports.ReportCommon.ReportDataSource> GetReportDataSource()
        {
            DAL.SaleInvoice.SaleInvoiceDAL DALObj = new DAL.SaleInvoice.SaleInvoiceDAL();
            List<SERLabelReportModel> ds = DALObj.GetSERLabelReportData(SaleInvoiceID);

            if(ds != null && ds.Count > 0)
            {
                SERLabelReportModel ObjToCopy = CommonFunctions.DeepCopy<SERLabelReportModel>(ds.First());
                ds.Add(ObjToCopy);
                ds.Add(ObjToCopy);
                ds.Add(ObjToCopy);
                ds.Add(ObjToCopy);

                ReportDisplayName_ = "SER Label" + ds[0].InvoiceNo.ToString("000#");
            }

            return new List<UCAReports.ReportCommon.ReportDataSource>() { 
                new UCAReports.ReportCommon.ReportDataSource() { DataSetName = "dsSERLabel", DataSource = ds }
            };
        }
    }
}