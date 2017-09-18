Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmCustomer
    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        TextEdit3.Enabled = True
        TextEdit4.Enabled = True
        TextEdit5.Enabled = True
        TextEdit6.Enabled = True
        TextEdit7.Enabled = True
        TextEdit8.Enabled = True

        TextEdit9.Enabled = True
        TextEdit10.Enabled = True
        TextEdit11.Enabled = True
        TextEdit12.Enabled = True
        TextEdit13.Enabled = True
        TextEdit14.Enabled = True
        TextEdit15.Enabled = True


    End Sub
    Private Sub lockAll()
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

    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"CUST_CODE", "CUST_NAME", "NPWP_CUST", "EMAIL_CUST", "ADDRESS_CUST", "POSTAL_CODE_CUST", "PHONE_CUST", "MOBILEPHONE_CUST", "FAX_CUST", "CONTACT_PERSON_CUST", "BANK_ACC_NO_CUST", "BANK_NAME_CUST", "ACCOUNTGROUP", "ACCOUNTGROUPNAME", "COUNTRY", "STATE_CUST"}

        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("CUSTOMER_NAME"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub clearinputCS()
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


        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
    End Sub
    Private Sub LoadView()
        SQL = ("SELECT * FROM T_CUSTOMER WHERE INACTIVE IS NULL ORDER BY CUST_CODE")
        GridControl1.DataSource = Nothing
        FILLGridView(SQL, GridControl1)
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE CUSTOMER
        If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit3, TextEdit4, TextEdit5, TextEdit6, TextEdit7, TextEdit8}) = True THEn
            Dim KDCUSTOMERCODE As String = TextEdit1.Text
            Dim CUSTOMERNAME As String = TextEdit2.Text
            Dim ADDRESS As String = TextEdit3.Text
            Dim PHONE As String = TextEdit4.Text
            Dim FAX As String = TextEdit5.Text
            Dim STATE As String = TextEdit6.Text
            Dim COUNTRY As String = TextEdit7.Text
            Dim POSTALCODE As String = TextEdit8.Text

            Dim CONTACTPERSON As String = TextEdit9.Text
            Dim MOBILEPHONE As String = TextEdit10.Text
            Dim EMAIL As String = TextEdit11.Text

            Dim ACCOUNTGROUP As String = ""
            Dim ACCOUNTGROUPN As String = TextEdit12.Text

            Dim BANKNAME As String = TextEdit13.Text
            Dim BANKACCOUNT As String = TextEdit14.Text

            Dim NPWP As String = TextEdit15.Text
            Dim STATUS As String = ""


            Dim INPUT_BY As String = USERNAME
            Dim INPUT_DATE As String = "SYSDATE"
            Dim UPDATE_BY As String = USERNAME
            Dim UPDATE_DATE As String = "SYSDATE"
            Dim INACTIVEDATE As String = "SYSDATE"
            Dim INACTIVE As String = "DEFAULT"

            SQL = " SELECT * FROM T_CUSTOMER WHERE CUST_CODE='" & TextEdit1.Text & "'"
            If CheckRecord(SQL) = 0 THEn
                SQL = "INSERT INTO T_CUSTOMER (CUST_CODE,CUST_NAME,NPWP_CUST,EMAIL_CUST,ADDRESS_CUST,POSTAL_CODE_CUST,PHONE_CUST,MOBILEPHONE_CUST,FAX_CUST,CONTACT_PERSON_CUST,BANK_ACC_NO_CUST,BANK_NAME_CUST,ACCOUNTGROUP,ACCOUNTGROUPNAME,COUNTRY,INPUT_BY,INPUT_DATE,INACTIVE,STATE_CUST,STATUS)" +
                      "VALUES ('" & KDCUSTOMERCODE & "','" & CUSTOMERNAME & "','" & NPWP & "','" & EMAIL & "','" & ADDRESS & "','" & POSTALCODE & "','" & PHONE & "','" & MOBILEPHONE & "','" & FAX & "','" & CONTACTPERSON & "','" & BANKACCOUNT & "','" & BANKNAME & "','" & ACCOUNTGROUP & "','" & ACCOUNTGROUPN & "','" & COUNTRY & "','" & USERNAME & "',SYSDATE,DEFAULT,'" & STATE & "','" & STATUS & "') "
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("SAVE SUCCESSFUL", vbInformation, "CUSTOMER")
                UnlockAll()
                clearinputCS()
            Else
                SQL = " SELECT * FROM T_CUSTOMER WHERE CUST_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                If CheckRecord(SQL) > 0 THEn
                    SQL = " UPDATE T_CUSTOMER SET CUST_NAME='" & CUSTOMERNAME & "',NPWP_CUST='" & NPWP & "',EMAIL_CUST='" & EMAIL & "',ADDRESS_CUST='" & ADDRESS & "',POSTAL_CODE_CUST='" & POSTALCODE & "',STATE_CUST='" & STATE & "',COUNTRY='" & COUNTRY & "',PHONE_CUST='" & PHONE & "',MOBILEPHONE_CUST='" & MOBILEPHONE & "',FAX_CUST='" & FAX & "', " +
                        " CONTACT_PERSON_CUST='" & CONTACTPERSON & "',BANK_ACC_NO_CUST ='" & BANKACCOUNT & "',BANK_NAME_CUST='" & BANKNAME & "',ACCOUNTGROUP='" & ACCOUNTGROUP & "',ACCOUNTGROUPNAME='" & ACCOUNTGROUPN & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,STATUS='" & STATUS & "' " +
                        " WHERE CUST_CODE='" & TextEdit1.Text & "' AND INACTIVE='X'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "CUSTOMER")
                    clearinputCS()
                End If
            End If
            If UCase(SimpleButton2.Text) = "UPDATE" THEn
                    SQL = " UPDATE T_CUSTOMER SET CUST_NAME='" & CUSTOMERNAME & "',NPWP_CUST='" & NPWP & "',EMAIL_CUST='" & EMAIL & "',ADDRESS_CUST='" & ADDRESS & "',POSTAL_CODE_CUST='" & POSTALCODE & "',STATE_CUST='" & STATE & "',COUNTRY='" & COUNTRY & "',PHONE_CUST='" & PHONE & "',MOBILEPHONE_CUST='" & MOBILEPHONE & "',FAX_CUST='" & FAX & "', " +
                            " CONTACT_PERSON_CUST='" & CONTACTPERSON & "',BANK_ACC_NO_CUST ='" & BANKACCOUNT & "',BANK_NAME_CUST='" & BANKNAME & "',ACCOUNTGROUP='" & ACCOUNTGROUP & "',ACCOUNTGROUPNAME='" & ACCOUNTGROUPN & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,STATUS='" & STATUS & "' " +
                            " WHERE CUST_CODE='" & TextEdit1.Text & "'"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("UPDATE SUCCESSFUL", vbInformation, "CUSTOMER")
                    clearinputCS()
                End If
            End If

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD CUSTOMER
        UnlockAll()
        TextEdit1.Text = ""
        TextEdit1.SELECT()

        SimpleButton1.Enabled = False
        SimpleButton2.Enabled = True
        SimpleButton3.Enabled = True

    End Sub
    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE CUSTOMER
        SQL = "UPDATE T_CUSTOMER SET INACTIVE='X',INACTIVE_DATE=SYSDATE WHERE CUST_CODE='" & TextEdit1.Text & "' AND INACTIVE IS NULL"
        ExecuteNonQuery(SQL)
        LoadView()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "CUSTOMER")
        clearinputCS()
    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL CUSTOMER
        clearinputCS()
        lockAll()
        SimpleButton2.Text = "Save" 'SAVE
        clearinputCS()

    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Me.Close()
    End Sub

    Private Sub FrrmCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "CUSTOMER"
        GridHeader()
        LoadView()
        lockAll()
        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl2.Enabled = False  'PANEL CRUDE
        End If
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "CUST_CODE").ToString()
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "CUST_NAME").ToString()
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "ADDRESS_CUST").ToString()
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "PHONE_CUST").ToString()
            TextEdit5.Text = GridView1.GetRowCellValue(e.RowHandle, "FAX_CUST").ToString()
            TextEdit6.Text = GridView1.GetRowCellValue(e.RowHandle, "STATE_CUST").ToString()
            TextEdit7.Text = GridView1.GetRowCellValue(e.RowHandle, "COUNTRY").ToString()
            TextEdit8.Text = GridView1.GetRowCellValue(e.RowHandle, "POSTAL_CODE_CUST").ToString()

            TextEdit9.Text = GridView1.GetRowCellValue(e.RowHandle, "CONTACT_PERSON_CUST").ToString()
            TextEdit10.Text = GridView1.GetRowCellValue(e.RowHandle, "MOBILEPHONE_CUST").ToString
            TextEdit11.Text = GridView1.GetRowCellValue(e.RowHandle, "EMAIL_CUST").ToString()
            TextEdit12.Text = GridView1.GetRowCellValue(e.RowHandle, "BANK_ACC_NO_CUST").ToString
            TextEdit13.Text = GridView1.GetRowCellValue(e.RowHandle, "BANK_NAME_CUST").ToString()
            TextEdit14.Text = GridView1.GetRowCellValue(e.RowHandle, "BANK_ACC_NO_CUST").ToString()
            TextEdit15.Text = GridView1.GetRowCellValue(e.RowHandle, "NPWP_CUST").ToString()

            UnlockAll()
            TextEdit1.Enabled = False
            SimpleButton2.Text = "Update"
        End If
    End Sub

    Private Sub FrmCustomer_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl1.Height = (Me.Height - 60) - (30 * 7)

    End Sub

    Private Sub TextEdit4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit4.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit5.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub

    Private Sub TextEdit8_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit8.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub

    Private Sub TextEdit10_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit10.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit14_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit14.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit15_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit15.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
End Class