﻿Imports Microsoft.Win32
Module GlobalUses
    Public parameters As String
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public DIRHome As String = DIRCommons & "\boro-get\" & My.Application.Info.AssemblyName
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
    Public UID As String
    Sub LoadRegedit()
        Try
            AddToLog("LoadRegedit@Memory", "Loading data...", False)
            UID = regKey.GetValue("UID")
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

                If parameter.ToLower.StartsWith("-init") Then
                    If args.Count = 2 Then 'no arg
                        BoroHearInterop(Main.Initializer(args(1)))
                    Else
                        BoroHearInterop(Main.Initializer(args(1), args(2)))
                    End If

                ElseIf parameter.ToLower.StartsWith("/addref") Then
                    BoroHearInterop(AddToReference(args(1)))

                ElseIf parameter.ToLower.StartsWith("/delref") Then
                    BoroHearInterop(RemoveFromReference(args(1)))

                ElseIf parameter.ToLower.StartsWith("/call") Then
                    If args.Count = 2 Then 'no param
                        BoroHearInterop(Main.ArbitraCall())
                    Else
                        BoroHearInterop(Main.ArbitraCall(args(1)))
                    End If

                ElseIf parameter.ToLower.StartsWith("-kill") Then
                    End
                End If
            Else
            End If
        Catch ex As Exception
            AddToLog("ReadParameters@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module
Module Arbitra
    Public fileCodeProvider As String
    Public isInitialized As Boolean = False
    Public ReferencesList As New ArrayList

    Function AddToReference(ByVal refPath As String) As String
        Try
            If refPath.ToLower = "basic" Then
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Core.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Data.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Data.DataSetExtensions.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Deployment.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Drawing.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Net.Http.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Windows.Forms.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Xml.dll")
                ReferencesList.Add("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Xml.Linq.dll")
                Return "Basic references added!"
            Else
                refPath = refPath.Replace("_", " ")
                ReferencesList.Add(refPath)
            End If
            Return "Reference added!"
        Catch ex As Exception
            Return AddToLog("AddToReference@Arbitra", "Error: " & ex.Message, True)
        End Try
    End Function
    Function RemoveFromReference(ByVal refPath As String) As String
        Try
            If refPath.ToLower = "basic" Then
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Core.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Data.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Data.DataSetExtensions.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Deployment.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Drawing.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Net.Http.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Windows.Forms.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Xml.dll")
                ReferencesList.Remove("C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\System.Xml.Linq.dll")
                Return "Basic references removed!"
            Else
                refPath = refPath.Replace("_", " ")
                ReferencesList.Remove(refPath)
            End If
            ReferencesList.Remove(refPath)
            Return "Reference removed!"
        Catch ex As Exception
            Return AddToLog("RemoveFromReference@Arbitra", "Error: " & ex.Message, True)
        End Try
    End Function

    Sub GetCodePathFile(ByVal thingPATH As String)
        Try
            'verificar que tipo de string se ha ingresado
            '   archivo > leer
            '   link > descargar > leer
            If thingPATH.ToLowerInvariant.StartsWith("http") Then
                If My.Computer.FileSystem.FileExists(DIRCommons & "\CodeProvider.vb") Then
                    My.Computer.FileSystem.DeleteFile(DIRCommons & "\CodeProvider.vb")
                End If
                My.Computer.Network.DownloadFile(thingPATH, DIRCommons & "\CodeProvider.vb")
            Else
                fileCodeProvider = thingPATH
            End If
        Catch ex As Exception
            AddToLog("GetCodePathFile@Arbitra", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module