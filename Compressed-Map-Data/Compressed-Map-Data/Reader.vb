Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports Newtonsoft.Json

Public NotInheritable Class Reader
    Implements IMapCompositor

    Private Const MAGIC As String = "CMD1"
    Private Const ENTRY_SIZE As Integer = 22  ' Z:2 + X:4 + Y:4 + Offset:8 + Len:4

    Private ReadOnly _cmdPath As String
    Private ReadOnly _info As TileMapInfo
    Private ReadOnly _index As Dictionary(Of Long, IndexEntry) ' key = PackKey(z,x,y)
    Private ReadOnly _dataStart As Long
    Private ReadOnly _tileCache As TileCache
    Private ReadOnly _fsLock As New Object()
    Private _fs As FileStream

    Private Class TileMapInfo
        Public Property x1 As Double
        Public Property x2 As Double
        Public Property y1 As Double
        Public Property y2 As Double
        Public Property minZoom As Integer
        Public Property maxZoom As Integer
    End Class

    Private Structure IndexEntry
        Public Offset As Long
        Public CompLen As Integer
    End Structure

    Private Class TileCache
        Private ReadOnly _cap As Integer
        Private ReadOnly _map As New Dictionary(Of Long, LinkedListNode(Of (Key As Long, Bmp As Bitmap)))()
        Private ReadOnly _lru As New LinkedList(Of (Key As Long, Bmp As Bitmap))()
        Private ReadOnly _lock As New Object()

        Public Sub New(capacity As Integer)
            _cap = Math.Max(4, capacity)
        End Sub

        Public Function TryGet(key As Long, ByRef bmp As Bitmap) As Boolean
            SyncLock _lock
                Dim node As LinkedListNode(Of (Key As Long, Bmp As Bitmap)) = Nothing
                If Not _map.TryGetValue(key, node) Then Return False
                _lru.Remove(node)
                _lru.AddFirst(node)
                bmp = node.Value.Bmp
                Return True
            End SyncLock
        End Function

        Public Sub Put(key As Long, bmp As Bitmap)
            SyncLock _lock
                If _map.ContainsKey(key) Then Return
                If _lru.Count >= _cap Then
                    Dim last = _lru.Last
                    _lru.RemoveLast()
                    _map.Remove(last.Value.Key)
                    last.Value.Bmp?.Dispose()
                End If
                Dim node = _lru.AddFirst((key, bmp))
                _map(key) = node
            End SyncLock
        End Sub

        Public Sub Dispose()
            SyncLock _lock
                For Each n In _lru
                    n.Bmp?.Dispose()
                Next
                _lru.Clear()
                _map.Clear()
            End SyncLock
        End Sub
    End Class

    Private Sub New(cmdPath As String, info As TileMapInfo,
                    index As Dictionary(Of Long, IndexEntry), dataStart As Long)
        _cmdPath = cmdPath
        _info = info
        _index = index
        _dataStart = dataStart
        _tileCache = New TileCache(capacity:=256)
        _fs = New FileStream(cmdPath, FileMode.Open, FileAccess.Read,
                                    FileShare.Read, bufferSize:=1 << 16)
    End Sub

    Public Shared Function TryCreate(Optional explicitPath As String = Nothing) As Reader
        Dim candidates As New List(Of String)()

        If Not String.IsNullOrEmpty(explicitPath) Then
            candidates.Add(explicitPath)
        End If

        Dim dir As New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory) ' For testing purposes only. A link to the actual CMD file should be provided via explicitPath.
        For i = 0 To 9
            For Each f In dir.GetFiles("*.CMD")
                If Not candidates.Contains(f.FullName) Then candidates.Add(f.FullName)
            Next
            If dir.Parent Is Nothing Then Exit For
            dir = dir.Parent
        Next

        For Each path In candidates
            Dim reader = TryLoad(path)
            If reader IsNot Nothing Then Return reader
        Next
        Return Nothing
    End Function

    Private Shared Function TryLoad(cmdPath As String) As Reader
        If Not File.Exists(cmdPath) Then Return Nothing
        Try
            Using fs As New FileStream(cmdPath, FileMode.Open, FileAccess.Read,
                                       FileShare.Read, bufferSize:=4096)
                Using br As New BinaryReader(fs, Encoding.UTF8, leaveOpen:=True)
                    Dim magic = Encoding.ASCII.GetString(br.ReadBytes(4))
                    If magic <> "CMD1" Then Return Nothing

                    Dim ver = br.ReadByte()
                    Dim tileCount = br.ReadInt32()
                    Dim jsonOff = br.ReadInt64()
                    Dim indexOff = br.ReadInt64()
                    Dim dataStart = br.ReadInt64()
                    Dim entrySize = br.ReadInt16()

                    ' JSON
                    fs.Seek(jsonOff, SeekOrigin.Begin)
                    Dim jsonLen = br.ReadInt32()
                    Dim jsonBytes = br.ReadBytes(jsonLen)
                    Dim jsonText = Encoding.UTF8.GetString(jsonBytes)
                    Dim info = JsonConvert.DeserializeObject(Of TileMapInfo)(jsonText)
                    If info Is Nothing Then Return Nothing

                    fs.Seek(indexOff, SeekOrigin.Begin)
                    Dim index As New Dictionary(Of Long, IndexEntry)(tileCount)
                    For i = 0 To tileCount - 1
                        Dim z = CInt(br.ReadInt16())
                        Dim x = br.ReadInt32()
                        Dim y = br.ReadInt32()
                        Dim offset = br.ReadInt64()
                        Dim clen = br.ReadInt32()

                        If entrySize > ENTRY_SIZE Then
                            br.ReadBytes(entrySize - ENTRY_SIZE)
                        End If
                        index(PackKey(z, x, y)) = New IndexEntry With {
                            .Offset = offset, .CompLen = clen
                        }
                    Next

                    Return New Reader(cmdPath, info, index, dataStart)
                End Using
            End Using
        Catch
            Return Nothing
        End Try
    End Function


    Public ReadOnly Property IsAvailable As Boolean Implements IMapCompositor.IsAvailable
        Get
            Return _info IsNot Nothing AndAlso _index IsNot Nothing AndAlso _index.Count > 0
        End Get
    End Property

    Public ReadOnly Property ExportMinZoom As Integer Implements IMapCompositor.ExportMinZoom
        Get
            Return If(_info Is Nothing, 0, Math.Max(0, _info.minZoom))
        End Get
    End Property

    Public ReadOnly Property ExportMaxZoom As Integer Implements IMapCompositor.ExportMaxZoom
        Get
            If _info Is Nothing Then Return 9
            Return Math.Max(ExportMinZoom, Math.Min(14, _info.maxZoom))
        End Get
    End Property

    Public Shared Function ComputeRenderSize(clientW As Integer, clientH As Integer) As Integer
        Dim wf = CSng(clientW)
        Dim hf = CSng(clientH)
        Dim diag = CInt(Math.Ceiling(Math.Sqrt(wf * wf + hf * hf)))
        Return Math.Max(diag, 512)
    End Function

    Public Function Render(truckX As Single, truckZ As Single, zoom As Single,
                           clientW As Integer, clientH As Integer,
                           ByRef usedTileZoom As Integer) As Bitmap Implements IMapCompositor.Render
        usedTileZoom = -1
        If Not IsAvailable OrElse zoom <= 0.0001F Then Return Nothing

        Dim outSize = ComputeRenderSize(clientW, clientH)
        Dim halfWorld = (outSize / 2.0F) / zoom

        Dim wx0 = truckX - halfWorld
        Dim wx1 = truckX + halfWorld
        Dim wz0 = truckZ - halfWorld
        Dim wz1 = truckZ + halfWorld

        Dim xSpan = _info.x2 - _info.x1
        Dim ySpan = _info.y2 - _info.y1
        If xSpan <= 0 OrElse ySpan <= 0 Then Return Nothing

        Dim mppTarget = (2.0F * halfWorld) / outSize
        Dim zUse = PickZoomFromMpp(mppTarget)
        usedTileZoom = zUse

        Dim n = 1 << zUse
        Dim tileWorldW = CSng(xSpan / n)
        Dim tileWorldH = CSng(ySpan / n)

        Dim bmp As New Bitmap(outSize, outSize, Imaging.PixelFormat.Format32bppPArgb)
        Using g = Graphics.FromImage(bmp)
            g.Clear(Color.FromArgb(224, 218, 200))
            g.InterpolationMode = InterpolationMode.NearestNeighbor
            g.PixelOffsetMode = PixelOffsetMode.Half
            g.CompositingMode = CompositingMode.SourceCopy

            Dim tx0 = CInt(Math.Floor((wx0 - _info.x1) / tileWorldW))
            Dim tx1 = CInt(Math.Floor((wx1 - _info.x1) / tileWorldW))
            Dim ty0 = CInt(Math.Floor((wz0 - _info.y1) / tileWorldH))
            Dim ty1 = CInt(Math.Floor((wz1 - _info.y1) / tileWorldH))

            tx0 = Math.Max(0, Math.Min(n - 1, tx0))
            tx1 = Math.Max(0, Math.Min(n - 1, tx1))
            ty0 = Math.Max(0, Math.Min(n - 1, ty0))
            ty1 = Math.Max(0, Math.Min(n - 1, ty1))

            For tx = tx0 To tx1
                For ty = ty0 To ty1
                    Dim tileBmp = GetTile(zUse, tx, ty)
                    If tileBmp Is Nothing Then Continue For

                    Dim twx0 = CSng(_info.x1 + tx * tileWorldW)
                    Dim twx1 = twx0 + tileWorldW
                    Dim twz0 = CSng(_info.y1 + ty * tileWorldH)
                    Dim twz1 = twz0 + tileWorldH

                    Dim px0 = (twx0 - wx0) / (wx1 - wx0) * outSize
                    Dim px1 = (twx1 - wx0) / (wx1 - wx0) * outSize
                    Dim py0 = (twz0 - wz0) / (wz1 - wz0) * outSize
                    Dim py1 = (twz1 - wz0) / (wz1 - wz0) * outSize

                    Dim ix0 = CInt(Math.Floor(CDbl(px0)))
                    Dim iy0 = CInt(Math.Floor(CDbl(py0)))
                    Dim ix1 = Math.Min(outSize, CInt(Math.Ceiling(CDbl(px1))) + 1)
                    Dim iy1 = Math.Min(outSize, CInt(Math.Ceiling(CDbl(py1))) + 1)

                    Dim dw = Math.Max(1, ix1 - ix0)
                    Dim dh = Math.Max(1, iy1 - iy0)
                    g.DrawImage(tileBmp,
                                New Rectangle(ix0, iy0, dw, dh),
                                New Rectangle(0, 0, tileBmp.Width, tileBmp.Height),
                                GraphicsUnit.Pixel)
                Next
            Next
        End Using

        Return bmp
    End Function


    Public Function BuildOfflineRenderZoomStops() As Single() Implements IMapCompositor.BuildOfflineRenderZoomStops
        If Not IsAvailable Then Return Array.Empty(Of Single)()
        Const r0 As Single = 0.12F
        Const r1 As Single = 18.0F
        Const steps = 480

        Dim bands As New List(Of (pick As Integer, rStart As Single, rEnd As Single))()
        Dim bandStartR = r0
        Dim lastPick = PickAtRenderZoom(r0)

        For i = 1 To steps
            Dim t = i / CSng(steps)
            Dim r = r0 + (r1 - r0) * t
            Dim p = PickAtRenderZoom(r)
            If p <> lastPick OrElse i = steps Then
                If lastPick >= ExportMinZoom AndAlso lastPick <= ExportMaxZoom Then
                    bands.Add((lastPick, bandStartR, r))
                End If
                bandStartR = r
                lastPick = p
            End If
        Next

        If bands.Count = 0 Then Return Array.Empty(Of Single)()

        Dim stepsPerBand = 6
        Dim candidates As New List(Of Single)()
        For Each b In bands
            Dim span = b.rEnd - b.rStart
            For j = 0 To stepsPerBand - 1
                Dim v = If(stepsPerBand = 1,
                           (b.rStart + b.rEnd) * 0.5F,
                           b.rStart + span * (j / CSng(stepsPerBand - 1)))
                candidates.Add(v)
            Next
        Next

        candidates = candidates.OrderBy(Function(x) x).ToList()

        Dim dedup As New List(Of Single)()
        Dim lastAdded As Single = -1.0F
        For Each v In candidates
            If dedup.Count = 0 OrElse Math.Abs(v - lastAdded) > 0.02F Then
                dedup.Add(v)
                lastAdded = v
            End If
        Next
        Return dedup.ToArray()
    End Function

    Private Function GetTile(z As Integer, x As Integer, y As Integer) As Bitmap
        Dim key = PackKey(z, x, y)

        Dim cached As Bitmap = Nothing
        If _tileCache.TryGet(key, cached) Then Return cached

        Dim entry As IndexEntry
        If Not _index.TryGetValue(key, entry) Then Return Nothing

        Dim _rawPng As Byte()
        SyncLock _fsLock
            _fs.Seek(_dataStart + entry.Offset, SeekOrigin.Begin)
            Dim compressed(entry.CompLen - 1) As Byte
            Dim totalRead = 0
            While totalRead < entry.CompLen
                Dim n = _fs.Read(compressed, totalRead, entry.CompLen - totalRead)
                If n = 0 Then Exit While
                totalRead += n
            End While

            Using ms As New MemoryStream(compressed)
                Using gs As New GZipStream(ms, CompressionMode.Decompress)
                    Using outMs As New MemoryStream()
                        gs.CopyTo(outMs)
                        _rawPng = outMs.ToArray()
                    End Using
                End Using
            End Using
        End SyncLock

        Dim bmp As Bitmap
        Using ms As New MemoryStream(_rawPng)
            ms.Position = 0
            Using tmp As New Bitmap(ms)
                bmp = New Bitmap(tmp)
            End Using
        End Using

        _tileCache.Put(key, bmp)
        Return bmp
    End Function

    Private Shared Function PackKey(z As Integer, x As Integer, y As Integer) As Long
        ' z: max 14 (4 Bit), x/y: max 2^14 = 16384 (14 Bit each)
        Return (CLng(z) << 28) Or (CLng(x) << 14) Or CLng(y)
    End Function

    Private Function PickZoomFromMpp(mppTarget As Single) As Integer
        Dim lo = Math.Max(_info.minZoom, 0)
        Dim hi = Math.Max(lo, Math.Min(ExportMaxZoom, 14))
        Dim best = lo
        For zt = hi To lo Step -1
            Dim n = 1 << zt
            Dim tileWorldW = CSng((_info.x2 - _info.x1) / n)
            Dim mppTile = tileWorldW / 256.0F
            If mppTile <= mppTarget * 1.35F Then
                best = zt
                Exit For
            End If
        Next
        Return best
    End Function

    Private Function PickAtRenderZoom(renderZoom As Single) As Integer
        If renderZoom <= 0.000001F Then Return ExportMinZoom
        Return PickZoomFromMpp(1.0F / renderZoom)
    End Function

    Public Sub Dispose()
        _tileCache.Dispose()
        SyncLock _fsLock
            _fs?.Dispose()
        End SyncLock
    End Sub

End Class