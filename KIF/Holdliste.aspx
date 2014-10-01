<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Holdliste.aspx.cs" Inherits="Holdliste" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        body
        {
            font-family: Verdana;
            font-size: 8pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList runat="server" ID="dd" AutoPostBack="true" />
        
        <table cellspacing="0" cellpadding="10"  >
            <asp:Repeater runat="server" ID="memberList">
                <HeaderTemplate>
                    <h2><%# Afdeling %></h2>
                </HeaderTemplate>

                <FooterTemplate>
                    </tr>
                </FooterTemplate>
                <ItemTemplate>
                    <tr style="background-color:<%# Container.ItemIndex % 2 == 0 ? "#d8d8d8" : "" %>">
                        <td><%# Container.ItemIndex + 1 %>. <%# Eval("Navn") %></td>
                        <td><%# Eval("Adresse") %></td>
                          <td><a href="sms:<%# Eval("Telefon") %>"><%# Eval("Telefon") %></a></td>
                            <td style="width:10px;"><%# GetEmail(Eval("Email") as String) %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater> 
            
        </table>
        <asp:PlaceHolder runat="server" Visible="<%# !String.IsNullOrEmpty(allMail) %>">
            <a href="" onclick="this.href='mailto:' + '<%# allMailWithSemiColon %>'">Send mail til alle</a>
            <a href="" onclick="this.href='mailto:' + '<%# allMail %>'">Send mail til alle fra IPad</a>
        </asp:PlaceHolder> 

    </form>
</body>
</html>
