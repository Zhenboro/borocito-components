Imports Microsoft.Win32
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
        AddToLog("Init", "broEstoraje " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" & " has started! " & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy"), True)
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
                Dim args() As String = parameter.Split(" ")

                If parameter.ToLower.StartsWith("/dirtozip") Then
                    Main.DirToZip(args(1), args(2))
                ElseIf parameter.ToLower.StartsWith("/ziptodir") Then
                    Main.ZipToDir(args(1), args(2))

                ElseIf parameter.ToLower.StartsWith("/renamefile") Then
                    Main.RenameFile(args(1), args(2))
                ElseIf parameter.ToLower.StartsWith("/renamedirectory") Then
                    Main.RenameDirectory(args(1), args(2))

                ElseIf parameter.ToLower.StartsWith("/copyfile") Then
                    Main.CopyFile(args(1), args(2))
                ElseIf parameter.ToLower.StartsWith("/copydirectory") Then
                    Main.CopyDirectory(args(1), args(2))

                ElseIf parameter.ToLower.StartsWith("/movefile") Then
                    Main.MoveFile(args(1), args(2))
                ElseIf parameter.ToLower.StartsWith("/movedirectory") Then
                    Main.MoveDirectory(args(1), args(2))

                ElseIf parameter.ToLower.StartsWith("/upload") Then
                    Main.UploadFile(args(1))

                End If

            End If
            End
        Catch ex As Exception
            AddToLog("ReadParameters@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub
End Module