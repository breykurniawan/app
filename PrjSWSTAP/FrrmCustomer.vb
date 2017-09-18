Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrrmCustomer
    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit46.Enabled = True
        TextEdit44.Enabled = True
        TextEdit34.Enabled = True
        TextEdit45.Enabled = True
        TextEdit43.Enabled = True
        TextEdit42.Enabled = True
        TextEdit33.Enabled = True
        TextEdit29.Enabled = True
        TextEdit24.Enabled = True
        TextEdit32.Enabled = True
        TextEdit27.Enabled = True
        TextEdit26.Enabled = True
        TextEdit3.Enabled = True
        TextEdit93.Enabled = True
        TextEdit19.Enabled = True
        TextEdit4.Enabled = True
        ComboBoxEdit5.Enabled = True
        TextEdit91.Enabled = True

        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete
        SimpleButton4.Enabled = True 'cancel
        SimpleButton5.Enabled = True 'close
    End Sub
    Private Sub lockAll()
        TextEdit1.Enabled = False
        TextEdit46.Enabled = False
        TextEdit44.Enabled = False
        TextEdit34.Enabled = False
        TextEdit45.Enabled = False
        TextEdit43.Enabled = False
        TextEdit42.Enabled = False
        TextEdit33.Enabled = False
        TextEdit29.Enabled = False
        TextEdit24.Enabled = False
        TextEdit32.Enabled = False
        TextEdit27.Enabled = False
        TextEdit26.Enabled = False
        TextEdit3.Enabled = False
        TextEdit93.Enabled = False
        TextEdit19.Enabled = False
        TextEdit4.Enabled = False
        ComboBoxEdit5.Enabled = False
        TextEdit91.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
        SimpleButton4.Enabled = False 'cancel
        SimpleButton5.Enabled = False 'close
    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"CUSTOMER_CODE,CUSTOMER_NAME,NPWP,EMAIL,ADDRESS,POSTAL_CODE,PHONE,MOBILE_PHONE,FAX,CONTACT_PERSON,BANK_ACCOUNT,BANK_NAME,ACCOUNT_GROUP,ACCOUNT_GROUPN,COUNTRY,INPUT_BY,INPUT_DATE,UPDATE_BY,UPDATE_DATE,INACTIVE,INACTIVE_DATE,STATE,STATUS"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        Dim repItemGraphicsEdit As New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
        repItemGraphicsEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        repItemGraphicsEdit.BestFitWidth = 50
        view.Columns("IMAGE").ColumnEdit = repItemGraphicsEdit

        'GROUPING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("ROLENAME"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub LoadUser()
        SQL = "SELECT CUSTOMER_CODE,CUSTOMER_NAME,NPWP,EMAIL,ADDRESS,POSTAL_CODE,PHONE,MOBILE_PHONE,FAX,CONTACT_PERSON,BANK_ACCOUNT,BANK_NAME,ACCOUNT_GROUP,ACCOUNT_GROUPN,COUNTRY,INPUT_BY,INPUT_DATE,UPDATE_BY,UPDATE_DATE,INACTIVE,INACTIVE_DATE,STATE,STATUS" +
            "FROM T_CUSTOMER A" +
            "LEFT JOIN CUSTOMER_CODE B On A.CUSTOMER_CODE And B.aktif='y'" +
            "WHERE .AKTIF='Y'" +
            "ORDER BY CUSTOMER_CODE"
        FILLGridView(SQL, GridControl1)

        GridControl1.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub
    Private Sub clearinputCS()
        TextEdit1.Enabled = ""
        TextEdit46.Enabled = ""
        TextEdit44.Enabled = ""
        TextEdit34.Enabled = ""
        TextEdit45.Enabled = ""
        TextEdit43.Enabled = ""
        TextEdit42.Enabled = ""
        TextEdit33.Enabled = ""
        TextEdit29.Enabled = ""
        TextEdit24.Enabled = ""
        TextEdit32.Enabled = ""
        TextEdit27.Enabled = ""
        TextEdit26.Enabled = ""
        TextEdit3.Enabled = ""
        TextEdit93.Enabled = ""
        TextEdit19.Enabled = ""
        TextEdit4.Enabled = ""
        ComboBoxEdit5.Enabled = ""
        TextEdit91.Enabled = ""

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete
        SimpleButton4.Enabled = False 'cancel
        SimpleButton5.Enabled = False 'close
    End Sub
    Private Sub LoadView()
        SQL = ("SELECT CUSTCODE AS CUSTCODE,CUSTRNAME,NPWPCUST,EMAILCUST,ADDRESSCUST,POSTALCODECUST,STATECUST,COUNTRY,PHONECUST,MOBILEPHONECUST,FAXCUST,CONTACTPERSONCUST,BANKACCCUST,BANKNAMECUST,ACCOUNTGROUP,ACCOUNTGROUPNAME,INPUTBY,INPUTDATE,UPDATEBY,UPDATEDATE,INACTIVEDATE,ISACTIVE,STATUS FROM T_CUSTOMER ORDER BY CUSTCODE")
        GridControl1.DataSource = Nothing
        FILLGridView(SQL, GridControl1)
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE CUSTOMER
        If Not IsEmptyText({TextEdit1, TextEdit46, TextEdit44, TextEdit34, TextEdit45, TextEdit43, TextEdit42, TextEdit33, TextEdit29, TextEdit24, TextEdit32, TextEdit27, TextEdit26, TextEdit19, TextEdit4, TextEdit91}) = True Then
            If Not IsEmptyCombo({ComboBoxEdit5}) = True Then
                SQL = " SELECT * FROM T_CUSTOMER WHERE CUSTOMER_CODE=" & TextEdit1.Text & "'"
                Dim KDCUSTOMERCODE As String = TextEdit1.Text
                Dim CUSTOMERNAME As String = TextEdit46.Text
                Dim NPWP As String = TextEdit44.Text
                Dim EMAIL As String = TextEdit34.Text
                Dim ADDRESS As String = TextEdit45.Text
                Dim POSTALCODE As String = TextEdit43.Text
                Dim STATE As String = TextEdit42.Text
                Dim COUNTRY As String = TextEdit33.Text
                Dim PHONE As String = TextEdit29.Text
                Dim MOBILEPHONE As String = TextEdit24.Text
                Dim FAX As String = TextEdit32.Text
                Dim CONTACTPERSON As String = TextEdit27.Text
                Dim BANKACCOUNT As String = TextEdit26.Text
                Dim BANKNAME As String = TextEdit3.Text
                Dim ACCOUNTGROUP As String = TextEdit93.Text
                Dim ACCOUNTGROUPNAME As String = TextEdit19.Text
                Dim INPUT_BY As String = USERNAME
                Dim INPUT_DATE As String = Now
                Dim UPDATE_BY As String = USERNAME
                Dim UPDATE_DATE As String = Now
                Dim INACTIVEDATE As String = TextEdit4.Text
                Dim isactive As String = ComboBoxEdit5.SelectedItem
                Dim STATUS As String = TextEdit91.Text
                If CheckRecord(SQL) = 0 Then
                    SQL = "INSERT INTO T_CUSTOMER (CUSTOMER_CODE,CUSTOMER_NAME,NPWP,EMAIL,ADDRESS,POSTAL_CODE,PHONE,MOBILE_PHONE,FAX,CONTACT_PERSON,BANK_ACCOUNT,BANK_NAME,ACCOUNT_GROUP,ACCOUNT_GROUPN,COUNTRY,INPUT_BY,INPUT_DATE,UPDATE_BY,UPDATE_DATE,INACTIVE,INACTIVE_DATE,STATE,STATUS)" +
                    "VALUES ('" & KDCUSTOMERCODE & "','" & CUSTOMERNAME & "','" & NPWP & "','" & EMAIL & "','" & ADDRESS & "','" & POSTALCODE & "','" & STATE & "','" & COUNTRY & "','" & PHONE & "','" & MOBILEPHONE & "','" & FAX & "','" & cONTACTPERSON & "','" & BANKACCOUNT & "','" & BANKNAME & "','" & ACCOUNTGROUP & "','" & ACCOUNTGROUPNAME & "','" & INACTIVEDATE & "','" & isactive & "','" & STATUS & "','Y') "
                    ExecuteNonQuery(SQL)
                    MsgBox("Insert  Successful", vbInformation, "CUSTOMER")

                    If CheckRecord(SQL) > 0 Then UpdateCode("CS")
                    LoadUser()
                    MsgBox("SAVE SUCCEEDED", vbInformation, "CUSTOMER")
                    UnlockAll()
                    clearinputCS()
                Else
                    SQL = "UPDATE T_CUSTOMER SET CUSTOMER_CODE='" & KDCUSTOMERCODE & "',CUSTOMER_NAME='" & CUSTOMERNAME & "',NPWP='" & NPWP & "',EMAIL='" & EMAIL & "',ADDRESS='" & ADDRESS & "',POSTAL_CODE='" & POSTALCODE & "',STATE='" & STATE & "',COUNTRY='" & COUNTRY & "',PHONE='" & PHONE & "',MOBILE_PHONE='" & MOBILEPHONE & "',FAX='" & FAX & "', +
                CONTACT_PERSON='" & cONTACTPERSON & "',BANK_ACCOUNT='" & BANKACCOUNT & "',BANK_NAME='" & BANKNAME & "',ACCOUNT_GROUP='" & ACCOUNTGROUP & "',ACCOUNT_GROUPN='" & ACCOUNTGROUPNAME & "',INACTIVEDATE='" & INACTIVEDATE & "',ISACTIVE='" & isactive & "',STATUS='" & STATUS & "' " +
                " WHERE CUSTOMER_CODE='" & TextEdit1.Text & "'"
                    ExecuteNonQuery(SQL)
                    MsgBox("UPDATE SUCCEEDED", vbInformation, "CUSTOMER")
                End If
            End If
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD CUSTOMER
        UnlockAll()
        TextEdit1.Text = Val(Strings.Right(GetCode("CS"), 2))
        TextEdit1.Enabled = False
    End Sub
    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE CUSTOMER
        SQL = "UPDATE T_CUSTOMER SET AKTIF= 'N' WHERE CUSTOMER_CODE'" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadUser()
        MsgBox("Delete Successful", vbInformation, "CUSTOMER")
    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL CUSTOMER
        TextEdit1.Text = ""
        TextEdit46.Text = ""
        TextEdit44.Text = ""
        TextEdit34.Text = ""
        TextEdit45.Text = ""
        TextEdit43.Text = ""
        TextEdit42.Text = ""
        TextEdit33.Text = ""
        TextEdit29.Text = ""
        TextEdit24.Text = ""
        TextEdit32.Text = ""
        TextEdit27.Text = ""
        TextEdit26.Text = ""
        TextEdit3.Text = ""
        TextEdit93.Text = ""
        TextEdit19.Text = ""
        TextEdit4.Text = ""
        ComboBoxEdit5.SelectedItem = ""
        TextEdit91.Text = ""
        clearinputCS()
        lockAll()
        SimpleButton2.Text = "SAVE" 'SAVE
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'CLOSE
        Me.Close()
    End Sub

    Private Sub FrrmCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "CUSTOMER"
        GridHeader()
        LoadUser()
        lockAll()
    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        If e.RowHandle < 0 Then
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
            SimpleButton4.Enabled = True 'cancel
            SimpleButton5.Enabled = False 'close
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete
            SimpleButton4.Enabled = True 'cancel
            SimpleButton5.Enabled = False 'close

            SimpleButton1.Text = "update" 'save

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "CUSTOMER_CODE").ToString()
            TextEdit46.Text = GridView1.GetRowCellValue(e.RowHandle, "CUSTOMER_NAME").ToString()
            TextEdit44.Text = GridView1.GetRowCellValue(e.RowHandle, "NPWP").ToString()
            TextEdit34.Text = GridView1.GetRowCellValue(e.RowHandle, "EMAIL").ToString()
            TextEdit45.Text = GridView1.GetRowCellValue(e.RowHandle, "ADDRESS").ToString()
            TextEdit43.Text = GridView1.GetRowCellValue(e.RowHandle, "POSTAL_CODE").ToString()
            TextEdit42.Text = GridView1.GetRowCellValue(e.RowHandle, "STATE").ToString()
            TextEdit33.Text = GridView1.GetRowCellValue(e.RowHandle, "COUNTRY").ToString()
            TextEdit29.Text = GridView1.GetRowCellValue(e.RowHandle, "PHONE").ToString()
            TextEdit24.Text = GridView1.GetRowCellValue(e.RowHandle, "MOBILE_PHONE").ToString
            TextEdit32.Text = GridView1.GetRowCellValue(e.RowHandle, "FAX").ToString()
            TextEdit27.Text = GridView1.GetRowCellValue(e.RowHandle, "CONTACT_PERSON").ToString()
            TextEdit26.Text = GridView1.GetRowCellValue(e.RowHandle, "BANK_ACCOUNT").ToString
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "BANK_NAME").ToString()
            TextEdit93.Text = GridView1.GetRowCellValue(e.RowHandle, "ACCOUNT_GROUP").ToString()
            TextEdit19.Text = GridView1.GetRowCellValue(e.RowHandle, "ACCOUNT_GROUPN").ToString
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "IACTIVE_DATE").ToString
            ComboBoxEdit5.SelectedItem = GridView1.GetRowCellValue(e.RowHandle, "ISACTIVE").ToString
            TextEdit91.Text = GridView1.GetRowCellValue(e.RowHandle, "STATUS").ToString

            TextEdit1.Enabled = True
            UnlockAll()
        End If
    End Sub
End Class