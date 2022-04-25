Public Class Init

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        StartUp.Init()
        ReadParameters(parameters)
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
    End Sub
    Sub ReadParameters(ByVal parametros As String)
        Try
            If parametros <> Nothing Then
                Dim parameter As String = parametros
                Dim args() As String = parameter.Split(" ")

                If args(0).ToLower = "/selecthk" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.SelectHKey(args(1)))

                ElseIf args(0).ToLower = "/selectkey" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.SelectKey(args(1)))

                ElseIf args(0).ToLower = "/getvalue" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.GetValue(args(1)))

                ElseIf args(0).ToLower = "/setvalue" Then
                    'Si el valor (args(2)) contiene espacios, generara un error (se mesclara en args(3) = SByte).
                    BoroHearInterop(vbCrLf & RegistryEditor.SetValue(args(1), args(2), args(3)))


                ElseIf args(0).ToLower = "/createsubkey" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.CreateSubKey(args(1)))

                ElseIf args(0).ToLower = "/deletevalue" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.DeleteValue(args(1)))

                ElseIf args(0).ToLower = "/deletesubkeytree" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.DeleteSubKeyTree(args(1)))

                ElseIf args(0).ToLower = "/deletesubkey" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.DeleteSubKey(args(1)))


                ElseIf args(0).ToLower = "/getvaluenames()" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.GetValueNames())

                ElseIf args(0).ToLower = "/getsubkeynames()" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.GetSubKeyNames())

                ElseIf args(0).ToLower = "/getvaluekind" Then
                    BoroHearInterop(vbCrLf & RegistryEditor.GetValueKind(args(1)))

                ElseIf args(0).ToLower = "/exit" Or args(0).ToLower = "/stop" Or args(0).ToLower = "/close" Then
                    End

                End If

            End If
        Catch ex As Exception
            AddToLog("ReadParameters@Init", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub SessionEvent(ByVal sender As Object, ByVal e As Microsoft.Win32.SessionEndingEventArgs)
        Try
            If e.Reason = Microsoft.Win32.SessionEndReasons.Logoff Then
                AddToLog("SessionEvent", "User is logging off!", True)
            ElseIf e.Reason = Microsoft.Win32.SessionEndReasons.SystemShutdown Then
                AddToLog("SessionEvent", "System is shutting down!", True)
            Else
                AddToLog("SessionEvent", "Something happend!", True)
            End If
        Catch ex As Exception
            AddToLog("SessionEvent@Init", "Error: " & ex.Message, True)
        End Try
    End Sub
End Class