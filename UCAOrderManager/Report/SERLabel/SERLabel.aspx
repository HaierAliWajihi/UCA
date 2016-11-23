<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SERLabel.aspx.cs" Inherits="UCAOrderManager.Report.SERLabel.SERLabel" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--<!DOCTYPE html>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> </title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Button ID="btnPrintReport" runat="server" OnClick="btnPrintReport_Click" Text="Print Report" />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="True" PageCountMode="Actual" ShowZoomControl="True" ShowToolBar="True" ShowPrintButton="True" ShowFindControls="True" ZoomMode="Percent" Height="100%" Width="100%">
        </rsweb:ReportViewer>   
    </div>
    </form>
</body>
</html>
