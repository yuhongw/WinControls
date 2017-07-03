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
            }

        .device {
            font-family: 微软雅黑;
            font-size: 12pt;
            color: white;
            display: inline-block;
        }

            .device td.name {
                min-width: 100px;
                line-height: 21px;
            }

            .device td.value {
                min-width: 50px;
                line-height: 21px;
            }

        .right {
            text-align: right;
        }

        .device .siteName {
            font-size: 14pt;
            text-align: center;
        }

        .siteCont {
            position: absolute;
            top: 250px;
            left: 200px;
            width:1250px;
        }
    </style>
</head>
<body>

    <div>
        <img src="UI.jpg" style="width: 1600px; height: 1000px;" />
    </div>
    <div class="siteCont">
        <%foreach (Site site in this.Data)
            { %>
        <div class="device" siteid="<%=site.Id%>">
        </div>
        <%} %>
    </div>
</body>
</html>

<script>

    function loadData() {
        $("div.device").each(
            function (index, elem) {
                $(elem).load("_Data.aspx?id=" + $(elem).attr("siteId"));
            });
    }
    loadData();
    setInterval(function () {
        loadData();
    }, <%=Interval%> *1000);
</script>
