Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmBeritaAcara
    Private Sub FrmBeritaAcara_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateEdit1.EditValue = Now
        Me.Text = "BERITA ACARA TARRA"
        TextEdit1.Text = "" : TextEdit1.Enabled = True : TextEdit1.Select()     'no ba 
        DateEdit1.Text = BA_DATE : DateEdit1.Properties.ReadOnly = True    'ba date 
        TextEdit2.Text = VEHICLE_NUMBER  'vehicle number
        TextEdit3.Text = GetTranspotVehicle(VEHICLE_NUMBER)   'transporter
        TextEdit4.Text = "" 'driver name
        TextEdit5.Text = "" 'lic name/sim
        TextEdit6.Text = WB_ACTUAL   'TARA ACTUAL
        TextEdit7.Text = GetTara(VEHICLE_NUMBER)  'TARA -STD
        TextEdit9.Text = Val(TextEdit6.Text) - Val(TextEdit7.Text)    'SELIESIH ACTUAL-STD
        MemoEdit1.Text = ""
        TextEdit10.Text = USERNAME
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Close()
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE BA
        If Not IsEmptyText({TextEdit1, TextEdit4, TextEdit5}) Then
            If MemoEdit1.Text = "" Then
                MessageBox.Show("SORRY THE TEXT SHOULD BE FILLED", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                MemoEdit1.Select()
                Exit Sub
            End If

            Dim BA_NO As String = Trim(TextEdit1.Text)
            Dim BA_DATE As String = DateEdit1.Text
            Dim BA_VEHICLENO As String = TextEdit2.Text
            Dim BA_TRANPORTER As String = TextEdit3.Text
            Dim BA_DRIVERNAME As String = TextEdit4.Text
            Dim BA_LINCENSENUMBER As String = TextEdit5.Text
            Dim BA_WTARA As Integer = TextEdit6.Text
            Dim BA_WSTD As Integer = TextEdit7.Text
            Dim BA_WSELISIH As Integer = TextEdit9.Text
            Dim BA_USER As String = TextEdit10.Text
            Dim BA_MEMO As String = MemoEdit1.Text

            SQL = "SELECT * FROM T_BERITA_ACARA WHERE NO_BERITA_ACARA='" & BA_NO & "'"
            If CheckRecord(SQL) = 0 Then
                SQL = "INSERT INTO T_BERITA_ACARA " +
                " (NO_BERITA_ACARA,BA_DATE, " +
                " POLICE_NO,TRANSPORTER, " +
                " DRIVER_NAME,LICENSE_NO, " +
                " TARRA_TIMBANGAN,TARRA_STANDARD, " +
                " SELISIH_TARRA,ALASAN) " +
                " VALUES " +
                " ('" & BA_NO & "',SYSDATE," +
                " '" & BA_VEHICLENO & "','" & BA_TRANPORTER & "'," +
                " '" & BA_DRIVERNAME & "','" & BA_LINCENSENUMBER & "'," +
                " '" & BA_WTARA & "','" & BA_WSTD & "'," +
                " '" & BA_WSELISIH & "','" & BA_MEMO & "') "
                ExecuteNonQuery(SQL)
                'UPDATE TARA BARU 
                SQL = "SELECT TARE,PLATE_NUMBER FROM T_VEHICLE WHERE PLATE_NUMBER='" & BA_VEHICLENO & "' AND INACTIVE='N'"
                If CheckRecord(SQL) > 0 Then
                    'uddate TARA LAMA DENGAN TARA BARU
                    SQL = "UPDATE T_VEHICLE SET TARE='" & BA_WTARA & "'" +
                        " WHERE PLATE_NUMBER= '" & BA_VEHICLENO & "'"
                    ExecuteNonQuery(SQL)
                End If
                MsgBox("SAVE SUCCESSFUL", vbInformation, "BERITA ACARA")
                Me.Close()
            Else
                MsgBox("NOMOR BERITA ACARA SUDAH ADA", vbInformation, "BERITA ACARA")
                TextEdit1.Select()
                Exit Sub
            End If

        End If
    End Sub

    Private Sub TextEdit3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit3.KeyPress
        'sim 
        e.Handled = NumericOnly(e)
    End Sub

    Private Sub TextEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles TextEdit1.EditValueChanged
        If Len(TextEdit1.Text) > 0 Then
            SimpleButton2.Enabled = True
        Else
            SimpleButton2.Enabled = False
        End If
    End Sub
End Class