Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading.Tasks

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmVehicle
    Dim WBIP As String
    Dim WBPORT As String
    Dim WBLEN As String
    Dim WBRL As String
    Dim WBDisplay As Integer
    Dim WBStstus As Boolean = False

    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        ComboBoxEdit1.Enabled = True

        TextEdit3.Enabled = True
        TextEdit4.Enabled = True
        TextEdit5.Enabled = True
        ComboBoxEdit2.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete
    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        ComboBoxEdit1.Enabled = False
        TextEdit3.Enabled = False
        TextEdit4.Enabled = False
        TextEdit5.Enabled = False
        ComboBoxEdit2.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub CLearInputVC()
        TextEdit1.Text = ""
        ComboBoxEdit1.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = "0"
        TextEdit5.Text = ""
        ComboBoxEdit2.Text = ""

        SimpleButton1.Enabled = True
        SimpleButton2.Enabled = False
        SimpleButton3.Enabled = False

    End Sub
    Private Sub LoadView()
        SQL = "SELECT A.VEHICLE_CODE,A.VEHICLE_TYPE,A.PLATE_NUMBER,A.TARE,A.OWNERSHIP,B.TRANSPORTER_NAME,A.STATUS  " +
            " FROM T_VEHICLE A " +
            " LEFT JOIN T_TRANSPORTER B ON B.TRANSPORTER_CODE=A.TRANSPORTER_CODE " +
            " WHERE A.INACTIVE <> 'X' " +
            " ORDER BY VEHICLE_CODE"
        GridControl4.DataSource = Nothing
        FILLGridView(SQL, GridControl4)
    End Sub
    Private Sub GridHeader()

        Dim view As ColumnView = CType(GridControl4.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"VEHICLE_CODE", "VEHICLE_TYPE", "PLATE_NUMBER", "TARE", "OWNERSHIP", "TRANSPORTER_NAME", "STATUS"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING
        Dim GridView As GridView = CType(GridControl4.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("TRANSPORTER_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("VEHICLE_CODE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.ExpandAllGroups()

    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE VEHICLE
        If Not IsEmptyText({TextEdit1, TextEdit3, TextEdit4, TextEdit5}) = True Then
            If Not IsEmptyCombo({ComboBoxEdit1, ComboBoxEdit2}) = True Then
                Dim VEHICLECODE As String = TextEdit1.Text
                Dim VEHICLETYPE As String = ComboBoxEdit1.Text
                Dim PLATENUMBER As String = TextEdit3.Text
                Dim TARRA As String = TextEdit4.Text
                Dim OWNERSHIP As String = TextEdit5.Text
                Dim TRANSPORTERCODE As String = GetCodeTrans(ComboBoxEdit2.Text)
                Dim STATUS As String = ""
                SQL = " SELECT * FROM T_VEHICLE WHERE VEHICLE_CODE= '" & VEHICLECODE & "'"
                If CheckRecord(SQL) = 0 Then
                    SQL = "INSERT INTO T_VEHICLE (VEHICLE_CODE,VEHICLE_TYPE,PLATE_NUMBER,TARE,OWNERSHIP,TRANSPORTER_CODE,STATUS)" +
                    "VALUES ('" & VEHICLECODE & "','" & VEHICLETYPE & "','" & PLATENUMBER & "','" & TARRA & "','" & OWNERSHIP & "','" & TRANSPORTERCODE & "','" & STATUS & "')"
                    ExecuteNonQuery(SQL)

                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "VEHICLE")
                    UnlockAll()
                    CLearInputVC()
                Else
                    SQL = " SELECT * FROM T_VEHICLE WHERE VEHICLE_CODE= '" & VEHICLECODE & "' AND INACTIVE='X'"
                    If CheckRecord(SQL) > 0 Then
                        SQL = "UPDATE T_VEHICLE SET VEHICLE_TYPE='" & VEHICLETYPE & "',PLATE_NUMBER='" & PLATENUMBER & "',OWNERSHIP='" & OWNERSHIP & "',TRANSPORTER_CODE='" & TRANSPORTERCODE & "',STATUS='" & STATUS & "'" +
                              ",INACTIVE='N' " +
                              " WHERE VEHICLE_CODE= '" & TextEdit1.Text & "' AND INACTIVE='X'"
                        ExecuteNonQuery(SQL)
                        LoadView()
                        MsgBox("SAVE SUCCESSFUL", vbInformation, "VEHICLE")
                        UnlockAll()
                        CLearInputVC()
                    End If

                End If
                If UCase(SimpleButton2.Text) = "UPDATE" Then
                    SQL = "UPDATE T_VEHICLE SET VEHICLE_TYPE='" & VEHICLETYPE & "',PLATE_NUMBER='" & PLATENUMBER & "',OWNERSHIP='" & OWNERSHIP & "',TRANSPORTER_CODE='" & TRANSPORTERCODE & "',STATUS='" & STATUS & "'" +
                    " WHERE VEHICLE_CODE= '" & TextEdit1.Text & "'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("UPDATE SUCCESSFUL", vbInformation, "VEHICLE")
                    UnlockAll()
                    CLearInputVC()
                End If
            End If
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'delete
        SQL = "UPDATE T_VEHICLE SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE VEHICLE_CODE='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        CLearInputVC()
        LoadView()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "VEHICLE")
    End Sub
    Private Sub FrmVehicle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "VEHICLE"
        GridHeader()
        LoadVType()
        LoadTransporter()
        LoadView()
        LockAll()
        GetWB()
    End Sub
    Private Sub LoadVType()
        SQL = "SELECT VEHICLE_TYPE FROM T_VEHICLE_TYPE WHERE INACTIVE<>'X' "
        FILLComboBoxEdit(SQL, 0, ComboBoxEdit1, False)
    End Sub
    Private Sub LoadTransporter()
        SQL = "SELECT TRANSPORTER_NAME FROM T_TRANSPORTER WHERE INACTIVE<>'x' "
        FILLComboBoxEdit(SQL, 0, ComboBoxEdit2, False)
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'add
        CLearInputVC()
        UnlockAll()
        TextEdit1.Select()

        ''Nyalakan Koneksi ke Timbangan
        LabelControl2.Text = GetWBConfig()
        If WBStstus = False Then
            WBStstus = True
            OnOffWb()
            SimpleButton6.Enabled = True
        End If

    End Sub
    Private Function GetWBConfig() As String
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
            GetWBConfig = "WBCODE. " & WBCode & "| IP." & WBIP & "| PORT." & WBPORT
        Catch
            GetWBConfig = "Not Found WB Config ...!!!"
        End Try
        Return GetWBConfig
    End Function

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        LockAll()
        CLearInputVC()
        SimpleButton2.Text = "Save" 'SAVE
        If WBStstus = True Then
            WBStstus = False
            OnOffWb()
            TxtWeight.Text = 0
            SimpleButton6.Enabled = False
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'close
        'Matikan Timbangan
        If WBStstus = True Then
            WBStstus = False
            OnOffWb()
            TxtWeight.Text = 0
        End If
        Close()
    End Sub
    Private Sub GridView4_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView4.RowCellClick
        If e.RowHandle < 0 Then
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView4.GetRowCellValue(e.RowHandle, "VEHICLE_CODE").ToString() 'VEHICLECODE
            ComboBoxEdit1.Text = GridView4.GetRowCellValue(e.RowHandle, "VEHICLE_TYPE").ToString() 'VEHICLETYPE
            TextEdit3.Text = GridView4.GetRowCellValue(e.RowHandle, "PLATE_NUMBER").ToString() 'PLATENUMBER
            TextEdit4.Text = GridView4.GetRowCellValue(e.RowHandle, "TARE").ToString()  'TARRE
            TextEdit5.Text = GridView4.GetRowCellValue(e.RowHandle, "OWNERSHIP").ToString() 'OWNERSHIP
            ComboBoxEdit2.Text = GridView4.GetRowCellValue(e.RowHandle, "TRANSPORTER_NAME").ToString() 'INACTIVE
            UnlockAll()
            TextEdit1.Enabled = False
            SimpleButton6.Enabled = False
        End If
    End Sub

    Private Sub FrmVehicle_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 7)
    End Sub

    Private Sub GetWB()
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
        LabelControl2.Text = "WBCODE. " & WBCode & "| IP." & WBIP & "| PORT." & WBPORT
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        Dim i As Integer
        If Val(TxtWeight.Text) > 0 Then
            For i = 1 To 10   'CHEK 10X 
                If i = 10 Then TextEdit4.Text = Val(TxtWeight.Text)
            Next
        End If
    End Sub

    Private Sub OnOffWb()
        'INDIKATOR SWITCH
        nPortIndicator = WBPORT
        If nPortIndicator = 0 Then
            MsgBox("THE IP Indicator port Is Not loaded yet")
            Exit Sub
        End If

        If WBStstus = True Then
            Timer1.Enabled = True
            WBDisplay = 0
            _Listener = New TcpListener(IPAddress.Any, 3002)
            _Listener.Start()
            Dim monitor As New MonitorInfo(_Listener, _Connections)
            ListenForClient(monitor)
            _ConnectionMontior = Task.Factory.StartNew(AddressOf DoMonitorConnections, monitor, TaskCreationOptions.LongRunning)
        ElseIf WBStstus = False Then
            Timer1.Enabled = False
            IndikatorON = False
            WBDisplay = 0
            CType(_ConnectionMontior.AsyncState, MonitorInfo).Cancel = True
            _Listener.Stop()
            _Listener = Nothing
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim WEIGHTDATA As String
        Dim BERAT As String
        ValLenWb = WBLEN
        RLLenWB = WBRL
        If Val(ValLenWb) = 0 Then ValLenWb = 5
        'AMBIL IP & PORT INDIKATOR
        WEIGHTDATA = GetSCSMessage(WBIP, CInt(WBPORT))
        BERAT = ""
        If RLLenWB = "R" Then
            BERAT = Microsoft.VisualBasic.Right(WEIGHTDATA, ValLenWb)
        Else
            BERAT = Microsoft.VisualBasic.Left(WEIGHTDATA, ValLenWb)
        End If
        If Val(BERAT) > 0 Then
            TxtWeight.Text = Val(BERAT)
        Else
            TxtWeight.Text = 0
        End If
    End Sub


#Region "WB"
    Private Sub AppendOutput(message As String) ''cetak hasil baca data
        If TxtIndikator.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf AppendOutput)
            Invoke(x, New Object() {(Text)})
        Else
            TxtIndikator.Text = CType(Num(message), String)
            If GETwEIGHT = True Then
                WEIGHT = TxtIndikator.Text
            End If
        End If
    End Sub
    Private Sub DoAcceptClient(result As IAsyncResult)
        Dim monitorInfo As MonitorInfo = CType(_ConnectionMontior.AsyncState, MonitorInfo)
        If monitorInfo.Listener IsNot Nothing AndAlso Not monitorInfo.Cancel Then
            Dim info As ConnectionInfo = CType(result.AsyncState, ConnectionInfo)
            monitorInfo.Connections.Add(info)
            info.AcceptClient(result)
            ListenForClient(monitorInfo)
            info.AwaitData()
            Dim doUpdateConnectionCountLabel As New Action(AddressOf UpdateConnectionCountLabel)
            Invoke(doUpdateConnectionCountLabel)
        End If
    End Sub
    Private Sub DoMonitorConnections()
        'Create delegate for updating output display
        Dim doAppendOutput As New Action(Of String)(AddressOf AppendOutput)
        'Create delegate for updating connection count label
        Dim doUpdateConnectionCountLabel As New Action(AddressOf UpdateConnectionCountLabel)

        'Get MonitorInfo instance FROM thread-save Task instance
        Dim monitorInfo As MonitorInfo = CType(_ConnectionMontior.AsyncState, MonitorInfo)
        'Report progress
        'Implement client connection processing loop
        Do
            'Create temporary list for recording closed connections
            Dim lostCount As Integer = 0
            'Examine each connection for processing
            For index As Integer = monitorInfo.Connections.Count - 1 To 0 Step -1
                Dim info As ConnectionInfo = monitorInfo.Connections(index)
                If info.Client.Connected Then
                    'Process connected client
                    If info.DataQueue.Count > 0 Then
                        'THE code in this If-Block should be modified to build 'message' objects
                        'according to THE protocol you defined for your data transmissions.
                        'This example simply sends all pending message bytes to THE output textbox.
                        'Without a protocol we cannot know what constitutes a complete message, so
                        'with multiple active clients we could see part of client1's first message,
                        'THEn part of a message FROM client2, followed by THE rest of client1's
                        'first message (assuming client1 sent more than 64 bytes).
                        Dim messageBytes As New List(Of Byte)
                        While info.DataQueue.Count > 0
                            Dim value As Byte
                            If info.DataQueue.TryDequeue(value) Then
                                messageBytes.Add(value)
                            End If
                        End While
                        Invoke(doAppendOutput, System.Text.Encoding.ASCII.GetString(messageBytes.ToArray))
                        ' cacah(System.Text.Encoding.ASCII.GetString(messageBytes.ToArray))
                    End If
                Else
                    'Clean-up any closed client connections
                    monitorInfo.Connections.Remove(info)
                    lostCount += 1
                End If
            Next
            If lostCount > 0 Then
                Invoke(doUpdateConnectionCountLabel)
            End If
            'Throttle loop to avoid wasting CPU time
            _ConnectionMontior.Wait(1)
        Loop While Not monitorInfo.Cancel
        'Close all connections before exiting monitor
        For Each info As ConnectionInfo In monitorInfo.Connections
            info.Client.Close()
        Next
        monitorInfo.Connections.Clear()
        'Update THE connection count label and report status
        Invoke(doUpdateConnectionCountLabel)
        'Me.Invoke(doAppendOutput, "Monitor Stopped.")
    End Sub
    Private Sub ListenForClient(monitor As MonitorInfo)
        Dim info As New ConnectionInfo(monitor)
        _Listener.BeginAcceptTcpClient(AddressOf DoAcceptClient, info)
    End Sub

    Private Sub FrmVehicle_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'Matikan Timbangan
        If WBStstus = True Then
            WBStstus = False
            OnOffWb()
            TxtWeight.Text = 0
        End If
    End Sub
    Private Sub TextEdit4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit4.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
#End Region
End Class