<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="prebooks.aspx.cs" Inherits="RoyalBooking.prebooks" %>

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
                    <input class="form-control has-feedback-left" id="inputSuccess2" placeholder="" type="text">
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12 form-group"></div>
                <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                    <input class="form-control has-feedback-left" id="single_cal1" placeholder="Start" aria-describedby="inputSuccess1Status1" type="text">
                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                    <span id="inputSuccess1Status1" class="sr-only">(success)</span>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-12 form-group">
                    <input class="form-control has-feedback-left" id="single_cal2" placeholder="End" aria-describedby="inputSuccess2Status2" type="text">
                    <span class="fa fa-calendar-o form-control-feedback left" aria-hidden="true"></span>
                    <span id="inputSuccess2Status2" class="sr-only">(success)</span>
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
                        <div class="table-responsive">
                            <table class="table table-striped jambo_table bulk_action">
                                <thead>
                                    <tr class="headings">
                                        <th>
                                            <input type="checkbox" id="check-all" class="flat">
                                        </th>
                                        <th class="column-title">Prebook </th>
                                        <th class="column-title">Date </th>
                                        <th class="column-title">Customer </th>
                                        <th class="bulk-actions" colspan="7">
                                            <a class="antoo" style="color: #fff; font-weight: 500;">Bulk Actions ( <span class="action-cnt"></span>) <i class="fa fa-chevron-down"></i></a>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="even pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000040</td>
                                        <td class=" ">May 23, 2014 11:47:56 PM </td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="odd pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000039</td>
                                        <td class=" ">May 23, 2014 11:30:12 PM</td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="even pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000040</td>
                                        <td class=" ">May 23, 2014 11:47:56 PM </td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="odd pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000039</td>
                                        <td class=" ">May 23, 2014 11:30:12 PM</td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="even pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000040</td>
                                        <td class=" ">May 23, 2014 11:47:56 PM </td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="odd pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000039</td>
                                        <td class=" ">May 23, 2014 11:30:12 PM</td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="even pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000040</td>
                                        <td class=" ">May 23, 2014 11:47:56 PM </td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="odd pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000039</td>
                                        <td class=" ">May 23, 2014 11:30:12 PM</td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="even pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000040</td>
                                        <td class=" ">May 23, 2014 11:47:56 PM </td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                    <tr class="odd pointer">
                                        <td class="a-center ">
                                            <input type="checkbox" class="flat" name="table_records">
                                        </td>
                                        <td class=" ">121000039</td>
                                        <td class=" ">May 23, 2014 11:30:12 PM</td>
                                        <td class=" ">John Blank L</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="form-group">
                                <div class="col-md-11 col-sm-11 col-xs-11 col-md-offset-1">
                                    <button type="submit" class="btn btn-danger">Delete</button>
                                    <button type="submit" class="btn btn-primary">Move</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </form>
</asp:Content>
