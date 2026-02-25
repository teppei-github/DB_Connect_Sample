Imports Microsoft.Data.SqlClient
Imports System.Data

Public Class Form2
    Inherits Form

    Private ReadOnly _schoolId As Integer
    Private ReadOnly dgv As New DataGridView()

    Public Sub New(selectedSchoolId As Integer)
        _schoolId = selectedSchoolId

        Text = "表示"
        StartPosition = FormStartPosition.CenterParent
        InitializeComponent()

        AddHandler Shown, AddressOf Form2_Shown
    End Sub

    Private Sub InitializeComponent()
        Dim root As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2,
            .Padding = New Padding(12)
        }
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 36))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim lbl As New Label() With {
            .Text = $"STEP校ID: {_schoolId}",
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft
        }

        dgv.Dock = DockStyle.Fill
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        root.Controls.Add(lbl, 0, 0)
        root.Controls.Add(dgv, 0, 1)

        Controls.Add(root)
        ClientSize = New Size(900, 600)
    End Sub

    Private Sub Form2_Shown(sender As Object, e As EventArgs)
        Try
            Dim dt As New DataTable()
            Dim creds = Module1.LoadDbCredentials()
            Dim connectionString = Module1.BuildConnectionString(creds.User, creds.Password)

            Dim sql As String = "SELECT Id, Name FROM school WHERE Id = @SchoolId"

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.Add("@SchoolId", SqlDbType.Int).Value = _schoolId
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            dgv.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("データの取得に失敗しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class

