<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisplayPages.aspx.cs" Inherits="KIF_DisplayPages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .pageBreak {
            page-break-before: always; 
        }
        body {
            font-size: 200%;
        }
        @page {
            margin: 0mm;
        }
    </style>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div style="text-align: center;">
            <asp:Repeater runat="server" ID="nextWeek">
                <HeaderTemplate>
                    <h1>Ugens kampe</h1>    
                </HeaderTemplate>
                <ItemTemplate>
                    <h3><%# Eval("HjemmeHold") %> - <%# Eval("UdeHold") %></h3>
                    <h4><%# Eval("Tidspunkt") %></h4>
                    <p>Dommer: <%# Eval("Dommer") %></p>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div style="text-align: center;" class="pageBreak">
            <asp:Repeater runat="server" ID="udehold">
                <HeaderTemplate>
                    <h1>Udehold</h1>
                </HeaderTemplate>
                <ItemTemplate>
                    <h3><%# Eval("Hjemmehold") %></h3>
                    <h3><%# Eval("UdeHold") %></h3>
                    <h4><%# Eval("Tidspunkt") %></h4>
                    <br />
                </ItemTemplate>
                
            </asp:Repeater>
        </div>

        <div style="text-align: center;" class="pageBreak">
            <asp:Repeater runat="server" ID="hjemmehold">
                <HeaderTemplate>
                    <h1>Hjemmehold</h1>
                </HeaderTemplate>
                <ItemTemplate>
                    <h3><%# Eval("Hjemmehold") %></h3>
                    <h4><%# Eval("Tidspunkt") %></h4>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>


        <div style="text-align: center;" class="pageBreak">
            <asp:Repeater runat="server" ID="lastWeek">
                <HeaderTemplate>
                    <h1>Sidste uges resultater</h1>
                </HeaderTemplate>
                <ItemTemplate>
                    <h3><%# Eval("HjemmeHold") %> - <%# Eval("UdeHold") %></h3>
                    <h4><%# Eval("Tidspunkt") %>
                        <p style="font-size: 200%"><%# Eval("UdeHoldScore") %> - <%# Eval("HjemmeHoldScore") %></p>
                    </h4>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>

    </form>
</body>
</html>
