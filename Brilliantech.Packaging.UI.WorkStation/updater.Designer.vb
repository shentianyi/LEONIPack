<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class updater
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
        Me.components = New System.ComponentModel.Container()
        Me.AppUpdater_up = New Microsoft.Samples.AppUpdater.AppUpdater(Me.components)
        CType(Me.AppUpdater_up, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AppUpdater_up
        '
        Me.AppUpdater_up.ChangeDetectionMode = Microsoft.Samples.AppUpdater.ChangeDetectionModes.ServerManifestCheck
        Me.AppUpdater_up.Downloader.DownloadRetryAttempts = 1
        Me.AppUpdater_up.Downloader.UpdateRetryAttempts = 1
        Me.AppUpdater_up.Poller.AutoStart = False
        '
        'updater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(332, 88)
        Me.Name = "updater"
        Me.Text = "updater"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        CType(Me.AppUpdater_up, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AppUpdater_up As Microsoft.Samples.AppUpdater.AppUpdater
End Class
