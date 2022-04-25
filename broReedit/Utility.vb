Imports Microsoft.Win32
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
                        Process.Start(regKey.GetValue("boro-hear"), content)
                    End If
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
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Microsoft\Borocito"
    Public DIRHome As String = DIRCommons & "\boro-get\" & My.Application.Info.AssemblyName
End Module
Module StartUp
    Sub Init()
        AddToLog("Init", My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" & " has started! " & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy"), True)
        Try
            CommonActions()
            RegisterInstance()
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
Module RegistryEditor
    Dim currentHiveKey As String
    Dim currentHiveKeyReg As RegistryKey
    Dim currentKey As String

    Function SelectHKey(ByVal hiveKey As String) As String 'Funciona 24/04 14:37
        Try
            currentHiveKey = hiveKey
            Select Case hiveKey
                Case "ClassesRoot"
                    currentHiveKeyReg = Registry.ClassesRoot
                Case "CurrentConfig"
                    currentHiveKeyReg = Registry.CurrentConfig
                Case "CurrentUser"
                    currentHiveKeyReg = Registry.CurrentUser
                Case "LocalMachine"
                    currentHiveKeyReg = Registry.LocalMachine
                Case "PerformanceData"
                    currentHiveKeyReg = Registry.PerformanceData
                Case "Users"
                    currentHiveKeyReg = Registry.Users
                Case Else
                    currentHiveKeyReg = Registry.CurrentUser
            End Select
            Return "HiveKey selected " & currentHiveKey
        Catch ex As Exception
            Return AddToLog("SelectHKey@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function SelectKey(ByVal nameKey As String) As String 'Funciona 24/04 14:38
        Try
            currentKey = nameKey
            currentHiveKeyReg = Registry.CurrentUser.OpenSubKey(nameKey, True)
            Return "Key selected " & currentKey
        Catch ex As Exception
            Return AddToLog("SelectKey@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function

    Function GetValue(ByVal valueName As String) As String 'Funciona 24/04 14:39
        Try
            Return currentHiveKeyReg.GetValue(valueName)
        Catch ex As Exception
            Return AddToLog("GetValue@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function SetValue(ByVal valueName As String, ByVal value As String, Optional ByVal valueKind As String = Nothing) As String  'Funciona 24/04 14:44
        Try
            If valueKind = Nothing Or valueKind.ToLower = "null" Then
                valueKind = 1
            End If
            currentHiveKeyReg.SetValue(valueName, value, valueKind)
            Return "Seted! Name " & valueName & " Value " & value
        Catch ex As Exception
            Return AddToLog("SetValue@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function DeleteValue(ByVal valueName As String) As String 'Funciona 24/04 14:56
        Try
            currentHiveKeyReg.DeleteValue(valueName)
            Return "Deleted! " & valueName
        Catch ex As Exception
            Return AddToLog("DeleteValue@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function

    Function CreateSubKey(ByVal subKey As String) As String 'Funciona 24/04 15:04
        Try
            currentHiveKeyReg.CreateSubKey(subKey, True)
            Return "Key created! " & subKey
        Catch ex As Exception
            Return AddToLog("CreateSubKey@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function DeleteSubKey(ByVal subKey As String) As String 'Funciona 24/04 15:11
        Try
            currentHiveKeyReg.DeleteSubKey(subKey)
            Return "Key deleted! " & subKey
        Catch ex As Exception
            Return AddToLog("DeleteSubKey@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function DeleteSubKeyTree(ByVal subKey As String) As String 'Funciona 24/04 15:12
        Try
            currentHiveKeyReg.DeleteSubKeyTree(subKey)
            Return "Key tree deleted! " & subKey
        Catch ex As Exception
            Return AddToLog("DeleteSubKeyTree@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function

    Function GetValueNames() As String  'Funciona 24/04 15:44
        Try
            Dim contenido As String = Nothing
            For Each item As String In currentHiveKeyReg.GetValueNames()
                contenido &= item & vbCrLf
            Next
            Return contenido
        Catch ex As Exception
            Return AddToLog("GetValueNames@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function GetSubKeyNames() As String 'Funciona 24/04 15:45
        Try
            Dim contenido As String = Nothing
            For Each item As String In currentHiveKeyReg.GetSubKeyNames()
                contenido &= item & vbCrLf
            Next
            Return contenido
        Catch ex As Exception
            Return AddToLog("GetSubKeyNames@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
    Function GetValueKind(ByVal valueName As String) As String 'Funciona 24/04 15:46
        Try
            Return currentHiveKeyReg.GetValueKind(valueName)
        Catch ex As Exception
            Return AddToLog("GetValueKind@RegistryEditor", "Error: " & ex.Message, True)
        End Try
    End Function
End Module