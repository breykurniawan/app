Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmSalesOrder
    Private Sub FrmSalesOrder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "SALES ORDER"
        GridHeader()
        LoadMaterial()
        LoadCustomer()
        LoadView()
        LockAll()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl5.Enabled = False  'PANEL CRUDE
        End If
    End Sub

    Private Sub LoadMaterial()
        SQL = "SELECT MATERIAL_CODE,MATERIAL_NAME FROM T_MATERIAL WHERE INACTIVE<>'X' "
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit2, False)
    End Sub
    Private Sub LoadCustomer()
        SQL = "SELECT CUST_CODE,CUST_NAME FROM T_CUSTOMER WHERE INACTIVE<>'X'  "
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit1, False)
    End Sub
    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        DateEdit1.Enabled = True
        DateEdit2.Enabled = True
        ComboBoxEdit1.Enabled = True
        TextEdit3.Enabled = True
        TextEdit5.Enabled = True
        TextEdit6.Enabled = True
        TextEdit7.Enabled = True
        TextEdit8.Enabled = True
        ComboBoxEdit2.Enabled = True
        TextEdit9.Enabled = True
        CheckEdit1.Enabled = True


        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete

    End Sub
    Private Sub LockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        DateEdit1.Enabled = False
        DateEdit2.Enabled = False
        ComboBoxEdit1.Enabled = False
        TextEdit3.Enabled = False
        TextEdit5.Enabled = False
        TextEdit6.Enabled = False
        TextEdit7.Enabled = False
        TextEdit8.Enabled = False
        ComboBoxEdit2.Enabled = False
        TextEdit9.Enabled = False
        CheckEdit1.Enabled = False


        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

        SimpleButton2.Text = "Save"
    End Sub
    Private Sub ClearInputSO()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        DateEdit1.Text = ""
        DateEdit2.Text = ""
        ComboBoxEdit1.Text = ""
        TextEdit3.Text = ""
        TextEdit5.Text = ""
        TextEdit6.Text = ""
        TextEdit7.Text = ""
        TextEdit8.Text = ""
        ComboBoxEdit2.Text = ""
        TextEdit9.Text = ""
        CheckEdit1.Checked = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"SO_NO", "SO_QTY", "CUST_NAME", "INCOTERMS1", "INCOTERMS2", "SALDO", "ITEMNO", "MATERIAL_NAME", "SC_NO", "TOLERANCE", "SO_START", "SO_END", "COMPLATE"}
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
        New GridColumnSortInfo(GridView.Columns("CUST_NAME"), DevExpress.Data.ColumnSortOrder.Ascending),
        New GridColumnSortInfo(GridView.Columns("SO_NO"), DevExpress.Data.ColumnSortOrder.Ascending)
        }, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()

    End Sub
    Private Sub LoadView()
        SQL = "SELECT  A.SO_NO,A.SC_NO,A.CUST_CODE,C.CUST_NAME,A.INCOTERMS1,A.INCOTERMS2,A.SALDO,A.STATUS,A.ITEMNO, " +
        " A.MATERIAL_CODE,B.MATERIAL_NAME,A.SO_QTY,A.TOLERANCE, " +
        " A.SO_START,A.SO_END,A.COMPLATE,A.INACTIVE " +
        " FROM T_SALESORDER A " +
        " LEFT JOIN T_MATERIAL B On B.MATERIAL_CODE=A.MATERIAL_CODE " +
        " LEFT JOIN T_CUSTOMER C On C.CUST_CODE=A.CUST_CODE " +
        " WHERE A.INACTIVE<>'X'"
        GridControl1.DataSource = Nothing
        FILLGridView(SQL, GridControl1)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()

    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'SAVE
        If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit3, TextEdit5, TextEdit6, TextEdit7, TextEdit8, TextEdit9}) = True THEn
            If Not IsEmptyCombo({ComboBoxEdit1, ComboBoxEdit2}) THEn

                Dim SONUMBER As String = TextEdit1.Text
                Dim SOQUANTITY As String = TextEdit2.Text
                Dim SOSTARTDATE As String = FrmDate(DateEdit1, "dd/MM/yyyy")
                Dim SOENDDATE As String = FrmDate(DateEdit2, "dd/MM/yyyy")
                Dim CUSTCODE As String = GetCodeCust(ComboBoxEdit1.Text)
                Dim SALDO As String = TextEdit3.Text
                Dim STATUS As String = ""
                Dim INCOTERMS1 As String = TextEdit5.Text
                Dim INCOTERMS2 As String = TextEdit6.Text
                Dim ITEMNO As String = TextEdit7.Text
                Dim SCNUMBER As String = TextEdit8.Text
                Dim MATERIALCODE As String = GetCodeMaterial(ComboBoxEdit2.Text)
                Dim TOLERANCE As String = TextEdit9.Text
                Dim COMPLETE As String = "N"
                If CheckEdit1.Checked = True THEn COMPLETE = "Y"

                SQL = "SELECT * FROM T_SALESORDER WHERE SO_NO='" & TextEdit1.Text & "'"
                If CheckRecord(SQL) = 0 THEn
                    SQL = "INSERT INTO T_SALESORDER (SO_NO,SO_QTY,SO_START,SO_END,CUST_CODE,SALDO,INCOTERMS1,INCOTERMS2,ITEMNO,SC_NO,MATERIAL_CODE,TOLERANCE,INPUT_BY,INPUT_DATE,INACTIVE,COMPLATE)" +
                    " VALUES('" & SONUMBER & "','" & SOQUANTITY & "'," & SOSTARTDATE & "," & SOENDDATE & ",'" & CUSTCODE & "','" & SALDO & "','" & INCOTERMS1 & "','" & INCOTERMS2 & "','" & ITEMNO & "','" & SCNUMBER & "','" & MATERIALCODE & "','" & TOLERANCE & "','" & USERNAME & "',SYSDATE,'N','" & COMPLETE & "')"
                    ExecuteNonQuery(SQL)
                    LoadView()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "SALES ORDER")
                    UnlockAll()
                    ClearInputSO()
                Else
                    SQL = "SELECT * FROM T_SALESORDER WHERE SO_NO='" & TextEdit1.Text & "' AND INACTIVE='X'"
                    If CheckRecord(SQL) > 0 THEn
                        SQL = "UPDATE T_SALESORDER SET SO_QTY='" & SOQUANTITY & "',SO_START=" & SOSTARTDATE & ",SO_END=" & SOENDDATE & ",CUST_CODE='" & CUSTCODE & "',SALDO ='" & SALDO & "',STATUS='" & STATUS & "',INCOTERMS1='" & INCOTERMS1 & "',INCOTERMS2='" & INCOTERMS2 & "',ITEMNO='" & ITEMNO & "',SC_NO='" & SCNUMBER & "',MATERIAL_CODE='" & MATERIALCODE & "',TOLERANCE='" & TOLERANCE & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,COMPLATE='" & COMPLETE & "'" +
                          " WHERE SO_NO= '" & TextEdit1.Text & "' AND INACTIVE='X'"
                        ExecuteNonQuery(SQL)
                        LoadView()
                        MsgBox("SAVE SUCCESSFUL", vbInformation, "SALES ORDER")
                        UnlockAll()
                        ClearInputSO()
                    End If
                End If

                SQL = "UPDATE T_SALESORDER SET SO_QTY='" & SOQUANTITY & "',SO_START=" & SOSTARTDATE & ",SO_END=" & SOENDDATE & ",CUST_CODE='" & CUSTCODE & "',SALDO ='" & SALDO & "',STATUS='" & STATUS & "',INCOTERMS1='" & INCOTERMS1 & "',INCOTERMS2='" & INCOTERMS2 & "',ITEMNO='" & ITEMNO & "',SC_NO='" & SCNUMBER & "',MATERIAL_CODE='" & MATERIALCODE & "',TOLERANCE='" & TOLERANCE & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,COMPLATE='" & COMPLETE & "'" +
                      " WHERE SO_NO= '" & TextEdit1.Text & "'"
                ExecuteNonQuery(SQL)
                LoadView()
                MsgBox("UPDATE SUCCESSFUL", vbInformation, "SALES ORDER")
                UnlockAll()
                ClearInputSO()
            End If
            End If

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'add
        UnlockAll()
        TextEdit1.SELECT()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE
        SQL = "UPDATE T_SALESORDER SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE SO_NO='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadView()
        ClearInputSO()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "SALES ORDER")
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        ClearInputSO()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
    End Sub
    Private Sub GridView6_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView6.RowCellClick
        If e.RowHandle < 0 THEn
            SimpleButton1.Enabled = True 'add
            SimpleButton2.Enabled = False 'save
            SimpleButton3.Enabled = False 'delete
        Else
            SimpleButton1.Enabled = False 'add
            SimpleButton2.Enabled = True 'save
            SimpleButton3.Enabled = True 'delete

            SimpleButton2.Text = "Update" 'SAVE



            TextEdit1.Text = GridView6.GetRowCellValue(e.RowHandle, "SO_NO").ToString() 'SONUMBER
            TextEdit2.Text = GridView6.GetRowCellValue(e.RowHandle, "SO_QTY").ToString() 'SOQUANTITY
            Dim SO_START As DateTime = GridView6.GetRowCellValue(e.RowHandle, "SO_START").ToString()
            DateEdit1.Text = SO_START.ToString("M/d/yyyy")
            Dim SO_END As DateTime = GridView6.GetRowCellValue(e.RowHandle, "SO_END").ToString()
            DateEdit2.Text = SO_END.ToString("M/d/yyyy")
            ComboBoxEdit1.Text = GridView6.GetRowCellValue(e.RowHandle, "CUST_NAME").ToString() 'CUSTOMERCODE
            TextEdit3.Text = GridView6.GetRowCellValue(e.RowHandle, "SALDO").ToString() 'CUSTOMERNAME
            TextEdit5.Text = GridView6.GetRowCellValue(e.RowHandle, "INCOTERMS1").ToString() 'SALDO
            TextEdit6.Text = GridView6.GetRowCellValue(e.RowHandle, "INCOTERMS2").ToString() 'INCOTERM1
            TextEdit7.Text = GridView6.GetRowCellValue(e.RowHandle, "ITEMNO").ToString() 'INCOTERMS2

            TextEdit8.Text = GridView6.GetRowCellValue(e.RowHandle, "SC_NO").ToString() 'ITEMNO
            ComboBoxEdit2.SELECTedItem = GridView6.GetRowCellValue(e.RowHandle, "MATERIAL_NAME").ToString() 'MATERIAL
            TextEdit9.Text = GridView6.GetRowCellValue(e.RowHandle, "TOLERANCE").ToString()  'INACTIVEDATE
            If GridView6.GetRowCellValue(e.RowHandle, "COMPLATE").ToString() = "N" THEn
                CheckEdit1.Checked = False   'COMPLETE
            Else
                CheckEdit1.Checked = True   'COMPLETE
            End If

            UnlockAll()
            TextEdit1.Enabled = False
        End If
    End Sub

    Private Sub FrmSalesOrder_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl3.Height = (Me.Height - 60) - (30 * 6.3)
    End Sub

    Private Sub TextEdit2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit2.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit3.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit7.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit8_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit8.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
    Private Sub TextEdit9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit9.KeyPress
        'phone
        e.Handled = NumericOnly(e)
    End Sub
End Class