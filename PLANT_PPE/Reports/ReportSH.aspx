<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportSH.aspx.cs" Inherits="PLANT_PPE.Reports.ReportSH" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PPE Online</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana"
                Font-Size="8pt" Height="100%" InteractiveDeviceInfos="(Collection)" PageCountMode="Actual"
                ProcessingMode="Remote" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                ShowPromptAreaButton="false" PromptAreaCollapsed="true" Width="100%" CssClass="panelReport">
            </rsweb:ReportViewer> 
        </div>
    </form>
</body>
</html>