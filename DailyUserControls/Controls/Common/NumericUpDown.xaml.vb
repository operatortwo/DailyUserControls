Imports System.ComponentModel
Public Class NumericUpDown
    Inherits UserControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window
        Width = 75
        Height = 30
        HorizontalAlignment = HorizontalAlignment.Left
        VerticalAlignment = VerticalAlignment.Top
        Margin = New Thickness(140, 130, 0, 0)

    End Sub

    Private Const DefaultValue As Double = 0
    Private Const DefaultMinValue As Double = 0
    Private Const DefaultMaxValue As Double = 100
    Private Const DefaultStepValue As Double = 1
    Private Const DefaultDecimalPlaces As Integer = 0
    Private Const MaximumDecimalPlaces As Integer = 10

    Public FocusBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H56, &H9D, &HE5))

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

#Region "Control Region"
    Private Sub upButton_Click(ByVal sender As Object, ByVal e As EventArgs)

        Value += StepValue

        'Dim x As Double

        'x = Value
        'x = MinimumValue

    End Sub

    Private Sub downButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Value -= StepValue
    End Sub

    Private Sub userControl_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles userControl.PreviewKeyDown
        If e.Key = Key.Up Then
            Value += StepValue
            e.Handled = True
        ElseIf e.Key = Key.Down Then
            Value -= StepValue
            e.Handled = True
        End If
    End Sub
    Private Sub userControl_MouseWheel(sender As Object, e As MouseWheelEventArgs) Handles userControl.MouseWheel

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
    Private Sub userControl_GotFocus(sender As Object, e As RoutedEventArgs) Handles userControl.GotFocus
        FocusRect.Stroke = FocusBrush
    End Sub

    Private Sub userControl_LostFocus(sender As Object, e As RoutedEventArgs) Handles userControl.LostFocus
        CheckInput()
        ' check and set value
        FocusRect.Stroke = Brushes.Transparent
    End Sub
    Private Sub userControl_Loaded(sender As Object, e As RoutedEventArgs) Handles userControl.Loaded
        ' make sure value is shown at startup (if = default value)        
        ' ensure that value is shown at startup with the desired trailing zeros
        ShowValue()
    End Sub
    Private Sub userControl_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles userControl.MouseDown
        Me.Focus()
    End Sub
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
