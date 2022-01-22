Public Class Init

    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        ReadParameters(Command())
        StartUp.Init()
    End Sub

    Sub ReadParameters(ByVal parametros As String)
        Me.Hide()
        Try
            'parametros = RMTDSK|True|-ServerIP=192.168.8.175 -ServerPort=15243
            Dim args As String() = parametros.Split("|")
            PacketName = args(0) 'RMTDSK
            MustRunAtEnd = args(1) 'True
            PacketRunParameters = args(2) '-ServerIP=192.168.8.175 -ServerPort=15243
        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Class