
Public Interface IMapCompositor

    ReadOnly Property IsAvailable As Boolean
    ReadOnly Property ExportMinZoom As Integer
    ReadOnly Property ExportMaxZoom As Integer

    Function Render(truckX As Single, truckZ As Single, zoom As Single, clientW As Integer, clientH As Integer, ByRef usedTileZoom As Integer) As Drawing.Bitmap

    Function BuildOfflineRenderZoomStops() As Single()

End Interface