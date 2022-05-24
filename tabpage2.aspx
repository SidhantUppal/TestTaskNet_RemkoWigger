<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="tabpage2.aspx.vb" Inherits="WebApplication1.tabpage2" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v21.2, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <title>- Tabpage -</title>
    <meta name="description" content="Page Title">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no, user-scalable=no, minimal-ui">
    <!-- Call App Mode on ios devices -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <!-- Remove Tap Highlight on Windows Phone IE -->
    <meta name="msapplication-tap-highlight" content="no">
    <!-- base css -->
    <link id="vendorsbundle" rel="stylesheet" media="screen, print" href="css/vendors.bundle.css">
    <link id="appbundle" rel="stylesheet" media="screen, print" href="css/app.bundle.css">
    <%--<link id="mytheme" rel="stylesheet" media="screen, print" href="#">--%>
    <link id="myskin" rel="stylesheet" media="screen, print" href="css/skins/skin-master.css">
    <!-- Place favicon.ico in the root directory -->
    <link rel="apple-touch-icon" sizes="180x180" href="img/favicon/apple-touch-icon.png">
    <link rel="icon" type="image/png" sizes="32x32" href="img/favicon/favicon-32x32.png">
    <link rel="mask-icon" href="img/favicon/safari-pinned-tab.svg" color="#5bbad5">
</head>

<body class="mod-bg-1">
    <script>

        function UpdateContacts(cbo) {
            debugger;

            switch (cbo) {
                case 'org':
                    var cmbOrg = cbo_mtc_organisation;
                    var strID = 'org|' + cmbOrg.GetValue().toString();

                    // Added, Clear manufacturer
                    bgv_Maintenance.GetEditor("id_manufacturer").SetValue("")
                    break;
                case 'man':
                    var cmbMan = bgv_Maintenance.GetEditor("id_manufacturer");
                    var strID = 'man|' + cmbMan.GetValue().toString();
                    // Added, Clear manufacturer
                    bgv_Maintenance.GetEditor("id_mo").SetValue("")
                    break;
                case 'mo':
                    var cmbMO = bgv_Maintenance.GetEditor("id_mo");
                    var strID = 'mo|' + cmbMO.GetValue().toString();
                    break;
                case 'aop':
                    var cmbAOP = bgv_Maintenance.GetEditor("id_air_operator");
                    var strID = 'aop|' + cmbAOP.GetValue().toString();
                    break;
                case 'prs':
                    //alert(cbo_mtc_contact.GetValue().toString());
                    var strID = 'prs|' + cbo_mtc_contact.GetValue().toString()
                default:
            }
            bcp_mtc_Contacts.PerformCallback(strID);
        }

        function bgv_Maintenance_EndCallback(s, e) {
            if (s.IsEditing())
                bgv_Maintenance_changeClientVisibility();
        }

        function bcp_mtc_Contacts_EndCallback(s, e) {
            bgv_Maintenance_changeClientVisibility();
        }

        function bgv_Maintenance_changeClientVisibility() {
            var org = cbo_mtc_organisation.GetValue();
            ////alert(org);
            bgv_Maintenance.GetEditor("id_manufacturer").SetVisible(org == "man");
            bgv_Maintenance.GetEditor("id_mo").SetVisible(org == "mo");
            bgv_Maintenance.GetEditor("id_air_operator").SetVisible(org == "aop");
        }

    </script>
    <form runat="server">

        <!-- BEGIN Page Wrapper -->
        <div class="page-wrapper">
            <div class="page-inner">

                <main id="js-page-content" role="main" class="page-content">


                    <div class="subheader">
                        <div class="row w-100">
                            <div class="col mb-2 mb-lg-0">

                                <h1 class="subheader-title">
                                    <i class='subheader-icon fal fa-cube mr-2'></i>Basket  
                                <small>
                                    <dx:ASPxLabel ID="lbl_Basket" runat="server"></dx:ASPxLabel>
                                </small>
                                    <dx:BootstrapButton ID="btn_User1" runat="server" Text="User 1 / Person 1000009" CssClasses-Control="btn-default btn-sm mr-1" SettingsBootstrap-RenderOption="None" OnCommand="Change_Session" CommandArgument="user1" CausesValidation="False" CssClasses-Icon="fal fa-user"></dx:BootstrapButton>
                                    <dx:BootstrapButton ID="btn_User2" runat="server" Text="User 2 / Person 1000001" CssClasses-Control="btn-default btn-sm mr-1" SettingsBootstrap-RenderOption="None" OnCommand="Change_Session" CommandArgument="user2" CausesValidation="False" CssClasses-Icon="fal fa-user"></dx:BootstrapButton>
                                    <dx:BootstrapButton ID="btn_PostData" runat="server" Text="Post data" CssClasses-Control="btn-default btn-sm mr-1" SettingsBootstrap-RenderOption="None" OnCommand="btn_PostData_Command" CausesValidation="False" CssClasses-Icon="fal fa-user"></dx:BootstrapButton>
                                </h1>
                            </div>
                        </div>
                        <div class="row w-100">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text=""></dx:ASPxLabel>
                        </div>
                    </div>

                    <div class="alert alert-primary">
                        <div class="d-flex flex-start w-100">
                            <div class="d-flex flex-fill">
                                <div class="flex-fill">
                                    <asp:SqlDataSource ID="ds_air_operator" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnectionString %>"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="ds_mo" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnectionString %>"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="ds_manufacturer" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnectionString %>"></asp:SqlDataSource>

                                    <dx:BootstrapPageControl runat="server" ID="BootstrapPageControl1" ViewStateMode="Enabled">
                                        <TabPages>
                                            <dx:BootstrapTabPage Text="Maintenance" ActiveTabIconCssClass="fal fa-check">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                                        <!---------------------------------------------------------------------------------->
                                                        <!--Tab page - Maintenance                                                        -->
                                                        <!---------------------------------------------------------------------------------->

                                                        <asp:SqlDataSource ID="ds_Maintenance" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnectionString %>">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="id_component" Type="Int32" />
                                                            </SelectParameters>
                                                            <InsertParameters>
                                                                <asp:Parameter Name="id_component" Type="Int32" />
                                                                <asp:Parameter Name="id_mo" Type="Int32" />
                                                                <asp:Parameter Name="id_manufacturer" Type="Int32" />
                                                                <asp:Parameter Name="id_air_operator" Type="Int32" />
                                                                <asp:Parameter Name="id_person" Type="Int32" />
                                                                <asp:Parameter Name="work_order_id" Type="String" />
                                                                <asp:Parameter Name="maintenance" Type="String" />
                                                                <asp:Parameter Name="date_finished" Type="DateTime" />
                                                            </InsertParameters>
                                                            <UpdateParameters>
                                                                <asp:Parameter Name="id_maintenance" Type="Int32" />
                                                                <asp:Parameter Name="id_mo" Type="Int32" />
                                                                <asp:Parameter Name="id_manufacturer" Type="Int32" />
                                                                <asp:Parameter Name="id_air_operator" Type="Int32" />
                                                                <asp:Parameter Name="id_person" Type="Int32" />
                                                                <asp:Parameter Name="work_order_id" Type="String" />
                                                                <asp:Parameter Name="maintenance" Type="String" />
                                                                <asp:Parameter Name="date_finished" Type="DateTime" />
                                                            </UpdateParameters>
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="id_maintenance" Type="Int32" />
                                                            </DeleteParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource ID="ds_contact_person" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnectionString %>">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="id" Type="Int32" ConvertEmptyStringToNull="true" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <dx:BootstrapGridView ID="bgv_Maintenance" runat="server" ClientInstanceName="bgv_Maintenance" KeyFieldName="id_maintenance" CssClasses-HeaderRow="bg-primary-50 gridheader" DataSourceID="ds_Maintenance" OnInitNewRow="bgv_Maintenance_InitNewRow" OnRowValidating="bgv_Maintenance_RowValidating" OnStartRowEditing="bgv_Maintenance_StartRowEditing" OnCellEditorInitialize="bgv_Maintenance_CellEditorInitialize" OnRowInserting="bgv_Maintenance_RowInserting" OnRowUpdating="bgv_Maintenance_RowUpdating" OnRowDeleting="bgv_Maintenance_RowDeleting" OnCommandButtonInitialize="bgv_Maintenance_CommandButtonInitialize" OnHtmlEditFormCreated="bgv_Maintenance_HtmlEditFormCreated">
                                                            <CssClasses Table="table-hover" />
                                                            <SettingsBootstrap Striped="True" />
                                                            <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="true" />
                                                            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                            <SettingsDataSecurity AllowEdit="true" AllowInsert="true" AllowDelete="true" />
                                                            <Toolbars>
                                                                <dx:BootstrapGridViewToolbar>
                                                                    <Items>
                                                                        <dx:BootstrapGridViewToolbarItem Command="New" CssClass="btn-default btn-xs mr-1" SettingsBootstrap-RenderOption="None">
                                                                            <SettingsBootstrap RenderOption="None"></SettingsBootstrap>
                                                                        </dx:BootstrapGridViewToolbarItem>
                                                                    </Items>
                                                                </dx:BootstrapGridViewToolbar>
                                                            </Toolbars>
                                                            <SettingsCommandButton EditButton-RenderMode="Image" EditButton-IconCssClass="fal fa-edit" DeleteButton-RenderMode="Image" DeleteButton-IconCssClass="fal fa-trash-alt">
                                                                <EditButton IconCssClass="fal fa-edit" ButtonType="Image" RenderMode="Image"></EditButton>
                                                                <DeleteButton IconCssClass="fal fa-trash-alt" ButtonType="Image" RenderMode="Image"></DeleteButton>
                                                            </SettingsCommandButton>
                                                            <Columns>
                                                                <dx:BootstrapGridViewDataColumn FieldName="id_maintenance" Visible="False" />
                                                                <dx:BootstrapGridViewTextColumn FieldName="mtc_by_org" Visible="False"  />
                                                                <dx:BootstrapGridViewCommandColumn Width="5%" MinWidth="100" MaxWidth="100" ShowEditButton="true" ShowDeleteButton="true" AdaptivePriority="1" VisibleIndex="0" />
                                                                <dx:BootstrapGridViewDateColumn FieldName="date_finished" Width="10%" AdaptivePriority="3">
                                                                    <HeaderCaptionTemplate>Date</HeaderCaptionTemplate>
                                                                    <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy" EditFormatString="dd-MMM-yyyy" UseMaskBehavior="true">
                                                                        <ClearButton DisplayMode="Always" />
                                                                        <ButtonTemplate>
                                                                            <span class="btn btn-secondary dropdown-toggle" data-toggle="dropdown-show"><i class="fal fa-calendar"></i></span>
                                                                        </ButtonTemplate>
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                                                        </ValidationSettings>
                                                                    </PropertiesDateEdit>
                                                                </dx:BootstrapGridViewDateColumn>
                                                                <dx:BootstrapGridViewTextColumn FieldName="work_order_id" Width="10%" AdaptivePriority="4">
                                                                    <HeaderCaptionTemplate>No.</HeaderCaptionTemplate>
                                                                </dx:BootstrapGridViewTextColumn>
                                                                <dx:BootstrapGridViewMemoColumn FieldName="maintenance" AdaptivePriority="2">
                                                                    <HeaderCaptionTemplate>Maintenance</HeaderCaptionTemplate>
                                                                    <PropertiesMemoEdit Rows="8">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                                                        </ValidationSettings>
                                                                    </PropertiesMemoEdit>
                                                                </dx:BootstrapGridViewMemoColumn>
                                                                <dx:BootstrapGridViewTextColumn FieldName="mtc_by" Width="15%" AdaptivePriority="5">
                                                                    <HeaderCaptionTemplate>Maintenance by</HeaderCaptionTemplate>
                                                                </dx:BootstrapGridViewTextColumn>
                                                                <dx:BootstrapGridViewTextColumn FieldName="mtc_by_person" Width="15%" AdaptivePriority="5">
                                                                    <HeaderCaptionTemplate>Maintenance by</HeaderCaptionTemplate>
                                                                </dx:BootstrapGridViewTextColumn>
                                                                <dx:BootstrapGridViewComboBoxColumn FieldName="id_manufacturer" Name="id_manufacturer" Visible="false">
                                                                    <PropertiesComboBox DropDownStyle="DropDown" ValueField="id_manufacturer" TextField="name" NullText="Select a manufacturer" DataSourceID="ds_manufacturer">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateContacts('man'); }"></ClientSideEvents>
                                                                    </PropertiesComboBox>
                                                                </dx:BootstrapGridViewComboBoxColumn>
                                                                <dx:BootstrapGridViewComboBoxColumn FieldName="id_mo" Name="id_mo" Visible="false">
                                                                    <PropertiesComboBox DropDownStyle="DropDown" ValueField="id_mo" TextField="name" NullText="Select a maintenance organisation" DataSourceID="ds_mo">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateContacts('mo'); }"></ClientSideEvents>
                                                                    </PropertiesComboBox>
                                                                </dx:BootstrapGridViewComboBoxColumn>
                                                                <dx:BootstrapGridViewComboBoxColumn FieldName="id_air_operator" Name="id_air_operator" Visible="false">
                                                                    <PropertiesComboBox DropDownStyle="DropDown" ValueField="id_air_operator" TextField="name" NullText="Select an air operator" DataSourceID="ds_air_operator">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateContacts('aop'); }"></ClientSideEvents>
                                                                        <ItemTemplate>
                                                                            <i class='subheader-icon fal fa-<%# IIf(Eval("aop_type") = "company", "warehouse", "user-friends") %>'></i><%# Eval("name") %>
                                                                        </ItemTemplate>
                                                                    </PropertiesComboBox>
                                                                </dx:BootstrapGridViewComboBoxColumn>
                                                                <dx:BootstrapGridViewComboBoxColumn FieldName="id_person" Name="id_person" Visible="false"></dx:BootstrapGridViewComboBoxColumn>
                                                            </Columns>
                                                            <ClientSideEvents EndCallback="bgv_Maintenance_EndCallback" />
                                                            <SettingsPager PageSize="10" NumericButtonCount="6">
                                                                <PageSizeItemSettings Visible="true" Items="10,25,50,100" />
                                                            </SettingsPager>
                                                            <Templates>
                                                                <EditForm>
                                                                    <div class="col-xl-12 container-overflow">
                                                                        <div id="pnl_edit" class="panel min-height container-overflow">
                                                                            <div class="panel-hdr">
                                                                                <h2><i class='fal fa-edit mr-2'></i>Add / edit maintenance
                                                                                </h2>
                                                                            </div>
                                                                            <div class="panel-content m-2 p-2 border border-primary rounded">
                                                                                <div class="row">
                                                                                    <div class="col-xl-6">
                                                                                        <div class="panel">
                                                                                            <div class="panel-container show">
                                                                                                <div class="panel-content">
                                                                                                    <div class="row p-1">
                                                                                                        <div class="col-4">
                                                                                                            Work order ID
                                                                                                        </div>
                                                                                                        <div class="col-8 container-overflow">
                                                                                                            <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_mtc_WO_ID" ReplacementType="EditFormCellEditor" ColumnID="work_order_id" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="row p-1">
                                                                                                        <div class="col-4">
                                                                                                            Date finished
                                                                                                        </div>
                                                                                                        <div class="col-8 container-overflow">
                                                                                                            <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_Date_Finished" ReplacementType="EditFormCellEditor" ColumnID="date_finished" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <dx:BootstrapCallbackPanel runat="server" ID="bcp_mtc_Contacts" ClientInstanceName="bcp_mtc_Contacts" OnCallback="bcp_mtc_Contacts_Callback">
                                                                                                        <ContentCollection>
                                                                                                            <dx:ContentControl runat="server">
                                                                                                                <div class="row p-1">
                                                                                                                    <div class="col-4">
                                                                                                                        Maintenance by
                                                                                                                    </div>
                                                                                                                    <div class="col-8 container-overflow">
                                                                                                                        <dx:BootstrapComboBox ID="cbo_mtc_organisation" runat="server" ClientInstanceName="cbo_mtc_organisation">
                                                                                                                            <Items>
                                                                                                                                <dx:BootstrapListEditItem Value="man" Text="Manufacturer" />
                                                                                                                                <dx:BootstrapListEditItem Value="mo" Text="Maintenance Organisation" />
                                                                                                                                <dx:BootstrapListEditItem Value="aop" Text="Air operator" />
                                                                                                                            </Items>
                                                                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateContacts('org'); }"></ClientSideEvents>
                                                                                                                        </dx:BootstrapComboBox>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="row p-1">
                                                                                                                    <div class="col-4">
                                                                                                                    </div>
                                                                                                                    <div class="col-8 container-overflow">
                                                                                                                        <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_mtc_man" ReplacementType="EditFormCellEditor" ColumnID="id_manufacturer" />
                                                                                                                        <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_mtc_mo" ReplacementType="EditFormCellEditor" ColumnID="id_mo" />
                                                                                                                        <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_mtc_aop" ReplacementType="EditFormCellEditor" ColumnID="id_air_operator" />
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="row p-1">
                                                                                                                    <div class="col-4">
                                                                                                                    </div>
                                                                                                                    <div class="col-8 container-overflow">
                                                                                                                        <dx:BootstrapComboBox ID="cbo_mtc_contact" runat="server" ClientInstanceName="cbo_mtc_contact" ValueField="id_person" TextField="name" NullText="Select a contact" AllowNull="true" OnInit="cbo_mtc_contact_Init">
                                                                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateContacts('prs'); }" />
                                                                                                                        </dx:BootstrapComboBox>
                                                                                                                        <dx:BootstrapTextBox ID="txt_test" runat="server"></dx:BootstrapTextBox>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </dx:ContentControl>
                                                                                                        </ContentCollection>
                                                                                                        <ClientSideEvents EndCallback="bcp_mtc_Contacts_EndCallback" />
                                                                                                    </dx:BootstrapCallbackPanel>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-xl-6">
                                                                                        <div class="panel">
                                                                                            <div class="panel-container show">
                                                                                                <div class="panel-content">
                                                                                                    <div class="row p-1">
                                                                                                        <div class="col">
                                                                                                            Maintenance
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="row p-1">
                                                                                                        <div class="col container-overflow">
                                                                                                            <dx:BootstrapGridViewTemplateReplacement runat="server" ID="gtr_mtc_Maintenance" ReplacementType="EditFormCellEditor" ColumnID="maintenance" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row p-1">
                                                                                    <div class="col col-lg-2">
                                                                                    </div>
                                                                                    <div class="col-lg-6 container-overflow">
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row p-1">
                                                                                    <div class="align-right">
                                                                                        <dx:BootstrapGridViewTemplateReplacement ID="gtr_mtc_UpdateButton" ReplacementType="EditFormUpdateButton" runat="server" />
                                                                                        <dx:BootstrapGridViewTemplateReplacement ID="gtr_mtc_CancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </EditForm>
                                                            </Templates>
                                                        </dx:BootstrapGridView>

                                                    </dx:ContentControl>
                                                </ContentCollection>
                                            </dx:BootstrapTabPage>
                                            <%------------------------------------------------------------------------------
                                                    Tab page - Tab 2
                                                    ------------------------------------------------------------------------------%>
                                            <dx:BootstrapTabPage Text="Tab 2" ActiveTabIconCssClass="fal fa-check">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                                    </dx:ContentControl>
                                                </ContentCollection>
                                            </dx:BootstrapTabPage>
                                        </TabPages>
                                    </dx:BootstrapPageControl>

                                </div>
                            </div>
                        </div>
                    </div>

                </main>

            </div>
        </div>

        <script src="js/vendors.bundle.js"></script>
        <script src="js/app.bundle.js"></script>
    </form>

</body>

</html>
