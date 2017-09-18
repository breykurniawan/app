Public Class UCGeneralConfig
    Private Sub SimpleButton2_Click_1(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'close
        Me.ParentForm.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'save
        If Not IsEmptyText({TextEdit1}) Then
            SaveConfig()
        End If
    End Sub

    Private Sub UCGeneralConfig_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadConfig()
    End Sub
    Public Sub LoadConfig()
        ' GetConfig() 'Load Config from setting

        TextEdit1.Text = CompanyCode
        TextEdit2.Text = Company  'My.Settings.Company
        TextEdit3.Text = MillPlant
        TextEdit4.Text = LocationSite
        TextEdit5.Text = StoreLocation1
        TextEdit6.Text = StoreLocation2
        TextEdit7.Text = ComportSetting
        TextEdit8.Text = WBCode
        TextEdit9.Text = IPCamera1
        TextEdit10.Text = IPCamera2
        TextEdit11.Text = IPIndicator
        ComboBoxEdit1.Text = LoadingRampTransit
        ComboBoxEdit2.Text = SAP
    End Sub

    Public Sub SaveConfig()
        My.Settings.CompanyCode = TextEdit1.Text
        My.Settings.Save()
        My.Settings.Company = TextEdit2.Text
        My.Settings.Save()
        My.Settings.Millplant = TextEdit3.Text
        My.Settings.Save()
        My.Settings.LocationSite = TextEdit4.Text
        My.Settings.Save()
        My.Settings.StoreLocation1 = TextEdit5.Text
        My.Settings.Save()
        My.Settings.StoreLocation2 = TextEdit6.Text
        My.Settings.Save()
        My.Settings.ComportSetting = TextEdit7.Text
        My.Settings.Save()
        My.Settings.WBCode = TextEdit8.Text
        My.Settings.Save()
        My.Settings.IPCamera1 = TextEdit9.Text
        My.Settings.Save()
        My.Settings.IPCamera2 = TextEdit10.Text
        My.Settings.Save()
        My.Settings.IPIndicator = TextEdit11.Text
        My.Settings.Save()
        My.Settings.LoadingRampTransit = ComboBoxEdit1.Text
        My.Settings.Save()
        My.Settings.SAP = ComboBoxEdit2.Text
        My.Settings.Save()
    End Sub
End Class
