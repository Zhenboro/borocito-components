Module Main

    Sub Main(ByVal args As String())
        Console.Title = My.Application.Info.ProductName & " (" & My.Application.Info.Version.ToString & ")"
        Init()
        ReadParameters(args)
        If ConsoleMode = 0 Then
            ShowWindow(handle, SW_HIDE)
            'ShowWindow(handle, SW_SHOW)
        End If
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
                ConsoleMode = command.Split("=")(1)
                If ConsoleMode = 0 Then
                    ShowWindow(handle, SW_HIDE)
                ElseIf ConsoleMode = 1 Then
                    ShowWindow(handle, SW_SHOW)
                Else
                    Dim consola As New Consola
                    Windows.Forms.Application.Run(consola)
                End If
            End If

            If ActualMode = "Component" Then

            End If
            If command.ToLower.StartsWith("main") Then
                ActualMode = "Main"
                Return "Switch console to '" & ActualMode & "'"
            ElseIf command.ToLower.StartsWith("component") Then
                ActualMode = "Component"
                Return "Switch console to '" & ActualMode & "'"
            Else
                If ActualMode = "Component" Then
                    'tratar de obtener el nombre del componente


                    'si no, desde algun componente de boro-get
                End If
            End If
            Return ReadCommandLine(command)
        Catch ex As Exception
            Return AddToLog("[" & handle.ToString & "]", "Error: " & ex.Message, True)
        End Try
    End Function
End Module