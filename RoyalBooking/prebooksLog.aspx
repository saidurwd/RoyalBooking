<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="prebooksLog.aspx.cs" Inherits="RoyalBooking.prebooksLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="">
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 form-group">
                    <input class="form-control" id="txtdescription" placeholder="Product Description" type="text" runat="server">
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 form-group">
                    <input class="form-control" id="txtvendor" placeholder="Vendor" type="text" runat="server">
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 form-group text-right col-lg-offset-3 col-md-offset-3 col-sm-offset-2 col-xs-offset-2">
                    PO Date:
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 form-group">
                    <input class="form-control has-feedback-left hasDatePicker" id="txtDateFrom" placeholder="Start" aria-describedby="inputSuccess1Status1" type="text" runat="server">
                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                    <span id="inputSuccess1Status1" class="sr-only">(success)</span>
                </div>
                <div class="col-lg-1 col-md-1 col-sm-2 col-xs-2">
                    <asp:Button ID="Button1" runat="server" Text="Import PO" OnClick="Button1_Click" CssClass="btn btn-primary pull-right btn-block" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                    <asp:DropDownList ID="ddOrderType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select an Order type" Value=""></asp:ListItem>
                        <asp:ListItem Text="Prebook" Value="P" />
                        <asp:ListItem Text="Standing" Value="S" />
                        <asp:ListItem Text="Double" Value="Double" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-7 col-sm-7 col-xs-12 form-group"></div>
                <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                    <input class="form-control has-feedback-left hasDatePicker" id="txtDateTo" placeholder="End" aria-describedby="inputSuccess2Status2" type="text" runat="server">
                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                    <span id="inputSuccess2Status2" class="sr-only">(success)</span>
                </div>
                <div class="col-lg-1 col-md-1 col-sm-2 col-xs-12">
                    <asp:Button ID="Button2" runat="server" Text="Search" OnClick="Button2_Click" CssClass="btn btn-primary pull-right  btn-block" />
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Manage PO</h2>
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
                                                <asp:CheckBox runat="server" ID="chkRow" CssClass="flat" />
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
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <asp:Button ID="Button3" runat="server" Text="Delete" OnClientClick="if(!deleteConfirm()) return false;" OnClick="btnDelete_Click" CssClass="btn btn-danger btn-lg btn-block" />
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 form-group">
                                    <input class="form-control" id="txtPercent" placeholder="" type="text" runat="server">
                                    <span class="fa fa-percent form-control-feedback right" aria-hidden="true"></span>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2"></div>
                                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center">
                                    <asp:Button ID="btnImportVA" runat="server" Text="Import VA" OnClick="btnImportVA_Click" CssClass="btn btn-primary btn-lg" />
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4"></div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <button type="button" id="btnMoveModal" class="btn btn-primary btn-lg btn-block" data-toggle="modal" data-target="#myModal">Move</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="myModalLabel">Move Prebook</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-12 form-group text-right">
                                New Date Range:           
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 form-group">
                                <asp:TextBox runat="server" ID="txtDateMoveStart" class="form-control has-feedback-left hasDatePicker" placeholder="Start" aria-describedby="inputSuccess3Status3" type="text" />
                                <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                <span id="inputSuccess3Status3" class="sr-only">(success)</span>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 form-group">
                                <asp:TextBox runat="server" ID="txtDateMoveEnd" class="form-control has-feedback-left hasDatePicker" placeholder="Start" aria-describedby="inputSuccess4Status4" type="text" />
                                <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                                <span id="inputSuccess4Status4" class="sr-only">(success)</span>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <hr />
                        <p>
                            Are you sure you want to move <span id="spnMoveProduct"></span>to the date range <span id="spnDateMoveStart"></span>to <span id="spnDateMoveEnd"></span>, from the <span id="noOfChecked"></span>prebooks and according Pos?
                        </p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" Text="&nbsp;&nbsp;Yes&nbsp;&nbsp;" OnClick="btnMove_Click" CssClass="btn btn-primary pull-left" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">&nbsp;&nbsp;No&nbsp;&nbsp;</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
