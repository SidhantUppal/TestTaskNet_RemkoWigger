Imports DevExpress.Web
Imports DevExpress.Web.Bootstrap

Public Class tabpage2
    Inherits System.Web.UI.Page

    Dim id_component As Integer
    Dim lst_user_aop As List(Of Integer)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        InitialValues()
        Permission()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ASPxLabel1.Text = "Person: " & Session("id_person") & ", access level: " & Session("access_level")

        CustomizeDataSources()

        If Not Page.IsPostBack Then

        Else

        End If

    End Sub

    Sub InitialValues()

        If Session("access_level") = Nothing Then
            Session("access_level") = 2
        End If
        If Session("id_person") = Nothing Then
            Session("id_person") = 1000001
        End If

    End Sub

    Sub Permission()

        lst_user_aop = New List(Of Integer)
        If Session("access_level") = 1 Then
            lst_user_aop.Add(1000004)
        Else
            lst_user_aop.Add(1000001)
            lst_user_aop.Add(1000004)
            lst_user_aop.Add(1000005)
            lst_user_aop.Add(1000007)
        End If

    End Sub

    Sub CustomizeDataSources()

        Dim str_SQL_select As String = ""
        Dim str_SQL_insert As String = ""
        Dim str_SQL_update As String = ""
        Dim str_SQL_delete As String = ""

        ' Companies
        '----------------------------------------------------------------
        str_SQL_select = "select * " &
                         "from   [air_operator] " &
                         "where  [id_air_operator] in ({0}) "

        ds_air_operator.SelectCommand = String.Format(str_SQL_select, String.Join(", ", lst_user_aop))


        ' Maintenance Organisation
        '----------------------------------------------------------------
        str_SQL_select = "select * " &
                         "from   [mo] " &
                         "order by [name] "

        ds_mo.SelectCommand = str_SQL_select

        ' Manufacturer
        '----------------------------------------------------------------
        str_SQL_select = "select * " &
                         "from   [manufacturer] " &
                         "order by [name] "

        ds_manufacturer.SelectCommand = str_SQL_select

        ' Maintenance
        '----------------------------------------------------------------
        str_SQL_select = "select * " &
                         " ,     case when [man].[id_manufacturer] is not null then 'Manufacturer' " &
                         "            when [mo].[id_mo] is not null then 'Maintenance Organisation' " &
                         "            when [aop].[id_air_operator] is not null then 'Air Operator' " &
                         "       end as mtc_by_org " &
                         " ,     case when [man].[id_manufacturer] is not null then [man].[name] " &
                         "            when [mo].[id_mo] is not null then [mo].[name] " &
                         "            when [aop].[id_air_operator] is not null then [aop].[name] " &
                         "       end as mtc_by " &
                         " ,     [prs].[name] as mtc_by_person " &
                         "from   [maintenance] mtc left join [manufacturer] man on [mtc].[man_id_manufacturer] = [man].[id_manufacturer] " &
                         "                         left join [mo] mo on [mtc].[mo_id_mo] = [mo].[id_mo] " &
                         "                         left join [air_operator] aop on [mtc].[aop_id_air_operator] = [aop].[id_air_operator] " &
                         "                         left join [person] prs on [mtc].[prs_id_person] = [prs].[id_person] " &
                         "where  [mtc].[id_component] = @id_component " &
                         "order by [mtc].[date_finished] desc "

        ds_Maintenance.SelectCommand = str_SQL_select
        ds_Maintenance.SelectParameters("id_component").DefaultValue = 1000123

        str_SQL_insert = "insert into [maintenance] (" &
                         "       [id_component] " &
                         " ,     [man_id_manufacturer] " &
                         " ,     [mo_id_mo] " &
                         " ,     [aop_id_air_operator] " &
                         " ,     [prs_id_person] " &
                         " ,     [work_order_id] " &
                         " ,     [date_finished] " &
                         " ,     [maintenance] " &
                         "       ) values (" &
                         "       @id_component " &
                         " ,     @id_manufacturer " &
                         " ,     @id_mo " &
                         " ,     @id_air_operator " &
                         " ,     @id_person " &
                         " ,     @work_order_id " &
                         " ,     @date_finished " &
                         " ,     @maintenance " &
                         ") "

        ds_Maintenance.InsertCommand = str_SQL_insert

        str_SQL_update = "update [maintenance] " &
                         "set    [date_finished]       = @date_finished " &
                         " ,     [work_order_id]       = @work_order_id " &
                         " ,     [maintenance]         = @maintenance " &
                         " ,     [man_id_manufacturer] = @id_manufacturer " &
                         " ,     [mo_id_mo]            = @id_mo " &
                         " ,     [aop_id_air_operator] = @id_air_operator " &
                         " ,     [prs_id_person]       = @id_person " &
                         "where  [id_maintenance] = @id_maintenance "

        ds_Maintenance.UpdateCommand = str_SQL_update

        str_SQL_delete = "delete from [maintenance] " &
                         "where  [id_maintenance] = @id_maintenance "

        ds_Maintenance.DeleteCommand = str_SQL_delete

    End Sub


    Protected Sub bgv_Maintenance_CommandButtonInitialize(sender As Object, e As BootstrapGridViewCommandButtonEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Show/hide command buttons
        '
        '----------------------------------------------------------------

        If e.VisibleIndex < 0 Then Return

        If bgv_Maintenance.VisibleRowCount > 0 Then

            Dim bln_edit As Boolean = True
            Dim bln_delete As Boolean = True

            If Session("access_level") = 1 Then

                If Not IsDBNull(bgv_Maintenance.GetRowValues(e.VisibleIndex, "id_air_operator")) Then
                    If lst_user_aop.Contains(bgv_Maintenance.GetRowValues(e.VisibleIndex, "id_air_operator")) Then
                        bln_edit = True
                        bln_delete = True
                    Else
                        bln_edit = False
                        bln_delete = False
                    End If
                Else
                    bln_edit = False
                    bln_delete = False
                End If

            End If

            If e.ButtonType = ColumnCommandButtonType.Edit Then
                e.Visible = bln_edit
            End If
            If e.ButtonType = ColumnCommandButtonType.Delete Then
                e.Visible = bln_delete
            End If

        End If

    End Sub

    Protected Sub bgv_Maintenance_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Set new values for update
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_org, cbo_con As BootstrapComboBox
        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)
        cbo_con = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

        Select Case cbo_org.Value
            Case "man"
                ds_Maintenance.UpdateParameters("id_manufacturer").DefaultValue = e.NewValues("id_manufacturer") 'Session("mtc_id_cmy")
            Case "mo"
                ds_Maintenance.UpdateParameters("id_mo").DefaultValue = e.NewValues("id_mo")'Session("mtc_id_cmy")
            Case "aop"
                ds_Maintenance.UpdateParameters("id_air_operator").DefaultValue = e.NewValues("id_air_operator") 'Session("mtc_id_cmy")
        End Select

        ds_Maintenance.UpdateParameters("id_person").DefaultValue = Session("mtc_id_prs") 'cbo_con.SelectedItem.Value
        ds_Maintenance.UpdateParameters("work_order_id").DefaultValue = e.NewValues("work_order_id")
        ds_Maintenance.UpdateParameters("maintenance").DefaultValue = e.NewValues("maintenance")
        ds_Maintenance.UpdateParameters("date_finished").DefaultValue = e.NewValues("date_finished")
        ds_Maintenance.UpdateParameters("id_maintenance").DefaultValue = bgv_Maintenance.GetRowValuesByKeyValue(e.Keys(0), "id_maintenance")
        ds_Maintenance.Update()

        e.Cancel = True
        bgv_Maintenance.CancelEdit()

    End Sub

    Protected Sub bgv_Maintenance_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Set new values for insert
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_org, cbo_con As BootstrapComboBox
        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)
        cbo_con = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

        Select Case cbo_org.Value
            Case "man"
                ds_Maintenance.InsertParameters("id_manufacturer").DefaultValue = e.NewValues("id_manufacturer")'Session("mtc_id_cmy")
            Case "mo"
                ds_Maintenance.InsertParameters("id_mo").DefaultValue = e.NewValues("id_mo")'Session("mtc_id_cmy")
            Case "aop"
                ds_Maintenance.InsertParameters("id_air_operator").DefaultValue = e.NewValues("id_air_operator") 'Session("mtc_id_cmy")
        End Select

        ds_Maintenance.InsertParameters("id_person").DefaultValue = Session("mtc_id_prs") 'cbo_con.Value
        ds_Maintenance.InsertParameters("work_order_id").DefaultValue = e.NewValues("work_order_id")
        ds_Maintenance.InsertParameters("maintenance").DefaultValue = e.NewValues("maintenance")
        ds_Maintenance.InsertParameters("date_finished").DefaultValue = e.NewValues("date_finished")
        ds_Maintenance.InsertParameters("id_component").DefaultValue = 1000123
        ds_Maintenance.Insert()

        e.Cancel = True
        bgv_Maintenance.CancelEdit()

    End Sub

    Protected Sub bgv_Maintenance_RowDeleting(sender As Object, e As Data.ASPxDataDeletingEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Set new values for delete
        '
        '----------------------------------------------------------------

        ds_Maintenance.DeleteParameters("id_maintenance").DefaultValue = e.Values("id_maintenance")
        ds_Maintenance.Delete()

        e.Cancel = True

    End Sub

    Protected Sub bgv_Maintenance_InitNewRow(sender As Object, e As DevExpress.Web.Data.ASPxDataInitNewRowEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Actions upon initializing new row
        '
        '----------------------------------------------------------------

        e.NewValues("date_finished") = Now()

    End Sub

    Protected Sub bgv_Maintenance_RowValidating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataValidationEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Actions upon row validating
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_org As BootstrapComboBox
        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)

        If cbo_org.Value = "man" Then
            If e.NewValues("id_manufacturer") Is Nothing Then
                AddError(e.Errors, bgv_Maintenance.Columns("id_manufacturer"), "Please, select a manufacturer.")
            End If
        End If

        If cbo_org.Value = "mo" Then
            If e.NewValues("id_mo") Is Nothing Then
                AddError(e.Errors, bgv_Maintenance.Columns("id_mo"), "Please, select a maintenance organisation.")
            End If
        End If

        If cbo_org.Value = "aop" Then
            If e.NewValues("id_air_operator") Is Nothing Then
                AddError(e.Errors, bgv_Maintenance.Columns("id_air_operator"), "Please, select an air operator.")
            End If
        End If

        'If String.IsNullOrEmpty(e.RowError) AndAlso e.Errors.Count > 0 Then
        '    e.RowError = "Please, correct all errors."
        'End If

    End Sub

    Protected Sub bgv_Maintenance_StartRowEditing(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxStartRowEditingEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Actions upon start row editing
        '
        '----------------------------------------------------------------

        Session("mtc_id_prs") = Nothing
        Session("mtc_id_cmy") = Nothing

        If Not bgv_Maintenance.IsNewRowEditing Then
            bgv_Maintenance.DoRowValidation()
        End If

    End Sub

    Protected Sub bgv_Maintenance_CellEditorInitialize(ByVal sender As Object, ByVal e As BootstrapGridViewEditorEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Actions upon cell editor initializing
        '
        '----------------------------------------------------------------

        Dim cbo_editor As BootstrapComboBox
        If bgv_Maintenance.IsNewRowEditing Then

            Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

            Dim cbo_org, cbo_con As BootstrapComboBox
            cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)
            cbo_con = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

            If Session("access_level") = 1 Then

                If e.Column.FieldName = "id_manufacturer" Then
                    cbo_editor = e.Editor
                    cbo_editor.ClientVisible = False
                End If
                If e.Column.FieldName = "id_mo" Then
                    cbo_editor = e.Editor
                    cbo_editor.ClientVisible = False
                End If
                If e.Column.FieldName = "id_air_operator" Then
                    cbo_editor = e.Editor
                    If lst_user_aop.Count = 1 Then
                        cbo_editor.SelectedIndex = 0
                        bgv_Maintenance_Contacts("aop", lst_user_aop.Item(0))
                        For Each item In cbo_con.Items
                            If item.value = Session("id_person") Then
                                cbo_con.SelectedItem = item
                            End If
                        Next
                    Else
                        cbo_editor.SelectedIndex = -1
                        cbo_editor.ClientEnabled = True
                    End If
                End If

            Else
                If e.Column.FieldName = "id_manufacturer" Then
                    cbo_editor = e.Editor
                    cbo_editor.ReadOnly = False
                End If
                If e.Column.FieldName = "id_mo" Then
                    cbo_editor = e.Editor
                    cbo_editor.ReadOnly = False
                End If
                If e.Column.FieldName = "id_air_operator" Then
                    cbo_editor = e.Editor
                    cbo_editor.ReadOnly = False
                End If
            End If

        End If

        CType(e.Editor, ASPxEdit).ValidationSettings.Display = Display.Dynamic

    End Sub

    Protected Sub bgv_Maintenance_HtmlEditFormCreated(sender As Object, e As ASPxGridViewEditFormEventArgs)

        '----------------------------------------------------------------
        '
        ' Description : Maintenance - Actions on HTML form created
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_org, cbo_con As BootstrapComboBox
        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)
        cbo_con = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)

        Dim ddd = cbo_org.SelectedItem
        Dim dddd = cbo_org.SelectedIndex
        Dim ddddd = cbo_org.Value

        If Session("access_level") = 1 Then
            cbo_org.Items.Remove(cbo_org.Items.FindByValue("man"))
            cbo_org.Items.Remove(cbo_org.Items.FindByValue("mo"))
        End If

        If bgv_Maintenance.IsNewRowEditing Then

            If Session("access_level") = 1 Then
                cbo_org.Value = "aop"
                'cbo_org.ClientEnabled = False
            Else
                cbo_org.Value = "mo"
            End If

        Else

            Dim id As String
            Select Case bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"mtc_by_org"}).ToString()
                Case "Manufacturer"
                    cbo_org.Value = "man"
                    id = bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_manufacturer"}).ToString()
                    'Session("mtc_id_cmy") = id
                    bgv_Maintenance_Contacts("man", id)
                Case "Maintenance Organisation"
                    cbo_org.Value = "mo"
                    id = bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_mo"}).ToString()
                    'Session("mtc_id_cmy") = id
                    bgv_Maintenance_Contacts("mo", id)
                Case "Air Operator"
                    cbo_org.Value = "aop"
                    id = bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_air_operator"}).ToString()
                    'Session("mtc_id_cmy") = id
                    bgv_Maintenance_Contacts("aop", id)
                Case Else
                    cbo_org.Value = "aop"
                    cbo_org.ClientEnabled = False
            End Select

            Dim id_person As String = bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_person"}).ToString

            If Session("mtc_id_prs") Is Nothing Then
                Session("mtc_id_prs") = id_person
            End If

            cbo_con.Value = Session("mtc_id_prs")

        End If

    End Sub

    Protected Sub bcp_mtc_Contacts_Callback(sender As Object, e As CallbackEventArgsBase)

        '----------------------------------------------------------------
        '
        ' Description : Refresh panel contents
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_org, cbo_con As BootstrapComboBox
        cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)
        cbo_con = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

        Dim whatArray() As String = Split(e.Parameter, "|")
        Dim what As String = whatArray(0)
        Dim val As String = whatArray(1)

        Dim str_SQL As String = ""
        Select Case what
            Case "org"

                cbo_con.SelectedIndex = -1
                cbo_con.ClientEnabled = False
                Session("org") = val

            Case "man"

                cbo_con.ClientEnabled = True
                Session("mtc_id_cmy") = val
                bgv_Maintenance_Contacts("man", val)

            Case "mo"

                cbo_con.ClientEnabled = True
                Session("mtc_id_cmy") = val
                bgv_Maintenance_Contacts("mo", val)

            Case "aop"

                cbo_con.ClientEnabled = True
                Session("mtc_id_cmy") = val
                bgv_Maintenance_Contacts("aop", val)

            Case "prs"

                Session("mtc_id_prs") = val

                Select Case cbo_org.Value
                    Case "man"
                        'bgv_Maintenance_Contacts("man", bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_manufacturer"}).ToString())
                        bgv_Maintenance_Contacts("man", Session("mtc_id_cmy"))
                    Case "mo"
                        'bgv_Maintenance_Contacts("mo", bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_mo"}).ToString())
                        bgv_Maintenance_Contacts("mo", Session("mtc_id_cmy"))
                    Case "aop"
                        'bgv_Maintenance_Contacts("aop", bgv_Maintenance.GetRowValues(bgv_Maintenance.EditingRowVisibleIndex, New String() {"id_air_operator"}).ToString())
                        bgv_Maintenance_Contacts("aop", Session("mtc_id_cmy"))
                End Select

                cbo_con.Value = Session("mtc_id_prs")

        End Select

        Dim org_options() As String = {"man", "mo", "aop"}
        If org_options.Contains(what) Then

            If cbo_con.Items.Count = 1 Then
                cbo_con.SelectedIndex = 0
                Session("mtc_id_prs") = cbo_con.Value
            Else
                cbo_con.SelectedIndex = -1
                Session("mtc_id_prs") = Nothing
            End If

        End If

    End Sub

    Protected Sub bgv_Maintenance_Contacts(ByVal org As String, ByVal id As Integer)

        '----------------------------------------------------------------
        '
        ' Description : Show company contacts
        '
        '----------------------------------------------------------------

        Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

        Dim cbo_con As BootstrapComboBox = TryCast(bcp_con.FindControl("cbo_mtc_contact"), BootstrapComboBox)

        Dim str_SQL As String = ""
        Select Case org
            Case "man"

                str_SQL = "select * " &
                          "from   [person] " &
                          "where  [contact_for_man] = @id "

            Case "mo"

                str_SQL = "select * " &
                          "from   [person] " &
                          "where  [contact_for_mo] = @id "

            Case "aop"

                str_SQL = "select * " &
                          "from   [person] " &
                          "where  [contact_for_aop] = @id "

        End Select

        ds_contact_person.SelectCommand = str_SQL
        ds_contact_person.SelectParameters("id").DefaultValue = id

        cbo_con.DataSource = ds_contact_person
        cbo_con.DataBind()

    End Sub


    Private Sub AddError(ByVal errors As Dictionary(Of GridViewColumn, String), ByVal column As GridViewColumn, ByVal errorText As String)

        '----------------------------------------------------------------
        '
        ' Description : Add error
        '
        '----------------------------------------------------------------

        If errors.ContainsKey(column) Then
            Return
        End If
        errors(column) = errorText

    End Sub

    Protected Sub Change_Session(sender As Object, e As CommandEventArgs)

        Select Case e.CommandArgument
            Case "user1"
                Session("access_level") = 1
                Session("id_person") = 1000009
            Case "user2"
                Session("access_level") = 2
                Session("id_person") = 1000001
        End Select

        Response.Redirect(Request.RawUrl)

    End Sub

    Protected Sub cbo_mtc_contact_Init(sender As Object, e As EventArgs)

        If Session("org") = Nothing Then

            'Dim bcp_con As BootstrapCallbackPanel = TryCast(bgv_Maintenance.FindEditFormTemplateControl("bcp_mtc_Contacts"), BootstrapCallbackPanel)

            Dim cbo_con As BootstrapComboBox
            'cbo_org = TryCast(bcp_con.FindControl("cbo_mtc_organisation"), BootstrapComboBox)


            cbo_con = TryCast(sender, BootstrapComboBox)

            Dim str_SQL As String = ""
            Select Case Session("org")
                Case "man"

                    str_SQL = "select * " &
                              "from   [person] " &
                              "where  [contact_for_man] = @id "

                Case "mo"

                    str_SQL = "select * " &
                              "from   [person] " &
                              "where  [contact_for_mo] = @id "

                Case "aop"

                    str_SQL = "select * " &
                              "from   [person] " &
                              "where  [contact_for_aop] = @id "

            End Select


            ds_contact_person.SelectCommand = str_SQL
            ds_contact_person.SelectParameters("id").DefaultValue = Session("mtc_id_cmy")

            cbo_con.DataSource = ds_contact_person
            cbo_con.DataBind()

        End If

    End Sub

    Protected Sub btn_PostData_Command(sender As Object, e As CommandEventArgs)


        Dim str_keys As String() = Request.Form.AllKeys
        'Response.Write(Now())
        For i As Integer = 0 To str_keys.Length - 1
            'Response.Write(str_keys(i) & ": " & Request.Form(str_keys(i)) & "<br>")
            ASPxLabel1.Text += str_keys(i) & ": " & Request.Form(str_keys(i)) & vbCrLf
        Next
        'ASPxLabel1.Text = Now()

    End Sub
End Class