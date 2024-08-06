namespace FileTransfer2
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbl状況 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btnRun = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txt転送1稼働時間 = new System.Windows.Forms.TextBox();
            this.chk転送1 = new System.Windows.Forms.CheckBox();
            this.chk転送2 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt転送2稼働時間 = new System.Windows.Forms.TextBox();
            this.lbl表示 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl状況
            // 
            this.lbl状況.Font = new System.Drawing.Font("Meiryo UI", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl状況.Location = new System.Drawing.Point(42, 18);
            this.lbl状況.Name = "lbl状況";
            this.lbl状況.Size = new System.Drawing.Size(400, 50);
            this.lbl状況.TabIndex = 4;
            this.lbl状況.Text = "停止中";
            this.lbl状況.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(480, 18);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(181, 50);
            this.btnRun.TabIndex = 6;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "稼働時間";
            // 
            // txt転送1稼働時間
            // 
            this.txt転送1稼働時間.Location = new System.Drawing.Point(205, 129);
            this.txt転送1稼働時間.Name = "txt転送1稼働時間";
            this.txt転送1稼働時間.Size = new System.Drawing.Size(164, 19);
            this.txt転送1稼働時間.TabIndex = 13;
            // 
            // chk転送1
            // 
            this.chk転送1.AutoSize = true;
            this.chk転送1.Location = new System.Drawing.Point(76, 129);
            this.chk転送1.Name = "chk転送1";
            this.chk転送1.Size = new System.Drawing.Size(54, 16);
            this.chk転送1.TabIndex = 15;
            this.chk転送1.Text = "転送1";
            this.chk転送1.UseVisualStyleBackColor = true;
            this.chk転送1.CheckedChanged += new System.EventHandler(this.chk転送1_CheckedChanged);
            // 
            // chk転送2
            // 
            this.chk転送2.AutoSize = true;
            this.chk転送2.Location = new System.Drawing.Point(76, 186);
            this.chk転送2.Name = "chk転送2";
            this.chk転送2.Size = new System.Drawing.Size(54, 16);
            this.chk転送2.TabIndex = 26;
            this.chk転送2.Text = "転送2";
            this.chk転送2.UseVisualStyleBackColor = true;
            this.chk転送2.CheckedChanged += new System.EventHandler(this.chk転送2_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "稼働時間";
            // 
            // txt転送2稼働時間
            // 
            this.txt転送2稼働時間.Location = new System.Drawing.Point(205, 183);
            this.txt転送2稼働時間.Name = "txt転送2稼働時間";
            this.txt転送2稼働時間.Size = new System.Drawing.Size(164, 19);
            this.txt転送2稼働時間.TabIndex = 24;
            // 
            // lbl表示
            // 
            this.lbl表示.Location = new System.Drawing.Point(57, 442);
            this.lbl表示.Name = "lbl表示";
            this.lbl表示.Size = new System.Drawing.Size(830, 81);
            this.lbl表示.TabIndex = 27;
            this.lbl表示.Text = "表示";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 689);
            this.Controls.Add(this.lbl表示);
            this.Controls.Add(this.chk転送2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt転送2稼働時間);
            this.Controls.Add(this.chk転送1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt転送1稼働時間);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.lbl状況);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbl状況;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt転送1稼働時間;
        private System.Windows.Forms.CheckBox chk転送1;
        private System.Windows.Forms.CheckBox chk転送2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt転送2稼働時間;
        private System.Windows.Forms.Label lbl表示;
    }
}

