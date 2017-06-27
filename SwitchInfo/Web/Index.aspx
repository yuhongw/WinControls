<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>
<%@ Import Namespace="SwitchInfo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <div>
        <img src="UI.jpg" />
    </div>

    <%foreach (Site site in this.Data){%>

    <ul>
        <li><%=site.Name %></li>
        <li><ul>
            <%foreach (KV kv in site.Values){%>
                <li><span><%=kv.Key%></span><span><%=kv.Value%></span></li>
            <%}%>
            </ul>
        </li>
    </ul>
    <%}%>
</body>
</html>
