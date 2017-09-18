Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmTransporter
    Private Sub UnlockAll()
        TextEdit17.Enabled = True
        TextEdit72.Enabled = True
        TextEdit70.Enabled = True
        TextEdit67.Enabled = True
        TextEdit71.Enabled = True
        TextEdit69.Enabled = True
        MemoEdit1.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete
    End Sub
    Private Sub LockAll()
        TextEdit17.Enabled = False
        TextEdit72.Enabled = False
        TextEdit70.Enabled = False
        TextEdit67.Enabled = False
        TextEdit71.Enabled = False
        TextEdit69.Enabled = False
        MemoEdit1.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub ClearInputTP()
        TextEdit17.Text = ""
        TextEdit72.Text = ""
        TextEdit70.Text = ""
        TextEdit67.Text = ""
        TextEdit71.Text = ""
        TextEdit69.Text = ""
        MemoEdit1.Text = ""

        LockAll()
        SimpleButton2.Text = "Save" 'SAVE

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl3.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"TRANSPORTER_CODE", "TRANSPORTER_NAME", "CONTACT_PERSON", "NPWP", "ADDRESS", "PHONE", "FAX"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn
        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING
        Dim GridView As GridView = CType(GridControl3.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("TRANSPORTER_CODE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 0)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub LoadView()
        SQL = ("SELECT TRANSPORTER_CODE ,TRANSPORTER_NAME,NPWP,ADDRESS,PHONE,FAX,CONTACT_PERSON,STATUS FROM T_TRANSPORTER WHERE INACTIVE IS NULL ORDER BY TRANSPORTER_CODE")
        GridControl3.DataSource = Nothing
        FILLGridView(SQL, GridControl3)
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD TRANSPORTER
        UnlockAll()
        TextEdit17.Text = ""
        TextEdit17.SELECT()
    End Sub
    Private Sub LoadTranspot()
        SQL = "SELECT TRANSPORTER_CODE,TRANSPORTER_NAME,NPWP,ADDRESS,PHONE,FAX,CONTACT_PERSON,STATUS  " +
        " FROM T_TRANSPORTER A  " +
        " WHERE a.INACTIVE IS NULL " +
        " Order By TRANSPORTER_CODE "
        FILLGridView(SQL, GridControl3)

        GridControl3.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl3.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub



    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE TRANSPORTER
        If Not IsEmptyText({TextEdit17, TextEdit72, TextEdit70, TextEdit67, TextEdit71, TextEdit69}) = True THEn

            Dim TRANSPORTERCODE As String = TextEdit17.Text
            Dim TRANSPORTERNAME As String = TextEdit72.Text
            Dim NPWP As String = TextEdit69.Text
            Dim ADDRESS As String = MemoEdit1.Text
            Dim PHONE As String = TextEdit67.Text
            Dim FAX As String = TextEdit71.Text
            Dim CONTACTPERSON As String = TextEdit70.Text
            Dim INACTIVE As String = "DEFAULT"
            Dim STATUS As String = ""

            SQL = " SELECT * FROM T_TRANSPORTER WHERE TRANSPORTER_CODE= '" & TextEdit17.Text & "' "
            If CheckRecord(SQL) = 0 THEn
                SQL = "INSERT INTO T_TRANSPORTER (TRANSPORTER_CODE,TRANSPORTER_NAME,NPWP,ADDRESS,PHONE,FAX,CONTACT_PERSON,STATUS)" +
                        "VALUES ('" & TRANSPORTERCODE & "','" & TRANSPORTERNAME & "','" & NPWP & "','" & ADDRESS & "','" & PHONE & "','" & FAX & "','" & CONTACTPERSON & "','" & STATUS & "')"
                ExecuteNonQuery(SQL)
                LoadTranspot()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "TRANSPORTER")
                ClearInputTP()
                UnlockAll()
            Else
                SQL = " SELECT * FROM T_TRANSPORTER WHERE TRANSPORTER_CODE= '" & TextEdit17.Text & "' AND INACTIVE='X' "
                If CheckRecord(SQL) > 0 THEn
                    SQL = " UPDATE T_TRANSPORTER SET TRANSPORTER_CODE='" & TRANSPORTERCODE & "',TRANSPORTER_NAME='" & TRANSPORTERNAME & "',NPWP='" & NPWP & "',ADDRESS='" & ADDRESS & "',PHONE='" & PHONE & "',FAX='" & FAX & "',CONTACT_PERSON='" & CONTACTPERSON & "',INACTIVE=DEFAULT,INACTIVE_DATE=SYSDATE,STATUS='" & STATUS & "'" +
                          " WHERE TRANSPORTER_CODE= '" & TextEdit17.Text & "' AND INACTIVE='X'"
                    ExecuteNonQuery(SQL)
                    LoadTranspot()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "TRANSPORTER")
                    ClearInputTP()
                End If
                If UCase(SimpleButton2.Text) = "UPDATE" THEn
                    SQL = " UPDATE T_TRANSPORTER SET TRANSPORTER_CODE='" & TRANSPORTERCODE & "',TRANSPORTER_NAME='" & TRANSPORTERNAME & "',NPWP='" & NPWP & "',ADDRESS='" & ADDRESS & "',PHONE='" & PHONE & "',FAX='" & FAX & "',CONTACT_PERSON='" & CONTACTPERSON & "',INACTIVE=DEFAULT,INACTIVE_DATE=SYSDATE,STATUS=DEFAULT " +
                      " WHERE TRANSPORTER_CODE= '" & TextEdit17.Text & "'"
                    ExecuteNonQuery(SQL)
                    LoadTranspot()
                    MsgBox("UPDATE SUCCESSFUL", vbInformation, "TRANSPORTER")
                End If
            End If
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE TRANSPORTER
        SQL = "UPDATE T_TRANSPORTER SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE  WHERE TRANSPORTER_CODE='" & TextEdit17.Text & "' INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        LoadTranspot()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "TRANSPORTER")
        ClearInputTP()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL TRANSPORTER
        TextEdit17.Text = ""
        TextEdit72.Text = ""
        TextEdit70.Text = ""
        TextEdit67.Text = ""
        TextEdit71.Text = ""
        TextEdit69.Text = ""
        MemoEdit1.Text = ""

        LockAll()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub FrmTransporter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "TRANSPORTER"
        GridHeader()
        LoadTranspot()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
    End Sub
    Private Sub GridView3_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView3.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            TextEdit17.Text = GridView3.GetRowCellValue(e.RowHandle, "TRANSPORTER_CODE").ToString() 'TRANSPORTERCODE
            TextEdit72.Text = GridView3.GetRowCellValue(e.RowHandle, "TRANSPORTER_NAME").ToString() 'TRANSPORTERNAME
            TextEdit69.Text = GridView3.GetRowCellValue(e.RowHandle, "NPWP").ToString() 'NPWP
            MemoEdit1.Text = GridView3.GetRowCellValue(e.RowHandle, "ADDRESS").ToString() 'ADDRESS
            TextEdit67.Text = GridView3.GetRowCellValue(e.RowHandle, "PHONE").ToString()  'PHONE
            TextEdit71.Text = GridView3.GetRowCellValue(e.RowHandle, "FAX").ToString()  'FAX
            TextEdit70.Text = GridView3.GetRowCellValue(e.RowHandle, "CONTACT_PERSON").ToString()  'CONTACTPERSON


            UnlockAll()
            TextEdit17.Enabled = False

            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update"
        End If
    End Sub

    Private Sub SimpleButton5_Click_1(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Me.Close()
    End Sub
    Private Sub FrmTransporter_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 4)
    End Sub
    Private Sub TextEdit67_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit67.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit71_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit71.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit69_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit69.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
End Class