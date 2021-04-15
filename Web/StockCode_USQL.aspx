<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockCode_USQL.aspx.cs" Inherits="Web.LStockCode_USQL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td colspan="2">Update StockCode</td>
            </tr>
            <tr>
                <td class="auto-style1">Code</td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Name</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">Update StockCode</td>
                <td>

                    <asp:Button ID="btnUpdateStockCode" runat="server" OnClick="btnUpdateByID_Click" Text="Update" Width="83px" />
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>StockCode</td>
            </tr>
            <tr>

                <td>
                    <asp:GridView ID="gvStockCodes" runat="server">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>