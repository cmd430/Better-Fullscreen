﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BetterFullscreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BetterFullscreen))
        Me.TrayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TrayMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToggleWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RichTextBox_EventLog = New System.Windows.Forms.RichTextBox()
        Me.GroupBox_Events = New System.Windows.Forms.GroupBox()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.NumericUpDown_Left = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown_Top = New System.Windows.Forms.NumericUpDown()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.NumericUpDown_Height = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown_Width = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_Class = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_Title = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox_Games = New System.Windows.Forms.ComboBox()
        Me.Button_SaveApp = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.NumericUpDown_DefaultLeft = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown_DefaultTop = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.NumericUpDown_DefaultHeight = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown_DefaultWidth = New System.Windows.Forms.NumericUpDown()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ComboBox_Modifier = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.ComboBox_Key = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox_ApplicationSettings = New System.Windows.Forms.GroupBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.ComboBox_TriggerEvents = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.CheckBox_StartHidden = New System.Windows.Forms.CheckBox()
        Me.CheckBox_StartWithWindows = New System.Windows.Forms.CheckBox()
        Me.GroupBox_GameSettings = New System.Windows.Forms.GroupBox()
        Me.CheckBox_ForceTopMost = New System.Windows.Forms.CheckBox()
        Me.CheckBox_ProfileEnabled = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CaptureMouse = New System.Windows.Forms.CheckBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.NumericUpDown_Delay = New System.Windows.Forms.NumericUpDown()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Button_Remove = New System.Windows.Forms.Button()
        Me.Panel_titleRadioButtonContainer = New System.Windows.Forms.Panel()
        Me.RadioButton_TitleIncludes = New System.Windows.Forms.RadioButton()
        Me.RadioButton_TitleFullMatch = New System.Windows.Forms.RadioButton()
        Me.RadioButton_TitleStartsWith = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.TrayMenu.SuspendLayout()
        Me.GroupBox_Events.SuspendLayout()
        CType(Me.NumericUpDown_Left, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Top, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Height, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_Width, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_DefaultLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_DefaultTop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_DefaultHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown_DefaultWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.GroupBox_ApplicationSettings.SuspendLayout()
        Me.GroupBox_GameSettings.SuspendLayout()
        CType(Me.NumericUpDown_Delay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_titleRadioButtonContainer.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TrayIcon
        '
        Me.TrayIcon.ContextMenuStrip = Me.TrayMenu
        Me.TrayIcon.Icon = CType(resources.GetObject("TrayIcon.Icon"), System.Drawing.Icon)
        Me.TrayIcon.Text = "Better Fullscreen"
        Me.TrayIcon.Visible = True
        '
        'TrayMenu
        '
        Me.TrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToggleWindowToolStripMenuItem, Me.ReloadToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.TrayMenu.Name = "ContextMenuStrip1"
        Me.TrayMenu.Size = New System.Drawing.Size(149, 76)
        '
        'ToggleWindowToolStripMenuItem
        '
        Me.ToggleWindowToolStripMenuItem.Name = "ToggleWindowToolStripMenuItem"
        Me.ToggleWindowToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ToggleWindowToolStripMenuItem.Text = "Toggle Config"
        '
        'ReloadToolStripMenuItem
        '
        Me.ReloadToolStripMenuItem.Name = "ReloadToolStripMenuItem"
        Me.ReloadToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ReloadToolStripMenuItem.Text = "Reload"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(145, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'RichTextBox_EventLog
        '
        Me.RichTextBox_EventLog.BackColor = System.Drawing.Color.White
        Me.RichTextBox_EventLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_EventLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox_EventLog.HideSelection = False
        Me.RichTextBox_EventLog.Location = New System.Drawing.Point(3, 16)
        Me.RichTextBox_EventLog.Name = "RichTextBox_EventLog"
        Me.RichTextBox_EventLog.ReadOnly = True
        Me.RichTextBox_EventLog.Size = New System.Drawing.Size(633, 177)
        Me.RichTextBox_EventLog.TabIndex = 1
        Me.RichTextBox_EventLog.Text = ""
        '
        'GroupBox_Events
        '
        Me.GroupBox_Events.Controls.Add(Me.RichTextBox_EventLog)
        Me.GroupBox_Events.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_Events.Location = New System.Drawing.Point(3, 334)
        Me.GroupBox_Events.Name = "GroupBox_Events"
        Me.GroupBox_Events.Size = New System.Drawing.Size(639, 196)
        Me.GroupBox_Events.TabIndex = 3
        Me.GroupBox_Events.TabStop = False
        Me.GroupBox_Events.Text = "Events"
        '
        'Button_Save
        '
        Me.Button_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Save.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Button_Save.Location = New System.Drawing.Point(6, 289)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(176, 23)
        Me.Button_Save.TabIndex = 18
        Me.Button_Save.Text = "Save Profile"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label7.Location = New System.Drawing.Point(185, 160)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(25, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Left"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label8.Location = New System.Drawing.Point(54, 160)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(26, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Top"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label9.Location = New System.Drawing.Point(164, 178)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(12, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "x"
        '
        'NumericUpDown_Left
        '
        Me.NumericUpDown_Left.Location = New System.Drawing.Point(188, 176)
        Me.NumericUpDown_Left.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_Left.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_Left.Name = "NumericUpDown_Left"
        Me.NumericUpDown_Left.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_Left.TabIndex = 13
        '
        'NumericUpDown_Top
        '
        Me.NumericUpDown_Top.Location = New System.Drawing.Point(57, 176)
        Me.NumericUpDown_Top.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_Top.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_Top.Name = "NumericUpDown_Top"
        Me.NumericUpDown_Top.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_Top.TabIndex = 12
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label10.Location = New System.Drawing.Point(3, 178)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(44, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Position"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label6.Location = New System.Drawing.Point(185, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Height"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label5.Location = New System.Drawing.Point(54, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Width"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label4.Location = New System.Drawing.Point(164, 139)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(12, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "x"
        '
        'NumericUpDown_Height
        '
        Me.NumericUpDown_Height.Location = New System.Drawing.Point(188, 137)
        Me.NumericUpDown_Height.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_Height.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_Height.Name = "NumericUpDown_Height"
        Me.NumericUpDown_Height.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_Height.TabIndex = 7
        '
        'NumericUpDown_Width
        '
        Me.NumericUpDown_Width.Location = New System.Drawing.Point(57, 137)
        Me.NumericUpDown_Width.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_Width.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_Width.Name = "NumericUpDown_Width"
        Me.NumericUpDown_Width.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_Width.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label3.Location = New System.Drawing.Point(3, 139)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Size"
        '
        'TextBox_Class
        '
        Me.TextBox_Class.Location = New System.Drawing.Point(57, 98)
        Me.TextBox_Class.Name = "TextBox_Class"
        Me.TextBox_Class.Size = New System.Drawing.Size(228, 20)
        Me.TextBox_Class.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label2.Location = New System.Drawing.Point(3, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Class"
        '
        'TextBox_Title
        '
        Me.TextBox_Title.Location = New System.Drawing.Point(57, 52)
        Me.TextBox_Title.Name = "TextBox_Title"
        Me.TextBox_Title.Size = New System.Drawing.Size(228, 20)
        Me.TextBox_Title.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(3, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Title"
        '
        'ComboBox_Games
        '
        Me.ComboBox_Games.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Games.FormattingEnabled = True
        Me.ComboBox_Games.Location = New System.Drawing.Point(6, 19)
        Me.ComboBox_Games.Name = "ComboBox_Games"
        Me.ComboBox_Games.Size = New System.Drawing.Size(170, 21)
        Me.ComboBox_Games.Sorted = True
        Me.ComboBox_Games.TabIndex = 0
        '
        'Button_SaveApp
        '
        Me.Button_SaveApp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveApp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Button_SaveApp.Location = New System.Drawing.Point(9, 289)
        Me.Button_SaveApp.Name = "Button_SaveApp"
        Me.Button_SaveApp.Size = New System.Drawing.Size(319, 23)
        Me.Button_SaveApp.TabIndex = 18
        Me.Button_SaveApp.Text = "Save Settings"
        Me.Button_SaveApp.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label11.Location = New System.Drawing.Point(228, 101)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(25, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Left"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label12.Location = New System.Drawing.Point(103, 101)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(26, 13)
        Me.Label12.TabIndex = 15
        Me.Label12.Text = "Top"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label13.Location = New System.Drawing.Point(210, 119)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(12, 13)
        Me.Label13.TabIndex = 14
        Me.Label13.Text = "x"
        '
        'NumericUpDown_DefaultLeft
        '
        Me.NumericUpDown_DefaultLeft.Location = New System.Drawing.Point(231, 117)
        Me.NumericUpDown_DefaultLeft.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_DefaultLeft.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_DefaultLeft.Name = "NumericUpDown_DefaultLeft"
        Me.NumericUpDown_DefaultLeft.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_DefaultLeft.TabIndex = 13
        '
        'NumericUpDown_DefaultTop
        '
        Me.NumericUpDown_DefaultTop.Location = New System.Drawing.Point(106, 117)
        Me.NumericUpDown_DefaultTop.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_DefaultTop.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_DefaultTop.Name = "NumericUpDown_DefaultTop"
        Me.NumericUpDown_DefaultTop.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_DefaultTop.TabIndex = 12
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label14.Location = New System.Drawing.Point(6, 119)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(81, 13)
        Me.Label14.TabIndex = 11
        Me.Label14.Text = "Default Position"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label15.Location = New System.Drawing.Point(228, 62)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(38, 13)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "Height"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label16.Location = New System.Drawing.Point(103, 62)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(35, 13)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "Width"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label17.Location = New System.Drawing.Point(210, 80)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(12, 13)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "x"
        '
        'NumericUpDown_DefaultHeight
        '
        Me.NumericUpDown_DefaultHeight.Location = New System.Drawing.Point(231, 78)
        Me.NumericUpDown_DefaultHeight.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_DefaultHeight.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_DefaultHeight.Name = "NumericUpDown_DefaultHeight"
        Me.NumericUpDown_DefaultHeight.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_DefaultHeight.TabIndex = 7
        '
        'NumericUpDown_DefaultWidth
        '
        Me.NumericUpDown_DefaultWidth.Location = New System.Drawing.Point(106, 78)
        Me.NumericUpDown_DefaultWidth.Maximum = New Decimal(New Integer() {6480, 0, 0, 0})
        Me.NumericUpDown_DefaultWidth.Minimum = New Decimal(New Integer() {6480, 0, 0, -2147483648})
        Me.NumericUpDown_DefaultWidth.Name = "NumericUpDown_DefaultWidth"
        Me.NumericUpDown_DefaultWidth.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_DefaultWidth.TabIndex = 6
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label18.Location = New System.Drawing.Point(6, 80)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(64, 13)
        Me.Label18.TabIndex = 5
        Me.Label18.Text = "Default Size"
        '
        'ComboBox_Modifier
        '
        Me.ComboBox_Modifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Modifier.FormattingEnabled = True
        Me.ComboBox_Modifier.Items.AddRange(New Object() {"None", "Alt", "Ctrl", "Shift"})
        Me.ComboBox_Modifier.Location = New System.Drawing.Point(106, 38)
        Me.ComboBox_Modifier.Name = "ComboBox_Modifier"
        Me.ComboBox_Modifier.Size = New System.Drawing.Size(97, 21)
        Me.ComboBox_Modifier.TabIndex = 0
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label19.Location = New System.Drawing.Point(103, 22)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(44, 13)
        Me.Label19.TabIndex = 20
        Me.Label19.Text = "Modifier"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label20.Location = New System.Drawing.Point(6, 41)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(94, 13)
        Me.Label20.TabIndex = 21
        Me.Label20.Text = "Add Game Hotkey"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label21.Location = New System.Drawing.Point(228, 22)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(25, 13)
        Me.Label21.TabIndex = 23
        Me.Label21.Text = "Key"
        '
        'ComboBox_Key
        '
        Me.ComboBox_Key.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Key.FormattingEnabled = True
        Me.ComboBox_Key.Items.AddRange(New Object() {"F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"})
        Me.ComboBox_Key.Location = New System.Drawing.Point(231, 38)
        Me.ComboBox_Key.Name = "ComboBox_Key"
        Me.ComboBox_Key.Size = New System.Drawing.Size(97, 21)
        Me.ComboBox_Key.TabIndex = 22
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.67762!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.32238!))
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox_ApplicationSettings, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox_GameSettings, 1, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(639, 325)
        Me.TableLayoutPanel3.TabIndex = 6
        '
        'GroupBox_ApplicationSettings
        '
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label26)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label25)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.ComboBox_TriggerEvents)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label24)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.CheckBox_StartHidden)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.CheckBox_StartWithWindows)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label21)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.ComboBox_Key)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.ComboBox_Modifier)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label20)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label18)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label19)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.NumericUpDown_DefaultWidth)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.NumericUpDown_DefaultHeight)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Button_SaveApp)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label17)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label11)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label16)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label12)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label15)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label13)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.Label14)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.NumericUpDown_DefaultLeft)
        Me.GroupBox_ApplicationSettings.Controls.Add(Me.NumericUpDown_DefaultTop)
        Me.GroupBox_ApplicationSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_ApplicationSettings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_ApplicationSettings.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox_ApplicationSettings.Name = "GroupBox_ApplicationSettings"
        Me.GroupBox_ApplicationSettings.Size = New System.Drawing.Size(337, 319)
        Me.GroupBox_ApplicationSettings.TabIndex = 0
        Me.GroupBox_ApplicationSettings.TabStop = False
        Me.GroupBox_ApplicationSettings.Text = "Application Settings"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label26.Location = New System.Drawing.Point(103, 140)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(40, 13)
        Me.Label26.TabIndex = 28
        Me.Label26.Text = "Events"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(6, 160)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(76, 13)
        Me.Label25.TabIndex = 27
        Me.Label25.Text = "Trigger Events"
        '
        'ComboBox_TriggerEvents
        '
        Me.ComboBox_TriggerEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TriggerEvents.FormattingEnabled = True
        Me.ComboBox_TriggerEvents.Items.AddRange(New Object() {"Foreground Window Changed", "Mouse Capture Start", "Foreground Window Changed & Mouse Capture Start"})
        Me.ComboBox_TriggerEvents.Location = New System.Drawing.Point(106, 156)
        Me.ComboBox_TriggerEvents.Name = "ComboBox_TriggerEvents"
        Me.ComboBox_TriggerEvents.Size = New System.Drawing.Size(222, 21)
        Me.ComboBox_TriggerEvents.TabIndex = 0
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label24.Location = New System.Drawing.Point(210, 41)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(13, 13)
        Me.Label24.TabIndex = 26
        Me.Label24.Text = "+"
        '
        'CheckBox_StartHidden
        '
        Me.CheckBox_StartHidden.AutoSize = True
        Me.CheckBox_StartHidden.Checked = True
        Me.CheckBox_StartHidden.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_StartHidden.Location = New System.Drawing.Point(106, 206)
        Me.CheckBox_StartHidden.Name = "CheckBox_StartHidden"
        Me.CheckBox_StartHidden.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_StartHidden.TabIndex = 25
        Me.CheckBox_StartHidden.Text = "Start Hidden"
        Me.CheckBox_StartHidden.UseVisualStyleBackColor = True
        '
        'CheckBox_StartWithWindows
        '
        Me.CheckBox_StartWithWindows.AutoSize = True
        Me.CheckBox_StartWithWindows.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_StartWithWindows.Location = New System.Drawing.Point(106, 183)
        Me.CheckBox_StartWithWindows.Name = "CheckBox_StartWithWindows"
        Me.CheckBox_StartWithWindows.Size = New System.Drawing.Size(117, 17)
        Me.CheckBox_StartWithWindows.TabIndex = 24
        Me.CheckBox_StartWithWindows.Text = "Start with Windows"
        Me.CheckBox_StartWithWindows.UseVisualStyleBackColor = True
        '
        'GroupBox_GameSettings
        '
        Me.GroupBox_GameSettings.Controls.Add(Me.CheckBox_ForceTopMost)
        Me.GroupBox_GameSettings.Controls.Add(Me.CheckBox_ProfileEnabled)
        Me.GroupBox_GameSettings.Controls.Add(Me.CheckBox_CaptureMouse)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label22)
        Me.GroupBox_GameSettings.Controls.Add(Me.NumericUpDown_Delay)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label23)
        Me.GroupBox_GameSettings.Controls.Add(Me.Button_Remove)
        Me.GroupBox_GameSettings.Controls.Add(Me.ComboBox_Games)
        Me.GroupBox_GameSettings.Controls.Add(Me.Button_Save)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label1)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label7)
        Me.GroupBox_GameSettings.Controls.Add(Me.TextBox_Title)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label8)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label2)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label9)
        Me.GroupBox_GameSettings.Controls.Add(Me.TextBox_Class)
        Me.GroupBox_GameSettings.Controls.Add(Me.NumericUpDown_Left)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label3)
        Me.GroupBox_GameSettings.Controls.Add(Me.NumericUpDown_Top)
        Me.GroupBox_GameSettings.Controls.Add(Me.NumericUpDown_Width)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label10)
        Me.GroupBox_GameSettings.Controls.Add(Me.NumericUpDown_Height)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label6)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label4)
        Me.GroupBox_GameSettings.Controls.Add(Me.Label5)
        Me.GroupBox_GameSettings.Controls.Add(Me.Panel_titleRadioButtonContainer)
        Me.GroupBox_GameSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_GameSettings.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_GameSettings.Location = New System.Drawing.Point(346, 3)
        Me.GroupBox_GameSettings.Name = "GroupBox_GameSettings"
        Me.GroupBox_GameSettings.Size = New System.Drawing.Size(290, 319)
        Me.GroupBox_GameSettings.TabIndex = 1
        Me.GroupBox_GameSettings.TabStop = False
        Me.GroupBox_GameSettings.Text = "Game Settings"
        '
        'CheckBox_ForceTopMost
        '
        Me.CheckBox_ForceTopMost.AutoSize = True
        Me.CheckBox_ForceTopMost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_ForceTopMost.Location = New System.Drawing.Point(6, 264)
        Me.CheckBox_ForceTopMost.Name = "CheckBox_ForceTopMost"
        Me.CheckBox_ForceTopMost.Size = New System.Drawing.Size(101, 17)
        Me.CheckBox_ForceTopMost.TabIndex = 27
        Me.CheckBox_ForceTopMost.Text = "Force Top Most"
        Me.CheckBox_ForceTopMost.UseVisualStyleBackColor = True
        '
        'CheckBox_ProfileEnabled
        '
        Me.CheckBox_ProfileEnabled.AutoSize = True
        Me.CheckBox_ProfileEnabled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_ProfileEnabled.Location = New System.Drawing.Point(188, 21)
        Me.CheckBox_ProfileEnabled.Name = "CheckBox_ProfileEnabled"
        Me.CheckBox_ProfileEnabled.Size = New System.Drawing.Size(97, 17)
        Me.CheckBox_ProfileEnabled.TabIndex = 26
        Me.CheckBox_ProfileEnabled.Text = "Profile Enabled"
        Me.CheckBox_ProfileEnabled.UseVisualStyleBackColor = True
        '
        'CheckBox_CaptureMouse
        '
        Me.CheckBox_CaptureMouse.AutoSize = True
        Me.CheckBox_CaptureMouse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_CaptureMouse.Location = New System.Drawing.Point(6, 241)
        Me.CheckBox_CaptureMouse.Name = "CheckBox_CaptureMouse"
        Me.CheckBox_CaptureMouse.Size = New System.Drawing.Size(98, 17)
        Me.CheckBox_CaptureMouse.TabIndex = 25
        Me.CheckBox_CaptureMouse.Text = "Capture Mouse"
        Me.CheckBox_CaptureMouse.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label22.Location = New System.Drawing.Point(54, 199)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(64, 13)
        Me.Label22.TabIndex = 23
        Me.Label22.Text = "Milliseconds"
        '
        'NumericUpDown_Delay
        '
        Me.NumericUpDown_Delay.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.NumericUpDown_Delay.Location = New System.Drawing.Point(57, 215)
        Me.NumericUpDown_Delay.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.NumericUpDown_Delay.Name = "NumericUpDown_Delay"
        Me.NumericUpDown_Delay.Size = New System.Drawing.Size(97, 20)
        Me.NumericUpDown_Delay.TabIndex = 22
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label23.Location = New System.Drawing.Point(3, 217)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(34, 13)
        Me.Label23.TabIndex = 21
        Me.Label23.Text = "Delay"
        '
        'Button_Remove
        '
        Me.Button_Remove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Remove.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Button_Remove.Location = New System.Drawing.Point(188, 289)
        Me.Button_Remove.Name = "Button_Remove"
        Me.Button_Remove.Size = New System.Drawing.Size(95, 23)
        Me.Button_Remove.TabIndex = 20
        Me.Button_Remove.Text = "Remove Profile"
        Me.Button_Remove.UseVisualStyleBackColor = True
        '
        'Panel_titleRadioButtonContainer
        '
        Me.Panel_titleRadioButtonContainer.Controls.Add(Me.RadioButton_TitleIncludes)
        Me.Panel_titleRadioButtonContainer.Controls.Add(Me.RadioButton_TitleFullMatch)
        Me.Panel_titleRadioButtonContainer.Controls.Add(Me.RadioButton_TitleStartsWith)
        Me.Panel_titleRadioButtonContainer.Location = New System.Drawing.Point(57, 71)
        Me.Panel_titleRadioButtonContainer.Name = "Panel_titleRadioButtonContainer"
        Me.Panel_titleRadioButtonContainer.Size = New System.Drawing.Size(228, 22)
        Me.Panel_titleRadioButtonContainer.TabIndex = 31
        '
        'RadioButton_TitleIncludes
        '
        Me.RadioButton_TitleIncludes.AutoSize = True
        Me.RadioButton_TitleIncludes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_TitleIncludes.Location = New System.Drawing.Point(166, 2)
        Me.RadioButton_TitleIncludes.Name = "RadioButton_TitleIncludes"
        Me.RadioButton_TitleIncludes.Size = New System.Drawing.Size(65, 17)
        Me.RadioButton_TitleIncludes.TabIndex = 30
        Me.RadioButton_TitleIncludes.Tag = "2"
        Me.RadioButton_TitleIncludes.Text = "Includes"
        Me.RadioButton_TitleIncludes.UseVisualStyleBackColor = True
        '
        'RadioButton_TitleFullMatch
        '
        Me.RadioButton_TitleFullMatch.AutoSize = True
        Me.RadioButton_TitleFullMatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_TitleFullMatch.Location = New System.Drawing.Point(3, 2)
        Me.RadioButton_TitleFullMatch.Name = "RadioButton_TitleFullMatch"
        Me.RadioButton_TitleFullMatch.Size = New System.Drawing.Size(74, 17)
        Me.RadioButton_TitleFullMatch.TabIndex = 28
        Me.RadioButton_TitleFullMatch.Tag = "0"
        Me.RadioButton_TitleFullMatch.Text = "Full Match"
        Me.RadioButton_TitleFullMatch.UseVisualStyleBackColor = True
        '
        'RadioButton_TitleStartsWith
        '
        Me.RadioButton_TitleStartsWith.AutoSize = True
        Me.RadioButton_TitleStartsWith.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_TitleStartsWith.Location = New System.Drawing.Point(83, 2)
        Me.RadioButton_TitleStartsWith.Name = "RadioButton_TitleStartsWith"
        Me.RadioButton_TitleStartsWith.Size = New System.Drawing.Size(77, 17)
        Me.RadioButton_TitleStartsWith.TabIndex = 29
        Me.RadioButton_TitleStartsWith.Tag = "1"
        Me.RadioButton_TitleStartsWith.Text = "Starts With"
        Me.RadioButton_TitleStartsWith.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.GroupBox_Events, 0, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 202.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(645, 533)
        Me.TableLayoutPanel4.TabIndex = 7
        '
        'BetterFullscreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(651, 539)
        Me.Controls.Add(Me.TableLayoutPanel4)
        Me.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(667, 578)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(667, 578)
        Me.Name = "BetterFullscreen"
        Me.Padding = New System.Windows.Forms.Padding(3)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Better Fullscreen"
        Me.TopMost = True
        Me.TrayMenu.ResumeLayout(False)
        Me.GroupBox_Events.ResumeLayout(False)
        CType(Me.NumericUpDown_Left, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Top, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Height, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_Width, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_DefaultLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_DefaultTop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_DefaultHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown_DefaultWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.GroupBox_ApplicationSettings.ResumeLayout(False)
        Me.GroupBox_ApplicationSettings.PerformLayout()
        Me.GroupBox_GameSettings.ResumeLayout(False)
        Me.GroupBox_GameSettings.PerformLayout()
        CType(Me.NumericUpDown_Delay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_titleRadioButtonContainer.ResumeLayout(False)
        Me.Panel_titleRadioButtonContainer.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TrayIcon As NotifyIcon
    Friend WithEvents TrayMenu As ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReloadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToggleWindowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RichTextBox_EventLog As RichTextBox
    Friend WithEvents GroupBox_Events As GroupBox
    Friend WithEvents ComboBox_Games As ComboBox
    Friend WithEvents TextBox_Title As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox_Class As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents NumericUpDown_Height As NumericUpDown
    Friend WithEvents NumericUpDown_Width As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents NumericUpDown_Left As NumericUpDown
    Friend WithEvents NumericUpDown_Top As NumericUpDown
    Friend WithEvents Label10 As Label
    Friend WithEvents Button_Save As Button
    Friend WithEvents Button_SaveApp As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents NumericUpDown_DefaultLeft As NumericUpDown
    Friend WithEvents NumericUpDown_DefaultTop As NumericUpDown
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents NumericUpDown_DefaultHeight As NumericUpDown
    Friend WithEvents NumericUpDown_DefaultWidth As NumericUpDown
    Friend WithEvents Label18 As Label
    Friend WithEvents ComboBox_Modifier As ComboBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents ComboBox_Key As ComboBox
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents GroupBox_ApplicationSettings As GroupBox
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents GroupBox_GameSettings As GroupBox
    Friend WithEvents Button_Remove As Button
    Friend WithEvents CheckBox_StartWithWindows As CheckBox
    Friend WithEvents Label22 As Label
    Friend WithEvents NumericUpDown_Delay As NumericUpDown
    Friend WithEvents Label23 As Label
    Friend WithEvents CheckBox_CaptureMouse As CheckBox
    Friend WithEvents CheckBox_ProfileEnabled As CheckBox
    Friend WithEvents CheckBox_ForceTopMost As CheckBox
    Friend WithEvents RadioButton_TitleIncludes As RadioButton
    Friend WithEvents RadioButton_TitleStartsWith As RadioButton
    Friend WithEvents RadioButton_TitleFullMatch As RadioButton
    Friend WithEvents CheckBox_StartHidden As CheckBox
    Friend WithEvents Label24 As Label
    Friend WithEvents Panel_titleRadioButtonContainer As Panel
    Friend WithEvents Label25 As Label
    Friend WithEvents ComboBox_TriggerEvents As ComboBox
    Friend WithEvents Label26 As Label
End Class
