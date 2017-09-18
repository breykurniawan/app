Imports System.IO
Imports System.Net
Public Class SplashScreen1
    'Sub New()
    '    InitializeComponent()
    'End Sub

    Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
        MyBase.ProcessCommand(cmd, arg)
    End Sub

    Private Sub SplashScreen1_Load(sender As Object, e As EventArgs) Handles Me.Load
        labelControl1.Text = "Copyright © 2010 - " & Format(Now, "yyyy")
        LabelControl3.Text = "Ver." & AppVersion
    End Sub
    Public Enum SplashScreenCommand
        SomeCommandId
    End Enum
End Class
