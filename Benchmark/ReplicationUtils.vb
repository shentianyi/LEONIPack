﻿Imports Nini.Config
Imports System.Data.SqlServerCe

Public Class ReplicationUtils
    Private Shared config As IConfigSource = New IniConfigSource("configures\ReplConf.ini")

    Public Shared Sub ReplicateMasterData(ByVal name As String)
        Dim repl As SqlCeReplication = Nothing
        repl = New SqlCeReplication()
        repl.Subscriber = name
        CommonData(repl, name)
        Try
            repl.InternetUrl = config.Configs("MasterData").GetString("InternetUrl")
            repl.Publication = config.Configs("MasterData").GetString("Publication")
  
                repl.Publication = config.Configs("MasterData").GetString("Publication")
                Try

                    repl.AddSubscription(AddOption.ExistingDatabase)
                Catch ex As Exception

                End Try

                repl.Synchronize()


        Catch ex As Exception
            repl.Dispose()
        End Try

    End Sub

    Public Shared Sub CreateNew(ByVal name As String)
        Dim repl As SqlCeReplication = Nothing
        repl = New SqlCeReplication()
        repl.Subscriber = name
        CommonData(repl, name)
        Try
            repl.InternetUrl = config.Configs("New").GetString("InternetUrl")
            repl.Publication = config.Configs("New").GetString("Publication")
            repl.AddSubscription(AddOption.CreateDatabase)
            repl.Synchronize()
        Catch ex As Exception
            repl.Dispose()
        End Try
    End Sub

    Public Shared Sub ReplicateRealTime(ByVal name As String)
        Dim repl As SqlCeReplication = Nothing
        repl = New SqlCeReplication()

        CommonData(repl, name)
        Try
            repl.InternetUrl = config.Configs("RealTime").GetString("InternetUrl")
            repl.Publication = config.Configs("RealTime").GetString("Publication")
            ' Create the local SQL Server Database subscription
            If Not IO.File.Exists(config.Configs("Common").GetString("DBName")) Then
                CreateNew(name)
            Else
                repl.Synchronize()

            End If

        Catch ex As Exception
            repl.Dispose()
        End Try
    End Sub

    Public Shared Sub CommonData(ByRef repl As SqlCeReplication, ByVal name As String)
        repl.Publisher = config.Configs("Common").GetString("Publisher")
        repl.PublisherDatabase = config.Configs("Common").GetString("PublisherDatabase")
        repl.PublisherLogin = config.Configs("Common").GetString("PublisherLogin")
        repl.PublisherPassword = config.Configs("Common").GetString("PublisherPassword")
        'If String.IsNullOrEmpty(My.Settings.subscriberName) Then
        '    My.Settings.subscriberName = Guid.NewGuid.ToString
        '    My.Settings.Save()
        'End If
        'repl.Subscriber = My.Settings.subscriberName
        repl.SubscriberConnectionString = config.Configs("Common").GetString("SubscriberConnectionString") & name & ".sdf"
    End Sub
End Class
