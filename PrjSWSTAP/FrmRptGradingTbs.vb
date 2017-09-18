Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Export
Imports DevExpress.XtraPrinting

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Imports Microsoft.VisualBasic
Imports System
Public Class FrmRptGradingTbs
    Private Sub GridControl1_Click(sender As Object, e As EventArgs) Handles GridControl1.Click

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Me.Close()

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        LoadView()
    End Sub

    Private Sub LoadView()
        Dim tgl1 As String
        Dim tgl2 As String

        DateEdit1.Properties.Mask.Culture = New System.Globalization.CultureInfo("en-US")
        DateEdit1.Properties.Mask.EditMask = "dd-MM-yyyy"
        DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = True
        DateEdit1.Properties.CharacterCasing = CharacterCasing.Upper
        tgl1 = "TO_DATE('" & DateEdit1.Text & "','dd-MM-yyyy')"

        DateEdit2.Properties.Mask.Culture = New System.Globalization.CultureInfo("en-US")
        DateEdit2.Properties.Mask.EditMask = "dd-MM-yyyy"
        DateEdit2.Properties.Mask.UseMaskAsDisplayFormat = True
        DateEdit2.Properties.CharacterCasing = CharacterCasing.Upper
        tgl2 = "TO_DATE('" & DateEdit2.Text & "','dd-MM-yyyy')"

        If DateEdit1.Text <> "" = True Or DateEdit2.Text <> "" = True THEn
            SQL = "SELECT * FROM V_RPT_GRADING WHERE INPUT_DATE BETWEEN " & tgl1 & " AND " & tgl2 & "" +
            " ORDER BY INPUT_DATE DESC"
        Else
            SQL = "SELECT * FROM V_RPT_GRADING ORDER BY INPUT_DATE"
        End If
        FILLGridView(SQL, GridControl1)
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'exs to xls
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK THEn
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) THEn
                Dim nama As String = saveFileDialog1.FileName
                myStream.Close()
                GridView1.ExportToXls(nama)
                MsgBox("Export successfull!", MessageBoxIcon.Information, "Report Grading")
            End If
        End If
    End Sub

    Private Sub FrmRptGradingTbs_Load(sender As Object, e As EventArgs) Handles Me.Load
        DateEdit1.Text = ""
        DateEdit2.Text = ""
    End Sub
End Class