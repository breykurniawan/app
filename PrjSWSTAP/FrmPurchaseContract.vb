Imports System.IO

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports Devart.Data
Imports Devart.Data.Oracle
Imports Devart.Common
Public Class FrmPurchaseContract

    Private Sub FrmPurchaseContract_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "PURCHASE CONTRACT"
        LoadUser()
        GridHeader()
        lockAll()
        LoadMaterial()
        LoadVendor()
        'MODUL INI DI PAKAI BUAT CHECK STATUS SITE SAP/BUKAN
        If StatusSite = True Or SAP = "" THEn
            PanelControl3.Enabled = False  'PANEL CRUDE
        End If
    End Sub
    Private Sub LoadMaterial()
        SQL = "SELECT MATERIAL_CODE,MATERIAL_NAME FROM T_MATERIAL WHERE INACTIVE <>'X'"
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit2, False)
    End Sub
    Private Sub LoadVendor()
        SQL = "SELECT VENDOR_CODE,VENDOR_NAME FROM T_VENDOR WHERE INACTIVE <> 'X' "
        FILLComboBoxEdit(SQL, 1, ComboBoxEdit1, False)
    End Sub


    Private Sub UnlockAll()
        TextEdit1.Enabled = True
        TextEdit2.Enabled = True
        DateEdit1.Enabled = True
        DateEdit2.Enabled = True
        ComboBoxEdit1.Enabled = True
        TextEdit3.Enabled = True
        TextEdit4.Enabled = True
        TextEdit5.Enabled = True
        ComboBoxEdit2.Enabled = True
        TextEdit6.Enabled = True
        TextEdit8.Enabled = True


        SimpleButton1.Enabled = False 'add
        SimpleButton2.Enabled = True 'save
        SimpleButton3.Enabled = True 'delete
    End Sub
    Private Sub lockAll()
        TextEdit1.Enabled = False
        TextEdit2.Enabled = False
        DateEdit1.Enabled = False
        DateEdit2.Enabled = False
        ComboBoxEdit1.Enabled = False
        TextEdit3.Enabled = False
        TextEdit4.Enabled = False
        TextEdit5.Enabled = False
        ComboBoxEdit2.Enabled = False
        TextEdit6.Enabled = False
        TextEdit8.Enabled = False

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

    End Sub
    Private Sub ClearInputPC()
        TextEdit1.Text = ""
        TextEdit2.Text = ""
        DateEdit1.Text = ""
        DateEdit2.Text = ""
        ComboBoxEdit1.Text = ""
        TextEdit3.Text = ""
        TextEdit4.Text = ""
        TextEdit5.Text = ""
        ComboBoxEdit2.Text = ""
        TextEdit6.Text = ""
        TextEdit8.Text = ""

        SimpleButton1.Enabled = True 'add
        SimpleButton2.Enabled = False 'save
        SimpleButton3.Enabled = False 'delete

        SimpleButton2.Text = "Save"
    End Sub
    Private Sub GridHeader()
        Dim view As ColumnView = CType(GridControl1.MainView, ColumnView)
        Dim fieldNames() As String = New String() {"CONTRACT_NUMBER", "DOCUMENT_TYPE", "CONTRACT_STARTDATE", "CONTRACT_ENDDATE", "VENDOR_NAME", "INCOTERMS1", "INCOTERMS2", "ITEMNO", "MATERIAL_NAME", "FLATE_RATE", "GRADING"}
        Dim I As Integer
        Dim Column As DevExpress.XtraGrid.Columns.GridColumn

        view.Columns.Clear()
        For I = 0 To fieldNames.Length - 1
            Column = view.Columns.AddField(fieldNames(I))
            Column.VisibleIndex = I
        Next

        'GROUPING & SORTING
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.SortInfo.ClearAndAddRange(New GridColumnSortInfo() {
        New GridColumnSortInfo(GridView.Columns("VENDOR_CODE"), DevExpress.Data.ColumnSortOrder.Ascending)}, 1)
        GridView.BestFitColumns()
        GridView.ExpandAllGroups()
    End Sub
    Private Sub LoadUser()
        SQL = "SELECT A.CONTRACT_NUMBER ,A.DOCUMENT_TYPE,A.CONTRACT_STARTDATE,A.CONTRACT_ENDDATE,B.VENDOR_NAME,A.INCOTERMS1,A.INCOTERMS2,ITEMNO,C.MATERIAL_NAME,A.FLATE_RATE,A.STATUS,A.GRADING " +
            " FROM T_PURCHASECONTRACT A " +
            " LEFT JOIN T_VENDOR B ON B.VENDOR_CODE=A.VENDOR_CODE " +
            " LEFT JOIN T_MATERIAL C ON C.MATERIAL_CODE=A.MATERIALCODE  " +
            " WHERE A.INACTIVE <>'X' " +
            " ORDER BY A.CONTRACT_NUMBER"
        FILLGridView(SQL, GridControl1)
        GridControl1.DataSource = ExecuteQuery(SQL)
        Dim GridView As GridView = CType(GridControl1.FocusedView, GridView)
        GridView.ExpandAllGroups()
    End Sub
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        'save purchase contract
        If Not IsEmptyText({TextEdit1, TextEdit2, TextEdit2, TextEdit5, TextEdit6, TextEdit8}) THEn
            If Not IsEmptyCombo({ComboBoxEdit1, ComboBoxEdit2}) THEn
                Dim CON_NO As String = TextEdit1.Text
                Dim DOC_TYPE As String = TextEdit2.Text
                Dim CON_START As String = FrmDate(DateEdit1, "dd/MM/yyyy")
                Dim CON_END As String = FrmDate(DateEdit2, "dd/MM/yyyy")
                Dim VENDOR_CODE As String = GetCodeVendor(ComboBoxEdit1.Text)
                Dim IN1 As String = TextEdit3.Text
                Dim IN2 As String = TextEdit4.Text
                Dim ITEMNO As String = TextEdit5.Text
                Dim MAT_CODE As String = GetCodeMaterial(ComboBoxEdit2.Text)
                Dim FLRATE As String = TextEdit6.Text
                Dim GRADING As String = TextEdit8.Text

                SQL = "SELECT * FROM T_PURCHASECONTRACT WHERE CONTRACT_NUMBER='" & TextEdit1.Text & "' "
                If CheckRecord(SQL) = 0 THEn
                    SQL = " INSERT INTO T_PURCHASECONTRACT (CONTRACT_NUMBER,DOCUMENT_TYPE,CONTRACT_STARTDATE,CONTRACT_ENDDATE,VENDOR_CODE, " +
                        " INCOTERMS1,INCOTERMS2,ITEMNO,MATERIALCODE,FLATE_RATE,GRADING,STATUS,INACTIVE,INPUT_BY,INPUT_DATE) " +
                        " VALUES ('" & CON_NO & "','" & DOC_TYPE & "'," & CON_START & "," & CON_END & ",'" & VENDOR_CODE & "', " +
                        " '" & IN1 & "','" & IN2 & "','" & ITEMNO & "','" & MAT_CODE & "','" & FLRATE & "','" & GRADING & "','','N','" & USERNAME & "',SYSDATE) "
                    ExecuteNonQuery(SQL)
                    LoadUser()
                    MsgBox("SAVE SUCCESSFUL", vbInformation, "PURCHASE CONTRACT")
                    UnlockAll()
                    ClearInputPC()
                Else
                    SQL = "SELECT * FROM T_PURCHASECONTRACT WHERE CONTRACT_NUMBER='" & TextEdit1.Text & "' AND INACTIVE='X' "
                    If CheckRecord(SQL) > 0 THEn
                        SQL = "UPDATE T_PURCHASECONTRACT SET DOCUMENT_TYPE='" & DOC_TYPE & "',CONTRACT_STARTDATE=" & CON_START & ",CONTRACT_ENDDATE=" & CON_END & ",VENDOR_CODE='" & VENDOR_CODE & "',INCOTERMs1='" & IN1 & "',INCOTERMS2='" & IN2 & "',ITEMNO='" & ITEMNO & "',MATERIALCODE='" & MAT_CODE & "',FLATE_RATE='" & FLRATE & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,GRADING='" & GRADING & "'" +
                         " WHERE CONTRACT_NUMBER= '" & CON_NO & "' and INACTIVE='X'"
                        ExecuteNonQuery(SQL)
                        LoadUser()
                        MsgBox("UPDATE SUCCESSFUL", vbInformation, "PURCHASE CONTRACT")
                        ClearInputPC()
                    End If
                End If
                If UCase(SimpleButton2.Text) = "UPDATE" THEn
                    SQL = "UPDATE T_PURCHASECONTRACT SET DOCUMENT_TYPE='" & DOC_TYPE & "',CONTRACT_STARTDATE=" & CON_START & ",CONTRACT_ENDDATE=" & CON_END & ",VENDOR_CODE='" & VENDOR_CODE & "',INCOTERMs1='" & IN1 & "',INCOTERMS2='" & IN2 & "',ITEMNO='" & ITEMNO & "',MATERIALCODE='" & MAT_CODE & "',FLATE_RATE='" & FLRATE & "',UPDATE_BY='" & USERNAME & "',UPDATE_DATE=SYSDATE,GRADING='" & GRADING & "'" +
                        " WHERE CONTRACT_NUMBER= '" & CON_NO & "'"
                    ExecuteNonQuery(SQL)
                    LoadUser()
                    MsgBox("UPDATE SUCCESSFUL", vbInformation, "PURCHASE CONTRACT")
                    ClearInputPC()
                End If
                End If
            End If

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'ADD
        ClearInputPC()
        UnlockAll()

        TextEdit1.Text = ""
        TextEdit1.SELECT()

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        'DELETE
        SQL = "UPDATE T_PURCHASECONTRACT SET INACTIVE= 'X',INACTIVE_DATE=SYSDATE WHERE CONTRACT_NUMBER='" & TextEdit1.Text & "'"
        ExecuteNonQuery(SQL)
        LoadUser()
        ClearInputPC()
        MsgBox("DELETE SUCCESSFUL", vbInformation, "PURCHASE CONTRACT")
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        'CANCEL
        ClearInputPC()
        lockAll()
        SimpleButton2.Text = "Save" 'SAVE
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        Me.Close()
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


            SimpleButton2.Text = "Update" 'save

            TextEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "CONTRACT_NUMBER").ToString()
            TextEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "DOCUMENT_TYPE").ToString()
            Dim CON_START As DateTime = GridView1.GetRowCellValue(e.RowHandle, "CONTRACT_STARTDATE").ToString()
            DateEdit1.Text = CON_START.ToString("M/d/yyyy")
            Dim CON_END As DateTime = GridView1.GetRowCellValue(e.RowHandle, "CONTRACT_ENDDATE").ToString()
            DateEdit2.Text = CON_END.ToString("M/d/yyyy")
            ComboBoxEdit1.Text = GridView1.GetRowCellValue(e.RowHandle, "VENDOR_NAME").ToString()
            TextEdit3.Text = GridView1.GetRowCellValue(e.RowHandle, "INCOTERMS1").ToString()
            TextEdit4.Text = GridView1.GetRowCellValue(e.RowHandle, "INCOTERMS2").ToString()
            TextEdit5.Text = GridView1.GetRowCellValue(e.RowHandle, "ITEMNO").ToString()

            TextEdit6.Text = GridView1.GetRowCellValue(e.RowHandle, "FLATE_RATE").ToString()
            ComboBoxEdit2.Text = GridView1.GetRowCellValue(e.RowHandle, "MATERIAL_NAME").ToString()
            TextEdit8.Text = GridView1.GetRowCellValue(e.RowHandle, "GRADING").ToString()

            UnlockAll()
            TextEdit1.Enabled = False

        End If
    End Sub

    Private Sub FrmPurchaseContract_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        PanelControl2.Height = (Me.Height - 60) - (30 * 6.3)
    End Sub
    Private Sub TextEdit5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit5.KeyPress
        'item no
        e.Handled = NumericOnly(e)
    End Sub


End Class