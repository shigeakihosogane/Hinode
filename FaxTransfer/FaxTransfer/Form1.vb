Imports System
Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms
Imports Microsoft.Win32


Public Class Form1

    Private watcher As System.IO.FileSystemWatcher = Nothing

    Dim tennsoumoto As String
    Dim kannsi As String
    Dim tennsousaki As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SettingClass.設定読込()

        'txt転送元.Text = "\\192.168.2.240\fax受信"
        'txt監視.Text = "\\192.168.2.240\fax転送"
        'txt転送先.Text = "\\192.168.2.240\ThereforeDATA"

    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        SettingClass.設定保存()

    End Sub

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click

        If lblステータス.Text = "停止中" Then
            If txt転送元.Text <> "" And txt監視.Text <> "" And txt転送先.Text <> "" Then
                lblステータス.Text = "稼働中"
                btn1.Text = "停止"
                転送開始()
                '監視開始()
            Else
                MsgBox("フォルダを選択してください。")
            End If
        Else
            lblステータス.Text = "停止中"
            btn1.Text = "開始"
            転送停止()
            '監視停止()
        End If

    End Sub

    Public Sub 転送開始()

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
        Console.WriteLine("監視を開始しました。")


    End Sub

    Public Sub 転送停止()

        '監視を終了
        watcher.EnableRaisingEvents = False
        watcher.Dispose()
        watcher = Nothing
        Console.WriteLine("監視を終了しました。")

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

        Console.WriteLine(("ファイル 「" + e.FullPath + "」の名前が変更されました。"))

        Dim fname As String = Path.GetFileNameWithoutExtension(e.FullPath)
        fname = Replace(fname, Space(1), String.Empty)
        Dim fnarray As Array = Split(fname, "_")

        Dim flg As Boolean = True
        Dim errorstr As String = ""

        If IsNumeric(fnarray(0)) Then '-数値

            Dim zyutyuuCD As Decimal = CDec(fnarray(0))

            Dim cnstr As String = "Data Source=SV201710;Initial Catalog=HINODEDB;USER ID=sa;PASSWORD=cube%6614;"
            Dim sqlstr As String = "SELECT 開始日, 終了日, 担当部署, 備考 FROM T_TF_D_index WHERE 受注ID = " & zyutyuuCD


            Dim cn As SqlClient.SqlConnection
            Dim cmd As SqlClient.SqlCommand
            Dim reader As SqlClient.SqlDataReader

            Dim sdate As Date
            Dim edate As Date
            Dim busyo As String = ""
            Dim bikou As String = ""

            cn = New SqlClient.SqlConnection(cnstr)

            cmd = cn.CreateCommand
            cmd.CommandText = sqlstr

            cn.Open()
            'Console.WriteLine(cn.State)

            reader = cmd.ExecuteReader()

            If reader.HasRows = True Then '---レコードあり

                Do While reader.Read()
                    sdate = reader("開始日")
                    edate = reader("終了日")
                    busyo = reader("担当部署")
                    bikou = reader("備考")
                Loop

                fname = fnarray(1)
                fname = fname & "_" & fnarray(2)
                fname = fname & "_" & fnarray(3)
                fname = fname & "_" & fnarray(0)
                fname = fname & "_" & Format(sdate, "yyyymmdd")
                fname = fname & "_" & Format(edate, "yyyymmdd")
                fname = fname & "_" & busyo
                fname = fname & "_" & bikou
                If fnarray.Length = 4 Then
                    fname = fname & "_1"
                Else
                    fname = fname & "_" & fnarray(4)
                End If
                fname = fname & ".pdf"


                reader.Close()
                cmd.Dispose()
                cn.Close()
                cn.Dispose()

                Try
                    System.IO.File.Copy(e.FullPath, tennsousaki & "\" & fname & ".pdf", True)
                    'Console.WriteLine(fname)
                Catch ex As System.IO.FileNotFoundException 'コピーするにファイルがない場合
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

            Else '--------------------------レコードなし

                flg = False
                errorstr = "受注CD該当なし"

            End If



        Else '-数値以外

            flg = False
            errorstr = "受注CD不正"

        End If


        If flg = False Then

            fname = errorstr
            fname = fname & "_" & fnarray(1)
            fname = fname & "_" & fnarray(2)
            fname = fname & "_" & fnarray(3)
            If fnarray.Length = 4 Then
                fname = fname
            Else
                fname = fname & " _" & fnarray(4)
            End If
            fname = fname & ".pdf"

            System.IO.File.Copy(e.FullPath, tennsoumoto & "\" & fname & ".pdf", False)

        End If


        'System.IO.File.Delete(e.FullPath)


    End Sub


    Private Sub btn参照1_Click(sender As Object, e As EventArgs) Handles btn参照1.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "フォルダを指定してください。"
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = "C:\Windows"
        fbd.ShowNewFolderButton = True

        If fbd.ShowDialog(Me) = DialogResult.OK Then
            txt転送元.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btn参照2_Click(sender As Object, e As EventArgs) Handles btn参照2.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "フォルダを指定してください。"
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = "C:\Windows"
        fbd.ShowNewFolderButton = True

        If fbd.ShowDialog(Me) = DialogResult.OK Then
            txt監視.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub btn参照3_Click(sender As Object, e As EventArgs) Handles btn参照3.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "フォルダを指定してください。"
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = "C:\Windows"
        fbd.ShowNewFolderButton = True

        If fbd.ShowDialog(Me) = DialogResult.OK Then
            txt転送先.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub 監視開始()


        Dim watcher As New System.IO.FileSystemWatcher
        '監視するディレクトリを指定
        watcher.Path = txt監視.Text
        '*.txtファイルを監視、すべて監視するときは""にする
        watcher.Filter = ""
        'ファイル名とディレクトリ名と最終書き込む日時の変更を監視
        watcher.NotifyFilter = System.IO.NotifyFilters.FileName
        'サブディレクトリは監視しない
        watcher.IncludeSubdirectories = False
        '必要に応じてバッファサイズを変更
        'watcher.InternalBufferSize = 4096
        '同期的に監視を開始する
        Dim changedResult As System.IO.WaitForChangedResult = watcher.WaitForChanged(System.IO.WatcherChangeTypes.Renamed)

        MsgBox(changedResult.Name)

        Console.WriteLine(changedResult.Name)



        '===========================================================================================================================================


        If changedResult.TimedOut Then
            Console.WriteLine("タイムアウトしました。")
            Return
        End If

        '変更があったときに結果を表示する
        Select Case changedResult.ChangeType
            Case System.IO.WatcherChangeTypes.Renamed
                Console.WriteLine(("ファイル 「" +
                    changedResult.OldName + "」の名前が「" +
                    changedResult.Name + "」に変更されました。"))
            Case System.IO.WatcherChangeTypes.Changed
                Console.WriteLine(("ファイル 「" +
                    changedResult.Name + "」が変更されました。"))
            Case System.IO.WatcherChangeTypes.Created
                Console.WriteLine(("ファイル 「" +
                    changedResult.Name + "」が作成されました。"))
            Case System.IO.WatcherChangeTypes.Deleted
                Console.WriteLine(("ファイル 「" +
                    changedResult.Name + "」が削除されました。"))

        End Select

    End Sub

    Private Sub 監視停止()

        'watcher.EnableRaisingEvents = False
        'watcher.Dispose()
        'watcher = Nothing

    End Sub



    Private Sub btnテスト_Click(sender As Object, e As EventArgs) Handles btnテスト.Click




    End Sub


End Class

Public Class SettingItemClass
    Public tennsoumoto As String
    Public kannsi As String
    Public tennsousaki As String
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

        sr.Close()

    End Sub


End Class

