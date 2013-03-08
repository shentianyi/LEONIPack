Imports Brilliantech.Packaging.Data
Imports System.Threading.ThreadPool
Imports System.Threading.Thread
Imports System.Threading
Imports Nini.Config
Imports Brilliantech.Packaging.WS
Imports System.Random

Module Module1
    Private config As IConfigSource = New IniConfigSource("configures\ReplConf.ini")
    Sub main()
        Console.WriteLine("开始运行")
        Dim counter As Integer = 45
        Dim name As List(Of String) = New List(Of String)
        For i = 0 To counter
            name.Add(i.ToString)
        Next

        For Each Str As String In name
            QueueUserWorkItem(New WaitCallback(AddressOf Insert), Str)
        Next

        For Each Str As String In name
            QueueUserWorkItem(New WaitCallback(AddressOf Repl), Str)

        Next
        Console.WriteLine("添加完成")
        Console.ReadLine()
    End Sub

    Private Sub Repl(ByVal obj As Object)
        Thread.Sleep(100000)
        Dim rdm As System.Random = New System.Random


        Do While True
            Try
                ReplicationUtils.ReplicateMasterData(CType(obj, String))
                Console.WriteLine("db:" + CType(obj, String) + " 同步成功")
            Catch ex As Exception
                Console.WriteLine("db:" + CType(obj, String) + " 同步失败 " & ex.Message)
            End Try
            Dim min As Integer = rdm.Next(15, 30)
            Thread.Sleep(min * 60 * 1000)
        Loop


    End Sub
    Private Sub Insert(ByVal obj As Object)
        Dim name As String = CType(obj, String)
        Dim connStr As String = config.Configs("Common").GetString("SubscriberConnectionString") & name & ".sdf"
        ReplicationUtils.CreateNew(name)
        Do While True
            Try

           
            Dim capa As Integer = 300
            Dim packId As String = Now.ToString("yyyyMMddhhmmssffff")
            Dim toInsert As SinglePackage = New SinglePackage
            toInsert.Capa = capa
            toInsert.ContainerID = "MBPL"
            toInsert.PartNr = "91G104803"
            toInsert.PlanedDate = Now.ToString("yyyy-MM-dd")

            toInsert.Status = PackageStatus.NewCreated


            toInsert.WrkstnID = "N2-COC/N3COC"
            toInsert.Rowguid = Guid.NewGuid
            toInsert.PackageID = packId

            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            packageRepo.Insert(toInsert)
            unitOfWork.Commit()

            For i = 1 To capa

                Dim beginTime As DateTime = Now
                Dim unitOfWork1 As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo1 As SinglePackageRepo = New SinglePackageRepo(unitOfWork1)
                Dim package As SinglePackage = packageRepo1.GetByID(packId)

                    package.PackageItems.Add(New PackageItem With _
                          {.itemUid = Guid.NewGuid, _
                              .itemSeq = package.PackageItems.Count + 1, _
                           .packageID = packId, _
                           .packagingTime = Now(), .TNr = Now.ToString("yyyyMMddhhmmssffff")})
                unitOfWork1.Commit()
                Dim endTime As DateTime = Now
                Dim consume As TimeSpan = endTime - beginTime

                Console.WriteLine("db:" + name + ":添加一个Item,耗时：" & consume.TotalSeconds)


                Thread.Sleep(10000)

            Next
            Console.WriteLine("db:" + name + "添加一个Pakcage")
                Thread.Sleep(1000)
            Catch ex As Exception
                Console.WriteLine("插入时出错 " & ex.ToString)
            End Try
        Loop
        
    End Sub
End Module
