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


Public Class FrmRptBeritaAcara

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'view report
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
        ShowReport("RptBA", SQL, "T_BA")
    End Sub

    Private Sub FrmRptBeritaAcara_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "REPORT BERITA ACARA TARRA"
        DateEdit1.Properties.Mask.EditMask = "dd-MM-yyyy"
        DateEdit2.Properties.Mask.EditMask = "dd-MM-yyyy"

        CreateHeader()
        loadview()
    End Sub
    Private Sub CreateHeader()
        '        BA_DATE, NO_BERITA_ACARA, POLICE_NO,
        'TRANSPORTER, DRIVER_NAME, LICENSE_NO, TARRA_TIMBANGAN, TARRA_STANDARD, SELISIH_TARRA, ALASAN
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"BLN", "TRANSPORTER", "NO_BERITA_ACARA", "BA_DATE", "POLICE_NO", "DRIVER_NAME", "LICENSE_NO", "TARRA_TIMBANGAN", "TARRA_STANDARD", "SELISIH_TARRA", "ALASAN"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next
        'GROUPING & SORTING
        'Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        'GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        'New GridColumnSortInfo(GridView.Columns("BLN"), DevExpress.Data.ColumnSortOrder.Descending),
        'New GridColumnSortInfo(GridView.Columns("TRANSPORTER"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)

        GridView1.BestFitColumns()
        GridView1.ExpandAllGroups()
    End Sub
    Private Sub loadview()
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

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
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
                MsgBox("Export successfull!", MessageBoxIcon.Information, "Berita Acara")
            End If
        End If
    End Sub
    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs) Handles SimpleButton7.Click
        'exs to pdf
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK THEn
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) THEn
                Dim nama As String = saveFileDialog1.FileName
                myStream.Close()
                GridView1.ExportToPdf(nama)
                MsgBox("Export successfull!", MessageBoxIcon.Information, "Berita Acara")
            End If
        End If
    End Sub

    Private Sub SimpleButton8_Click(sender As Object, e As EventArgs) Handles SimpleButton8.Click
        'exs to CSV
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK THEn
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) THEn
                Dim nama As String = saveFileDialog1.FileName
                myStream.Close()
                GridView1.ExportToCsv(nama)
                MsgBox("Export successfull!", MessageBoxIcon.Information, "Berita Acara")
            End If
        End If
    End Sub

    Private Sub DateEdit2_EditValueChanged(sender As Object, e As EventArgs) Handles DateEdit2.EditValueChanged
        If DateEdit2.EditValue > DateEdit1.EditValue THEn loadview()
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        loadview()
    End Sub

    Private Sub FrmRptBeritaAcara_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 180
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'close
        Me.Close()
    End Sub

End Class