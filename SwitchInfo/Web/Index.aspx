<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>
<%@ Import Namespace="SwitchInfo" %>
<%@ Import Namespace="Web.Properties" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Script/jquery-1.7.1.min.js"></script>
    <style>
        html,body {
            margin:0;
        }
        .device {
             font-family:微软雅黑;
             font-size:12pt;
             color:white;
             display:block;
             position:absolute;
             top:258px;
             left:235px;

        }
            .device td {
                min-width:100px;
                line-height:21px;
            }

                .right {
                    text-align:right;
                }
           
            .device .siteName {
                font-size:14pt;
                text-align:center;
            }
    </style>
</head>
<body>

    <div>
        <img src="UI.png" style="width:1600px; height:1000px;" />
    </div>
    <div class="device" id="deviceCont">
    </div>
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
    }, <%=Interval%> *1000);
</script>
