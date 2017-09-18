Imports System.ComponentModel
Imports System.Threading
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraSplashScreen
Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Public Class FrmUserLogin
    Dim attempts As Integer = 1
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'cancel
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'Sign In
                If Not IsEmptyText(New DevExpress.XtraEditors.TextEdit() {TextEdit1, TextEdit2}) THEn
            If TextEdit1.Text = My.Settings.UserSetup And TextEdit2.Text = My.Settings.UserPass THEn
                config = True
                TblLogin = "Sign Out"
                Me.Close()
                USERNAME = TextEdit1.Text
                Exit Sub
            End If

            If CheckLogin(TextEdit1.Text, TextEdit2.Text) = True THEn
                If LogOn() = True THEn  'KUNCI USER 1 X LOGIN
                    TextEdit1.Text = ""
                    TextEdit2.Text = ""
                    FrmMain.BunifuFlatButton1.Text = "Sign In"
                    Close()
                Else
                    FrmMain.BunifuFlatButton1.Text = "Sign Out"
                    TextEdit1.Text = "" : TextEdit2.Text = ""
                    FrmMain.LabelControl7.Text = "Welcome, " & USERNAME
                    CreateMenu() 'BUAT MENU
                    Close()
                End If
            ElseIf attempts = 3 THEn
                MsgBox("Maximum count of retries(3),And you'reach THE maximum attempts!Try again later", MsgBoxStyle.Critical, "WARNING")
                Close()
            Else
                MsgBox("Username and Password is incorrect! re-enter again you currently have reached attempt " & attempts & " of 3.", MsgBoxStyle.Critical, "WARNING")
                attempts = attempts + 1
                TextEdit1.Text = ""
                TextEdit2.Text = ""
                TextEdit1.Focus()
            End If
        End If
    End Sub


    Private Sub FrmUserLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'LOADING 
        Me.Text = "USER LOGIN"
        TextEdit1.Text = "" : TextEdit2.Text = ""

    End Sub

    Private Sub CreateMenu()
        'CREATE NAVIBAR MENU
        'GET GROUP
        Try
            FrmMain.NavBarControl1.BeginUpdate()
            SQL = "SELECT DISTINCT URUT,ACCESSID,MENU FROM V_MENUBAR WHERE USERNAME='" & USERNAME & "' AND TYPE=0 ORDER BY URUT,ACCESSID"

            Dim DTG As DataTable = ExecuteQuery(SQL)
            Dim N As Integer = 0
            Dim DRG As New DataTableReader(DTG)

            While DRG.Read
                Dim header As New List(Of String)()
                header.Add(DRG("MENU").ToString())

                For Each hdr As String In header
                    Dim group As New NavBarGroup(hdr)
                    Dim hd As Integer = CType(GetImgGrp(hdr), Integer) 'ambil gambar dan isi
                    group.LargeImageIndex = 0
                    group.Expanded = False
                    'group.SmallImage = ImgGrp.Images(GetImgGrp(hdr))
                    group.SmallImage = FrmMain.ImgGrp.Images(hd)
                    'GET ITEM
                    SQL = "SELECT DISTINCT URUT,ACCESSID,MENU FROM V_MENUBAR WHERE USERNAME='" & USERNAME & "' AND GROUPMENU='" + hdr + "' ORDER BY URUT,ACCESSID ASC"
                    Dim DTSUB As DataTable = ExecuteQuery(SQL)
                    Dim I As Integer = 0
                    Dim DRSUB As New DataTableReader(DTSUB)

                    While DRSUB.Read
                        Dim JudulLists As New List(Of String)()
                        JudulLists.Add(DRSUB("MENU").ToString())

                        For Each Judul As String In JudulLists
                            Dim item As New NavBarItem(Judul.ToString())
                            'MsgBox(group.Caption & item.Caption)
                            FrmMain.NavBarControl1.Items.Add(item)
                            'item.SmallImage = ImageCollection1.Images(Judul)
                            group.ItemLinks.Add(item)
                        Next
                    End While

                    If group.ItemLinks.Count > 0 THEn
                        FrmMain.NavBarControl1.Groups.Add(group)
                    Else
                        FrmMain.NavBarControl1.Groups.Add(group)
                    End If
                Next

            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            FrmMain.NavBarControl1.EndUpdate()
        End Try
    End Sub
End Class