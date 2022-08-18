Imports System.Runtime.InteropServices
Module Main
    <DllImport("kernel32.dll")>
    Function GetConsoleWindow() As IntPtr
    End Function
    <DllImport("user32.dll")>
    Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    Private Const SW_HIDE As Integer = 0
    Private Const SW_SHOW As Integer = 5

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
        While Not canExit
            AddToLog(handle.ToString, ReadCommandLine(Console.ReadLine()), False)
        End While
        SaveRegedit()
    End Sub

    Function ReadCommandLine(ByVal command As String) As String
        Try

            If command.ToLower.StartsWith("--consolemode") Then
                ConsoleMode = command.Split("=")(1)

            ElseIf command.ToLower = "//" Then

            ElseIf command.ToLower = "@exit" Then
                canExit = True

            Else
                Return "Can't proccess."
            End If

            Return "No proccessed."
        Catch ex As Exception
            Return AddToLog("ReadCommandLine@boro-config", "Error: " & ex.Message, True)
        End Try
    End Function

End Module