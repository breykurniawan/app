<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDataMasters
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDataMasters))
        Me.BunifuGradientPanel1 = New ns1.BunifuGradientPanel()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.BackstageViewControl1 = New DevExpress.XtraBars.Ribbon.BackstageViewControl()
        Me.BackstageViewClientControl1 = New DevExpress.XtraBars.Ribbon.BackstageViewClientControl()
        Me.BackstageViewTabItem1 = New DevExpress.XtraBars.Ribbon.BackstageViewTabItem()
        Me.BunifuGradientPanel1.SuspendLayout()
        CType(Me.BackstageViewControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BackstageViewControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BunifuGradientPanel1
        '
        Me.BunifuGradientPanel1.BackgroundImage = CType(resources.GetObject("BunifuGradientPanel1.BackgroundImage"), System.Drawing.Image)
        Me.BunifuGradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BunifuGradientPanel1.Controls.Add(Me.LabelControl1)
        Me.BunifuGradientPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.BunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.DarkGreen
        Me.BunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.White
        Me.BunifuGradientPanel1.GradientTopLeft = System.Drawing.Color.Green
        Me.BunifuGradientPanel1.GradientTopRight = System.Drawing.Color.White
        Me.BunifuGradientPanel1.Location = New System.Drawing.Point(0, 0)
        Me.BunifuGradientPanel1.Name = "BunifuGradientPanel1"
        Me.BunifuGradientPanel1.Quality = 10
        Me.BunifuGradientPanel1.Size = New System.Drawing.Size(872, 43)
        Me.BunifuGradientPanel1.TabIndex = 3
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl1.Appearance.ForeColor = System.Drawing.Color.White
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Appearance.Options.UseForeColor = True
        Me.LabelControl1.Location = New System.Drawing.Point(12, 12)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(123, 14)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = "Configuration Menu"
        '
        'BackstageViewControl1
        '
        Me.BackstageViewControl1.Appearance.BackColor = System.Drawing.Color.DimGray
        Me.BackstageViewControl1.Appearance.Options.UseBackColor = True
        Me.BackstageViewControl1.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Yellow
        Me.BackstageViewControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BackstageViewControl1.ForeColor = System.Drawing.Color.Green
        Me.BackstageViewControl1.Location = New System.Drawing.Point(0, 43)
        Me.BackstageViewControl1.Name = "BackstageViewControl1"
        Me.BackstageViewControl1.Size = New System.Drawing.Size(872, 454)
        Me.BackstageViewControl1.TabIndex = 4
        Me.BackstageViewControl1.Text = "BackstageViewControl1"
        '
        'BackstageViewClientControl1
        '
        Me.BackstageViewClientControl1.Location = New System.Drawing.Point(195, 0)
        Me.BackstageViewClientControl1.Name = "BackstageViewClientControl1"
        Me.BackstageViewClientControl1.Size = New System.Drawing.Size(677, 454)
        Me.BackstageViewClientControl1.TabIndex = 1
        '
        'BackstageViewTabItem1
        '
        Me.BackstageViewTabItem1.Caption = "BackstageViewTabItem1"
        Me.BackstageViewTabItem1.ContentControl = Me.BackstageViewClientControl1
        Me.BackstageViewTabItem1.Name = "BackstageViewTabItem1"
        Me.BackstageViewTabItem1.Selected = True
        '
        'FrmDataMasters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(872, 497)
        Me.Controls.Add(Me.BackstageViewControl1)
        Me.Controls.Add(Me.BunifuGradientPanel1)
        Me.Name = "FrmDataMasters"
        Me.Text = "FrmDataMasters"
        Me.BunifuGradientPanel1.ResumeLayout(False)
        Me.BunifuGradientPanel1.PerformLayout()
        CType(Me.BackstageViewControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BackstageViewControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BunifuGradientPanel1 As ns1.BunifuGradientPanel
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents BackstageViewControl1 As DevExpress.XtraBars.Ribbon.BackstageViewControl
    Friend WithEvents BackstageViewClientControl1 As DevExpress.XtraBars.Ribbon.BackstageViewClientControl
    Friend WithEvents BackstageViewTabItem1 As DevExpress.XtraBars.Ribbon.BackstageViewTabItem
End Class
