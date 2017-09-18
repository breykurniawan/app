Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmClearTiketTimbang
    Private Sub FrmClearTiketTimbang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "CLEAR PENDING TICKET"
        LoadView()
    End Sub

    Private Sub FrmClearTiketTimbang_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 155
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'DELETE
        If Not IsEmptyText({TextEdit1}) = True THEn
            Dim MEMO As String = MemoEdit1.Text
            SQL = "UPDATE T_WBTICKET SET DELETED=1,REMARKS='" & MEMO & "' WHERE WEIGHT_OUT IS NULL AND DELETED=0 AND NO_TICKET LIKE '%" & TextEdit1.Text & "%'"
            ExecuteNonQuery(SQL)
            TextEdit1.Text = ""

            MemoEdit1.Text = ""
            GridControl1.DataSource = Nothing
            LoadView()
            MsgBox("Delete Succesfull", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Me.Close()
    End Sub
    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            MsgBox("Data Tidak ada", vbInformation, Me.Text)
        Else
            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "NO_TICKET").ToString()  'ID
            SimpleButton1.Enabled = True 'save
        End If
    End Sub
    Private Sub LoadView()
        If TextEdit1.Text <> "" THEn
            SQL = "SELECT * FROM V_TICKET_FINISH WHERE WEIGHT_OUT IS NULL AND NO_TICKET LIKE '%" & TextEdit1.Text & "%' AND ROWNUM <=500 ORDER BY DATE_IN DESC"
        Else
            SQL = "SELECT * FROM V_TICKET_FINISH WHERE WEIGHT_OUT IS NULL AND ROWNUM <=500 ORDER BY DATE_IN DESC"
        End If
        FILLGridView(SQL, GridControl1)
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'FIND
        LoadView()
    End Sub
End Class