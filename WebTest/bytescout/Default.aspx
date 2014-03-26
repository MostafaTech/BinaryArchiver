<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="bytescout_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="requiresActiveX=true" />
</head>
<body>
<form id="form1" runat="server">

<OBJECT id="DemoActiveX" 
    classid="clsid:8271095B-6A00-4742-AF68-BD50ABF17A46" 
    codebase=""></OBJECT>

<br />
<input type="button" value="client scan" onclick="scan()" />
<script type="text/javascript">
    function scan() {
        document.getElementById('DemoActiveX').StartScan();
    }
</script>

</form>
</body>
</html>
