<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up THE component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing THEn
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by THE Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: THE following procedure is required by THE Windows Form Designer
    'It can be modified using THE Windows Form Designer.  
    'Do not modify it using THE code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim SplashScreenManager1 As DevExpress.XtraSplashScreen.SplashScreenManager = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.PrjSWSTAP.SplashScreen1), True, True)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        Me.NavBarControl1 = New DevExpress.XtraNavBar.NavBarControl()
        Me.NavBarGroup1 = New DevExpress.XtraNavBar.NavBarGroup()
        Me.NavBarItem1 = New DevExpress.XtraNavBar.NavBarItem()
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.PanelControl4 = New DevExpress.XtraEditors.PanelControl()
        Me.BunifuFlatButton1 = New ns1.BunifuFlatButton()
        Me.BunifuFlatButton3 = New ns1.BunifuFlatButton()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.XtraTabbedMdiManager1 = New DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(Me.components)
        Me.ImgGrp = New DevExpress.Utils.ImageCollection(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel6 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel7 = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.NavBarControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl4.SuspendLayout()
        CType(Me.XtraTabbedMdiManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImgGrp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplashScreenManager1
        '
        SplashScreenManager1.ClosingDelay = 500
        '
        'PanelControl2
        '
        Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.PanelControl2.Appearance.Options.UseBackColor = True
        Me.PanelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl2.Controls.Add(Me.NavBarControl1)
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelControl2.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(248, 485)
        Me.PanelControl2.TabIndex = 2
        '
        'NavBarControl1
        '
        Me.NavBarControl1.ActiveGroup = Me.NavBarGroup1
        Me.NavBarControl1.Appearance.GroupHeader.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NavBarControl1.Appearance.GroupHeader.ForeColor = System.Drawing.Color.DarkGreen
        Me.NavBarControl1.Appearance.GroupHeader.Options.UseFont = True
        Me.NavBarControl1.Appearance.GroupHeader.Options.UseForeColor = True
        Me.NavBarControl1.Appearance.Item.BorderColor = System.Drawing.Color.OliveDrab
        Me.NavBarControl1.Appearance.Item.Options.UseBorderColor = True
        Me.NavBarControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat
        Me.NavBarControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NavBarControl1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.NavBarControl1.Groups.AddRange(New DevExpress.XtraNavBar.NavBarGroup() {Me.NavBarGroup1})
        Me.NavBarControl1.Items.AddRange(New DevExpress.XtraNavBar.NavBarItem() {Me.NavBarItem1})
        Me.NavBarControl1.Location = New System.Drawing.Point(0, 137)
        Me.NavBarControl1.LookAndFeel.SkinName = "Office 2016 Black"
        Me.NavBarControl1.Name = "NavBarControl1"
        Me.NavBarControl1.NavigationPaneGroupClientHeight = 500
        Me.NavBarControl1.OptionsNavPane.ExpandedWidth = 248
        Me.NavBarControl1.Size = New System.Drawing.Size(248, 348)
        Me.NavBarControl1.TabIndex = 6
        Me.NavBarControl1.Text = "NavBarControl1"
        Me.NavBarControl1.View = New DevExpress.XtraNavBar.ViewInfo.StandardSkinExplorerBarViewInfoRegistrator("Visual Studio 2013 Dark")
        '
        'NavBarGroup1
        '
        Me.NavBarGroup1.Caption = "HOME"
        Me.NavBarGroup1.Expanded = True
        Me.NavBarGroup1.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.NavBarItem1)})
        Me.NavBarGroup1.Name = "NavBarGroup1"
        Me.NavBarGroup1.SmallImage = Global.PrjSWSTAP.My.Resources.Resources.Home_16px
        '
        'NavBarItem1
        '
        Me.NavBarItem1.Caption = "EXIT"
        Me.NavBarItem1.Name = "NavBarItem1"
        '
        'PanelControl3
        '
        Me.PanelControl3.Appearance.BackColor = System.Drawing.Color.ForestGreen
        Me.PanelControl3.Appearance.Options.UseBackColor = True
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.PictureEdit1)
        Me.PanelControl3.Controls.Add(Me.LabelControl4)
        Me.PanelControl3.Controls.Add(Me.LabelControl5)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl3.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(248, 137)
        Me.PanelControl3.TabIndex = 5
        '
        'PictureEdit1
        '
        Me.PictureEdit1.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit1.EditValue = Global.PrjSWSTAP.My.Resources.Resources.triputra_agro_persada_logo
        Me.PictureEdit1.Location = New System.Drawing.Point(69, 3)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit1.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit1.Size = New System.Drawing.Size(107, 93)
        Me.PictureEdit1.TabIndex = 4
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Tahoma", 9.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl4.Appearance.Options.UseFont = True
        Me.LabelControl4.Appearance.Options.UseTextOptions = True
        Me.LabelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl4.Location = New System.Drawing.Point(12, 97)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(233, 14)
        Me.LabelControl4.TabIndex = 2
        Me.LabelControl4.Text = "MILL PLANT"
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Tahoma", 9.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl5.Appearance.Options.UseFont = True
        Me.LabelControl5.Appearance.Options.UseTextOptions = True
        Me.LabelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl5.Location = New System.Drawing.Point(12, 117)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(233, 14)
        Me.LabelControl5.TabIndex = 3
        Me.LabelControl5.Text = "LOCATION"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 9.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl3.Appearance.Options.UseFont = True
        Me.LabelControl3.Location = New System.Drawing.Point(8, 12)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(66, 14)
        Me.LabelControl3.TabIndex = 1
        Me.LabelControl3.Text = "SITE NAME"
        '
        'PanelControl1
        '
        Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.ForestGreen
        Me.PanelControl1.Appearance.BackColor2 = System.Drawing.Color.ForestGreen
        Me.PanelControl1.Appearance.Options.UseBackColor = True
        Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl1.Controls.Add(Me.PanelControl4)
        Me.PanelControl1.Controls.Add(Me.LabelControl3)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(248, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(714, 40)
        Me.PanelControl1.TabIndex = 3
        '
        'PanelControl4
        '
        Me.PanelControl4.Appearance.BackColor = System.Drawing.Color.ForestGreen
        Me.PanelControl4.Appearance.BackColor2 = System.Drawing.Color.PaleGreen
        Me.PanelControl4.Appearance.BorderColor = System.Drawing.Color.ForestGreen
        Me.PanelControl4.Appearance.Options.UseBackColor = True
        Me.PanelControl4.Appearance.Options.UseBorderColor = True
        Me.PanelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl4.ContentImageAlignment = System.Drawing.ContentAlignment.TopCenter
        Me.PanelControl4.Controls.Add(Me.BunifuFlatButton1)
        Me.PanelControl4.Controls.Add(Me.BunifuFlatButton3)
        Me.PanelControl4.Controls.Add(Me.LabelControl7)
        Me.PanelControl4.Dock = System.Windows.Forms.DockStyle.Right
        Me.PanelControl4.Location = New System.Drawing.Point(290, 0)
        Me.PanelControl4.Name = "PanelControl4"
        Me.PanelControl4.Size = New System.Drawing.Size(424, 40)
        Me.PanelControl4.TabIndex = 1
        '
        'BunifuFlatButton1
        '
        Me.BunifuFlatButton1.Activecolor = System.Drawing.Color.SteelBlue
        Me.BunifuFlatButton1.BackColor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BunifuFlatButton1.BorderRadius = 0
        Me.BunifuFlatButton1.ButtonText = "Sign In"
        Me.BunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BunifuFlatButton1.DisabledColor = System.Drawing.Color.Gray
        Me.BunifuFlatButton1.Dock = System.Windows.Forms.DockStyle.Right
        Me.BunifuFlatButton1.Font = New System.Drawing.Font("TeamViewer12", 9.0!, System.Drawing.FontStyle.Bold)
        Me.BunifuFlatButton1.Iconcolor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton1.Iconimage = Global.PrjSWSTAP.My.Resources.Resources.Admin_16px
        Me.BunifuFlatButton1.Iconimage_right = Nothing
        Me.BunifuFlatButton1.Iconimage_right_Selected = Nothing
        Me.BunifuFlatButton1.Iconimage_Selected = Nothing
        Me.BunifuFlatButton1.IconMarginLeft = 0
        Me.BunifuFlatButton1.IconMarginRight = 0
        Me.BunifuFlatButton1.IconRightVisible = True
        Me.BunifuFlatButton1.IconRightZoom = 0R
        Me.BunifuFlatButton1.IconVisible = True
        Me.BunifuFlatButton1.IconZoom = 40.0R
        Me.BunifuFlatButton1.IsTab = False
        Me.BunifuFlatButton1.Location = New System.Drawing.Point(222, 0)
        Me.BunifuFlatButton1.Name = "BunifuFlatButton1"
        Me.BunifuFlatButton1.Normalcolor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton1.OnHovercolor = System.Drawing.SystemColors.ActiveCaption
        Me.BunifuFlatButton1.OnHoverTextColor = System.Drawing.Color.White
        Me.BunifuFlatButton1.selected = False
        Me.BunifuFlatButton1.Size = New System.Drawing.Size(101, 40)
        Me.BunifuFlatButton1.TabIndex = 5
        Me.BunifuFlatButton1.Text = "Sign In"
        Me.BunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.BunifuFlatButton1.Textcolor = System.Drawing.Color.White
        Me.BunifuFlatButton1.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'BunifuFlatButton3
        '
        Me.BunifuFlatButton3.Activecolor = System.Drawing.Color.Orange
        Me.BunifuFlatButton3.BackColor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BunifuFlatButton3.BorderRadius = 0
        Me.BunifuFlatButton3.ButtonText = "Config"
        Me.BunifuFlatButton3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BunifuFlatButton3.DisabledColor = System.Drawing.Color.Gray
        Me.BunifuFlatButton3.Dock = System.Windows.Forms.DockStyle.Right
        Me.BunifuFlatButton3.Font = New System.Drawing.Font("TeamViewer12", 9.0!, System.Drawing.FontStyle.Bold)
        Me.BunifuFlatButton3.Iconcolor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton3.Iconimage = Global.PrjSWSTAP.My.Resources.Resources.SettingsW_16px
        Me.BunifuFlatButton3.Iconimage_right = Nothing
        Me.BunifuFlatButton3.Iconimage_right_Selected = Nothing
        Me.BunifuFlatButton3.Iconimage_Selected = Nothing
        Me.BunifuFlatButton3.IconMarginLeft = 0
        Me.BunifuFlatButton3.IconMarginRight = 0
        Me.BunifuFlatButton3.IconRightVisible = True
        Me.BunifuFlatButton3.IconRightZoom = 0R
        Me.BunifuFlatButton3.IconVisible = True
        Me.BunifuFlatButton3.IconZoom = 40.0R
        Me.BunifuFlatButton3.IsTab = False
        Me.BunifuFlatButton3.Location = New System.Drawing.Point(323, 0)
        Me.BunifuFlatButton3.Name = "BunifuFlatButton3"
        Me.BunifuFlatButton3.Normalcolor = System.Drawing.Color.Transparent
        Me.BunifuFlatButton3.OnHovercolor = System.Drawing.SystemColors.Highlight
        Me.BunifuFlatButton3.OnHoverTextColor = System.Drawing.Color.White
        Me.BunifuFlatButton3.selected = False
        Me.BunifuFlatButton3.Size = New System.Drawing.Size(101, 40)
        Me.BunifuFlatButton3.TabIndex = 4
        Me.BunifuFlatButton3.Text = "Config"
        Me.BunifuFlatButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.BunifuFlatButton3.Textcolor = System.Drawing.Color.White
        Me.BunifuFlatButton3.TextFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LabelControl7.Appearance.Options.UseFont = True
        Me.LabelControl7.Location = New System.Drawing.Point(163, 12)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(55, 13)
        Me.LabelControl7.TabIndex = 3
        Me.LabelControl7.Text = "Welcome,"
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "Office 2016 Colorful"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'XtraTabbedMdiManager1
        '
        Me.XtraTabbedMdiManager1.Appearance.BackColor = System.Drawing.Color.DarkGray
        Me.XtraTabbedMdiManager1.Appearance.BackColor2 = System.Drawing.Color.DarkGray
        Me.XtraTabbedMdiManager1.Appearance.BorderColor = System.Drawing.Color.DarkGray
        Me.XtraTabbedMdiManager1.Appearance.Options.UseBackColor = True
        Me.XtraTabbedMdiManager1.Appearance.Options.UseBorderColor = True
        Me.XtraTabbedMdiManager1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.XtraTabbedMdiManager1.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
        Me.XtraTabbedMdiManager1.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal
        Me.XtraTabbedMdiManager1.MdiParent = Me
        Me.XtraTabbedMdiManager1.ShowHeaderFocus = DevExpress.Utils.DefaultBoolean.[True]
        '
        'ImgGrp
        '
        Me.ImgGrp.ImageStream = CType(resources.GetObject("ImgGrp.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImgGrp.Images.SetKeyName(0, "Menu_16px.png")
        Me.ImgGrp.Images.SetKeyName(1, "About_16px.png")
        Me.ImgGrp.Images.SetKeyName(2, "About_24px.png")
        Me.ImgGrp.Images.SetKeyName(3, "Add User Male_24px.png")
        Me.ImgGrp.Images.SetKeyName(4, "Admin_16px.png")
        Me.ImgGrp.Images.SetKeyName(5, "Connected_16px.png")
        Me.ImgGrp.Images.SetKeyName(6, "Contacts_16px.png")
        Me.ImgGrp.Images.SetKeyName(7, "Database_16px.png")
        Me.ImgGrp.Images.SetKeyName(8, "Delivery_24px.png")
        Me.ImgGrp.Images.SetKeyName(9, "Documents_24px.png")
        Me.ImgGrp.Images.SetKeyName(10, "Download_24px.png")
        Me.ImgGrp.Images.SetKeyName(11, "Enter_16px.png")
        Me.ImgGrp.Images.SetKeyName(12, "EnterW_16px.png")
        Me.ImgGrp.Images.SetKeyName(13, "Exit_16px.png")
        Me.ImgGrp.Images.SetKeyName(14, "Home_16px.png")
        Me.ImgGrp.Images.SetKeyName(15, "Home_24px.png")
        Me.ImgGrp.Images.SetKeyName(16, "Import_24px.png")
        Me.ImgGrp.Images.SetKeyName(17, "Info_16px.png")
        Me.ImgGrp.Images.SetKeyName(18, "Key_24px.png")
        Me.ImgGrp.Images.SetKeyName(19, "Lock_16px.png")
        Me.ImgGrp.Images.SetKeyName(20, "Microsoft Excel_16px.png")
        Me.ImgGrp.Images.SetKeyName(21, "Password_16px.png")
        Me.ImgGrp.Images.SetKeyName(22, "Settings_16px.png")
        Me.ImgGrp.Images.SetKeyName(23, "SettingsW_16px.png")
        Me.ImgGrp.Images.SetKeyName(24, "Speed_24px.png")
        Me.ImgGrp.Images.SetKeyName(25, "Support_24px.png")
        Me.ImgGrp.Images.SetKeyName(26, "Survey_24px.png")
        Me.ImgGrp.Images.SetKeyName(27, "Taxi_24px.png")
        Me.ImgGrp.Images.SetKeyName(28, "Undo_24px.png")
        Me.ImgGrp.Images.SetKeyName(29, "User Group Man Man_24px.png")
        Me.ImgGrp.Images.SetKeyName(30, "User Male_16px.png")
        Me.ImgGrp.Images.SetKeyName(31, "User_24px.png")
        Me.ImgGrp.Images.SetKeyName(32, "Video Call_16px.png")
        Me.ImgGrp.Images.SetKeyName(33, "Wi-Fi_16px.png")
        Me.ImgGrp.Images.SetKeyName(34, "Wi-Fi_24px.png")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2, Me.ToolStripStatusLabel3, Me.ToolStripStatusLabel4, Me.ToolStripStatusLabel5, Me.ToolStripStatusLabel6, Me.ToolStripStatusLabel7})
        Me.StatusStrip1.Location = New System.Drawing.Point(248, 463)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(714, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(45, 17)
        Me.ToolStripStatusLabel1.Text = "Version"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(62, 17)
        Me.ToolStripStatusLabel2.Text = "Hostname"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(61, 17)
        Me.ToolStripStatusLabel3.Text = "Date Time"
        '
        'ToolStripStatusLabel4
        '
        Me.ToolStripStatusLabel4.ForeColor = System.Drawing.Color.DodgerBlue
        Me.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4"
        Me.ToolStripStatusLabel4.Size = New System.Drawing.Size(57, 17)
        Me.ToolStripStatusLabel4.Text = "Code Site"
        '
        'ToolStripStatusLabel5
        '
        Me.ToolStripStatusLabel5.BackColor = System.Drawing.Color.DarkGray
        Me.ToolStripStatusLabel5.ForeColor = System.Drawing.Color.Gray
        Me.ToolStripStatusLabel5.Name = "ToolStripStatusLabel5"
        Me.ToolStripStatusLabel5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ToolStripStatusLabel5.Size = New System.Drawing.Size(25, 17)
        Me.ToolStripStatusLabel5.Text = "WB"
        '
        'ToolStripStatusLabel6
        '
        Me.ToolStripStatusLabel6.BackColor = System.Drawing.Color.DarkGray
        Me.ToolStripStatusLabel6.ForeColor = System.Drawing.Color.Gray
        Me.ToolStripStatusLabel6.Name = "ToolStripStatusLabel6"
        Me.ToolStripStatusLabel6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ToolStripStatusLabel6.Size = New System.Drawing.Size(38, 17)
        Me.ToolStripStatusLabel6.Text = "Cam1"
        '
        'ToolStripStatusLabel7
        '
        Me.ToolStripStatusLabel7.BackColor = System.Drawing.Color.DarkGray
        Me.ToolStripStatusLabel7.ForeColor = System.Drawing.Color.Gray
        Me.ToolStripStatusLabel7.Name = "ToolStripStatusLabel7"
        Me.ToolStripStatusLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ToolStripStatusLabel7.Size = New System.Drawing.Size(38, 17)
        Me.ToolStripStatusLabel7.Text = "Cam2"
        '
        'FrmMain
        '
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.ForeColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.Appearance.Options.UseForeColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Center
        Me.BackgroundImageStore = Global.PrjSWSTAP.My.Resources.Resources.TAP_SMAL_BANER
        Me.ClientSize = New System.Drawing.Size(962, 485)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.PanelControl2)
        Me.DoubleBuffered = True
        Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.LookAndFeel.SkinName = "Office 2013"
        Me.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.Name = "FrmMain"
        Me.Text = "PT TRIPUTA AGRO PERSADA GROUP"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.NavBarControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.PanelControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl4.ResumeLayout(False)
        Me.PanelControl4.PerformLayout()
        CType(Me.XtraTabbedMdiManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImgGrp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WiTHEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WiTHEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WiTHEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WiTHEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WiTHEvents PanelControl4 As DevExpress.XtraEditors.PanelControl
    Friend WiTHEvents NavBarControl1 As DevExpress.XtraNavBar.NavBarControl
    Friend WiTHEvents NavBarGroup1 As DevExpress.XtraNavBar.NavBarGroup
    Friend WiTHEvents NavBarItem1 As DevExpress.XtraNavBar.NavBarItem
    Friend WiTHEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WiTHEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WiTHEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WiTHEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WiTHEvents Timer1 As Timer
    Friend WiTHEvents XtraTabbedMdiManager1 As DevExpress.XtraTabbedMdi.XtraTabbedMdiManager
    Friend WiTHEvents BunifuFlatButton3 As ns1.BunifuFlatButton
    Friend WiTHEvents BunifuFlatButton1 As ns1.BunifuFlatButton
    Friend WiTHEvents ImgGrp As DevExpress.Utils.ImageCollection
    Friend WiTHEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WiTHEvents StatusStrip1 As StatusStrip
    Friend WiTHEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel3 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel4 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel5 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel6 As ToolStripStatusLabel
    Friend WiTHEvents ToolStripStatusLabel7 As ToolStripStatusLabel
End Class
