<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="FileArchiver" Namespace="MTech.FileArchiver" TagPrefix="mtc" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body dir="rtl">
<form id="form1" runat="server">

<script type="text/javascript" src="jquery.js"></script>
<link href="scripts/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
<link href="scripts/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
<script src="scripts/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
<script src="scripts/colorbox/jquery.colorbox-fa.js" type="text/javascript"></script>

<asp:Button ID="Button1" runat="server" Text="login" onclick="Button1_Click" />

<mtc:FileArchiverWebControl runat="server" ID="archiver1" ClientInstanceName="archiver1" HostNamePort="192.168.1.72" />

<br /><br />
<input type="button" value="dgfajhgds" onclick="$.colorbox({inline:true,href:'#test_dialog',width:500,height:200});" />
<div style="display:none;">
    <div id="test_dialog">
        hello
    </div>
</div>

</form>
</body>
</html>
