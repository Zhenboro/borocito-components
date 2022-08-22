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
End Class