
Imports System.IO
Imports System.Net

Imports System.Net.Sockets
Imports System.Threading.Tasks

Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Imports System.ComponentModel
Imports System.Drawing.Imaging

Imports AForge.Video.DirectShow
Imports AForge.Video


Public Class FrmWbOut
    Private Delegate Sub AppendTextBoxDelegate(ByVal TB As String, ByVal txt As String)
    Dim Stream1 As JPEGStream
    Dim Stream2 As JPEGStream
    Dim source1 As String '//CAM1
    Dim source2 As String ' //CAM2

    Public Sub New()
        InitializeComponent()
        '//CCTV
        Stream1 = New JPEGStream(source1)
        Stream2 = New JPEGStream(source2)
        AddHandler Stream1.NewFrame, New NewFrameEventHandler(AddressOf Stream_NewFream1)
        AddHandler Stream2.NewFrame, New NewFrameEventHandler(AddressOf Stream_NewFream2)

        Bw1.WorkerReportsProgress = True
        Bw1.WorkerSupportsCancellation = True
        Dim clientSocket As New System.Net.Sockets.TcpClient()
    End Sub

    Private Sub FrmWbOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "WB OUT"
        resultLabel.Text = "Start"
        WB_ON = True
        GetWBConfig()
        INDICATORON()
    End Sub
#Region "BW"
    ' This event handler is WHERE THE time-consuming work is done.
    Private Sub backgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles Bw1.DoWork
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        GetWBConfig()
        Dim Ip As String = WBIP
        Dim Port As Int32 = WBPORT

        Try
            Do Until Not WB_ON = True
                If (worker.CancellationPending = True) Then
                    e.Cancel = True
                    Exit Do
                Else
                    Dim responseData As [String] = [String].Empty
                    responseData = GetSCSMessage(Ip, Port)
                    worker.ReportProgress(responseData)
                End If
            Loop
        Catch ex As Exception
        End Try
    End Sub
    Private Sub backgroundWorker1_ProgressChanged(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs) Handles Bw1.ProgressChanged
        TxtWeight.Text = (e.ProgressPercentage.ToString())
    End Sub

    Private Sub backgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs) Handles Bw1.RunWorkerCompleted
        If e.Cancelled = True Then
            resultLabel.Text = "Canceled!"
        ElseIf e.Error IsNot Nothing Then
            resultLabel.Text = "Error: " & e.Error.Message
        Else
            resultLabel.Text = "Done!"
            INDICATORON()
        End If
    End Sub
#End Region
#Region "CCTV"
    Private counter As Integer
    Private Sub Stream_NewFream1(sender As Object, eventargs As NewFrameEventArgs)
        Dim bmp As Bitmap = DirectCast(eventargs.Frame.Clone(), Bitmap)
        PictureBox1.Image = bmp
    End Sub
    Private Sub Stream_NewFream2(sender As Object, eventargs As NewFrameEventArgs)
        Dim bmp As Bitmap = DirectCast(eventargs.Frame.Clone(), Bitmap)
        PictureBox2.Image = bmp
    End Sub

#End Region
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        WB_ON = False
        INDICATOROFF()
        CCTV_OFF()
        Close()
    End Sub

    Private Sub FrmWbOut_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        INDICATOROFF()
        CCTV_OFF()
    End Sub
    Private Sub INDICATORON()
        WB_ON = True
        If Bw1.IsBusy <> True Then
            ' Start THE asynchronous operation.
            Bw1.RunWorkerAsync()
            resultLabel.Text = "Connected..."
        End If
    End Sub
    Private Sub INDICATOROFF()
        WB_ON = False
        If Bw1.WorkerSupportsCancellation = True Then
            ' Cancel THE asynchronous operation.
            Bw1.CancelAsync()
        End If
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD 
        '//VALIDASI AWAL INPUT HARUS KONDISI TIMBANGAN KOSONG
        '//WARNING BERAT SAAT MULAI HARUS KOSONG
        If Val(TxtWeight.Text) > 0 Then
            MsgBox("Berat Jembatan Timbang Belum 0 Kg, Silakan Kosongkan terlebih dahulu Jembatan Timbang", vbInformation, Me.Text)
            SimpleButton1.Enabled = True 'ADD 
            SimpleButton2.Enabled = False 'SAVE
        Else
            If WB_ON = False Then WB_ON = True
            ClearAllText()
            SimpleButton1.Enabled = False 'ADD 
            SimpleButton2.Enabled = True  'SAVE 
            TextEdit3.Text = Format(Now, "dd-MM-yyyy")   'DATE
            '//TAMPILKAN DATA YANG BELUM TIMBANG KELUAR..
            ShowDataWBin()
            CCTV_ON()
        End If
    End Sub
    Private Sub ClearAllText()
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = "" : TextEdit6.Text = "" : TextEdit7.Text = "" : TextEdit8.Text = "" : TextEdit9.Text = "" : TextEdit10.Text = ""
        TextEdit11.Text = "" : TextEdit12.Text = "" : TextEdit13.Text = "" : TextEdit14.Text = "" : TextEdit15.Text = "" : TextEdit16.Text = "" : TextEdit17.Text = "" : TextEdit18.Text = "" : TextEdit19.Text = "" : TextEdit20.Text = ""
        TextEdit21.Text = "" : TextEdit22.Text = "" : TextEdit23.Text = "" : TextEdit24.Text = "" : TextEdit25.Text = "" : TextEdit26.Text = "" : TextEdit27.Text = "" : TextEdit28.Text = "" : TextEdit29.Text = "" : TextEdit30.Text = ""
        TextEdit31.Text = "" : TextEdit32.Text = "" : TextEdit33.Text = "" : TextEdit34.Text = "" : TextEdit35.Text = "" : TextEdit36.Text = "" : TextEdit37.Text = "" : TextEdit38.Text = ""
    End Sub

    Private Sub DisebelAllText()
        TextDisebled({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9, TextEdit10})
        TextDisebled({TextEdit11, TextEdit12, TextEdit13, TextEdit14, TextEdit15, TextEdit16, TextEdit17, TextEdit18, TextEdit19, TextEdit20})
        TextDisebled({TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27, TextEdit28, TextEdit29, TextEdit30})
        TextDisebled({TextEdit31, TextEdit32, TextEdit33, TextEdit34, TextEdit35, TextEdit36, TextEdit37, TextEdit38})
        MemoEdit1.Enabled = False
    End Sub



    Private Sub ShowDataWBin()
        'CARI TIKET KELUAR

        LSQL = " SELECT NO_TICKET,POLICE_NO,WEIGHT_IN,DATE_IN FROM V_TICKET_FINISH WHERE WEIGHT_OUT IS NULL ORDER BY DATE_IN DESC"
        LField = "NO_TICKET"
        ValueLoV = ""
        TextEdit2.Text = FrmShowLOV(FrmLoV, LSQL, "NO_TICKET", "WB IN DATA")
        If Not IsEmptyText({TextEdit2}) = True Then
            TextEdit6.Text = GetTara(VEHICLE_NUMBER)
            Dim DT As New DataTable
            SQL = "SELECT A.* ,B.CONTRACT_NO,B.DELIVERY_QUANTITY,B.ITEMNO,B.SALESORDERNO_DUP " +
            " FROM T_WBTICKET A " +
            " LEFT JOIN T_WBTICKET_DETAIL B ON B.NO_TICKET= A.NO_TICKET " +
            " WHERE A.NO_TICKET ='" & TextEdit2.Text & "'"

            DT = ExecuteQuery(SQL)
            If DT.Rows.Count > 0 Then
                'ISI DATA 
                TextEdit4.Text = DT.Rows(0).Item("VEHICLE_CODE").ToString  'POLICE NO
                TextEdit8.Text = DT.Rows(0).Item("WEIGHT_IN").ToString  'WEIGH IN
                TextEdit9.Text = DT.Rows(0).Item("WEIGHT_OUT").ToString  'WEIGH OUT
                TextEdit10.Text = DT.Rows(0).Item("NETTO").ToString  'NETTO
                TextEdit11.Text = DT.Rows(0).Item("ADJUST_WEIGHT").ToString  'ADJ WEIGHT PERSEN
                TextEdit38.Text = DT.Rows(0).Item("ADJUST_WEIGHT").ToString  'ADJ WEIGHT DECIMAL

                TextEdit12.Text = DT.Rows(0).Item("ADJUST_NETTO").ToString  'ADJ NETTO

                TextEdit13.Text = DT.Rows(0).Item("MATERIAL_CODE").ToString  'MATERIAL
                TextEdit14.Text = DT.Rows(0).Item("SUPPLIER_CODE").ToString  'VENDOR
                TextEdit15.Text = DT.Rows(0).Item("CONTRACT_NO").ToString  'CONTRACT
                TextEdit16.Text = DT.Rows(0).Item("CUSTOMER_CODE").ToString  'CUSTOMER

                TextEdit17.Text = DT.Rows(0).Item("SO_NUMBER1").ToString  'SALES ORDER1
                TextEdit37.Text = DT.Rows(0).Item("SO_NUMBER2").ToString  'SALES ORDER2

                MemoEdit1.Text = DT.Rows(0).Item("REMARKS").ToString  'REMAKS

                TextEdit18.Text = DT.Rows(0).Item("DRIVER_NAME").ToString  'DRIVER
                TextEdit19.Text = DT.Rows(0).Item("SIM").ToString  'SIM
                TextEdit20.Text = DT.Rows(0).Item("VEHICLE_CODE").ToString  'VEHICLE NO
                TextEdit21.Text = DT.Rows(0).Item("TRANSPORTER_CODE").ToString  'TRANSPORTER
                TextEdit22.Text = DT.Rows(0).Item("ADJUST_NETTO").ToString  'DO TRANSPORTER

                TextEdit23.Text = DT.Rows(0).Item("DO_SPB").ToString  'NAB/SPB
                TextEdit24.Text = DT.Rows(0).Item("ESTATE").ToString  'ESTATE
                TextEdit25.Text = DT.Rows(0).Item("AFDELING").ToString  'AFDELING
                TextEdit26.Text = DT.Rows(0).Item("BLOCK").ToString  'BLOCK
                TextEdit27.Text = DT.Rows(0).Item("FFB_UNITS").ToString  'FFB UNIT

                TextEdit28.Text = DT.Rows(0).Item("TAHUN_TANAM").ToString  'PLATING YEAR
                TextEdit29.Text = Val(TextEdit11.Text) / Val(TextEdit27.Text) 'ABW=ADJUST WEIGHT /FFB UNIT
                TextEdit30.Text = DT.Rows(0).Item("LOADER1").ToString  'LOADER1
                TextEdit31.Text = DT.Rows(0).Item("LOADER2").ToString  'LOADER2
                TextEdit32.Text = DT.Rows(0).Item("LOADER3").ToString  'LOADER3

                TextEdit33.Text = DT.Rows(0).Item("FFA").ToString  'FFA
                TextEdit34.Text = DT.Rows(0).Item("MOISTURE").ToString  'MOISTURE
                TextEdit35.Text = DT.Rows(0).Item("DIRT").ToString  'DIRT
                TextEdit36.Text = DT.Rows(0).Item("NO_SEGEL").ToString  'NO SEGEL


                TextEdit7.Text = GetTipeTrWB(TextEdit2.Text) 'TYPE TR WB

                DisebelAllText() 'DISEBEL ALL

                'BUKA SESUAI VALIDASI
                MemoEdit1.Enabled = True 'REMARKS 

                'JENIS TRANSAKSI
                'PERNERIMAAN,PENGELUARAN ,NUMPANG TIMBANG
                Dim JTRAN As String = ""
                Dim MATERIAL As String = TextEdit13.Text
                Dim SPL As String = Microsoft.VisualBasic.Left(TextEdit14.Text, 4)
                Dim CUSTOMER As String = TextEdit16.Text

                Dim CONTRACT As String = TextEdit15.Text
                Dim SO1 As String = TextEdit17.Text

                If SPL <> "" And CONTRACT <> "" Then
                    JTRAN = "PENERIMAAN"
                ElseIf SPL = "VINT" Or SPL = "TRST" Then
                    JTRAN = "PENERIMAAN"
                ElseIf CUSTOMER <> "" And SO1 <> "" Then
                    JTRAN = "PENGELUARAN"
                ElseIf MATERIAL <> "" Then
                    JTRAN = "NUMPANG TIMBANG"
                End If

                LabelControl89.Text = JTRAN
                Me.Text = JTRAN & " - WB OUT"
                Select Case UCase(JTRAN)
                    Case "PENERIMAAN"
                        'MATERIAL
                        If MATERIAL = "501010001" Then
                            TextEnebled({TextEdit11, TextEdit38}) 'ADJUST WB 
                            TextEnebled({TextEdit30, TextEdit31, TextEdit32}) 'LOADER 123
                            TextEnebled({TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27}) 'NAB.AFDELING,BLOCK,FFB,PL
                        Else
                            TextDisebled({TextEdit11, TextEdit38}) 'ADJUST WB
                            TextDisebled({TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27}) 'NAB.AFDELING,BLOCK,FFB,PL
                        End If
                        'VENDOR
                        If SPL = "VINT" Then
                            TextDisebled({TextEdit11, TextEdit38}) 'ADJUST WB
                        Else
                            TextEnebled({TextEdit11, TextEdit38}) 'ADJUST WB 
                        End If
                        'VENDOR INTERNAL FLAG KHUSUS
                    Case "PENGELUARAN"
                    Case "NUMPANG TIMBANG"

                End Select

            End If

        End If
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click

        If Val(TxtWeight.Text) <> "0" Then
            'GET PIC & WB

            TextEdit5.Text = TxtWeight.Text 'WEIGHT
            TextEdit9.Text = TextEdit5.Text 'WEIGHT OUT
            TextEdit10.Text = Val(TextEdit9.Text) - Val(TextEdit8.Text) 'NETTO
            TextEdit11.Text = Val(TextEdit11.Text) 'ADJUST_WEIGHT
            TextEdit12.Text = Val(TextEdit10.Text) - Val(TextEdit11.Text) 'ADJUST_NETTO
        End If
    End Sub

    Private Sub CCTV_OFF()
        LabelControl41.Text = "CAM 1 Off"
        LabelControl42.Text = "CAM 2 Off"
        If Stream1.IsRunning = True Then Stream1.Stop()
        If Stream2.IsRunning = True Then Stream2.Stop()
    End Sub

    Private Sub CCTV_ON()
        Dim source1 As String = GetCCTVParam(IPCamera1)
        Dim source2 As String = GetCCTVParam(IPCamera2)
        LabelControl41.Text = "CAM 1 Start"
        LabelControl42.Text = "CAM 2 Start"
        Stream1.Source = source1
        Stream2.Source = source2
        If Stream1.IsRunning = False Then Stream1.Start()
        If Stream2.IsRunning = False Then Stream2.Start()
    End Sub

    Private Sub SimpleButton14_Click(sender As Object, e As EventArgs) Handles SimpleButton14.Click
        'CANCEL
        ClearAllText()
        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        CCTV_OFF()
    End Sub

    'ADJUST WG PERSEN
    Private Sub TextEdit11_EditValueChanged(sender As Object, e As EventArgs) Handles TextEdit11.EditValueChanged
        If TextEdit11.Text <> "" Then TextEdit38.Text = ""
    End Sub
    'ADJUST WG DESIMAL
    Private Sub TextEdit38_EditValueChanged(sender As Object, e As EventArgs) Handles TextEdit38.EditValueChanged
        If TextEdit38.Text <> "" Then TextEdit11.Text = ""
    End Sub

    Private Sub CAPTURE_CCTV()
        Dim tiket As String = TextEdit2.Text
        Dim time As DateTime = DateTime.Now
        Dim format As String = "MMM ddd d HH mm yyyy"
        'Dim strFilename As [String] = "Capture-" + time.ToString(format) + ".jpg"
        Dim strFilename As String = "CAM1" + tiket + ".jpg"
        If Stream1.IsRunning Then
            Dim picture As Bitmap = PictureBox1.Image
            picture.Save(strFilename, ImageFormat.Jpeg)
        End If
        Dim strFilename2 As String = "CAM2" + tiket + ".jpg"
        If Stream2.IsRunning Then
            Dim picture As Bitmap = PictureBox2.Image
            picture.Save(strFilename, ImageFormat.Jpeg)
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE HEADER DAN DETAIL 
        '//WB OUT ADA SPLIT CONTRACT UNTUK BERAT YANG  SUDAH MELEBIHI KONTRAK YANG DI PAKAI
        CAPTURE_CCTV()

        Dim NO_TIKET As String = TextEdit2.Text
        SQL = "SELECT * FROM T_WBTICKET WHERE NO_TICKET ='" & NO_TIKET & "'"
        If CheckRecord(SQL) > 0 Then
            UpdateTWB(NO_TIKET)
        End If


        ' PRINT TIKET BY MATERIAL
        Dim MATERIAL As String = TextEdit13.Text
        Dim DTX As New DataTable
        SQL = "SELECT MATERIAL FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit2.Text & "'"
        DTX = ExecuteQuery(SQL)
        If DTX.Rows.Count > 0 Then
            MATERIAL = DTX.Rows(0).Item("MATERIAL").ToString
            Select Case UCase(MATERIAL)
                Case = "FFB"
                    SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit2.Text & "' AND MATERIAL='FFB'"
                    ShowReport("RPTTICKETFFB", SQL, "R_TIKET")
                Case = "CPO"
                    SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit2.Text & "'  AND MATERIAL='CPO'"
                    ShowReport("RPTTICKETCPO", SQL, "R_TIKETCPO")
                Case <> "FFB" And "CPO"
                    SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit2.Text & "' AND MATERIAL<>'FFB' AND MATERIAL<>'CPO' "
                    ShowReport("RPTTICKETFFB", SQL, "R_TIKET")
            End Select
        End If
    End Sub
    Private Sub UpdateTWB(NOTICKET)
        'UPDATE HEADER
        SQL = "Update T_WBTICKET SET " +
            " CUSTOMER_CODE ='" & TextEdit16.Text & "'," +
            " SUPPLIER_CODE ='" & TextEdit14.Text & "'," +
            " TRANSPORTER_CODE ='" & TextEdit21.Text & "'," +
            " VEHICLE_CODE ='" & TextEdit20.Text & "'," +
            " DO_SPB ='" & TextEdit23.Text & "'," +
            " WEIGHT_OUT ='" & TextEdit9.Text & "'," +
            " NETTO ='" & TextEdit10.Text & "'," +
            " ADJUST_WEIGHT ='" & TextEdit11.Text & "'," +
            " ADJUST_NETTO ='" & TextEdit12.Text & "'," +
            " MATERIAL_CODE ='" & TextEdit13.Text & "'," +
            " DRIVER_NAME ='" & TextEdit18.Text & "'," +
            " FFA ='" & TextEdit33.Text & "'," +
            " MOISTURE ='" & TextEdit34.Text & "'," +
            " DIRT ='" & TextEdit35.Text & "'," +
            " NO_SEGEL ='" & TextEdit36.Text & "'," +
            " SIM ='" & TextEdit19.Text & "'," +
            " REMARKS ='" & MemoEdit1.Text & "'," +
            " TAHUN_TANAM ='" & TextEdit28.Text & "'," +
            " ESTATE ='" & TextEdit24.Text & "'," +
            " AFDELING ='" & TextEdit25.Text & "'," +
            " BLOCK ='" & TextEdit26.Text & "'," +
            " FFB_UNITS ='" & TextEdit27.Text & "'," +
            " LOADER1 ='" & TextEdit30.Text & "'," +
            " LOADER2 ='" & TextEdit31.Text & "'," +
            " LOADER3 ='" & TextEdit32.Text & "'," +
            " SO_NUMBER1 ='" & TextEdit17.Text & "'," +
            " SO_NUMBER2 ='" & TextEdit37.Text & "'," +
            " ABW='" & TextEdit29.Text & "' " +
            " WHERE NO_TICKET ='" & NOTICKET & "'"
        ExecuteNonQuery(SQL)

        'UPDATE DETAIL
        SQL = "UPDATE T_WBTICKET_DETAIL " +
            " SET " +
            " NO_TICKET," +
            " CONTRACT_NO, " +
            " ITEMNO, " +
            " DELIVERY_QUANTITY, " +
            " SALESORDERNO_DUP, " +
            " INPUT_BY, " +
            " INPUT_DATE, " +
            " UP_DATE, " +
            " UP_DATEBY=''" +
            " WHERE NO_TICKET ='" & NOTICKET & "'"
        ExecuteNonQuery(SQL)

        'CHEK KUOTA KONTRAK 
        'JIKA MELEBIHI KUOTA KONTRAK YANG DIPILIH MAKA MUNCUL POPUP SPLIT KONTRAK UNTUK
        'PEMBUATAN KONTRAK BARU / PILIH KONTRAK LAIN DENGAN SISA YANG MENCUKUPI .. 
        '1 NO TIKET (HEADER ) DETAIL 1 NO TIKET 2 NO KONTRAK (LAMA DAN BARU)

        MsgBox("SAVE SUCCESFUL", vbInformation, Me.Text)
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'MATERIAL
        'MATERIAL
        LSQL = "SELECT MATERIAL_CODE,MATERIAL_NAME ,INACTIVE FROM T_MATERIAL   WHERE INACTIVE IS NULL OR INACTIVE='N'"
        LField = "MATERIAL_CODE"
        ValueLoV = ""
        TextEdit13.Text = FrmShowLOV(FrmLoV, LSQL, "MATERIAL", "MATERIAL")

    End Sub
End Class