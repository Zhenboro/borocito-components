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
                Boro_Get.PacketName = args(0)
                AddToLog("BORO-GET", "Instance for " & Boro_Get.PacketName, True)
                If args(1).ToLower = "uninstall" Then 'Comandos de accion para componentes
                    BypassInit = True
                    Boro_Get.isUninstall = True
                    Boro_Get.Uninstall()
                ElseIf args(1).ToLower = "status" Then
                    BypassInit = True
                    Boro_Get.PacketInfo(args(0))
                ElseIf args(1).ToLower = "installed" Then
                    BypassInit = True
                    Boro_Get.IndexEveryInstalledComponent()
                ElseIf args(1).ToLower = "stop" Then
                    BypassInit = True
                    Boro_Get.StopComponent()
                Else
                    Boro_Get.MustRunAtEnd = Boolean.Parse(args(1))
                    If args(2).ToLower <> "null" Then
                        Boro_Get.PacketRunParameters = " "
                        For i = 2 To args.Count - 1
                            Boro_Get.PacketRunParameters &= args(i) & " "
                        Next
                        Boro_Get.PacketRunParameters = Boro_Get.PacketRunParameters.TrimStart()
                        Boro_Get.PacketRunParameters = Boro_Get.PacketRunParameters.TrimEnd()
                    End If
                End If
            Else
                Boro_Get.PacketName = parameters
            End If
        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Class
