Imports System.ComponentModel

Public Class VU_Bar


    Public Sub New()

        InitializeComponent()

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 14
        'Height = 40
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top

        ''Margin = New Thickness(100, 100, 0, 0)

    End Sub

    Private Const DefaultMaxValue As Single = 100

#Region "Hide some Properties"
    ' hide unused Properties of UserControl in designer
    <Browsable(False)>
    Public Overloads Property Foreground As Brush
    <Browsable(False)>
    Public Overloads Property Background As Brush
    '<Browsable(False)>
    'Public Overloads Property BorderBrush As Brush
#End Region

#Region "Appearance"

    Private Shared ReadOnly DefaultBackFillBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HF0, &HF0, &HF0))

    Private Shared ReadOnly FillPropertyMetadata As New FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly FillProperty As DependencyProperty = DependencyProperty.Register("Fill", GetType(Brush), GetType(VU_Bar), FillPropertyMetadata)

    Public Property Fill As Brush
        Get
            Return CType((GetValue(FillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property

    Private Shared ReadOnly BackFillPropertyMetadata As New FrameworkPropertyMetadata(DefaultBackFillBrush, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly BackFillProperty As DependencyProperty = DependencyProperty.Register("BackFill", GetType(Brush), GetType(VU_Bar), BackFillPropertyMetadata)

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

    Private Shared ReadOnly ValuePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.AffectsRender, Nothing, New CoerceValueCallback(AddressOf CoerceValue))
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Single), GetType(VU_Bar), ValuePropertyMetadata)
    'Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Single), GetType(VU_Meter), New FrameworkPropertyMetadata(0.0F, Nothing, New CoerceValueCallback(AddressOf CoerceValue)))
    <Description("the current value"), Category("VU Bar")>
    Public Property Value() As Single
        Get
            Return CSng(GetValue(ValueProperty))
        End Get
        Set(ByVal value As Single)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Single = CSng(value)
        Dim maxValue As Single = CSng(d.GetValue(MaximumValueProperty))

        If newValue > maxValue Then
            Return maxValue
        End If

        Return newValue
    End Function

    'Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Single), GetType(VU_Meter), New FrameworkPropertyMetadata(DefaultMaxValue, New PropertyChangedCallback(AddressOf OnMaximumValueChanged), New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue", GetType(Single), GetType(VU_Bar), New FrameworkPropertyMetadata(DefaultMaxValue, Nothing, New CoerceValueCallback(AddressOf CoerceMaximumValue)))
    <Description("Highest valid value"), Category("VU Bar")>
    Public Property MaximumValue() As Single
        Get
            Return CSng(GetValue(MaximumValueProperty))
        End Get
        Set(ByVal value As Single)
            SetValue(MaximumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMaximumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Single = CSng(value)

        If newValue = 0 Then
            Return 1
        End If

        Return newValue
    End Function

#End Region

#Region "Render Region"
    Protected Overrides Sub OnRender(ByVal dc As DrawingContext)

        Dim height As Single = CSng(RenderSize.Height)

        Dim foreHeight As Single = height / MaximumValue * Value
        Dim backHeight As Single = height - foreHeight

        Dim backRect As New Rect(New Point(0, 0), New Size(RenderSize.Width, height - foreHeight))
        Dim foreRect As New Rect(New Point(0, 0), New Size(RenderSize.Width, height))

        dc.DrawRectangle(Fill, Nothing, foreRect)
        dc.DrawRectangle(BackFill, Nothing, backRect)

        ' first we divide (calculate) the control rectangle into a lower part and an upper part.
        ' the lower part represents the value, the upper part simulates transparency and is using the background brush
        ' this would work with SolidColorBrush for the 'Fill' Property but not with LinearGradientBrush
        ' for that reason, we fill the whole control rectangle with the 'Fill' brush, and overlay the upper part
        ' with the BackFill (Background) Brush.

    End Sub

    '--- first version, works only with SolidColorBrush (Fill Property)
    '
    'Protected Overrides Sub OnRender(ByVal dc As DrawingContext)

    '    Dim height As Single = CSng(RenderSize.Height)

    '    Dim foreHeight As Single = height / MaximumValue * Value
    '    Dim backHeight As Single = height - foreHeight

    '    Dim backRect As New Rect(New Point(0, 0), New Size(RenderSize.Width, height - foreHeight))
    '    Dim foreRect As New Rect(New Point(0, height - foreHeight), New Size(RenderSize.Width, foreHeight))

    '    dc.DrawRectangle(BackFill, Nothing, backRect)
    '    dc.DrawRectangle(Fill, Nothing, foreRect)

    'End Sub


#End Region


End Class
