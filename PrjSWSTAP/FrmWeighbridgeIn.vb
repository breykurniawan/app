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

Imports AForge
Imports AForge.Video
Imports System.Drawing.Imaging

Public Class FrmWeighbridgeIn
    Dim WBDisplay As Integer
    Dim WBStstus As Boolean = False
    Private counter As Integer

    Dim Stream1 As JPEGStream
    Dim Stream2 As JPEGStream
    Dim Parameter1 As String = ""
    Dim Parameter2 As String = ""
    Public Sub New()
        ' This call is required by THE designer.
        InitializeComponent()
        ' Add any initialization after THE InitializeComponent() call.
        Stream1 = New JPEGStream(Parameter1)
        AddHandler Stream1.NewFrame, New NewFrameEventHandler(AddressOf Stream_NewFream)
    End Sub

    Private Sub Stream_NewFream(sender As Object, eventargs As NewFrameEventArgs)
        Dim bmp As Bitmap = DirectCast(eventargs.Frame.Clone(), Bitmap)
        PictureEdit1.Image = bmp
    End Sub

    Private Sub FrmWeighbridgeIn_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "PENERIMAAN WB IN"
        CreateHeader()
        ClearAll()
        LockAll()
        SimpleButton1.Select()
        'ambil Konfig WB & CCTV

        LabelControl34.Text = GetWBConfig()
    End Sub


    Private Sub SimpleButton14_Click(sender As Object, e As EventArgs) Handles SimpleButton14.Click
        'CANCEL 
        ClearAll()
        LockAll()
        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        SimpleButton1.Select()
        'MATIKAN TIMBANGAN
    End Sub
    Private Sub CreateHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"DATE_TICKET", "VEHICLE_CODE", "PLATE_NUMBER", "WEIGHT", "TARE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("TRANSPORTER_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("VEHICLE_CODE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.ExpandAllGroups()

    End Sub
    Private Sub InitializeTimer()
        ' Run this procedure in an appropriate event.  
        counter = 0
        Timer1.Interval = 600
        Timer1.Enabled = True
    End Sub

    Private Sub LockAll()
        TextDisebled({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9, TextEdit10})
        TextDisebled({TextEdit11, TextEdit12, TextEdit13, TextEdit14, TextEdit15, TextEdit16, TextEdit17, TextEdit18, TextEdit19, TextEdit20})
        TextDisebled({TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27, TextEdit28, TextEdit29})
        'tombol off
        buttonOff()
    End Sub
    Private Sub buttonOff()
        SimpleButton4.Enabled = False
        SimpleButton6.Enabled = False
        SimpleButton7.Enabled = False
        SimpleButton8.Enabled = False
        SimpleButton9.Enabled = False
        SimpleButton10.Enabled = False
        SimpleButton11.Enabled = False
        SimpleButton12.Enabled = False
    End Sub
    Private Sub buttonOn()
        SimpleButton4.Enabled = True
        SimpleButton6.Enabled = True
        SimpleButton7.Enabled = True
        SimpleButton8.Enabled = True
        SimpleButton9.Enabled = True
        SimpleButton10.Enabled = True
        SimpleButton11.Enabled = True
        SimpleButton12.Enabled = True
    End Sub


    Private Sub UnLockAll()
        TextEnabled({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9, TextEdit10})
        TextEnabled({TextEdit11, TextEdit12, TextEdit13, TextEdit14, TextEdit15, TextEdit16, TextEdit17, TextEdit18, TextEdit19, TextEdit20})
        TextEnabled({TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27, TextEdit28, TextEdit29})
    End Sub
    Private Sub ClearAll()
        TextClear({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9, TextEdit10})
        TextClear({TextEdit11, TextEdit12, TextEdit13, TextEdit14, TextEdit15, TextEdit16, TextEdit17, TextEdit18, TextEdit19, TextEdit20})
        TextClear({TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27, TextEdit28, TextEdit29})
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UnLockAll()  'ALL TEXT ON
        buttonOn() 'ALL BUTTON ON

        TextEdit1.Enabled = False   'KIT
        TextEdit2.Enabled = False 'TIKET

        TextEdit2.Text = GetTiketNew(WBCode)
        'TextEdit2.Text = ""
        TextEdit3.Text = Format(Now, "dd-MM-yyyy")
        TextEdit4.Text = ""
        SimpleButton4.Focus()
        SimpleButton1.Enabled = False
        ''Nyalakan Koneksi ke Timbangan
        LabelControl34.Text = GetWBConfig()

        'nyalakan cctv
        'parameter ambil config
        'Parameter1 = GetCCTVParam(IPCamera1)
        'Parameter2 = GetCCTVParam(IPCamera2)
        'Stream1.Source = Parameter1
        'Stream2.Source = Parameter2
        'If Stream1.IsRunning = False THEn Stream1.Start()
        'If Stream2.IsRunning = False THEn Stream2.Start()

        ' Start input
        TextEdit4.Enabled = True : TextEdit4.BackColor = Color.White 'VEHICLE NUMBER
        SimpleButton4.Enabled = True : SimpleButton4.Select()  'vehicle number 
        TextEdit5.Enabled = True : TextEdit5.BackColor = Color.LemonChiffon  'WEIGHT VALUE
        TextEdit6.Enabled = True : TextEdit6.BackColor = Color.LemonChiffon 'TARA

        'DEFAULT DISEBEL
        'CUSTOMER,'SALES ORDER,'DELIVERY TIPE,'TRANSPORTER,'
        'TOMBOL VOUCER GRADING
        '
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        'Get WBvalue
        Dim i As Integer
        If WBDisplay > 0 Then
            TextEdit5.Text = WBDisplay
            For i = 1 To 20   'CHEK 10X 
                If i = 10 And TextEdit5.Text = WBDisplay Then
                    TextEdit5.Text = WBDisplay
                End If
            Next
            Validasi_Tara()
            If TextEdit4.Text <> "" Then
                TextEdit6.Text = GetTara(VEHICLE_NUMBER)

            Else
                TextEdit6.Text = "0"
            End If
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'close
        'WB OFF
        If WBStstus = True Then
            WBStstus = False
            TxtWeight.Text = 0
        End If
        'CAMERA OFF
        If Stream1.IsRunning = True Then Stream1.Stop()
        If Stream2.IsRunning = True Then Stream2.Stop()
        PictureEdit1.Image = Nothing
        PictureEdit2.Image = Nothing
        Close()
    End Sub
    Private Sub TextEdit5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit5.KeyPress
        e.Handled = NumericOnly(e)
    End Sub

    Private Sub TextEdit6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit6.KeyPress
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit26_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit26.KeyPress
        'ffa
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit27_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit27.KeyPress
        'moisture
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit28_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit28.KeyPress
        'dirt
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit25_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit25.KeyPress
        'planting year
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit24_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit24.KeyPress
        'ffb unit
        e.Handled = NumericOnly(e)
    End Sub
#Region "WB"
    Private Sub UpdateIndicator()
        On Error Resume Next
        Dim WEIGHTDATA As String
        Dim WBTEMP As Integer = 0
        Dim BERAT As String
        ValLenWb = WBLEN
        RLLenWB = WBRL
        If Val(ValLenWb) = 0 Then ValLenWb = 5
        'AMBIL IP & PORT INDIKATOR
        If SimpleButton1.Enabled = False Then
            WEIGHTDATA = GetSCSMessage(WBIP, CInt(WBPORT))
            BERAT = ""
            If RLLenWB = "R" Then
                BERAT = Microsoft.VisualBasic.Right(WEIGHTDATA, ValLenWb)
            Else
                BERAT = Microsoft.VisualBasic.Left(WEIGHTDATA, ValLenWb)
            End If
            If Val(BERAT) > 0 Then
                WBTEMP = WBDisplay
                WBDisplay = Val(BERAT)
                Dim I As Integer
                For I = 0 To 3
                    If WBTEMP = WBDisplay Then
                        SimpleButton6.Enabled = True
                    Else
                        SimpleButton6.Enabled = False
                    End If
                Next
            Else
                WBDisplay = 0
                SimpleButton6.Enabled = False
            End If
            TxtWeight.Text = WBDisplay
        Else
            TxtWeight.Text = 0
        End If
    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'VEHICLE NUMBER
        LSQL = "SELECT distinct VEHICLE_CODE,VEHICLE_TYPE,PLATE_NUMBER,TARE,TRANSPORTER_CODE  " +
            " FROM T_VEHICLE WHERE trim(INACTIVE)='' or trim(INACTIVE) is null "
        LField = "PLATE_NUMBER"
        ValueLoV = ""
        TextEdit4.Text = FrmShowLOV(FrmLoV, LSQL, "PLATE_NUMBER", "VEHICLE")
        VEHICLE_NUMBER = Trim(TextEdit4.Text)
        TextEdit6.Text = GetTara(VEHICLE_NUMBER)
    End Sub

    Private Sub Validasi_Tara()
        If CheckEdit1.Checked = True And Val(TextEdit6.Text) > 0 = True And Val(TextEdit5.Text) > 0 = True Then
            If VEHICLE_MAX_TARA < Val(TextEdit5.Text) = True Or Val(TextEdit5.Text) < VEHICLE_MIN_TARA = True Then
                FrmShowUp(FrmPopBA)
                If BA_DIALOG = True Then
                    VEHICLE_NUMBER = Trim(TextEdit4.Text)
                    BA_DATE = Trim(TextEdit3.Text)
                    WB_ACTUAL = Trim(TextEdit5.Text)
                    FrmShowUp(FrmBeritaAcara)
                Else
                    TextEdit4.Text = ""
                    TextEdit5.Text = 0
                    TextEdit6.Text = 0
                End If
            End If
        End If
    End Sub

    Private Sub FrmWeighbridgeIn_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'WB OFF
        If WBStstus = True Then
            WBStstus = False
            TxtWeight.Text = 0
        End If
    End Sub

    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs) Handles SimpleButton7.Click
        'JNS TIMBANGAN
        Dim CODE As String
        'LSQL = "SELECT SUBSTR(GD_ID,3) GD_ID,GD_DESC  FROM T_LOOKUP_GROUP_DETAIL WHERE GROUP_ID='03'"
        LSQL = "SELECT * FROM T_WB_TYPE WHERE INACTIVE ='N'"
        LField = "WB_TYPE_CODE"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "JENIS ", "Jenis Timbangan")
        TextEdit7.Text = Val(CODE)
    End Sub

    Private Sub SimpleButton8_Click(sender As Object, e As EventArgs) Handles SimpleButton8.Click
        'MATERIAL
        LSQL = "SELECT MATERIAL_CODE,MATERIAL_NAME ,INACTIVE FROM T_MATERIAL   WHERE INACTIVE IS NULL OR INACTIVE='N'"
        LField = "MATERIAL_CODE"
        ValueLoV = ""
        TextEdit8.Text = FrmShowLOV(FrmLoV, LSQL, "MATERIAL", "MATERIAL")
        'open all
        TextEnabled({TextEdit18, TextEdit19, TextEdit20, TextEdit22, TextEdit23, TextEdit24, TextEdit25, TextEdit26, TextEdit27, TextEdit28, TextEdit29})
        'VALIDASI MATERIAL
        Select Case TextEdit8.Text
            Case "501010001" 'ffb/tbs
                'FFA,'MOISTURE,'DIRT,'NO SEGEL
                TextDisebled({TextEdit26, TextEdit27, TextEdit28, TextEdit29})
            Case "501010002" 'CPO
                'LOADER1,'LOADER2,'LOADER3,'NAB,'AFDELING,'BLOCK,'FFB UNIT,'PLATING YEAR
                TextDisebled({TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25})
            Case "501010003" 'PK
                'NAB,'AFDELING,'BLOCK,'FFB UNIT,'PLATING YEAR
                TextDisebled({TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25})
            Case "501010004" 'SHELL
                'NAB,'AFDELING,'BLOCK,'FFB UNIT,'PLATING YEAR
                TextDisebled({TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25})
            Case "501010005" 'FIBER
                'NAB,'AFDELING,'BLOCK,'FFB UNIT,'PLATING YEAR
                TextDisebled({TextEdit18, TextEdit19, TextEdit20, TextEdit21, TextEdit22, TextEdit23, TextEdit24, TextEdit25})
        End Select
    End Sub

    Private Sub SimpleButton9_Click(sender As Object, e As EventArgs) Handles SimpleButton9.Click
        'SUPPLIER/VENDOR
        LSQL = "SELECT VENDOR_CODE,VENDOR_NAME  FROM T_VENDOR WHERE INACTIVE IS NULL OR INACTIVE ='N' "
        LField = "VENDOR_CODE"
        ValueLoV = ""
        TextEdit9.Text = FrmShowLOV(FrmLoV, LSQL, "SUPPLIER", "SUPPLIER/VENDOR TIMBANG")
        TextEdit11.Text = ""
        Select Case Microsoft.VisualBasic.Left(TextEdit9.Text, 4)
            Case "VINT" 'ffb/tbs
                TextEdit10.Enabled = True 'CONTRACT 
        End Select
    End Sub

    Private Sub SimpleButton10_Click(sender As Object, e As EventArgs) Handles SimpleButton10.Click
        'CONTRACT NO
        Dim CODE As String
        LSQL = " SELECT CONTRACT_NO,VENDORID,ITEMNO,MATERIALCODE FROM T_CONTRACT  " +
        " WHERE VENDORID Like '%" & TextEdit3.Text & "%' " +
        " And MATERIALCODE Like '%" & TextEdit7.Text & "%' " +
        " And INACTIVE Is NULL Or INACTIVE ='N'"
        LField = "CONTRACT_NO"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "CONTRACT_NO", "CONTRACT_NO")
        TextEdit10.Text = CODE
    End Sub
    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'CUSTOMER
        LSQL = "SELECT CUST_CODE,CUST_NAME,INACTIVE  FROM T_CUSTOMER WHERE INACTIVE IS NULL OR INACTIVE ='N'"
        LField = "CUST_CODE"
        ValueLoV = ""
        TextEdit11.Text = FrmShowLOV(FrmLoV, LSQL, "CUSTOMER", "CUSTOMER")
        TextEdit9.Text = ""
    End Sub
    Private Sub SimpleButton15_Click(sender As Object, e As EventArgs) Handles SimpleButton15.Click
        'SALES ORDER
        Dim CODE As String
        LSQL = " SELECT SO_NO,SO_QTY,MATERIAL_CODE,SC_NO,SO_START,SO_END FROM T_SALESORDER " +
        " WHERE CUST_CODE Like '%" & TextEdit4.Text & "%'" +
        " And MATERIAL_CODE  Like '%" & TextEdit7.Text & " %'" +
        " And  INACTIVE IS NULL OR INACTIVE='N'"
        LField = "SO_NO"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "SO_NO", "SALES_ORDER")
        TextEdit12.Text = CODE
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'UpdateIndicator()
        If counter >= 10 Then
            ' Exit loop code.  
            'Timer1.Enabled = False
            ' Run your procedure here.  
            UpdateIndicator()  'get data every 10 detik
            counter = 0
        Else

            UpdateIndicator()
            ' Increment counter.  
            counter = counter + 1
            LabelControl36.Text = "Procedures Run: " & counter.ToString
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'VALIDASI WAJIB INPUT
        If Not IsEmptyText({TextEdit8}) = True Then 'MATERIAL,
            'SUPPLIER ,CONTRACT <> VINT,TRST,MILL 
            Dim SPL As String = Microsoft.VisualBasic.Left(TextEdit9.Text, 4)
            If SPL = "VINT" Then
                IsEmptyText({TextEdit9})
                IsEmptyText({TextEdit10})  'CONTRACT <> 
                IsEmptyText({TextEdit21}) 'NAB
                IsEmptyText({TextEdit22}) 'AFDELING
                IsEmptyText({TextEdit23}) 'BLOCK
                IsEmptyText({TextEdit10})
            ElseIf SPL = "TRST" Or SPL = "MILL" Then
                IsEmptyText({TextEdit9})
                IsEmptyText({TextEdit10})  'CONTRACT <> 
            End If
            'MATERIAL
            Dim MTRL As String = TextEdit8.Text
            If MTRL = "5010100001" Or MTRL = "TBS" Then
                'FFB UNIT WAJIB
                IsEmptyText({TextEdit24})
                'PLATING YEAR WAJIB
                IsEmptyText({TextEdit25})
            End If
            'CAPTURE IMAGE DARI CCTV 1 DAN 2
            If Not (PictureEdit1.Image Is Nothing) Then
                Dim varBmp As New Bitmap(PictureEdit1.Image)
                Dim newBitmap As New Bitmap(varBmp)
                varBmp.Dispose()
                varBmp = Nothing
                varBmp.Save("C:\TEMP\CCTV1" & GetTiketNew(WBCode) & ".png", ImageFormat.Png)
            Else
                MessageBox.Show("null exception")
            End If
            If Not (PictureEdit2.Image Is Nothing) Then
                Dim varBmp As New Bitmap(PictureEdit2.Image)
                Dim newBitmap As New Bitmap(varBmp)
                varBmp.Dispose()
                varBmp = Nothing
                varBmp.Save("C:\TEMP\CCTV2" & GetTiketNew(WBCode) & ".png", ImageFormat.Png)
            Else
                MessageBox.Show("null exception")
            End If

            'SIMPAN DATA
            'Convert IMAGE
            Dim imagename As String = "C:\TEMP\CCTV1" & GetTiketNew(WBCode) & ".png" 'lokasi file
            Dim fls As FileStream
            fls = New FileStream(imagename, FileMode.Open, FileAccess.Read)
            Dim blob As Byte() = New Byte(fls.Length - 1) {}
            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length))
            fls.Close()

            Dim blobParameter As New OracleParameter()
            blobParameter.OracleDbType = OracleDbType.Blob
            blobParameter.ParameterName = "BlobParameter1"
            blobParameter.Value = blob

            Dim imagename2 As String = "C:\TEMP\CCTV2" & GetTiketNew(WBCode) & ".png" 'lokasi file
            Dim fls2 As FileStream
            fls2 = New FileStream(imagename2, FileMode.Open, FileAccess.Read)
            Dim blob2 As Byte() = New Byte(fls.Length - 1) {}
            fls2.Read(blob2, 0, System.Convert.ToInt32(fls.Length))
            fls2.Close()
            Dim blobParameter2 As New OracleParameter()
            blobParameter2.OracleDbType = OracleDbType.Blob
            blobParameter2.ParameterName = "BlobParameter2"
            blobParameter2.Value = blob2

            SQL = "INSERT INTO T_WBTICKET (NO_TICKET,PIC_PLATIN,PIC_DRIVEIN) VALUES " +
                "(NO_TICKET," + " :BlobParameter1," + ":BlobParameter2)"
            ExecuteNonQuery(SQL)

        End If
    End Sub

    Private Sub SimpleButton12_Click(sender As Object, e As EventArgs) Handles SimpleButton12.Click
        'DRIVER

    End Sub
#End Region


End Class



