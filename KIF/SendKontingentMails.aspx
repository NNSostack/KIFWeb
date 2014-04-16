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

    </div>
    </form>
</body>
</html>
