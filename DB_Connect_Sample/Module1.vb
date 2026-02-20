Imports Microsoft.Data.SqlClient ' 新しい推奨パッケージ

Module Module1
    Public Class SchoolOption
        Public Property Id As Integer
        Public Property Name As String
    End Class

    Sub DB_Connect()
        ' 接続文字列（環境に合わせて変更）
        Dim connectionString As String =
            "Server=localhost;Database=TestDB;Integrated Security=True;TrustServerCertificate=True"

        Dim query As String = "SELECT TOP 10 Id, Name FROM Users"

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Console.WriteLine("データベースに接続しました。")

                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim id As Integer = reader.GetInt32(0)
                            Dim name As String = reader.GetString(1)
                            Console.WriteLine($"ID: {id}, Name: {name}")
                        End While
                    End Using
                End Using
            End Using

        Catch ex As SqlException
            Console.WriteLine("SQLエラー: " & ex.Message)
        Catch ex As Exception
            Console.WriteLine("一般エラー: " & ex.Message)
        End Try

        Console.WriteLine("処理終了。")
    End Sub

    Public Function GetSchools(Optional connectionString As String = Nothing) As List(Of SchoolOption)
        If String.IsNullOrWhiteSpace(connectionString) Then
            connectionString = "Server=localhost;Database=TestDB;Integrated Security=True;TrustServerCertificate=True"
        End If

        Dim schools As New List(Of SchoolOption)()
        Dim query As String = "SELECT Id, Name FROM school ORDER BY Name"

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        schools.Add(New SchoolOption With {
                            .Id = reader.GetInt32(0),
                            .Name = reader.GetString(1)
                        })
                    End While
                End Using
            End Using
        End Using

        Return schools
    End Function
End Module
