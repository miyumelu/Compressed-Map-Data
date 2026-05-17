<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainPage
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Window_Panel = New Panel()
        Minimize_Button = New Label()
        Close_Button = New Label()
        Form_Label = New Label()
        Label1 = New Label()
        txtSource = New TextBox()
        btnSrc = New Button()
        btnDest = New Button()
        txtDest = New TextBox()
        lbl2 = New Label()
        progress = New ProgressBar()
        lblStatus = New Label()
        btnPack = New Button()
        btnTest = New Button()
        Window_Panel.SuspendLayout()
        SuspendLayout()
        ' 
        ' Window_Panel
        ' 
        Window_Panel.BackColor = Color.FromArgb(CByte(20), CByte(22), CByte(30))
        Window_Panel.Controls.Add(Minimize_Button)
        Window_Panel.Controls.Add(Close_Button)
        Window_Panel.Controls.Add(Form_Label)
        Window_Panel.Dock = DockStyle.Top
        Window_Panel.Location = New Point(0, 0)
        Window_Panel.Name = "Window_Panel"
        Window_Panel.Size = New Size(540, 40)
        Window_Panel.TabIndex = 0
        ' 
        ' Minimize_Button
        ' 
        Minimize_Button.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Minimize_Button.AutoSize = True
        Minimize_Button.Font = New Font("Segoe UI", 12F)
        Minimize_Button.ForeColor = Color.White
        Minimize_Button.Location = New Point(478, 9)
        Minimize_Button.Name = "Minimize_Button"
        Minimize_Button.Size = New Size(24, 21)
        Minimize_Button.TabIndex = 2
        Minimize_Button.Text = "M"
        ' 
        ' Close_Button
        ' 
        Close_Button.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Close_Button.AutoSize = True
        Close_Button.Font = New Font("Segoe UI", 12F)
        Close_Button.ForeColor = Color.White
        Close_Button.Location = New Point(506, 9)
        Close_Button.Name = "Close_Button"
        Close_Button.Size = New Size(19, 21)
        Close_Button.TabIndex = 1
        Close_Button.Text = "X"
        ' 
        ' Form_Label
        ' 
        Form_Label.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Form_Label.Font = New Font("Segoe UI", 12F)
        Form_Label.ForeColor = Color.White
        Form_Label.Location = New Point(0, 9)
        Form_Label.Name = "Form_Label"
        Form_Label.Size = New Size(540, 20)
        Form_Label.TabIndex = 1
        Form_Label.Text = "Compressed Map Data Tool"
        Form_Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 12F)
        Label1.ForeColor = Color.White
        Label1.Location = New Point(20, 57)
        Label1.Name = "Label1"
        Label1.Size = New Size(492, 21)
        Label1.TabIndex = 1
        Label1.Text = "DAY.MAP/NIGHT.MAP Directory (contains TileMapInfo.json and Tiles\)"
        ' 
        ' txtSource
        ' 
        txtSource.BackColor = Color.FromArgb(CByte(50), CByte(52), CByte(65))
        txtSource.BorderStyle = BorderStyle.FixedSingle
        txtSource.Font = New Font("Segoe UI", 12F)
        txtSource.ForeColor = Color.White
        txtSource.Location = New Point(20, 89)
        txtSource.Name = "txtSource"
        txtSource.Size = New Size(430, 29)
        txtSource.TabIndex = 2
        ' 
        ' btnSrc
        ' 
        btnSrc.BackColor = Color.FromArgb(CByte(60), CByte(62), CByte(80))
        btnSrc.FlatStyle = FlatStyle.Flat
        btnSrc.Font = New Font("Segoe UI", 12F)
        btnSrc.ForeColor = Color.White
        btnSrc.Location = New Point(460, 89)
        btnSrc.Name = "btnSrc"
        btnSrc.Size = New Size(60, 29)
        btnSrc.TabIndex = 3
        btnSrc.Text = "..."
        btnSrc.UseVisualStyleBackColor = False
        ' 
        ' btnDest
        ' 
        btnDest.BackColor = Color.FromArgb(CByte(60), CByte(62), CByte(80))
        btnDest.FlatStyle = FlatStyle.Flat
        btnDest.Font = New Font("Segoe UI", 12F)
        btnDest.ForeColor = Color.White
        btnDest.Location = New Point(460, 164)
        btnDest.Name = "btnDest"
        btnDest.Size = New Size(60, 29)
        btnDest.TabIndex = 6
        btnDest.Text = "..."
        btnDest.UseVisualStyleBackColor = False
        ' 
        ' txtDest
        ' 
        txtDest.BackColor = Color.FromArgb(CByte(50), CByte(52), CByte(65))
        txtDest.BorderStyle = BorderStyle.FixedSingle
        txtDest.Font = New Font("Segoe UI", 12F)
        txtDest.ForeColor = Color.White
        txtDest.Location = New Point(20, 164)
        txtDest.Name = "txtDest"
        txtDest.Size = New Size(430, 29)
        txtDest.TabIndex = 5
        ' 
        ' lbl2
        ' 
        lbl2.AutoSize = True
        lbl2.Font = New Font("Segoe UI", 12F)
        lbl2.ForeColor = Color.White
        lbl2.Location = New Point(20, 132)
        lbl2.Name = "lbl2"
        lbl2.Size = New Size(138, 21)
        lbl2.TabIndex = 4
        lbl2.Text = "Output-file (.CMD)"
        ' 
        ' progress
        ' 
        progress.Location = New Point(20, 208)
        progress.Name = "progress"
        progress.Size = New Size(500, 25)
        progress.Style = ProgressBarStyle.Continuous
        progress.TabIndex = 7
        ' 
        ' lblStatus
        ' 
        lblStatus.Font = New Font("Segoe UI", 12F)
        lblStatus.ForeColor = Color.White
        lblStatus.Location = New Point(20, 242)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(500, 22)
        lblStatus.TabIndex = 8
        lblStatus.Text = "Ready."
        ' 
        ' btnPack
        ' 
        btnPack.BackColor = Color.FromArgb(CByte(0), CByte(150), CByte(100))
        btnPack.FlatStyle = FlatStyle.Flat
        btnPack.Font = New Font("Segoe UI", 12F)
        btnPack.ForeColor = Color.White
        btnPack.Location = New Point(20, 275)
        btnPack.Name = "btnPack"
        btnPack.Size = New Size(245, 40)
        btnPack.TabIndex = 9
        btnPack.Text = "▶  Start Compression"
        btnPack.UseVisualStyleBackColor = False
        ' 
        ' btnTest
        ' 
        btnTest.BackColor = Color.FromArgb(CByte(60), CByte(62), CByte(80))
        btnTest.FlatStyle = FlatStyle.Flat
        btnTest.Font = New Font("Segoe UI", 12F)
        btnTest.ForeColor = Color.White
        btnTest.Location = New Point(275, 275)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(245, 40)
        btnTest.TabIndex = 10
        btnTest.Text = "Test File"
        btnTest.UseVisualStyleBackColor = False
        ' 
        ' MainPage
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(30), CByte(32), CByte(40))
        ClientSize = New Size(540, 340)
        Controls.Add(btnTest)
        Controls.Add(btnPack)
        Controls.Add(lblStatus)
        Controls.Add(progress)
        Controls.Add(btnDest)
        Controls.Add(txtDest)
        Controls.Add(lbl2)
        Controls.Add(btnSrc)
        Controls.Add(txtSource)
        Controls.Add(Label1)
        Controls.Add(Window_Panel)
        FormBorderStyle = FormBorderStyle.None
        Name = "MainPage"
        StartPosition = FormStartPosition.CenterScreen
        Text = "MainPage"
        Window_Panel.ResumeLayout(False)
        Window_Panel.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Window_Panel As Panel
    Friend WithEvents Form_Label As Label
    Friend WithEvents Minimize_Button As Label
    Friend WithEvents Close_Button As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSource As TextBox
    Friend WithEvents btnSrc As Button
    Friend WithEvents btnDest As Button
    Friend WithEvents txtDest As TextBox
    Friend WithEvents lbl2 As Label
    Friend WithEvents progress As ProgressBar
    Friend WithEvents lblStatus As Label
    Friend WithEvents btnPack As Button
    Friend WithEvents btnTest As Button
End Class
