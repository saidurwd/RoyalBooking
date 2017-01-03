<%@ Page Title="" Language="C#" MasterPageFile="~/login.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="RoyalBooking.company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="margin-top: 50px;">
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 col-lg-offset-3 col-md-offset-3 col-sm-offset-3">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Select your company</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-sm-4 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <asp:HyperLink ID="HyperLink4" NavigateUrl="~/index.aspx?id=1" runat="server">
                                    <img class="img-responsive" alt="" src="../images/logo.png">
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-sm-8 col-md-8 col-sm-8 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <div class="caption" style="margin-top: 10px;">
                                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/index.aspx?id=1" runat="server" CssClass="btn btn-primary btn-lg btn-block">
                                        Royal Flowers Inc. Domestic
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-sm-4 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <asp:HyperLink ID="HyperLink5" NavigateUrl="~/index.aspx?id=1" runat="server">
                                    <img class="img-responsive" alt="" src="../images/logo.png">
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-sm-8 col-md-8 col-sm-8 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <div class="caption" style="margin-top: 10px;">
                                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/index.aspx?id=2" runat="server" CssClass="btn btn-primary btn-lg btn-block">
                                        Royal Flowers Inc. International
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-sm-4 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <asp:HyperLink ID="HyperLink6" NavigateUrl="~/index.aspx?id=1" runat="server">
                                    <img class="img-responsive" alt="" src="../images/logo.png">
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-sm-8 col-md-8 col-sm-8 col-xs-12">
                            <div class="thumbnail" style="height: 100px;">
                                <div class="caption" style="margin-top: 10px;">
                                    <asp:HyperLink ID="HyperLink3" NavigateUrl="~/index.aspx?id=3" runat="server" CssClass="btn btn-primary btn-lg btn-block">
                                        Royal Flowers GmbH
                                    </asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
