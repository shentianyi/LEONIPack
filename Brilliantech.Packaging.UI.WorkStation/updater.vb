Imports Nini

Public Class updater

    Private UpdateUrl
    Private SecondsBeteweenDownloadRety As Integer
    Private UpdateRetryAttempts As Integer
    Private DownloadRetryAttempts As Integer
    Private PollerInterval As Integer
    Private PollerInitInterval As Integer
    Private showDefaultUI As Boolean
    Private Shared configs As Nini.Config.IniConfigSource = _
        New Nini.Config.IniConfigSource( _
        IO.Path.Combine(My.Application.Info.DirectoryPath, _
                        "configures\AutoUpdate.ini"))


    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        UpdateUrl = configs.Configs("Update").GetString("UpdateUrl")
        SecondsBeteweenDownloadRety = configs.Configs("Update").GetInt("SecondsBeteweenDownloadRety")
        UpdateRetryAttempts = configs.Configs("Update").GetInt("UpdateRetryAttempts")
        DownloadRetryAttempts = configs.Configs("Update").GetInt("DownloadRetryAttempts")
        PollerInterval = configs.Configs("Update").GetInt("PollerInterval")
        PollerInitInterval = configs.Configs("Update").GetInt("PollerInitInterval")
        showDefaultUI = configs.Configs("Update").GetBoolean("showDefaultUI")

        AppUpdater_up.UpdateUrl = UpdateUrl
        AppUpdater_up.Downloader.SecondsBetweenDownloadRetry = SecondsBeteweenDownloadRety
        AppUpdater_up.Downloader.UpdateRetryAttempts = UpdateRetryAttempts
        AppUpdater_up.Poller.PollInterval = PollerInterval
        AppUpdater_up.Poller.InitialPollInterval = PollerInitInterval
        AppUpdater_up.ShowDefaultUI = showDefaultUI
        AppUpdater_up.Poller.Start()
    End Sub
    Private Sub updater_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class