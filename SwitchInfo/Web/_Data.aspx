<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_Data.aspx.cs" Inherits="Web._Data" %>
<%@ Import Namespace="SwitchInfo" %>
<%if (site != null)
    { %>
<div class="siteName"><%=site.Name %></div>
<table>
    <%foreach (KV kv in site.Values)
        {%>
    <tr>
        <td><%=kv.Key%></td>
        <td class="right"><%=kv.Value%></td>
    </tr>
    <%}
        }%>
</table>
