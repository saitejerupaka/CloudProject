<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectWeb.Login" %>

<!DOCTYPE html>
<META Http-Equiv="Cache-Control" Content="no-cache"/>
<META Http-Equiv="Pragma" Content="no-cache"/>
<META Http-Equiv="Expires" Content="0"/>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="login" runat="server">
        <div>
            <label>Login:  </label>
            <div>
                <br />
                <label>If u want to register please download application and register</label>
                <br />
            </div>
            <br />
            <div>
                <asp:label runat="server">UserName: </asp:label>
                <asp:TextBox ID="tbUName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Required" ControlToValidate="tbUName"></asp:RequiredFieldValidator>
            </div>
            <br />
            <div>
                <asp:label runat="server">Password: </asp:label>
                <asp:TextBox TextMode="Password" ID="tbPwd" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ErrorMessage="Required" ControlToValidate="tbPwd"></asp:RequiredFieldValidator>
            </div>
            <br />
            <div>
                <asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" />
            </div>
            <br />
            <div>
                <asp:label ID="lblFeedBack" runat="server" visible="false"></asp:label>
            </div>
            <br/>
        </div>
    </form>
</body>
</html>
