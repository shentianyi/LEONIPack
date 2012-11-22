<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button_auto = New System.Windows.Forms.Button()
        Me.Button_start = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DateTimePicker_end = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker_start = New System.Windows.Forms.DateTimePicker()
        Me.SuspendLayout()
        '
        'Button_auto
        '
        Me.Button_auto.Location = New System.Drawing.Point(14, 12)
        Me.Button_auto.Name = "Button_auto"
        Me.Button_auto.Size = New System.Drawing.Size(366, 23)
        Me.Button_auto.TabIndex = 0
        Me.Button_auto.Text = "自动转移"
        Me.Button_auto.UseVisualStyleBackColor = True
        '
        'Button_start
        '
        Me.Button_start.Location = New System.Drawing.Point(305, 97)
        Me.Button_start.Name = "Button_start"
        Me.Button_start.Size = New System.Drawing.Size(75, 23)
        Me.Button_start.TabIndex = 5
        Me.Button_start.Text = "开始转移"
        Me.Button_start.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "时间从"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 12)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "时间到"
        '
        'DateTimePicker_end
        '
        Me.DateTimePicker_end.Location = New System.Drawing.Point(59, 99)
        Me.DateTimePicker_end.Name = "DateTimePicker_end"
        Me.DateTimePicker_end.Size = New System.Drawing.Size(200, 21)
        Me.DateTimePicker_end.TabIndex = 2
        '
        'DateTimePicker_start
        '
        Me.DateTimePicker_start.Location = New System.Drawing.Point(59, 54)
        Me.DateTimePicker_start.Name = "DateTimePicker_start"
        Me.DateTimePicker_start.Size = New System.Drawing.Size(200, 21)
        Me.DateTimePicker_start.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(400, 158)
        Me.Controls.Add(Me.Button_start)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DateTimePicker_end)
        Me.Controls.Add(Me.DateTimePicker_start)
        Me.Controls.Add(Me.Button_auto)
        Me.Name = "Form1"
        Me.Text = "包装系统数据备份器"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_auto As System.Windows.Forms.Button
    Friend WithEvents Button_start As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker_end As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker_start As System.Windows.Forms.DateTimePicker

End Class
