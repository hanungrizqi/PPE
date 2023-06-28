using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PLANT_PPE.Reports
{
    public partial class ReportSH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pv_cust_generateReport(Request.QueryString["PPE_NO"], Request.Url.ToString());
            }
        }
        private void pv_cust_generateReport(string PPE_NO, string url)
        {
            Microsoft.Reporting.WebForms.ReportParameter[] i_cls_rParam = new Microsoft.Reporting.WebForms.ReportParameter[1];
            i_cls_rParam[0] = new Microsoft.Reporting.WebForms.ReportParameter("PPE_NO", PPE_NO);

            string i_str_rpt = "Rpt_PPE_SecHead";

            ReportViewer2.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["report_server_url"].ToString());
            ReportViewer2.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["report_path"].ToString() + i_str_rpt;
            string a = ReportViewer2.ServerReport.ReportServerUrl.ToString() + ReportViewer2.ServerReport.ReportPath;
            string b = url;
            ReportViewer2.ServerReport.SetParameters(i_cls_rParam);
            ReportViewer2.ServerReport.Refresh();
        }
    }
}