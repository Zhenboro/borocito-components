Public Class Main

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        CheckForIllegalCrossThreadCalls = False
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
            AddToLog("SessionEvent@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Public myprocess As New Process
    Public StartInfo As New System.Diagnostics.ProcessStartInfo
    Public SR As System.IO.StreamReader
    Public SW As System.IO.StreamWriter
    Function ProcessCommand(ByVal argumentLine As String) As String
        Try
            StartInfo.FileName = "cmd"
            StartInfo.RedirectStandardInput = True
            StartInfo.RedirectStandardOutput = True
            StartInfo.CreateNoWindow = True
            StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            StartInfo.UseShellExecute = False
            myprocess.StartInfo = StartInfo
            myprocess.Start()

            SR = myprocess.StandardOutput
            SW = myprocess.StandardInput

            SW.WriteLine(argumentLine)
            SW.WriteLine("exit")

            Return SR.ReadToEnd

            'Dim info As New ProcessStartInfo
            'info.FileName = "cmd.exe"
            'info.Arguments = argumentLine
            'info.RedirectStandardOutput = True
            'info.CreateNoWindow = True
            'info.UseShellExecute = False
            'info.WindowStyle = ProcessWindowStyle.Hidden
            'info.UseShellExecute = False
            'Dim myProcess As Process = Process.Start(info)
            'Return myProcess.StandardOutput.ReadToEnd.ToString()
        Catch ex As Exception
            Return AddToLog("ProcessCommand@Main", "Error: " & ex.Message, True)
        End Try
    End Function
    'https://stackoverflow.com/questions/53464819/how-to-send-commands-to-cmd-from-vb-net
End Class