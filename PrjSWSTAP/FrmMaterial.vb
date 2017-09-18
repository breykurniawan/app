Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmMaterial
    Private Sub FrmMaterial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "MATERIAL"
        GridHeader()
        LoadView()
        LoadType()
        ClearInputMT()
        Lockall()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
    End Sub
    Private Sub Lockall()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        ComboBoxEdit1.Enabled = False

    End Sub
    Private Sub UNLockall()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        ComboBoxEdit1.Enabled = True

    End Sub

    Private Sub ClearInputMT()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        ComboBoxEdit1.Text = ""

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"MATERIAL_CODE", "MATERIAL_NAME", "MATERIAL_TYPE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("MATERIAL_TYPE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub LoadView()
        SQL = "SELECT A.MATERIAL_CODE,A.MATERIAL_NAME,B.MATERIAL_JENIS MATERIAL_TYPE" +
            " FROM T_MATERIAL A " +
            " LEFT JOIN T_MATERIAL_JENIS B ON B.MATERIAL_JENIS_CODE=A.MATERIAL_JENIS_CODE " +
            " WHERE A.INACTIVE IS NULL AND B.INACTIVE IS NULL " +
            " Order By b.MATERIAL_JENIS"
        GridControl1.DataSource = Nothing
        FILLGridView(SQL, GridControl1)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'save
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True THEn
            If Not IsEmptyCombo({ComboBoxEdit1}) = True THEn
                Dim MATERIALCODE As String = TextEdit1.Text
                Dim MATERIALNAME As String = TextEdit2.Text
                Dim MATERIALTYPE As String = GetMtJenisCode(ComboBoxEdit1.Text)
                'TRY
                SQL = "SELECT *FROM T_MATERIAL WHERE INACTIVE IS NULL AND MATERIAL_CODE='" & MATERIALCODE & "' "
                If CheckRecord(SQL) = 0 THEn
                    SQL = " INSERT INTO T_MATERIAL (MATERIAL_CODE,MATERIAL_NAME,MATERIAL_JENIS_CODE)" +
                          " VALUES ('" & MATERIALCODE & "','" & MATERIALNAME & "','" & MATERIALTYPE & "')"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "MATERIAL_CODE")
                    ClearInputMT()
                Else
                    SQL = "SELECT MATERIAL_JENIS_CODE,MATERIAL_JENIS FROM T_MATERIAL_JENIS WHERE INACTIVE ='X' "
                    If CheckRecord(SQL) > 0 THEn
                        SQL = " UPDATE T_MATERIAL SET MATERIAL_NAME='" & MATERIALNAME & "',MATERIAL_JENIS_CODE='" & MATERIALTYPE & "'  ,INACTIVE=DEFAULT,STATUS="",INACTIVE_DATE="" " +
                              " WHERE MATERIAL_CODE='" & MATERIALCODE & "' AND INACTIVE ='X' "
                        ExecuteNonQuery(SQL)
                        LoadView()
                        MsgBox("UPDATE SUCCESSFUL", vbInformation, "MATERIAL_CODE")
                    End If

                    If UCase(SimpleButton2.Text) = "UPDATE" THEn
                        SQL = " UPDATE T_MATERIAL SET MATERIAL_NAME='" & MATERIALNAME & "',MATERIAL_JENIS_CODE='" & MATERIALTYPE & "' " +
                          " WHERE MATERIAL_CODE='" & MATERIALCODE & "' AND INACTIVE IS NULL "
                        ExecuteNonQuery(SQL)
                        LoadView()
                        MsgBox("UPDATE SUCCESSFUL", vbInformation, "MATERIAL_CODE")

                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UNLockall()
        ClearInputMT()
        LoadType()
        'TextEdit1.Text = GetCode("MT")
        TextEdit1.Text = "" 'FREE CODE 
        TextEdit1.SELECT()
        SimpleButton1.Enabled = False
        SimpleButton2.Enabled = True
        SimpleButton3.Enabled = False
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE
        SQL = "UPDATE T_MATERIAL SET INACTIVE = 'X',INACTIVE_DATE=SYSDATE WHERE MATERIAL_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        LoadView()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "MATERIAL")
        ClearInputMT()
    End Sub
    Private Sub LoadType()
        SQL = "SELECT MATERIAL_JENIS_CODE,MATERIAL_JENIS FROM T_MATERIAL_JENIS WHERE INACTIVE IS NULL "
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit1, False)
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        ClearInputMT()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub
    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_CODE").ToString() 'MATERIALCODE
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_NAME").ToString() 'MATERIALNAME
            ComboBoxEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_TYPE").ToString() 'MATERIALTYPE

            UNLockall()
            TextEdit1.Enabled = False

        End If
    End Sub

    Private Sub FrmMaterial_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 3.2)
    End Sub

End Class