
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading.Tasks
Imports System.ComponentModel
Imports System.Drawing.Imaging

Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Imports AForge.Video.DirectShow
Imports AForge.Video

Public Class FrmWbIn
    Private Delegate Sub AppendTextBoxDelegate(ByVal TB As String, ByVal txt As String)
    Dim source1 As String '//CAM1
    Dim source2 As String ' //CAM2
    Dim stream1 As JPEGStream
    Dim stream2 As JPEGStream

    Public Sub New()
        InitializeComponent()
        '//CCTV
        stream1 = New JPEGStream(source1)
        stream2 = New JPEGStream(source2)
        AddHandler stream1.NewFrame, New NewFrameEventHandler(AddressOf Stream_NewFream1)
        AddHandler stream2.NewFrame, New NewFrameEventHandler(AddressOf Stream_NewFream2)
        'BW
        BW1.WorkerReportsProgress = True
        BW1.WorkerSupportsCancellation = True
        'WB
        Dim clientSocket As New System.Net.Sockets.TcpClient()
    End Sub

    Private Sub FrmWbOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = nFormName
        resultLabel.Text = "Start"
        GetWBConfig()
        INDICATORON()
    End Sub
#Region "BW"
    Private Sub backgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As DoWorkEventArgs) Handles BW1.DoWork
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
    Private Sub backgroundWorker1_ProgressChanged(ByVal sender As System.Object, ByVal e As ProgressChangedEventArgs) Handles BW1.ProgressChanged
        TxtWeight.Text = (e.ProgressPercentage.ToString())
    End Sub
    Private Sub backgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As RunWorkerCompletedEventArgs) Handles BW1.RunWorkerCompleted
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
        If BW1.IsBusy <> True Then
            BW1.RunWorkerAsync()
            resultLabel.Text = "Connected..."
        End If
    End Sub
    Private Sub INDICATOROFF()
        WB_ON=False 
        If BW1.WorkerSupportsCancellation = True Then
            BW1.CancelAsync()
        End If
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD 
        'VALIDASI AWAL INPUT HARUS KONDISI TIMBANGAN KOSONG
        'WARNING BERAT SAAT MULAI HARUS KOSONG
        If Val(TxtWeight.Text) > 0 Then
            MsgBox("Berat Jembatan Timbang Belum 0 Kg, Silakan Kosongkan terlebih dahulu Jembatan Timbang", vbInformation, Me.Text)
            SimpleButton1.Enabled = True 'ADD 
            SimpleButton2.Enabled = False 'SAVE
        Else
            ClearAllText()
            SimpleButton1.Enabled = False 'ADD 
            SimpleButton2.Enabled = True  'SAVE 
            TextEdit3.Text = Format(Now, "dd-MM-yyyy")   'DATE
            TextEdit2.Text = GetTiketNew(WBCode) 'GETTIKET
            CCTV_ON()
        End If
        'DEFAULT DISEBEL
        'KIT,CUSTOMER,SO,DELIVERY,TRANSPOT,
        TextDisebled({TextEdit1, TextEdit11, TextEdit29, TextEdit12, TextEdit16})
    End Sub
    Private Sub ClearAllText()
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = "" : TextEdit6.Text = "" : TextEdit7.Text = "" : TextEdit8.Text = "" : TextEdit9.Text = "" : TextEdit10.Text = ""
        TextEdit11.Text = "" : TextEdit12.Text = "" : TextEdit13.Text = "" : TextEdit14.Text = "" : TextEdit15.Text = "" : TextEdit16.Text = "" : TextEdit17.Text = "" : TextEdit18.Text = "" : TextEdit19.Text = "" : TextEdit20.Text = ""
        TextEdit21.Text = "" : TextEdit22.Text = "" : TextEdit23.Text = "" : TextEdit24.Text = "" : TextEdit25.Text = "" : TextEdit26.Text = "" : TextEdit27.Text = ""
        TextEdit28.Text = ""
    End Sub

    Private Sub DisebelAllText()
        TextDisebled({TextEdit1, TextEdit2, TextEdit3, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9, TextEdit10})
        TextDisebled({TextEdit11, TextEdit12, TextEdit13, TextEdit14, TextEdit15, TextEdit16, TextEdit17, TextEdit18, TextEdit19, TextEdit20})
        TextDisebled({TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27})
        TextDisebled({TextEdit28})
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
                ' TextEdit38.Text = DT.Rows(0).Item("ADJUST_WEIGHT").ToString  'ADJ WEIGHT DECIMAL

                TextEdit12.Text = DT.Rows(0).Item("ADJUST_NETTO").ToString  'ADJ NETTO

                TextEdit13.Text = DT.Rows(0).Item("MATERIAL_CODE").ToString  'MATERIAL
                TextEdit14.Text = DT.Rows(0).Item("SUPPLIER_CODE").ToString  'VENDOR
                TextEdit15.Text = DT.Rows(0).Item("CONTRACT_NO").ToString  'CONTRACT
                TextEdit16.Text = DT.Rows(0).Item("CUSTOMER_CODE").ToString  'CUSTOMER

                TextEdit17.Text = DT.Rows(0).Item("SO_NUMBER1").ToString  'SALES ORDER1



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

                'TextEdit28.Text = DT.Rows(0).Item("TAHUN_TANAM").ToString  'PLATING YEAR
                'TextEdit29.Text = Val(TextEdit11.Text) / Val(TextEdit27.Text) 'ABW=ADJUST WEIGHT /FFB UNIT
                'TextEdit30.Text = DT.Rows(0).Item("LOADER1").ToString  'LOADER1
                'TextEdit31.Text = DT.Rows(0).Item("LOADER2").ToString  'LOADER2
                'TextEdit32.Text = DT.Rows(0).Item("LOADER3").ToString  'LOADER3

                'TextEdit33.Text = DT.Rows(0).Item("FFA").ToString  'FFA
                'TextEdit34.Text = DT.Rows(0).Item("MOISTURE").ToString  'MOISTURE
                'TextEdit35.Text = DT.Rows(0).Item("DIRT").ToString  'DIRT
                'TextEdit36.Text = DT.Rows(0).Item("NO_SEGEL").ToString  'NO SEGEL


                TextEdit7.Text = GetTipeTrWB(TextEdit2.Text) 'TYPE TR WB

                DisebelAllText() 'DISEBEL ALL

                'BUKA SESUAI VALIDASI

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
                            TextEnebled({TextEdit11}) 'ADJUST WB 
                            'TextEnebled({TextEdit30, TextEdit31, TextEdit32}) 'LOADER 123
                            TextEnebled({TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27}) 'NAB.AFDELING,BLOCK,FFB,PL
                        Else
                            TextDisebled({TextEdit11}) 'ADJUST WB
                            TextDisebled({TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27}) 'NAB.AFDELING,BLOCK,FFB,PL
                        End If
                        'VENDOR
                        If SPL = "VINT" Then
                            TextDisebled({TextEdit11}) 'ADJUST WB
                        Else
                            TextEnebled({TextEdit11}) 'ADJUST WB 
                        End If
                        'VENDOR INTERNAL FLAG KHUSUS
                    Case "PENGELUARAN"
                    Case "NUMPANG TIMBANG"

                End Select

            End If

        End If
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        'CAPTURE IMAGE
        stream1.Source = GetCCTVParam(IPCamera1)
        stream2.Source = GetCCTVParam(IPCamera2)
        If stream1.IsRunning = False Then stream1.Start()
        If stream2.IsRunning = False Then stream2.Start()

        If stream1.IsRunning = True Then stream1.SignalToStop()
        If stream2.IsRunning = True Then stream2.SignalToStop()
        Dim ntiket As String = GetTiketNew(WBCode)
        SIMPANGAMBAR(ntiket)                                'SAVE GAMBAR
        'Get WBvalue
        If TxtWeight.Text > 0 Then
            TextEdit5.Text = TxtWeight.Text                'WB WEIGHT
            TextEdit6.Text = GetTara(TextEdit4.Text)       'TARA  DARI NO VEHICLE
        End If
    End Sub

    Private Sub CCTV_OFF()
        If Ping(IPCamera1) = True Then
            stream1.Source = GetCCTVParam(IPCamera1)
            If stream1.IsRunning = True Then stream1.Stop()
            LabelControl41.Text = "CAM 1 Off"
        End If
        If Ping(IPCamera2) = True Then
            stream2.Source = GetCCTVParam(IPCamera2)
            If stream2.IsRunning = True Then stream2.Stop()
            LabelControl42.Text = "CAM 2 Off"
        End If
    End Sub
    Private Sub CCTV_ON()
        If Ping(IPCamera1) = True Then
            stream1.Source = GetCCTVParam(IPCamera1)
            If stream1.IsRunning = False Then stream1.Start()
            LabelControl41.Text = "CAM 1 On"
        Else
            LabelControl41.Text = "CAM 1 Off"
        End If

        If Ping(IPCamera2) = True Then
            stream2.Source = GetCCTVParam(IPCamera2)
            If stream2.IsRunning = False Then stream2.Start()
            LabelControl42.Text = "CAM 2 On"
        Else
            LabelControl42.Text = "CAM 2 Off"
        End If
    End Sub
    Private Sub SimpleButton14_Click(sender As Object, e As EventArgs) Handles SimpleButton14.Click
        'CANCEL
        ClearAllText()
        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        CCTV_OFF()
    End Sub

    'ADJUST WG PERSEN
    'Private Sub TextEdit11_EditValueChanged(sender As Object, e As EventArgs) Handles TextEdit11.EditValueChanged
    '    If TextEdit11.Text <> "" THEn TextEdit38.Text = ""
    'End Sub
    'ADJUST WG DESIMAL
    'Private Sub TextEdit38_EditValueChanged(sender As Object, e As EventArgs)
    '    If TextEdit38.Text <> "" THEn TextEdit11.Text = ""
    'End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE
        'VALIDASI 'NO TIKET,KENDARANN,BERAT,
        IsEmptyText({TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9})
        Dim SPL As String = Microsoft.VisualBasic.Left(TextEdit9.Text, 4)
        If SPL = "VINT" Or SPL = "TRST" Or SPL = "MILL" Then
            IsEmptyText({TextEdit10, TextEdit13, TextEdit21}) 'KONTRAK, REFF ,AFDELING,BLOK
        End If

        Dim NO_TIKET As String = TextEdit2.Text
        SQL = "SELECT * FROM T_WBTICKET WHERE NO_TICKET ='" & NO_TIKET & "'"
        If CheckRecord(SQL) = 0 Then
            SIMPANTIKET(NO_TIKET)
        End If
    End Sub
    Private Sub SIMPANTIKET(ByVal NOTICKET)

        SIMPANGAMBAR(GetTiketNew(WBCode))

        Dim imagename As String = LabelControl41.Text
        Dim fls As FileStream
        fls = New FileStream(imagename, FileMode.Open, FileAccess.Read)
        Dim blob As Byte() = New Byte(fls.Length - 1) {}
        fls.Read(blob, 0, System.Convert.ToInt32(fls.Length))
        fls.Close()

        Dim imagename2 As String = LabelControl42.Text
        'Dim IMAGENAME2 As String = "C://IMAGE_SWS/TEMP/CAM2-INWB2220170900001-C.jpg"
        Dim fls2 As FileStream
        fls2 = New FileStream(imagename2, FileMode.Open, FileAccess.Read)
        Dim blob2 As Byte() = New Byte(fls2.Length - 1) {}
        fls2.Read(blob2, 0, System.Convert.ToInt32(fls2.Length))
        fls2.Close()

        Dim NO_TICKET As String = TextEdit2.Text
        SQL = "INSERT INTO T_WBTICKET ( " +
            " NO_TICKET,CUSTOMER_CODE,SUPPLIER_CODE,TRANSPORTER_CODE,VEHICLE_CODE,WBCode,DO_SPB,DATE_IN, " +
            " WEIGHT_IN,NETTO,MATERIAL_CODE," +
            " DRIVER_NAME, EMP_NAME, FFA, MOISTURE, DIRT, NO_SEGEL, SIM, TIMECAPTUREIN, " +
            " NO_NOKI, DELIVERY_TYPE, JNS_TIMBANGAN, REMARKS, INPUT_BY, INPUT_DATE, " +
            " STATUS, TAHUN_TANAM, ESTATE, AFDELING, BLOCK, FFB_UNITS, " +
            " KIT, PIC_PLATIN, PIC_DRIVERIN, LOADER1, LOADER2, LOADER3, SO_NUMBER1, SO_NUMBER2, ABW) VALUES ( " +
            " '" & NOTICKET & "','" & TextEdit11.Text & "','" & TextEdit9.Text & "', '" & TextEdit16.Text & "','" & TextEdit4.Text & "','" & WBCode & "' ,'" & TextEdit20.Text & "',SYSDATE, " +
            " '" & TextEdit5.Text & "','" & TextEdit6.Text & "','" & TextEdit8.Text & "'," +
            " '" & TextEdit14.Text & "','" & USERNAME & "', '" & TextEdit25.Text & "','" & TextEdit26.Text & "','" & TextEdit27.Text & "','" & TextEdit28.Text & "','" & TextEdit15.Text & "' ,SYSDATE, " +
            " '','" & TextEdit12.Text & "','" & TextEdit7.Text & "', '','" & USERNAME & "',SYSDATE, " +
            " '', '" & TextEdit24.Text & "', '','" & TextEdit21.Text & "','" & TextEdit22.Text & "','" & TextEdit23.Text & "'," +
            " '', " + " :BlobParameter, " + " :BlobParameter2,'" & TextEdit17.Text & "' ,'" & TextEdit18.Text & "' , '" & TextEdit19.Text & "' , '" & TextEdit29.Text & "' ,'','') "

        Dim CMD As OracleCommand = New OracleCommand(SQL, CONN)
        Dim paramCollection As OracleParameterCollection = CMD.Parameters
        Dim parameter As Object = New OracleParameter("BlobParameter", OracleDbType.Blob)
        Dim parameter2 As Object = New OracleParameter("BlobParameter2", OracleDbType.Blob)

        paramCollection.AddWithValue(":BlobParameter", blob)
        paramCollection.AddWithValue(":BlobParameter2", blob2)

        CMD.Connection.Open()
        CMD.ExecuteNonQuery()
        CMD.Dispose()



    End Sub
    Private Sub SIMPANGAMBAR(ByVal NOTICKET)
        'PROSES GAMBAR
        Dim time As DateTime = DateTime.Now
        ' Use current time
        Dim format As String = "MMM ddd d HH mm yyyy"
        ' Use this format
        Dim strFilename As [String] = "CAM1-IN" + NOTICKET + ".jpg"
        strFilename = My.Settings.PathImage & "/" & strFilename
        If stream1.IsRunning = False Then
            Dim picture As Bitmap = PictureBox1.Image
            If picture IsNot Nothing Then
                picture.Save(strFilename, ImageFormat.Jpeg)
                LabelControl41.Text = strFilename
            Else
                strFilename = My.Settings.PathImage & "/SWS1.JPG"
                picture.Save(strFilename, ImageFormat.Jpeg)
                LabelControl41.Text = strFilename
            End If
        End If

        Dim strFilename2 As [String] = "CAM2-IN" + NOTICKET + ".jpg"
        strFilename2 = My.Settings.PathImage & "/" & strFilename2
        If stream2.IsRunning = False Then
            Dim picture2 As Bitmap = PictureBox2.Image
            If picture2 IsNot Nothing Then
                picture2.Save(strFilename2, ImageFormat.Jpeg)
                LabelControl42.Text = strFilename2
            Else
                strFilename2 = My.Settings.PathImage & "/SWS2.JPG"
                picture2.Save(strFilename2, ImageFormat.Jpeg)
                LabelControl42.Text = strFilename2
            End If
        End If
    End Sub
    'GANTI INSERT
    Private Sub UpdateTWB(NOTICKET)
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
            " ESTATE ='" & TextEdit24.Text & "'," +
            " AFDELING ='" & TextEdit25.Text & "'," +
            " BLOCK ='" & TextEdit26.Text & "'," +
            " FFB_UNITS ='" & TextEdit27.Text & "'," +
            " SO_NUMBER1 ='" & TextEdit17.Text & "'," +
                        " WHERE NO_TICKET ='" & NOTICKET & "'"
        ExecuteNonQuery(SQL)

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
        MsgBox("SAVE SUCCESFUL", vbInformation, Me.Text)
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'VEHICLE NUMBER
        LSQL = " SELECT distinct VEHICLE_CODE,VEHICLE_TYPE,PLATE_NUMBER,TARE,TRANSPORTER_CODE  " +
               " FROM T_VEHICLE WHERE INACTIVE is null "
        LField = "PLATE_NUMBER"
        ValueLoV = ""
        TextEdit4.Text = FrmShowLOV(FrmLoV, LSQL, "PLATE_NUMBER", "VEHICLE")
        VEHICLE_NUMBER = Trim(TextEdit4.Text)
        TextEdit6.Text = GetTara(VEHICLE_NUMBER)
    End Sub

    Private Sub SimpleButton13_Click(sender As Object, e As EventArgs) Handles SimpleButton13.Click
        'WB TYPE
        Dim CODE As String
        LSQL = "SELECT * FROM T_WB_TYPE WHERE INACTIVE IS NULL"
        LField = "WB_TYPE_CODE"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "JENIS ", "Jenis Timbangan")
        TextEdit7.Text = Val(CODE)
    End Sub

    Private Sub SimpleButton8_Click(sender As Object, e As EventArgs) Handles SimpleButton8.Click
        'MATERIAL
        'MATERIAL
        LSQL = "SELECT MATERIAL_CODE,MATERIAL_NAME ,INACTIVE FROM T_MATERIAL  WHERE INACTIVE IS NULL "
        LField = "MATERIAL_CODE"
        ValueLoV = ""
        TextEdit8.Text = FrmShowLOV(FrmLoV, LSQL, "MATERIAL", "MATERIAL")

        TextEnabled({TextEdit17, TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24})
        TextEnabled({TextEdit25, TextEdit26, TextEdit27, TextEdit28})

        If UCase(TextEdit8.Text) = "501010001" Then
            'FFA,MOISTURE,DIRT,NO SEGEL (DISEBEL)
            TextDisebled({TextEdit24, TextEdit25, TextEdit26, TextEdit27})
        Else
            'LOADER 1,2,3,NAB,AFDELING,BLOCK,FFB UNIT,PL YEAR,(DISEBEL)
            TextDisebled({TextEdit17, TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24})
        End If

    End Sub

    Private Sub SimpleButton9_Click(sender As Object, e As EventArgs) Handles SimpleButton9.Click
        'SUPPLIER
        'SUPPLIER/VENDOR
        LSQL = "SELECT VENDOR_CODE,VENDOR_NAME  FROM T_VENDOR WHERE INACTIVE IS NULL "
        LField = "VENDOR_CODE"
        ValueLoV = ""
        TextEdit9.Text = FrmShowLOV(FrmLoV, LSQL, "SUPPLIER", "SUPPLIER")
        Dim SPL As String = Microsoft.VisualBasic.Left(TextEdit9.Text, 4)
        TextEnabled({TextEdit10}) 'CONTRACT
        If SPL = "VINT" Then
            TextDisebled({TextEdit10}) 'CONTRACT
        ElseIf SPL = "TRST" Then
            TextDisebled({TextEdit10}) 'CONTRACT
        ElseIf SPL = "MILL" Then
            TextDisebled({TextEdit10}) 'CONTRACT
        End If

    End Sub

    Private Sub SimpleButton10_Click(sender As Object, e As EventArgs) Handles SimpleButton10.Click
        'CONTRACT NUMBER
        Dim CODE As String
        LSQL = " SELECT CONTRACT_NO,VENDORID,ITEMNO,MATERIALCODE FROM T_CONTRACT  " +
        " WHERE VENDORID Like '%" & TextEdit3.Text & "%' " +
        " And MATERIALCODE Like '%" & TextEdit7.Text & "%' " +
        " And INACTIVE Is NULL "
        LField = "CONTRACT_NO"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "CONTRACT_NO", "CONTRACT_NO")
        TextEdit10.Text = CODE
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'CUATOMER
        'CUSTOMER
        LSQL = "SELECT CUST_CODE,CUST_NAME,INACTIVE  FROM T_CUSTOMER WHERE INACTIVE IS NULL "
        LField = "CUST_CODE"
        ValueLoV = ""
        TextEdit11.Text = FrmShowLOV(FrmLoV, LSQL, "CUSTOMER", "CUSTOMER")
        TextEdit9.Text = ""
    End Sub
    Private Sub TextEdit4_LostFocus(sender As Object, e As EventArgs) Handles TextEdit4.LostFocus
        'VALIDASI INPUT NO KENDARAAN

    End Sub

    Private Sub TxtWeight_EditValueChanged(sender As Object, e As EventArgs) Handles TxtWeight.EditValueChanged
        Dim KG As Integer = TxtWeight.Text
        If KG > 0 Then
            SimpleButton6.Enabled = True
        Else
            SimpleButton6.Enabled = False
        End If
    End Sub

End Class