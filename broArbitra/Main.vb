Imports System.CodeDom
Imports System.Reflection
Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hide()
        parameters = Command()
        StartUp.Init()
        ReadParameters(parameters)
        AddHandler Microsoft.Win32.SystemEvents.SessionEnding, AddressOf SessionEvent
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
        Catch ex As Exception
            AddToLog("SessionEvent@Main", "Error: " & ex.Message, True)
        End Try
    End Sub

#Region "TheCompiler"
    Dim DLL As Assembly
    Dim Proveedor As VBCodeProvider
    Dim Compilador As Compiler.ICodeCompiler
    Dim Parametros As CodeDom.Compiler.CompilerParameters
    Dim Resultado As Compiler.CompilerResults
    Dim instancia

    Function Initializer(ByVal codePATH As String, Optional ByVal param As String = Nothing) As String
        Try
            If Not isInitialized Then
                GetCodePathFile(codePATH)
                Proveedor = New VBCodeProvider
                Compilador = Proveedor.CreateCompiler
                Parametros = New CodeDom.Compiler.CompilerParameters
                Parametros.GenerateInMemory = True
                Parametros.GenerateExecutable = False
                'Parametros.OutputAssembly = ""
                If ReferencesList.Count > 0 Then
                    For Each item As String In ReferencesList
                        Parametros.ReferencedAssemblies.Add(item)
                    Next
                End If
                Resultado = Compilador.CompileAssemblyFromSource(Parametros, My.Computer.FileSystem.ReadAllText(fileCodeProvider))
                DLL = Resultado.CompiledAssembly

                instancia = DLL.CreateInstance("broCodeProvider")

                'Uso la clase y llamo al metodo, luego devuelvo lo devuelto
                Dim valorDevuelto = instancia.Initiali(param)
                isInitialized = True
                Return "Instance created! " & valorDevuelto
            Else
                Return "Instance already created!"
            End If
        Catch ex As Exception
            Return AddToLog("Initializer@Init", "Error: " & ex.Message, True)
        End Try
    End Function

    Function ArbitraCall(Optional ByVal parametro As String = Nothing) As String
        Try
            If isInitialized Then
                Dim valorDevuelto = instancia.Arbitra(parametro)
                Return valorDevuelto
            Else
                Return "No instances? You must first initialize the thing. Use '-init [param]'"
            End If
        Catch ex As Exception
            Return AddToLog("ArbitraCall@Init", "Error: " & ex.Message, True)
        End Try
    End Function
#End Region
End Class