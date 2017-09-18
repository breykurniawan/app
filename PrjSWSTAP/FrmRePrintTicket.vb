Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmRePrintTicket
    Private Sub FrmRePrintTicket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Re Print Ticket"

        DateEdit1.Properties.Mask.Culture = New System.Globalization.CultureInfo("en-US")
        DateEdit1.Properties.Mask.EditMask = "MM/dd/yyyy"
        DateEdit1.Properties.Mask.UseMaskAsDisplayFormat = True
        DateEdit1.Properties.CharacterCasing = CharacterCasing.Upper

        DateEdit2.Properties.Mask.Culture = New System.Globalization.CultureInfo("en-US")
        DateEdit2.Properties.Mask.EditMask = "MM/dd/yyyy"
        DateEdit2.Properties.Mask.UseMaskAsDisplayFormat = True
        DateEdit2.Properties.CharacterCasing = CharacterCasing.Upper

        Dim myDate As Date = DateTime.Now
        'Format(myDate, "dd/MM/yyyy")
        DateEdit1.EditValue = Format(myDate, "MM/01/yyyy")
        DateEdit2.EditValue = Format(myDate, "MM/dd/yyyy")

        TextEdit1.Text = ""
        TextEdit2.Text = ""

        SimpleButton1.Enabled = False

        RadioButton1.Checked = True
        RadioButton2.Checked = False

        LoadView()
    End Sub

    Private Sub FrmRePrintTicket_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 210
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
        If RadioButton1.Checked = True THEn
            SQL = "SELECT * FROM V_TICKET_FINISH  " +
                " WHERE WEIGHT_OUT Is Not NULL " +
                " And MATERIAL='FFB' " +
                " And DATE_IN BETWEEN " & tgl1 & " AND " & tgl2 & "  " +
                " And NO_TICKET LIKE '%" & TextEdit1.Text & "%' " +
                " And POLICE_NO LIKE '%" & TextEdit2.Text & "%'"
            FILLGridView(SQL, GridControl1)
        Else
            SQL = "SELECT * FROM V_TICKET_FINISH  " +
               " WHERE WEIGHT_OUT Is Not NULL " +
               " And MATERIAL<>'FFB' " +
               " And NO_TICKET LIKE '%" & TextEdit1.Text & "%' " +
               " And DATE_IN BETWEEN " & tgl1 & " AND " & tgl2 & "  " +
            " And POLICE_NO LIKE '%" & TextEdit2.Text & "%'"
            FILLGridView(SQL, GridControl1)
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Close()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = ""
        LoadView()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        TextEdit1.Text = "" : TextEdit2.Text = "" : TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = ""
        LoadView()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        TextEdit3.Text = "" : TextEdit4.Text = "" : TextEdit5.Text = ""
        LoadView()
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            MsgBox("Data Tidak ada", vbInformation, Me.Text)
        Else
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "NO_TICKET").ToString()  'ID
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL").ToString()  'material 

            SimpleButton1.Enabled = True 'save
            SimpleButton1.Text = "Print" 'SAVE

            If TextEdit3.Text <> "" THEn
                SimpleButton1.Enabled = True
                TextEdit5.Text = UCase(GetTipeTrWB(TextEdit3.Text))   'tranas type
            Else
                SimpleButton1.Enabled = False
                TextEdit5.Text = UCase(GetTipeTrWB(TextEdit3.Text))   'tranas type
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'PRINT TICKET
        If TextEdit3.Text <> "" THEn
            Dim JnsTimbang As String = ""
            Dim dtr As New DataTable
            SQL = "SELECT jns_timbangan FROM t_wbticket WHERE no_ticket='" & TextEdit3.Text & "'"
            dtr = ExecuteQuery(SQL)
            If dtr.Rows.Count > 0 THEn JnsTimbang = dtr.Rows(0).Item("jns_timbangan").ToString
            dtr.Clear()

            If RadioButton1.Checked = True THEn
                ' FFB  
                SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit3.Text & "' AND MATERIAL='FFB'"
                ShowReport("RPTTICKETFFB", SQL, "R_TIKET")
            ElseIf ucase(TextEdit4.Text) = "CPO" THEn
                'TIMBANG SENDIRI NON FFB (CPO DSB) 
                SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit3.Text & "'  AND MATERIAL='CPO'"
                ShowReport("RPTTICKETCPO", SQL, "R_TIKETCPO")
            Else
                'SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit3.Text & "'"
                'ShowReport("RPTTICKETFNT", SQL, "R_TIKET")
                SQL = "SELECT * FROM V_TICKET_FINISH WHERE TRIM(NO_TICKET)='" & TextEdit3.Text & "' AND MATERIAL<>'FFB' AND MATERIAL<>'CPO' "
                ShowReport("RPTTICKETFFB", SQL, "R_TIKET")
            End If
        End If
    End Sub
End Class