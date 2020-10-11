<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.txt転送元 = New System.Windows.Forms.TextBox()
        Me.txt監視 = New System.Windows.Forms.TextBox()
        Me.txt転送先 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btn参照1 = New System.Windows.Forms.Button()
        Me.btn参照2 = New System.Windows.Forms.Button()
        Me.btn参照3 = New System.Windows.Forms.Button()
        Me.lblステータス = New System.Windows.Forms.Label()
        Me.btn1 = New System.Windows.Forms.Button()
        Me.btnテスト = New System.Windows.Forms.Button()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        Me.btn参照4 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtscan = New System.Windows.Forms.TextBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txt転送元
        '
        Me.txt転送元.Location = New System.Drawing.Point(108, 212)
        Me.txt転送元.Name = "txt転送元"
        Me.txt転送元.Size = New System.Drawing.Size(313, 19)
        Me.txt転送元.TabIndex = 0
        '
        'txt監視
        '
        Me.txt監視.Location = New System.Drawing.Point(108, 258)
        Me.txt監視.Name = "txt監視"
        Me.txt監視.Size = New System.Drawing.Size(313, 19)
        Me.txt監視.TabIndex = 1
        '
        'txt転送先
        '
        Me.txt転送先.Location = New System.Drawing.Point(108, 306)
        Me.txt転送先.Name = "txt転送先"
        Me.txt転送先.Size = New System.Drawing.Size(313, 19)
        Me.txt転送先.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 215)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "転送元フォルダ"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(38, 261)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 12)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "監視フォルダ"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 309)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 12)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "転送先フォルダ"
        '
        'btn参照1
        '
        Me.btn参照1.Location = New System.Drawing.Point(436, 210)
        Me.btn参照1.Name = "btn参照1"
        Me.btn参照1.Size = New System.Drawing.Size(75, 23)
        Me.btn参照1.TabIndex = 6
        Me.btn参照1.Text = "参照"
        Me.btn参照1.UseVisualStyleBackColor = True
        '
        'btn参照2
        '
        Me.btn参照2.Location = New System.Drawing.Point(436, 256)
        Me.btn参照2.Name = "btn参照2"
        Me.btn参照2.Size = New System.Drawing.Size(75, 23)
        Me.btn参照2.TabIndex = 7
        Me.btn参照2.Text = "参照"
        Me.btn参照2.UseVisualStyleBackColor = True
        '
        'btn参照3
        '
        Me.btn参照3.Location = New System.Drawing.Point(436, 304)
        Me.btn参照3.Name = "btn参照3"
        Me.btn参照3.Size = New System.Drawing.Size(75, 23)
        Me.btn参照3.TabIndex = 8
        Me.btn参照3.Text = "参照"
        Me.btn参照3.UseVisualStyleBackColor = True
        '
        'lblステータス
        '
        Me.lblステータス.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblステータス.AutoSize = True
        Me.lblステータス.Font = New System.Drawing.Font("MS UI Gothic", 72.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblステータス.Location = New System.Drawing.Point(91, 41)
        Me.lblステータス.Name = "lblステータス"
        Me.lblステータス.Size = New System.Drawing.Size(330, 97)
        Me.lblステータス.TabIndex = 10
        Me.lblステータス.Text = "停止中"
        '
        'btn1
        '
        Me.btn1.Location = New System.Drawing.Point(436, 115)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(75, 23)
        Me.btn1.TabIndex = 11
        Me.btn1.Text = "開始"
        Me.btn1.UseVisualStyleBackColor = True
        '
        'btnテスト
        '
        Me.btnテスト.Location = New System.Drawing.Point(436, 431)
        Me.btnテスト.Name = "btnテスト"
        Me.btnテスト.Size = New System.Drawing.Size(75, 23)
        Me.btnテスト.TabIndex = 12
        Me.btnテスト.Text = "テスト"
        Me.btnテスト.UseVisualStyleBackColor = True
        '
        'EventLog1
        '
        Me.EventLog1.SynchronizingObject = Me
        '
        'btn参照4
        '
        Me.btn参照4.Location = New System.Drawing.Point(436, 402)
        Me.btn参照4.Name = "btn参照4"
        Me.btn参照4.Size = New System.Drawing.Size(75, 23)
        Me.btn参照4.TabIndex = 15
        Me.btn参照4.Text = "参照"
        Me.btn参照4.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(38, 407)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 12)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "scanフォルダ"
        '
        'txtscan
        '
        Me.txtscan.Location = New System.Drawing.Point(108, 404)
        Me.txtscan.Name = "txtscan"
        Me.txtscan.Size = New System.Drawing.Size(313, 19)
        Me.txtscan.TabIndex = 13
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(6, 27)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(83, 16)
        Me.RadioButton1.TabIndex = 16
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "イベント監視"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(6, 49)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(83, 16)
        Me.RadioButton2.TabIndex = 17
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "タイマー監視"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Location = New System.Drawing.Point(436, 33)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(106, 76)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "処理選択"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(546, 450)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btn参照4)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtscan)
        Me.Controls.Add(Me.btnテスト)
        Me.Controls.Add(Me.btn1)
        Me.Controls.Add(Me.lblステータス)
        Me.Controls.Add(Me.btn参照3)
        Me.Controls.Add(Me.btn参照2)
        Me.Controls.Add(Me.btn参照1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt転送先)
        Me.Controls.Add(Me.txt監視)
        Me.Controls.Add(Me.txt転送元)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "FaxTransfer"
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txt転送元 As TextBox
    Friend WithEvents txt監視 As TextBox
    Friend WithEvents txt転送先 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btn参照1 As Button
    Friend WithEvents btn参照2 As Button
    Friend WithEvents btn参照3 As Button
    Friend WithEvents lblステータス As Label
    Friend WithEvents btn1 As Button
    Friend WithEvents btnテスト As Button
    Friend WithEvents EventLog1 As EventLog
    Friend WithEvents btn参照4 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtscan As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents Timer1 As Timer
End Class
