Imports System.ComponentModel
Imports System.Runtime.InteropServices      ' for setCursorPos

Public Class Knob

    Inherits UserControl

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 60
        'Height = 60
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top
        'Margin = New Thickness(140, 180, 0, 0)

        Value = 20

    End Sub

    ' Constants
    Private Const DefaultValue As Double = 0
    Private Const DefaultMinValue As Double = 0
    Private Const DefaultMaxValue As Double = 100
    Private Const DefaultStepValue As Double = 1
    Private Const DefaultDecimalPlaces As Integer = 0
    Private Const MaximumDecimalPlaces As Integer = 10
    Private Const UsableRotationAngle As Integer = 300
    Private Const UsableRotationFactor As Integer = UsableRotationAngle \ 100

    Public FocusBrush As New SolidColorBrush(Color.FromArgb(255, 86, 157, 229))

    ' for MouseCapture
    Public Shared MouseBasePosition As Point                ' relative to current control
    Public Shared MouseCurrentPosition As Point             ' relative to current control

    Private Const SenseWayY = 120
    Private Const MidZoneWidth = 40                 ' vertical zone for full-range value change
    Private Const MinMaxZoneHeight = 25

    'Private Shared ValueAtMouseBasePosition As Double

    ' Needed for Mouse relocation at MouseDown / MouseUp (no SetCursorPos-function in WPF available)
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetCursorPos(ByVal X As Integer, ByVal Y As Integer) As Boolean
    End Function

    Private UpdatingValue As Boolean
    Private UpdatingArcAngle As Boolean
    Private UpdatingProgressValue As Boolean



#Region "Hide some Properties"
    ' hide unused Properties of UserControl in designer
    <Browsable(False)>
    Public Overloads Property Foreground As Brush
    <Browsable(False)>
    Public Overloads Property Background As Brush
    <Browsable(False)>
    Public Overloads Property BorderBrush As Brush
#End Region

#Region "Appearance"

    Private Shared ReadOnly DefaultBackFillBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HF0, &HF0, &HF0))

    Private Shared ReadOnly FillPropertyMetadata As New FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly FillProperty As DependencyProperty = DependencyProperty.Register("Fill", GetType(Brush), GetType(Knob), FillPropertyMetadata)

    '<Description("Ellipse Fill"), Category("Knob")>   ' appears in VS property
    Public Property Fill As Brush
        Get
            Return CType((GetValue(FillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokePropertyMetadata As New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeProperty As DependencyProperty = DependencyProperty.Register("Stroke", GetType(Brush), GetType(Knob), StrokePropertyMetadata)

    '<Description("Ellipse Fill"), Category("Knob")>   ' appears in VS property
    Public Property Stroke As Brush
        Get
            Return CType((GetValue(StrokeProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokeThicknessPropertyMetadata As New FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("StrokeThickness", GetType(Double), GetType(Knob), StrokeThicknessPropertyMetadata)

    <Description("Ellipse Stroke"), Category("Knob")>   ' appears in VS property
    Public Property StrokeThickness As Double
        Get
            Return CDbl(GetValue(StrokeThicknessProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property

    Private Shared ReadOnly BackFillPropertyMetadata As New FrameworkPropertyMetadata(DefaultBackFillBrush, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly BackFillProperty As DependencyProperty = DependencyProperty.Register("BackFill", GetType(Brush), GetType(Knob), BackFillPropertyMetadata)

    '<Description("Ellipse Fill"), Category("Knob")>   ' appears in VS property
    Public Property BackFill As Brush
        Get
            Return CType((GetValue(BackFillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackFillProperty, value)
        End Set
    End Property

#End Region

#Region "Value Region"

    '--- Value Property
    ' changing this value affects render (calls OnRender), set at Metadata Option 'AffectsRender' below

    ' define on 1 line:
    'Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(Knob), New FrameworkPropertyMetadata(DefaultValue, New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue)))

    ' define on 2 lines:
    Private Shared ReadOnly ValuePropertyMetadata As New FrameworkPropertyMetadata(DefaultValue, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue))
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(Knob), ValuePropertyMetadata)

    ' appears in code
    ''' <summary>
    ''' Gets or sets the value of Knob
    ''' </summary>
    <Description("The current value of Knob"), Category("Knob")>   ' appears in VS property
    Public Property Value() As Double
        Get
            Return CDbl(GetValue(ValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(ValueProperty, value)
        End Set

        'It is recommended that the accessors of dependency properties not contain additional logic 
        'because clients And WPF can bypass the accessors And Call GetValue And SetValue directly.       
    End Property

    Private Overloads Shared Function CoerceValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)
        Dim minValue As Double = CDbl(d.GetValue(MinimumValueProperty))
        Dim maxValue As Double = CDbl(d.GetValue(MaximumValueProperty))

        Dim control As Knob = CType(d, Knob)
        newValue = control.RoundToStep(newValue)

        If newValue > maxValue Then
            newValue = maxValue
        ElseIf newValue < minValue Then
            newValue = minValue
        End If

        Return newValue
    End Function

    Private Function RoundToStep(value As Double) As Double
        ' round to step-value, if necessary
        Dim steps As Double = Fix(value / StepValue)        ' number of steps
        Dim smod As Double = Math.Abs(value Mod StepValue)  ' get the remainder

        If smod >= StepValue / 2 Then           ' rounding needed ?
            If value > 0 Then                   ' is value negative ?
                steps += 1
            Else
                steps -= 1
            End If
        End If

        Return steps * StepValue               ' re-calculate the value
    End Function

    Private Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As Knob = CType(d, Knob)
        control.UpdatingValue = True

        Dim places As Integer = CInt(d.GetValue(DecimalPlacesProperty))
        Dim arcValue As Single

        Dim min As Double = CDbl(d.GetValue(MinimumValueProperty))
        Dim max As Double = CDbl(d.GetValue(MaximumValueProperty))
        Dim value As Double = CDbl(d.GetValue(ValueProperty))
        Dim diff As Double
        diff = Math.Abs(min - value)

        If control.UpdatingArcAngle = False Then
            arcValue = CSng(100 * UsableRotationFactor / (max - min) * diff)
            d.SetCurrentValue(ArcAngleProperty, arcValue)
        End If

        If control.UpdatingProgressValue = False Then
            Dim progressValue As Single
            progressValue = CSng((CSng(d.GetValue(ArcAngleProperty)) / UsableRotationFactor))
            d.SetCurrentValue(ProgressValueProperty, progressValue)
        End If

        control.ShowValue()             ' show value as text in the middle of the control

        Dim e As New RoutedPropertyChangedEventArgs(Of Double)(CDbl(args.OldValue), CDbl(args.NewValue), ValueChangedEvent)
        control.OnValueChanged(e)

        control.UpdatingValue = False
    End Sub

    ''' <summary>
    ''' Identifies the ValueChanged routed event.
    ''' </summary>
    Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of Double)), GetType(Knob))

    ' appears in code (event-handler)
    ''' <summary>
    ''' Occurs when the Value property changes
    ''' </summary>
    <Description("Occurs when the Value property changes")>         ' appears in VS Property panel (events)
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
        MyBase.RaiseEvent(args)
    End Sub

#End Region

#Region "Angle and ProgressValue"
    'Private Const StartAngle As Single = 270.0          ' from 3 o'clock, clockwise (270 = 12 o'clock)


    Private Shared ReadOnly StartAnglePropertyMetadata As New FrameworkPropertyMetadata(120.0F, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnStartAngleChanged), New CoerceValueCallback(AddressOf CoerceStartAngle))
    Public Shared ReadOnly StartAngleProperty As DependencyProperty = DependencyProperty.Register("StartAngle", GetType(Single), GetType(Knob), StartAnglePropertyMetadata)
    <Description("Start of the Arc, relative to three o'clock, clockwise"), Category("Knob")>   ' appears in VS property
    Public Property StartAngle As Single
        Get
            Return CSng(GetValue(StartAngleProperty))
        End Get
        Set(ByVal value As Single)
            SetValue(StartAngleProperty, value)
        End Set
    End Property
    Private Shared Function CoerceStartAngle(ByVal depObj As DependencyObject, ByVal baseVal As Object) As Object
        Dim angle As Single = CSng(baseVal)
        If angle > 360 Then
            angle = 360
        ElseIf angle < 0.0 Then
            angle = 0.0
        End If
        Return angle
    End Function
    Private Shared Sub OnStartAngleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        'Dim value As Single

        'value = CSng((CSng(d.GetValue(ArcAngleProperty)) / 3.6))
        'd.SetCurrentValue(ProgressValueProperty, value)

    End Sub

    'Private Shared ReadOnly ArcAnglePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnArcAngleChanged), New CoerceValueCallback(AddressOf CoerceArcAngle))
    Private Shared ReadOnly ArcAnglePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.None, New PropertyChangedCallback(AddressOf OnArcAngleChanged), New CoerceValueCallback(AddressOf CoerceArcAngle))
    Public Shared ReadOnly ArcAngleProperty As DependencyProperty = DependencyProperty.Register("ArcAngle", GetType(Single), GetType(Knob), ArcAnglePropertyMetadata)
    <Description("The current Angle for Arc"), Category("Knob")>   ' appears in VS property
    Public Property ArcAngle As Single
        Get
            Return CSng(GetValue(ArcAngleProperty))
        End Get
        Set(ByVal value As Single)
            SetValue(ArcAngleProperty, value)
        End Set
    End Property

    Private Shared Function CoerceArcAngle(ByVal depObj As DependencyObject, ByVal baseVal As Object) As Object
        Dim angle As Single = CSng(baseVal)
        If angle > 360 Then
            angle = 360
        ElseIf angle < 0.0 Then
            angle = 0.0
        End If
        Return angle
    End Function
    Private Shared Sub OnArcAngleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As Knob = CType(d, Knob)
        control.UpdatingArcAngle = True

        If control.UpdatingValue = False Then
            Dim min As Double = CDbl(d.GetValue(MinimumValueProperty))
            Dim max As Double = CDbl(d.GetValue(MaximumValueProperty))
            Dim prog As Single = CSng(d.GetValue(ProgressValueProperty))
            Dim value As Double
            value = min + (max - min) / 100 * CSng(d.GetValue(ProgressValueProperty))
            d.SetCurrentValue(ValueProperty, value)
        End If

        If control.UpdatingProgressValue = False Then
            Dim progressValue As Single
            progressValue = CSng((CSng(d.GetValue(ArcAngleProperty)) / UsableRotationFactor))
            d.SetCurrentValue(ProgressValueProperty, progressValue)
        End If

        control.UpdatingArcAngle = False
    End Sub

    Private Shared ReadOnly ProgressValuePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.None, New PropertyChangedCallback(AddressOf OnProgressValueChanged), New CoerceValueCallback(AddressOf CoerceProgressValue))
    Public Shared ProgressValueProperty As DependencyProperty = DependencyProperty.Register("ProgressValue", GetType(Single), GetType(Knob), ProgressValuePropertyMetadata)

    <Description("The current Progress Value"), Category("Knob")>   ' appears in VS property
    Public Property ProgressValue As Single
        Get
            Return CSng(GetValue(ProgressValueProperty))
        End Get
        Set(value As Single)
            SetValue(ProgressValueProperty, value)
        End Set
    End Property
    Private Shared Function CoerceProgressValue(ByVal depObj As DependencyObject, ByVal baseVal As Object) As Object
        Dim value As Single = CSng(baseVal)
        If value > 100 Then
            value = 100
        ElseIf value < 0.0 Then
            value = 0.0
        End If
        Return value
    End Function
    Private Shared Sub OnProgressValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As Knob = CType(d, Knob)
        control.UpdatingProgressValue = True

        If control.UpdatingValue = False Then
            Dim min As Double = CDbl(d.GetValue(MinimumValueProperty))
            Dim max As Double = CDbl(d.GetValue(MaximumValueProperty))
            Dim prog As Single = CSng(d.GetValue(ProgressValueProperty))
            Dim value As Double
            value = min + (max - min) / 100 * CSng(d.GetValue(ProgressValueProperty))
            d.SetCurrentValue(ValueProperty, value)
        End If

        If control.UpdatingArcAngle = False Then
            Dim arcValue As Single
            arcValue = CSng(d.GetValue(ProgressValueProperty)) * UsableRotationFactor
            d.SetCurrentValue(ArcAngleProperty, arcValue)
        End If

        control.UpdatingProgressValue = False
    End Sub




#End Region

#Region "MinimumValue Region"

    '--- MinimumValue Property

    Public Shared ReadOnly MinimumValueProperty As DependencyProperty = DependencyProperty.Register("MinimumValue", GetType(Double), GetType(Knob), New FrameworkPropertyMetadata(DefaultMinValue, New PropertyChangedCallback(AddressOf OnMinimumValueChanged), New CoerceValueCallback(AddressOf CoerceMinimumValue)))
    <Description("Lowest valid value"), Category("Knob")>
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
        'Dim control As Knob = CType(d, Knob)
    End Sub

#End Region

#Region "MaximumValue Region"
    '--- MaximumValue Property

    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Double), GetType(Knob), New FrameworkPropertyMetadata(DefaultMaxValue, New PropertyChangedCallback(AddressOf OnMaximumValueChanged), New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    <Description("Highest valid value"), Category("Knob")>
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
        'Dim control As Knob = CType(d, Knob)
    End Sub

#End Region

#Region "StepValue Region"
    '--- StepValue Property

    Public Shared ReadOnly StepValueProperty As DependencyProperty = DependencyProperty.Register("StepValue", GetType(Double), GetType(Knob), New FrameworkPropertyMetadata(DefaultStepValue, New PropertyChangedCallback(AddressOf OnStepValueChanged), New CoerceValueCallback(AddressOf CoerceStepValue)))
    <Description("Step for increment/decrement Value"), Category("Knob")>
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
            Return 1                    ' Schritt darf nicht 0 sein
        ElseIf newValue > maxValue Then
            Return maxValue
        End If

        Return newValue
    End Function

    Private Shared Sub OnStepValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As Knob = CType(d, Knob)
    End Sub

#End Region

#Region "DecimalPlaces Region"
    '--- StepValue Property

    Public Shared ReadOnly DecimalPlacesProperty As DependencyProperty = DependencyProperty.Register("DecimalPlaces", GetType(Integer), GetType(Knob), New FrameworkPropertyMetadata(DefaultDecimalPlaces, New PropertyChangedCallback(AddressOf OnDecimalPlacesChanged), New CoerceValueCallback(AddressOf CoerceDecimalPlaces)))
    <Description("Decimal Places for Value"), Category("Knob")>
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
        'Dim control As Knob = CType(d, Knob)
    End Sub

#End Region

#Region "Control"
    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        ' make sure value is shown at startup (if=default value        
        ' ensure that value is shown at startup with the trailing zeros you want
        ShowValue()
    End Sub

    Private Sub UserControl_GotFocus(sender As Object, e As RoutedEventArgs)
        Rectangle1.Stroke = FocusBrush
    End Sub

    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        Rectangle1.Stroke = Brushes.Transparent

        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then
            el.ReleaseMouseCapture()
            Cursor = Cursors.Arrow
            RemoveAdorner()
        End If

    End Sub
    Private Sub UserControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)

        Select Case e.Key
            Case Key.Up
                Value += StepValue
                e.Handled = True
            Case Key.Down
                Value -= StepValue
                e.Handled = True
            Case Key.PageUp
                Value += 5 * StepValue
                e.Handled = True
            Case Key.PageDown
                Value -= 5 * StepValue
                e.Handled = True
            Case Key.Home
                Value = MaximumValue
                e.Handled = True
            Case Key.End
                Value = MinimumValue
                e.Handled = True
        End Select

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
    Private Sub ShowValue()
        Dim str As String

        ' append zeros if necessary
        If DecimalPlaces > 0 Then               ' only if decimal places
            str = Value.ToString("F" & DecimalPlaces.ToString)
        Else
            str = CStr(Math.Round(Value, 0))
        End If

        TextBox1.Text = str
    End Sub

    Private Sub UserControl_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        If Me.IsFocused = False Then
            Me.Focus()
            'Exit Sub
        End If


        ' capture will raise MouseMove, set initial values before
        'ValueAtMouseBasePosition = Value
        'ValueAtMouseBasePosition = MinimumValue + (MaximumValue - MinimumValue) / 2     'also for neg. values

        Dim curpos As Point
        curpos = e.GetPosition(Me)              ' get current (rel) position
        MouseBasePosition.X = Me.Width / 2                  ' set to mid x of control
        MouseBasePosition.Y = Me.Height / 2                 ' xxx set to mid y of control

        MouseCurrentPosition = MouseBasePosition

        Dim valueRange As Double = MaximumValue - MinimumValue
        Dim yoffs As Double = SenseWayY / valueRange * (Value - MinimumValue)

        MouseCurrentPosition.Y = Me.Height / 2 + SenseWayY / 2 - yoffs



        ' adjust cursorposition
        Dim pt As Point
        'pt = Me.PointToScreen(e.GetPosition(Me))
        pt = Me.PointToScreen(MouseCurrentPosition)
        SetCursorPos(CInt(pt.X), CInt(pt.Y))

        ' capture now
        Dim el As UIElement = CType(sender, UIElement)
        el.CaptureMouse()
        Cursor = Cursors.ScrollNS

        'ShowMouseDelta()
        ShowAdorner()


    End Sub

    Private Sub UserControl_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim el As UIElement = CType(sender, UIElement)

        If el.IsMouseCaptured Then
            el.ReleaseMouseCapture()
            Cursor = Cursors.Arrow
            RemoveAdorner()

            Dim pt As Point
            pt = Me.PointToScreen(New Point(Me.Width / 2, Me.Height / 2))
            pt.X += MidZoneWidth / 4
            SetCursorPos(CInt(pt.X), CInt(pt.Y))
        End If
    End Sub

    Private Sub UserControl_MouseMove(sender As Object, e As MouseEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then

            MouseCurrentPosition = e.GetPosition(Me)

            ChangeValue()

            Dim myAdornerLayer As AdornerLayer
            myAdornerLayer = AdornerLayer.GetAdornerLayer(Me)
            myAdornerLayer.Update()
        End If
    End Sub

    Private Sub ChangeValue()

        Dim rect As Rect

        '--- maximum rectangle (at top of sense-pad)
        rect.X = MouseBasePosition.X - MidZoneWidth / 2
        rect.Y = MouseBasePosition.Y - SenseWayY / 2 - MinMaxZoneHeight
        rect.Width = MidZoneWidth
        rect.Height = MinMaxZoneHeight

        If rect.Contains(MouseCurrentPosition) Then
            Value = MaximumValue
            Exit Sub
        End If

        '--- minimum rectangle (at bottom of sense-pad)
        rect.X = MouseBasePosition.X - MidZoneWidth / 2
        rect.Y = MouseBasePosition.Y + SenseWayY / 2
        rect.Width = MidZoneWidth
        rect.Height = MinMaxZoneHeight

        If rect.Contains(MouseCurrentPosition) Then
            Value = MinimumValue
            Exit Sub
        End If

        '--- sense rectangle
        rect.X = MouseBasePosition.X - MidZoneWidth / 2
        rect.Y = MouseBasePosition.Y - SenseWayY / 2
        rect.Width = MidZoneWidth
        rect.Height = SenseWayY

        If rect.Contains(MouseCurrentPosition) Then
            ' inside sense pad, calculate value
            Dim ValueDeltaRange As Double = MaximumValue - MinimumValue
            Dim deltaY As Double
            deltaY = MouseBasePosition.Y + SenseWayY / 2 - MouseCurrentPosition.Y

            Value = MinimumValue + (ValueDeltaRange / SenseWayY * deltaY)       ' allow MinVal's other than 0
            Exit Sub
        Else
            'outside sense-pad and min/max pads -> arc

            Dim deltaX As Double
            Dim deltaY As Double
            ' set 0 degree to six o'clock
            deltaX = MouseBasePosition.X - MouseCurrentPosition.X
            'deltaX = MouseCurrentPosition.X - MouseBasePosition.X
            'deltaY = MouseBasePosition.Y - MouseCurrentPosition.Y
            deltaY = MouseCurrentPosition.Y - MouseBasePosition.Y
            Dim degree As Double
            degree = Math.Atan2(deltaX, deltaY) * 180 / Math.PI

            If degree < 0 Then
                degree = 360 + degree
            End If

            If degree > 330 Then degree = 330

            'Dim str As String
            'Str = "dx: " & deltaX & "  dy: " & deltaY & "  deg: " & degree
            'Console.WriteLine(str)

            'ArcAngle = CSng(degree - 30)           ' breaks the data binding to the slider
            Value = MinimumValue + (MaximumValue - MinimumValue) / 300 * CSng(degree - 30)

            'Multiply the return value by 180 / Math.PI to convert from radians to degrees.

            Exit Sub
        End If

    End Sub



    Private Sub ShowAdorner()

        Dim myAdornerLayer As AdornerLayer
        myAdornerLayer = AdornerLayer.GetAdornerLayer(Me)

        Dim AdornerArray() As Adorner = myAdornerLayer.GetAdorners(Me)

        'If AdornerArray Is Nothing Then
        myAdornerLayer = AdornerLayer.GetAdornerLayer(Me)
        myAdornerLayer.Add(New CaptureAdorner(Me))
        'End If

    End Sub

    Private Sub RemoveAdorner()

        Dim myAdornerLayer As AdornerLayer
        myAdornerLayer = AdornerLayer.GetAdornerLayer(Me)

        Dim toRemoveArray() As Adorner = myAdornerLayer.GetAdorners(Me)

        If toRemoveArray IsNot Nothing Then
            For i = 1 To toRemoveArray.Length
                myAdornerLayer.Remove(toRemoveArray(i - 1))
            Next
        End If

    End Sub

    Public Class CaptureAdorner
        Inherits Adorner
        Sub New(ByVal adornedElement As UIElement)
            MyBase.New(adornedElement)
        End Sub

        Protected Overrides Sub OnRender(ByVal drawingContext As System.Windows.Media.DrawingContext)
            MyBase.OnRender(drawingContext)

            Dim adornedElementRect As New Rect(AdornedElement.RenderSize)
            Dim renderBrush As New SolidColorBrush(Colors.Green)
            renderBrush.Opacity = 0.05
            Dim renderPen As New Pen(New SolidColorBrush(Colors.Navy), 1.5)

            Dim pen As New Pen(Brushes.Black, 1)
            pen.DashStyle = DashStyles.Dot

            'drawingContext.DrawRectangle(Nothing, pen, adornedElementRect)

            Dim rect As Rect

            rect.X = MouseBasePosition.X - MidZoneWidth / 2
            rect.Y = MouseBasePosition.Y - SenseWayY / 2

            rect.Width = MidZoneWidth
            rect.Height = SenseWayY
            'drawingContext.DrawRectangle(renderBrush, pen, rect)        ' sense rectangle

            rect.Y -= MinMaxZoneHeight
            rect.Height = MinMaxZoneHeight
            'drawingContext.DrawRectangle(renderBrush, pen, rect)        ' max rectangle

            rect.Y = MouseBasePosition.Y + SenseWayY / 2
            'drawingContext.DrawRectangle(renderBrush, pen, rect)        ' min rectangle



            rect.X = MouseBasePosition.X - MidZoneWidth / 2
            rect.Y = MouseBasePosition.Y - SenseWayY / 2 - MinMaxZoneHeight

            rect.Width = MidZoneWidth
            rect.Height = SenseWayY + MinMaxZoneHeight * 2
            drawingContext.DrawRectangle(renderBrush, pen, rect)        ' sense rectangle

            Dim p1 As Point
            Dim p2 As Point

            p1.X = MouseBasePosition.X - MidZoneWidth / 2
            p1.Y = MouseBasePosition.Y - SenseWayY / 2

            p2.X = p1.X + MidZoneWidth
            p2.Y = p1.Y

            drawingContext.DrawLine(pen, p1, p2)                        ' upper separator

            p1.Y += SenseWayY
            p2.Y += SenseWayY
            drawingContext.DrawLine(pen, p1, p2)                        ' lower separator

            '--- line

            'Dim relP1 As Point = MouseBasePosition
            'Dim relP2 As Point = MouseCurrentPosition

            'Dim LinePen As New Pen(New SolidColorBrush(Colors.Green), 1)
            'drawingContext.DrawLine(LinePen, relP1, relP2)

        End Sub
    End Class


#End Region

#Region "Render"
    '           ¦
    '           ¦   H    / ¦
    '           ¦     /    ¦ opp (y)
    '           ¦  /       ¦  
    '  .........¦<.........¦ 
    '           ¦   adjac (x)
    '           ¦
    '           ¦   adjacent = H * cos a
    '           ¦   opp = H * sin a
    '           ¦    
    '   
    '   H = Radius
    '   Example: x =  H * cos(a) 
    '            y =  H * sin(a)
    '
    '            to radians: * Pi / 180

    Protected Overrides Sub OnRender(ByVal dc As DrawingContext)

        If BackFill IsNot Nothing Then
            Dim myEllipseGeometry As New EllipseGeometry()
            myEllipseGeometry.Center = New Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0)
            myEllipseGeometry.RadiusX = (RenderSize.Width - StrokeThickness) / 2.0
            myEllipseGeometry.RadiusY = (RenderSize.Height - StrokeThickness) / 2.0

            dc.DrawGeometry(BackFill, New Pen(), myEllipseGeometry)
        End If

        Dim maxWidth = RenderSize.Width - StrokeThickness - (Rectangle1.StrokeThickness * 2)
        Dim maxHeight = RenderSize.Height - StrokeThickness - (Rectangle1.StrokeThickness * 2)

        Dim xStart = maxWidth / 2.0 * Math.Cos((StartAngle) * Math.PI / 180.0)
        Dim yStart = maxHeight / 2.0 * Math.Sin((StartAngle) * Math.PI / 180.0)

        Dim xEnd = maxWidth / 2.0 * Math.Cos((StartAngle + ArcAngle) * Math.PI / 180.0)
        Dim yEnd = maxHeight / 2.0 * Math.Sin((StartAngle + ArcAngle) * Math.PI / 180.0)

        'Dim geom = New StreamGeometry

        'If ArcAngle = 0 Then Return geom   ' suppress start-line

        If ArcAngle < 360 Then
            Dim geom = New StreamGeometry

            Using ctx = geom.Open
                'ctx.BeginFigure(New Point(RenderSize.Width / 2.0 - xStart, RenderSize.Height / 2.0 + yStart), True, True)
                ctx.BeginFigure(New Point(RenderSize.Width / 2.0 + xStart, RenderSize.Height / 2.0 + yStart), True, True)
                ctx.ArcTo(New Point(RenderSize.Width / 2.0 + xEnd, RenderSize.Height / 2.0 + yEnd), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, ArcAngle > 180, SweepDirection.Clockwise, True, False)
                ctx.LineTo(New Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0), True, False)
            End Using

            dc.DrawGeometry(Fill, New Pen(Stroke, StrokeThickness), geom)
        Else
            Dim myEllipseGeometry As New EllipseGeometry()
            myEllipseGeometry.Center = New Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0)
            myEllipseGeometry.RadiusX = (RenderSize.Width - StrokeThickness) / 2.0
            myEllipseGeometry.RadiusY = (RenderSize.Height - StrokeThickness) / 2.0

            dc.DrawGeometry(Fill, New Pen(Stroke, StrokeThickness), myEllipseGeometry)
        End If


        Dim center As New Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0)

        'dc.DrawLine(New Pen(Stroke, StrokeThickness), center, New Point(RenderSize.Width / 2.0 + LxEnd, RenderSize.Height / 2.0 + LyEnd))

        'dc.DrawGeometry(Fill, New Pen(Stroke, StrokeThickness), geom)

    End Sub


#End Region
    '--- usage:
    '
    '<local:CPieSlice x : Name="PieSlice" Width="200" Height="200"
    '                    StartAngle="0" ArcAngle="280" Stroke="#FF0E2346" StrokeThickness="2">
    '<local: CPieSlice.Fill>
    '<RadialGradientBrush>
    '<GradientStop Color = "#FFE7F388" Offset="0"/>
    '<GradientStop Color = "#FF7BE42E" Offset="1"/>
    '           </RadialGradientBrush>
    '      </local:CPieSlice.Fill>
    ' </local:CPieSlice>


    '--- Radians
    '  An angle of 360° corresponds to a radian (radian/rad) of 2*pi
    '
    '--- conversion
    ' Definition: 360° = 2*Pi rad
    ' Degrees to radians: Degrees * Pi / 180°


    '--- ArcTo Parameter
    '
    'point 
    'The target point for the end of the arc.

    'size 
    'The radii (half the width and half the height) of an oval whose perimeter is used to draw the angle.
    'If the oval has a strong roundness in all directions, the arc will be rounded,
    'and when the oval is nearly flat, the arc is also nearly flat.
    'For example, a very large width and height will represent a very large oval, resulting in a slight curvature for the angle. 

    'rotationAngle (? does not work)    
    'The rotation angle of the oval that specifies the curve. The curvature of the arc can be rotated with this parameter.

    'isLargeArc     
    'true to draw the arc at an angle greater than 180 degrees, false otherwise.

    'sweepDirection     
    'A value that indicates whether the arc is drawn in the clockwise direction or the counterclockwise direction.

    'isStroked     
    'true to draw a segment with strokes when using a pen, false otherwise.

    'isSmoothJoin     
    'true to draw the connection between this segment and the previous segment as a corner,
    'if drawing strokes with a pen, False otherwise.


End Class
