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
Public Class FrmRptDetailSWS
    Private Sub FrmRptDetailSWS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadView()
    End Sub

    Private Sub LabelControl4_Click(sender As Object, e As EventArgs) Handles LabelControl4.Click

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Me.Close()
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
            SQL = "SELECT TO_CHAR(BA_DATE,'MM-yyyy')BLN,BA_DATE,NO_BERITA_ACARA,POLICE_NO, " +
            " TRANSPORTER,DRIVER_NAME,LICENSE_NO,TARRA_TIMBANGAN,TARRA_STANDARD,SELISIH_TARRA,ALASAN " +
            " FROM T_BERITA_ACARA " +
            " WHERE BA_DATE BETWEEN " & tgl1 & " AND " & tgl2 & "" +
            " ORDER BY BA_DATE DESC"
        Else
            SQL = "SELECT TO_CHAR(BA_DATE,'MM-yyyy')BLN,BA_DATE,NO_BERITA_ACARA,POLICE_NO, " +
            " TRANSPORTER,DRIVER_NAME,LICENSE_NO,TARRA_TIMBANGAN,TARRA_STANDARD,SELISIH_TARRA,ALASAN " +
            " FROM T_BERITA_ACARA " +
            " ORDER BY BA_DATE DESC"
        End If
        FILLGridView(SQL, GridControl1)
    End Sub


End Class