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
    Function BoroHearInterop(Optional ByVal content As String = Nothing) As Boolean
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-hear", True)
            If regKey Is Nothing Then
                Return False
            Else
                Try
                    Process.Start(regKey.GetValue("boro-hear"), content)
                    Return True
                Catch
                    Return False
                End Try
            End If
        Catch ex As Exception
            Console.WriteLine("[BoroHearInterop@Utility]Error: " & ex.Message)
            Return False
        End Try
    End Function
End Module
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito\boro-get"
    Public DIRRepoFile As String = DIRCommons & "\RepoList.txt"
    Public DIRPacket As String = DIRCommons & "\" & PacketName
    Public DIRPacketRepo As String = DIRPacket & "\" & PacketName & ".txt"
    Public DIRPacketFile As String = DIRPacket & "\" & PacketName & ".zip"

    Public RepoFileUrl As String

    Public isUninstall As Boolean = False
    Public MustRecharge As Boolean = False
    Public PacketName As String = Nothing
    Public PacketRunParameters As String = Nothing
    Public MustRunAtEnd As Boolean = True
End Module
Module StartUp
    Sub Init()
        Try
            DIRPacketRepo = DIRPacket & "\" & PacketName & ".txt"
            DIRPacketFile = DIRPacket & "\" & PacketName & ".zip"
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
                MustRecharge = True
            End If
            If Not My.Computer.FileSystem.DirectoryExists(DIRPacket) Then
                My.Computer.FileSystem.CreateDirectory(DIRPacket)
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
    Dim execFilePath As String
    Dim installFolder As String
    Dim WithEvents asyncDownloaderZip As New Net.WebClient
    Dim asyncDownloaderZipURI As Uri

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
            'Descargar repo zip
            asyncDownloaderZipURI = New Uri(zipFileURL)
            asyncDownloaderZip.DownloadFileAsync(asyncDownloaderZipURI, DIRPacketFile)
        Catch ex As Exception
            AddToLog("DownloadZipPackage@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Private Sub DownloadInstallPackage_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles asyncDownloaderZip.DownloadFileCompleted
        CallInstaller(DIRPacketFile)
    End Sub

    Sub CallInstaller(ByVal zipFilePath As String)
        Try
            SetInstallVars()
            If Not isUninstall Then
                If Not isInstalled(zipFilePath) Then
                    FinishInstall()
                End If
                End
            Else
                Uninstall()
                End
            End If
        Catch ex As Exception
            AddToLog("CallEXInstaller@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub

    Function isInstalled(ByVal zipFilePath As String) As Boolean
        Try
            If MustRecharge Then
                AddToLog("isInstalled@PacketAdministrator", "Recharging...", False)
                Uninstall()
                ZipFile.ExtractToDirectory(zipFilePath, installFolder)
                Return False
            End If
            If My.Computer.FileSystem.FileExists(execFilePath) Then
                AddToLog("isInstalled@PacketAdministrator", "Already exist! Running it...", False)
                MustRunAtEnd = True
                FinishInstall()
                Return True
            Else
                AddToLog("isInstalled@PacketAdministrator", "Installing...", False)
                ZipFile.ExtractToDirectory(zipFilePath, installFolder)
                Return False
            End If
        Catch ex As Exception
            AddToLog("isInstalled@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Function
    Sub FinishInstall()
        Try
            AddToLog("FinishInstall@PacketAdministrator", "Finishing...", False)
            RegisterInstall()
            If MustRunAtEnd Then
                AddToLog("FinishInstall@PacketAdministrator", "Running it...", False)
                Dim argsToRun As String = Nothing
                argsToRun = PacketRunParameters
                Process.Start(execFilePath, argsToRun)
            End If
        Catch ex As Exception
            AddToLog("FinishInstall@PacketAdministrator", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Sub SetInstallVars()
        Try
            AddToLog("SetInstallVars@PacketAdministrator", "Setting install varaibles...", False)
            installFolder = DIRCommons & "\" & PacketName
            If My.Computer.FileSystem.DirectoryExists(installFolder) Then
                My.Computer.FileSystem.CreateDirectory(installFolder)
            End If
            execFilePath = installFolder & "\" & GetIniValue("ASSEMBLY", "Executable", DIRPacketRepo)
        Catch ex As Exception
            AddToLog("SetInstallVars@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub RegisterInstall()
        Try
            AddToLog("RegisterInstall@PacketAdministrator", "Checking registrer...", False)
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get\\" & PacketName
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If regKey Is Nothing Then
                AddToLog("RegisterInstall@PacketAdministrator", "Registering...", False)
                Registry.CurrentUser.CreateSubKey(llaveReg, True)
                regKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            Else
                AddToLog("RegisterInstall@PacketAdministrator", "Already registered!", False)
            End If
            If regKey.GetValue("PacketName") = Nothing Or regKey.GetValue("Name") = Nothing Or regKey.GetValue("Version") = Nothing Then
                regKey.SetValue(PacketName, execFilePath)
                regKey.SetValue("Author", GetIniValue("GENERAL", "Author", DIRPacketRepo))
                regKey.SetValue("From", GetIniValue("GENERAL", "From", DIRPacketRepo))
                regKey.SetValue("Name", GetIniValue("ASSEMBLY", "Name", DIRPacketRepo))
                regKey.SetValue("Executable", GetIniValue("ASSEMBLY", "Executable", DIRPacketRepo))
                regKey.SetValue("Version", GetIniValue("ASSEMBLY", "Version", DIRPacketRepo))
                regKey.SetValue("Web", GetIniValue("ASSEMBLY", "Web", DIRPacketRepo))
                regKey.SetValue("Binaries", GetIniValue("INSTALLER", "Binaries", DIRPacketRepo))
                regKey.SetValue("Installer", GetIniValue("INSTALLER", "Installer", DIRPacketRepo))
                regKey.SetValue("InstallFolder", GetIniValue("INSTALLER", "InstallFolder", DIRPacketRepo))
            End If
        Catch ex As Exception
            AddToLog("RegisterInstall@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub Uninstall()
        Try
            AddToLog("Uninstall@PacketAdministrator", "Uninstalling...", False)
            Try
                Dim proc = Process.GetProcessesByName(PacketName)
                For i As Integer = 0 To proc.Count - 1
                    proc(i).Kill()
                Next i
            Catch ex As Exception
                AddToLog("Uninstall(CheckRunning)@PacketAdministrator", "Error: " & ex.Message, True)
            End Try
            If My.Computer.FileSystem.FileExists(execFilePath) Then
                My.Computer.FileSystem.DeleteFile(execFilePath)
            End If
            If My.Computer.FileSystem.DirectoryExists(installFolder) Then
                My.Computer.FileSystem.DeleteDirectory(installFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get"
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If regKey IsNot Nothing Then
                regKey.DeleteSubKeyTree(PacketName)
            End If
        Catch ex As Exception
            AddToLog("Uninstall@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub PacketInfo(ByVal packetName As String)
        Try
            Threading.Thread.Sleep(1500)
            Dim contenido As String = Nothing
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get\\" & packetName
            Dim isRunning As String = "?"
            Try
                Dim p() As Process = Process.GetProcessesByName(packetName)
                If p.Count > 0 Then
                    isRunning = "Yes"
                Else
                    isRunning = "No"
                End If
            Catch ex As Exception
                AddToLog("PacketInfo(CheckRunning)@PacketAdministrator", "Error: " & ex.Message, True)
            End Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If regKey Is Nothing Then
                contenido = "[BORO-GET STATUS] No data to read"
            Else
                contenido = vbCrLf & "[BORO-GET: STATUS.START]" &
                    vbCrLf & "Author: " & regKey.GetValue("Author") &
                    vbCrLf & "Name: " & regKey.GetValue("Name") &
                    vbCrLf & "Version: " & regKey.GetValue("Version") &
                    vbCrLf & packetName & ": " & regKey.GetValue(packetName) &
                    vbCrLf & "Binaries: " & regKey.GetValue("Binaries") &
                    vbCrLf & "isRunning: " & isRunning &
                    vbCrLf & "[BORO-GET: STATUS.END]" & vbCrLf
            End If
            BoroHearInterop(contenido)
        Catch ex As Exception
            AddToLog("PacketInfo@PacketAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module