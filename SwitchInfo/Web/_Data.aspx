<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_Data.aspx.cs" Inherits="Web._Data" %>
<%@ Import Namespace="SwitchInfo" %>

<table>
<thead>
  <tr><td class="rowHead">设备名称</td><td>输入带宽</td><td>输出带宽</td><td>在线用户</td><td>访问用户SL2</td><td>访问用户SL3</td><td>访问用户SL4</td></tr>
</thead>
<tbody>
<%foreach(Site site in Data){%>
<tr>
<td><%=site.Name %></td>
    <%foreach (KV kv in site.Values){%>
        <td class="value right"><%=kv.Value%></td>
    <%}%>
</tr>
<%}%>
</tbody>
</table>