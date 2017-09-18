Public Class FrmParameter
    Private Sub FrmParameter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Loadsetting

    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE FROM GENERAL
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'SIMPAN
        If Not IsEmptyText({TextEdit1}) = True Then
            simpansetting()
            MsgBox("Save Succeeded", vbInformation, "General Parameter")
            Loadsetting()
        End If
    End Sub

    Private Sub simpansetting()
        My.Settings.SinggleFrmActive = If(CheckEdit1.Checked = True, 1, 0)
        My.Settings.Save()
        My.Settings.PathImage = TextEdit1.Text
        My.Settings.Save()

        singgeleForm = If(My.Settings.SinggleFrmActive = 1, True, False)
        pathimage = My.Settings.PathImage
    End Sub
    Public Sub Loadsetting()
        CheckEdit1.Checked = If(My.Settings.SinggleFrmActive = 1, True, False)
        singgeleForm = If(My.Settings.SinggleFrmActive = 1, True, False)

        TextEdit1.Text = My.Settings.PathImage
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'FIND IMAGE
        OpenFileDialog1.Filter = "Image File (*.JPG)|*.JPG|All Image (*.All)|*.*"
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            ' Get THE file name.
            Dim path As String = OpenFileDialog1.FileName
            Dim FileLen As Integer = Len(path) - Len(OpenFileDialog1.SafeFileName)
            TextEdit1.Text = path.Substring(0, FileLen)
        End If
    End Sub

End Class