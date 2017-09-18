Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports DevExpress.XtraSplashScreen
Public Class FrmRoleAccesRight
    Private Sub FrmRoleAccesRight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "ROLE ACCESS RIGHT"
        fillCbRole()
        CreateHeaderMenu()
        LoadRoleMenu(ComboBoxEdit1.Text)
        LoadRoleMember(ComboBoxEdit1.Text)
    End Sub

    Private Sub fillCbRole()
        SQL = "SELECT DISTINCT ROLEID,ROLENAME FROM T_ROLE WHERE AKTIF='Y' ORDER BY ROLENAME "
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit1, False)
    End Sub

    Private Sub CreateHeaderMenu()
        Dim edit As RepositoryItemCheckEdit = New RepositoryItemCheckEdit()
        edit.ValueChecked = "TRUE"
        edit.ValueUnchecked = "FALSE"

        'MENU
        Dim View4 As ColumnView = CType(GridControl4.MainView, ColumnView)
        Dim FieldNames4() As String = New String() {"GROUP_NAME", "MENUID", "MENU", "CHK"}
        Dim I As Integer
        Dim Column4 As DevExpress.XtraGrid.Columns.GridColumn
        View4.Columns.Clear()
        For I4 = 0 To FieldNames4.Length - 1
            Column4 = View4.Columns.AddField(FieldNames4(I4))
            Column4.VisibleIndex = I
        Next
        View4.Columns("CHK").ColumnEdit = edit
        View4.Columns("CHK").OptionsColumn.AllowEdit = True
        View4.Columns("GROUP_NAME").OptionsColumn.AllowEdit = False
        View4.Columns("MENUID").OptionsColumn.AllowEdit = False
        View4.Columns("MENU").OptionsColumn.AllowEdit = False

        Dim GridView4 As GridView = CType(GridControl4.FocusedView, GridView)
        GridView4.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView4.Columns("GROUP_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView4.Columns("MENUID"), DevExpress.Data.ColumnSortOrder.Ascending)
        }, 1)
        GridView4.OptionsView.ShowGroupPanel = False

        'USER
        Dim View2 As ColumnView = CType(GridControl2.MainView, ColumnView)
        Dim FieldNames2() As String = New String() {"USERNAME", "CHK"}
        Dim I2 As Integer
        Dim Column2 As DevExpress.XtraGrid.Columns.GridColumn

        View2.Columns.Clear()
        For I2 = 0 To FieldNames2.Length - 1
            Column2 = View2.Columns.AddField(FieldNames2(I2))
            Column2.VisibleIndex = I2
        Next
        View2.Columns("CHK").ColumnEdit = edit
        View2.Columns("USERNAME").OptionsColumn.AllowEdit = False
    End Sub

    Private Sub LoadRoleMember(ByVal role As String)
        'DATA USER NAME (MEMBER ROLE)
        If ComboBoxEdit1.Text <> "" THEn
            Dim ROLEID As String = GetCodeRole(ComboBoxEdit1.Text)
            SQL = "SELECT B.USERNAME ,B.ROLEID,B.USERID, " +
            " Case WHEN " +
            " (SELECT COUNT(A.ROLEID) FROM T_ROLEACCESSRIGHTS A " +
            " WHERE a.ROLEID = b.ROLEID And a.ROLEID ='" & ROLEID & "')=0 THEN 'FALSE' ELSE 'TRUE' END " +
            " AS CHK " +
            " FROM T_USERPROFILE B " +
            " WHERE B.USERNAME Is Not null " +
            " Order By B.USERID "
        Else
            SQL = "SELECT B.USERNAME ,B.ROLEID,B.USERID, " +
            " 'FALSE' " +
            " AS CHK " +
            " FROM T_USERPROFILE B " +
            " WHERE B.USERNAME Is Not null " +
            " Order By B.USERID "
        End If
        FILLGridView(SQL, GridControl2)
        'GridView2.OptionsBehavior.Editable = True
        GridView2.ExpandAllGroups()
        'GridView2.Columns("CHK").OptionsColumn.AllowEdit = True

    End Sub
    Private Sub LoadRoleMenu(ByVal ROLENAME As String)
        'DATA MENU
        If ComboBoxEdit1.Text <> "" THEn
            Dim ROLEID As String = GetCodeRole(ComboBoxEdit1.Text)
            'SQL = "SELECT NVL(b.ACCESSID, a.ACCESSID) As GROUP_ID,NVL(B.ACCESSNAME,A.ACCESSNAME) As GROUP_NAME " +
            SQL = "SELECT CONCAT(CONCAT(B.URUT,'.'),NVL(B.ACCESSNAME,A.ACCESSNAME)) As GROUP_NAME " +
            ",A.ACCESSID As MENUID,A.ACCESSNAME As MENU, " +
            " Case a.TYPE " +
            " WHEN '0' THEN 'GROUP' " +
            " WHEN '1' THEN 'MENU' " +
            " End As TYPEMENU, " +
            " CASE WHEN " +
            " (SELECT COUNT(C.ACCESSID) " +
            " FROM T_ROLEACCESSRIGHTS C " +
            " WHERE C.ACCESSID = a.ACCESSID And C.ROLEID ='" & ROLEID & "')=0 " +
            " THEN 'FALSE'" +
            " ELSE 'TRUE'" +
            " END" +
            " AS CHK " +
            " FROM T_ACCESSRIGHTS A  " +
            " Left Join T_ACCESSRIGHTS B On A.PARENTID=B.ACCESSID " +
            " WHERE  A.TYPE=1" +
            " ORDER BY A.ACCESSID, b.ACCESSNAME "
        Else
            'SQL = "SELECT NVL(b.ACCESSID, a.ACCESSID) As GROUP_ID,NVL(B.ACCESSNAME,A.ACCESSNAME) As GROUP_NAME" +
            SQL = "SELECT CONCAT(CONCAT(B.URUT,'.'),NVL(B.ACCESSNAME,A.ACCESSNAME)) As GROUP_NAME " +
                ",A.ACCESSID As MENUID,A.ACCESSNAME As MENU, " +
            " Case a.TYPE " +
            " WHEN '0' THEN 'GROUP' " +
            " WHEN '1' THEN 'MENU' " +
            " End As TYPEMENU, " +
            " 'FALSE' AS CHK " +
            " FROM T_ACCESSRIGHTS A  " +
            " Left Join T_ACCESSRIGHTS B On A.PARENTID=B.ACCESSID " +
            " WHERE  A.TYPE=1" +
            " ORDER BY A.ACCESSID, b.ACCESSNAME "
        End If
        FILLGridView(SQL, GridControl4)
        GridView4.OptionsBehavior.Editable = True
        GridView4.ExpandAllGroups()
        GridView4.Columns("CHK").OptionsColumn.AllowEdit = True

    End Sub
    Private Sub FrmRoleAccesRight_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Width = Me.Width - (GridControl2.Width + Panel1.Width + 10)
    End Sub

    Private Sub ComboBoxEdit1_SELECTedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEdit1.SELECTedIndexChanged
        LoadRoleMember(ComboBoxEdit1.Text)
        LoadRoleMenu(ComboBoxEdit1.Text)
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'close
        Me.Close()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'load
        LoadRoleMenu(ComboBoxEdit1.Text)
        LoadRoleMember(ComboBoxEdit1.Text)
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'user
        Dim i As Integer
        Dim X As Integer
        Dim Roleid As String = GetCodeRole(ComboBoxEdit1.Text)
        Dim user As String = GetCodeRole(ComboBoxEdit1.Text)

        SplashScreenManager.ShowDefaultWaitForm()
        For i = 0 To GridView2.RowCount - 1
            Dim GView As GridView = CType(GridControl2.FocusedView, GridView)
            Dim Userid As String = GetCodeUser(CType((GView.GetRowCellValue(i, "USERNAME")), String))

            Dim nInsert As Boolean = CType((GView.GetRowCellValue(i, "CHK")), String)
            If nInsert = True THEn
                UpdateUserRole(Userid, Roleid)
            End If
        Next
        LoadRoleMember(ComboBoxEdit1.Text)
        SQL = "SELECT * FROM T_ACCESSRIGHTS WHERE TYPE=1"
        X = CheckRecord(SQL)

        SQL = "DELETE  T_ROLEACCESSRIGHTS WHERE ROLEID='" & Roleid & "'"
            ExecuteNonQuery(SQL)
        'menu
        If X > 0 THEn
            For i = 0 To X - 1
                Dim GView As GridView = CType(GridControl4.FocusedView, GridView)
                Dim Userid As String = GetCodeUser(CType((GView.GetRowCellValue(i, "USERNAME")), String))
                Dim nInsert As Boolean = CType((GView.GetRowCellValue(i, "CHK")), String)
                Dim MenuID As String = CType((GView.GetRowCellValue(i, "MENUID")), String)
                If nInsert = True THEn
                    UpdateUserMenu(Userid, Roleid, MenuID)
                    UpdateMenuGroup(Userid, MenuID, Roleid)
                End If
            Next
        End If
        'GROUP


        LoadRoleMember(ComboBoxEdit1.Text)
        SplashScreenManager.CloseDefaultWaitForm()

        MsgBox("Update Successful", vbInformation, "Role Access Right")
    End Sub

    Private Sub UpdateUserRole(ByVal UserId As String, ByVal Roleid As String)
        SQL = "SELECT * FROM T_USERPROFILE  WHERE USERID='" & UserId & "' AND AKTIF='Y' "
        If CheckRecord(SQL) = 0 THEn
            'delete
        Else
            'update
            SQL = "UPDATE T_USERPROFILE SET ROLEID='" & Roleid & "' ,UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE WHERE USERID='" & UserId & "' AND AKTIF='Y'"
            '   ExecuteNonQuery(SQL)
        End If
    End Sub

    Private Sub UpdateMenuGroup(ByVal userid As String, ByVal Menuid As String, ByVal ROLE As String)
        Dim parentid As String = GetParentId(Menuid)

        SQL = "SELECT * FROM T_ROLEACCESSRIGHTS  WHERE ACCESSID='" & parentid & "' AND ROLEID='" & ROLE & "' "
        If CheckRecord(SQL) = 0 THEn
            'insert

            SQL = "INSERT INTO T_ROLEACCESSRIGHTS (ROLEID, ACCESSID, INPUT_BY, INPUT_DATE)" +
            "VALUES('" & ROLE & "','" & parentid & "', '" & userid & "', SYSDATE)"
            ExecuteNonQuery(SQL)
        End If
    End Sub
    Private Sub UpdateUserMenu(ByVal UserId As String, ByVal Roleid As String, ByVal Menuid As String)
        SQL = "INSERT INTO T_ROLEACCESSRIGHTS (ROLEID, ACCESSID, INPUT_BY, INPUT_DATE)" +
        "VALUES('" & Roleid & "','" & Menuid & "', '" & UserId & "', SYSDATE)"
        ExecuteNonQuery(SQL)
    End Sub


End Class