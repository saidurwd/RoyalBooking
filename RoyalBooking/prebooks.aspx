﻿<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="prebooks.aspx.cs" Inherits="RoyalBooking.prebooks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="Button1" runat="server" Text="Import Prebooks" OnClick="Button1_Click" CssClass="btn btn-primary pull-right" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                        <input class="form-control has-feedback-left" id="txtdescription" placeholder="" type="text" runat="server">
                    </div>
                    <div class="col-md-5 col-sm-5 col-xs-12 form-group"></div>
                    <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                        <input class="form-control has-feedback-left hasDatePicker" id="txtDateFrom" placeholder="Start" aria-describedby="inputSuccess1Status1" type="text" runat="server">
                        <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                        <span id="inputSuccess1Status1" class="sr-only">(success)</span>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                        <input class="form-control has-feedback-left hasDatePicker" id="txtDateTo" placeholder="End" aria-describedby="inputSuccess2Status2" type="text" runat="server">
                        <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                        <span id="inputSuccess2Status2" class="sr-only">(success)</span>
                    </div>
                    <div class="col-lg-1 col-md-1 col-sm-1 col-xs-12">
                        <asp:Button ID="Button2" runat="server" Text="Search" OnClick="Button2_Click" CssClass="btn btn-primary btn-block pull-right" />
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Manage Prebooks</h2>
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
                                                <asp:TextBox runat="server" ID="txtKeyField" Text='<%#Eval("number") %>' Style="display: none" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="number" HeaderText="Prebook" SortExpression="number" />
                                        <asp:BoundField DataField="truckDate" HeaderText="Date" SortExpression="truckDate" />
                                        <asp:BoundField DataField="customerName" HeaderText="Customer" SortExpression="customerName" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-group space-20">
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <button type="submit" class="btn btn-danger btn-lg btn-block">Delete</button>
                                </div>
                                <div class="col-md-10 col-sm-10 col-xs-10"></div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <button type="submit" class="btn btn-primary btn-lg btn-block">Move</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
