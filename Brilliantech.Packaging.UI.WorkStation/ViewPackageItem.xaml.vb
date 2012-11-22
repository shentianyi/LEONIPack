Imports Brilliantech.Packaging.Data
Imports Brilliantech.Packaging.UI.WorkStation.PackService
Public Class ViewPackageItem
    Private packageId As String
    Private currentPackage As FullPackageInfo
    Private currentItems As List(Of PackageItem)

    Public Sub New(ByVal pid As String)
        Me.New()
        packageId = pid
    End Sub
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
       
        BindPackage(packageId)

    End Sub

    Private Sub BindPackage(ByVal pid As String)
        If Not String.IsNullOrEmpty(pid) Then
            Dim client As PackProcessClient = New PackProcessClient
            Dim infos() As FullPackageInfo = client.GetPackageInfos(pid, String.Empty, String.Empty, _
                                                                     String.Empty, -1, New Date(1990, 1, 1), New Date(2200, 1, 1))
            Try
                client.Close()

            Catch ex As Exception
                client.Abort()
            End Try
            If infos IsNot Nothing Then
                Try
                    currentPackage = infos(0)
                Catch ex As Exception

                End Try
                If currentPackage Is Nothing Then
                    Dim infoboard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "找不到包装信息")
                    infoboard.ShowDialog()
                Else
                    BindMeta()
                    BindDataGrid()
                End If
            End If


        Else
            Dim infoboard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "请输入包装箱号")
            infoboard.ShowDialog()
        End If
    End Sub

    Private Sub BindMeta()
        Me.tb_packageId.Text = packageId
        Me.Label_partNr.Content = currentPackage.PartNr


    End Sub

    Private Sub BindDataGrid()
        Dim client As PackProcessClient = New PackProcessClient
        currentItems = client.GetItemsByPackageId(packageId).ToList
        Try
            client.Close()
        Catch ex As Exception
            client.Abort()
        End Try
        Me.DataGrid_orderDetails.ItemsSource = currentItems

    End Sub
End Class
