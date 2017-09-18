Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions ' Namespace untuk manipulasi registry
Imports System.Text

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common

Imports DevExpress
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns

Public Class FrmVendorr

    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        TextEdit3.Enabled = True
        TextEdit4.Enabled = True
        Textedit5.Enabled = True
        Textedit6.Enabled = True
        TextEdit7.Enabled = True
        TextEdit8.Enabled = True
        TextEdit9.Enabled = True
        TextEdit10.Enabled = True
        TextEdit11.Enabled = True
        TextEdit12.Enabled = True
        TextEdit13.Enabled = True
        TextEdit14.Enabled = True
        TextEdit15.Enabled = True
        ComboBoxEdit1.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete

    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False

        TextEdit3.Enabled = False
        TextEdit4.Enabled = False
        TextEdit5.Enabled = False
        TextEdit6.Enabled = False
        TextEdit7.Enabled = False
        TextEdit8.Enabled = False
        TextEdit9.Enabled = False
        TextEdit10.Enabled = False
        TextEdit11.Enabled = False
        TextEdit12.Enabled = False
        TextEdit13.Enabled = False
        TextEdit14.Enabled = False
        TextEdit15.Enabled = False
        ComboBoxEdit1.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub

    Private Sub FrmCustomer_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 8)
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE VENDOR
        If Not IsEmptyText({TextEdit1, TextEdit2}) = True Then
            If Not IsEmptyCombo({ComboBoxEdit1}) = True Then
                Dim VENCODE As String = TextEdit1.Text
                Dim VENNAME As String = TextEdit2.Text
                Dim NPWP As String = TextEdit3.Text
                Dim EMAIL As String = TextEdit4.Text
                Dim ADDRESS As String = TextEdit5.Text
                Dim POSTCODE As String = TextEdit6.Text
                Dim STATE As String = TextEdit7.Text
                Dim COUNTRY As String = TextEdit8.Text
                Dim PHONE As String = TextEdit9.Text
                Dim MPHONE As String = TextEdit12.Text
                Dim FAX As String = TextEdit10.Text
                Dim CP As String = TextEdit11.Text
                Dim BANKACC As String = TextEdit13.Text
                Dim ACCOUNTGROUP As String = TextEdit14.Text
                Dim ACCOUNTGROUPN As String = TextEdit15.Text
                Dim INPUT_BY As String = USERNAME
                Dim INPUT_DATE As String = Now
                Dim UPDATE_BY As String = USERNAME
                Dim UPDATE_DATE As String = Now

                Dim INACTIVEDATE As String = Now
                Dim INACTIVE As String = "N"
                Dim STATUS As String = "N"
                Dim INTER As String = ComboBoxEdit1.Text

                SQL = "SELECT * FROM T_VENDOR WHERE VENDOR_CODE='" & TextEdit1.Text & "'"
                If CheckRecord(SQL) = 0 Then
                    SQL = " INSERT INTO T_VENDOR (VENDOR_CODE,VENDOR_NAME,INTERNAL,NPWP,EMAIL,ADDRESS,POSTAL_CODE,STATE,COUNTRY,PHONE,MOBILEPHONE,FAX,CONTACTPERSON,BANKACCOUNT,ACCOUNTGROUP,ACCOUNTGROUPN,INPUTBY,INPUTDATE,INACTIVE,STATUS)" +
                      " VALUES('" & VENCODE & "','" & VENNAME & "','" & INTER & "','" & NPWP & "','" & EMAIL & "','" & ADDRESS & "','" & POSTCODE & "','" & STATE & "','" & COUNTRY & "','" & PHONE & "','" & MPHONE & "','" & FAX & "','" & CP & "','" & BANKACC & "','" & ACCOUNTGROUP & "','" & ACCOUNTGROUPN & "','" & USERNAME & "',SYSDATE,'" & INACTIVE & "','" & STATUS & "')"
                    ExecuteNonQuery(SQL)
                    LoadUser()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "VENDOR")
                    UnlockAll()
                    ClearInputVN()
                Else
                    SQL = "SELECT * FROM T_VENDOR WHERE VENDOR_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                    If CheckRecord(SQL) > 0 Then
                        SQL = "UPDATE T_VENDOR SET VENDOR_NAME='" & VENNAME & "',INTERNAL='" & INTER & "',NPWP='" & NPWP & "',EMAIL='" & EMAIL & "',ADDRESS='" & ADDRESS & "',POSTAL_CODE='" & POSTCODE & "',STATE='" & STATE & "',COUNTRY='" & COUNTRY & "',PHONE='" & PHONE & "',MOBILEPHONE='" & MPHONE & "',FAX='" & FAX & "',CONTACTPERSON='" & CP & "',BANKACCOUNT='" & BANKACC & "',ACCOUNTGROUP='" & ACCOUNTGROUP & "',ACCOUNTGROUPN='" & ACCOUNTGROUPN & "',UPDATEDATE=SYSDATE,UPDATEBY='" & USERNAME & "',STATUS='" & STATUS & "' ,INACTIVE='N'" +
                       " WHERE VENDOR_CODE= '" & TextEdit1.Text & "' AND INACTIVE='X'"
                        ExecuteNonQuery(SQL)
                        LoadUser()
                        MsgBox("SAVE SUCCESSFUL", vbInformation, "VENDOR")
                        ClearInputVN()
                    End If
                End If
                If UCase(SimpleButton2.Text) = "UPDATE" Then
                    SQL = "UPDATE T_VENDOR SET VENDOR_NAME='" & VENNAME & "',INTERNAL='" & INTER & "',NPWP='" & NPWP & "',EMAIL='" & EMAIL & "',ADDRESS='" & ADDRESS & "',POSTAL_CODE='" & POSTCODE & "',STATE='" & STATE & "',COUNTRY='" & COUNTRY & "',PHONE='" & PHONE & "',MOBILEPHONE='" & MPHONE & "',FAX='" & FAX & "',CONTACTPERSON='" & CP & "',BANKACCOUNT='" & BANKACC & "',ACCOUNTGROUP='" & ACCOUNTGROUP & "',ACCOUNTGROUPN='" & ACCOUNTGROUPN & "',UPDATEDATE=SYSDATE,UPDATEBY='" & USERNAME & "',STATUS='" & STATUS & "'" +
                       " WHERE VENDOR_CODE= '" & TextEdit1.Text & "'"
                    ExecuteNonQuery(SQL)
                    LoadUser()
                    MsgBox("UPDATE SUCCESSFUL", vbInformation, "VENDOR")
                    ClearInputVN()
                End If
            End If
        End If
    End Sub
    Private Sub ClearInputVN()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        TextEdit5.Text = ""
        TextEdit6.Text = ""
        TextEdit7.Text = ""
        TextEdit8.Text = ""
        TextEdit9.Text = ""
        TextEdit10.Text = ""
        TextEdit11.Text = ""
        TextEdit12.Text = ""
        TextEdit13.Text = ""
        TextEdit14.Text = ""
        TextEdit15.Text = ""
        ComboBoxEdit1.Text = ""

        SimpleButton2.Text = "Save"

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"VENDOR_CODE", "VENDOR_NAME", "NPWP", "EMAIL", "ADDRESS", "POSTAL_CODE", "STATE", "COUNTRY", "INTERNAL", "PHONE", "MOBILEPHONNE", "FAX", "CONTACTPERSON", "BANKACCOUNT", "ACCOUNTGROUP", "ACCOUNTGROUPN"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next
        GridView1.BestFitColumns()
        GridView1.ExpandAllGroups()
    End Sub
    Private Sub LoadUser()
        SQL = "SELECT   VENDOR_CODE,  VENDOR_NAME,INTERNAL  ,NPWP,  EMAIL,  ADDRESS,  POSTAL_CODE,  STATE,  COUNTRY,  PHONE,  MOBILEPHONE,FAX,  CONTACTPERSON,  BANKACCOUNT,  ACCOUNTGROUP,  ACCOUNTGROUPN,STATUS  " +
            " FROM T_VENDOR " +
            " WHERE INACTIVE <> 'X' " +
            " ORDER BY VENDOR_NAME "
        FILLGridView(SQL, GridControl1)
        GridControl1.DataSource = ExecuteQuery(SQL)
        GridView1.BestFitColumns()
        GridView1.ExpandAllGroups()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        UnlockAll()
        TextEdit1.Text = "" 'FREE CODE
        TextEdit1.Select()
    End Sub
    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE
        SQL = "UPDATE T_VENDOR SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE VENDOR_CODE='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadUser()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "VENDOR")
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        TextEdit5.Text = ""
        TextEdit6.Text = ""
        TextEdit7.Text = ""
        TextEdit8.Text = ""
        TextEdit9.Text = ""
        TextEdit10.Text = ""
        TextEdit11.Text = ""
        TextEdit12.Text = ""
        TextEdit13.Text = ""
        TextEdit14.Text = ""
        TextEdit15.Text = ""
        ComboBoxEdit1.Text = ""
        LockAll()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub
    Private Sub FrmVendorr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "VENDOR"
        GridHeader()
        LoadUser()
        ClearInputVN()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite() = True Or SAP = "" Then
            PanelControl3.Enabled = False  'PANEL CRUDE
        End If
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub
    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 Then
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete

        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "VENDOR_CODE").ToString()
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "VENDOR_NAME").ToString()
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "NPWP").ToString()
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "EMAIL").ToString()
            TextEdit5.Text = GridView1.GetRowCellValue(e.RowHandle, "ADDRESS").ToString()
            TextEdit6.Text = GridView1.GetRowCellValue(e.RowHandle, "POSTAL_CODE").ToString()
            TextEdit7.Text = GridView1.GetRowCellValue(e.RowHandle, "STATE").ToString()
            TextEdit8.Text = GridView1.GetRowCellValue(e.RowHandle, "COUNTRY").ToString()
            TextEdit9.Text = GridView1.GetRowCellValue(e.RowHandle, "PHONE").ToString()
            TextEdit11.Text = GridView1.GetRowCellValue(e.RowHandle, "CONTACTPERSON").ToString()
            TextEdit10.Text = GridView1.GetRowCellValue(e.RowHandle, "FAX").ToString()
            TextEdit12.Text = GridView1.GetRowCellValue(e.RowHandle, "MOBILEPHONE").ToString()
            TextEdit13.Text = GridView1.GetRowCellValue(e.RowHandle, "BANKACCOUNT").ToString()
            TextEdit14.Text = GridView1.GetRowCellValue(e.RowHandle, "ACCOUNTGROUP").ToString()
            TextEdit15.Text = GridView1.GetRowCellValue(e.RowHandle, "ACCOUNTGROUPN").ToString()
            ComboBoxEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "INTERNAL").ToString()

            UnlockAll()
            TextEdit1.Enabled = False

        End If
    End Sub

    Private Sub TextEdit3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit3.KeyPress
        'NPWP
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit6.KeyPress
        'CODE POST
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit9.KeyPress
        'PHONE 
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit12_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit12.KeyPress
        'M PHONE
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit13_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit13.KeyPress
        'bANK aCCOUNT
        e.Handled = NumericOnly(e)
    End Sub
End Class
