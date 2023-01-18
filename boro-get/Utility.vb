﻿Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32
Module Utility
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
        Try
            Dim OverWrite As Boolean = False
            If My.Computer.FileSystem.FileExists(DIRCommons & "\" & My.Application.Info.AssemblyName & ".log") Then
                OverWrite = True
            End If
            Dim finalContent As String = Nothing
            If flag = True Then
                finalContent = " [!!!]"
            End If
            Dim Message As String = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & finalContent & " [" & from & "] " & content
            Console.WriteLine("[" & from & "]" & finalContent & " " & content)
            Try
                My.Computer.FileSystem.WriteAllText(DIRCommons & "\" & My.Application.Info.AssemblyName & ".log", vbCrLf & Message, OverWrite)
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
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get\\boro-hear", True)
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
            AddToLog("BoroHearInterop@Utility", "Error: " & ex.Message, True)
            Return False
        End Try
    End Function
End Module
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito\boro-get"
End Module
Module StartUp
    Sub Init()
        Try
            RegisterInstance()
            Boro_Get.DIRPacket = DIRCommons & "\" & Boro_Get.PacketName
            Boro_Get.DIRPacketRepo = Boro_Get.DIRPacket & "\" & Boro_Get.PacketName & ".txt"
            Boro_Get.DIRPacketFile = Boro_Get.DIRPacket & "\" & Boro_Get.PacketName & ".zip"
            CommonActions()
            Boro_Get.GlobalVariables.GetRepoLink()
            Boro_Get.PacketAdministrator.SearchInRepoList(Boro_Get.PacketName)
        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CommonActions()
        Try
            If Not My.Computer.FileSystem.DirectoryExists(DIRCommons) Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
                Boro_Get.MustRecharge = True
            End If
            If Not My.Computer.FileSystem.DirectoryExists(Boro_Get.DIRPacket) Then
                My.Computer.FileSystem.CreateDirectory(Boro_Get.DIRPacket)
            End If
            If My.Computer.FileSystem.FileExists(Boro_Get.DIRRepoFile) Then
                My.Computer.FileSystem.DeleteFile(Boro_Get.DIRRepoFile)
            End If
            If My.Computer.FileSystem.FileExists(Boro_Get.DIRPacketRepo) Then
                My.Computer.FileSystem.DeleteFile(Boro_Get.DIRPacketRepo)
            End If
            If My.Computer.FileSystem.FileExists(Boro_Get.DIRPacketFile) Then
                My.Computer.FileSystem.DeleteFile(Boro_Get.DIRPacketFile)
            End If
        Catch ex As Exception
            AddToLog("CommonActions@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub RegisterInstance()
        Try
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get"
            Dim registerKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If registerKey IsNot Nothing Then
                registerKey.SetValue("Version", My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")")
            End If
        Catch ex As Exception
            AddToLog("RegisterInstance@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module
