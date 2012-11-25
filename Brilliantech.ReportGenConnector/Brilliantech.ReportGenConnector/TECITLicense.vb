Imports TECIT.TFORMer
''' <summary>
''' 一次性注册License信息,每次调用Print函数前调用
''' </summary>
''' <remarks></remarks>
Public Class TECITLicense
    Private Shared ReadOnly pf_licensee As String = "Mem: Cai Zhuo Information & Technology Co.,Ltd"
    Private Shared ReadOnly pf_licenseKey As String = "45D392D071FA5767B4D85D2EE7ED7E76"
    Private Shared ReadOnly pf_numberOfLicense As Integer = 1
    Private Shared ReadOnly pf_licenseKind As LicenseKind = LicenseKind.Developer

    Public Shared Sub Register()
        TFORMer.License(pf_licensee, pf_licenseKind, pf_numberOfLicense, pf_licenseKey)
    End Sub
End Class
