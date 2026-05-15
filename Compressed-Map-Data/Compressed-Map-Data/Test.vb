Imports System.IO

Public NotInheritable Class Test

    Public Shared Function Run(cmdPath As String) As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine("=== CMD Map Test ===")
        sb.AppendLine($"File : {cmdPath}")

        If Not File.Exists(cmdPath) Then
            sb.AppendLine("ERROR: File not found!")
            Return sb.ToString
        End If

        Dim fi As New FileInfo(cmdPath)
        sb.AppendLine($"Size : {fi.Length / 1024.0 / 1024.0:F1} MB")

        Dim _reader = Reader.TryCreate(cmdPath)
        If _reader Is Nothing OrElse Not _reader.IsAvailable Then
            sb.AppendLine("ERROR: Reader could not read the file (Magic/Data corrupted).")
            Return sb.ToString
        End If

        sb.AppendLine($"ZoomMin : {_reader.ExportMinZoom}")
        sb.AppendLine($"ZoomMax : {_reader.ExportMaxZoom}")
        sb.AppendLine($"Tiles   : {_reader.ExportMinZoom}..{_reader.ExportMaxZoom} Zoom-levels available")

        Dim stops = _reader.BuildOfflineRenderZoomStops()
        sb.AppendLine($"ZoomStops: {stops.Length} Levels ({If(stops.Length > 0, $"{stops(0):F2} – {stops(stops.Length - 1):F2}", "–")})")

        sb.AppendLine("")
        sb.AppendLine("Test-Render (512x512, Centerpoint)…")
        Dim sw = System.Diagnostics.Stopwatch.StartNew()
        Dim usedZ As Integer = -1
        Dim bmp = _reader.Render(0, 0, 1.0F, 512, 512, usedZ)
        sw.Stop()

        If bmp Is Nothing Then
            sb.AppendLine("WARNING: Render gave Nothing back (Position 0/0 possibly outside the map).")
            sb.AppendLine("         Trying with actual truck position.")
        Else
            sb.AppendLine($"OK  – {bmp.Width}x{bmp.Height} px, TileZoom={usedZ}, {sw.ElapsedMilliseconds} ms")

            Dim outPath = Path.Combine(Path.GetDirectoryName(cmdPath), "CMD_Test_Render.png")
            Try
                bmp.Save(outPath, System.Drawing.Imaging.ImageFormat.Png)
                sb.AppendLine($"Image saved: {outPath}")
            Catch ex As Exception
                sb.AppendLine($"Image could not be saved: {ex.Message}")
            End Try
            bmp.Dispose()
        End If

        If stops.Length > 0 Then
            Dim midStop = stops(stops.Length \ 2)
            sw.Restart()
            Dim bmp2 = _reader.Render(0, 0, midStop, 512, 512, usedZ)
            sw.Stop()
            If bmp2 IsNot Nothing Then
                sb.AppendLine($"Zoom {midStop:F2} – TileZoom={usedZ}, {sw.ElapsedMilliseconds} ms")
                bmp2.Dispose()
            End If
        End If

        _reader.Dispose()
        sb.AppendLine("")
        sb.AppendLine("=== Test completed ===")
        Return sb.ToString
    End Function

End Class