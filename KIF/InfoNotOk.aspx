<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InfoNotOk.aspx.cs" Inherits="KIF_InfoNotOk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        td input {
            width: 300px; 
        }
        
        td {
            vertical-align: top;
            font-weight: bold;
            width: 200px;  
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:PlaceHolder runat="server" Visible="<%# InfoUpdated %>">
        <h2>Informationerne er nu opdateret. Tak for hjælpen.</h2>
    </asp:PlaceHolder>

    <asp:Panel runat="server" Visible="<%# Valid && !InfoUpdated %>"> 
        <h2>Her kan du ændre de oplysninger der mangler eller er forkerte.</h2>
        <p>Tryk på den grønne knap når du er færdig. På forhånd tak for hjælpen.</p>
        <table>
            <tr>
                <td>Navn</td>
                <td><asp:TextBox runat="server" ID="txtName"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Fødselsdato</td>
                <td><asp:Calendar runat="server" ID="cal"></asp:Calendar></td>
            </tr>
            <tr>
                <td>Adresse</td>
                <td><asp:TextBox runat="server" ID="txtAddress"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Postnummer</td>
                <td><asp:TextBox runat="server" ID="txtZip"></asp:TextBox></td>
            </tr>
            <tr>
                <td>By</td>
                <td><asp:TextBox runat="server" ID="txtCity"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Telefon</td>
                <td><asp:TextBox runat="server" ID="txtPhone"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email</td>
                <td><asp:TextBox runat="server" ID="txtEmail"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <br />
                    <asp:Button ID="confirmInfo" runat="server" Text="Bekræft oplysninger" BackColor="#49D719" Height="50" Width="200" ForeColor="White" OnClick="confirmInfo_Click"   /></td>
            </tr>
        </table>
        <br />
        


    </asp:Panel>
    </form>
</body>
</html>
 