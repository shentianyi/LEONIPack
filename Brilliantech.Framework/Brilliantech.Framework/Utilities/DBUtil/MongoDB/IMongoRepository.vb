Imports MongoDB.Driver.Builders
Imports Brilliantech.Framework.Utilities.DBUtil
Namespace Utilities.DBUtil.MongoDB
    Public Interface IMongoRepository(Of T As Class)
        Inherits IGenericRepository(Of T)

        Sub Mapping()

    End Interface
End Namespace