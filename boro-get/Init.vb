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
            If parameters.Contains("|") Then
                Dim args As String() = parametros.Split("|")
                PacketName = args(0) 'RMTDSK
                MustRunAtEnd = args(1) 'True
                PacketRunParameters = args(2) '-ServerIP=192.168.8.175 -ServerPort=15243
            ElseIf parameters.Contains(" ") Then
                Dim args As String() = parametros.Split(" ")
                PacketName = args(0)
                MustRunAtEnd = Boolean.Parse(args(1))
                PacketRunParameters = " "
                For i = 2 To args.Count - 1
                    PacketRunParameters &= args(i) & " "
                Next
                PacketRunParameters = PacketRunParameters.TrimStart()
                PacketRunParameters = PacketRunParameters.TrimEnd()
            Else
                PacketName = parameters
            End If
        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Class