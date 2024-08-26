Imports System.ComponentModel
Public Class SmallSlider
    Inherits UserControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 75
        'Height = 20
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top
        'Margin = New Thickness(140, 130, 0, 0)

    End Sub

    Private Const DefaultValue As Double = 0
    Private Const DefaultMinValue As Double = 0
    Private Const DefaultMaxValue As Double = 100
    Private Const DefaultStepValue As Double = 1
    Private Const DefaultDecimalPlaces As Integer = 0
    Private Const MaximumDecimalPlaces As Integer = 10

    Public FocusBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H56, &H9D, &HE5))

#Region "Appearance"

    Private Shared ReadOnly DefaultSliderBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HFD, &H40, &H40))

    Public Shared ReadOnly SliderBrushProperty As DependencyProperty = DependencyProperty.Register("SliderBrush", GetType(Brush), GetType(SmallSlider), New UIPropertyMetadata(DefaultSliderBrush))
    ' appears in code
    ''' <summary>
    ''' Brush of the slider rectangle
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property SliderBrush As Brush
        Get
            Return CType(GetValue(SliderBrushProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(SliderBrushProperty, value)
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    Public Shared ReadOnly LeftRightLookProperty As DependencyProperty = DependencyProperty.Register("LeftRightLook", GetType(Boolean), GetType(SmallSlider), New PropertyMetadata(New PropertyChangedCallback(AddressOf OnLeftRightLookChanged)))

    ' appears in code
    ''' <summary>
    ''' TRUE sets the origin of the value bar to the center, as needed for balance/pan audio control.
    ''' </summary>      
    Private Const LRLookdesc = "TRUE sets the origin of the value bar to the center, as needed for balance/pan audio control."
    <Description(LRLookdesc), Category("Small Slider")>
    Public Property LeftRightLook As Boolean
        Get
            Return GetValue(LeftRightLookProperty)
        End Get
        Set(value As Boolean)

        End Set
    End Property

    Private Shared Sub OnLeftRightLookChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SmallSlider = CType(d, SmallSlider)

        If control.LeftRightLook = False Then
            control.ViewboxValue.HorizontalAlignment = HorizontalAlignment.Left
            'control.CenterMark.Visibility = Visibility.Visible
            control.ShowSlider()
        Else
            control.ViewboxValue.HorizontalAlignment = HorizontalAlignment.Center
            'control.CenterMark.Visibility = Visibility.Hidden
            control.ShowSlider()
        End If
    End Sub


#End Region

#Region "Value Region"

    '--- Value Property

    'Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Decimal), GetType(ImageButton), New UIPropertyMetadata("ImageButton"))

    ''' <summary>
    ''' Identifies the Value dependency property.
    ''' </summary>
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(SmallSlider), New FrameworkPropertyMetadata(DefaultValue, New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue)))

    ' appears in code
    ''' <summary>
    ''' Gets or sets the value of Small Slider
    ''' </summary>
    <Description("The current value of Small Slider"), Category("Small Slider")>   ' appears in VS property
    Public Property Value() As Double
        Get
            Return CDbl(GetValue(ValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        Dim minValue As Double = CDbl(d.GetValue(MinimumValueProperty))
        Dim maxValue As Double = CDbl(d.GetValue(MaximumValueProperty))

        If newValue > maxValue Then
            Return maxValue
        ElseIf newValue < minValue Then
            Return minValue
        End If

        Return newValue
    End Function

    Private Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SmallSlider = CType(d, SmallSlider)
        Dim places As Integer = CInt(d.GetValue(DecimalPlacesProperty))

        'control.TextBox1.Text = CStr(CDbl(d.GetValue(ValueProperty)))        
        control.ShowValue()

        Dim e As New RoutedPropertyChangedEventArgs(Of Double)(CDbl(args.OldValue), CDbl(args.NewValue), ValueChangedEvent)
        control.OnValueChanged(e)
    End Sub

    ''' <summary>
    ''' Identifies the ValueChanged routed event.
    ''' </summary>
    Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of Double)), GetType(SmallSlider))

    ' appears in code (event-handler)
    ''' <summary>
    ''' Occurs when the Value property changes
    ''' </summary>
    <Description("Occurs when the Value property changes")>                 ' appears in VS Property sheet (events)
    Public Custom Event ValueChanged As RoutedPropertyChangedEventHandler(Of Double)
        AddHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Double))
            MyBase.AddHandler(ValueChangedEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Double))
            MyBase.RemoveHandler(ValueChangedEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As System.Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
        End RaiseEvent
    End Event

    ''' <summary>
    ''' Raises the ValueChanged event.
    ''' </summary>
    ''' <param name="args">Arguments associated with the ValueChanged event.</param>
    Protected Overridable Sub OnValueChanged(ByVal args As RoutedPropertyChangedEventArgs(Of Double))
        If SkipValueChangedEvent = False Then
            MyBase.RaiseEvent(args)
        End If
    End Sub

    Private SkipValueChangedEvent As Boolean

    ''' <summary>
    ''' Set the value without raising ValueChangedEvent. This can be useful for initializing the control, synchronizing
    '''  the UI with operations in code, and in other special situations.
    ''' </summary>
    ''' <param name="Value"></param>
    Public Sub SetValueSilent(Value As Double)
        SkipValueChangedEvent = True
        SetValue(ValueProperty, Value)
        SkipValueChangedEvent = False
    End Sub

#End Region

#Region "MinimumValue Region"

    '--- MinimumValue Property

    Public Shared ReadOnly MinimumValueProperty As DependencyProperty = DependencyProperty.Register("MinimumValue", GetType(Double), GetType(SmallSlider), New FrameworkPropertyMetadata(DefaultMinValue, New PropertyChangedCallback(AddressOf OnMinimumValueChanged), New CoerceValueCallback(AddressOf CoerceMinimumValue)))
    <Description("Lowest valid value"), Category("Small Slider")>
    Public Property MinimumValue() As Double
        Get
            Return CDbl(GetValue(MinimumValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(MinimumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMinimumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        Dim maxValue As Double = CDbl(d.GetValue(MaximumValueProperty))

        If newValue > maxValue Then
            Return maxValue - 1
        End If

        Return newValue
    End Function

    Private Shared Sub OnMinimumValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As SmallSlider = CType(d, SmallSlider)
    End Sub

#End Region

#Region "MaximumValue Region"
    '--- MaximumValue Property

    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Double), GetType(SmallSlider), New FrameworkPropertyMetadata(DefaultMaxValue, New PropertyChangedCallback(AddressOf OnMaximumValueChanged), New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    <Description("Highest valid value"), Category("Small Slider")>
    Public Property MaximumValue() As Double
        Get
            Return CDbl(GetValue(MaximumValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(MaximumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMaximumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        Dim minValue As Double = CDbl(d.GetValue(MinimumValueProperty))

        If newValue < minValue Then
            Return minValue + 1
        End If

        Return newValue
    End Function

    Private Shared Sub OnMaximumValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As SmallSlider = CType(d, SmallSlider)
    End Sub

#End Region

#Region "StepValue Region"
    '--- StepValue Property

    Public Shared ReadOnly StepValueProperty As DependencyProperty = DependencyProperty.Register("StepValue", GetType(Double), GetType(SmallSlider), New FrameworkPropertyMetadata(DefaultStepValue, New PropertyChangedCallback(AddressOf OnStepValueChanged), New CoerceValueCallback(AddressOf CoerceStepValue)))
    <Description("Step for increment/decrement Value"), Category("Small Slider")>
    Public Property StepValue() As Double
        Get
            Return CDbl(GetValue(StepValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(StepValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceStepValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        Dim maxValue As Double = CDbl(d.GetValue(MaximumValueProperty))

        If newValue = 0 Then
            Return 1                    ' Step cannot be 0
        ElseIf newValue > maxValue Then
            Return maxValue
        End If

        Return newValue
    End Function

    Private Shared Sub OnStepValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As SmallSlider = CType(d, SmallSlider)
    End Sub

#End Region

#Region "DecimalPlaces Region"
    '--- StepValue Property

    Public Shared ReadOnly DecimalPlacesProperty As DependencyProperty = DependencyProperty.Register("DecimalPlaces", GetType(Integer), GetType(SmallSlider), New FrameworkPropertyMetadata(DefaultDecimalPlaces, New PropertyChangedCallback(AddressOf OnDecimalPlacesChanged), New CoerceValueCallback(AddressOf CoerceDecimalPlaces)))
    <Description("Decimal Places for Value"), Category("Small Slider")>
    Public Property DecimalPlaces() As Integer
        Get
            Return CInt(GetValue(DecimalPlacesProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(DecimalPlacesProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceDecimalPlaces(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Integer = CInt(value)

        If newValue < 0 Then
            Return 0
        ElseIf newValue > MaximumDecimalPlaces Then
            Return MaximumDecimalPlaces
        End If

        Return newValue
    End Function

    Private Shared Sub OnDecimalPlacesChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As SmallSlider = CType(d, SmallSlider)
    End Sub

#End Region

#Region "DisplayedValueOffset Region"
    '--- DsiplayedValueOffset Property

    Public Shared ReadOnly DisplayedValueOfsetProperty As DependencyProperty = DependencyProperty.Register("DisplayedValueOffset", GetType(Double), GetType(SmallSlider), New FrameworkPropertyMetadata(0.0, New PropertyChangedCallback(AddressOf OnDisplayedValueOffsetChanged), New CoerceValueCallback(AddressOf CoerceDisplayedValueOffset)))
    <Description("Offset added to value for TextBox content. Can also be negative"), Category("Small Slider")>
    Public Property DisplayedValueOffset() As Double
        Get
            Return CDbl(GetValue(DisplayedValueOfsetProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(DisplayedValueOfsetProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceDisplayedValueOffset(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        'Dim minValue As Double = CDbl(d.GetValue(MinimumValueProperty))

        Return newValue
    End Function

    Private Shared Sub OnDisplayedValueOffsetChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As SmallSlider = CType(d, SmallSlider)
    End Sub

#End Region

#Region "Control Region"
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        ' make sure value is shown at startup (if = default value)        
        ' ensure that value is shown at startup with the desired trailing zeros
        ShowValue()
    End Sub
    Private Sub UserControl_MouseMove(sender As Object, e As MouseEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then

            If e.LeftButton = MouseButtonState.Pressed Then
                Dim curpos As Point
                curpos = e.GetPosition(Me)              ' get current (rel) position

                Dim gwidth As Double = SliderRectGrid.RenderSize.Width
                Dim valueRange As Double = MaximumValue - MinimumValue
                Dim newValue As Double

                'newValue = gwidth / valueRange * curpos.X, DecimalPlaces
                newValue = (valueRange / gwidth * curpos.X) + MinimumValue

                Value = Math.Round(newValue, DecimalPlaces)
            End If

        End If
    End Sub

    Private Sub UserControl_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)

        If HasEffectiveKeyboardFocus = False Then
            Me.Focus()
        Else
            Dim el As UIElement = CType(sender, UIElement)
            el.CaptureMouse()

            Dim curpos As Point
            curpos = e.GetPosition(Me)              ' get current (rel) position

            Dim gwidth As Double = SliderRectGrid.RenderSize.Width
            Dim valueRange As Double = MaximumValue - MinimumValue
            Dim newValue As Double

            'newValue = gwidth / valueRange * curpos.X, DecimalPlaces
            newValue = (valueRange / gwidth * curpos.X) + MinimumValue

            Value = Math.Round(newValue, DecimalPlaces)

            ' capture will raise MouseMove, set initial values before
            ' capture now

        End If

    End Sub

    Private Sub UserControl_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then
            el.ReleaseMouseCapture()
        End If
    End Sub

    Private Sub UserControl_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        ' The amount of the delta value is currently not taken into account
        ' Increase/decrease value
        '   --> see also MouseWheelDeltaForOneLine (= 120)

        If e.Delta > 0 Then
            Value += StepValue
        End If

        ' If the mouse wheel delta is negative, move the box down.
        If e.Delta < 0 Then
            Value -= StepValue
        End If

    End Sub

    Private Sub UserControl_GotFocus(sender As Object, e As RoutedEventArgs)
        FocusRect.Stroke = FocusBrush
    End Sub

    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        FocusRect.Stroke = Brushes.Transparent

        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then
            el.ReleaseMouseCapture()
        End If
    End Sub

    Private Sub ShowValue()
        Dim str As String = CStr(Value + DisplayedValueOffset)

        ' append zeros if necessary
        If DecimalPlaces > 0 Then               ' only if decimal places
            str = Value.ToString("F" & DecimalPlaces.ToString)
        End If

        TextBox1.Text = str

        ShowSlider()
    End Sub

    Private Sub ShowSlider()
        Dim swidth As Double
        Dim gwidth As Double = SliderRectGrid.RenderSize.Width
        Dim valueRange As Double = MaximumValue - MinimumValue

        If LeftRightLook = False Then
            SliderRect.Visibility = Visibility.Visible
            CenterMark.Visibility = Visibility.Visible
            SliderRectLeft.Visibility = Visibility.Collapsed
            SliderRectRight.Visibility = Visibility.Collapsed

            If valueRange > 0 Then
                swidth = gwidth * (Value - MinimumValue) / valueRange
                SliderRect.Width = swidth
            End If

        Else
            Dim Lwidth As Double
            Dim Rwidth As Double

            SliderRect.Visibility = Visibility.Collapsed
            CenterMark.Visibility = Visibility.Collapsed
            SliderRectLeft.Visibility = Visibility.Visible
            SliderRectRight.Visibility = Visibility.Visible

            If valueRange > 0 Then

                ' old - right part has incorrect width when Minimum is negative
                'If Value < valueRange / 2 Then
                '    Lwidth = gwidth * (valueRange / 2 - Value) / valueRange
                'ElseIf Value > valueRange / 2 Then
                '    Rwidth = gwidth * (Value - valueRange / 2) / valueRange
                'End If

                If Value < (valueRange / 2 + MinimumValue) Then
                    Lwidth = gwidth * (valueRange / 2 - Value + MinimumValue) / valueRange
                ElseIf Value > (valueRange / 2 + MinimumValue) Then
                    Rwidth = gwidth * (Value - valueRange / 2 - MinimumValue) / valueRange
                End If

                SliderRectLeft.Width = Lwidth
                SliderRectRight.Width = Rwidth
            End If
        End If

    End Sub

    Private Sub UserControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)

        Select Case e.Key
            Case Key.Up
                Value += StepValue
                e.Handled = True
            Case Key.Right
                Value += StepValue
                e.Handled = True
            Case Key.Down
                Value -= StepValue
                e.Handled = True
            Case Key.Left
                Value -= StepValue
                e.Handled = True
            Case Key.Home
                Dim halfValue As Double
                halfValue = Math.Round(((MaximumValue - MinimumValue) / 2) + MinimumValue, DecimalPlaces)
                If Value < halfValue Then
                    Value = halfValue
                Else
                    Value = MaximumValue
                End If
                e.Handled = True
            Case Key.End
                Dim halfValue As Double
                halfValue = Math.Round(((MaximumValue - MinimumValue) / 2) + MinimumValue, DecimalPlaces)
                If Value > halfValue Then
                    Value = halfValue
                Else
                    Value = MinimumValue
                End If
                e.Handled = True
        End Select

    End Sub

    Private Sub UserControl_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Focus()
    End Sub



#End Region

End Class
