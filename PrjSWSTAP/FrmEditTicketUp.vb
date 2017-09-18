Public Class FrmEditTicketUp
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Close()
    End Sub

    Private Sub FrmEditTicketUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If nEditTicket <> "" THEn
            TextEdit1.Text = nEditTicket
            TextEdit2.Text = GetTipeTrWB(nEditTicket)

            Validasi_input()
            ISI_DATA()
        End If
    End Sub


    Private Sub ISI_DATA()
        'LOAD DATA
        SQL = "SELECT * FROM T_WBTICKET WHERE NO_TICKET='" & TextEdit1.Text & "'"
        Dim DtWB As New DataTable
        DtWB = ExecuteQuery(SQL)
        If DtWB.Rows.Count > 0 THEn
            TextEdit3.Text = DtWB.Rows(0).Item("SUPPLIER_CODE").ToString 'VENDOR
            TextEdit4.Text = DtWB.Rows(0).Item("CUSTOMER_CODE").ToString 'CUSTOMER
            TextEdit5.Text = DtWB.Rows(0).Item("TRANSPORTER_CODE").ToString ' TRANSPORTER_CODE
            TextEdit7.Text = DtWB.Rows(0).Item("VEHICLE_CODE").ToString ' VEHICLE_CODE
            TextEdit11.Text = DtWB.Rows(0).Item("DO_SPB").ToString ' DO_SPB 
            TextEdit6.Text = DtWB.Rows(0).Item("MATERIAL_CODE").ToString ' MATERIAL_CODE
            TextEdit9.Text = DtWB.Rows(0).Item("DRIVER_NAME").ToString ' DRIVER_NAME 
            TextEdit10.Text = DtWB.Rows(0).Item("SIM").ToString ' SIM
            TextEdit12.Text = DtWB.Rows(0).Item("JNS_TIMBANGAN").ToString ' JNS_TIMBANGAN
            MemoEdit1.Text = DtWB.Rows(0).Item("REMARKS").ToString ' REMARKS 
            TextEdit13.Text = DtWB.Rows(0).Item("STATUS").ToString ' STATUS 
            TextEdit22.Text = DtWB.Rows(0).Item("TAHUN_TANAM").ToString 'TAHUN TANAM
            TextEdit21.Text = DtWB.Rows(0).Item("ESTATE").ToString 'ESTATE
            TextEdit20.Text = DtWB.Rows(0).Item("AFDELING").ToString 'AFDELING
            TextEdit19.Text = DtWB.Rows(0).Item("BLOCK").ToString 'BLOCK
            TextEdit18.Text = DtWB.Rows(0).Item("FFB_UNITS").ToString 'FFB UNIT
            TextEdit17.Text = DtWB.Rows(0).Item("FFA").ToString 'FFA
            TextEdit16.Text = DtWB.Rows(0).Item("MOISTURE").ToString 'MOISTER
            TextEdit15.Text = DtWB.Rows(0).Item("DIRT").ToString 'DIRT
            TextEdit14.Text = DtWB.Rows(0).Item("NO_SEGEL").ToString 'NO SEGEL
        End If
        DtWB.Clear()
        'DETAIL
        SQL = "SELECT SALESORDERNO_DUP,CONTRACT_NO FROM T_WBTICKET_DETAIL WHERE NO_TICKET='" & TextEdit1.Text & "'"
        Dim DtWBD As New DataTable
        DtWBD = ExecuteQuery(SQL)
        If DtWBD.Rows.Count > 0 THEn
            If UCase(TextEdit2.Text) = "PENGIRIMAN" THEn
                TextEdit24.Text = DtWBD.Rows(0).Item("SALESORDERNO_DUP").ToString 'SALES ORDER
                TextEdit23.Text = ""
            Else
                TextEdit24.Text = ""
                TextEdit23.Text = DtWBD.Rows(0).Item("CONTRACT_NO").ToString 'CONTRACT NUMBER
            End If
        End If
        DtWBD.Clear()
    End Sub

    Private Sub Validasi_input()
        closeall()
        'DI MATIKAN FUNGSI NYA
        SELECT Case TextEdit2.Text.ToUpper
            Case = "PENERIMAAN"
                'HEADER
                TextEdit4.ReadOnly = True : SimpleButton3.Enabled = False 'CUSTOMER
                TextEdit17.ReadOnly = True 'FFA
                TextEdit16.ReadOnly = True 'MOISTER
                TextEdit15.ReadOnly = True 'DIRT
                TextEdit14.ReadOnly = True 'NO SEGEL
                'DETAIL
                TextEdit23.ReadOnly = True : SimpleButton10.Enabled = False 'SALES ORDER
            Case = "PENGIRIMAN"
                'HEADER
                TextEdit3.ReadOnly = True : SimpleButton2.Enabled = False 'VENDOR
                TextEdit11.ReadOnly = True 'DO_SPB
                TextEdit22.ReadOnly = True 'TAHUN TANAM
                TextEdit21.ReadOnly = True 'ESTATE
                TextEdit20.ReadOnly = True 'AFDELING
                TextEdit19.ReadOnly = True 'BLOCK
                TextEdit18.ReadOnly = True 'FFB UNIT
                'DETAIL
                TextEdit24.ReadOnly = True : SimpleButton9.Enabled = False 'CONTRACT NUMBER
            Case = "NUMPANG TIMBANG"
                'HEADER 
                TextEdit4.ReadOnly = True : SimpleButton4.Enabled = False 'CUSTOMER
                TextEdit22.ReadOnly = True  'TAHUN TANAM
                TextEdit21.ReadOnly = True 'ESTATE
                TextEdit20.ReadOnly = True 'AFDELING
                TextEdit19.ReadOnly = True 'BLOCK
                TextEdit18.ReadOnly = True 'FFB UNIT
                TextEdit17.ReadOnly = True 'FFA
                TextEdit16.ReadOnly = True 'MOISTER
                TextEdit15.ReadOnly = True 'DIRT
                TextEdit14.ReadOnly = True 'NO SEGEL
                'DETAIL
                TextEdit24.ReadOnly = True : SimpleButton9.Enabled = False 'CONTRACT NUMBER
                TextEdit23.ReadOnly = True : SimpleButton10.Enabled = False 'SALES ORDER
        End SELECT
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'SAVE/UPDATE

        'BACKUP DULU HEADER DAN DETAIL KE LOG
        InsertLog("Revisi Ticket", TextEdit1.Text)
        SELECT Case TextEdit2.Text.ToUpper
            Case "PENERIMAAN"
                'HEADER
                SQL = " UPDATE T_WBTICKET Set " +
                    " SUPPLIER_CODE ='" & TextEdit3.Text & "', " +
                    " TRANSPORTER_CODE ='" & TextEdit5.Text & "', " +
                    " VEHICLE_CODE ='" & TextEdit7.Text & "', " +
                    " DO_SPB ='" & TextEdit11.Text & "', " +
                    " MATERIAL_CODE ='" & TextEdit6.Text & "', " +
                    " DRIVER_NAME ='" & TextEdit9.Text & "', " +
                    " EMP_NAME ='" & USERNAME & "', " + 'BLM
                    " SIM ='" & TextEdit10.Text & "', " +
                    " JNS_TIMBANGAN ='" & TextEdit12.Text & "', " +
                    " REMARKS ='" & MemoEdit1.Text & "', " +
                    " STATUS ='" & TextEdit13.Text & "', " +
                    " TAHUN_TANAM ='" & TextEdit22.Text & "', " +
                    " ESTATE ='" & TextEdit21.Text & "', " +
                    " AFDELING ='" & TextEdit20.Text & "', " +
                    " BLOCK ='" & TextEdit19.Text & "', " +
                    " FFB_UNITS ='" & TextEdit18.Text & "' " +
                    " WHERE NO_TICKET='" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
                'DETAIL
                SQL = "Update T_WBTICKET_DETAIL SET " +
                    " CONTARCT_NUMBER ='" & TextEdit13.Text & "' " +
                    " WHERE NO_TICKET='" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
            Case "PENGIRIMAN"
                'HEADER
                SQL = " UPDATE T_WBTICKET SET " +
                   " CUSTOMER_CODE ='" & TextEdit4.Text & "', " +
                   " TRANSPORTER_CODE ='" & TextEdit5.Text & "', " +
                   " VEHICLE_CODE ='" & TextEdit7.Text & "', " +
                   " MATERIAL_CODE ='" & TextEdit6.Text & "', " +
                   " DRIVER_NAME ='" & TextEdit9.Text & "', " +
                   " EMP_NAME ='" & USERNAME & "', " + 'BLM
                   " SIM ='" & TextEdit10.Text & "', " +
                   " JNS_TIMBANGAN ='" & TextEdit12.Text & "', " +
                   " REMARKS ='" & MemoEdit1.Text & "', " +
                   " STATUS ='" & TextEdit13.Text & "', " +
                   " FFA ='" & TextEdit17.Text & "', " +
                   " MOISTURE ='" & TextEdit16.Text & "', " +
                   " DIRT ='" & TextEdit15.Text & "', " +
                   " NO_SEGEL ='" & TextEdit14.Text & "', " +
                   " WHERE NO_TICKET='" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
                'DETAIL
                SQL = "Update T_WBTICKET_DETAIL SET " +
                " SALESORDERNO_DUP ='" & TextEdit23.Text & "' " +
                " WHERE NO_TICKET='" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
            Case "NUMPANG TIMBANG"
                'HEADER
                SQL = " UPDATE T_WBTICKET SET " +
                   " SUPPLIER_CODE ='" & TextEdit3.Text & "', " +
                   " TRANSPORTER_CODE ='" & TextEdit5.Text & "', " +
                   " VEHICLE_CODE ='" & TextEdit7.Text & "', " +
                   " MATERIAL_CODE ='" & TextEdit6.Text & "', " +
                   " DRIVER_NAME ='" & TextEdit9.Text & "', " +
                   " EMP_NAME ='" & USERNAME & "', " + 'BLM
                   " JNS_TIMBANGAN ='" & TextEdit12.Text & "', " +
                   " REMARKS ='" & MemoEdit1.Text & "', " +
                   " STATUS ='" & TextEdit13.Text & "', " +
                   " WHERE NO_TICKET='" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
        End SELECT
        MsgBox("Data Berhasil di Revisi", vbInformation, Me.Text)

    End Sub

    Private Sub closeall()

        TextEdit3.ReadOnly = False
        TextEdit4.ReadOnly = False
        TextEdit5.ReadOnly = False
        TextEdit6.ReadOnly = False
        TextEdit7.ReadOnly = False
        TextEdit8.ReadOnly = False
        TextEdit9.ReadOnly = False
        TextEdit10.ReadOnly = False
        TextEdit11.ReadOnly = False

        TextEdit12.ReadOnly = False
        TextEdit13.ReadOnly = False
        TextEdit14.ReadOnly = False
        TextEdit15.ReadOnly = False
        TextEdit16.ReadOnly = False
        TextEdit17.ReadOnly = False
        TextEdit18.ReadOnly = False
        TextEdit19.ReadOnly = False
        TextEdit20.ReadOnly = False
        TextEdit21.ReadOnly = False
        TextEdit22.ReadOnly = False

        MemoEdit1.ReadOnly = False

        TextEdit23.ReadOnly = False
        TextEdit24.ReadOnly = False

        SimpleButton2.Enabled = True
        SimpleButton3.Enabled = True
        SimpleButton4.Enabled = True
        SimpleButton5.Enabled = True

        SimpleButton6.Enabled = True
        SimpleButton7.Enabled = True
        SimpleButton8.Enabled = True
        SimpleButton9.Enabled = True
        SimpleButton10.Enabled = True

        SimpleButton11.Enabled = True
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'VENDOR POP
        LSQL = "SELECT VENDOR_CODE,VENDOR_NAME  FROM T_VENDOR WHERE INACTIVE IS NULL OR INACTIVE ='N' "
        LField = "VENDOR_CODE"
        ValueLoV = ""
        TextEdit3.Text = FrmShowLOV(FrmLoV, LSQL, "SUPPLIER", "SUPPLIER/VENDOR TIMBANG")
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'CUSTOMER
        LSQL = "SELECT CUST_CODE,CUST_NAME,INACTIVE  FROM T_CUSTOMER WHERE INACTIVE IS NULL OR INACTIVE ='N'"
        LField = "CUST_CODE"
        ValueLoV = ""
        TextEdit4.Text = FrmShowLOV(FrmLoV, LSQL, "CUSTOMER", "CUSTOMER")

    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'TRASPORTER
        LSQL = "SELECT TRANSPORTER_CODE,TRANSPORTER_NAME  FROM T_TRANSPORTER  WHERE INACTIVE IS NULL OR INACTIVE ='N' "
        LField = "TRANSPORTER_CODE"
        ValueLoV = ""
        TextEdit5.Text = FrmShowLOV(FrmLoV, LSQL, "TRANSPORTER", "TRANSPORTER")
    End Sub

    Private Sub SimpleButton7_Click(sender As Object, e As EventArgs) Handles SimpleButton7.Click
        'MATERILA
        LSQL = "SELECT MATERIAL_CODE,MATERIAL_NAME ,INACTIVE FROM T_MATERIAL   WHERE INACTIVE IS NULL OR INACTIVE='N'"
        LField = "MATERIAL_CODE"
        ValueLoV = ""
        TextEdit6.Text = FrmShowLOV(FrmLoV, LSQL, "MATERIAL", "MATERIAL")
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        'VEHICLE
        LSQL = "SELECT VEHICLE_CODE,PLATE_NUMBER  FROM T_VEHICLE  WHERE INACTIVE Is NULL Or INACTIVE='N'"
        LField = "VEHICLE_CODE"
        ValueLoV = ""
        TextEdit7.Text = FrmShowLOV(FrmLoV, LSQL, "VEHICLE", "VEHICLE")
    End Sub

    Private Sub SimpleButton8_Click(sender As Object, e As EventArgs) Handles SimpleButton8.Click
        'DRIVER
        LSQL = "SELECT DRIVER_CODE,DRIVER_NAME  FROM T_DRIVER   WHERE TRANSPORTER_CODE LIKE '%" & TextEdit5.Text & "%' AND INACTIVE IS NULL OR INACTIVE='N'"
        LField = "DRIVER_CODE"
        ValueLoV = ""
        TextEdit9.Text = FrmShowLOV(FrmLoV, LSQL, "DRIVER", "DRIVER")
    End Sub

    Private Sub SimpleButton11_Click(sender As Object, e As EventArgs) Handles SimpleButton11.Click
        'JNS TIMBANGAN
        Dim CODE As String
        LSQL = "SELECT SUBSTR(GD_ID,3) GD_ID,GD_DESC  FROM T_LOOKUP_GROUP_DETAIL WHERE GROUP_ID='03'"
        LField = "GD_ID"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "CODE", "JENIS TIMBANG")
        TextEdit12.Text = Val(CODE)
    End Sub

    Private Sub SimpleButton9_Click(sender As Object, e As EventArgs) Handles SimpleButton9.Click
        'CONTRACT NUMBER
        Dim CODE As String
        LSQL = " SELECT CONTRACT_NO,VENDORID,ITEMNO,MATERIALCODE FROM T_CONTRACT  " +
        " WHERE VENDORID Like '%" & TextEdit3.Text & "%' " +
        " And MATERIALCODE Like '%" & TextEdit7.Text & "%' " +
        " And INACTIVE Is NULL Or INACTIVE ='N'"
        LField = "CONTRACT_NO"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "CONTRACT_NO", "CONTRACT_NO")
        TextEdit24.Text = CODE
    End Sub

    Private Sub SimpleButton10_Click(sender As Object, e As EventArgs) Handles SimpleButton10.Click
        'SALES ORDER
        Dim CODE As String
        LSQL = " SELECT SO_NO,SO_QTY,MATERIAL_CODE,SC_NO,SO_START,SO_END FROM T_SALESORDER " +
        " WHERE CUST_CODE Like '%" & TextEdit4.Text & "%'" +
        " And MATERIAL_CODE  Like '%" & TextEdit7.Text & " %'" +
        " And  INACTIVE IS NULL OR INACTIVE='N'"
        LField = "SO_NO"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "SO_NO", "SALES_ORDER")
        TextEdit23.Text = CODE
    End Sub
End Class