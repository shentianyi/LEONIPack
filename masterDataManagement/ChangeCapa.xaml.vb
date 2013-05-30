Imports Brilliantech.Packaging.Data
Imports masterDataManagement.packSvc
Public Class ChangeCapa
    Private currentPackage As SinglePackage
    Public Sub New(ByVal packId As String)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        Dim client As packSvc.PackProcessClient = New PackProcessClient
        Dim result As PackageMessage = client.FindByID(packId)
        Try
            client.Close()
        Catch ex As Exception
            client.Abort()
        End Try
        If result IsNot Nothing Then
            If result.Package IsNot Nothing Then
                currentPackage = result.Package
            Else
                MsgBox("没有找到ID号为" & packId & "的启动标签")
                Me.Close()
            End If
        Else
            MsgBox("网络中断", MsgBoxStyle.Critical)
            Me.Close()
        End If

    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.Label_packId.Content = currentPackage.packageID
        Me.Label_partNr.Content = currentPackage.partNr
        Me.TextBox_capa.Text = currentPackage.capa
    End Sub

    Private Sub bt_ok_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bt_ok.Click
        If String.IsNullOrWhiteSpace(Me.TextBox_capa.Text) Or Not IsNumeric(Me.TextBox_capa.Text) Or Me.TextBox_capa.Text < 1 Then
            MsgBox("请输入一个大于零的数字")
        Else
            currentPackage.capa = Me.TextBox_capa.Text
            Dim client As packSvc.PackProcessClient = New PackProcessClient
            Dim result As ServiceMessage = client.ModifyPackage(currentPackage)
            Try
                client.Close()
            Catch ex As Exception
                client.Abort()
            End Try
            If result.ReturnedResult = True Then
                MsgBox("更新成功")
                Me.Close()
            Else
                MsgBox("更新失败：" & result.ReturnedMessage(0))
            End If
        End If
    End Sub
End Class
