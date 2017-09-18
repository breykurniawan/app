Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Public Class FrmAccessRight
    Private Sub FrmAccessRight_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "ACCESS RIGHT MENU"
        GridHeader()
        LoadAccesmenu()
        FillGroup()
        FilltFormName()
        ComboBoxEdit3.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    End Sub

    Private Sub GridHeader()
        Dim View As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim FieldNames() As String = New String() {"GROUP_ID", "NO", "GROUP_NAME", "MENUID", "MENU", "TYPEMENU", "IMGID", "FRMNAME"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        View.Columns.Clear()
        For I = 0 To FieldNames.Length - 1
            Column = View.Columns.AddField(FieldNames(I))
            Column.VisibleIndex = I
        Next
        ''TEST + CHECK BOX
        'Dim edit As RepositoryItemCheckEdit = New RepositoryItemCheckEdit()
        'Dim trueValue As Int64 = 1
        'Dim falseValue As Int64 = 0
        'edit.ValueChecked = trueValue
        'edit.ValueUnchecked = falseValue
        'View.Columns("CHK").ColumnEdit = edit
        'GridControl1.RepositoryItems.Add(edit)
        'View.Columns("CHK").OptionsColumn.AllowEdit = True
        'GridView1.OptionsSELECTion.ShowCheckBoxSELECTorInGroupRow = DevExpress.Utils.DefaultBoolean.True
        'END TEST

        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("GROUP_ID"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("GROUP_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("MENUID"), DevExpress.Data.ColumnSortOrder.Ascending)
        }, 1)
        GridView1.BestFitColumns()
        GridView1.ExpandAllGroups()

    End Sub
    Private Sub FilltFormName()
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim types As Type() = myAssembly.GetTypes()
        ComboBoxEdit3.Properties.Items.Clear()
        For Each myType As Object In types
            If myType.name.ToString <> "" THEn
                Dim frmname As String = myType.name
                If (frmname.Substring(0, 3)) = "Frm" THEn
                    ComboBoxEdit3.Properties.Items.Add(myType.name.ToString)
                End If
            End If
        Next
        ComboBoxEdit3.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
    End Sub
    Private Sub FillGroup()
        SQL = "SELECT ACCESSNAME,ACCESSID FROM  T_ACCESSRIGHTS  WHERE  TYPE=0 ORDER BY ACCESSID"
        FILLComboBoxEdit(SQL, 0, ComboBoxEdit2, False)
    End Sub

    Private Sub FrmAccessRight_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 230
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Close()
    End Sub

    Private Sub ComboBoxEdit1_SELECTedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEdit1.SELECTedIndexChanged
        'GROUP
        Dim Parent As String = GetParentId(ComboBoxEdit2.Text)
        If ComboBoxEdit1.SELECTedIndex = 0 THEn  'GROUP
            TextEdit1.Text = GetCodeGrp()
            ComboBoxEdit2.Enabled = False
            ComboBoxEdit3.Enabled = False
            ComboBoxEdit2.Text = ""
            ComboBoxEdit3.Text = ""
            TextEdit4.Enabled = True
        Else
            TextEdit1.Text = Parent
            ComboBoxEdit2.Enabled = True
            ComboBoxEdit3.Enabled = True
            TextEdit4.Enabled = False
        End If
        TextEdit1.Enabled = False
    End Sub

    Private Sub LoadAccesmenu()
        SQL = " SELECT NVL(B.ACCESSID,A.ACCESSID) AS GROUP_ID,A.URUT AS NO,NVL(B.ACCESSNAME,A.ACCESSNAME) AS GROUP_NAME,A.ACCESSID AS MENUID,A.ACCESSNAME AS MENU, " +
        " CASE A.TYPE " +
        " WHEN '0' THEN 'GROUP' " +
        " WHEN '1' THEN 'MENU' " +
        " End As TYPEMENU  ,A.IMGID,A.FRMNAME " +
        " FROM  T_ACCESSRIGHTS A " +
        " LEFT JOIN T_ACCESSRIGHTS B On A.PARENTID=B.ACCESSID " +
        " ORDER BY A.URUT,A.ACCESSID,B.ACCESSNAME"
        FILLGridView(SQL, GridControl1)
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        'GRID ACCESS RIGHT
        '"MENUID", "MENU", "TYPEMENU", "IMGID", "FRMNAME"
        On Error Resume Next
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'ADD
            SimpleButton3.Enabled = False 'SAVE
            SimpleButton2.Enabled = False 'EDIT
            TextEdit1.Enabled = True
            TextEdit4.Enabled = True

        Else
            SimpleButton1.Enabled = False 'ADD
            SimpleButton2.Enabled = True 'EDIT
            SimpleButton3.Enabled = False 'SAVE

            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "MENU").ToString() 'NAME
            ComboBoxEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "TYPEMENU").ToString() 'NAME
            ComboBoxEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "GROUP_NAME").ToString() 'NAME
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "IMGID").ToString() 'NAME
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "NO").ToString()  'ID
            ComboBoxEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "FRMNAME").ToString() 'NAME
            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "MENUID").ToString()  'ID
            TextEdit1.Enabled = False
            TextEdit4.Enabled = False
            GridView1.Columns("CHK").OptionsColumn.AllowEdit = True

            SimpleButton3.Enabled = True
            SimpleButton3.Text = "Update"

            If ComboBoxEdit1.SELECTedIndex = 0 THEn
                ComboBoxEdit2.Text = ""
                ComboBoxEdit3.Text = ""
                TextEdit4.Enabled = True
            End If
        End If
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL

        TextEdit2.Text = ""
        ComboBoxEdit1.Text = ""
        TextEdit3.Text = ""
        ComboBoxEdit2.Text = ""
        ComboBoxEdit3.Text = ""
        TextEdit1.SELECT()
        TextEdit1.Text = ""

        SimpleButton3.Text = "Save"

        SimpleButton1.Enabled = True 'ADD
        SimpleButton2.Enabled = False 'SAVE
        SimpleButton3.Enabled = False 'DELETE


    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        ''DELETE
        Dim ACCCESSID As String = TextEdit1.Text
        SQL = "DELETE T_ACCESSRIGHTS WHERE ACCESSID='" & ACCCESSID & "'"
        ExecuteNonQuery(SQL)
        LoadAccesmenu()

        FillGroup()
        FilltFormName()


        TextEdit2.Text = ""
        ComboBoxEdit1.Text = ""
        ComboBoxEdit2.Text = ""
        TextEdit3.Text = ""
        ComboBoxEdit3.Text = ""

        SimpleButton1.Enabled = True 'ADD
        SimpleButton2.Enabled = False 'SAVE
        SimpleButton3.Enabled = False 'DELETE


        MsgBox("Delete Successful", vbInformation, "Acces Right Menu")

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        TextEdit1.Text = ""
        TextEdit1.Enabled = False


        TextEdit2.Text = ""
        ComboBoxEdit1.Text = ""
        ComboBoxEdit2.Text = ""
        TextEdit3.Text = ""
        ComboBoxEdit3.Text = ""

        TextEdit2.SELECT()

        SimpleButton1.Enabled = False 'ADD
        SimpleButton2.Enabled = False 'EDIT
        SimpleButton3.Enabled = True 'SAVE

        SimpleButton3.Text = "Save"

        FillGroup()
        FilltFormName()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'SAVE
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True THEn
            If Not IsEmptyCombo({ComboBoxEdit1}) = True THEn
                Dim MENUID As String = TextEdit1.Text
                Dim MENU As String = TextEdit2.Text
                Dim TYPE As String = ComboBoxEdit1.Text
                Dim PARENTID As String = GetMenuId(ComboBoxEdit2.Text)
                Dim IMGID As String = TextEdit3.Text
                Dim URUT As String = TextEdit4.Text
                Dim FRMNAME As String = ComboBoxEdit3.Text

                If TYPE = "MENU" THEn
                    TYPE = "1"
                Else
                    TYPE = "0"
                End If

                SQL = "SELECT * FROM T_ACCESSRIGHTS WHERE ACCESSID ='" & TextEdit1.Text & "' "
                If CheckRecord(SQL) = 0 THEn
                    SQL = "INSERT INTO T_ACCESSRIGHTS (ACCESSID,ACCESSNAME,TYPE,PARENTID,IMGID,FRMNAME,URUT)" +
                    " VALUES ('" & MENUID & "','" & MENU & "','" & TYPE & "','" & PARENTID & "','" & IMGID & "','" & FRMNAME & "','" & URUT & "')	"
                    ExecuteNonQuery(SQL)
                    LoadAccesmenu()
                    MsgBox("Insert  Successful", vbInformation, "Access Right Menu")
                Else
                    SQL = "UPDATE T_ACCESSRIGHTS SET ACCESSNAME='" & MENU & "',TYPE='" & TYPE & "',PARENTID='" & PARENTID & "',IMGID='" & IMGID & "',FRMNAME='" & FRMNAME & "' ,URUT='" & URUT & "' " +
                      "WHERE ACCESSID='" & MENUID & "'"
                    ExecuteNonQuery(SQL)
                    LoadAccesmenu()
                    FillGroup()
                    MsgBox("Update Successful", vbInformation, "Access Right Menu")
                End If
                TextEdit1.Text = ""
                TextEdit2.Text = ""
                TextEdit3.Text = ""

                ComboBoxEdit1.Text = ""
                ComboBoxEdit2.Text = ""
                ComboBoxEdit3.Text = ""

                TextEdit1.SELECT()

                SimpleButton1.Enabled = True 'ADD
                SimpleButton2.Enabled = False 'EDIT
                SimpleButton3.Enabled = False 'SAVE


            End If
        End If
    End Sub

    Private Sub ComboBoxEdit2_SELECTedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEdit2.SELECTedIndexChanged
        'MENUID
        Dim Menuid As String = GetMenuId(ComboBoxEdit2.Text)
        If ComboBoxEdit1.SELECTedIndex = 1 THEn TextEdit1.Text = GetCodeSub(Menuid)
    End Sub

    Private Sub gridViewT_CellValueChanging(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs)
        If e.Column.Name = "CheckColumn" AndAlso e.RowHandle > -1 THEn
            If Not (e.Value Is Nothing) AndAlso DirectCast(e.Value, Boolean) THEn
                ' Dim dr As DataRow = DirectCast(GridControl1.DataSource, DataTable).Rows(e.RowHandle)
                Dim dr As DataRow = DirectCast(sender, GridView).GetDataRow(e.RowHandle)

            End If
        End If
    End Sub
End Class
