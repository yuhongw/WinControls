<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>
<%@ Import Namespace="SwitchInfo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Script/jquery-1.7.1.min.js"></script>
    <style>
        .device {
             font-family:微软雅黑;
             font-size:8pt;
             color:white;
             display:block;
             position:absolute;
             top:175px;
             left:120px;

        }
            .device td {
                
                line-height:12px;
            }
           
            .device .siteName {
                font-size:12pt;
                text-align:center;
            }
    </style>
</head>
<body>

    <div>
        <img src="UI.jpg" />
    </div>

    <%foreach (Site site in this.Data){%>
    <div class="device" id="deviceCont">
    </div>
    <%}%>
</body>
</html>

<script>
    
    function loadData()
    {
        $("#deviceCont").load("_Data.aspx");
    }
    loadData();
    setInterval(function () {
        loadData();
    }, 5000);
</script>
