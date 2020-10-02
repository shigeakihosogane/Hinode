Imports System
Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms
Imports Microsoft.Win32


Public Class Form1

    Private watcher1 As System.IO.FileSystemWatcher = Nothing

    Dim tennsoumoto As String
    Dim kannsi As String
    Dim tennsousaki As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainClass.設定読込()

        'txt転送元.Text = "\\192.168.2.240\fax受信"
        'txt監視.Text = "\\192.168.2.240\fax転送"
        'txt転送先.Text = "\\192.168.2.240\ThereforeDATA"

    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        MainClass.設定保存()

    End Sub

    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn1.Click

        If lblステータス.Text = "停止中" Then
            lblステータス.Text = "稼働中"
            btn1.Text = "停止"
            転送開始()
        Else
            lblステータス.Text = "停止中"
            btn1.Text = "開始"
            転送停止()
        End If

    End Sub

    Public Sub 転送開始()

        If Not (watcher1 Is Nothing) Then
            Return
        End If

        watcher1 = New System.IO.FileSystemWatcher

        tennsoumoto = txt転送元.Text
        kannsi = txt監視.Text
        tennsousaki = txt転送先.Text

        '監視するディレクトリを指定
        watcher1.Path = kannsi

        '最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
        watcher1.NotifyFilter = System.IO.NotifyFilters.LastAccess Or
            System.IO.NotifyFilters.LastWrite Or
            System.IO.NotifyFilters.FileName Or
            System.IO.NotifyFilters.DirectoryName
        'すべてのファイルを監視
        watcher1.Filter = "*.pdf"
        'UIのスレッドにマーシャリングする
        'コンソールアプリケーションでの使用では必要ない
        watcher1.SynchronizingObject = Me

        'イベントハンドラの追加
        'AddHandler watcher1.Changed, AddressOf watcher_Changed1
        AddHandler watcher1.Created, AddressOf watcher_Changed1
        'AddHandler watcher1.Deleted, AddressOf watcher_Changed1


        '監視を開始する
        watcher1.EnableRaisingEvents = True
        Console.WriteLine("転送1監視を開始しました。")


    End Sub

    Public Sub 転送停止()

        '監視を終了
        watcher1.EnableRaisingEvents = False
        watcher1.Dispose()
        watcher1 = Nothing
        Console.WriteLine("転送1監視を終了しました。")

    End Sub

    Private Sub watcher_Changed1(ByVal source As System.Object, ByVal e As System.IO.FileSystemEventArgs)

        Select Case e.ChangeType
            Case System.IO.WatcherChangeTypes.Changed
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が変更されました。"))
            Case System.IO.WatcherChangeTypes.Deleted
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が削除されました。"))
            Case System.IO.WatcherChangeTypes.Created
                Console.WriteLine(("ファイル 「" + e.FullPath + "」が作成されました。"))

                Dim fname As String = Path.GetFileNameWithoutExtension(e.FullPath)
                Dim fnarray As Array = Split(fname, "_")

                Dim flg As Boolean = True
                Dim errorstr As String = ""

                If IsNumeric(fnarray(0)) Then '-数値

                    Try

                        Dim zyutyuuCD As Decimal = CDec(fnarray(0))

                        Dim cnstr As String = "Data Source=SV201710;Initial Catalog=HINODEDB;USER ID=sa;PASSWORD=cube%6614;"
                        Dim sqlstr As String = "SELECT 開始日, 終了日, 担当部署, 備考 FROM T_TF_D_index WHERE 受注ID = " & zyutyuuCD


                        Dim cn As SqlClient.SqlConnection
                        Dim cmd As SqlClient.SqlCommand
                        Dim reader As SqlClient.SqlDataReader

                        Dim sdate As Date
                        Dim edate As Date
                        Dim busyo As String
                        Dim bikou As String

                        cn = New SqlClient.SqlConnection(cnstr)

                        cmd = cn.CreateCommand
                        cmd.CommandText = sqlstr

                        cn.Open()
                        Console.WriteLine(cn.State)

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
                            fname = fname & "_" & ".pdf"


                            reader.Close()
                            cmd.Dispose()
                            cn.Close()
                            cn.Dispose()


                            System.IO.File.Copy(e.FullPath, tennsousaki & "\" & fname, True)
                            System.IO.File.Delete(e.FullPath)

                            'Console.WriteLine(fname)




                        Else '--------------------------レコードなし

                            flg = False
                            errorstr = "受注CD該当なし"

                        End If


                    Catch ex As System.IO.FileNotFoundException '例外が発生

                        flg = False
                        errorstr = ex.Message

                    End Try

                Else '-数値以外

                    flg = False
                    errorstr = "受注CD不正"

                End If

                If flg = False Then

                    fname = errorstr
                    fname = fname & "_" & fnarray(1)
                    fname = fname & "_" & fnarray(2)
                    fname = fname & "_" & fnarray(3)

                    System.IO.File.Copy(e.FullPath, tennsoumoto & "\" & fname & ".pdf", True)
                    System.IO.File.Delete(e.FullPath)

                End If



        End Select

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


    Private Sub btnテスト_Click(sender As Object, e As EventArgs) Handles btnテスト.Click







    End Sub


End Class

'XMLファイルに保存するオブジェクトのためのクラス
Public Class SampleClass
    Public tennsoumoto As String
    Public kannsi As String
    Public tennsousaki As String
End Class

Class MainClass
    'SampleClassオブジェクトをXMLファイルに保存する
    Public Shared Sub 設定保存()
        '保存先のファイル名
        Dim strPath As String = ""
        strPath = System.Reflection.Assembly.GetExecutingAssembly().Location
        strPath = System.IO.Path.GetDirectoryName(strPath)
        strPath = strPath & "\config.xml"

        '保存するクラス(SampleClass)のインスタンスを作成
        Dim obj As New SampleClass()
        obj.tennsoumoto = Form1.txt転送元.Text
        obj.kannsi = Form1.txt監視.Text
        obj.tennsousaki = Form1.txt転送先.Text

        'XmlSerializerオブジェクトを作成
        'オブジェクトの型を指定する
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(SampleClass))
        '書き込むファイルを開く（UTF-8 BOM無し）
        Dim sw As New System.IO.StreamWriter(strPath, False, New System.Text.UTF8Encoding(False))
        'シリアル化し、XMLファイルに保存する
        serializer.Serialize(sw, obj)
        'ファイルを閉じる
        sw.Close()
    End Sub

    'XMLファイルをSampleClassオブジェクトに復元する
    Public Shared Sub 設定読込()
        '保存元のファイル名
        Dim strPath As String = ""
        strPath = System.Reflection.Assembly.GetExecutingAssembly().Location
        strPath = System.IO.Path.GetDirectoryName(strPath)
        strPath = strPath & "\config.xml"

        'XmlSerializerオブジェクトを作成
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(SampleClass))
        '読み込むファイルを開く
        Dim sr As New System.IO.StreamReader(strPath, New System.Text.UTF8Encoding(False))
        'XMLファイルから読み込み、逆シリアル化する
        Dim obj As SampleClass = DirectCast(serializer.Deserialize(sr), SampleClass)

        Form1.txt転送元.Text = obj.tennsoumoto
        Form1.txt監視.Text = obj.kannsi
        Form1.txt転送先.Text = obj.tennsousaki

        'ファイルを閉じる
        sr.Close()

    End Sub


End Class

