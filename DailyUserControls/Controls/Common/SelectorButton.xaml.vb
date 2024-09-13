Imports System.ComponentModel

Public Class SelectorButton
    Inherits UserControl

    Public Sub New()

        ' this call is required for the Designer
        InitializeComponent()

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 100
        'Height = 25
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top
        'Margin = New Thickness(150, 140, 0, 0)

    End Sub

    Private Const DefaultValue As Integer = 0
    Private Const DefaultMinValue As Integer = 0
    Private Const DefaultMaxValue As Integer = 100

    Private Const DefaultText As String = "Sel.Button"

    Public FocusBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H56, &H9D, &HE5))

    Public Shared DefaultInnerBorderBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H70, &H70, &H70))
    Private Shared DefaultPressedBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HDC, &HF2, &HFF))

#Region "Appearance"

    Public Shared Shadows ReadOnly BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(SelectorButton), New UIPropertyMetadata(Brushes.LightGray))

    Public Overloads Property Background As Brush
        Get
            Return CType(GetValue(BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly InnerBorderBrushProperty As DependencyProperty = DependencyProperty.Register("InnerBorderBrush", GetType(Brush), GetType(SelectorButton), New UIPropertyMetadata(DefaultInnerBorderBrush))
    ' appears in code
    ''' <summary>
    ''' Inner Border brush
    ''' </summary>    
    <Description("Inner Border brush")>   ' appears in VS property
    Public Overloads Property InnerBorderBrush As Brush
        Get
            Return CType(GetValue(InnerBorderBrushProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(InnerBorderBrushProperty, value)
        End Set
    End Property

    Public Shared Shadows ReadOnly ForegroundProperty As DependencyProperty = DependencyProperty.Register("Foreground", GetType(Brush), GetType(SelectorButton), New UIPropertyMetadata(Brushes.Black))

    Public Overloads Property Foreground As Brush
        Get
            Return CType(GetValue(ForegroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(ForegroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ButtonPressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBackground", GetType(Brush), GetType(SelectorButton), New UIPropertyMetadata(DefaultPressedBrush))
    ' appears in code
    ''' <summary>
    ''' Color when the Button is pressed
    ''' </summary>
    <Description("Color when the Button is pressed")>   ' appears in VS property
    Public Overloads Property ButtonPressedBackground As Brush
        Get
            Return CType(GetValue(ButtonPressedBackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(ButtonPressedBackgroundProperty, value)
        End Set
    End Property

#End Region

#Region "Text"

    Private Shared ReadOnly TextPropertyMetadata As New FrameworkPropertyMetadata(DefaultText)
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(SelectorButton), TextPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Text on the button
    ''' </summary>
    <Description("The Text on the button"), Category("Selector Button")>   ' appears in VS property
    Public Property Text As String

        Get
            Return CType(GetValue(TextProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set

    End Property


    Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment", GetType(HorizontalAlignment), GetType(SelectorButton), New UIPropertyMetadata(HorizontalAlignment.Center))
    ' appears in code
    ''' <summary>
    ''' Horizontal alignment of the text
    ''' </summary>
    <Description("Horizontal text alignment"), Category("Selector Button")>   ' appears in VS property
    Public Property TextAlignment As HorizontalAlignment

        Get
            Return CType(GetValue(TextAlignmentProperty), HorizontalAlignment)
        End Get
        Set(ByVal value As HorizontalAlignment)
            SetValue(TextAlignmentProperty, value)
        End Set

    End Property

    Public Shared ReadOnly TextPaddingProperty As DependencyProperty = DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(SelectorButton), New UIPropertyMetadata(New Thickness()))
    ' appears in code
    ''' <summary>
    ''' Padding of textblock
    ''' </summary>
    <Description("Padding of text"), Category("Selector Button")>   ' appears in VS property
    Public Property TextPadding As Thickness

        Get
            Return CType(GetValue(TextPaddingProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(TextAlignmentProperty, value)
        End Set

    End Property


#End Region

#Region "Value Region"

    '--- Value Property

    'Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Decimal), GetType(ImageButton), New UIPropertyMetadata("ImageButton"))

    ''' <summary>
    ''' Identifies the Value dependency property.
    ''' </summary>
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Integer), GetType(SelectorButton), New FrameworkPropertyMetadata(DefaultValue, New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue)))

    ' appears in code
    ''' <summary>
    ''' Gets or sets the value of Numeric Up Down
    ''' </summary>
    <Description("The current value of SelectorButton"), Category("Selector Button")>   ' appears in VS property
    Public Property Value() As Integer
        Get
            Return CInt(GetValue(ValueProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Integer = CInt(value)
        Dim minValue As Integer = CInt(d.GetValue(MinimumValueProperty))
        Dim maxValue As Integer = CInt(d.GetValue(MaximumValueProperty))

        If newValue > maxValue Then
            Return maxValue
        ElseIf newValue < minValue Then
            Return minValue
        End If

        Return newValue
    End Function

    Private Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SelectorButton = CType(d, SelectorButton)

        'control.TextBox1.Text = CStr(CDbl(d.GetValue(ValueProperty)))        
        'control.ShowValue()

        Dim e As New RoutedPropertyChangedEventArgs(Of Integer)(CInt(args.OldValue), CInt(args.NewValue), ValueChangedEvent)
        control.OnValueChanged(e)
    End Sub

    ''' <summary>
    ''' Identifies the ValueChanged routed event.
    ''' </summary>
    Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of Integer)), GetType(SelectorButton))

    ' appears in code (event-handler)
    ''' <summary>
    ''' Occurs when the Value property changes
    ''' </summary>
    <Description("Occurs when the Value property changes")>                 ' appears in VS Property sheet (events)
    Public Custom Event ValueChanged As RoutedPropertyChangedEventHandler(Of Integer)
        AddHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Integer))
            MyBase.AddHandler(ValueChangedEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedPropertyChangedEventHandler(Of Integer))
            MyBase.RemoveHandler(ValueChangedEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As System.Object, ByVal e As RoutedPropertyChangedEventArgs(Of Integer))
        End RaiseEvent
    End Event

    ''' <summary>
    ''' Raises the ValueChanged event.
    ''' </summary>
    ''' <param name="args">Arguments associated with the ValueChanged event.</param>
    Protected Overridable Sub OnValueChanged(ByVal args As RoutedPropertyChangedEventArgs(Of Integer))
        MyBase.RaiseEvent(args)
    End Sub

#End Region

#Region "MinimumValue Region"

    '--- MinimumValue Property

    Public Shared ReadOnly MinimumValueProperty As DependencyProperty = DependencyProperty.Register("MinimumValue", GetType(Integer), GetType(SelectorButton), New FrameworkPropertyMetadata(DefaultMinValue, New PropertyChangedCallback(AddressOf OnMinimumValueChanged), New CoerceValueCallback(AddressOf CoerceMinimumValue)))
    <Description("Lowest valid value"), Category("Selector Button")>
    Public Property MinimumValue() As Integer
        Get
            Return CInt(GetValue(MinimumValueProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(MinimumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMinimumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Integer = CInt(value)
        Dim maxValue As Integer = CInt(d.GetValue(MaximumValueProperty))

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

    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Integer), GetType(SelectorButton), New FrameworkPropertyMetadata(DefaultMaxValue, New PropertyChangedCallback(AddressOf OnMaximumValueChanged), New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    <Description("Highest valid value"), Category("Selector Button")>
    Public Property MaximumValue() As Integer
        Get
            Return CInt(GetValue(MaximumValueProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(MaximumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMaximumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Integer = CInt(value)
        Dim minValue As Integer = CInt(d.GetValue(MinimumValueProperty))

        If newValue < minValue Then
            Return minValue + 1
        End If

        Return newValue
    End Function

    Private Shared Sub OnMaximumValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        'Dim control As NumericUpDown = CType(d, NumericUpDown)
    End Sub

#End Region


#Region "Control Region"

    Private Sub upButton_Click(sender As Object, e As RoutedEventArgs)
        Value += 1
    End Sub

    Private Sub downButton_Click(sender As Object, e As RoutedEventArgs)
        Value -= 1
    End Sub

    Public Event Click As RoutedEventHandler
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles Button.Click
        RaiseEvent Click(Me, e)
    End Sub

    Private Sub UserControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Up Then
            Value += 1
            e.Handled = True
        ElseIf e.Key = Key.Down Then
            Value -= 1
            e.Handled = True
        End If
    End Sub

    Private Sub UserControl_MouseWheel(sender As Object, e As MouseWheelEventArgs)
        ' The amount of the delta value is currently not taken into account
        ' Increase/decrease value
        '   --> see also MouseWheelDeltaForOneLine (= 120)

        If e.Delta > 0 Then
            Value += 1
            e.Handled = True                    ' avoid scrolling inside scrollbar
        End If

        ' If the mouse wheel delta is negative, move the box down.
        If e.Delta < 0 Then
            Value -= 1
            e.Handled = True                    ' avoid scrolling inside scrollbar
        End If
    End Sub

    Private Sub UserControl_GotFocus(sender As Object, e As RoutedEventArgs)
        FocusRect.Stroke = FocusBrush
    End Sub

    Private Sub UserControl_LostFocus(sender As Object, e As RoutedEventArgs)
        FocusRect.Stroke = Brushes.Transparent
    End Sub

    Private Sub UserControl_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Focus()
    End Sub

    Private ReadOnly DisabledOpacityMask As SolidColorBrush = New SolidColorBrush(Color.FromArgb(&H54, 0, 0, 0))
    Private Sub UserControl_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        If CBool(e.NewValue) = True Then
            Grid1.OpacityMask = Nothing
        Else
            Grid1.OpacityMask = DisabledOpacityMask
        End If

    End Sub

#End Region


End Class