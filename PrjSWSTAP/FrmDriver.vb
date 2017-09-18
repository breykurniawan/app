Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmDriver
    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        TextEdit3.Enabled = True
        TextEdit4.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        TextEdit3.Enabled = False
        TextEdit4.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub ClearInput()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""

    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl8.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"DRIVER_CODE", "DRIVER_NAME", "TRANSPORTER_NAME", "LICENSE_NUMBER"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING & ORDER BY
        Dim GridView As GridView = CType(GridControl8.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("TRANSPORTER_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("DRIVER_CODE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 0)
        GridView.OptionsView.ColumnAutoWidth = False
        GridView.OptionsView.BestFitMaxRowCount = -1
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub LoadDriver()
        SQL = " SELECT A.DRIVER_CODE,B.TRANSPORTER_NAME,A.DRIVER_NAME,A.LICENSE_NUMBER,A.STATUS  " +
        " FROM T_DRIVER A " +
        " LEFT JOIN T_TRANSPORTER B ON A.TRANSPORTER_CODE=B.TRANSPORTER_CODE " +
        " WHERE A.INACTIVE IS NULL  " +
        " And B.INACTIVE IS NULL " +
        " Order By TRANSPORTER_NAME, DRIVER_CODE"
        FILLGridView(SQL, GridControl8)
        GridControl8.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl8.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub

    Private Sub FrmDriver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "DRIVER"
        GridHeader()
        LoadDriver()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl1.Enabled = False  'PANEL CRUDE
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE DRIVER
        If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit3, TextEdit4}) THEn
            Dim DRIVERCODE As String = Trim(TextEdit1.Text)
            Dim DRIVERNAME As String = TextEdit2.Text
            Dim TRANSPORTERCODE As String = GetCodeTrans(TextEdit4.Text)
            Dim LINCENSENUMBER As String = TextEdit3.Text
            Dim STATUS As String = ""
            SQL = "SELECT * FROM T_DRIVER WHERE DRIVER_CODE='" & TextEdit1.Text & "'"
            If CheckRecord(SQL) = 0 THEn
                SQL = "INSERT INTO T_DRIVER (DRIVER_CODE,DRIVER_NAME,TRANSPORTER_CODE,LICENSE_NUMBER,INACTIVE,INACTIVE_DATE,STATUS)" +
                    " VALUES('" & DRIVERCODE & "','" & DRIVERNAME & "','" & TRANSPORTERCODE & "','" & LINCENSENUMBER & "',DEFAULT,SYSDATE,'" & STATUS & "')"
                ExecuteNonQuery(SQL)
                LoadDriver()
                UnlockAll()
                ClearInput()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "DRIVER")
            Else
                SQL = "SELECT * FROM T_DRIVER WHERE DRIVER_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL "
                If CheckRecord(SQL) > 0 THEn
                    SQL = " UPDATE T_DRIVER SET DRIVER_NAME='" & DRIVERNAME & "',TRANSPORTER_CODE='" & TRANSPORTERCODE & "',LICENSE_NUMBER='" & LINCENSENUMBER & "',STATUS='" & STATUS & "' " +
                              " INACTIVE=DEFAULT,INACTIVE_DATE=SYSDATE " +
                              " WHERE DRIVER_CODE= '" & TextEdit1.Text & "' AND INACTIVE ='X'"
                    ExecuteNonQuery(SQL)
                    LoadDriver()
                    UnlockAll()
                    ClearInput()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "DRIVER")
                End If
            End If
            If UCase(SimpleButton2.Text) = "UPDATE" THEn
                SQL = " UPDATE T_DRIVER SET DRIVER_NAME='" & DRIVERNAME & "',TRANSPORTER_CODE='" & TRANSPORTERCODE & "',LICENSE_NUMBER='" & LINCENSENUMBER & "',STATUS='" & STATUS & "' " +
                          " WHERE DRIVER_CODE= '" & TextEdit1.Text & "' AND INACTIVE IS NULL "
                ExecuteNonQuery(SQL)
                LoadDriver()
                UnlockAll()
                ClearInput()
                MsgBox("UPDATE SUCCESSFUL", vbInformation, "DRIVER")
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UnlockAll()
        TextEdit1.Text = ""
        TextEdit1.SELECT()
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'delete
        SQL = "UPDATE T_DRIVER SET INACTIVE = 'X',INACTIVE_DATE=SYSDATE WHERE DRIVER_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        LoadDriver()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "DRIVER")
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        SimpleButton2.Text = "Save"
        LockAll()
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub

    Private Sub GridView8_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView8.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'del
        Else
            TextEdit1.Text = GridView8.GetRowCellValue(e.RowHandle, "DRIVER_CODE").ToString()  'ID
            TextEdit2.Text = GridView8.GetRowCellValue(e.RowHandle, "DRIVER_NAME").ToString() 'NAME
            TextEdit4.Text = GridView8.GetRowCellValue(e.RowHandle, "TRANSPORTER_NAME").ToString() 'NAME
            TextEdit3.Text = GridView8.GetRowCellValue(e.RowHandle, "LICENSE_NUMBER").ToString() 'SIM
            UnlockAll()
            TextEdit1.Enabled = False

            SimpleButton1.Enabled = False 'ADD
            SimpleButton2.Enabled = True 'SAVE
            SimpleButton3.Enabled = True 'DEL

            SimpleButton2.Text = "Update" 'SAVE
        End If
    End Sub

    Private Sub FrmDriver_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl2.Height = (Me.Height - 60) - (30 * 5)
    End Sub

    Private Sub TextEdit3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit3.KeyPress
        'sim 
        e.Handled = NumericOnly(e)
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        LSQL = "SELECT TRANSPORTER_CODE,TRANSPORTER_NAME FROM T_TRANSPORTER WHERE INACTIVE Is NULL "
        LField = "TRANSPORTER_NAME"
        ValueLoV = ""
        TextEdit4.Text = FrmShowLOV(FrmLoV, LSQL, "TRANSPORTER_CODE", "TRANSPORTER")
    End Sub
End Class