﻿Imports System.IO.Compression
Imports Microsoft.Win32
Namespace Boro_Get

    Module GlobalVariables
        Public execFilePath As String
        Public installFolder As String
        Public asyncDownloaderZipURI As Uri
        Public isAlreadyInstalled As Boolean

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

        Sub GetRepoLink()
            Try
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
        Public WithEvents asyncDownloaderZip As New Net.WebClient
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
                    isAlreadyInstalled = True
                    MustRunAtEnd = True
                    FinishInstall()
                    Return True
                Else
                    AddToLog("isInstalled@PacketAdministrator", "Installing...", False)
                    isAlreadyInstalled = False
                    ZipFile.ExtractToDirectory(zipFilePath, installFolder)
                    Return False
                End If
            Catch ex As Exception
                AddToLog("isInstalled@PacketAdministrator", "Error: " & ex.Message, True)
                Return False
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
                If Not isAlreadyInstalled Then
                    Dim contenido As String = Nothing
                    contenido = vbCrLf & "[BORO-GET: PACKET.ADMIN]" &
                        vbCrLf & PacketName & " has been installed!" &
                        vbCrLf & "Run: " & MustRunAtEnd &
                        vbCrLf & "Parameters: " & PacketRunParameters &
                        vbCrLf & "[BORO-GET: PACKET.ADMIN]" & vbCrLf
                    BoroHearInterop(contenido)
                End If
            Catch ex As Exception
                AddToLog("FinishInstall@PacketAdministrator", "Error: " & ex.Message, True)
                End
            End Try
        End Sub
        Sub SetInstallVars()
            Try
                AddToLog("SetInstallVars@PacketAdministrator", "Setting install variables...", False)
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
                'If regKey.GetValue(PacketName) = Nothing Or regKey.GetValue("Name") = Nothing Or regKey.GetValue("Version") = Nothing Then
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
                'End If
            Catch ex As Exception
                AddToLog("RegisterInstall@PacketAdministrator", "Error: " & ex.Message, True)
            End Try
        End Sub
    End Module

    Module PacketManager
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
                Dim contenido As String = Nothing
                contenido = vbCrLf & "[BORO-GET: UNINSTALL.START]" &
                        vbCrLf & PacketName & " has been uninstalled!" &
                        vbCrLf & "[BORO-GET: UNINSTALL.END]" & vbCrLf
                BoroHearInterop(contenido)
            Catch ex As Exception
                AddToLog("Uninstall@PacketAdministrator", "Error: " & ex.Message, True)
            End Try
        End Sub

        Sub StopComponent()
            Try
                Dim proc = Process.GetProcessesByName(PacketName)
                If proc.Count = 0 Then
                    BoroHearInterop(PacketName & " has no instances")
                Else
                    For i As Integer = 0 To proc.Count - 1
                        proc(i).Kill()
                    Next i
                    BoroHearInterop(PacketName & " has been closed!")
                End If
            Catch ex As Exception
                AddToLog("StopComponent@PacketAdministrator", "Error: " & ex.Message, True)
            End Try
        End Sub

        Sub PacketInfo(ByVal packetName As String)
            Try
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
End Namespace
