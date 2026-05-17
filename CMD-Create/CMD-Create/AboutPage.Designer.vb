<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutPage
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
        Close_Button = New Label()
        Form_Label = New Label()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        Window_Panel.SuspendLayout()
        SuspendLayout()
        ' 
        ' Window_Panel
        ' 
        Window_Panel.BackColor = Color.FromArgb(CByte(20), CByte(22), CByte(30))
        Window_Panel.Controls.Add(Close_Button)
        Window_Panel.Controls.Add(Form_Label)
        Window_Panel.Dock = DockStyle.Top
        Window_Panel.Location = New Point(0, 0)
        Window_Panel.Name = "Window_Panel"
        Window_Panel.Size = New Size(373, 40)
        Window_Panel.TabIndex = 1
        ' 
        ' Close_Button
        ' 
        Close_Button.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Close_Button.AutoSize = True
        Close_Button.Font = New Font("Segoe UI", 12F)
        Close_Button.ForeColor = Color.White
        Close_Button.Location = New Point(339, 9)
        Close_Button.Name = "Close_Button"
        Close_Button.Size = New Size(19, 21)
        Close_Button.TabIndex = 3
        Close_Button.Text = "X"
        ' 
        ' Form_Label
        ' 
        Form_Label.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Form_Label.Font = New Font("Segoe UI", 12F)
        Form_Label.ForeColor = Color.White
        Form_Label.Location = New Point(0, 9)
        Form_Label.Name = "Form_Label"
        Form_Label.Size = New Size(373, 20)
        Form_Label.TabIndex = 4
        Form_Label.Text = "About this App"
        Form_Label.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.Font = New Font("Segoe UI", 12F)
        Label1.ForeColor = Color.White
        Label1.Location = New Point(0, 52)
        Label1.Name = "Label1"
        Label1.Size = New Size(373, 20)
        Label1.TabIndex = 5
        Label1.Text = "Compressed Map Data Tool"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.Font = New Font("Segoe UI", 12F)
        Label2.ForeColor = Color.White
        Label2.Location = New Point(0, 161)
        Label2.Name = "Label2"
        Label2.Size = New Size(373, 20)
        Label2.TabIndex = 6
        Label2.Text = "Created by Miyu Melu"
        Label2.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label3.Font = New Font("Segoe UI", 12F)
        Label3.ForeColor = Color.White
        Label3.Location = New Point(0, 217)
        Label3.Name = "Label3"
        Label3.Size = New Size(373, 28)
        Label3.TabIndex = 7
        Label3.Text = "Copyright (C) 2026"
        Label3.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.Font = New Font("Segoe UI", 12F)
        Label4.ForeColor = Color.White
        Label4.Location = New Point(0, 87)
        Label4.Name = "Label4"
        Label4.Size = New Size(373, 20)
        Label4.TabIndex = 8
        Label4.Text = "CMD-Version: 0.0.001b"
        Label4.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label5.Font = New Font("Segoe UI", 12F)
        Label5.ForeColor = Color.White
        Label5.Location = New Point(0, 127)
        Label5.Name = "Label5"
        Label5.Size = New Size(373, 20)
        Label5.TabIndex = 9
        Label5.Text = "Development ID: CMDTL00001EXTBETA"
        Label5.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label6.Font = New Font("Segoe UI", 12F)
        Label6.ForeColor = Color.White
        Label6.Location = New Point(0, 181)
        Label6.Name = "Label6"
        Label6.Size = New Size(373, 20)
        Label6.TabIndex = 10
        Label6.Text = "https://miyumelu.com"
        Label6.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label7.Font = New Font("Segoe UI", 12F)
        Label7.ForeColor = Color.White
        Label7.Location = New Point(0, 107)
        Label7.Name = "Label7"
        Label7.Size = New Size(373, 20)
        Label7.TabIndex = 11
        Label7.Text = "App-Version: 1.0.0"
        Label7.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' AboutPage
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(30), CByte(32), CByte(40))
        ClientSize = New Size(373, 254)
        Controls.Add(Label7)
        Controls.Add(Label6)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Window_Panel)
        FormBorderStyle = FormBorderStyle.None
        Name = "AboutPage"
        Text = "AboutPage"
        Window_Panel.ResumeLayout(False)
        Window_Panel.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Window_Panel As Panel
    Friend WithEvents Close_Button As Label
    Friend WithEvents Form_Label As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
End Class
