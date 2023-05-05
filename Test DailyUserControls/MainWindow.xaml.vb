Class MainWindow

    Private Shared VU_RefreshTimer As New Timers.Timer(50)       ' 50 ms Screen Timer (= 20 FPS)

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim appver As String = My.Application.Info.Version.ToString(3)
        Dim libname As Reflection.AssemblyName = GetType(DailyUserControls.ImageButton).Assembly.GetName()
        Dim libver As String = libname.Version.ToString(3)
        Me.Title = Title & " V " & appver & " - Library V " & libver

        AddHandler VU_RefreshTimer.Elapsed, AddressOf VU_RefreshTimer_Tick

        tiImageButton.Focus()

    End Sub

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
    Private Sub nud1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles nud1.ValueChanged
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText("Value changed = " & nud1.Value & vbCrLf)
        tbNudMsg.ScrollToEnd()
        'e.Handled = True
    End Sub

    Private Sub nud2_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles nud2.ValueChanged
        If tbNudMsg.LineCount > 200 Then tbNudMsg.Clear()
        tbNudMsg.AppendText(e.NewValue & vbCrLf)
        tbNudMsg.ScrollToEnd()
    End Sub

    Private Sub nud3_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles nud3.ValueChanged
        tbNudMsg.AppendText("nud3 value: " & e.NewValue & vbCrLf)
        tbNudMsg.ScrollToEnd()
    End Sub

    Private Sub btnNudSilent_Click(sender As Object, e As RoutedEventArgs) Handles btnNudSilent.Click
        nud1.SetValueSilent(5.5)
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

#End Region

#Region "Toggle button"

#End Region

#Region "VU-Bar"

    Private Sub btnVU_Tap_Click(sender As Object, e As RoutedEventArgs) Handles btnVU_Tap.Click

        If VU_RefreshTimer.Enabled = False Then VU_RefreshTimer.Start()
        VU_Bar1.Value = VU_Bar1.MaximumValue

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
            VU_RefreshTimer.Stop()
        Else
            VU_Bar1.Value = VU_Bar1.Value * VU_decFactor.Value
        End If

    End Sub

#End Region

#Region "Progress Circle"

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


End Class
