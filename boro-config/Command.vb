Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Module Command
    <DllImport("kernel32.dll")>
    Function GetConsoleWindow() As IntPtr
    End Function
    <DllImport("user32.dll")>
    Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    Public Const SW_HIDE As Integer = 0
    Public Const SW_SHOW As Integer = 5
    Public handle = GetConsoleWindow()

    Public ActualMode As String = "Main"
    Public IsFlagged As Boolean = False
    Public SetCMDColor As ConsoleColor = ConsoleColor.White
    Public canExit As Boolean = False

    Function ReadCommandLine(ByVal command As String) As String
        Try
            Dim args() As String = command.Split(" ")
            If args(0).ToLower = "@clear" Then
                Console.Clear()

            ElseIf args(0).ToLower = "@help" Then
                Return "Find help at https://github.com/Zhenboro/borocito-components/tree/dev/boro-config"

            ElseIf args(0).ToLower = "@uninstall" Then
                Return "Ups, not coded yet!"

            ElseIf args(0).ToLower = "@exit" Then
                canExit = True
                End
            End If

            If ActualMode = "boro-config" Then
                If command.ToLower.StartsWith("set") Then
                    If args(1).ToLower.StartsWith("ownerserver") Then
                        OwnerServer = args(2)
                        Return "OwnerServer = " & OwnerServer

                    ElseIf args(1).ToLower.StartsWith("uid") Then
                        UID = args(2)
                        Return "UID = " & UID

                    ElseIf args(1).ToLower.StartsWith("consolemode") Then
                        ConsoleMode = args(2)
                        Return "ConsoleMode = " & ConsoleMode

                    Else

                        Return "No Key-Value?"
                    End If

                ElseIf command.ToLower = "@rollback" Then
                    Return "no lmaoooo"

                ElseIf command.ToLower = "@commit" Then
                    Return SaveRegedit()

                Else
                    Return "Unknow command."
                End If

            ElseIf ActualMode = "boro-get" Then
                If command.ToLower = "Main" Then
                    ActualMode = command.ToLower
                    Return "Switch console to '" & ActualMode & "'"
                Else
                    Return BoroGet_ReadCommandLine(args)
                End If
            ElseIf ActualMode = "component" Then
                If command.ToLower = "Main" Then
                    ActualMode = command.ToLower
                    Return "Switch console to '" & ActualMode & "'"
                Else
                    Return AnotherCommand(args)
                End If
            Else
            End If
            Return "*uwu*"
        Catch ex As Exception
            Return AddToLog("ReadCommandLine@boro-config", "Error: " & ex.Message, True)
        End Try
    End Function

    Function BoroGet_ReadCommandLine(ByVal command() As String) As String
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get", True)
            If regKey Is Nothing Then
                Return "boro-get is not installed."
            Else
                If command(0).ToLower.StartsWith("set") Then
                    regKey.SetValue(command(1), command(2))
                    Return command(1) & " key = " & command(2) & " value"

                ElseIf command(0).ToLower.StartsWith("remove") Then
                    regKey.DeleteValue(command(1))
                    Return command(1) & "value deleted"

                ElseIf command(0).ToLower.StartsWith("get") Then
                    Return command(1) & " = " & regKey.GetValue(command(1))

                Else
                    Return "Unknow command."
                End If
            End If
        Catch ex As Exception
            Return AddToLog("BoroGet_ReadCommandLine@boro-config", "Error: " & ex.Message, True)
        End Try
    End Function

    Function AnotherCommand(ByVal command() As String) As String
        'llamar cualquier componente de BorocitoCLI
        Try
            Dim infoSwitch As Boolean = False
            'Obtiene la linea de comandos y la separa por espacios
            Dim parametros As String = Nothing
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get\\" & command(0), True)
            'checkar que no este command() vacio
            If command.Length = 1 Then
                Return "Can't process '" & command(0) & "' without some more information."
            End If
            'Si el comando no tiene mas parametros, entonces junta y envia
            If command(1).ToLower <> "null" Then
                parametros = " "
                For i = 1 To command.Count - 1
                    If command(i) <> "+info" Then
                        parametros &= command(i) & " "
                    Else
                        infoSwitch = True
                    End If
                Next
                parametros = parametros.TrimStart()
                parametros = parametros.TrimEnd()
            End If
            'Obtiene la ruta del proceso que se quiere llamar desde el registro
            'Se llama al proceso con los argumentos
            Dim startInfo As New ProcessStartInfo
            startInfo.FileName = regKey.GetValue(command(0))
            startInfo.Arguments = parametros
            Dim Proceso As Process = Process.Start(startInfo)
            Dim procInformation As String
            If infoSwitch Then
                procInformation = "   ProcessName: " & Proceso.ProcessName &
                    vbCrLf & "  Argument: " & parametros &
                    vbCrLf & "  Id: " & Proceso.Id &
                    vbCrLf & "  MachineName: " & Proceso.MachineName &
                    vbCrLf & "  MainWindowTitle: " & Proceso.MainWindowTitle &
                    vbCrLf & "  Responding: " & Proceso.Responding &
                    vbCrLf & "  StartTime: " & Proceso.StartTime &
                    vbCrLf & "  BasePriority: " & Proceso.BasePriority
            Else
                procInformation = command(0) & " " & parametros
            End If
            Return procInformation
        Catch ex As Exception
            Return AddToLog("AnotherCommand@boro-config", "Error: " & ex.Message, True)
        End Try
    End Function

End Module