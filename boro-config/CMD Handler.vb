Public Class CMD_Handler
    Public Sub New()

    End Sub

    Function GeneralConsoleReader(ByVal command As String) As String
        Try
            Dim commandArgs() As String = command.Split(" ")

            ConsoleController(command)

            Return ""
        Catch ex As Exception
            Return AddToLog("GeneralConsoleReader@CMD Handler", "Error: " & ex.Message, True)
        End Try
    End Function

    Function ConsoleController(ByVal command As String) As String
        Try
            Dim commandArgs() As String = command.Split(" ")

            If commandArgs(0).ToLower = "consolemode" Then
                ConsoleMode = commandArgs(1).ToLower

            End If
            Return ""
        Catch ex As Exception
            Return AddToLog("ConsoleController@CMD Handler", "Error: " & ex.Message, True)
        End Try
    End Function
End Class