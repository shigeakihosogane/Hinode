Public Class UserControl1
    Private Sub UserControl1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        参照(Form1.UserControl1.TextBox1)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        参照(Form1.UserControl1.TextBox2)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        参照(Form1.UserControl1.TextBox3)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        参照(Form1.UserControl1.TextBox4)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        参照(Form1.UserControl1.TextBox5)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        参照(Form1.UserControl1.TextBox6)
    End Sub

    Private Sub 参照(ByVal tb As TextBox)
        If Form1.CheckBox1.Checked = True Then
            MsgBox("稼働中はフォルダを変更できません")
        Else
            Dim fbd As New FolderBrowserDialog
            fbd.Description = "フォルダを指定してください。"
            fbd.RootFolder = Environment.SpecialFolder.Desktop
            fbd.SelectedPath = "C:\Windows"
            fbd.ShowNewFolderButton = True

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                tb.Text = fbd.SelectedPath
            End If
        End If


    End Sub

End Class
