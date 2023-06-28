using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PLANT_PPE.Reports
{
    public partial class ReportReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pv_cust_generateReport(Request.QueryString["PPE_NO"]);
            }
        }
        private void pv_cust_generateReport(string PPE_NO)
        {
            Microsoft.Reporting.WebForms.ReportParameter[] i_cls_rParam = new Microsoft.Reporting.WebForms.ReportParameter[1];
            i_cls_rParam[0] = new Microsoft.Reporting.WebForms.ReportParameter("PPE_NO", PPE_NO);

            string i_str_rpt = "ReportReview";

            reportReview.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["report_server_url"].ToString());
            reportReview.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["report_path"].ToString() + i_str_rpt;
            reportReview.ServerReport.SetParameters(i_cls_rParam);
            reportReview.ServerReport.Refresh();
        }
    }
}