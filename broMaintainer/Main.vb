Public Class Main

    Private Sub Init_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        StartUp.Init()
        ReadParameters(parameters)
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
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

#Region "Maintainer"
    Function FullClear()
        Try
            Dim contenido As String = "Full clear report" & vbCrLf
            contenido &= "DeleteFiles Function" & vbCrLf
            contenido &= DeleteFiles("*.log") & vbCrLf
            contenido &= DeleteFiles("*.jpg") & vbCrLf
            contenido &= DeleteFiles("*.zip") & vbCrLf
            contenido &= DeleteFiles("*.txt")

            contenido &= vbCrLf & vbCrLf

            contenido &= "BoroGetComponents Function" & vbCrLf
            contenido &= boroGETcomponents("*.log") & vbCrLf
            contenido &= boroGETcomponents("*.zip") & vbCrLf
            contenido &= boroGETcomponents("*.avi") & vbCrLf
            contenido &= boroGETcomponents("*.wav") & vbCrLf
            contenido &= boroGETcomponents("*.jpg") & vbCrLf
            contenido &= boroGETcomponents("*.png") & vbCrLf
            contenido &= boroGETcomponents("*.gif") & vbCrLf
            contenido &= boroGETcomponents("*.mp4") & vbCrLf
            Return contenido
        Catch ex As Exception
            Return AddToLog("FullClear@Init", "Error: " & ex.Message, True)
        End Try
    End Function

    Function DeleteFiles(ByVal wildcard As String)
        Try
            Dim indexFile As Integer = 0
            For Each file As String In My.Computer.FileSystem.GetFiles(DIRCommons,
                                                                             FileIO.SearchOption.SearchTopLevelOnly,
                                                                             wildcard)
                My.Computer.FileSystem.DeleteFile(file)
                indexFile += 1
            Next
            Return "Deleted " & indexFile & " " & wildcard & " files."
        Catch ex As Exception
            Return AddToLog("DeleteFiles@Init", "Error: " & ex.Message, True)
        End Try
    End Function

    Function boroGETcomponents(ByVal wildcard As String)
        Try
            Dim indexFile As Integer = 0
            For Each file As String In My.Computer.FileSystem.GetFiles(DIRCommons & "\boro-get",
                                                                         FileIO.SearchOption.SearchAllSubDirectories,
                                                                         wildcard)
                My.Computer.FileSystem.DeleteFile(file)
                indexFile += 1
            Next
            Return "Deleted " & indexFile & " " & wildcard & " files."
        Catch ex As Exception
            Return AddToLog("boroGETcomponents@Init", "Error: " & ex.Message, True)
        End Try
    End Function
#End Region
End Class