<%@ Page Title="" Language="C#" MasterPageFile="~/login.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RoyalBooking.login1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div class="login_wrapper">
            <div class="animate form login_form">
                <section class="login_content">
                    <form runat="server">
                        <h1>Login Form</h1>
                        <div>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Username" required=""></asp:TextBox>
                        </div>
                        <div>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Password" required="" TextMode="Password"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <asp:Button ID="ButtonLogin" runat="server" CssClass="btn btn-default submit" Text="Log in" OnClick="ButtonLogin_Click" />
                        </div>
                        <div class="clearfix"></div>
                        <div class="separator">
                            <br />
                            <div>
                                <h1>Royal Booking!</h1>
                                <p>Copyright © 2016 Aphix Inc. All rights reserved.</p>
                            </div>
                        </div>
                    </form>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
