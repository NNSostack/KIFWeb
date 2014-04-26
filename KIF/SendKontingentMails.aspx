<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendKontingentMails.aspx.cs" Inherits="KIF_SendKontingentMails" ValidateRequest="false"   %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Testemail: <asp:TextBox runat="server" ID="txtTestMail" />
        Kodeord: <asp:TextBox runat="server" ID="txtSecurity" />
        <asp:Button runat="server" Text="Send kontingentmails" OnClick="send_Click" />
        <br/>
        <br />
        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtMessage" Width="600" Height="400" />
        <br />
        <asp:Button runat="server" ID="showTest" Text="Vis mailindhold" OnClick="showTest_Click" /> 
        <asp:Button runat="server" ID="cmdShowNotDownloads" Text="Vis medlemmer, der ikke har downloaded girokort" OnClick="showNoDownload_Click" /> 

        <br />
        <br />
        <table>
            <asp:Repeater runat="server" ID="rptFritaget">
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Fritaget for kontingent (<%# ((IList)rptFritaget.DataSource).Count %>)</h2>
                        </td> 
                    </asp:PlaceHolder>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Telefon") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>


            <asp:Repeater runat="server" ID="rptNoDownload">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Ikke fritaget for kontingent (<%# ((IList)rptNoDownload.DataSource).Count %>)</h2>
                        </td> 
                    </asp:PlaceHolder>
                
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Telefon") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
