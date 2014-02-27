<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FilesDisplay.aspx.cs" Inherits="ProjectWeb.Web.FilesDisplay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div align="right">
                <asp:LinkButton ID="lbtnLogOut" runat="server" Text="LogOut" BorderWidth="3" BorderColor="Red" BackColor="#ffffcc" ForeColor="#ff0000" OnClick="lbtnLogOut_Click"></asp:LinkButton>
            </div>
            <br />
            <br />
            <asp:Label ID="lblUserName" runat="server"></asp:Label>
            <br />
            <br />
            <div>
                <asp:GridView ID="gvFiles" runat="server"
                    Width="100%" BorderColor="Black"
                    BorderStyle="Ridge" CellPadding="2" GridLines="Horizontal" AutoGenerateColumns="False"
                    BorderWidth="2px" BackColor="White" AllowSorting="True"
                    DataKeyNames="FileID" OnRowCommand="gvFiles_RowCommand">
                    <AlternatingRowStyle BackColor="#ffff99" ForeColor="Black" CssClass="alternating-row" />
                    <RowStyle Wrap="False" HorizontalAlign="Center" ForeColor="Black"
                        BackColor="#DEDFDE"></RowStyle>
                    <HeaderStyle ForeColor="#E7E7FF" CssClass="boldLabel" BackColor="#0000ff"></HeaderStyle>
                    <Columns>
                        <asp:BoundField HeaderText="FileName" DataField="FileName" />
                        <asp:BoundField HeaderText="File Path in System" DataField="FilePath" />
                        <asp:BoundField HeaderText="Last Modified" DataField="LastModified" />
                        <asp:CommandField ControlStyle-Font-Italic="true" ControlStyle-BackColor="#00ff00" ControlStyle-BorderColor="#000000" ControlStyle-BorderWidth="2" HeaderText="Download File" ShowSelectButton="true" SelectText="Download" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
