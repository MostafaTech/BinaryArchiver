<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Network.aspx.cs" Inherits="Network" %>
<%@ Register Assembly="FileArchiver" Namespace="MTech.FileArchiver" TagPrefix="mtc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body dir="rtl">
<script type="text/javascript" src="jquery.js"></script>
<form id="form1" runat="server">
<asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
<mtc:FileArchiverNetworkWebControl runat="server" ID="archiver1" ClientInstanceName="archiver1" />


</form>
</body>
</html>
