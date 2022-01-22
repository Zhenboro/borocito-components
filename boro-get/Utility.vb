Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO.Compression
Imports Microsoft.Win32
Module Utility
    Public tlmContent As String
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
        Try
            Dim OverWrite As Boolean = False
            If My.Computer.FileSystem.FileExists(DIRCommons & "\boro-get.log") Then
                OverWrite = True
            End If
            Dim finalContent As String = Nothing
            If flag = True Then
                finalContent = " [!!!]"
            End If
            Dim Message As String = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & finalContent & " [" & from & "] " & content
            tlmContent = tlmContent & Message & vbCrLf
            Console.WriteLine("[" & from & "]" & finalContent & " " & content)
            Try
                My.Computer.FileSystem.WriteAllText(DIRCommons & "\boro-get.log", vbCrLf & Message, OverWrite)
            Catch
            End Try
        Catch ex As Exception
            Console.WriteLine("[AddToLog@Utility]Error: " & ex.Message)
        End Try
    End Sub
    <DllImport("kernel32")>
    Private Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
        'Use GetIniValue("KEY_HERE", "SubKEY_HERE", "filepath")
    End Function
    Public Function GetIniValue(section As String, key As String, filename As String, Optional defaultValue As String = Nothing) As String
        Dim sb As New StringBuilder(500)
        If GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, filename) > 0 Then
            Return sb.ToString
        Else
            Return defaultValue
        End If
    End Function
End Module
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito\boro-get"
    Public DIRRepoFile As String = DIRCommons & "\RepoList.txt"
    Public DIRPacketRepo As String = DIRCommons & "\" & PacketName & ".txt"
    Public DIRPacketFile As String = DIRCommons & "\" & PacketName & ".zip"

    Public RepoFileUrl As String

    Public PacketName As String
    Public PacketRunParameters As String
    Public MustRunAtEnd As Boolean
End Module
Module StartUp
    Sub Init()
        Try
            DIRPacketRepo = DIRCommons & "\" & PacketName & ".txt"
            DIRPacketFile = DIRCommons & "\" & PacketName & ".zip"
            CommonActions()
            GetRepoLink()
            SearchInRepoList(PacketName)
        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CommonActions()
        Try
            If Not My.Computer.FileSystem.DirectoryExists(DIRCommons) Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
            If My.Computer.FileSystem.FileExists(DIRRepoFile) Then
                My.Computer.FileSystem.DeleteFile(DIRRepoFile)
            End If
            If My.Computer.FileSystem.FileExists(DIRPacketRepo) Then
                My.Computer.FileSystem.DeleteFile(DIRPacketRepo)
            End If
            If My.Computer.FileSystem.FileExists(DIRPacketFile) Then
                My.Computer.FileSystem.DeleteFile(DIRPacketFile)
            End If
        Catch ex As Exception
            AddToLog("CommonActions@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub GetRepoLink()
        Try
            'RepoFileUrl = "https://chemic-jug.000webhostapp.com/Borocito/Boro-Get/RepoList.ini"
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get", True)
            If regKey Is Nothing Then
                End
            Else
                RepoFileUrl = regKey.GetValue("RepoListURL")
            End If
        Catch ex As Exception
            AddToLog("GetRepoLink@StartUp", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Module
Module PacketAdministrator
    Sub SearchInRepoList(ByVal packetName As String)
        Try
            If My.Computer.FileSystem.FileExists(DIRRepoFile) Then
                My.Computer.FileSystem.DeleteFile(DIRRepoFile)
            End If
            'Descargar repo list
            My.Computer.Network.DownloadFile(RepoFileUrl, DIRRepoFile)
            'Verificar la existencia del paquete en el repo list
            Dim repoInfoToDownload As String = GetIniValue("REPOS", packetName, DIRRepoFile)
            If repoInfoToDownload <> Nothing Then
                'de existir, llamar a DownloadFromRepoList
                DownloadFromRepoInfo(repoInfoToDownload)
            Else
                End
            End If
        Catch ex As Exception
            AddToLog("SearchInRepoList@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Sub DownloadFromRepoInfo(ByVal repoFileLink As String)
        Try
            If My.Computer.FileSystem.FileExists(DIRPacketRepo) Then
                My.Computer.FileSystem.DeleteFile(DIRPacketRepo)
            End If
            'Descargar repo info
            My.Computer.Network.DownloadFile(repoFileLink, DIRPacketRepo)
            'verificar si existe un link
            Dim packageZipURL As String = GetIniValue("INSTALLER", "Binaries", DIRPacketRepo)
            If packageZipURL <> Nothing Then
                DownloadZipPackage(packageZipURL)
            Else
                End
            End If
        Catch ex As Exception
            AddToLog("DownloadFromRepoInfo@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Sub DownloadZipPackage(ByVal zipFileURL As String)
        Try
            If My.Computer.FileSystem.FileExists(DIRPacketFile) Then
                My.Computer.FileSystem.DeleteFile(DIRPacketFile)
            End If
            'Descargar repo info
            My.Computer.Network.DownloadFile(zipFileURL, DIRPacketFile)
            'Llamar al instalador
            CallInstaller(DIRPacketFile)
        Catch ex As Exception
            AddToLog("DownloadZipPackage@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Sub CallInstaller(ByVal zipFilePath As String)
        Try
            Install(zipFilePath)
            FinishInstall()
            End
        Catch ex As Exception
            AddToLog("CallEXInstaller@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub

    Dim installFolder As String = GetIniValue("INSTALLER", "InstallFolder", DIRPacketRepo)
    Sub Install(ByVal zipFilePath As String)
        Try
            installFolder = GetIniValue("INSTALLER", "InstallFolder", DIRPacketRepo)
            installFolder = installFolder.Replace("%temp%", "C:\Users\" & Environment.UserName & "\AppData\Local\Temp")
            installFolder = installFolder.Replace("%username%", Environment.UserName)
            installFolder = installFolder.Replace("%localappdata%", "C:\Users\" & Environment.UserName & "\AppData\Local")
            installFolder = installFolder.Replace("%appdata%", "C:\Users\" & Environment.UserName & "\AppData\Roaming")
            ZipFile.ExtractToDirectory(zipFilePath, installFolder)
        Catch ex As Exception
            AddToLog("Install@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub FinishInstall()
        Try
            If MustRunAtEnd Then
                Dim argsToRun As String = Nothing
                argsToRun = PacketRunParameters
                Process.Start(installFolder & "\" & GetIniValue("ASSEMBLY", "Executable", DIRPacketRepo), argsToRun)
            End If
        Catch ex As Exception
            AddToLog("FinishInstall@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Module