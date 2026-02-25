Imports Microsoft.Data.SqlClient ' 新しい推奨パッケージ
Imports System.IO
Imports System.Text

Module Module1
    Public Class SchoolOption
        Public Property Id As Integer
        Public Property Name As String
    End Class

    Public Class DbCredentials
        Public Property User As String
        Public Property Password As String
    End Class

    Private Const DefaultServer As String = "localhost"
    Private Const DefaultDatabase As String = "TestDB"
    Private Const DbIniFileName As String = "DB.ini"

    Public Function GetDbIniPath() As String
        Return Path.Combine(Application.StartupPath, DbIniFileName)
    End Function

    Public Function DbIniExists() As Boolean
        Return File.Exists(GetDbIniPath())
    End Function

    Public Function LoadDbCredentials() As DbCredentials
        Dim creds As New DbCredentials With {.User = "", .Password = ""}
        Dim iniPath = GetDbIniPath()
        If Not File.Exists(iniPath) Then
            Return creds
        End If

        Dim currentSection As String = ""
        For Each rawLine In File.ReadAllLines(iniPath, Encoding.UTF8)
            Dim line = rawLine.Trim()
            If line.Length = 0 Then Continue For
            If line.StartsWith(";", StringComparison.Ordinal) OrElse line.StartsWith("#", StringComparison.Ordinal) Then Continue For

            If line.StartsWith("[") AndAlso line.EndsWith("]") AndAlso line.Length >= 2 Then
                currentSection = line.Substring(1, line.Length - 2).Trim()
                Continue For
            End If

            Dim eq = line.IndexOf("="c)
            If eq <= 0 Then Continue For

            Dim key = line.Substring(0, eq).Trim()
            Dim value = line.Substring(eq + 1).Trim()

            If String.Equals(currentSection, "DB", StringComparison.OrdinalIgnoreCase) Then
                If String.Equals(key, "User", StringComparison.OrdinalIgnoreCase) Then
                    creds.User = value
                ElseIf String.Equals(key, "Password", StringComparison.OrdinalIgnoreCase) Then
                    creds.Password = value
                End If
            End If
        Next

        Return creds
    End Function

    Public Sub SaveDbCredentials(user As String, password As String)
        Dim iniPath = GetDbIniPath()
        Dim content As String =
$"[DB]{Environment.NewLine}User={user}{Environment.NewLine}Password={password}{Environment.NewLine}"
        File.WriteAllText(iniPath, content, Encoding.UTF8)
    End Sub

    Public Function BuildConnectionString(Optional user As String = Nothing, Optional password As String = Nothing) As String
        Dim builder As New SqlConnectionStringBuilder() With {
            .DataSource = DefaultServer,
            .InitialCatalog = DefaultDatabase,
            .TrustServerCertificate = True
        }

        If Not String.IsNullOrWhiteSpace(user) Then
            builder.IntegratedSecurity = False
            builder.UserID = user
            builder.Password = If(password, "")
        Else
            builder.IntegratedSecurity = True
        End If

        Return builder.ConnectionString
    End Function

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
            Dim creds = LoadDbCredentials()
            connectionString = BuildConnectionString(creds.User, creds.Password)
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
