Imports Microsoft.Data.SqlClient

Public Class Form3
    Inherits Form

    Private ReadOnly txtUser As New TextBox()
    Private ReadOnly txtPassword As New TextBox()
    Private ReadOnly btnConnect As New Button()
    Private ReadOnly btnCancel As New Button()

    Public Sub New()
        Text = "DB接続"
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ShowInTaskbar = False

        InitializeComponent()
        AddHandler Shown, AddressOf Form3_Shown
    End Sub

    Private Sub InitializeComponent()
        Dim layout As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 4,
            .Padding = New Padding(12)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 44))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 44))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 56))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim lblUser As New Label() With {.Text = "ユーザー", .AutoSize = True, .Anchor = AnchorStyles.Left}
        Dim lblPassword As New Label() With {.Text = "パスワード", .AutoSize = True, .Anchor = AnchorStyles.Left}

        txtUser.Dock = DockStyle.Fill
        txtPassword.Dock = DockStyle.Fill
        txtPassword.UseSystemPasswordChar = True

        btnConnect.Text = "接続"
        btnConnect.AutoSize = True
        AddHandler btnConnect.Click, AddressOf BtnConnect_Click

        btnCancel.Text = "キャンセル"
        btnCancel.AutoSize = True
        AddHandler btnCancel.Click,
            Sub()
                DialogResult = DialogResult.Cancel
                Close()
            End Sub

        Dim buttons As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.RightToLeft,
            .WrapContents = False
        }
        buttons.Controls.Add(btnConnect)
        buttons.Controls.Add(btnCancel)

        layout.Controls.Add(lblUser, 0, 0)
        layout.Controls.Add(txtUser, 1, 0)
        layout.Controls.Add(lblPassword, 0, 1)
        layout.Controls.Add(txtPassword, 1, 1)
        layout.SetColumnSpan(buttons, 2)
        layout.Controls.Add(buttons, 0, 2)

        Controls.Add(layout)

        AcceptButton = btnConnect
        CancelButton = btnCancel
        ClientSize = New Size(520, 180)
    End Sub

    Private Sub Form3_Shown(sender As Object, e As EventArgs)
        Dim creds = Module1.LoadDbCredentials()
        txtUser.Text = creds.User
        txtPassword.Text = creds.Password
        txtUser.Select()
    End Sub

    Private Sub BtnConnect_Click(sender As Object, e As EventArgs)
        btnConnect.Enabled = False
        Try
            Dim user = txtUser.Text.Trim()
            Dim password = txtPassword.Text

            Module1.SaveDbCredentials(user, password)

            Dim connectionString = Module1.BuildConnectionString(user, password)
            Using conn As New SqlConnection(connectionString)
                conn.Open()
            End Using

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("接続に失敗しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnConnect.Enabled = True
        End Try
    End Sub
End Class

