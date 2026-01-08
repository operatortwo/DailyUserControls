Imports DailyUserControls

Class MainWindow

    Private Shared VU_RefreshTimer As New Timers.Timer(50)       ' 50 ms Screen Timer (= 20 FPS)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        'Dim appver As String = My.Application.Info.Version.ToString(3)
        Dim appver As String = My.Application.Info.Version.ToString
        Dim libname As Reflection.AssemblyName = GetType(DailyUserControls.ImageButton).Assembly.GetName()
        Dim libver As String = libname.Version.ToString
        Me.Title = Title & " V " & appver & " - Library V " & libver

        AddHandler VU_RefreshTimer.Elapsed, AddressOf VU_RefreshTimer_Tick

        tiImageButton.Focus()

        '--- Data to CheckboxFilterList
        CbflistEnum.ItemList = [Enum].GetValues(GetType(Months))
        CbflistVal.ItemList = ValueList
        CbflistRef.ItemList = ColorList
        '--- for simple string-list: 
        ' until V 1.0.9.1: need "DisplayMember = "Name"
        ' V 1.0.9.2 and up: when DisplayMember is empty, DisplayMember is set automatically to "Name"
        CbflistString.ItemList = StringList

    End Sub

#Region "Info About"
    Private Sub MiAbout_Click(sender As Object, e As RoutedEventArgs) Handles MiAbout.Click
        Dim win As New AboutWindow
        win.Owner = Me
        win.ShowDialog()
    End Sub
#End Region

#Region "Image Button"
    Private Sub cbToggleIsEnabled_Checked(sender As Object, e As RoutedEventArgs) Handles cbToggleIsEnabled.Checked
        ImageButton1.IsEnabled = True
    End Sub

    Private Sub cbToggleIsEnabled_Unchecked(sender As Object, e As RoutedEventArgs) Handles cbToggleIsEnabled.Unchecked
        ImageButton1.IsEnabled = False
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As RoutedEventArgs) Handles ImageButton1.Click
        tblk_ImageButtonMsg.Text = "ImageButton pressed"
    End Sub

    Private Sub PlayButton_Click(sender As Object, e As RoutedEventArgs) Handles PlayButton.Click
        tblk_ImageButtonMsg.Text = "Play pressed"
    End Sub

    Private Sub StopButton_Click(sender As Object, e As RoutedEventArgs) Handles StopButton.Click
        tblk_ImageButtonMsg.Text = "Stop pressed"
    End Sub
#End Region

#Region "Numeric UpDown"

#Region "NumericUpDown 1"
    Private Sub NumericUpDown_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles NumericUpDown1.ValueChanged
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText("Value changed = " & NumericUpDown1.Value & vbCrLf)
        tbNudMsg.ScrollToEnd()
        'e.Handled = True
    End Sub

    Private Sub btnNudSilent_Click(sender As Object, e As RoutedEventArgs) Handles btnNudSilent.Click
        NumericUpDown1.SetValueSilent(5.5)
    End Sub

    Private Sub cbNumericUpDownWheelHandled_Click(sender As Object, e As RoutedEventArgs) Handles cbNumericUpDownWheelHandled.Click
        If cbNumericUpDownWheelHandled.IsChecked = True Then
            NumericUpDown1.MouseWheelHandled = True
            tbNumericUpDownWheel.Clear()
        Else
            NumericUpDown1.MouseWheelHandled = False
        End If
    End Sub

    Private Sub NumericUpDownGrid_MouseWheel(sender As Object, e As MouseWheelEventArgs) Handles NumericUpDownGrid.MouseWheel
        Dim nud As NumericUpDown = TryCast(e.Source, NumericUpDown)
        If nud IsNot Nothing Then
            Dim sname As String = nud.Name
            If tbNumericUpDownWheel.LineCount > 100 Then tbNumericUpDownWheel.Clear()
            If nud.Name = "NumericUpDown1" Then
                tbNumericUpDownWheel.AppendText("Wheel: " & NumericUpDown1.Value & vbCrLf)
                tbNumericUpDownWheel.ScrollToEnd()
            End If
        End If
    End Sub

#End Region

    Private Sub nud2_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles nud2.ValueChanged
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText("nud2 value: " & e.NewValue & vbCrLf)
        tbNudMsg.ScrollToEnd()
    End Sub

    Private Sub nud3_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles nud3.ValueChanged
        tbNudMsg.AppendText("nud3 value: " & e.NewValue & vbCrLf)
        tbNudMsg.ScrollToEnd()
    End Sub

    Private Sub nud3_SpinUp(sender As Object, e As RoutedEventArgs) Handles nud3.SpinUp
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText("SpinUp Event => Min" & vbCrLf)
        tbNudMsg.ScrollToEnd()
        nud3.SetValueSilent(nud3.MinimumValue)
    End Sub

    Private Sub nud3_SpinDown(sender As Object, e As RoutedEventArgs) Handles nud3.SpinDown
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText("SpinDown Event => Max" & vbCrLf)
        tbNudMsg.ScrollToEnd()
        nud3.SetValueSilent(nud3.MaximumValue)
    End Sub

#End Region

#Region "Selector Button"
    Private Sub SelectorButton1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Integer)) Handles SelectorButton1.ValueChanged
        If tbSelButtonMsg.LineCount > 200 Then tbSelButtonMsg.Clear()
        tbSelButtonMsg.AppendText("Value changed = " & SelectorButton1.Value & vbCrLf)
        tbSelButtonMsg.ScrollToEnd()
    End Sub

    Private Sub SelectorButton1_Click(sender As Object, e As RoutedEventArgs) Handles SelectorButton1.Click
        If tbSelButtonMsg.LineCount > 200 Then tbSelButtonMsg.Clear()
        tbSelButtonMsg.AppendText("Button pressed" & vbCrLf)
        tbSelButtonMsg.ScrollToEnd()
    End Sub

#End Region

#Region "Small Slider"
    Private Sub SmallSlider1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles SmallSlider1.ValueChanged
        If tbSmallSliderValueChanged.LineCount > 200 Then tbSmallSliderValueChanged.Clear()
        tbSmallSliderValueChanged.AppendText("Value changed = " & SmallSlider1.Value & vbCrLf)
        tbSmallSliderValueChanged.ScrollToEnd()
    End Sub

    Private Sub cbSmallSliderWheelHandled_Click(sender As Object, e As RoutedEventArgs) Handles cbSmallSliderWheelHandled.Click
        If cbSmallSliderWheelHandled.IsChecked = True Then
            SmallSlider1.MouseWheelHandled = True
            tbSmallSliderWheel.Clear()
        Else
            SmallSlider1.MouseWheelHandled = False
        End If
    End Sub

    Private Sub SmallSliderGrid_MouseWheel(sender As Object, e As MouseWheelEventArgs) Handles SmallSliderGrid.MouseWheel
        Dim sld As SmallSlider = TryCast(e.Source, SmallSlider)
        If sld IsNot Nothing Then
            Dim sname As String = sld.Name
            If tbSmallSliderWheel.LineCount > 100 Then tbSmallSliderWheel.Clear()
            If sld.Name = "SmallSlider1" Then
                tbSmallSliderWheel.AppendText("Wheel: " & SmallSlider1.Value & vbCrLf)
                tbSmallSliderWheel.ScrollToEnd()
            End If
        End If
    End Sub

#End Region

#Region "Toggle button"
    Private Sub ToggleButton1_Checked(sender As Object, e As RoutedEventArgs) Handles ToggleButton1.Checked

    End Sub
#End Region

#Region "VU-Bar"

    Private Sub btnVU_Tap_Click(sender As Object, e As RoutedEventArgs) Handles btnVU_Tap.Click

        If VU_RefreshTimer.Enabled = False Then VU_RefreshTimer.Start()
        VU_Bar1.Value = VU_Bar1.MaximumValue
        VU_Bar2.Value = VU_Bar1.Value
        VU_Bar3.Value = VU_Bar1.Value

    End Sub

    Private Sub VU_RefreshTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        'If Me.Dispatcher.CheckAccess Then
        'VU_Refresh()
        'Else
        Me.Dispatcher.Invoke(New ScreenRefresh_Delegate(AddressOf VU_Refresh))
        'End If
    End Sub

    Public Delegate Sub ScreenRefresh_Delegate()
    Private Sub VU_Refresh()

        'decrease value at each refresh                        
        If VU_Bar1.Value < VU_Bar1.MaximumValue / 10 Then
            VU_Bar1.Value = 0
            VU_Bar2.Value = 0
            VU_Bar3.Value = 0
            VU_RefreshTimer.Stop()
        Else
            VU_Bar1.Value = VU_Bar1.Value * VU_decFactor.Value
            VU_Bar2.Value = VU_Bar1.Value
            VU_Bar3.Value = VU_Bar1.Value
        End If

    End Sub

#End Region

#Region "Progress Circle"

#End Region

#Region "Progress Ring"
    Private Sub btnSetProgRingDefaultStartA_Click(sender As Object, e As RoutedEventArgs) Handles btnSetProgRingDefaultStartA.Click
        SldProgressRingStartAngle.Value = 270
    End Sub



#End Region

#Region "Knob"
    Private Sub knob1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles knob1.ValueChanged
        If tbKnobMsg.LineCount > 200 Then tbKnobMsg.Clear()
        tbKnobMsg.AppendText("Value changed = " & knob1.Value & vbCrLf)
        tbKnobMsg.AppendText("Arc Angle = " & knob1.ArcAngle & vbCrLf)
        tbKnobMsg.AppendText("Progr.Val. = " & knob1.ProgressValue & vbCrLf)
        tbKnobMsg.AppendText("---" & vbCrLf)
        tbKnobMsg.ScrollToEnd()
    End Sub

#End Region

#Region "DataGridTextColumnX"

    Private BrushCounter As Integer
    Private DgBackBrushes() As Brush = {Brushes.LightGoldenrodYellow, Brushes.LightGreen, Brushes.LightBlue, Brushes.White}
    Private DgSelBrushes() As Brush = {Brushes.Violet, Brushes.Blue, Brushes.ForestGreen, Brushes.Black}

    Private Sub btnSetDgColOptionsByCode_Click(sender As Object, e As RoutedEventArgs) Handles btnSetDgColOptionsByCode.Click

        Try
            Dim dg As DataGrid = DataGrid1

            For Each col In dg.Columns
                ' only if col is GetType(DataGridTextColumnX)
                Dim colx = TryCast(col, DataGridTextColumnX)
                If colx IsNot Nothing Then
                    colx.Background = DgBackBrushes(BrushCounter)
                    colx.FocusedBackground = Brushes.Yellow
                    colx.FocusedForeground = Brushes.Red
                    colx.SelectedBackground = DgSelBrushes(BrushCounter)
                    colx.SelectedForeground = SystemColors.HighlightTextBrush
                End If
            Next

            BrushCounter += 1
            If BrushCounter >= 4 Then
                BrushCounter = 0
            End If

        Catch
        End Try

    End Sub

#End Region

#Region "MessageWindow"

    Private Sub btnSimpleMessage_Click(sender As Object, e As RoutedEventArgs) Handles btnSimpleMessage.Click
        MessageWindow.Show("This is a simple message", "Message")
    End Sub

    Private Sub btnMessage1_Click(sender As Object, e As RoutedEventArgs) Handles btnMessage1.Click
        MessageWindow.Show(Me, "Here is the Information", "This is a new Message", MessageIcon.Information, Brushes.PaleGoldenrod)
    End Sub

    Private Sub btnMessage2_Click(sender As Object, e As RoutedEventArgs) Handles btnMessage2.Click
        MessageWindow.Show(Me, "Example 2" & vbCrLf & "with Icon, Background and Font Size 14", "New Message", MessageIcon.StatusOk, Brushes.BlanchedAlmond, 14)
    End Sub

#End Region

#Region "QuestionWindow"

    Private Sub btnSimpleQuestion_Click(sender As Object, e As RoutedEventArgs) Handles btnSimpleQuestion.Click
        lblQuestionResult.Content = ""
        Dim result As QuestionWindowResult
        result = QuestionWindow.Show(Me, "Yes or No ?")
        lblQuestionResult.Content = result
    End Sub

    Private Sub btnQuestion1_Click(sender As Object, e As RoutedEventArgs) Handles btnQuestion1.Click
        lblQuestionResult.Content = ""
        Dim result As QuestionWindowResult
        result = QuestionWindow.Show(Me, "Do you want to continue ?", "Closing", QuestionWindowButton.OkCancel)
        lblQuestionResult.Content = result
    End Sub

    Private Sub btnQuestion2_Click(sender As Object, e As RoutedEventArgs) Handles btnQuestion2.Click
        lblQuestionResult.Content = ""
        Dim result As QuestionWindowResult
        result = QuestionWindow.Show(Me, "Do you want to save the changes ?", "Closing",
                                     QuestionWindowButton.YesNoCancel, Brushes.Lavender, 16)

        lblQuestionResult.Content = result
    End Sub


#End Region


End Class
