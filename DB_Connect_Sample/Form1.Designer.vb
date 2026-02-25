<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Button1 = New Button()
        Button2 = New Button()
        Label1 = New Label()
        STEP_Scholl_Box = New ComboBox()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(557, 348)
        Button1.Name = "Button1"
        Button1.Size = New Size(198, 72)
        Button1.TabIndex = 0
        Button1.Text = "接続"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(340, 348)
        Button2.Name = "Button2"
        Button2.Size = New Size(198, 72)
        Button2.TabIndex = 3
        Button2.Text = "表示"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(55, 41)
        Label1.Name = "Label1"
        Label1.Size = New Size(137, 32)
        Label1.TabIndex = 1
        Label1.Text = "STEP校一覧"
        ' 
        ' STEP_Scholl_Box
        ' 
        STEP_Scholl_Box.DropDownStyle = ComboBoxStyle.DropDownList
        STEP_Scholl_Box.FormattingEnabled = True
        STEP_Scholl_Box.Location = New Point(227, 38)
        STEP_Scholl_Box.Name = "STEP_Scholl_Box"
        STEP_Scholl_Box.Size = New Size(528, 40)
        STEP_Scholl_Box.TabIndex = 2
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(13F, 32F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(STEP_Scholl_Box)
        Controls.Add(Label1)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents STEP_Scholl_Box As ComboBox

End Class
