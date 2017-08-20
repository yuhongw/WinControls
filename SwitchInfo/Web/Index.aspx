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
        html, body {
            margin: 0;
            background-color:#1071b5;
        }

        table {
            border-collapse: collapse;
            border: solid #fff;
            border-width: 1px 1px 1px 1px;
            margin:15px 15px 150px 15px;
        }

            table th, table td {
                border: solid #fff;
                border-width: 1px 1px 1px 1px;
                padding: 2px;
                line-height:40px;
                min-width:150px;
                text-align:center;
                font-family: 微软雅黑;
                font-size: 12pt;
                color: white;
            }

        thead td {
            background-color:#154;
            opacity:0.8;
        }

        td.rowHead {
            min-width:200px;
        }

        .device .siteName {
            font-size: 14pt;
            text-align: center;
        }

        #siteCont {
            position: absolute;
            top: 250px;
            left: 225px;
            width:1250px;
            color:white;
        }
        #divInfo {
            position:absolute;
            display:block;
            top:130px;
            left:300px;
            color:white;
        }
        
    </style>
</head>
<body>

    <div>
        <img src="UI.jpg" style="width: 1600px; height: 1000px;" />
    </div>
    <div id="divInfo" >-在线用户数合计:<span id="sum"></span> - 最后更新时间:<span id="time"></span> - 更新时间间隔:<span><%=IntervalUpdate%>分钟 -</span></div>
    <div id="siteCont">
    </div>
</body>
</html>

<script>

    $.ajaxSetup({
        cache: false //close AJAX cache
    });

    function loadData() {
        $("#siteCont").load("_Data.aspx");
        $("#time").load("_GetInfo.aspx?id=lastUpdateTime&&"+Date.now());
        $("#sum").load("_GetInfo.aspx?id=userCountSum&&"+Date.now());
    }
    loadData();
    setInterval(function () {
        loadData();
    }, <%=Interval%> *1000);
</script>
