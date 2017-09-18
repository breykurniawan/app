Public Class FrmUserRole
    Private Sub FrmUserRole_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "USER ROLE"
        'LOAD DATA
        LoadUserRole
    End Sub

    Private Sub LoadUserRole()
        SQL = "SELECT ROLEID,ROLENAME FROM T_ROLE WHERE AKTIF='Y' ORDER BY ROLEID"
        GridControl1.DataSource = Nothing
        FILLGridView(SQL, GridControl1)
        TextEdit1.Text = ""
        TextEdit2.Text = ""

        SimpleButton1.SELECT()

        SimpleButton1.Enabled = True 'ADD
        SimpleButton2.Enabled = False 'EDIT
        SimpleButton3.Enabled = False 'SAVE

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UnlockAll()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit1.SELECT()

        SimpleButton1.Enabled = False 'ADD
        SimpleButton2.Enabled = False 'EDIT
        SimpleButton3.Enabled = True 'SAVE
    End Sub

    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
    End Sub
    Private Sub lockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit1.SELECT()

        SimpleButton1.Enabled = True 'ADD
        SimpleButton2.Enabled = False 'EDIT
        SimpleButton3.Enabled = False 'SAVE
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        ''EDIT
        'TextEdit1.Enabled = False
        'TextEdit2.Enabled = True
        'TextEdit2.SELECT()
        'SimpleButton1.Enabled = False 'ADD
        'SimpleButton2.Enabled = False 'EDIT
        'SimpleButton3.Enabled = True 'SAVE
        'DELETE
        SQL = "UPDATE T_ROLE SET AKTIF='N' WHERE ROLEID='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadUserRole()
        MsgBox("Delete Successful", vbInformation, "USER ROLE")
    End Sub

    Private Sub FrmUserRole_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 150
    End Sub

    Private Sub gridView2_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowCellClick
        'GRID INDIKATOR
        On Error Resume Next
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'ADD
            SimpleButton3.Enabled = False 'SAVE
            SimpleButton2.Enabled = False 'EDIT
        Else
            SimpleButton1.Enabled = False 'ADD
            SimpleButton2.Enabled = True 'EDIT
            SimpleButton3.Enabled = False 'SAVE

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "ROLEID").ToString()  'ID
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "ROLENAME").ToString() 'NAME

            TextEdit1.Enabled = False
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True THEn
            Dim ROLEID As String = TextEdit1.Text
            Dim ROLENAME As String = TextEdit2.Text
            SQL = "SELECT ROLEID,ROLENAME FROM T_ROLE WHERE ROLEID ='" & TextEdit1.Text & "' "
            If CheckRecord(SQL) = 0 THEn
                SQL = "INSERT INTO T_ROLE (ROLEID,ROLENAME,INPUT_BY,AKTIF)" +
                    " VALUES ('" & ROLEID & "','" & ROLENAME & "','" & USERNAME & "','Y')	"
                ExecuteNonQuery(SQL)
                LoadUserRole()
                MsgBox("Insert  Successful", vbInformation, "USER ROLE")
            Else
                SQL = "UPDATE T_ROLE SET ROLENAME='" & ROLENAME & "',UPDATE_BY='" & USERNAME & "'" +
                      "WHERE ROLEID='" & ROLEID & "'"
                ExecuteNonQuery(SQL)
                LoadUserRole()
                MsgBox("Update Successful", vbInformation, "USER ROLE")
            End If
            TextEdit1.Text = ""
            TextEdit2.Text = ""
            TextEdit1.SELECT()

            SimpleButton1.Enabled = True 'ADD
            SimpleButton2.Enabled = False 'EDIT /delete
            SimpleButton3.Enabled = False 'SAVE
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'Close()
        Me.Close()
    End Sub

End Class