Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Text
Namespace Boro_Comm

    Module Connector
        Dim MISTREAM As Stream
        Dim CLIENTE As TcpClient
        Dim THREADSERVIDOR As Thread
        Dim IsConnected As Boolean = False

        Sub ConnectorManager()
            Try
                If Not IsConnected Then
                    ConnectTo()
                Else
                    DisconnectFrom()
                End If
            Catch ex As Exception
                AddToLog("ConnectorManager@Boro-Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub
        Sub ConnectTo()
            Try
                IsConnected = True
                CLIENTE = New TcpClient
                CLIENTE.Connect("localhost", 13120)
                MISTREAM = CLIENTE.GetStream
                THREADSERVIDOR = New Thread(AddressOf LEER)
                THREADSERVIDOR.Start()
            Catch ex As Exception
                AddToLog("ConnectTo@Boro-Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub
        Sub DisconnectFrom()
            Try
                IsConnected = False
                Try
                    CLIENTE.Close()
                    THREADSERVIDOR.Abort()
                Catch
                End Try
            Catch ex As Exception
                AddToLog("DisconnectFrom@Boro-Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub

        Private Sub LEER()
            Try
                Dim MIBUFFER() As Byte
                While True
                    Try
                        MIBUFFER = New Byte(100) {}
                        MISTREAM.Read(MIBUFFER, 0, MIBUFFER.Length)
                        Dim MENSAJE As String = Encoding.UTF7.GetString(MIBUFFER)
                        If MENSAJE.Contains("AAAAAAAAAAAAAAAAA") Then 'PARA EVITAR UN EXTRAÑO ¿ECO?
                            'ES ESE ¿ECO?
                        Else
                            If MENSAJE.StartsWith("") Then

                            ElseIf MENSAJE.StartsWith("") Then

                            End If
                        End If
                    Catch ex As Exception
                        AddToLog("LEER.While@Boro-Comm::Connector", "Error: " & ex.Message, True)
                        Exit While
                    End Try
                End While
                DisconnectFrom()
            Catch ex As Exception
                AddToLog("LEER@Boro-Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub
        Public Sub ENVIAR(ByVal MENSAJE As String, Optional ByVal usePrefix As Boolean = True)
            Try
                If usePrefix Then
                    MENSAJE = "Boro-Hear> " & MENSAJE
                End If
                Dim MIBUFFER() As Byte
                MIBUFFER = Encoding.UTF7.GetBytes(MENSAJE)
                MISTREAM.Write(MIBUFFER, 0, MIBUFFER.Length)
            Catch ex As Exception
                AddToLog("ENVIAR@Boro-Comm::Connector", "Error: " & ex.Message, True)
            End Try
        End Sub
    End Module

End Namespace
