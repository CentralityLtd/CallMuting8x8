<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CallMuting8x8
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CallMuting8x8))
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ScanTimer = New System.Windows.Forms.Timer(Me.components)
        Me.tTennantName = New System.Windows.Forms.TextBox()
        Me.tURL = New System.Windows.Forms.TextBox()
        Me.teMail = New System.Windows.Forms.TextBox()
        Me.tTitles = New System.Windows.Forms.TextBox()
        Me.LastActive = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ConfigPanel = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.configerrortext = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ConfigPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Call Muting (8x8)"
        Me.NotifyIcon1.Visible = True
        '
        'ScanTimer
        '
        Me.ScanTimer.Interval = 2000
        '
        'tTennantName
        '
        Me.tTennantName.Location = New System.Drawing.Point(171, 23)
        Me.tTennantName.Name = "tTennantName"
        Me.tTennantName.ReadOnly = True
        Me.tTennantName.Size = New System.Drawing.Size(271, 22)
        Me.tTennantName.TabIndex = 0
        '
        'tURL
        '
        Me.tURL.Location = New System.Drawing.Point(171, 51)
        Me.tURL.Name = "tURL"
        Me.tURL.ReadOnly = True
        Me.tURL.Size = New System.Drawing.Size(271, 22)
        Me.tURL.TabIndex = 1
        '
        'teMail
        '
        Me.teMail.Location = New System.Drawing.Point(171, 80)
        Me.teMail.Name = "teMail"
        Me.teMail.ReadOnly = True
        Me.teMail.Size = New System.Drawing.Size(271, 22)
        Me.teMail.TabIndex = 2
        '
        'tTitles
        '
        Me.tTitles.Location = New System.Drawing.Point(171, 109)
        Me.tTitles.Name = "tTitles"
        Me.tTitles.ReadOnly = True
        Me.tTitles.Size = New System.Drawing.Size(271, 22)
        Me.tTitles.TabIndex = 3
        '
        'LastActive
        '
        Me.LastActive.Location = New System.Drawing.Point(171, 137)
        Me.LastActive.Name = "LastActive"
        Me.LastActive.ReadOnly = True
        Me.LastActive.Size = New System.Drawing.Size(271, 22)
        Me.LastActive.TabIndex = 3
        Me.LastActive.Text = "<Not Active>"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Tennant Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "URL"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Current User eMail"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 17)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "# Titles"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 140)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(137, 17)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Last Active App Title"
        '
        'ConfigPanel
        '
        Me.ConfigPanel.Controls.Add(Me.Label6)
        Me.ConfigPanel.Controls.Add(Me.configerrortext)
        Me.ConfigPanel.Location = New System.Drawing.Point(12, 12)
        Me.ConfigPanel.Name = "ConfigPanel"
        Me.ConfigPanel.Size = New System.Drawing.Size(444, 169)
        Me.ConfigPanel.TabIndex = 8
        Me.ConfigPanel.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(4, 4)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(139, 17)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Configuration Errors:"
        '
        'configerrortext
        '
        Me.configerrortext.Location = New System.Drawing.Point(4, 34)
        Me.configerrortext.Multiline = True
        Me.configerrortext.Name = "configerrortext"
        Me.configerrortext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.configerrortext.Size = New System.Drawing.Size(437, 132)
        Me.configerrortext.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'CallMuting8x8
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(468, 192)
        Me.Controls.Add(Me.ConfigPanel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LastActive)
        Me.Controls.Add(Me.tTitles)
        Me.Controls.Add(Me.teMail)
        Me.Controls.Add(Me.tURL)
        Me.Controls.Add(Me.tTennantName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CallMuting8x8"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Call Muting Utility (8x8)"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.ConfigPanel.ResumeLayout(False)
        Me.ConfigPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ScanTimer As System.Windows.Forms.Timer
    Friend WithEvents tTennantName As System.Windows.Forms.TextBox
    Friend WithEvents tURL As System.Windows.Forms.TextBox
    Friend WithEvents teMail As System.Windows.Forms.TextBox
    Friend WithEvents tTitles As System.Windows.Forms.TextBox
    Friend WithEvents LastActive As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ConfigPanel As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents configerrortext As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
