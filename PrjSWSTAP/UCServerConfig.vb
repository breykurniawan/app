Public Class UCServerConfig
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'close
        Me.ParentForm.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'save
        If Not IsEmptyCombo({ComboBoxEdit1}) Then
            If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5}) Then
                If ComboBoxEdit1.Text = "LOKAL" Then
                    '    My.Settings.DBSourceLocal = TextEdit1.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBVerLocal = TextEdit2.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBNameLocal = TextEdit3.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBUserLocal = TextEdit4.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBPassLocal = TextEdit5.Text
                    '    My.Settings.Save()

                    '    'OpenConnLocal()
                    '    'CheckConLocal()
                    '    'CloseConnLocal()
                    'ElseIf ComboBoxEdit1.Text = "STAGING" Then
                    '    My.Settings.DBSourceStaging = TextEdit1.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBVerStaging = TextEdit2.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBNameStaging = TextEdit3.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBUserStaging = TextEdit4.Text
                    '    My.Settings.Save()
                    '    My.Settings.DBPassStaging = TextEdit5.Text
                    '    My.Settings.Save()

                    '    'OpenConnStaging()
                    '    'CheckConStaging()
                    '    'CloseConnStaging()
                End If
            End If
        End If
    End Sub

    Private Sub ComboBoxEdit1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        TextEdit5.Text = ""
        LabelControl7.Text = ""
        If ComboBoxEdit1.Text = "LOKAL" Then
            LocalConfig()
        ElseIf ComboBoxEdit1.Text = "STAGING" Then
            StagingConfig()
        End If
    End Sub

    Private Sub LocalConfig()
        TextEdit1.Text = My.Settings.DBSourceLocal.ToString
        TextEdit2.Text = My.Settings.DBVerLocal.ToString
        TextEdit3.Text = My.Settings.DBNameLocal.ToString
        TextEdit4.Text = My.Settings.DBUserLocal.ToString
        TextEdit5.Text = My.Settings.DBPassLocal.ToString
    End Sub
    Private Sub StagingConfig()
        TextEdit1.Text = My.Settings.DBSourceStaging.ToString
        TextEdit2.Text = My.Settings.DBVerStaging.ToString
        TextEdit3.Text = My.Settings.DBNameStaging.ToString
        TextEdit4.Text = My.Settings.DBUserStaging.ToString
        TextEdit5.Text = My.Settings.DBPassStaging.ToString
    End Sub
    Private Sub CheckConLocal()
        If OpenConnLocal() = True Then
            LabelControl7.Text = "Connection Successful"
            LabelControl7.BackColor = Color.Green
        Else
            LabelControl7.Text = "Connection Failed"
            LabelControl7.BackColor = Color.Red
        End If
    End Sub
    Private Sub CheckConStaging()
        If OpenConnStaging() = True Then
            LabelControl7.Text = "Connection Successful"
            LabelControl7.BackColor = Color.Green
        Else
            LabelControl7.Text = "Connection Failed"
            LabelControl7.BackColor = Color.Red
        End If
    End Sub
End Class
