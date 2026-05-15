Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports Newtonsoft.Json

Public NotInheritable Class Packer

    Private Const MAGIC As String = "CMD1"
    Private Const VERSION As Byte = 1
    Private Const HEADER_SIZE As Integer = 64
    Private Const ENTRY_SIZE As Integer = 22

    Public Shared Sub Pack(dayMapFolder As String, outputCmdPath As String, Optional progress As Action(Of String) = Nothing)

        Dim jsonPath = Path.Combine(dayMapFolder, "TileMapInfo.json")
        If Not File.Exists(jsonPath) Then
            Throw New FileNotFoundException("TileMapInfo.json not found.", jsonPath)
        End If
        Dim jsonBytes = Encoding.UTF8.GetBytes(File.ReadAllText(jsonPath))


        Dim tilesRoot = Path.Combine(dayMapFolder, "Tiles")
        If Not Directory.Exists(tilesRoot) Then
            Throw New DirectoryNotFoundException("Tiles-Directory not found: " & tilesRoot)
        End If

        Dim entries As New List(Of TileEntry)()
        For Each zDir In Directory.GetDirectories(tilesRoot).OrderBy(Function(d) d)
            Dim zVal As Integer
            If Not Integer.TryParse(Path.GetFileName(zDir), zVal) Then Continue For
            For Each xDir In Directory.GetDirectories(zDir).OrderBy(Function(d) d)
                Dim xVal As Integer
                If Not Integer.TryParse(Path.GetFileName(xDir), xVal) Then Continue For
                For Each pngFile In Directory.GetFiles(xDir, "*.png").OrderBy(Function(f) f)
                    Dim yVal As Integer
                    If Not Integer.TryParse(Path.GetFileNameWithoutExtension(pngFile), yVal) Then Continue For
                    entries.Add(New TileEntry With {
                        .Z = zVal, .X = xVal, .Y = yVal, .SourcePath = pngFile
                    })
                Next
            Next
        Next

        If entries.Count = 0 Then
            Throw New InvalidOperationException("No PNG-Tiles found under " & tilesRoot)
        End If

        progress?.Invoke($"0 % – 0 / {entries.Count} Tiles …")

        Dim jsonOffset As Long = HEADER_SIZE
        Dim indexOffset As Long = jsonOffset + 4 + jsonBytes.Length
        Dim dataOffset As Long = indexOffset + CLng(entries.Count) * ENTRY_SIZE

        Dim runningOffset As Long = 0
        For i = 0 To entries.Count - 1
            Dim e = entries(i)
            e.DataOffset = runningOffset
            Dim raw = File.ReadAllBytes(e.SourcePath)
            Using ms As New MemoryStream()
                Using gs As New GZipStream(ms, CompressionLevel.Optimal)
                    gs.Write(raw, 0, raw.Length)
                End Using
                e.RawBytes = ms.ToArray()
            End Using
            e.Length = e.RawBytes.Length
            runningOffset += e.Length

            If progress IsNot Nothing AndAlso (i Mod 1000 = 0) Then
                Dim pct = CInt(Math.Round((i + 1) / entries.Count * 50.0))
                progress($"{pct} % – {i + 1} / {entries.Count} read …")
            End If
        Next

        Dim tmpPath = outputCmdPath & ".tmp"
        Using fs As New FileStream(tmpPath, FileMode.Create, FileAccess.Write,
                                   FileShare.None, bufferSize:=1 << 20)
            Using bw As New BinaryWriter(fs, Encoding.UTF8, leaveOpen:=True)

                bw.Write(Encoding.ASCII.GetBytes(MAGIC))
                bw.Write(VERSION)
                bw.Write(CInt(entries.Count))
                bw.Write(jsonOffset)
                bw.Write(indexOffset)
                bw.Write(dataOffset)
                bw.Write(CShort(ENTRY_SIZE))
                bw.Write(New Byte(28) {})
                ' 4+1+4+8+8+8+2+29 = 64

                If fs.Position <> jsonOffset Then
                    Throw New InvalidOperationException($"Header-Error: Position {fs.Position} ≠ jsonOffset {jsonOffset}")
                End If

                bw.Write(CInt(jsonBytes.Length))
                bw.Write(jsonBytes)

                If fs.Position <> indexOffset Then
                    Throw New InvalidOperationException($"JSON-Error: Position {fs.Position} ≠ indexOffset {indexOffset}")
                End If

                For Each e In entries
                    bw.Write(CShort(e.Z))
                    bw.Write(CInt(e.X))
                    bw.Write(CInt(e.Y))
                    bw.Write(e.DataOffset)
                    bw.Write(CInt(e.Length))
                    ' 2+4+4+8+4 = 22 = ENTRY_SIZE
                Next

                If fs.Position <> dataOffset Then
                    Throw New InvalidOperationException($"Data-Error: Position {fs.Position} ≠ dataOffset {dataOffset}")
                End If

                For i = 0 To entries.Count - 1
                    bw.Write(entries(i).RawBytes)
                    entries(i).RawBytes = Nothing

                    If progress IsNot Nothing AndAlso (i Mod 500 = 0 OrElse i = entries.Count - 1) Then
                        Dim pct = 50 + CInt(Math.Round((i + 1) / entries.Count * 50.0))
                        progress($"{pct} % – {i + 1} / {entries.Count} written …")
                    End If
                Next

            End Using
        End Using

        If File.Exists(outputCmdPath) Then File.Delete(outputCmdPath)
        File.Move(tmpPath, outputCmdPath)

        Dim sizeMB = New FileInfo(outputCmdPath).Length / (1024.0 * 1024.0)
        progress?.Invoke($"Finished! {outputCmdPath} – {sizeMB:F1} MB")
    End Sub

    Private Class TileEntry
        Public Property Z As Integer
        Public Property X As Integer
        Public Property Y As Integer
        Public Property SourcePath As String
        Public Property DataOffset As Long
        Public Property Length As Integer
        Public Property RawBytes As Byte()
    End Class

End Class