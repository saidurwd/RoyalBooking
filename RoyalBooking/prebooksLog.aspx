<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="prebooksLog.aspx.cs" Inherits="RoyalBooking.prebooksLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Prebooks Log</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive" style="height: 600px; overflow: auto;">
                                <asp:GridView ID="GridView1" runat="server" AllowPaging="False" AllowSorting="False" CssClass="table table-striped jambo_table bulk_action" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ControlStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkRow" CssClass="flat" Style="display: none"/>
                                                <asp:TextBox runat="server" ID="txtKeyField" Text='<%#Eval("Id") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtprebookItemId" Text='<%#Eval("prebookItemId") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtNumber" Text='<%#Eval("number") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtprebook" Text='<%#Eval("prebook") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtpoItemId" Text='<%#Eval("poItemId") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtProductDescription" Text='<%#Eval("productDescription") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtTruckDate" Text='<%#Eval("truckDate") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtNewTruckDate" Text='<%#Eval("NewTruckDate") %>' Style="display: none" />
                                                
                                                <asp:TextBox runat="server" ID="txtInvoiceNumber" Text='<%#Eval("InvoiceNumber") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtcustomerIdPB" Text='<%#Eval("customerIdPB") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtcustomerPoNumber" Text='<%#Eval("customerPoNumber") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtcomments" Text='<%#Eval("comments") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtshipToId" Text='<%#Eval("shipToId") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtunitPrice" Text='<%#Eval("unitPrice") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtmarkCodePB" Text='<%#Eval("markCodePB") %>' Style="display: none" />
                                                <asp:TextBox runat="server" ID="txtpbQty" Text='<%#Eval("pbQty") %>' Style="display: none" />
                                                
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="shipDate" HeaderText="PO Date" SortExpression="shipDate" />
                                        <asp:BoundField DataField="truckDate" HeaderText="Prebook Date" SortExpression="truckDate" />
                                        <asp:BoundField DataField="customerName" HeaderText="Customer" SortExpression="customerName" />
                                        <asp:BoundField DataField="number" HeaderText="PO" SortExpression="number" />
                                        <asp:BoundField DataField="prebook" HeaderText="prebook" SortExpression="prebook" />
                                        <asp:BoundField DataField="productDescription" HeaderText="Product" SortExpression="productDescription" ControlStyle-CssClass="hello" />
                                        <asp:BoundField DataField="totalBoxes" HeaderText="totalBoxes" SortExpression="totalBoxes" />
                                        <asp:BoundField DataField="boxType" HeaderText="boxType" SortExpression="boxType" />
                                        <asp:BoundField DataField="vendorName" HeaderText="vendorName" SortExpression="vendorName" />
                                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                                        <asp:BoundField DataField="orderType" HeaderText="Order Type" SortExpression="orderType" />
                                        <asp:BoundField DataField="Action Status" HeaderText="Action Status" SortExpression="Action Status" />
                                        <asp:BoundField DataField="NewTruckDate" HeaderText="New Ship Date" SortExpression="NewTruckDate" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-group space-20">
                                <div class="col-lg-11 col-md-11 col-sm-11 col-xs-11"></div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <asp:Button ID="btnMove" runat="server" Text="Move" OnClientClick="if(!moveConfirm()) return false;" OnClick="btnMove_Click" CssClass="btn btn-danger btn-lg btn-block" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
