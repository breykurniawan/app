Imports System.Net.Sockets
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions ' Namespace untuk manipulasi registry
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Environment

Imports DevExpress
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Utils

Imports FastReport

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Imports AForge.Video

Module ModuleSWS

    Public SQL As String

    Public USERNAME, ROLEID, ROLENAME As String
    Public bScaleStartOff As Boolean

    'id PT
    Public nPT As String
    Public nAlamat As String
    Public nKota As String
    Public nTelp As String
    Public nRptName As String
    Public GETwEIGHT As Boolean

    Public WEIGHT As String
    Public nNamaSite, nIdSite As String
    Public nCountConect As String
    Public nPortIndicator As Integer

    Public CAMERAON As Boolean
    Public IndikatorON As Boolean
    Public WeightIndicator As String

    Public StKoneksiIndikator As String
    Public TxtIndikator As TextBox


    Public _Listener As TcpListener
    Public _Connections As New List(Of ConnectionInfo)
    Public _ConnectionMontior As Task
    Public nDataBerat As String
    Public net As Double

    Public CompanyCode As String
    Public Company As String
    Public LocationSite As String

    Public MillPlant As String
    Public StoreLocation1 As String
    Public StoreLocation2 As String
    Public ComportSetting As String
    Public WBCode As String
    Public IPCamera1 As String
    Public IPCamera2 As String
    Public IPIndicator As String
    Public LoadingRampTransit As String
    Public SAP As String
    Public AppVersion As String
    Public config As Boolean = False
    Public STonlineUser As Boolean = False

    Public singgeleForm As String
    Public pathimage As String
    Public RLLenWB As String
    Public ValLenWb As String

    'KONEKSI SERVER LOCAL
    Public CONN As New OracleConnection
    Public CMD As New OracleCommand
    Public TRANS As OracleTransaction
    Public DR As OracleDataReader
    Public DA As OracleDataAdapter
    Public DS As DataSet
    Public DT As DataTable

    Public DBSourceLocal As String
    Public DBPortLocal As String
    Public DBNameLocal As String
    Public DBVerLocal As String
    Public DBUserLocal As String
    Public DBPassLocal As String
    Public ConStringLocal As String

    'KONEKSI SERVER STAGING(1) 
    Public CONN1 As New OracleConnection
    Public CMD1 As New OracleCommand
    Public TRANS1 As OracleTransaction
    Public DR1 As OracleDataReader
    Public DA1 As OracleDataAdapter
    Public DS1 As DataSet
    Public DT1 As DataTable

    Public DBSourceStaging As String
    Public DBPortStaging As String
    Public DBNameStaging As String
    Public DBVerStaging As String
    Public DBUserStaging As String
    Public DBPassStaging As String
    Public ConStringStaging As String

    Public TblLogin As String = "Sign In"

    Public blobParameter As New OracleParameter
    'PARAMETER FORM GENERAL
    Public nFormName As String = ""

    'PARAMETER UNTUK LOV
    Public LHeader As String
    Public LSQL As String
    Public LField As String
    Public ValueLoV As String

    'PARAMETER UNTUK EditTiket
    Public nEditTicket As String = ""

    'PARAMETER UNTUK VALIDASI BERITA ACARA
    Public VEHICLE_NUMBER As String
    Public VEHICLE_TARA As Integer
    Public VEHICLE_MAX_TARA As Integer
    Public VEHICLE_MIN_TARA As Integer
    Public WB_ACTUAL As Integer
    Public BA_DATE As String
    Public BA_DIALOG As Boolean = False

    'TIMER WB
    Public WBIP As String
    Public WBPORT As String
    Public WBLEN As String
    Public WBRL As String

    Public WB_ON As Boolean = False

    Delegate Sub SetTextCallback(ByVal [text] As String) 'Added to prevent threading errors during receiveing of data

#Region "General Config"
    Public Sub GetConfig()
        CompanyCode = My.Settings.CompanyCode.ToString
        Company = My.Settings.Company.ToString
        LocationSite = My.Settings.LocationSite.ToString
        MillPlant = My.Settings.Millplant.ToString
        StoreLocation1 = My.Settings.StoreLocation1.ToString
        StoreLocation2 = My.Settings.StoreLocation2.ToString
        ComportSetting = My.Settings.ComportSetting.ToString
        WBCode = My.Settings.WBCode.ToString
        IPCamera1 = My.Settings.IPCamera1.ToString
        IPCamera2 = My.Settings.IPCamera2.ToString
        IPIndicator = My.Settings.IPIndicator.ToString
        LoadingRampTransit = My.Settings.LoadingRampTransit.ToString
        SAP = My.Settings.SAP.ToString
        AppVersion = My.Settings.AppVersion.ToString
        'local seting
        DBSourceLocal = My.Settings.DBSourceLocal.ToString
        DBPortLocal = My.Settings.DBPortLocal.ToString
        DBNameLocal = My.Settings.DBNameLocal.ToString
        DBVerLocal = My.Settings.DBVerLocal.ToString
        DBUserLocal = My.Settings.DBUserLocal.ToString
        DBPassLocal = My.Settings.DBPassLocal.ToString
        'staging
        DBSourceStaging = My.Settings.DBSourceStaging.ToString
        DBPortStaging = My.Settings.DBPortStaging.ToString
        DBNameStaging = My.Settings.DBNameStaging.ToString
        DBVerStaging = My.Settings.DBVerStaging.ToString
        DBUserStaging = My.Settings.DBUserStaging.ToString
        DBPassStaging = My.Settings.DBPassStaging.ToString

        'parameter
        singgeleForm = If(My.Settings.SinggleFrmActive = 1, True, False)
        pathimage = My.Settings.PathImage

    End Sub

#End Region
#Region "FTP"
    Public Sub FtpUploadFile(ByVal filetoupload As String, ByVal ftpuri As String, ByVal ftpusername As String, ByVal ftppassword As String)
        ' Create a web request that will be used to talk with THE server and set THE request method to upload a file by ftp.
        Dim ftpRequest As FtpWebRequest = CType(WebRequest.Create(ftpuri), FtpWebRequest)
        Try
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
            ' Confirm THE Network credentials based on THE user name and password passed in.
            ftpRequest.Credentials = New NetworkCredential(ftpusername, ftppassword)
            ' Read into a Byte array THE contents of THE file to be uploaded 
            Dim bytes() As Byte = System.IO.File.ReadAllBytes(filetoupload)
            ' Transfer THE byte array contents into THE request stream, write and THEn close when done.
            ftpRequest.ContentLength = bytes.Length
            Using UploadStream As Stream = ftpRequest.GetRequestStream()
                UploadStream.Write(bytes, 0, bytes.Length)
                UploadStream.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        MessageBox.Show("Process Complete")
    End Sub
    Private Sub FTPDownloadFile(ByVal downloadpath As String, ByVal ftpuri As String, ByVal ftpusername As String, ByVal ftppassword As String)
        'Create a WebClient.
        Dim request As New WebClient()
        ' Confirm THE Network credentials based on THE user name and password passed in.
        request.Credentials = New NetworkCredential(ftpusername, ftppassword)
        'Read THE file data into a Byte array
        Dim bytes() As Byte = request.DownloadData(ftpuri)
        Try
            '  Create a FileStream to read THE file into
            Dim DownloadStream As FileStream = IO.File.Create(downloadpath)
            '  Stream this data into THE file
            DownloadStream.Write(bytes, 0, bytes.Length)
            '  Close THE FileStream
            DownloadStream.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        MessageBox.Show("Process Complete")
    End Sub
#End Region
#Region "Oracle Koneksi"
    Public Sub GetStagingDbConfig()
        ConStringStaging = "Data Source=(DESCRIPTION=" + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" + "(HOST=" & DBSourceStaging & ")" + "(PORT=" & DBPortStaging & ")))(CONNECT_DATA=(SERVER=DEDICATED)" + "(SERVICE_NAME=" & DBVerStaging & ")));" + "User Id=" & DBUserStaging & ";Password=" & DBPassStaging & ";"
    End Sub
    Public Sub GetLocalDbConfig()
        ConStringLocal = "Data Source=(DESCRIPTION=" + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" + "(HOST=" & DBSourceLocal & ")" + "(PORT=" & DBPortLocal & ")))(CONNECT_DATA=(SERVER=DEDICATED)" + "(SERVICE_NAME=" & DBVerLocal & ")));" + "User Id=" & DBUserLocal & ";Password=" & DBPassLocal & ";"
    End Sub
    Public Function StatusSite() As Boolean
        StatusSite = False
        If SAP = "" Then
            StatusSite = False
        ElseIf SAP = "N" Then
            StatusSite = False
        Else
            StatusSite = True
        End If
        Return StatusSite
    End Function

    Public Function OpenConnLocal() As Boolean
        OpenConnLocal = False
        GetLocalDbConfig()
        Try
            If CONN.State = ConnectionState.Closed Then
                CONN = New OracleConnection(ConStringLocal)
                CONN.Open()
                OpenConnLocal = True
            Else
                OpenConnLocal = True
            End If
        Catch ex As Exception
            CONN.Close()
            OpenConnLocal = False
        End Try
    End Function

    Public Sub CloseConnLocal()
        If CONN.State = ConnectionState.Open Then
            CONN.Close()
        End If
    End Sub

    Public Function OpenConnStaging() As Boolean
        OpenConnStaging = False
        GetStagingDbConfig()
        Try
            CONN1 = New OracleConnection(ConStringStaging)
            If CONN1.State = ConnectionState.Closed Then
                CONN1 = New OracleConnection(ConStringStaging)
                CONN1.Open()
                OpenConnStaging = True
            Else
                OpenConnStaging = True
            End If
        Catch ex As Exception
            CONN1.Close()
            OpenConnStaging = False
        End Try
        Return OpenConnStaging()
    End Function
    Public Sub CloseConnStaging()
        If CONN1.State = ConnectionState.Open Then
            CONN1.Close()
        End If
    End Sub
#End Region
#Region "Tool"
    Public Sub FrmChildShow(frm As Form)
        SplashScreenManager.ShowDefaultWaitForm()
        frm.MdiParent = FrmMain
        frm.Show()
        SplashScreenManager.CloseDefaultWaitForm()
    End Sub
    Public Sub FrmShowUp(frm As Form)
        SplashScreenManager.ShowDefaultWaitForm()
        frm.ShowDialog()
        SplashScreenManager.CloseDefaultWaitForm()
    End Sub
    Public Function FrmShowLOV(frm As Form, LSQL As String, Lfield As String, Header As String) As String
        LHeader = Header
        ValueLoV = ""
        FrmShowLOV = ""
        frm.ShowDialog()
        FrmShowLOV = ValueLoV
        Return FrmShowLOV
    End Function

    Public Function Ping(hostNameOrAddress As String) As Boolean
        On Error Resume Next
        Ping = False
        If My.Computer.Network.Ping(hostNameOrAddress, 1000) Then
            ' MsgBox("Server pinged successfully.")
            Ping = True
        Else
            'MsgBox("Ping request timed out.")
            Ping = False
        End If
        Return Ping
    End Function

    Public Function GetListItem(ByRef lst As ListView, ByVal i As Integer) As String
        On Error Resume Next
        Dim tt As String
        GetListItem = lst.SelectedItems.Item(0).SubItems(i).ToString
        Return GetListItem
    End Function
#End Region
#Region "Olah Data"
    Public Function LogOn() As Boolean
        Dim Time As String = Now
        Dim IP As String = GetIPAddr()
        LogOn = False
        'CHECK LOG
        SQL = "SELECT * FROM TLOGINHISTORY WHERE USERID='" & USERNAME & "' AND USED='Y'"
        If CheckRecord(SQL) > 0 Then
            Dim OnPC As String = GetLastOnline(USERNAME)
            MsgBox("YOU ARE ONLINE ON " & OnPC, vbInformation, "Info Login")
            LogOn = True
        Else
            'INSERT LOG
            SQL = "INSERT INTO TLOGINHISTORY (LOGINDATE,USERID,IPADDRESS,REMARK,USED) " +
              "VALUES('" & Time & "','" & USERNAME & "','" & IP & "','SUCCESS','Y')"
            ExecuteNonQuery(SQL)
            'LogOn = False
        End If
        Return LogOn
    End Function
    Public Sub LogOFF()
        Dim Time As String = Now
        Dim IP As String = GetIPAddr()
        'CHECK LOG
        SQL = "SELECT * FROM TLOGINHISTORY WHERE USERID='" & USERNAME & "' AND USED='Y'"
        If CheckRecord(SQL) > 0 Then
            Dim OnPC As String = GetLastOnline(USERNAME)
            SQL = "UPDATE TLOGINHISTORY SET used='N',LOGOUTDATE='" & Time & "' WHERE used='Y' and userid='" & USERNAME & "' AND IPADDRESS='" & OnPC & "'"
            ExecuteNonQuery(SQL)
        End If
    End Sub
    Public Sub AddaColumn(ByRef ColumnString As String, ByRef LV As ListView)
        Dim NewCH As New ColumnHeader
        NewCH.Text = ColumnString
        LV.Columns.Add(NewCH)
    End Sub

    Public Function GetLsvItem(ByVal nLsv As Object, i As Integer) As String
        Try
            Dim lst As ListView
            lst = CType(nLsv, ListView)
            GetLsvItem = lst.Items(lst.FocusedItem.Index).SubItems(i).Text
        Catch
            GetLsvItem = ""
        End Try
        Return GetLsvItem
    End Function
    Public Function GetLstSEQ(ByVal nLsv As Object) As String
        GetLstSEQ = "0"
        Try
            Dim lst As ListView
            Dim i As Integer
            lst = CType(nLsv, ListView)
            For i = 0 To lst.Items.Count - 1
                GetLstSEQ = CType(CInt(lst.Items(i).Text) = i + 1, String)
            Next
        Catch
            GetLstSEQ = "0"
        End Try
        Return GetLstSEQ
    End Function

    Public Function ExecuteQuery(ByVal Query As String) As DataTable
        ExecuteQuery = Nothing
        If Not OpenConnLocal() Then
            Exit Function
        End If
        CMD = CONN.CreateCommand()
        TRANS = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
        CMD.Transaction = TRANS
        Try
            CMD.CommandText = Query
            CMD.ExecuteReader()
            TRANS.Commit()
            DA = New OracleDataAdapter
            DA.SelectCommand = CMD
            DS = New DataSet
            DA.Fill(DS)
            DT = DS.Tables(0)
            CONN.Close()
            CMD = Nothing
        Catch ex As Exception
            TRANS.Rollback()
            CMD = Nothing
            CONN.Close()
        End Try
        Return DT
    End Function
    Public Function GetLastOnline(ByVal nUsername As String) As String
        GetLastOnline = Nothing
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Dim strsql As String = " SELECT * FROM TLOGINHISTORY WHERE ROWNUM <= 1 And used='Y' and userid='" & nUsername & "' order by logindate desc "
        Dim CONN As New OracleConnection(ConStringLocal)
        CMD = New OracleCommand(strsql, CONN)
        CONN.Open()
        Dim RDR As OracleDataReader = CMD.ExecuteReader
        While RDR.Read
            GetLastOnline = RDR("IPADDRESS").ToString
        End While
        RDR.Close()
        CMD = Nothing
    End Function


    Public Function CheckRecord(ByVal SSQL As String) As Double
        CheckRecord = 0
        Dim i As Integer = 0
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If

            CMD = New OracleCommand(SSQL, CONN)
            Dim Odr As OracleDataReader = CMD.ExecuteReader

            While Odr.Read()
                CheckRecord = CheckRecord + 1
            End While
        Finally
            CMD = Nothing
        End Try
        Return CheckRecord
    End Function

    Public Function NumericOnly(e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        Dim strValid As String = "0123456789"
        If strValid.IndexOf(e.KeyChar) < 0 AndAlso Not (e.KeyChar = Convert.ToChar(Keys.Back)) Then
            ' not valid
            MsgBox("Numeric Only")
            Return True
        Else
            ' valid
            Return False
        End If
    End Function
    Public Function LetterOnly(e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        Dim strValid As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ. "
        If strValid.IndexOf(e.KeyChar) < 0 AndAlso Not (e.KeyChar = Convert.ToChar(Keys.Back)) Then
            ' not valid
            MsgBox("Words Only")
            Return True
        Else
            ' valid
            Return False
        End If
    End Function
    Public Sub ExecuteNonQuery(ByVal query As String)
        If Not OpenConnLocal() Then
            Exit Sub
        End If
        CMD = CONN.CreateCommand
        TRANS = CONN.BeginTransaction(IsolationLevel.ReadCommitted)
        CMD.Transaction = TRANS
        Try
            CMD.CommandText = query
            CMD.ExecuteNonQuery()
            TRANS.Commit()
        Catch e As Exception
            TRANS.Rollback()
        End Try
        CMD = Nothing
        CONN.Close()
    End Sub

    Public Function AmbilTgl() As String
        AmbilTgl = Nothing
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Dim strsql As String = "SELECT SYSDATE TGL FROM DUAL"
        Dim CONN As New OracleConnection(ConStringLocal)
        CMD = New OracleCommand(strsql, CONN)
        CONN.Open()
        Dim RDR As OracleDataReader = CMD.ExecuteReader
        While RDR.Read
            AmbilTgl = RDR("TGL").ToString
        End While
        RDR.Close()
        CMD = Nothing
    End Function

    Public Function GetSite() As String
        GetSite = Nothing
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Dim strsql As String = " SELECT IDSITE , MASTER NAMASITE FROM d_master " +
                                   " WHERE aktif='Y' and IDTABEL='MS'"
        Dim CONN As New OracleConnection(ConStringLocal)
        CMD = New OracleCommand(strsql, CONN)
        CONN.Open()
        Dim RDR As OracleDataReader = CMD.ExecuteReader
        While RDR.Read
            nNamaSite = RDR("NAMASITE").ToString
            nIdSite = RDR("IDSITE").ToString
        End While
        RDR.Close()
        CMD = Nothing
    End Function
    Public Function GetTara(ByVal NOPOL As String) As String
        GetTara = 0
        VEHICLE_TARA = 0
        VEHICLE_MAX_TARA = 0
        VEHICLE_MIN_TARA = 0
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Dim strsql As String = " SELECT DISTINCT PLATE_NUMBER,TARE,MAX_TARA,MIN_TARA FROM V_TOLERANCE_VEHICLE  " +
                               " WHERE trim(PLATE_NUMBER) = '" & NOPOL & "'"
        Dim CONN As New OracleConnection(ConStringLocal)
        CMD = New OracleCommand(strsql, CONN)
        CONN.Open()
        Dim RDR As OracleDataReader = CMD.ExecuteReader
        While RDR.Read
            ' GetTara = RDR("TARE").ToString
            VEHICLE_TARA = Val(RDR("TARE").ToString)
            VEHICLE_MAX_TARA = Val(RDR("MAX_TARA").ToString)
            VEHICLE_MIN_TARA = Val(RDR("MIN_TARA").ToString)
        End While
        RDR.Close()
        CMD = Nothing
        Return GetTara
    End Function

    Public Function CheckLogin(ByVal nUser As String, ByVal nPass As String) As Boolean
        CheckLogin = False
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Dim i As Integer = 0
        Dim queryString As String = "SELECT * FROM T_USERPROFILE WHERE USERNAME='" & nUser & "' and PASSWD ='" & nPass & "' "
        Dim conn As New OracleConnection(ConStringLocal)
        Dim cmd As New OracleCommand(queryString, conn)
        conn.Open()
        Dim DR As OracleDataReader = cmd.ExecuteReader
        Try
            While DR.Read()
                USERNAME = DR("USERNAME").ToString
                ROLEID = DR("ROLEID").ToString
                CheckLogin = True
            End While
            i = i + 1

        Finally
            DR.Close()
            cmd = Nothing
        End Try
        Return CheckLogin
    End Function

    Public Function PeriodeJalan() As String
        Dim dt As New DataTable
        Try
            SQL = "SELECT TO_CHAR(sysdate, 'YYYYMM') periode FROM dual"
            dt = ExecuteQuery(SQL)
            PeriodeJalan = dt.Rows(0).Item("periode").ToString
            dt.Clear()
        Catch
            PeriodeJalan = "190001"
        End Try
        Return PeriodeJalan
    End Function
    Public Function GetMaxTr(ByVal nTabel As String) As String
        Dim date1 As Date = Now
        Dim Th As String = date1.ToString("yyMM")
        Dim Bln As String = date1.ToString("MM")

        SQL = "SELECT max(SUBSTR(trim(NO_TICKET),-11)) MaxTicket FROM " & nTabel & ""
        Dim DTS As New DataTable
        DTS = ExecuteQuery(SQL)
        If DTS.Rows.Count > 0 Then
            GetMaxTr = DTS.Rows(0).Item("MaxTicket").ToString
        Else
            GetMaxTr = Th & "00001"
        End If
        Return GetMaxTr
    End Function
    Public Function GetTiketNew(Ind As String) As String
        Dim sql As String = ""
        Dim Th As String
        Dim Bln As String
        Dim nTiket As String = ""
        Dim date1 As Date = Now
        Dim Code As String
        Dim PERIODE As String = PeriodeJalan()

        Th = date1.ToString("yyyyMM")
        Bln = date1.ToString("MM")

        Th = Left(PERIODE, 4) & Right(PERIODE, 2)
        Bln = Right(PERIODE, 2)

        If Bln = "" Or Th = "" Then
            Th = date1.ToString("yyyyMM")
            Bln = date1.ToString("MM")
        End If

        Code = CStr(GetMaxTr("T_WBTICKET") + 1)

        GetTiketNew = ""

        Try
            If Code = "" Then
                GetTiketNew = Th & Right(Code, 5)
            ElseIf Th > Left(Code, 4) Then
                'Call ResetCode(Ind)
                Code = "000001"
                GetTiketNew = Th & Right(Code, 5)
            ElseIf CInt(Left(Code, 4)) <> 0 Then
                GetTiketNew = Th & Right(Code, 5)
            End If
            GetTiketNew = Ind & GetTiketNew
        Catch ex As Exception
            MsgBox("Error:" & ex.ToString)
        End Try
        Return GetTiketNew
    End Function
    Public Function GetFrmName(nMenuName As String) As String
        GetFrmName = ""
        Dim ssql As String
        If Not OpenConnLocal() Then
            Exit Function
        End If

        ssql = "SELECT FRMNAME  FROM T_ACCESSRIGHTS WHERE ACCESSNAME='" & nMenuName & "'"
        Dim cmd As New OracleCommand(ssql, CONN)
        Dim oRs As OracleDataReader = cmd.ExecuteReader
        Try
            While oRs.Read
                GetFrmName = oRs("frmname").ToString
            End While
        Catch
        End Try
        Return GetFrmName
    End Function
    Public Function GetMaxID(nTable, nField) As String
        GetMaxID = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim ssql As String
            ssql = "SELECT MAX(" & nField & ")MAX FROM " & nTable & " "
            Dim cmd As New OracleCommand(ssql, CONN)
            Dim oRs As OracleDataReader = cmd.ExecuteReader
            While oRs.Read
                GetMaxID = Val(oRs("MAX")) + 1
            End While
            cmd = Nothing
        Catch
            GetMaxID = 1
        End Try
        Return GetMaxID
    End Function

    Public Function GetMaxCode(nTable, nField, nCODE) As String
        GetMaxCode = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim ssql As String
            'cARI KODE
            ssql = "SELECT MAX(" & nField & ")MAX FROM " & nTable & ""
            Dim cmd As New OracleCommand(ssql, CONN)
            Dim oRs As OracleDataReader = cmd.ExecuteReader
            While oRs.Read
                Dim i As Integer = Len(Num(oRs("MAX")))
                Dim TmpCode As String = Right(oRs("MAX"), i)
                TmpCode = Val(TmpCode) + 1
                GetMaxCode = nCODE & Right("00000000" & CInt(TmpCode) + 1, oRs("LEN"))
            End While
            cmd = Nothing
        Catch
            GetMaxCode = 1
        End Try
        Return GetMaxCode
    End Function

    Public Function GetUserMaxID() As String
        GetUserMaxID = ""
        Dim ssql As String
        If Not OpenConnLocal() Then
            Exit Function
        End If
        ssql = "SELECT MAX(USERID)MAX FROM T_USERPROFILE "
        Dim cmd As New OracleCommand(ssql, CONN)
        Dim oRs As OracleDataReader = cmd.ExecuteReader
        Try
            While oRs.Read
                GetUserMaxID = oRs("MAX") + 1
            End While
        Catch
        End Try
        Return GetUserMaxID
    End Function
    Public Function GetMatJenisMaxID() As String
        GetMatJenisMaxID = ""
        Dim ssql As String
        If Not OpenConnLocal() Then
            Exit Function
        End If
        ssql = "SELECT NVL((MAX(MATERIAL_JENIS_CODE)),0)MAX FROM T_MATERIAL_JENIS"
        Dim cmd As New OracleCommand(ssql, CONN)
        Dim oRs As OracleDataReader = cmd.ExecuteReader
        Try
            While oRs.Read
                GetMatJenisMaxID = oRs("MAX") + 1
            End While
        Catch
        End Try
        Return GetMatJenisMaxID
    End Function
    Public Function GetCodeUser() As String
        GetCodeUser = ""
        Dim ssql As String
        If Not OpenConnLocal() Then
            Exit Function
        End If
        ssql = "SELECT MAX(USERID)+1 AS CODE FROM  T_USERPROFILE "

        Dim cmd As New OracleCommand(ssql, CONN)
        Dim oRs As OracleDataReader = cmd.ExecuteReader
        While oRs.Read
            GetCodeUser = CInt(oRs("CODE").ToString)
        End While
        Return GetCodeUser
    End Function
    Public Function GetCode(nKode As String) As String
        GetCode = ""
        Dim ssql As String
        If Not OpenConnLocal() Then
            Exit Function
        End If
        ssql = "SELECT urut FROM  M_CODE " +
                   " WHERE IDTABEL = '" & nKode & "'"
        Dim cmd As New OracleCommand(ssql, CONN)
        Dim oRs As OracleDataReader = cmd.ExecuteReader
        While oRs.Read
            GetCode = nKode & Right("00000000" & CInt(oRs("urut").ToString) + 1, 6)
        End While
        Return GetCode
    End Function

    Public Function GetTime()
        GetTime = ""
        SQL = "SELECT TO_CHAR (SYSDATE, 'MM-DD-YYYY HH24:MI:SS') NOW FROM DUAL"
        ExecuteQuery(SQL)
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            GetTime = DT("now").ToString()
        Catch
        End Try
        Return GetTime
    End Function
    Public Sub UpdateCode(nKode As String)
        If Not OpenConnLocal() Then
            Exit Sub
        End If
        Dim ssql As String
        ssql = "update M_CODE " +
                " set urut =urut+1 " +
                " WHERE IDTABEL = '" & nKode & "'"
        ExecuteNonQuery(ssql)
    End Sub
    Public Function ResetCode(nKode As String) As String
        ResetCode = ""
        Dim ssql As String
        ssql = "update M_CODE" +
                   " set urut =0" +
                   " WHERE IDTABEL = '" & nKode & "'"
        ExecuteNonQuery(ssql)
        CMD = Nothing
        Return ResetCode
    End Function
    Public Sub FILLComboBoxEdit(ByVal sql As String, ByVal index As Integer, ByVal Cbx As DevExpress.XtraEditors.ComboBoxEdit, ByVal editText As Boolean)
        Cbx.Properties.Items.Clear()
        Try
            If Not OpenConnLocal() Then
                Exit Sub
            End If
            If sql = Nothing Then Exit Sub
            CMD = New OracleCommand(sql, CONN)
            CONN.Open()
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            Cbx.Properties.Items.Clear()
            While rdr.Read
                Cbx.Properties.Items.Add(rdr(index).ToString)
            End While
            If editText = False Then Cbx.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            rdr.Close()
        Catch ex As Exception
            MsgBox("Error:" & ex.ToString)
        Finally
            CONN.Close()
        End Try
    End Sub
    Public Sub FILLGridView(ByVal sql As String, ByVal DgView As DevExpress.XtraGrid.GridControl)
        Dim View As GridView = CType(DgView.FocusedView, GridView)
        Try
            If Not OpenConnLocal() Then
                Exit Sub
            End If
            DgView.DataSource = Nothing
            DgView.DataSource = ExecuteQuery(sql)
            View.OptionsView.ColumnAutoWidth = False
            View.OptionsView.BestFitMaxRowCount = -1
            View.BestFitColumns()
            View.OptionsBehavior.Editable = False
        Catch ex As Exception
            MsgBox("Error:" & ex.ToString)
        Finally
            CONN.Close()
        End Try
    End Sub

    Public Sub FillListView(ByVal sqlData As DataTable, ByVal lvList As ListView, ByVal imageID As Integer)
        Dim i As Integer
        Dim j As Integer
        Dim xsize As Integer
        lvList.Clear()
        If sqlData Is Nothing Then Exit Sub
        For i = 0 To sqlData.Columns.Count - 1
            lvList.Columns.Add(sqlData.Columns(i).ColumnName)
        Next i

        For i = 0 To sqlData.Rows.Count - 1
            lvList.Items.Add(sqlData.Rows(i).Item(0).ToString, imageID)
            For j = 1 To sqlData.Columns.Count - 1
                If Not IsDBNull(sqlData.Rows(i).Item(j)) Then
                    lvList.Items(i).SubItems.Add(sqlData.Rows(i).Item(j).ToString)
                Else
                    lvList.Items(i).SubItems.Add("")
                End If
            Next j
        Next i

        For i = 0 To sqlData.Columns.Count - 1
            xsize = CInt(lvList.Width / sqlData.Columns.Count - 8)
            'MsgBox(xsize)
            If xsize > 1440 Then
                lvList.Columns(i).Width = xsize
            Else
                lvList.Columns(i).Width = xsize
            End If
            lvList.Columns(i).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
        Next i
    End Sub

    Public Function GetImgGrp(ByVal nGroup As String) As String
        GetImgGrp = ""
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If
            Dim strsql As String = "SELECT DISTINCT IMGID FROM T_ACCESSRIGHTS  WHERE ACCESSNAME='" & nGroup & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                If Val(rdr("IMGID").ToString) <> 0 Then
                    GetImgGrp = rdr("IMGID").ToString
                Else
                    GetImgGrp = 0
                End If
            End While
            rdr.Close()
            CMD = Nothing
        Catch
        End Try
        Return GetImgGrp
    End Function
    Public Function GetCodeGrp() As String
        GetCodeGrp = ""
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If
            Dim strsql As String = "SELECT CONCAT('000',MAX(ACCESSID)+1) GKODE FROM  T_ACCESSRIGHTS  WHERE  TYPE=0"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                If Val(rdr("GKODE").ToString) <> 0 Then
                    GetCodeGrp = rdr("GKODE").ToString
                    GetCodeGrp = Right(GetCodeGrp, 4)
                End If
            End While
            rdr.Close()
            CMD = Nothing
        Catch
            CMD = Nothing
        End Try
        Return GetCodeGrp
    End Function

    Public Function GetMtJenisCode(ByVal MtType As String) As String
        GetMtJenisCode = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT MATERIAL_JENIS_CODE FROM  T_MATERIAL_JENIS WHERE  MATERIAL_JENIS='" & MtType & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetMtJenisCode = rdr("MATERIAL_JENIS_CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetMtJenisCode
    End Function

    Public Function GetCodeUser(ByVal UNAME As String) As String
        GetCodeUser = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT USERID FROM  T_USERPROFILE WHERE  USERNAME='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                If Val(rdr("USERID").ToString) <> 0 Then
                    GetCodeUser = rdr("USERID").ToString
                End If
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeUser
    End Function

    Public Function GetCodeTrans(ByVal UNAME As String) As String
        GetCodeTrans = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT TRANSPORTER_CODE AS CODE FROM T_TRANSPORTER WHERE TRANSPORTER_NAME='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeTrans = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeTrans
    End Function
    Public Function GetCodeVendor(ByVal UNAME As String) As String
        GetCodeVendor = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT VENDOR_CODE AS CODE FROM T_VENDOR WHERE VENDOR_NAME='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeVendor = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeVendor
    End Function

    Public Function FrmDate(ByVal nDateEdit As DevExpress.XtraEditors.DateEdit, ByVal nFormatdate As String)
        nDateEdit.Properties.Mask.Culture = New System.Globalization.CultureInfo("en-US")
        nDateEdit.Properties.Mask.EditMask = nFormatdate
        'nDateEdit.Properties.Mask.UseMaskAsDisplayFormat = True
        nDateEdit.Properties.CharacterCasing = CharacterCasing.Upper
        FrmDate = "TO_DATE('" & nDateEdit.Text & "','MM/dd/yyyy')"
        Return FrmDate
    End Function

    Public Function GetCodeMaterial(ByVal UNAME As String) As String
        GetCodeMaterial = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT MATERIAL_CODE AS CODE FROM T_MATERIAL WHERE MATERIAL_NAME='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeMaterial = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeMaterial
    End Function
    Public Function GetDeliveryContract(ByVal UNAME As String) As String
        GetDeliveryContract = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT INCOTERMS1 AS CODE ,INCOTERMS2 FROM T_CONTRACT WHERE CONTRACT_NO='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetDeliveryContract = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetDeliveryContract
    End Function
    Public Function GetSimDriver(ByVal UNAME As String, ByVal TRAN As String) As String
        GetSimDriver = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT DRIVER_CODE,DRIVER_NAME,LICENSE_NUMBER AS CODE,TRANSPORTER_CODE FROM T_DRIVER WHERE INACTIVE IS NULL AND DRIVER_NAME LIKE '%" & UNAME & "%' AND TRANSPORTER_CODE '%" & TRAN & "%'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetSimDriver = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetSimDriver
    End Function
    Public Function GetTranspotVehicle(ByVal VNumber As String) As String
        GetTranspotVehicle = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT PLATE_NUMBER,B.TRANSPORTER_CODE,B.TRANSPORTER_NAME  " +
            " FROM T_VEHICLE A " +
            " Left JOIN T_TRANSPORTER  B On A.TRANSPORTER_CODE=B.TRANSPORTER_CODE And B.INACTIVE IS NULL " +
            " WHERE A.INACTIVE IS NULL " +
            " AND A.PLATE_NUMBER='" & VNumber & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetTranspotVehicle = rdr("TRANSPORTER_NAME").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetTranspotVehicle
    End Function
    Public Function GetCodeCust(ByVal UNAME As String) As String
        GetCodeCust = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT CUST_CODE AS CODE FROM T_CUSTOMER WHERE CUST_NAME='" & UNAME & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeCust = rdr("CODE").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeCust
    End Function


    Public Function GetCodeRole(ByVal nRole As String) As String
        GetCodeRole = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT ROLEID FROM  T_ROLE WHERE  ROLENAME='" & nRole & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeRole = rdr("ROLEID").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetCodeRole
    End Function


    Public Function GetParent(ByVal nMenu As String) As String
        GetParent = ""
        If Not OpenConnLocal() Then
            Exit Function
        End If
        Try
            Dim strsql As String = "SELECT PARENTID,ACCESSNAME FROM T_ACCESSRIGHTS WHERE ACCESSID='" & nMenu & "' AND TYPE='1'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetParent = rdr("ACCESSNAME").ToString
            End While
            rdr.Close()
        Catch
        End Try
        CMD = Nothing
        Return GetParent
    End Function


    Public Function GetCodeSub(ByVal parentid As String) As String
        GetCodeSub = ""
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If
            Dim strsql As String = "SELECT CONCAT('000',NVL(MAX(ACCESSID)+1,0)) MKODE FROM  T_ACCESSRIGHTS  WHERE  TYPE=1 AND PARENTID='" & parentid & "'"
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetCodeSub = rdr("MKODE").ToString
                GetCodeSub = Right(GetCodeSub, 4)
                If GetCodeSub = "0000" Then GetCodeSub = Right(parentid, 2) & "01"

            End While
            rdr.Close()
            CMD = Nothing
        Catch
            CMD = Nothing
        End Try
        Return GetCodeSub
    End Function
    Public Function GetParentId(ByVal menuname As String) As String
        GetParentId = ""
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If

            Dim strsql As String = "SELECT ACCESSID,ACCESSNAME,PARENTID FROM T_ACCESSRIGHTS WHERE ACCESSID='" & menuname & "' AND TYPE=1"  'CARI PARENT DARI MENUID
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetParentId = rdr("PARENTID").ToString
            End While
            rdr.Close()
            CMD = Nothing
        Catch
            GetParentId = ""
            CMD = Nothing
        End Try
        Return GetParentId
    End Function


    Public Function GetTipeTrWB(ByVal nTicketNumber As String) As String
        GetTipeTrWB = ""
        Try
            Dim Cst As String = ""
            Dim Spl As String = ""
            Dim JnsT As String = ""
            SQL = "SELECT * FROM T_WBTICKET WHERE NO_TICKET='" & nTicketNumber & "' AND DELETED=0 "
            Dim DtWB As New DataTable
            DtWB = ExecuteQuery(SQL)
            If DtWB.Rows.Count > 0 Then
                Cst = DtWB.Rows(0).Item("customer_code").ToString
                Spl = DtWB.Rows(0).Item("supplier_code").ToString
                JnsT = DtWB.Rows(0).Item("Jns_Timbangan").ToString
                If Cst = "" And Spl <> "" Then
                    GetTipeTrWB = "Penerimaan"
                ElseIf Cst <> "" And Spl = "" Then
                    GetTipeTrWB = "Pengiriman"
                ElseIf Spl <> "" And JnsT = 2 Then
                    GetTipeTrWB = "Intransit"
                ElseIf Spl <> "" And JnsT = 3 Then
                    GetTipeTrWB = "Numpang Timbang"
                End If
            End If
            DtWB.Clear()
        Catch ex As Exception
            GetTipeTrWB = ""
        End Try
        Return GetTipeTrWB
    End Function
    Public Sub InsertLog(ByVal logName As String, ByVal NTicket As String)
        SQL = "INSERT INTO T_WBTICKET_LOG " +
        " (NO_TICKET,CUSTOMER_CODE,SUPPLIER_CODE,TRANSPORTER_CODE,  " +
        " VEHICLE_CODE,WBCODE,DO_SPB,DATE_IN, " +
        " WEIGHT_IN,DATE_OUT,WEIGHT_OUT,NETTO, " +
        " ADJUST_WEIGHT,ADJUST_NETTO,DEL_QTY,MATERIAL_CODE,   " +
        " DRIVER_NAME,EMP_NAME,FFA,MOISTURE, " +
        " DIRT,NO_SEGEL,SIM,PIC_PLATIN,  " +
        " PIC_DRIVERIN,TIMECAPTUREIN,PIC_PLAT_OUT,PIC_DRIVEROUT,  " +
        " TIMECAPTUREOUT,NO_NOKI,DELIVERY_TYPE,JNS_TIMBANGAN,  " +
        " REMARKS,INPUT_BY,INPUT_DATE,UPDATE_BY,  " +
        " UPDATE_DATE,STATUS,TAHUN_TANAM,ESTATE,  " +
        " AFDELING,BLOCK,FFB_UNITS,CUSTOMER_RECEIVE, " +
        " DELETED,VERIFIED,VERIFIED_DATE,CUSTOMER_RETURN,REF_TICKETNO,TICKET_CLOSED,  " +
        " NO_TICKET_TAP,CUSTOMERID_DUP,EXPORTED,KIT) " +
        " SELECT  " +
        " NO_TICKET,CUSTOMER_CODE,SUPPLIER_CODE,TRANSPORTER_CODE,  " +
        " VEHICLE_CODE,WBCODE,DO_SPB,DATE_IN, " +
        " WEIGHT_IN,DATE_OUT,WEIGHT_OUT,NETTO, " +
        " ADJUST_WEIGHT,ADJUST_NETTO,DEL_QTY,MATERIAL_CODE,   " +
        " DRIVER_NAME,EMP_NAME,FFA,MOISTURE, " +
        " DIRT,NO_SEGEL,SIM,PIC_PLATIN,  " +
        " PIC_DRIVERIN,TIMECAPTUREIN,PIC_PLAT_OUT,PIC_DRIVEROUT,  " +
        " TIMECAPTUREOUT,NO_NOKI,DELIVERY_TYPE,JNS_TIMBANGAN,  " +
        " REMARKS,INPUT_BY,INPUT_DATE,UPDATE_BY,  " +
        " UPDATE_DATE,STATUS,TAHUN_TANAM,ESTATE,  " +
        " AFDELING,BLOCK,FFB_UNITS,CUSTOMER_RECEIVE, " +
        " DELETED,VERIFIED,VERIFIED_DATE,CUSTOMER_RETURN,REF_TICKETNO,TICKET_CLOSED,  " +
        " NO_TICKET_TAP,CUSTOMERID_DUP,EXPORTED,KIT " +
        " FROM T_WBTICKET " +
        " WHERE NO_TICKET ='" & NTicket & "'"
        ExecuteNonQuery(SQL)
        'UPDATE NAMA LOG DAN USER DATE
        SQL = "UPDATE T_WBTICKET SET LOG_NAME='" & logName & "',LOG_BY='" & USERNAME & "',LOG_DATE=SYSDATE WHERE NO_TICKET='" & NTicket & "'"
        ExecuteNonQuery(SQL)
    End Sub
    Public Function GetMenuId(ByVal menuname As String) As String
        GetMenuId = ""
        Try
            If Not OpenConnLocal() Then
                Exit Function
            End If

            Dim strsql As String = "SELECT ACCESSID,ACCESSNAME,PARENTID FROM T_ACCESSRIGHTS WHERE ACCESSNAME='" & menuname & "' AND TYPE=0"  'CARI MENUID DARI NAMA (GROUP ONLY)
            CMD = New OracleCommand(strsql, CONN)
            Dim rdr As OracleDataReader = CMD.ExecuteReader
            While rdr.Read
                GetMenuId = rdr("ACCESSID").ToString
            End While
            rdr.Close()
            CMD = Nothing
        Catch
            GetMenuId = ""
            CMD = Nothing
        End Try
        Return GetMenuId
    End Function

    Public Function GetIPAddr()
        Dim h As System.Net.IPHostEntry = Nothing
        GetIPAddr = "127.0.0.1"
        Try
            h = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName)
            GetIPAddr = h.AddressList.GetValue(4).ToString
        Catch
        End Try
        Return GetIPAddr
    End Function

    '-------------------------Fast Report -------Start
    Public Sub ShowReport(ByRef nRpt As String, ByRef queryString As String, ByRef nTabelRpt As String)
        Dim Rpt As New Report
        Dim Fdss As New DataSet
        Dim Cmd As New OracleCommand
        Dim Da As New OracleDataAdapter

        Dim conn As OracleConnection = New OracleConnection(ConStringLocal)
        conn.ConnectionString = ConStringLocal
        conn.Open()

        With Cmd
            .Connection = conn
            .CommandText = queryString
            .CommandType = CommandType.Text
        End With
        Da.SelectCommand = Cmd
        'fill data to dataset
        Da.Fill(Fdss, nTabelRpt)
        ' register THE dataset
        Rpt.RegisterData(Fdss)
        ' load THE existing report
        Rpt.Load("..\..\" & nRpt & ".frx")
        'registe parameter
        GetConfig()
        Rpt.SetParameterValue("P1", nPT)
        Rpt.SetParameterValue("P2", nAlamat)
        Rpt.SetParameterValue("P3", nKota)
        Rpt.SetParameterValue("P4", nTelp)
        Rpt.SetParameterValue("P5", USERNAME)
        ' register THE dataset
        Rpt.RegisterData(Fdss)
        ' run THE report
        Rpt.Show()
        ' free resources used by report
        Rpt.Dispose()
        Da = Nothing
        conn.Close()
        conn = Nothing
    End Sub
    '-------------------------Fast Report -------End

    '-------------------------Check COMBO dan TEXT-------Start
    Public Function IsEmptyText(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            If Not (objText(i).Text.Length > 0) Then ' validas inputkan text, klo enggak diisi tampilkan peringatan
                MessageBox.Show("SORRY THE TEXT SHOULD BE FILLED", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                objText(i).Focus()
                ' objText(i).BackColor = Color.YellowGreen
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function IsZeroText(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer

        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            If Not Val(objText(i).Text) = 0 Then ' validas inputkan text, klo enggak diisi tampilkan peringatan
                MessageBox.Show("SORRY THE TEXT SHOULD BE ZEROO", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                objText(i).Focus()
                ' objText(i).BackColor = Color.YellowGreen
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function
    Public Function IsNumericOnly(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            If Not IsNumeric(objText(i).EditValue) = True Then ' validas inputkan text, klo enggak diisi tampilkan peringatan
                MessageBox.Show("SORRY THE TEXT SHOULD BE NUMERICE ONLY", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                objText(i).Focus()
                ' objText(i).BackColor = Color.YellowGreen
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function

    Public Function CheckRedonly(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            If Not (objText(i).Properties.ReadOnly = True) Then ' validas text Jika Readonly, warna abu2          
                'objText(i).BackColor = Color.Silver
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function

    Public Function TextRedonly(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            objText(i).Properties.ReadOnly = True ' Ubah Jadi readonly
            objText(i).BackColor = Color.LightGray
        Next
        Return False
    End Function
    Public Function TextDisebled(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            objText(i).BackColor = Color.LightGray
            objText(i).Enabled = False ' Ubah Jadi DISEBLE
        Next
        Return False
    End Function

    Public Function TextEnebled(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            objText(i).BackColor = Color.White
            objText(i).Enabled = True ' Ubah Jadi ENEBLE
        Next
        Return False
    End Function
    Public Function TextClear(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            objText(i).Enabled = True ' Ubah Jadi Enabled
            objText(i).Text = ""
            objText(i).ResetBackColor()
        Next
        Return False
    End Function

    Public Function TextEnabled(ByVal objText() As DevExpress.XtraEditors.TextEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            objText(i).Enabled = True ' Ubah Jadi Enabled
            objText(i).ResetBackColor()
        Next
        Return False
    End Function
    Public Function IsEmptyCombo(ByVal objText() As XtraEditors.ComboBoxEdit) As Boolean
        Dim i As Integer
        For i = 0 To objText.GetUpperBound(0) ' lakukan perulangan sebanyak array objek
            If Not (objText(i).Text.Length > 0) Then ' validas inputkan text, klo enggak diisi tampilkan peringatan
                MessageBox.Show("SORRY THE COMBO SHOULD BE FILLED", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                objText(i).Focus()
                Return True
                Exit Function
            End If
        Next

        Return False
    End Function
    '-------------------------Check COMBO dan TEXT-------END
#End Region
#Region "SCALE WEIGHT"
    Public Sub UpdateConnectionCountLabel()
        nCountConect = String.Format("{0} Connections", _Connections.Count)
    End Sub
    Public Function Num(value As String) As String
        'Dim rgx As New Regex("[^a-zA-Z ]")  'replace angka
        Dim rgx As New Regex("[^\d%@!() ?~]")
        Dim wordy As String = rgx.Replace(value, "")
        Num = wordy
    End Function
    Public Function GetSCSMessage(ByVal IP As String, ByVal Port As Int32) As String
        Dim tcpClient As New System.Net.Sockets.TcpClient()
        Try
            tcpClient.Connect(IP, Port)
            tcpClient.NoDelay = True
            Dim networkStream As NetworkStream = tcpClient.GetStream()
            If networkStream.CanWrite And networkStream.CanRead Then
                ' Do a simple write.
                Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes("$gimme")
                networkStream.Write(sendBytes, 0, sendBytes.Length)
                ' Read THE NetworkStream into a byte buffer.
                Dim bytes(tcpClient.ReceiveBufferSize) As Byte
                networkStream.Read(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
                ' Output THE data received FROM THE host to THE console.
                Dim ReturnData As String = Encoding.ASCII.GetString(bytes)
                If InStr(ReturnData, "Socket Server") > 0 Then
                    ' Rewrite output to server to get real data
                    networkStream.Write(sendBytes, 0, sendBytes.Length)
                    Dim newbytes(tcpClient.ReceiveBufferSize) As Byte
                    networkStream.Read(newbytes, 0, CInt(tcpClient.ReceiveBufferSize))
                    ReturnData = Encoding.ASCII.GetString(newbytes)
                End If
                Dim i As Integer = InStr(ReturnData, Chr(0), CompareMethod.Binary)
                ReturnData = Microsoft.VisualBasic.Left(ReturnData, i - 1)
                GetSCSMessage = Microsoft.VisualBasic.Left(CType(Num(ReturnData), String), WBLEN)
                StKoneksiIndikator = "WB ON"
                tcpClient.Close()
            Else
                If Not networkStream.CanRead Then
                    StKoneksiIndikator = "Link : Off"
                    tcpClient.Close()
                Else
                    If Not networkStream.CanWrite Then
                        StKoneksiIndikator = "Link : On"
                        tcpClient.Close()
                    End If
                End If
                GetSCSMessage = ""
            End If
        Catch ex As Exception
            GetSCSMessage = ""
            StKoneksiIndikator = "WB OFF"
        End Try
        Return GetSCSMessage
    End Function
#End Region
    Public Function GetWBConfig() As String

        GetWBConfig = ""
        'configurasi wb yang di pakai
        Try
            If WBPORT = "" Then WBPORT = "8080"
            If WBIP = "" Then WBIP = "127.0.0.1"
            SQL = "SELECT A.WBCODE ,B.IPADDRESS,B.PORT,B.LEN,B.RL " +
            " FROM T_CONFIG A " +
            " LEFT JOIN T_WB B ON B.NAMA=A.WBCODE " +
            " WHERE A.COMPANYCODE='" & CompanyCode & "'"
            Dim dts As New DataTable
            dts = ExecuteQuery(SQL)
            If dts.Rows.Count > 0 Then
                WBIP = dts.Rows(0).Item("IPADDRESS").ToString
                WBPORT = dts.Rows(0).Item("PORT").ToString
                WBLEN = dts.Rows(0).Item("LEN").ToString
                WBRL = dts.Rows(0).Item("RL").ToString
            End If
            GetWBConfig = "WB Setting. " & WBCode & "| IP." & WBIP & "| PORT." & WBPORT
        Catch
            GetWBConfig = "Not Found Setting ...!!!"
        End Try
        Return GetWBConfig
    End Function
    Public Function GetCCTVParam(ByVal ncctv As String) As String
        GetCCTVParam = ""
        Try
            Dim dt As New DataTable
            SQL = "SELECT CONFIG FROM T_CCTV WHERE NAMA='" & ncctv & "'"
            dt = ExecuteQuery(SQL)
            If dt.Rows.Count > 0 Then GetCCTVParam = dt.Rows(0).Item("config").ToString()
        Catch ex As Exception
            DT.Dispose()
        End Try
        DT.Clear()
        Return GetCCTVParam
    End Function
End Module

'Provides a simple container object to be used for THE state object passed to thIFe connection monitoring thread
Public Class MonitorInfo
    Public Property Cancel As Boolean
    Private _Connections As List(Of ConnectionInfo)
    Public ReadOnly Property Connections As List(Of ConnectionInfo)
        Get
            Return _Connections
        End Get
    End Property

    Private _Listener As TcpListener
    Public ReadOnly Property Listener As TcpListener
        Get
            Return _Listener
        End Get
    End Property

    Public Sub New(tcpListener As TcpListener, connectionInfoList As List(Of ConnectionInfo))
        _Listener = tcpListener
        _Connections = connectionInfoList
    End Sub
End Class

'Provides a container object to serve as THE state object for async client and stream operations
Public Class ConnectionInfo
    'hold a reference to entire monitor instead of just THE listener
    Private _Monitor As MonitorInfo
    Public ReadOnly Property Monitor As MonitorInfo
        Get
            Return _Monitor
        End Get
    End Property

    Private _Client As TcpClient
    Public ReadOnly Property Client As TcpClient
        Get
            Return _Client
        End Get
    End Property

    Private _Stream As NetworkStream
    Public ReadOnly Property Stream As NetworkStream
        Get
            Return _Stream
        End Get
    End Property

    Private _DataQueue As System.Collections.Concurrent.ConcurrentQueue(Of Byte)
    Public ReadOnly Property DataQueue As System.Collections.Concurrent.ConcurrentQueue(Of Byte)
        Get
            Return _DataQueue
        End Get
    End Property

    Private _LastReadLength As Integer
    Public ReadOnly Property LastReadLength As Integer
        Get
            Return _LastReadLength
        End Get
    End Property

    Private _Buffer(63) As Byte

    Public Sub New(monitor As MonitorInfo)
        _Monitor = monitor
        _DataQueue = New System.Collections.Concurrent.ConcurrentQueue(Of Byte)
    End Sub

    Public Sub AcceptClient(result As IAsyncResult)
        _Client = _Monitor.Listener.EndAcceptTcpClient(result)
        If _Client IsNot Nothing AndAlso _Client.Connected Then
            _Stream = _Client.GetStream
        End If
    End Sub

    Public Sub AwaitData()
        _Stream.BeginRead(_Buffer, 0, _Buffer.Length, AddressOf DoReadData, Me)
    End Sub

    Private Sub DoReadData(result As IAsyncResult)
        Dim info As ConnectionInfo = CType(result.AsyncState, ConnectionInfo)
        Try
            If info.Stream IsNot Nothing AndAlso info.Stream.CanRead Then
                info._LastReadLength = info.Stream.EndRead(result)

                For index As Integer = 0 To _LastReadLength - 1
                    info._DataQueue.Enqueue(info._Buffer(index))
                Next
                info.SendMessage("Received " & info._LastReadLength & " Bytes")
                For Each oTHErInfo As ConnectionInfo In info.Monitor.Connections
                    If Not oTHErInfo Is info Then
                        oTHErInfo.SendMessage(System.Text.Encoding.ASCII.GetString(info._Buffer))
                    End If
                Next
                info.AwaitData()
            Else
                info.Client.Close()
            End If
        Catch ex As Exception
            info._LastReadLength = -1
        End Try
    End Sub

    Private Sub SendMessage(message As String)
        If _Stream IsNot Nothing Then
            Dim messageData() As Byte = System.Text.Encoding.ASCII.GetBytes(message)
            Stream.Write(messageData, 0, messageData.Length)
        End If
    End Sub
End Class

Public Class SampleCallEveryXMinute

    Private WithEvents xTimer As New System.Windows.Forms.Timer
    Dim wbip As String
    Dim wbport As String
    Public Sub New(TickValue As Integer)
        xTimer = New System.Windows.Forms.Timer
        xTimer.Interval = TickValue
    End Sub

    Public Sub StartTimer()
        xTimer.Start()
    End Sub

    Public Sub StopTimer()
        xTimer.Stop()
    End Sub

    Private Sub Timer_Tick() Handles xTimer.Tick
    End Sub
End Class