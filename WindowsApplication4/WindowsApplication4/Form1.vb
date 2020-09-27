Imports System.IO.Ports
Imports System.Text
Imports System.Int32
Imports System.Threading
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms


Public Class Form1
    Dim InString1 As String
    Dim Pb, Pc, Pd, pex, ths, tls, tpaf, pttb, tpv As Double
    Dim SHOHT, SHOLT, SHODT, tpt, bts1 As Integer
    Dim Pdatummark, msg As Integer
    Dim report1 As String
    Dim time As String
    Public t, X1, X2, Y1, Y2 As Integer
    Public str As String = "Provider=Microsoft.ACE.Oledb.12.0;Data source=C:\Users\Master\Desktop\電能插座\(電力插座)變異數分析\WindowsApplication4\Database1.accdb" '資料庫檔案位置
    Public conn As OleDbConnection = New OleDbConnection(str)

    Public Function AsciiStringToHexString(ByVal asciiString As String) As String
        'Dim ascii() As Byte = System.Text.Encoding.Default.GetByte s(asciiString)
        Dim ascii() As Byte = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(asciiString)
        Dim count As Integer = ascii.Length
        Dim hexArray(count - 1) As String
        For idx As Integer = 0 To count - 1
            hexArray(idx) = ascii(idx).ToString("x2")
        Next
        Return String.Join(" ", hexArray)
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim buff() As Byte = {&H0, &H3, &H0, &H48, &H0, &H6, &H44, &HF}
        SerialPort1.Open()
        Button1.Enabled = False
        Button2.Enabled = True

        Timer1.Interval = 1000
        Timer1.Enabled = True
        Timer1.Start()
        SHOHT = 1
        SHOLT = 1
        SHODT = 0
        Pdatummark = 400
        'X1 = 0
        'X2 = 186
        'Y1 = 0
        'Y2 = 0
        'TODO: 這行程式碼會將資料載入 'Database1DataSet.資料表1' 資料表。您可以視需要進行移動或移除。
        ' Me.資料表3.TableAdapter.Fill(Me.Database1DataSet.資料表3)
        ' Timer2.Interval = 1000
        '  Timer2.Enabled = True   '顯示時間

        Timer3.Interval = 1500
        Timer3.Enabled = True   '每n秒更新一次資料庫


        SerialPort1.Write(buff, 0, 8)
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim v1, v2, i1, i2, p1, p2, k1, k2, k3, k4, pf1, pf2 As Double
        Dim V, I, P, KWH, PF As Double
        Dim ga, g1 As Double
        Dim caa As String

        TextBox2.Text = Now
        time = Format(Now(), "yyyy/M/d")
        Dim buff() As Byte = {&H0, &H3, &H0, &H48, &H0, &H6, &H44, &HF}
        Timer1.Interval = 1000
        Timer1.Start()
        InString1 = SerialPort1.ReadExisting()
        Thread.SpinWait(1000)
        SerialPort1.Write(buff, 0, 8)
        Thread.Sleep(500)
        InString1 = SerialPort1.ReadExisting()
        caa = AsciiStringToHexString(InString1)

        TextBox1.Text = caa
        Dim ch1 As Char() = New [Char]() {" "}
        Dim spi As String() = caa.Split(ch1)
        If spi.Length > 16 Then
            'Thread.Sleep(500)
            SerialPort1.Write(buff, 0, 8)
            Thread.Sleep(500)
        Else
            Thread.Sleep(500)

        End If
        If spi.Length > 16 Then
            v1 = H2D(spi(3))
            v2 = H2D(spi(4))
            V = ((v1 * 256) + v2) / 100
            'text1.Text = FormatNumber(0.12345, 3)
            Label1.Text = Math.Round(V, 2)  '若有資料則加到接收的文字框
            '    Thread.Sleep(500)
            i1 = H2D(spi(5))
            i2 = H2D(spi(6))
            I = ((i1 * 256) + i2) / 1000
            Label2.Text = Math.Round(I, 2) '若有資料則加到接收的文字框
            '   Thread.Sleep(500)
            p1 = H2D(spi(7))
            p2 = H2D(spi(8))
            P = ((p1 * 256) + p2)
            'text1.Text = FormatNumber(0.12345, 3)
            Label3.Text = Math.Round(P, 2)  '若有資料則加到接收的文字框
            '     Thread.Sleep(500)
            k1 = H2D(spi(9))
            k2 = H2D(spi(10))
            k3 = H2D(spi(11))
            k4 = H2D(spi(12))
            KWH = ((k1 * 66536) + (k2 * 4096) + (k3 * 256) + k4) / 3200
            If g1 = 0 Then
                g1 = KWH
            End If
            ga = KWH - g1
            'text1.Text = FormatNumber(0.12345, 3)
            Label4.Text = Math.Round(KWH, 2)  '若有資料則加到接收的文字框
            ' Label5.Text = Math.Round(ga, 2)
            '    Thread.Sleep(500)
            pf1 = H2D(spi(13))
            pf2 = H2D(spi(14))
            PF = ((pf1 * 256) + pf2) / 1000
            'text1.Text = FormatNumber(0.12345, 3)
            Label6.Text = Math.Round(PF, 2)  '若有資料則加到接收的文字框
            SHODT = SHODT + 1
            Thread.Sleep(50)






            'If SHODT <= 200 Then
            '    If P >= 400 Then

            '        Pb = Pb + P
            '        Pc = Pb / SHOHT
            '        TextBox4.Text = Math.Round(Pc, 2)
            '        SHOHT = SHOHT + 1

            '    ElseIf P < 400 Then

            '        Pd = Pd + P
            '        pe = Pd / SHOLT
            '        TextBox5.Text = Math.Round(pe, 2)
            '        SHOLT = SHOLT + 1
            '    End If
            'End If
            'If SHODT > 200 Then
            '    If P >= 400 Then
            '        VarianceH = P - Pc
            '        TextBox6.Text = Math.Round(VarianceH, 2)
            '        TextBox7.Text = "H"

            '    ElseIf P < 400 Then
            '        VarianceL = P - pe
            '        TextBox6.Text = Math.Round(VarianceL, 2)
            '        TextBox7.Text = "L"
            '    End If
            'End If
            'Dim x, y As Integer
            'Dim g As Graphics = PictureBox1.CreateGraphics()

            'g.DrawLine(Pens.Green, 0, 186, 735, 186)
            'If SHODT > 200 Then
            '    x = 186 - (TextBox6.Text * 9.3)
            '    g.DrawLine(Pens.Black, X1, X2, (t + 1) * 5, x)
            '    X1 = (t + 1) * 5
            '    X2 = x
            '    t = t + 1
            'End If
            'If t >= 735 Then
            '    t = 0
            'End If

        End If
    End Sub


    Public Function H2D(ByVal Hex As String) As Long
        Dim i As Long
        Dim b As Long

        Hex = UCase(Hex)
        For i = 1 To Len(Hex)
            Select Case Mid(Hex, Len(Hex) - i + 1, 1)
                Case "0" : b = b + 16 ^ (i - 1) * 0
                Case "1" : b = b + 16 ^ (i - 1) * 1
                Case "2" : b = b + 16 ^ (i - 1) * 2
                Case "3" : b = b + 16 ^ (i - 1) * 3
                Case "4" : b = b + 16 ^ (i - 1) * 4
                Case "5" : b = b + 16 ^ (i - 1) * 5
                Case "6" : b = b + 16 ^ (i - 1) * 6
                Case "7" : b = b + 16 ^ (i - 1) * 7
                Case "8" : b = b + 16 ^ (i - 1) * 8
                Case "9" : b = b + 16 ^ (i - 1) * 9
                Case "A" : b = b + 16 ^ (i - 1) * 10
                Case "B" : b = b + 16 ^ (i - 1) * 11
                Case "C" : b = b + 16 ^ (i - 1) * 12
                Case "D" : b = b + 16 ^ (i - 1) * 13
                Case "E" : b = b + 16 ^ (i - 1) * 14
                Case "F" : b = b + 16 ^ (i - 1) * 15
            End Select
        Next i
        H2D = b
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button1.Enabled = True
        Button2.Enabled = False
        SerialPort1.Close()

    End Sub

    Private Sub Label11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label11.Click

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        TextBox2.Text = Now
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: 這行程式碼會將資料載入 'Database1DataSet.資料表3' 資料表。您可以視需要進行移動或移除。
        Me.資料表3TableAdapter.Fill(Me.Database1DataSet.資料表3)
        'TODO: 這行程式碼會將資料載入 'Database1DataSet1.資料表3' 資料表。您可以視需要進行移動或移除。
        Me.資料表3TableAdapter.Fill(Me.Database1DataSet.資料表3)
        'TODO: 這行程式碼會將資料載入 'Database1DataSet1.資料表3' 資料表。您可以視需要進行移動或移除。
        Me.資料表3TableAdapter.Fill(Me.Database1DataSet.資料表3)
        'TODO: 這行程式碼會將資料載入 'Database1DataSet.資料表3' 資料表。您可以視需要進行移動或移除。
        Me.資料表3TableAdapter.Fill(Me.Database1DataSet.資料表3)
        Button2.Enabled = False
        ' time = Format(Now(), "yyyy/M/d")
        Pdatummark = 400
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Dim tatta As Integer
        conn.Open()               '抓取需要的資料放入DATABASE 中
        Dim str2 As String = "Insert Into 資料表3(時間,電壓,電流,功率,功因,總電度,變異數,DataRun,日期)Values ('" & TextBox2.Text & "','" & Label1.Text & "','" & Label2.Text & "','" & Label3.Text & "','" & Label6.Text & "','" & Label4.Text & "','" & TextBox6.Text & "','" & SHODT & "','" & time & "')"
        'Dim str2 As String = "Insert Into 資料表3(時間,電壓,電流,功率,電度,功因,總電度,變異數,DataRun,日期)Values ('" & TextBox2.Text & "','" & Label1.Text & "','" & Label2.Text & "','" & Label3.Text & "','" & Label5.Text & "','" & Label6.Text & "','" & Label4.Text & "','" & TextBox6.Text & "','" & SHODT & "','" & time & "')"
        'Dim str1 As String = "Insert Into 資料表1(時間,電壓)Values('" & TextBox2.Text & "','" & Label1.Text & "')"
        Dim cmd As OleDbCommand = New OleDbCommand(str2, conn)
        Try
            cmd.ExecuteNonQuery()
            conn.Close()
            TextBox3.Text = "新增成功"
        Catch ex As Exception

            TextBox3.Text = "資料更新失敗"
            conn.Close()
        End Try


        DataGridView1.Show()    '把Database中的資料全部顯示進DataGridView中
        conn.Open()
        ' Dim str4 As String = "SELECT * FROM 資料表3 WHERE 功率"
        Dim str1 As String = "select * from 資料表3"     '從資料表三中查詢資料
        Dim adp1 As OleDbDataAdapter = New OleDbDataAdapter(str1, conn)
        ' Dim adp2 As OleDbDataAdapter = New OleDbDataAdapter(str4, conn)
        Dim set1 As DataSet = New DataSet
        '  Dim set4 As DataSet = New DataSet
        adp1.Fill(set1, "1a")                               '將查詢結果放到記憶體set1上的"1a "表格內
        DataGridView1.DataSource = set1.Tables("1a")        '將記憶體的資料集合存放到視窗畫面上的DataGrid上
        Dim i As Integer = DataGridView1.Rows.Count - 1                     '把指標放在
        DataGridView1.CurrentCell = DataGridView1.Rows(i).Cells(0)          '新增的資料上面
        conn.Close()

        If bts1 = 1 Then                                    '若基準按鈕按下後

            tatta = tpaf + (pttb - Label3.Text) ^ 2
            tpv = tatta / (tpt + 1)
            TextBox6.Text = Math.Round(tpv, 2)
            TextBox18.Text = Math.Round(tpv, 2)
        End If

        '----------------------------------------------------------------------------"
        If bts1 = 0 Then
            If SHODT > 2 Then                                   '若資料跑2筆之後(不然算出來會有錯)
                conn.Open()

                Dim cmd1 As OleDbCommand = New OleDbCommand("select*from 資料表3 where 功率", conn)          '設定從資料表三中查詢功率那行的資料
                Dim dr As OleDbDataReader = cmd1.ExecuteReader
                Dim th, tl As Double
                Dim start(1000000) As Integer
                Dim starth(100000) As Integer
                Dim startl(1000000) As Integer
                Dim stl, sbl, sth, sbh As Integer
                Dim pas, pah, pal As Integer
                Dim VarianceH, VarianceL As Double

                Do While dr.Read()                          '一行一行讀取Database 中功率的資料
                    TextBox8.Text = dr.Item("功率")
                    start(pas) = TextBox8.Text
                    If start(pas) > Pdatummark Then         '如果為設定之功率準位以上
                        starth(pah) = start(pas)
                        sth = sth + start(pas)
                        pah = pah + 1


                    ElseIf start(pas) < Pdatummark Then     '如果為設定之功率準位以下
                        startl(pal) = start(pas)
                        stl = stl + start(pas)
                        pal = pal + 1


                    End If

                    pas = pas + 1




                Loop
                dr.Close()                                  '關閉查詢
                conn.Close()
                TextBox9.Text = pas
                If pah > 0 Then
                    sbh = sth / pah                         '計算平均
                    TextBox7.Text = pah                     '資料筆數
                    TextBox4.Text = sbh
                End If

                sbl = stl / pal
                TextBox5.Text = pal
                TextBox10.Text = sbl
                For aggg As Integer = 0 To pah - 1
                    VarianceH = VarianceH + (sbh - starth(aggg)) ^ 2                '計算變異數公式為(E(Xb-Xi)^2)/N-1

                Next
                th = VarianceH / (pah - 1)
                'th = Math.Sqrt(th)
                TextBox12.Text = Math.Round(th, 2)


                For ggg As Integer = 0 To pal - 1
                    VarianceL = VarianceL + (sbl - startl(ggg)) ^ 2


                Next
                tl = VarianceL / (pal - 1)
                tl = Math.Sqrt(tl)
                TextBox11.Text = Math.Round(tl, 2)

                If start(pas) > Pdatummark Then
                    TextBox6.Text = Math.Round(th, 2)
                ElseIf start(pas) < Pdatummark Then
                    TextBox6.Text = Math.Round(tl, 2)

                End If
                If SHODT > 3 Then


                    If msg = 0 Then
                        If th > ths * 3 Then
                            report1 = MsgBox(Now + "      設備異常", vbOKOnly, "警告")
                            msg = 1
                        End If
                        If tl > tls * 3 Then
                            msg = 1
                            report1 = MsgBox(Now + "      設備異常", vbOKOnly, "警告")

                        End If
                    End If
                End If
                If report1 = 1 Then
                    msg = 0
                    report1 = 0
                End If
                ths = th
                tls = tl
                pas = 0
                pah = 0
                pal = 0
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub Label15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub

    'Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
    '    conn.Open()

    '    Dim cmd1 As OleDbCommand = New OleDbCommand("select*from 資料表3 where 功率", conn)
    '    Dim dr As OleDbDataReader = cmd1.ExecuteReader
    '    Dim th, tl As Double
    '    Dim start(100000) As Integer
    '    Dim starth(10000) As Integer
    '    Dim startl(100000) As Integer
    '    Dim stl, sbl, sth, sbh As Integer
    '    Dim pas, pah, pal As Integer
    '    Dim VarianceH, VarianceL As Double

    '    Do While dr.Read()
    '        TextBox8.Text = dr.Item("功率")
    '        start(pas) = TextBox8.Text
    '        If start(pas) > 400 Then
    '            starth(pah) = start(pas)
    '            sth = sth + start(pas)
    '            pah = pah + 1


    '        ElseIf start(pas) < 400 Then
    '            startl(pal) = start(pas)
    '            stl = stl + start(pas)
    '            pal = pal + 1


    '        End If

    '        pas = pas + 1




    '    Loop
    '    dr.Close()
    '    conn.Close()
    '    TextBox9.Text = pas
    '    sbh = sth / pah
    '    TextBox7.Text = pah
    '    TextBox4.Text = sbh
    '    sbl = stl / pal
    '    TextBox5.Text = pal
    '    TextBox10.Text = sbl
    '    For aggg As Integer = 0 To pah - 1
    '        VarianceH = VarianceH + (sbh - starth(aggg)) ^ 2

    '    Next
    '    th = VarianceH / (pah - 1)
    '    th = Math.Sqrt(th)
    '    TextBox12.Text = Math.Round(th, 2)


    '    For ggg As Integer = 0 To pal - 1
    '        VarianceL = VarianceL + (sbl - startl(ggg)) ^ 2


    '    Next
    '    tl = VarianceL / (pal - 1)
    '    tl = Math.Sqrt(tl)
    '    TextBox11.Text = Math.Round(tl, 2)

    '    If start(pas) > 400 Then
    '        TextBox6.Text = Math.Round(th, 2)
    '    ElseIf start(pas) < 400 Then
    '        TextBox6.Text = Math.Round(tl, 2)

    '    End If


    '    pas = 0
    '    pah = 0
    '    pal = 0
    'End Sub

    'Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick

    'End Sub

    Private Sub DataGridView1_CellContentClick_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub
    Private Sub MonthCalendar1_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles MonthCalendar1.DateChanged
        MonthCalendar1.MaxSelectionCount = 14

        If MonthCalendar1.SelectionRange.Start = _
            MonthCalendar1.SelectionRange.End Then

            TextBox13.Text = CStr(MonthCalendar1.SelectionStart)

        Else

            TextBox14.Text = MonthCalendar1.SelectionRange.End

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        bts1 = 1
        Button3.Enabled = False
        Button4.Enabled = True
        conn.Open()
        Dim cmd2 As OleDbCommand = New OleDbCommand("select*from 資料表3 where 日期 & 功率", conn)
        Dim dr2 As OleDbDataReader = cmd2.ExecuteReader
        Dim pttl, tpav As Double
        Dim start2(1000000) As String
        Dim starth(1000000) As Integer
        Dim tstartl(1000000) As Integer
        Dim tstl, tsth As Integer
        Dim tpas, tpah, tpal, ddata As Integer
        tstl = 0
        tsth = 0
        Do While dr2.Read()                          '一行一行讀取Database 中功率的資料
            start2(tpas) = dr2.Item("日期")
            starth(tpas) = dr2.Item("功率")

            If start2(tpas) = TextBox13.Text Then    '符合最先日期
                If tstl = 0 Then
                    tpal = tpas
                    tstl = 1
                End If
            End If
            If start2(tpas) = TextBox14.Text Then   '符合最後日期

                tpah = tpas

            End If
            tpas = tpas + 1
        Loop
        dr2.Close()
        conn.Close()
        tpt = tpah - tpal
        tpt = tpt + 1
        TextBox15.Text = Math.Abs(tpt)     'base總筆數
        If tpt > 0 Then


            For tab As Integer = tpal To tpah
                If starth(tab) > 0 & starth(tab) < Pdatummark Then

                    pttl = pttl + starth(tab)   '加總數

                Else
                    ddata = ddata + 1

                End If

            Next
            pttb = pttl / tpt     '算平均

            For tav As Integer = tpal To tpah - ddata

                tpaf = tpaf + (pttb - starth(tav)) ^ 2  'base變異數分子

            Next
            tpav = tpaf / tpt
            TextBox16.Text = Math.Round(tpav, 2)
            TextBox17.Text = Math.Round(pttb, 2)
        End If
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged

    End Sub

    Private Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        bts1 = 0
        Button3.Enabled = True
        Button4.Enabled = False
        TextBox13.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
        TextBox16.Text = ""
        TextBox17.Text = ""
        TextBox18.Text = ""
    End Sub

    Private Sub DataGridView1_CellContentClick_2(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub TextBox8_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox8.TextChanged

    End Sub
End Class
