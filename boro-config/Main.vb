Module Main
    Public myConsola As New Consola

    Sub Main(ByVal args As String())
        Console.Title = My.Application.Info.ProductName & " (" & My.Application.Info.Version.ToString & ")"
        Init()
        ReadParameters(args)
        Console.WriteLine("Handle at " & handle.ToString)
        While Not canExit
            Console.Write(ActualMode & ">")
            AddToLog(handle.ToString, PreProcessor(Console.ReadLine()), IsFlagged, SetCMDColor)
            IsFlagged = False
            SetCMDColor = ConsoleColor.White
        End While
        SaveRegedit()
    End Sub

    Function PreProcessor(ByVal command As String) As String
        Try
            Dim args() As String = command.Split(" ")
            If command.ToLower.StartsWith("consolemode") Then
                HandleConsoleMode(command)
            End If

            'Switch between console types
            If command.ToLower.StartsWith("main") Then
                ActualMode = "Main"
                Return "Switch console to '" & ActualMode & "'"
            ElseIf command.ToLower.StartsWith("component") Then
                ActualMode = "Component"
                Return "Switch console to '" & ActualMode & "'"
            Else

            End If

            'action about actual console type
            If ActualMode = "Main" Then


            ElseIf ActualMode = "Component" Then
                'tratar de obtener el nombre del componente
                'si no, desde algun componente de boro-get

            End If

            'no reach?. process it anyway.
            'se pasara por cualquier procesador de comando, esperando que alguno tome el comando y lo procese.
            Return ReadCommandLine(command)
        Catch ex As Exception
            Return AddToLog("[" & handle.ToString & "]", "Error: " & ex.Message, True)
        End Try
    End Function

    Sub HandleConsoleMode(Optional param As String = Nothing)
        Try
            If param <> Nothing Then
                ConsoleMode = param.Split("=")(1)
            End If

            If ConsoleMode = 0 Then
                ShowWindow(handle, SW_HIDE)
            ElseIf ConsoleMode = 1 Then
                ShowWindow(handle, SW_SHOW)
            Else
                Windows.Forms.Application.Run(myConsola)
            End If
        Catch ex As Exception
            AddToLog("[" & handle.ToString & "]", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module