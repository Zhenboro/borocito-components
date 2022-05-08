﻿Imports Microsoft.Win32
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public DIRHome As String = DIRCommons & "\boro-get\" & My.Application.Info.AssemblyName
    Public HttpOwnerServer As String
End Module
Module Utility
    Public tlmContent As String
    Function AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False) As String
        Try
            Dim OverWrite As Boolean = False
            If My.Computer.FileSystem.FileExists(DIRHome & "\" & My.Application.Info.AssemblyName & ".log") Then
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
                My.Computer.FileSystem.WriteAllText(DIRHome & "\" & My.Application.Info.AssemblyName & ".log", vbCrLf & Message, OverWrite)
            Catch
            End Try
            Return finalContent & "[" & from & "]" & content
        Catch ex As Exception
            Console.WriteLine("[AddToLog@Utility]Error: " & ex.Message)
            Return "[AddToLog@Utility]Error: " & ex.Message
        End Try
    End Function
    Function BoroHearInterop(Optional ByVal content As String = Nothing) As Boolean
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-get\\boro-hear", True)
            If regKey Is Nothing Then
                Return False
            Else
                Try
                    If content <> Nothing Then
                        AddToLog("BoroHearInterop", content, False)
                        Process.Start(regKey.GetValue("boro-hear"), content)
                    End If
                    Return True
                Catch
                    Return False
                End Try
            End If
        Catch ex As Exception
            Console.WriteLine("[BoroHearInterop@Init]Error: " & ex.Message)
            Return False
        End Try
    End Function
End Module
Module Memory
    Public regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
    Public OwnerServer As String
    Public UID As String
    Sub LoadRegedit()
        Try
            AddToLog("LoadRegedit@Memory", "Loading data...", False)
            OwnerServer = regKey.GetValue("OwnerServer")
            UID = regKey.GetValue("UID")
            HttpOwnerServer = "http://" & OwnerServer
            RegisterInstance()
        Catch ex As Exception
            AddToLog("LoadRegedit@Memory", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub RegisterInstance()
        Try
            Dim llaveReg As String = "SOFTWARE\\Borocito\\boro-get\\" & My.Application.Info.AssemblyName
            Dim registerKey As RegistryKey = Registry.CurrentUser.OpenSubKey(llaveReg, True)
            If registerKey IsNot Nothing Then
                registerKey.SetValue("Version", My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")")
            End If
        Catch ex As Exception
            AddToLog("RegisterInstance@Memory", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module
Module StartUp
    Sub Init()
        AddToLog("Init", My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" & " has started! " & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy"), True)
        Try
            CommonActions()
            'Cargamos los datos del registro de Windows
            LoadRegedit()
        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CommonActions()
        Try
            If Not My.Computer.FileSystem.DirectoryExists(DIRCommons) Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
            If Not My.Computer.FileSystem.DirectoryExists(DIRHome) Then
                My.Computer.FileSystem.CreateDirectory(DIRHome)
            End If
        Catch ex As Exception
            AddToLog("CommonActions@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub ReadParameters(ByVal parametros As String)
        Try
            If parametros <> Nothing Then
                Dim parameter As String = parametros
                Dim args() As String = parameter.Split(" ")

                If parameter.ToLower.StartsWith("/dirtozip") Then
                    BoroHearInterop(Main.DirToZip(args(1), args(2)))
                ElseIf parameter.ToLower.StartsWith("/ziptodir") Then
                    BoroHearInterop(Main.ZipToDir(args(1), args(2)))

                ElseIf parameter.ToLower.StartsWith("/renamefile") Then
                    BoroHearInterop(Main.RenameFile(args(1), args(2)))
                ElseIf parameter.ToLower.StartsWith("/renamedirectory") Then
                    BoroHearInterop(Main.RenameDirectory(args(1), args(2)))

                ElseIf parameter.ToLower.StartsWith("/copyfile") Then
                    BoroHearInterop(Main.CopyFile(args(1), args(2)))
                ElseIf parameter.ToLower.StartsWith("/copydirectory") Then
                    BoroHearInterop(Main.CopyDirectory(args(1), args(2)))

                ElseIf parameter.ToLower.StartsWith("/movefile") Then
                    BoroHearInterop(Main.MoveFile(args(1), args(2)))
                ElseIf parameter.ToLower.StartsWith("/movedirectory") Then
                    BoroHearInterop(Main.MoveDirectory(args(1), args(2)))

                ElseIf parameter.ToLower.StartsWith("/getfiles") Then
                    BoroHearInterop(Main.GetFiles(args(1)))
                ElseIf parameter.ToLower.StartsWith("/getdirectories") Then
                    BoroHearInterop(Main.GetDirectories(args(1)))

                ElseIf parameter.ToLower.StartsWith("/fileinfo") Then
                    BoroHearInterop(Main.FileInfo(args(1)))
                ElseIf parameter.ToLower.StartsWith("/directoryinfo") Then
                    BoroHearInterop(Main.DirectoryInfo(args(1)))

                ElseIf parameter.ToLower.StartsWith("/upload") Then
                    BoroHearInterop(Main.UploadFile(args(1)))

                End If

            End If
            End
        Catch ex As Exception
            AddToLog("ReadParameters@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module