Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmEditTiket


    Private Sub FrmEditTiket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "WEIGHBRIDGE EDIT TICKET"
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

        SimpleButton1.Enabled = False
        LoadView()
    End Sub
    Private Sub LoadView()
        SQL = "SELECT * FROM T_WBTICKET WHERE WEIGHT_OUT IS NOT NULL AND DELETED=0 " +
            " AND DATE_IN BETWEEN TO_DATE ('" & DateEdit1.Text & "', 'mm/dd/yyyy') AND TO_DATE ('" & DateEdit2.Text & "', 'mm/dd/yyyy') " +
            " AND JNS_TIMBANGAN LIKE '%" & TextEdit2.Text & "%' " +
            " AND NO_TICKET LIKE '%" & TextEdit1.Text & "%'" +
            " ORDER BY DATE_IN DESC"
        FILLGridView(SQL, GridControl1)
    End Sub
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'close
        Me.Close()
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            MsgBox("Data Tidak ada", vbInformation, Me.Text)
        Else
            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "NO_TICKET").ToString()  'ID
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "JNS_TIMBANGAN").ToString()  'ID
            SimpleButton1.Enabled = True 'save
            SimpleButton1.Text = "Revisi" 'SAVE

            If TextEdit1.Text <> "" THEn
                SQL = "SELECT NO_TICKET,CONTRACT_NO,SALESORDERNO_DUP,ITEMNO,DELIVERY_QUANTITY " +
                " FROM T_WBTICKET_DETAIL " +
                " WHERE NO_TICKET Like '%" & TextEdit1.Text & "%'"
                FILLGridView(SQL, GridControl2)
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If TextEdit1.Text <> "" THEn
            'edit TIKET FINISH 
            nEditTicket = TextEdit1.Text
            FrmShowUp(FrmEditTicketUp)
        End If
    End Sub

    Private Sub FrmEditTiket_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 220
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Dim CODE As String
        LSQL = "SELECT SUBSTR(GD_ID,3) GD_ID,GD_DESC  FROM T_LOOKUP_GROUP_DETAIL WHERE GROUP_ID='03'"
        LField = "GD_ID"
        ValueLoV = ""
        CODE = FrmShowLOV(FrmLoV, LSQL, "CODE", "JENIS TIMBANG")
        TextEdit2.Text = Val(CODE)
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        LoadView()
    End Sub
End Class