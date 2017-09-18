Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Public Class FrmMaterialType
    Private Sub FrmMaterial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "MATERIAL TYPE"
        GridHeader()
        LoadView()
        ClearInputMT()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
    End Sub
    Private Sub ClearInputMT()
        TextEdit1.Text = ""
        TextEdit2.Text = ""

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"MATERIAL_TYPE_CODE", "MATERIAL_TYPE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'close
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'add
        UnlockAll()
        ClearInputMT()
        TextEdit1.Text = ""
        TextEdit1.SELECT()
        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'Save
        SimpleButton3.Enabled = True 'delete
    End Sub

    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
    End Sub

    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
    End Sub

    Private Sub LoadView()
        SQL = "SELECT MATERIAL_JENIS_CODE AS MATERIAL_TYPE_CODE,MATERIAL_JENIS AS MATERIAL_TYPE FROM T_MATERIAL_JENIS WHERE INACTIVE IS NULL ORDER BY MATERIAL_JENIS_CODE"
        FILLGridView(SQL, GridControl1)
    End Sub
    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'del
        SQL = "UPDATE T_MATERIAL_JENIS SET INACTIVE ='X',INACTIVE_DATE=SYSDATE WHERE MATERIAL_JENIS_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        LoadView()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "MATERIAL TYPE")
        'UPDATE MATERIAL YANG MENGGUNAKAN TYPE INI
        SQL = "UPDATE T_MATERIAL SET INACTIVE='X',INACTIVE_DATE=SYSDATE WHERE MATERIAL_JENIS_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        ClearInputMT()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        ClearInputMT()
        SimpleButton2.Text = "Save" 'save
    End Sub

    Private Sub FrmMaterialType_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 2.5)

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True THEn
            Dim MtJenisCode As String = TextEdit1.Text
            Dim MtJenis As String = TextEdit2.Text
            SQL = "SELECT * FROM T_MATERIAL_JENIS WHERE MATERIAL_JENIS_CODE='" & MtJenisCode & "'"
            If CheckRecord(SQL) = 0 THEn
                'INSERT
                SQL = "INSERT INTO T_MATERIAL_JENIS (MATERIAL_JENIS_CODE,MATERIAL_JENIS) VALUES ('" & MtJenisCode & "','" & MtJenis & "')"
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "MATERIAL TYPE")

                'UPDATE MATERIAL YANG MENGGUNAKAN TYPE INI
                SQL = "UPDATE T_MATERIAL SET INACTIVE =DEFAULT ,INACTIVE_DATE=SYSDATE,STATUS='' WHERE MATERIAL_JENIS_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                ExecuteNonQuery(SQL)
                ClearInputMT()
            Else

                'Update
                SQL = "SELECT * FROM T_MATERIAL_JENIS WHERE MATERIAL_JENIS_CODE='" & MtJenisCode & "' AND INACTIVE ='X'"
                If CheckRecord(SQL) > 0 THEn
                    SQL = "UPDATE T_MATERIAL_JENIS SET MATERIAL_JENIS='" & MtJenis & "',INACTIVE=DEFAULT WHERE MATERIAL_JENIS_CODE='" & MtJenisCode & "' AND INACTIVE='X'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "MATERIAL TYPE")
                    'UPDATE MATERIAL YANG MENGGUNAKAN TYPE INI
                    SQL = "UPDATE T_MATERIAL SET INACTIVE=DEFAULT,INACTIVE_DATE=SYSDATE,STATUS='' WHERE MATERIAL_JENIS_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                    ExecuteNonQuery(SQL)
                    ClearInputMT()
                End If

            End If
            'UPDATE
            If UCase(SimpleButton2.Text) = "UPDATE" THEn
                    SQL = "UPDATE T_MATERIAL_JENIS SET MATERIAL_JENIS='" & MtJenis & "' WHERE MATERIAL_JENIS_CODE='" & MtJenisCode & "'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                MsgBox("UPDATE SUCCESSFUL", vbInformation, "MATERIAL TYPE")
                ClearInputMT()
            End If
            End If

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

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_TYPE_CODE").ToString() 'MATERIAL TYPE CODE
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_TYPE").ToString() 'MATERIAL TYPE

            TextEdit1.Enabled = False

        End If
    End Sub
End Class