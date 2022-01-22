Public Class Init
    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Me.Hide()
        parameters = Command()
        ReadParameters(Command())
        StartUp.Init()
    End Sub

    Sub ReadParameters(ByVal parametros As String)
        Me.Hide()
        Try

        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Class