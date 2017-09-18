Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Public Class FrmWBType
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD VEHICLE TYPE
        UnlockAll()
        TextEdit1.Text = ""
        TextEdit1.Select()
    End Sub

    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete

    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub
    Private Sub ClearInputVT()
        TextEdit1.Text = ""
        TextEdit2.Text = ""

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub
    Private Sub GridHeader()
        'WB TYPE
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"WEIGHBRIDGE_TYPE_CODE", "WEIGHBRIDGE_TYPE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next
        GridView1.BestFitColumns()
        GridView1.ExpandAllGroups()
    End Sub
    Private Sub LoadView()
        SQL = "  SELECT WB_TYPE_CODE AS WEIGHBRIDGE_TYPE_CODE ,WB_TYPE AS WEIGHBRIDGE_TYPE " +
        " FROM T_WB_TYPE " +
        " WHERE INACTIVE <>'X' " +
        " ORDER BY WB_TYPE_CODE "
        FILLGridView(SQL, GridControl1)
        GridControl1.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub
    Private Sub FrmVehicleType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "WEIGHBRIDGE TYPE"
        GridHeader()
        LoadView()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite() = True Or SAP = "" Then
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE VEHICLE TYPE
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True Then
            SQL = " SELECT * FROM T_WB_TYPE WHERE WB_TYPE_CODE='" & TextEdit1.Text & "'"
            Dim WBTYPECODE As String = TextEdit1.Text
            Dim WBTYPE As String = TextEdit2.Text

            If CheckRecord(SQL) = 0 Then
                SQL = " INSERT INTO T_WB_TYPE (WB_TYPE_CODE,WB_TYPE,INACTIVE) " +
                      " VALUES('" & WBTYPECODE & "','" & WBTYPE & "','N') "
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "WEIGHBRIDGE TYPE")
                UnlockAll()
                ClearInputVT()
            Else
                SQL = "SELECT * FROM T_WB_TYPE WHERE WB_TYPE_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                If CheckRecord(SQL) > 0 Then
                    SQL = "UPDATE T_WB_TYPE SET WB_TYPE='" & WBTYPE & "',INACTIVE='N' WHERE WB_TYPE_CODE= '" & TextEdit1.Text & "' AND INACTIVE='X'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "WEIGHBRIDGE TYPE")
                    SimpleButton2.Text = "Save" 'save
                    ClearInputVT()
                End If
            End If
            If UCase(SimpleButton2.Text) = "UPDATE" Then
                SQL = "UPDATE T_WB_TYPE SET WB_TYPE='" & WBTYPE & "' WHERE WB_TYPE_CODE= '" & TextEdit1.Text & "' AND INACTIVE='N'"
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("UPDATE SUCCESSFUL", vbInformation, "WEIGHBRIDGE TYPE")
                SimpleButton2.Text = "Save" 'save
                ClearInputVT()
            End If
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        ' DELETE VEHICLE TYPE
        SQL = "UPDATE T_WB_TYPE SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE WB_TYPE_CODE='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadView()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "WEIGHBRIDGE TYPE")
        ClearInputVT()
    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'cancel
        ClearInputVT()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub
    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 Then
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "WEIGHBRIDGE_TYPE_CODE").ToString() 'VEHICLE_TYPE
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "WEIGHBRIDGE_TYPE").ToString() 'TOLERANCE

            UnlockAll()
            TextEdit1.Enabled = False

        End If
    End Sub

    Private Sub FrmWBType_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 3.2)
    End Sub

    Private Sub TextEdit1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit1.KeyPress
        'wb type
        e.Handled = NumericOnly(e)
    End Sub
End Class