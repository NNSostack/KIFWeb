<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendCheckInfoMails.aspx.cs" Inherits="KIF_SendCheckInfoMails" ValidateRequest="false"   %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-decoration:none; ">
        Testemail: <asp:TextBox runat="server" ID="txtTestMail" />
        Kodeord: <asp:TextBox runat="server" ID="txtSecurity" />
        <asp:Button runat="server" Text="Send Check Info Mails" OnClick="send_Click" />
        <asp:CheckBox runat="server" Text="Send KUN til dem der ikke har checked" ID="chkOnlyNotChecked" />
        <br />
        <asp:Button runat="server" Text="Vis ikke checked info" OnClick="showUnChecked_Click" />
        <br/>
        <br />
        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtMessage" Width="600" Height="400" />
        <br />
        <asp:Button runat="server" ID="showTest" Text="Vis mailindhold" OnClick="showTest_Click" /> 

    </div>
    </form>
</body>
</html>
