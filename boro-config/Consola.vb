Imports System.Windows.Forms
Public Class Consola

    Private Sub Consola_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = My.Application.Info.ProductName & " (" & My.Application.Info.Version.ToString & ")"
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            PreProcessor(TextBox1.Text)
        End If
    End Sub

    Private Sub Consola_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ConsoleMode = 1
        HandleConsoleMode()
    End Sub

    Public Sub AppendToConsole(ByVal message As String, Optional ByVal color As ConsoleColor = ConsoleColor.Black)
        Try
            RichTextBox1.AppendText(vbCrLf & message)
        Catch ex As Exception
            AddToLog("[" & Handle.ToString & "]", "Error: " & ex.Message, True)
        End Try
    End Sub
End Class