Public Class Init
    Public BypassInit As Boolean = False

    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        ReadParameters(Command())
        If Not BypassInit Then
            StartUp.Init()
        Else
            End
        End If
    End Sub

    Sub ReadParameters(ByVal parametros As String)
        Me.Hide()
        Try
            If parameters.Contains(" ") Then
                '<packetName> <Run?/actionCommand> <parameters...>
                Dim args As String() = parametros.Split(" ")
                PacketName = args(0)
                If args(1).ToLower = "uninstall" Then 'Comandos de accion para componentes
                    isUninstall = True
                ElseIf args(1).ToLower = "status" Then
                    BypassInit = True
                    PacketInfo(args(0))
                ElseIf args(1).ToLower = "stop" Then
                    BypassInit = True
                    StopComponent()
                Else
                    MustRunAtEnd = Boolean.Parse(args(1))
                    If args(2).ToLower <> "null" Then
                        PacketRunParameters = " "
                        For i = 2 To args.Count - 1
                            PacketRunParameters &= args(i) & " "
                        Next
                        PacketRunParameters = PacketRunParameters.TrimStart()
                        PacketRunParameters = PacketRunParameters.TrimEnd()
                    End If
                End If
            Else
                PacketName = parameters
            End If
            AddToLog("BORO-GET", "Instance for " & PacketName, True)
        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Class