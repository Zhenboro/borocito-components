Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Imports System.Net
Namespace Boro_Comm

    Module Cliente
        Public MISTREAM As Stream
        Public CLIENTE As TcpClient
        Public THREADSERVIDOR As Thread
        Public IsConnected As Boolean = False

        Sub ConnectorManager()
            Try
                If Not IsConnected Then
                    StartConnection()
                Else
                    StopConnection()
                End If
            Catch ex As Exception
                AddToLog("ConnectorManager@Boro_Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub

        Function CommandManager(ByVal command As String) As String
            Try
                If IsConnected Then
                    ENVIAR(command)
                Else
                    If StartConnection() Then
                        Return "I just connected :/"
                    Else
                        Return "Can't connect to CLI"
                    End If
                End If
                Return "Uhm, something happend!"
            Catch ex As Exception
                Return AddToLog("CommandManager@Boro_Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Function

        Function StartConnection() As Boolean
            Try
                CLIENTE = New TcpClient
                CLIENTE.Connect(IPAddress.Any, 13120)
                MISTREAM = CLIENTE.GetStream
                THREADSERVIDOR = New Thread(AddressOf LEER)
                THREADSERVIDOR.Start()
                Return True
            Catch ex As Exception
                AddToLog("StartConnection@Boro_Comm::Connector", "Error: " & ex.Message, True)
                Return False
            End Try
        End Function
        Function StopConnection() As Boolean
            Try
                CLIENTE.Close()
                THREADSERVIDOR.Abort()
                Return False
            Catch ex As Exception
                AddToLog("StopConnection@Boro_Comm::Connector", "Error: " & ex.Message, True)
                Return False
            End Try
        End Function

        Private Sub LEER()
            Dim MIBUFFER() As Byte
            While True
                Try
                    MIBUFFER = New Byte(100) {}
                    MISTREAM.Read(MIBUFFER, 0, MIBUFFER.Length)
                    Dim MENSAJE As String = Encoding.UTF7.GetString(MIBUFFER)
                    If MENSAJE.Contains("AAAAAAAAAAAAAAAAA") Then 'PARA EVITAR UN EXTRAÑO ¿ECO?
                        'ES ESE ¿ECO?
                    Else
                        'RichTextBox1.AppendText(vbCrLf & MENSAJE)
                        'RichTextBox1.ScrollToCaret()
                        If MENSAJE.StartsWith("Conexion:") Then
                            Dim MISPLIT As New ArrayList(MENSAJE.Split(":"))
                            'ListBox1.Items.Add(MISPLIT(1))
                        ElseIf MENSAJE.StartsWith("boro_conn|DISCONNECTED_BY_OWN") Then
                            StopConnection()
                        End If
                    End If
                Catch ex As Exception
                    AddToLog("LEER@Boro_Comm::Connector", "Error: " & ex.Message, True)
                    Exit While
                End Try
            End While
        End Sub

        Public Sub ENVIAR(ByVal MENSAJE As String)
            Try
                Dim MIBUFFER() As Byte
                MIBUFFER = Encoding.UTF7.GetBytes(MENSAJE)
                MISTREAM.Write(MIBUFFER, 0, MIBUFFER.Length)
            Catch ex As Exception
                AddToLog("ENVIAR@Boro_Comm::Cliente", "Error: " & ex.Message, True)
            End Try
        End Sub
    End Module

    Module Servidor
        Dim SERVIDOR As TcpListener
        Dim CLIENTES As New Hashtable
        Dim THREADSERVIDOR As Thread
        Dim CLIENTEIP As IPEndPoint
        Private Structure NUEVOCLIENTE
            Public SOCKETCLIENTE As Socket
            Public THREADCLIENTE As Thread
            Public MENSAJE As String
        End Structure

        Sub StartServer()
            Try
                SERVIDOR = New TcpListener(IPAddress.Any, 13120)
                SERVIDOR.Start()
                THREADSERVIDOR = New Thread(AddressOf ESCUCHAR)
                THREADSERVIDOR.Start()
            Catch ex As Exception
                AddToLog("ENVIAR@Boro_Comm::Servidor", "Error: " & ex.Message, True)
            End Try
        End Sub

        Public Sub ESCUCHAR()
            Dim CLIENTE As New NUEVOCLIENTE
            While True
                Try
                    CLIENTE.SOCKETCLIENTE = SERVIDOR.AcceptSocket
                    CLIENTEIP = CLIENTE.SOCKETCLIENTE.RemoteEndPoint
                    CLIENTE.THREADCLIENTE = New Thread(AddressOf LEER)
                    CLIENTES.Add(CLIENTEIP, CLIENTE)
                    'NUEVACONEXION(CLIENTEIP)
                    CLIENTE.THREADCLIENTE.Start()
                Catch ex As Exception
                End Try
            End While
        End Sub
        Public Sub LEER()
            Dim CLIENTE As New NUEVOCLIENTE
            Dim DATOS() As Byte
            Dim IP As IPEndPoint = CLIENTEIP
            CLIENTE = CLIENTES(IP)
            While True
                If CLIENTE.SOCKETCLIENTE.Connected Then
                    DATOS = New Byte(100) {}
                    Try
                        If CLIENTE.SOCKETCLIENTE.Receive(DATOS, DATOS.Length, 0) > 0 Then
                            CLIENTE.MENSAJE = Encoding.UTF7.GetString(DATOS)
                            CLIENTES(IP) = CLIENTE
                            'DatosRecibidos(IP)
                        Else

                            Exit While
                        End If
                    Catch ex As Exception

                        Exit While
                    End Try
                End If
            End While
            'Call CERRARTHREAD(IP)
        End Sub

    End Module
End Namespace