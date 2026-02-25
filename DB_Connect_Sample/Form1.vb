Public Class Form1
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If Not Module1.DbIniExists() Then
            Return
        End If

        Try
            LoadSchools()
        Catch ex As Exception
            MessageBox.Show("学校一覧の取得に失敗しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadSchools()
        Dim creds = Module1.LoadDbCredentials()
        Dim connectionString = Module1.BuildConnectionString(creds.User, creds.Password)

        Dim schools = Module1.GetSchools(connectionString)
        STEP_Scholl_Box.DropDownStyle = ComboBoxStyle.DropDownList
        STEP_Scholl_Box.DataSource = schools
        STEP_Scholl_Box.DisplayMember = NameOf(Module1.SchoolOption.Name)
        STEP_Scholl_Box.ValueMember = NameOf(Module1.SchoolOption.Id)

        If schools.Count > 0 Then
            STEP_Scholl_Box.SelectedIndex = 0
        Else
            STEP_Scholl_Box.SelectedIndex = -1
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Try
            Using f As New Form3()
                If f.ShowDialog(Me) = DialogResult.OK Then
                    LoadSchools()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("接続処理に失敗しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Button1.Enabled = True
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedObj = STEP_Scholl_Box.SelectedValue
        If selectedObj Is Nothing Then
            MessageBox.Show("STEP校が選択されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedId As Integer
        If TypeOf selectedObj Is Integer Then
            selectedId = CInt(selectedObj)
        ElseIf Not Integer.TryParse(selectedObj.ToString(), selectedId) Then
            MessageBox.Show("STEP校の選択値が不正です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim f As New Form2(selectedId)
        f.Show(Me)
    End Sub

End Class
