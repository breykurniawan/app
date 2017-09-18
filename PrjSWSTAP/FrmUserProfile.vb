Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Public Class FrmUserProfile
    Dim imagename As String

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        'FIND IMAGE
        OpenFileDialog1.Filter = "Image File (*.JPG)|*.JPG|All Image (*.All)|*.*"
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK THEn
            ' Get THE file name.
            Dim path As String = OpenFileDialog1.FileName
            Try
                'Dim text As String = File.ReadAllText(path)
                'Me.Text = text.Length.ToString
                MemoEdit1.Text = path
                imagename = path
                Dim bmp As New Bitmap(path)
                If Not IsNothing(PictureBox1.Image) THEn PictureBox1.Image.Dispose() 'Optional if you want to destroy THE previously loaded image
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                PictureBox1.Image = bmp
            Catch ex As Exception
                MsgBox("Not a valid image file.", vbInformation, "Image Not Valid")
            End Try
        End If
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE
        SQL = "UPDATE T_USERPROFILE SET AKTIF='N' WHERE USERID='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadUser()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "USER ROLE")
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE
        Dim UID As String = TextEdit1.Text
        Dim UNAME As String = TextEdit2.Text
        Dim EMAIL As String = TextEdit3.Text
        Dim ROLEID As String = GetCodeRole(ComboBoxEdit1.Text)
        Dim INPUT_BY As String = USERNAME
        Dim INPUT_DATE As String = Now
        Dim UPDATE_BY As String = USERNAME
        Dim UPDATE_DATE As String = Now
        Dim PASSWD As String = TextEdit4.Text
        'CHECK 
        If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit3, TextEdit4}) = True THEn
            If Not IsEmptyCombo({ComboBoxEdit1}) = True THEn
                SQL = "SELECT * FROM T_USERPROFILE WHERE AKTIF='Y' AND USERID='" & TextEdit1.Text & "'"
                imagename = MemoEdit1.Text
                If CheckRecord(SQL) = 0 THEn
                    Try
                        If imagename <> "" THEn
                            Dim fls As FileStream
                            fls = New FileStream(imagename, FileMode.Open, FileAccess.Read)
                            Dim blob As Byte() = New Byte(fls.Length - 1) {}
                            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length))
                            fls.Close()

                            SQL = "insert into T_USERPROFILE(USERID,USERNAME,PASSWD,EMAIL,ROLEID,INPUT_BY,IMAGE,AKTIF)" +
                            "VALUES ('" & UID & "','" & UNAME & "','" & PASSWD & "','" & EMAIL & "','" & ROLEID & "','" & INPUT_BY & "'," + " :BlobParameter,'Y' )"

                            Dim blobParameter As New OracleParameter()
                            blobParameter.OracleDbType = OracleDbType.Blob
                            blobParameter.ParameterName = "BlobParameter"
                            blobParameter.Value = blob

                            CMD = New OracleCommand(SQL, CONN)
                            CMD.Parameters.Add(blobParameter)
                            CMD.ExecuteNonQuery()
                            CMD.Dispose()

                            SQL = "SELECT * FROM T_USERPROFILE WHERE AKTIF='Y' AND USERID='" & TextEdit1.Text & "'"
                            If CheckRecord(SQL) > 0 THEn
                                MsgBox("SAVE SUCCESSFUL", vbInformation, "USER PROFILE")
                                UpdateCode("EM")
                                LoadUser()
                            End If
                        End If
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                Else
                    If UCase(SimpleButton2.Text) = "UPDATE" THEn
                        Try
                            Dim fls As FileStream
                            fls = New FileStream(imagename, FileMode.Open, FileAccess.Read)
                            Dim blob As Byte() = New Byte(fls.Length - 1) {}
                            fls.Read(blob, 0, System.Convert.ToInt32(fls.Length))
                            fls.Close()

                            SQL = "UPDATE T_USERPROFILE SET USERNAME= '" & UNAME & "',EMAIL='" & EMAIL & "',ROLEID='" & ROLEID & "',UPDATE_BY='" & UPDATE_BY & "', IMAGE=:BlobParameter " +
                                  " WHERE USERID='" & TextEdit1.Text & "'"

                            Dim blobParameter As New OracleParameter()
                            blobParameter.OracleDbType = OracleDbType.Blob
                            blobParameter.ParameterName = "BlobParameter"
                            blobParameter.Value = blob

                            CMD = New OracleCommand(SQL, CONN)
                            CMD.Parameters.Add(blobParameter)
                            CMD.ExecuteNonQuery()
                            CMD.Dispose()

                            LoadUser()
                            MsgBox("UPDATE SUCCESSFUL", vbInformation, "USER PROFILE")

                        Catch EX As Exception
                            MessageBox.Show(EX.Message)
                        End Try
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Me.Close()
    End Sub
    Private Sub GridHeader()
        Dim View As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim FieldNames() As String = New String() {"USERID", "USERNAME", "EMAIL", "ROLENAME", "IMAGE"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        View.Columns.Clear()
        For I = 0 To FieldNames.Length - 1
            Column = View.Columns.AddField(FieldNames(I))
            Column.VisibleIndex = I
        Next

        Dim repItemGraphicsEdit As New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
        repItemGraphicsEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        repItemGraphicsEdit.BestFitWidth = 50
        View.Columns("IMAGE").ColumnEdit = repItemGraphicsEdit


        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("ROLENAME"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub

    Private Sub FrmUserProfile_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Text = "USER PROFILE"
        LoadRole()
        GridHeader()
        LoadUser()
        LockAll()
    End Sub
    Private Sub LoadRole()
        SQL = "SELECT ROLENAME FROM t_role WHERE AKTIF='Y' ORDER BY ROLEID"
        FILLComboBoxEdit(SQL, 0, ComboBoxEdit1, False)
    End Sub

    Private Sub LoadUser()
        SQL = "SELECT USERID,USERNAME,EMAIL,ROLENAME,IMAGE " +
        "FROM T_USERPROFILE A " +
        "LEFT JOIN T_ROLE B On A.ROLEID=B.ROLEID And B.AKTIF='Y' " +
        "WHERE A.AKTIF='Y' " +
        "ORDER BY USERID"
        FILLGridView(SQL, GridControl1)

        GridControl1.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        TextEdit3.Enabled = False
        ComboBoxEdit1.Enabled = False
        MemoEdit1.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'del
        SimpleButton6.Enabled = False 'File open

    End Sub
    Private Sub UnLockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        TextEdit3.Enabled = True
        ComboBoxEdit1.Enabled = True
        MemoEdit1.Enabled = True
        TextEdit2.SELECT()
        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'del
        SimpleButton6.Enabled = True 'File Open
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UnLockAll()
        TextEdit1.Text = GetCodeUser()
        TextEdit1.SELECT()
    End Sub

    Private Sub FrmUserProfile_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = Me.Height - 250
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        ComboBoxEdit1.Text = ""
        MemoEdit1.Text = ""
        LockAll()
        PictureBox1.Image = Nothing
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub FillImageToPic(ByVal sql As String, ByVal PctImg As PictureBox, ByVal Picid As String)
        On Error Resume Next
        Dim dataTable As DataTable = ExecuteQuery(sql)
        'if THEre is an already an image in picturebox, THEn delete it 
        If Not (PctImg.Image Is Nothing) THEn
            PctImg.Image.Dispose()
        End If
        'using filestream object write THE column as bytes and store it as an image 
        pathimage = My.Settings.PathImage
        If pathimage <> "" THEn
            If System.IO.File.Exists(pathimage) THEn
                'THE file doesn't exist
                pathimage = Path.GetTempPath
            End If
        Else
            pathimage = Path.GetTempPath
        End If

        Dim FS As New FileStream(pathimage & "\" & Picid & ".jpg", FileMode.Create, FileAccess.Write)
        'Dim filename As String = Path.Combine(pth, FS.Name.ToString)
        For Each dataRow As DataRow In dataTable.Rows
            If dataRow(1).ToString() <> "" THEn
                Dim blob As Byte() = DirectCast(dataRow(1), Byte())
                FS.Write(blob, 0, blob.Length)
                MemoEdit1.Text = FS.Name.ToString
                FS.Close()
                FS = Nothing
                PctImg.Image = Image.FROMFile(Picid & ".jpg")
                PctImg.SizeMode = PictureBoxSizeMode.StretchImage
                PctImg.Refresh()
            End If
        Next
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'del
            SimpleButton6.Enabled = True 'FIND

        Else
            SimpleButton1.Enabled = False 'ADD
            SimpleButton2.Enabled = True 'SAVE
            SimpleButton3.Enabled = True 'DEL
            SimpleButton6.Enabled = True 'FIND

            SimpleButton2.Text = "Update" 'SAVE

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "USERID").ToString()  'ID
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "USERNAME").ToString() 'NAME
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "EMAIL").ToString() 'NAME

            ComboBoxEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "ROLENAME").ToString() 'NAME
            TextEdit1.Enabled = False
            SQL = " SELECT USERID,IMAGE FROM T_USERPROFILE WHERE AKTIF='Y' AND USERID='" & TextEdit1.Text & "'"
            FillImageToPic(SQL, PictureBox1, TextEdit1.Text)
            UnLockAll()
        End If
    End Sub
    Private Sub TextEdit1KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit1.KeyPress
        'dirt
        e.Handled = NumericOnly(e)
    End Sub

End Class