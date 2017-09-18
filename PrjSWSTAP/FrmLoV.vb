Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmLoV
    Dim rNUM1 As Integer = 0
    Dim rNUM2 As Integer = 20
    Dim PAR As Integer = 20

    Private Sub FrmLoV_Load(sender As Object, e As EventArgs) Handles Me.Load
        TextEdit2.Text = rNUM1 & " TO " & rNUM2 : TextEdit2.Enabled = False
        ValueLoV = ""
        LabelControl1.Text = LHeader & " Information"
        LabelControl2.Text = LHeader & " Search "
        TextEdit1.Text = ""
        LoadView()
        If My.Settings.SAP = "Y" Then SimpleButton1.Enabled = False  'SAP tombol new disebel

    End Sub

    Private Sub LoadView()
        GridView1.Columns.Clear()
        SQL = " SELECT * " +
        " FROM (SELECT ROWNUM RNUM,A.*  FROM  " +
        " ( " +
        " " & LSQL & " " +
        " ) A " +
        " WHERE ROWNUM <= " & rNUM2 & ")B " +
        " WHERE RNUM >= " & rNUM1 & ""
        'SQL = LSQL
        FILLGridView(SQL, GridControl1)
    End Sub

    Private Sub Search()
        On Error Resume Next
        'GridView1.ClearDocument()
        Dim nSQL As String = ""
        nSQL = " And UPPER(" & LField & ") Like '%" & UCase(Trim(TextEdit1.Text)) & "%' ORDER BY " & LField & " "
        nSQL = LSQL & nSQL

        SQL = " SELECT * " +
        " FROM (SELECT ROWNUM RNUM,A.*  FROM  " +
        " ( " +
        " " & nSQL & " " +
        " ) A " +
        " WHERE ROWNUM <= " & rNUM2 & ")B " +
        " WHERE RNUM >= " & rNUM1 & ""
        nSQL = SQL
        If CheckRecord(nSQL) = 0 Then
            nSQL = " WHERE UPPER(" & LField & ") like '%" & UCase(Trim(TextEdit1.Text)) & "%' ORDER BY " & LField & " "
            nSQL = SQL & nSQL
            FILLGridView(nSQL, GridControl1)
            If GridView1.RowCount.ToString = 0 Then MsgBox(LField & " Salah atau Masih Kosong..!! ", vbInformation, "Secure Weighbridge System")
        Else
            FILLGridView(nSQL, GridControl1)
            If GridView1.RowCount.ToString = 0 THEn MsgBox(LField & " Salah atau Masih Kosong..!! ", vbInformation, "Secure Weighbridge System")
        End If

    End Sub

    Private Sub GridView1_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles GridView1.RowCellClick
        On Error Resume Next
        If e.RowHandle < 0 THEn
            Exit Sub
        Else
            ValueLoV = GridView1.GetRowCellValue(e.RowHandle, LField).ToString()
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        rNUM1 = 0
        rNUM2 = 20
        TextEdit2.Text = rNUM1 & " TO " & rNUM2
        If ValueLoV = "" Then MsgBox("Data Belum di Pilih atau Masih Kosong..!! ", vbInformation, "Secure Weighbridge System")
        Close()
    End Sub

    Private Sub TextEdit1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextEdit1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) THEn Search()
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        If TextEdit1.Text <> "" THEn Search()
    End Sub

    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        'NAMBAH RANGE 
        rNUM1 = rNUM1 + PAR
        rNUM2 = rNUM2 + PAR
        TextEdit2.Text = rNUM1 & " TO " & rNUM2
        LoadView()
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        If rNUM1 > 0 Then
            rNUM1 = rNUM1 - PAR
            rNUM2 = rNUM2 - PAR
        Else
            rNUM1 = 0
            rNUM2 = PAR
        End If
        TextEdit2.Text = rNUM1 & " TO " & rNUM2
        LoadView()
    End Sub
End Class