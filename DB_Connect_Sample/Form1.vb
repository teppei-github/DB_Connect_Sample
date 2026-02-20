Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Try
            Dim schools = Module1.GetSchools()
            STEP_Scholl_Box.DropDownStyle = ComboBoxStyle.DropDownList
            STEP_Scholl_Box.DataSource = schools
            STEP_Scholl_Box.DisplayMember = NameOf(Module1.SchoolOption.Name)
            STEP_Scholl_Box.ValueMember = NameOf(Module1.SchoolOption.Id)
            If schools.Count > 0 Then
                STEP_Scholl_Box.SelectedIndex = 0
            Else
                STEP_Scholl_Box.SelectedIndex = -1
            End If
        Catch ex As Exception
            MessageBox.Show("学校一覧の取得に失敗しました。" & Environment.NewLine & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Button1.Enabled = True
        End Try
    End Sub

End Class
