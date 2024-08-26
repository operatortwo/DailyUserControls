Imports System.ComponentModel
Public Class NumericUpDown
    Inherits UserControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window (until 1.0.5.3)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 75
        'Height = 30
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
    Private Const DefaultUpDownColMinWidth As Double = 25   ' adjustment for Grid Column with UpDown buttons. used together with 'Width="Auto"'

#Region "Value Region"

    '--- Value Property

    'Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Decimal), GetType(ImageButton), New UIPropertyMetadata("ImageButton"))

    ''' <summary>
    ''' Identifies the Value dependency property.
    ''' </summary>
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Double), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultValue, New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue)))

    ' appears in code
    ''' <summary>
    ''' Gets or sets the value of Numeric Up Down
    ''' </summary>
    <Description("The current value of Numeric Up Down"), Category("Numeric UpDown")>   ' appears in VS property
    Public Property Value() As Double
        Get
            Return CDbl(GetValue(ValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(ValueProperty, value)
        End Set
    End Property

    'The Property is only used for type-safe access from the program code and should not include any logic,
    'apart from calling the GetValue() And SetValue() methods

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
        Dim control As NumericUpDown = CType(d, NumericUpDown)
        Dim places As Integer = CInt(d.GetValue(DecimalPlacesProperty))

        'control.TextBox1.Text = CStr(CDbl(d.GetValue(ValueProperty)))        
        control.ShowValue()

        Dim e As New RoutedPropertyChangedEventArgs(Of Double)(CDbl(args.OldValue), CDbl(args.NewValue), ValueChangedEvent)
        control.OnValueChanged(e)
    End Sub

    ''' <summary>
    ''' Identifies the ValueChanged routed event.
    ''' </summary>
    Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of Double)), GetType(NumericUpDown))

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

    Public Shared ReadOnly MinimumValueProperty As DependencyProperty = DependencyProperty.Register("MinimumValue", GetType(Double), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultMinValue, New PropertyChangedCallback(AddressOf OnMinimumValueChanged), New CoerceValueCallback(AddressOf CoerceMinimumValue)))
    <Description("Lowest valid value"), Category("Numeric UpDown")>
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
        'Dim control As NumericUpDown = CType(d, NumericUpDown)
    End Sub

#End Region

#Region "MaximumValue Region"
    '--- MaximumValue Property

    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Double), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultMaxValue, New PropertyChangedCallback(AddressOf OnMaximumValueChanged), New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    <Description("Highest valid value"), Category("Numeric UpDown")>
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
        'Dim control As NumericUpDown = CType(d, NumericUpDown)
    End Sub

#End Region

#Region "StepValue Region"
    '--- StepValue Property

    Public Shared ReadOnly StepValueProperty As DependencyProperty = DependencyProperty.Register("StepValue", GetType(Double), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultStepValue, New PropertyChangedCallback(AddressOf OnStepValueChanged), New CoerceValueCallback(AddressOf CoerceStepValue)))
    <Description("Step for increment/decrement Value"), Category("Numeric UpDown")>
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
        'Dim control As NumericUpDown = CType(d, NumericUpDown)
    End Sub

#End Region

#Region "DecimalPlaces Region"
    '--- StepValue Property

    Public Shared ReadOnly DecimalPlacesProperty As DependencyProperty = DependencyProperty.Register("DecimalPlaces", GetType(Integer), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultDecimalPlaces, New PropertyChangedCallback(AddressOf OnDecimalPlacesChanged), New CoerceValueCallback(AddressOf CoerceDecimalPlaces)))
    <Description("Decimal Places for Value"), Category("Numeric UpDown")>
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
        'Dim control As NumericUpDown = CType(d, NumericUpDown)
    End Sub

#End Region

#Region "Appearance"

    Public Shared ReadOnly TextBoxForegroundProperty As DependencyProperty = DependencyProperty.Register("TextBoxForeground", GetType(Brush), GetType(NumericUpDown), New UIPropertyMetadata(Brushes.Black))
    ' appears in code
    ''' <summary>
    ''' Background of the TextBox
    ''' </summary>
    <Description("Foregroundground of the TextBox"), Category("Numeric UpDown")>   ' appears in VS property
    Public Property TextBoxForeground As Brush
        Get
            Return CType(GetValue(TextBoxForegroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(TextBoxForegroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TextBoxBackgroundProperty As DependencyProperty = DependencyProperty.Register("TextBoxBackground", GetType(Brush), GetType(NumericUpDown), New UIPropertyMetadata(Brushes.White))
    ' appears in code
    ''' <summary>
    ''' Background of the TextBox
    ''' </summary>
    <Description("Background of the TextBox"), Category("Numeric UpDown")>   ' appears in VS property
    Public Property TextBoxBackground As Brush
        Get
            Return CType(GetValue(TextBoxBackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(TextBoxBackgroundProperty, value)
        End Set
    End Property

    Private Shared ReadOnly DefaultFocusStrokeBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H56, &H9D, &HE5))
    Public Shared ReadOnly FocusStrokeBrushProperty As DependencyProperty = DependencyProperty.Register("FocusStrokeBrush", GetType(Brush), GetType(NumericUpDown), New UIPropertyMetadata(DefaultFocusStrokeBrush))
    ''' <summary>
    ''' Stroke Brush of the focus rectangle
    ''' </summary>
    <Description("Stroke Brush of the focus rectangle"), Category("Numeric UpDown")>
    Public Property FocusStrokeBrush As Brush
        Get
            Return CType(GetValue(FocusStrokeBrushProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(FocusStrokeBrushProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FocusStrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("FocusStrokeThickness", GetType(Double), GetType(NumericUpDown), New UIPropertyMetadata(1.0))
    ''' <summary>
    ''' Stroke Thickness of the focus rectangle
    ''' </summary>
    <Description("Stroke Thickness of the focus rectangle"), Category("Numeric UpDown")>
    Public Property FocusStrokeThickness As Double
        Get
            Return CType(GetValue(FocusStrokeThicknessProperty), Double)
        End Get
        Set(ByVal value As Double)
            SetValue(FocusStrokeThicknessProperty, value)
        End Set
    End Property

#End Region

#Region "UpDown Column MinWidth"

    Public Shared ReadOnly UpDownColMinWidthProperty As DependencyProperty = DependencyProperty.Register("UpDownColMinWidth", GetType(Double), GetType(NumericUpDown), New FrameworkPropertyMetadata(DefaultUpDownColMinWidth, Nothing, New CoerceValueCallback(AddressOf CoerceUpDownColMinWidth)))
    <Description("Design adjustment for the Up/Dow buttons column. Default is 25"), Category("Numeric UpDown")>
    Public Property UpDownColMinWidth() As Double
        Get
            Return CDbl(GetValue(UpDownColMinWidthProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(UpDownColMinWidthProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceUpDownColMinWidth(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = CDbl(value)

        If newValue < 0 Then
            Return 0
        End If

        Return newValue
    End Function

#End Region

#Region "Control Region"
    Private Sub upButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Value = MaximumValue Then
            RaiseSpinUpEvent()
            Exit Sub
        End If
        Value += StepValue
    End Sub

    Private Sub downButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Value = MinimumValue Then
            RaiseSpinDownEvent()
            Exit Sub
        End If
        Value -= StepValue
    End Sub

    ''' <summary>
    ''' Simulate UpButton_Click. This can be used for cascading controls.
    ''' </summary>
    Public Sub PushUpButton()
        If Value = MaximumValue Then
            RaiseSpinUpEvent()
            Exit Sub
        End If
        Value += StepValue
    End Sub

    ''' <summary>
    ''' Simulate DownButton_Click. This can be used for cascading controls.
    ''' </summary>
    Public Sub PushDownButton()
        If Value = MinimumValue Then
            RaiseSpinDownEvent()
            Exit Sub
        End If
        Value -= StepValue
    End Sub

    Private Sub userControl_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles userControl.PreviewKeyDown
        If e.Key = Key.Up Then
            e.Handled = True
            If Value = MaximumValue Then
                RaiseSpinUpEvent()
                Exit Sub
            End If
            Value += StepValue
        ElseIf e.Key = Key.Down Then
            e.Handled = True
            If Value = MinimumValue Then
                RaiseSpinDownEvent()
                Exit Sub
            End If
            Value -= StepValue
        End If
    End Sub
    Private Sub userControl_MouseWheel(sender As Object, e As MouseWheelEventArgs) Handles userControl.MouseWheel

        ' The amount of the delta value is currently not taken into account
        ' Increase/decrease value
        '   --> see also MouseWheelDeltaForOneLine (= 120)

        If e.Delta > 0 Then
            If Value = MaximumValue Then
                RaiseSpinUpEvent()
                Exit Sub
            End If
            Value += StepValue
        End If

        ' If the mouse wheel delta is negative, move the box down.
        If e.Delta < 0 Then
            If Value = MinimumValue Then
                RaiseSpinDownEvent()
                Exit Sub
            End If
            Value -= StepValue
        End If

    End Sub
    Private Sub userControl_GotFocus(sender As Object, e As RoutedEventArgs) Handles userControl.GotFocus
        FocusRect.Stroke = FocusStrokeBrush
    End Sub

    Private Sub userControl_LostFocus(sender As Object, e As RoutedEventArgs) Handles userControl.LostFocus
        CheckInput()
        ' check and set value
        'FocusRect.Stroke = Brushes.Transparent
        FocusRect.Stroke = Nothing
    End Sub
    Private Sub userControl_Loaded(sender As Object, e As RoutedEventArgs) Handles userControl.Loaded
        ' make sure value is shown at startup (if = default value)        
        ' ensure that value is shown at startup with the desired trailing zeros
        ShowValue()
    End Sub
    Private Sub userControl_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles userControl.MouseDown
        Me.Focus()
    End Sub
    Private Sub upBotton_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs) Handles upBotton.PreviewMouseDown
        Focus()
    End Sub
    Private Sub downButton_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs) Handles downButton.PreviewMouseDown
        Focus()
    End Sub


#Region "SpinUp/Down events"
    Public Shared ReadOnly SpinUpEvent As RoutedEvent = EventManager.RegisterRoutedEvent("SpinUp", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(NumericUpDown))

    ' Provide CLR accessors for the event
    <Description("Occurs when the user wants to increase and the value is at maximum. This can be used to cascade multiple NumericUpDowns."), Category("Numeric UpDown")>   ' appears in VS property
    Public Custom Event SpinUp As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(SpinUpEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(SpinUpEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    ' This method raises the SpinUp event
    Private Sub RaiseSpinUpEvent()
        Dim newEventArgs As New RoutedEventArgs(SpinUpEvent)
        MyBase.RaiseEvent(newEventArgs)
    End Sub


    Public Shared ReadOnly SpinDownEvent As RoutedEvent = EventManager.RegisterRoutedEvent("SpinDown", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(NumericUpDown))

    ' Provide CLR accessors for the event
    <Description("Occurs when the user wants to decrease and the value is at minimum. This can be used to cascade multiple NumericUpDowns."), Category("Numeric UpDown")>   ' appears in VS property
    Public Custom Event SpinDown As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(SpinDownEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(SpinDownEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    ' This method raises the SpinDown event
    Private Sub RaiseSpinDownEvent()
        Dim newEventArgs As New RoutedEventArgs(SpinDownEvent)
        MyBase.RaiseEvent(newEventArgs)
    End Sub

#End Region

#End Region

#Region "TextBox Region"
    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.Key = Key.Enter Then
            CheckInput()                    ' check and set value

            Keyboard.Focus(ContentPresenter1)               ' set KeyboardFocus away from Textbox            
        End If
    End Sub

    Private Sub TextBox1_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles TextBox1.PreviewTextInput
        ' only allow special characters
        Dim regex As New Text.RegularExpressions.Regex("[0-9.-]")

        If regex.IsMatch(e.Text) = False Then
            e.Handled = True                                ' cancel
        End If

    End Sub

    Private Sub CheckInput()
        ' get Input from TextBox and try to convert to Double

        Dim str As String = TextBox1.Text
        Dim d As Double
        If Double.TryParse(str, d) = True Then              ' if valid number

            ' round to step value if necessary
            Dim steps As Double = Fix(d / StepValue)        ' number of steps
            Dim smod As Double = Math.Abs(d Mod StepValue)  ' remainder of the division

            If smod >= StepValue / 2 Then       ' is rounding necessary ?
                If d > 0 Then                   ' is number negative ?
                    steps += 1
                Else
                    steps -= 1
                End If
            End If

            d = steps * StepValue               ' calculate the number again

            If Value <> d Then                  ' only if number has changed
                Value = d                       ' set new value, text is updated via ValueChanged
                ShowValue()                     ' xxx
            Else
                ShowValue()                     ' Input did not result in a new value, show current value again
            End If
        Else                                    ' Input was invalid, display current value again
            ShowValue()
        End If

    End Sub

    Private Sub ShowValue()
        Dim str As String = CStr(Value)

        ' append zeros if necessary
        If DecimalPlaces > 0 Then               ' only if decimal places
            str = Value.ToString("F" & DecimalPlaces.ToString)
        End If

        TextBox1.Text = str
    End Sub















#End Region



End Class
