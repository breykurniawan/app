Imports System.Windows.Forms

Public Class Dialog1
    Dim Client As New Net.WebClient()

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim DR As DialogResult = FolderBrowserDialog1.ShowDialog
        If DR = Windows.Forms.DialogResult.OK Then
            Client.DownloadFile(Web_update.Downuri,
            FolderBrowserDialog1.SelectedPath.ToString &
            "\Program_update" & Date.Today.ToShortDateString.ToString & ".zip")
        End If
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Client.DownloadFile(Web_update.Downuri,
        My.Computer.FileSystem.SpecialDirectories.Temp.ToString & "\Program_update.zip")
        Process.Start(My.Computer.FileSystem.SpecialDirectories.Temp.ToString &
        "\Program_Update.zip")
        Application.Exit()
    End Sub

End Class