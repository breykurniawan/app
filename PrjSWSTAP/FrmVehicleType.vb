Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository
Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmVehicleType
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
        Dim view As ColumnView = CType(GridControl5.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"VEHICLE_TYPE", "TOLERANCE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next
        'GROUPING
        GridView5.BestFitColumns()
        GridView5.ExpandAllGroups()
    End Sub
    Private Sub LoadView()
        SQL = "SELECT VEHICLE_TYPE,TOLERANCE FROM T_VEHICLE_TYPE WHERE INACTIVE <> 'X'"
        FILLGridView(SQL, GridControl5)
        GridControl5.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl5.FocusedView, GridView)
        GridView.ExpandAllGroups()
        GridView.BestFitColumns()
    End Sub
    Private Sub FrmVehicleType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "VEHICLE TYPE"
        GridHeader()
        LoadView()
        UnlockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite() = True Or SAP = "" Then
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        SimpleButton3.Enabled = False

    End Sub


    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE VEHICLE TYPE
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True Then
            SQL = " SELECT * FROM T_VEHICLE_TYPE WHERE VEHICLE_TYPE='" & TextEdit1.Text & "'"
            Dim VEHICLETYPE As String = TextEdit1.Text
            Dim TOLERANCE As String = TextEdit2.Text
            If CheckRecord(SQL) = 0 Then
                SQL = "INSERT INTO T_VEHICLE_TYPE (VEHICLE_TYPE,TOLERANCE,INACTIVE)" +
                " VALUES('" & VEHICLETYPE & "','" & TOLERANCE & "','N')"
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "VEHICLE TYPE")
                UnlockAll()
                ClearInputVT()
            Else
                SQL = " SELECT * FROM T_VEHICLE_TYPE WHERE VEHICLE_TYPE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                If CheckRecord(SQL) > 0 Then
                    SQL = " UPDATE T_VEHICLE_TYPE SET TOLERANCE='" & TOLERANCE & "',INACTIVE='N'" +
                          " WHERE VEHICLE_TYPE= '" & TextEdit1.Text & "' AND INACTIVE='X' "
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "VEHICLE TYPE")
                End If
            End If

            If UCase(SimpleButton2.Text) = "UPDATE" Then
                SQL = "UPDATE T_VEHICLE_TYPE SET TOLERANCE='" & TOLERANCE & "'" +
                      " WHERE VEHICLE_CODE= '" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("UPDATE SUCCESSFUL", vbInformation, "VEHICLE TYPE")
            End If
        End If

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        ' DELETE VEHICLE TYPE
        SQL = "UPDATE T_VEHICLE_TYPE SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE VEHICLE_TYPE='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadView()
        ClearInputVT()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "VEHICLE TYPE")

    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'cancel
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        SimpleButton2.Text = "Save" 'SAVE

        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        SimpleButton3.Enabled = False

    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub
    Private Sub GridView5_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView5.RowCellClick
        If e.RowHandle < 0 Then
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete

        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView5.GetRowCellValue(e.RowHandle, "VEHICLE_TYPE").ToString() 'VEHICLE_TYPE
            TextEdit2.Text = GridView5.GetRowCellValue(e.RowHandle, "TOLERANCE").ToString() 'TOLERANCE


            TextEdit1.Enabled = False

        End If
    End Sub

    Private Sub FrmVehicleType_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 2.5)
    End Sub

    Private Sub TextEdit2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit2.KeyPress
        e.Handled = NumericOnly(e)
    End Sub
End Class