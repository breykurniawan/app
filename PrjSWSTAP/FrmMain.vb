Imports DevExpress.XtraNavBar
Imports DevExpress.XtraSplashScreen

Imports System.Environment
Imports System.Net
Imports System.Windows.Forms
Imports System.Reflection

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Public Class FrmMain
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetConfig()
        InfoSite()
        PanelControl2.Width = Me.Width / 6
        If Ping(IPIndicator) = True Then ToolStripStatusLabel5.BackColor = Color.LimeGreen
        If Ping(IPCamera1) = True Then ToolStripStatusLabel6.BackColor = Color.LimeGreen
        If Ping(IPCamera2) = True Then ToolStripStatusLabel7.BackColor = Color.LimeGreen
    End Sub
    Private Sub InfoSite()
        ToolStripStatusLabel1.Text = AppVersion
        ToolStripStatusLabel2.Text = GetIPAddr()
        ToolStripStatusLabel4.Text = CompanyCode
        LabelControl3.Text = Company
        LabelControl4.Text = LocationSite
        LabelControl5.Text = MillPlant
    End Sub
    Private Sub FrmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl2.Width = Me.Width / 6
    End Sub

    Private Sub NavBarControl1_LinkClicked(sender As Object, e As NavBarLinkEventArgs) Handles NavBarControl1.LinkClicked
        Dim Frm As String = e.Link.Caption
        Dim frmAktive As String = GetFrmName(Frm)
        Select Case Frm

            'HOME
            Case "EXIT"
                If MsgBox(UCase(" THEnAre you sure you want To Exit Program?"), MsgBoxStyle.YesNo, "Exit") = MsgBoxResult.Yes Then
                    'TUTUP SEMUA FORM AKTIF 
                    CloseAllForm()
                    If USERNAME <> "" Then
                        LogOFF()
                    End If
                    Close()
                End If
            Case <> "EXIT"
                'PANGGIL FORM BY DATA BASE (T_ACCESRIGHT)
                'BATASI HANYA 1 FORM YANG DAPAT DI PANGGIL DI EVENT YANG SAMA
                'BANDINGKAN, JIKA ADA EXIT SELECT JIKA TIDAK LANJUT PANGGIL FORM
                'HASIL SIT PH 2 FORM ACCTIVE DI CLOSE SEBELUM NEW FORM SHOW
                '(HANYA 1 FORM AKTIF)
                '
                If singgeleForm = True Then CloseAllForm()
                'nFormName = UCase(Frm.ToString)
                If frmAktive <> "" Then
                    For Each Frmak As Form In Me.MdiChildren 'CHECK FORM AKTIF
                        If Frmak.Name = FormByName(GetFrmName(Frm)).Name Then
                            Exit Select
                        End If
                    Next
                    If IsNothing(FormByName(GetFrmName(Frm))) = False Then
                        FrmChildShow(FormByName(GetFrmName(Frm)))
                    End If
                    nFormName = UCase(Frm.ToString)
                End If
        End Select
    End Sub
    Public Function FormByName(strFormName As String) As Form
        On Error Resume Next
        'PRODUCTNAME ='PrjSWSTAP' Tidak Bisa Di ubah..
        'efek Form Tidak bisa di ambil..
        Return System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(ProductName & "." & strFormName)

    End Function
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        LabelControl7.Text = "Welcome, " & USERNAME & "," & Format(Now, "dd-MM-yyyy")
        ToolStripStatusLabel3.Text = "Date " & Format(Now, "dd-MM-yyyy") & "Time. " & Format(Now, "HH:MM:ss")

    End Sub

    Private Sub BunifuFlatButton3_Click(sender As Object, e As EventArgs) Handles BunifuFlatButton3.Click
        FrmChildShow(FrmConfigurationMenu)
        BunifuFlatButton3.Visible = False
    End Sub

    Sub CloseAllForm()
        For Each frm As Form In Me.MdiChildren
            frm.Close()
        Next
    End Sub

    Private Sub FrmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        LogOFF() 'KELUAR DARI LOCK
        CloseAllForm()
    End Sub

    Private Sub RemoveMenu()
        'GET GROUP
        Try
            NavBarControl1.BeginUpdate()
            Dim I, J As Integer
            For I = 0 To NavBarControl1.Groups.Count - 1
                Dim CurrGroup As NavBarGroup = NavBarControl1.Groups(I)
                ' perform some operations on a group here
                For J = 0 To CurrGroup.ItemLinks.Count - 1
                    Dim CurrLink As NavBarItemLink = CurrGroup.ItemLinks(J)
                    Dim IT As Integer
                    For IT = 0 To NavBarControl1.Items.Count - 1
                        Dim Item As NavBarItem = NavBarControl1.Items(IT)
                        ' perform some operations on an item here
                        If Item.Caption.ToString <> "EXIT" Then NavBarControl1.Items(IT).Visible = False  'BIKIN VISIBEL ITEM
                    Next
                    If CurrGroup.Caption.ToString <> "HOME" Then NavBarControl1.Groups(I).Visible = False 'BIKIN VISIBLE GROUP
                Next
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            NavBarControl1.EndUpdate()
        End Try
    End Sub
    Private Sub BunifuFlatButton1_Click(sender As Object, e As EventArgs) Handles BunifuFlatButton1.Click
        'LOGIN BUTON
        If BunifuFlatButton1.Text = "Sign Out" Then
            BunifuFlatButton1.Text = "Sign In"
            LabelControl7.Text = "Welcome"
            CloseAllForm()
            RemoveMenu()
            LogOFF()
            USERNAME = ""
            config = False
            Exit Sub
        End If

        If BunifuFlatButton1.Text = "Sign In" Then
            BunifuFlatButton3.Visible = False
            BunifuFlatButton1.Text = "Sign Out"
            FrmChildShow(FrmUserLogin)
        End If
    End Sub
    Private Sub FrmMain_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If config = True Then
            BunifuFlatButton3.Text = "Config"
            BunifuFlatButton3.Visible = True
        Else
            BunifuFlatButton3.Text = "Config"
            BunifuFlatButton3.Visible = False
        End If
    End Sub

    Private Sub FrmMain_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        If config = True Then
            BunifuFlatButton3.Text = "Config"
            BunifuFlatButton3.Visible = True
        Else
            BunifuFlatButton3.Text = "Config"
            BunifuFlatButton3.Visible = False
        End If
    End Sub
End Class