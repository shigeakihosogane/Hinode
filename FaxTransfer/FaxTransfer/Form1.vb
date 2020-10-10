Imports System
Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.Threading
Imports System.Globalization

Public Class Form1

    Dim cnstr As String = "Data Source=SV201710;Initial Catalog=HINODEDB;USER ID=sa;PASSWORD=cube%6614;"

    Private watcher As System.IO.FileSystemWatcher = Nothing
    Private swatcher As System.IO.FileSystemWatcher = Nothing

    Dim tennsoumoto As String
    Dim kannsi As String
    Dim tennsousaki As String
    Dim scan As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SettingClass.設定読込()

        'txt転送元.Text = "\\192.168.2.240\fax受信"
        'txt監視.Text = "\\192.168.2.240\fax転送"
        'txt転送先.Text = "\\192.168.2.240\ThereforeDATA"

    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If lblステータス.Text = "稼働中" Then
            MsgBox("稼働中は閉じることができません！")
            e.Cancel = True
        Else
            SettingClass.設定保存()
        End If

    End Sub

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click

        If lblステータス.Text = "停止中" Then
            If txt転送元.Text <> "" And txt監視.Text <> "" And txt転送先.Text <> "" And txtscan.Text <> "" Then
                lblステータス.Text = "稼働中"
                btn1.Text = "停止"
                txt転送元.ReadOnly = True
                txt監視.ReadOnly = True
                txt転送先.ReadOnly = True
                txtscan.ReadOnly = True
                Therefore転送開始()
                scan転送開始()
                MsgBox("転送を開始しました。")
            Else
                MsgBox("フォルダを選択してください。")
            End If
        Else
            Dim result As DialogResult = MessageBox.Show("FAX転送を停止しますか？", "確認メッセージ",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2)
            If result = DialogResult.OK Then
                Dim inputText As String
                inputText = InputBox("パスワードを入力してください。", "確認メッセージ", "", Me.Left + 300, Me.Top + 200)
                If inputText = "Hinode8739" Then
                    lblステータス.Text = "停止中"
                    btn1.Text = "開始"
                    txt転送元.ReadOnly = False
                    txt監視.ReadOnly = False
                    txt転送先.ReadOnly = False
                    txtscan.ReadOnly = False
                    Therefore転送停止()
                    scan転送停止()
                    MsgBox("転送を停止しました。")
                Else
                    MsgBox("パスワードが違います！")
                End If

            End If

        End If

    End Sub

    Public Sub Therefore転送開始()

        If Not (watcher Is Nothing) Then
            Return
        End If

        watcher = New System.IO.FileSystemWatcher

        tennsoumoto = txt転送元.Text
        kannsi = txt監視.Text
        tennsousaki = txt転送先.Text

        '監視するディレクトリを指定
        watcher.Path = kannsi

        '最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
        watcher.NotifyFilter = System.IO.NotifyFilters.FileName

        'すべてのファイルを監視
        watcher.Filter = ""

        'UIのスレッドにマーシャリングする
        'コンソールアプリケーションでの使用では必要ない
        watcher.SynchronizingObject = Me

        'イベントハンドラの追加
        'AddHandler watcher.Changed, AddressOf watcher_Changed
        'AddHandler watcher.Created, AddressOf watcher_Changed
        'AddHandler watcher.Deleted, AddressOf watcher_Changed
        AddHandler watcher.Renamed, AddressOf watcher_Renamed

        '監視を開始する
        watcher.EnableRaisingEvents = True
        Console.WriteLine("Therefor転送を開始しました。")


    End Sub

    Public Sub Therefore転送停止()

        '監視を終了
        watcher.EnableRaisingEvents = False
        watcher.Dispose()
        watcher = Nothing
        Console.WriteLine("Therefor転送を終了しました。")

    End Sub

    Private Sub watcher_Changed(ByVal source As System.Object, ByVal e As System.IO.FileSystemEventArgs)

        Select Case e.ChangeType
            Case System.IO.WatcherChangeTypes.Changed
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が変更されました。"))
            Case System.IO.WatcherChangeTypes.Deleted
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が削除されました。"))
            Case System.IO.WatcherChangeTypes.Created
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が作成されました。"))
        End Select

    End Sub
    Private Sub watcher_Renamed(ByVal source As System.Object, ByVal e As System.IO.RenamedEventArgs)

        'Console.WriteLine(("ファイル 「" + e.FullPath + "」の名前が変更されました。"))

        Dim fname As String = Path.GetFileNameWithoutExtension(e.FullPath) 'ファイル名
        Dim kakutyousi As String = Path.GetExtension(e.FullPath) '拡張子

        '        fname = Replace(fname, Space(1), String.Empty)
        Dim fnarray As Array = Split(fname, "_")

        Dim flg As Boolean = True
        Dim errorstr As String = ""

        If IsNumeric(fnarray(0)) Then '-数値

            Dim zyutyuuCD As Decimal = CDec(fnarray(0))

            Dim sqlstr As String = "SELECT dbo.T_TF_D_Index.受注ID, dbo.T_TF_D_Index.開始日, dbo.T_TF_D_Index.終了日, dbo.T_TF_D_Index.担当部署, dbo.T_TF_D_Index.備考, dbo.T_TF_D_Index.荷主ID, dbo.T_TF_M_NinusiInfo.名称_HND, dbo.T_TF_M_NinusiInfo.名称_TF, dbo.T_TF_M_NinusiInfo.FAX番号 "
            sqlstr &= "FROM dbo.T_TF_D_Index LEFT OUTER JOIN dbo.T_TF_M_NinusiInfo ON dbo.T_TF_D_Index.荷主ID = dbo.T_TF_M_NinusiInfo.荷主ID "
            sqlstr &= "WHERE (dbo.T_TF_D_Index.受注ID = " & zyutyuuCD & ")"


            Dim cn As SqlClient.SqlConnection
            Dim cmd As SqlClient.SqlCommand
            Dim reader As SqlClient.SqlDataReader

            Dim sdate As String = ""
            Dim edate As String = ""
            Dim busyo As String = ""
            Dim bikou As String = ""
            Dim ninusiID As Integer = 0
            Dim ninusimeiHND As String = ""
            Dim ninusimeiTF As String = ""
            Dim FAXbanngou As String = ""


            cn = New SqlClient.SqlConnection(cnstr)

            cmd = cn.CreateCommand
            cmd.CommandText = sqlstr

            cn.Open()
            'Console.WriteLine(cn.State)

            reader = cmd.ExecuteReader()

            If reader.HasRows = True Then '---レコードあり

                Do While reader.Read()
                    If reader("開始日") IsNot DBNull.Value Then
                        sdate = Format(reader("開始日"), "yyyymmdd")
                    End If
                    If reader("終了日") IsNot DBNull.Value Then
                        edate = Format(reader("終了日"), "yyyymmdd")
                    End If
                    busyo = reader("担当部署")
                    bikou = reader("備考")
                    ninusiID = reader("荷主ID")
                    ninusimeiHND = reader("名称_HND")
                    ninusimeiTF = reader("名称_TF")
                    FAXbanngou = reader("FAX番号")
                Loop

                reader.Close()
                cmd.Dispose()
                cn.Close()
                cn.Dispose()



                Dim yousosuu As Integer = fnarray.Length
                Dim TSiti As Integer
                Dim dt As Date
                For TSiti = 0 To yousosuu - 1
                    If DateTime.TryParseExact(fnarray(TSiti), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, dt) = True Then
                        Exit For
                    Else
                        'MsgBox(i & " " & fnarray(i))
                    End If
                Next

                If yousosuu >= 4 Then '配列の要素数チェック----要素数4以上
                    '###########################################################################################ファイル名組み立て開始
                    If zyutyuuCD < 1000 Then '--------------------------------------------------------------------------------受注以外の処理（受注CD三桁以下）

                        Dim str As String = fnarray(1)
                        Dim strarray As Array = Split(str, "-")
                        Dim datestr As String = strarray(0)

                        If Date.TryParseExact(datestr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, dt) = True Then '送信時に入力する文字列が日付の場合
                            sdate = datestr
                            edate = datestr
                            If strarray.Length > 1 Then
                                bikou = strarray(1)
                                If strarray.Length > 2 Then
                                    Dim i As Integer
                                    For i = 2 To strarray.Length - 1
                                        bikou &= "-" & strarray(i)
                                    Next
                                End If
                            End If
                        Else '-----------------------------------------------------------送信時に入力する文字列が日付以外の場合
                            bikou = str

                        End If





                        If fnarray(2) = "登録名称不明" Or fnarray(2) = "scan" Or fnarray(2) = "" Then '荷主名
                                If ninusimeiTF = "" Then
                                    fname = ninusimeiHND
                                Else
                                    fname = ninusimeiTF
                                End If
                            Else
                                fname = fnarray(2)
                            End If

                            If fnarray(3) = "" Then '---------------------------------------FAX番号
                                If FAXbanngou = "" Then
                                    fname &= "_" & ""
                                Else
                                    fname &= "_" & FAXbanngou
                                End If
                            Else
                                fname &= "_" & fnarray(3)
                            End If

                            fname &= "_" & fnarray(4) '-------------------------------------タイムスタンプ
                            fname &= "_" & fnarray(0) '-------------------------------------受注CD
                            fname &= "_" & sdate '------------------------------------------開始日
                            fname &= "_" & edate '------------------------------------------終了日
                            fname &= "_" & busyo '------------------------------------------担当部署
                            fname &= "_" & bikou '------------------------------------------備考
                            If fnarray.Length = 5 Then '------------------------------------ページ
                                fname &= "_1"
                            Else
                                fname &= "_" & fnarray(5)
                            End If
                            fname &= ".pdf" '-----------------------------------------------拡張子



                        Else '-----------------------------------------------------------------------------------------------------------------通常受注処理

                        If fnarray(1) = "登録名称不明" Or fnarray(1) = "scan" Or fnarray(1) = "取込" Or fnarray(1) = "" Then '荷主名
                            If ninusimeiTF = "" Then
                                fname = ninusimeiHND
                            Else
                                fname = ninusimeiTF
                            End If
                        Else
                            fname = fnarray(1)
                            If ninusimeiTF = "" Then
                                荷主名保存(ninusiID, fnarray(1))
                            End If
                        End If

                        If fnarray(2) = "" Then '---------------------------------------FAX番号
                            If FAXbanngou = "" Then
                                fname &= "_" & ""
                            Else
                                fname &= "_" & FAXbanngou
                            End If
                        Else
                            fname &= "_" & fnarray(2)
                            If FAXbanngou = "" Then
                                FAX番号保存(ninusiID, fnarray(2))
                            End If
                        End If

                        fname &= "_" & fnarray(3) '-------------------------------------タイムスタンプ
                        fname &= "_" & fnarray(0) '-------------------------------------受注CD
                        fname &= "_" & sdate '------------------------------------------開始日
                        fname &= "_" & edate '------------------------------------------終了日
                        fname &= "_" & busyo '------------------------------------------担当部署
                        fname &= "_" & bikou '------------------------------------------備考
                        If fnarray.Length = 4 Then '------------------------------------ページ
                            fname &= "_1"
                        Else
                            fname &= "_" & fnarray(4)
                        End If
                        fname &= ".pdf" '-----------------------------------------------拡張子

                    End If
                    '#############################################################################################ファイル組み立て終了

                    Try '-----ファイルのコピー処理
                        If FileOpenCheck(e.FullPath) = True Then
                            System.IO.File.Copy(e.FullPath, tennsousaki & "\" & fname, True)
                        Else
                            flg = False
                            errorstr = "転送タイムアウト"
                        End If
                    Catch ex As System.IO.FileNotFoundException 'コピーする元ファイルがない場合
                        flg = False
                        errorstr = ex.Message
                    Catch ex As System.IO.DirectoryNotFoundException 'コピー先のファルダが存在しない
                        flg = False
                        errorstr = ex.Message
                    Catch ex As System.IO.IOException 'コピー先のファイルがすでに存在している場合
                        flg = False
                        errorstr = ex.Message
                    Catch ex As System.UnauthorizedAccessException 'コピー先のファイルへのアクセスが拒否された場合
                        flg = False
                        errorstr = ex.Message
                    End Try

                    'MsgBox(fname)

                Else '----------------------------------------------要素数4未満
                    flg = False
                    errorstr = "ファイル名不正"
                End If

            Else '受注CD適合なし
                flg = False
                errorstr = "受注CD該当なし"
            End If

        Else '数値以外
            flg = False
            errorstr = "受注CD不正"
        End If


        If flg = False Then '---------------------------------------------------------------------------------------------------エラー時処理

            Dim ac As Integer = fnarray.Length
            Dim i As Integer

            If fnarray(0) = "501" Or fnarray(0) = "601" Or fnarray(0) = "701" Or fnarray(0) = "801" Or fnarray(0) = "901" Then

                fname = errorstr
                fname &= "_" & fnarray(2)
                If ac > 3 Then
                    For i = 4 To ac
                        If i < ac Then
                            fname &= "_" & fnarray(i)
                        End If
                    Next
                    fname &= ".pdf"
                End If

            Else

                fname = errorstr
                fname &= "_" & fnarray(1)
                If ac > 2 Then
                    For i = 2 To ac
                        MsgBox(i)
                        If i < ac Then
                            fname &= "_" & fnarray(i)
                        End If
                    Next
                    fname &= ".pdf"
                End If

            End If

            If FileOpenCheck(e.FullPath) = True Then
                System.IO.File.Copy(e.FullPath, tennsoumoto & "\" & fname, False) 'エラーコードを付けて転送元へ返信
            End If

        End If

        If FileOpenCheck(e.FullPath) = True Then
            System.IO.File.Delete(e.FullPath) '作業ファイル削除
        End If




    End Sub

    Private Sub 荷主名保存(ByVal ninusiID As Integer, ByVal ninusimeiTF As String)

        Dim sqlstr As String = "UPDATE T_TF_M_NinusiInfo SET 名称_TF = '" & ninusimeiTF & "' WHERE 荷主ID =" & ninusiID

        Dim cn As SqlClient.SqlConnection
        Dim cmd As SqlClient.SqlCommand

        cn = New SqlClient.SqlConnection(cnstr)
        cn.Open()

        cmd = cn.CreateCommand
        cmd.CommandText = sqlstr
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
        cn.Dispose()

    End Sub
    Private Sub FAX番号保存(ByVal ninusiID As Integer, ByVal FAXbanngou As String)

        Dim sqlstr As String = "UPDATE T_TF_M_NinusiInfo SET FAX番号 = '" & FAXbanngou & "' WHERE 荷主ID =" & ninusiID

        Dim cn As SqlClient.SqlConnection
        Dim cmd As SqlClient.SqlCommand

        cn = New SqlClient.SqlConnection(cnstr)
        cn.Open()

        cmd = cn.CreateCommand
        cmd.CommandText = sqlstr
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
        cn.Dispose()

    End Sub

    Public Sub scan転送開始()

        If Not (swatcher Is Nothing) Then
            Return
        End If

        swatcher = New System.IO.FileSystemWatcher

        tennsoumoto = txt転送元.Text
        scan = txtscan.Text

        '監視するディレクトリを指定
        swatcher.Path = scan

        '最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
        swatcher.NotifyFilter = System.IO.NotifyFilters.FileName

        'すべてのファイルを監視
        swatcher.Filter = ""

        'UIのスレッドにマーシャリングする
        'コンソールアプリケーションでの使用では必要ない
        swatcher.SynchronizingObject = Me

        'イベントハンドラの追加
        'AddHandler swatcher.Changed, AddressOf swatcher_Changed
        AddHandler swatcher.Created, AddressOf swatcher_Changed
        'AddHandler swatcher.Deleted, AddressOf swatcher_Changed
        AddHandler swatcher.Renamed, AddressOf swatcher_Renamed

        '監視を開始する
        swatcher.EnableRaisingEvents = True
        Console.WriteLine("scan転送を開始しました。")


    End Sub

    Public Sub scan転送停止()

        '監視を終了
        swatcher.EnableRaisingEvents = False
        swatcher.Dispose()
        swatcher = Nothing
        Console.WriteLine("scan転送を終了しました。")

    End Sub

    Private Sub swatcher_Changed(ByVal source As System.Object, ByVal e As System.IO.FileSystemEventArgs)

        Select Case e.ChangeType
            Case System.IO.WatcherChangeTypes.Changed
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が変更されました。"))
            Case System.IO.WatcherChangeTypes.Deleted
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が削除されました。"))
            Case System.IO.WatcherChangeTypes.Created
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が作成されました。"))

                Dim kakutyousi As String = Path.GetExtension(e.FullPath)

                If kakutyousi <> ".tmp" Then

                    Dim fname As String = Path.GetFileName(e.FullPath)

                    fname = "scan__" & fname

                    If FileOpenCheck(e.FullPath) = True Then
                        System.IO.File.Copy(e.FullPath, tennsoumoto & "\" & fname, False)
                    End If

                    If FileOpenCheck(e.FullPath) = True Then
                        System.IO.File.Delete(e.FullPath)
                    End If

                End If

        End Select

    End Sub
    Private Sub swatcher_Renamed(ByVal source As System.Object, ByVal e As System.IO.RenamedEventArgs)
        'Console.WriteLine(("ファイル 「" + e.FullPath + "」の名前が変更されました。"))


        Dim fname As String = Path.GetFileNameWithoutExtension(e.FullPath)
        Dim kakutyousi As String = Path.GetExtension(e.FullPath)
        Dim dt As Date


        'MsgBox(fname & kakutyousi)

        If DateTime.TryParseExact(fname, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, dt) = True Then
            fname = "取込__" & fname & kakutyousi
        Else
            fname = "取込__" & Format(Now(), "yyyymmddhhmmss") & kakutyousi
        End If


        If FileOpenCheck(e.FullPath) = True Then
            System.IO.File.Copy(e.FullPath, tennsoumoto & "\" & fname, False)
        End If
        If FileOpenCheck(e.FullPath) = True Then
            System.IO.File.Delete(e.FullPath)
        End If
    End Sub

    Private Function FileOpenCheck(fpath As String) As Boolean

        Dim st As Stream = Nothing
        Dim maxcount As Integer = 60
        Dim i As Integer
        Dim c As Integer

        For i = 0 To maxcount
            Try
                st = File.Open(fpath, FileMode.Open, FileAccess.Read, FileShare.None)
                If Not st Is Nothing Then
                    'Console.WriteLine(c)
                    Exit For
                End If
            Catch ex As Exception
                System.Threading.Thread.Sleep(1000)
                c += 1
            End Try
        Next i

        If Not st Is Nothing Then
            st.Close()
            FileOpenCheck = True
        Else
            ' タイムアウト
            FileOpenCheck = False
        End If

    End Function


    Private Sub btn参照1_Click(sender As Object, e As EventArgs) Handles btn参照1.Click
        If lblステータス.Text = "稼働中" Then
            MsgBox("稼働中はフォルダを変更できません")
        Else
            Dim fbd As New FolderBrowserDialog
            fbd.Description = "フォルダを指定してください。"
            fbd.RootFolder = Environment.SpecialFolder.Desktop
            fbd.SelectedPath = "C:\Windows"
            fbd.ShowNewFolderButton = True

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                txt転送元.Text = fbd.SelectedPath
            End If
        End If
    End Sub

    Private Sub btn参照2_Click(sender As Object, e As EventArgs) Handles btn参照2.Click
        If lblステータス.Text = "稼働中" Then
            MsgBox("稼働中はフォルダを変更できません")
        Else
            Dim fbd As New FolderBrowserDialog
            fbd.Description = "フォルダを指定してください。"
            fbd.RootFolder = Environment.SpecialFolder.Desktop
            fbd.SelectedPath = "C:\Windows"
            fbd.ShowNewFolderButton = True

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                txt監視.Text = fbd.SelectedPath
            End If
        End If
    End Sub

    Private Sub btn参照3_Click(sender As Object, e As EventArgs) Handles btn参照3.Click
        If lblステータス.Text = "稼働中" Then
            MsgBox("稼働中はフォルダを変更できません")
        Else
            Dim fbd As New FolderBrowserDialog
            fbd.Description = "フォルダを指定してください。"
            fbd.RootFolder = Environment.SpecialFolder.Desktop
            fbd.SelectedPath = "C:\Windows"
            fbd.ShowNewFolderButton = True

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                txt転送先.Text = fbd.SelectedPath
            End If
        End If
    End Sub

    Private Sub btn参照4_Click(sender As Object, e As EventArgs) Handles btn参照4.Click
        If lblステータス.Text = "稼働中" Then
            MsgBox("稼働中はフォルダを変更できません")
        Else
            Dim fbd As New FolderBrowserDialog
            fbd.Description = "フォルダを指定してください。"
            fbd.RootFolder = Environment.SpecialFolder.Desktop
            fbd.SelectedPath = "C:\Windows"
            fbd.ShowNewFolderButton = True

            If fbd.ShowDialog(Me) = DialogResult.OK Then
                txtscan.Text = fbd.SelectedPath
            End If
        End If
    End Sub


    Private Sub btnテスト_Click(sender As Object, e As EventArgs) Handles btnテスト.Click




    End Sub

End Class

Public Class SettingItemClass
    Public tennsoumoto As String
    Public kannsi As String
    Public tennsousaki As String
    Public scan As String
End Class

Class SettingClass

    Public Shared Sub 設定保存()

        Dim strPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
        strPath = System.IO.Path.GetDirectoryName(strPath)
        strPath = strPath & "\setting.xml"

        Dim obj As New SettingItemClass()
        obj.tennsoumoto = Form1.txt転送元.Text
        obj.kannsi = Form1.txt監視.Text
        obj.tennsousaki = Form1.txt転送先.Text
        obj.scan = Form1.txtscan.Text

        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(SettingItemClass))
        Dim sw As New System.IO.StreamWriter(strPath, False, New System.Text.UTF8Encoding(False))
        serializer.Serialize(sw, obj)

        sw.Close()

    End Sub

    Public Shared Sub 設定読込()

        Dim strPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
        strPath = System.IO.Path.GetDirectoryName(strPath)
        strPath = strPath & "\setting.xml"

        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(SettingItemClass))
        Dim sr As New System.IO.StreamReader(strPath, New System.Text.UTF8Encoding(False))
        Dim obj As SettingItemClass = DirectCast(serializer.Deserialize(sr), SettingItemClass)

        Form1.txt転送元.Text = obj.tennsoumoto
        Form1.txt監視.Text = obj.kannsi
        Form1.txt転送先.Text = obj.tennsousaki
        Form1.txtscan.Text = obj.scan

        sr.Close()

    End Sub


End Class

