using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCAReports.SERLabel
{
    [Serializable]
    public class SERLabelReportModel
    {
        public int SaleInvoiceID { get; set; }

        public DateTime InvoiceDate { get; set; }

        public int InvoiceNo { get; set; }

        public int? SaleOrderID { get; set; }

        public int CustomerID { get; set; }

        public string BusinessName { get; set; }

        public string ContactName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public string IntPhoneNo { get; set; }

        public string AddressLine1
        {
            get
            {
                string Add = "";

                if (!String.IsNullOrWhiteSpace(Address))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Address;
                }
                return Add;
            }
        }

        public string AddressLine2
        {
            get
            {
                string Add = "";

                if (!String.IsNullOrWhiteSpace(City))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + City;
                }
                if (!String.IsNullOrWhiteSpace(Country))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Country;
                }
                if (!String.IsNullOrWhiteSpace(Postcode))
                {
                    Add += (!String.IsNullOrWhiteSpace(Add) ? ", " : "") + Postcode;
                }
                return Add;
            }
        }

        public string EmailContact { get; set; }

        public string AirportDestCity { get; set; }

        public DateTime? EstDelDate { get; set; }

        public string AWBNo { get; set; }

        public string DomesticFlight { get; set; }

        public string InternationalFlight { get; set; }
    }
}
