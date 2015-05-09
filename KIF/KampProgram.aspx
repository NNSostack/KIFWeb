<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KampProgram.aspx.cs" Inherits="KampProgram" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style media="screen" type="text/css">
        .strikethrough td {
            text-decoration: line-through; 
        }
        td.nostrikethrough {
            text-decoration: none; 
        }
        .blue
        {
            background-color: #ADD8E6;
        }
        .red
        {
            background-color: Red;
        }
        
        .yellow
        {
            background-color: Yellow;
        }
    </style> 

    <style media="print" type="text/css">
        .hideOnPrint {
            display:none;
        }
        .red
        {
            background-color: Red;
        }
        
        .yellow
        {
            background-color: Yellow;
        }
        
        tr
        {
            font-size: 10px; 
        }
        
        

    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="hideOnPrint">
            <a href="http://noerup-sostack.dk/kif/DisplayPages.aspx">Ugens kampe</a>
            <a href="http://noerup-sostack.dk/kif/DisplayPages.aspx?neextWeek=1">Næste uges kampe</a>
        </div>
        <table border="1px solid black;" cellspacing="0" cellpadding="5">
            <tr style="font-weight:bold;">
                <td>#</td>
                <td>Dag</td>
                <td>Dato</td>
                <td>Kl.</td>
                <td>Række</td>
                <td>Hjemmehold</td>
                <td>Udehold</td>
                <td><%# ShowKiosk ? "Kiosk" : "Dommer" %></td>
            </tr>
            <asp:Repeater runat="server" ID="list">
                <ItemTemplate>
                    <tr class="<%# IsNext((DateTime)(Eval("Date"))) + ((Boolean)Eval("Oversidder") ? " strikethrough" : "") as String %>">
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td class="<%# Color((DateTime)Eval("Date")) %>"><%# Eval("Dag") %></td>
                        <td><%# Eval("Dato") %></td>
                        <td><%# Eval("Kl") %></td>
                        <td><%# Eval("Række") %></td>
                        <td><%# Eval("Hjemmehold") %></td>
                        <td><%# Eval("Modstander") %></td>
                        <td class="<%# ShowKiosk ? "nostrikethrough" : "" %>"><%# ShowKiosk ? Eval("Kiosk") : Eval("Dommer") %></td>
                    </tr>
                </ItemTemplate>
        
            </asp:Repeater> 
        </table>
    </div>
    </form>
</body>
</html>
