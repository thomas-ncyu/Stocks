﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LStockCode2.aspx.cs" Inherits="Web.LStockCode2" %>

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