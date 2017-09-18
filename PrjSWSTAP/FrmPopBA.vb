Public Class FrmPopBA
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'close
        BA_DIALOG = False
        Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'OK
        BA_DIALOG = True
        Close()
    End Sub

End Class