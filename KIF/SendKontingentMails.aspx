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
        Send for medlemsnummer: <asp:TextBox runat="server" ID="txtMedlemsnummer" />
        Kodeord: <asp:TextBox runat="server" ID="txtSecurity" />
        <asp:Button runat="server" Text="Send kontingentmails (kun for dem med girokort)" OnClick="send_Click" />
        <asp:Button ID="Button1" runat="server" Text="Send mails" OnClick="send_Click2" />
        <br/>
        <br />
        Subject:<br />
        <asp:TextBox runat="server" ID="txtSubject" Width="600"  />
        <br />
        Body:
        <br />
        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtMessage" Width="600" Height="400" />
        <br />
        <asp:CheckBox runat="server" ID="chkOnlySendToMembersWithNoPayment" Text="Send KUN til dem, der ikke har betalt" />
        <asp:CheckBox runat="server" ID="chkOnlySendToMembersWhoHaveNotDownloadedGiro" Text="Send KUN til dem, der ikke har downloaded girokort" />
        <br />
        <asp:Button runat="server" ID="showTest" Text="Vis mailindhold" OnClick="showTest_Click" /> 
        <asp:Button runat="server" ID="cmdShowNotDownloads" Text="Vis medlemmer, der ikke har downloaded girokort" OnClick="showNoDownload_Click" /> 
        <br />



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
                        <td><%# Eval("Rabat") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>

            <asp:Repeater runat="server" ID="rptNoInvoice">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Kontingentkørsel er ikke kørt for (<%# ((IList)rptNoInvoice.DataSource).Count %>)</h2>
                        </td> 
                    </asp:PlaceHolder>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Telefon") %></td>
                        <td><%# Eval("Rabat") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>

            <asp:Repeater runat="server" ID="rptDiscount">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Medlemmer med rabat</h2>
                        </td> 
                    </asp:PlaceHolder>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Telefon") %></td>
                        <td><%# Eval("Rabat") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>



            <asp:Repeater runat="server" ID="rptNoDownload">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Ikke fritaget for kontingent og ikke hentet girokort (<%# ((IList)rptNoDownload.DataSource).Count %>)</h2>
                        </td> 
                    </asp:PlaceHolder>
                
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><a href="sms:<%# Eval("Telefon") %>"><%# Eval("Telefon") %></a></td>
                        <td><%# Eval("Rabat") %></td>
                    </tr>
                    <%# SetEmail((String)Eval("Email")) %>
                </ItemTemplate> 
                <FooterTemplate>
                    <tr>
                        <td colspan="6">
                            <a href="mailto:<%# allEmails %>">Send email til alle</a>
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Repeater runat="server" ID="rptHasDownloaded">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="<%# Container.ItemIndex == 0 %>">
                        <td colspan="6">
                            <h2>Medlemmer der har hentet girokort</h2>
                        </td> 
                    </asp:PlaceHolder>
                    <tr>
                        <td><%# Container.ItemIndex + 1 %></td>
                        <td><%# Eval("Årgang") %></td>
                        <td><%# Eval("MemberId") %></td>
                        <td><%# Eval("Navn") %></td>
                        <td><%# Eval("Email") %></td>
                        <td><%# Eval("Telefon") %></td>
                        <td><%# Eval("Rabat") %></td>
                    </tr>
                </ItemTemplate> 
            </asp:Repeater>


                <tr>
                    <td colspan="6">
                        <asp:Button ID="cmdGetGiroKort" runat="server" Text="Hent girokort" OnClick="cmdGetGiroKort_Click" />
                        Download til: <asp:TextBox Width="500" runat="server" ID="txtDownloadPath" Text="C:\Users\Nikolaj Sostack\Downloads\KIF" />
 
                    </td>

                </tr>

            
        </table>
    </div>
    </form>
</body>
</html>
