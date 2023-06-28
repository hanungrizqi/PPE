using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace API_PLANT_PPE.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
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

            string i_str_rpt = "ReviewPPE";

            RptPPEViewer.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["report_server_url"].ToString());
            RptPPEViewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["report_path"].ToString() + i_str_rpt;
            RptPPEViewer.ServerReport.SetParameters(i_cls_rParam);
            RptPPEViewer.ServerReport.Refresh();
        }
    }
}