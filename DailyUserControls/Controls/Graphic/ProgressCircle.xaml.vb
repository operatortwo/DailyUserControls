Imports System.ComponentModel

Public Class ProgressCircle
    Inherits UserControl

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 100
        'Height = 100
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top
        'Margin = New Thickness(240, 160, 0, 0)

        ArcAngle = 60

    End Sub

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

    'Private Shared ReadOnly FillPropertyMetadata As New FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnArcAngleChanged), New CoerceValueCallback(AddressOf CoerceArcAngle))
    Private Shared ReadOnly FillPropertyMetadata As New FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly FillProperty As DependencyProperty = DependencyProperty.Register("Fill", GetType(Brush), GetType(ProgressCircle), FillPropertyMetadata)

    '<Description("Ellipse Fill"), Category("Progress Circle")>   ' appears in VS property
    Public Property Fill As Brush
        Get
            Return CType((GetValue(FillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(FillProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokePropertyMetadata As New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeProperty As DependencyProperty = DependencyProperty.Register("Stroke", GetType(Brush), GetType(ProgressCircle), StrokePropertyMetadata)

    '<Description("Ellipse Fill"), Category("Progress Circle")>   ' appears in VS property
    Public Property Stroke As Brush
        Get
            Return CType((GetValue(StrokeProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokeThicknessPropertyMetadata As New FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("StrokeThickness", GetType(Double), GetType(ProgressCircle), StrokeThicknessPropertyMetadata)

    <Description("Ellipse Stroke"), Category("Progress Circle")>   ' appears in VS property
    Public Property StrokeThickness As Double
        Get
            Return CDbl(GetValue(StrokeThicknessProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property

    Private Shared ReadOnly BackFillPropertyMetadata As New FrameworkPropertyMetadata(DefaultBackFillBrush, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly BackFillProperty As DependencyProperty = DependencyProperty.Register("BackFill", GetType(Brush), GetType(ProgressCircle), BackFillPropertyMetadata)

    '<Description("Ellipse Fill"), Category("Progress Circle")>   ' appears in VS property
    Public Property BackFill As Brush
        Get
            Return CType((GetValue(BackFillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackFillProperty, value)
        End Set
    End Property


#End Region

#Region "Angle and Value"
    'Private Const StartAngle As Single = 270.0          ' from 3 o'clock, clockwise (270 = 12 o'clock)


    Private Shared ReadOnly StartAnglePropertyMetadata As New FrameworkPropertyMetadata(270.0F, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnStartAngleChanged), New CoerceValueCallback(AddressOf CoerceStartAngle))
    Public Shared ReadOnly StartAngleProperty As DependencyProperty = DependencyProperty.Register("StartAngle", GetType(Single), GetType(ProgressCircle), StartAnglePropertyMetadata)
    <Description("Start of the Arc, relative to three o'clock, clockwise"), Category("Progress Circle")>   ' appears in VS property
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



    Private Shared ReadOnly ArcAnglePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnArcAngleChanged), New CoerceValueCallback(AddressOf CoerceArcAngle))
    Public Shared ReadOnly ArcAngleProperty As DependencyProperty = DependencyProperty.Register("ArcAngle", GetType(Single), GetType(ProgressCircle), ArcAnglePropertyMetadata)
    <Description("The current Angle for Arc"), Category("Progress Circle")>   ' appears in VS property
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
        Dim control As ProgressCircle = CType(d, ProgressCircle)
        control.UpdatingArcAngle = True

        If control.UpdatingProgressValue = False Then
            Dim value As Single
            'value = CSng((CSng(d.GetValue(ArcAngleProperty)) / 3.0))
            'value = CSng((CSng(d.GetValue(ArcAngleProperty)) - StartAngle) / 3.6)
            value = CSng((CSng(d.GetValue(ArcAngleProperty)) / 3.6))
            d.SetCurrentValue(ProgressValueProperty, value)
        End If

        control.UpdatingArcAngle = False
    End Sub

    Private Shared ReadOnly ProgressValuePropertyMetadata As New FrameworkPropertyMetadata(0.0F, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf OnProgressValueChanged), New CoerceValueCallback(AddressOf CoerceProgressValue))
    Public Shared ProgressValueProperty As DependencyProperty = DependencyProperty.Register("ProgressValue", GetType(Single), GetType(ProgressCircle), ProgressValuePropertyMetadata)

    <Description("The current Progress Value"), Category("Progress Circle")>   ' appears in VS property
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
        Dim control As ProgressCircle = CType(d, ProgressCircle)
        control.UpdatingProgressValue = True

        If control.UpdatingArcAngle = False Then
            Dim value As Single
            value = CSng(CSng(d.GetValue(ProgressValueProperty)) * 3.6)
            'd.SetCurrentValue(ArcAngleProperty, value + StartAngle)
            'd.SetCurrentValue(ArcAngleProperty, value - StartAngle)
            d.SetCurrentValue(ArcAngleProperty, value)
        End If

        control.UpdatingProgressValue = False
    End Sub

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
    '             y =  H * sin(a)
    '
    '             to radians * Pi / 180

    Protected Overrides Sub OnRender(ByVal dc As DrawingContext)

        If BackFill IsNot Nothing Then
            Dim myEllipseGeometry As New EllipseGeometry()
            myEllipseGeometry.Center = New Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0)
            myEllipseGeometry.RadiusX = (RenderSize.Width - StrokeThickness) / 2.0
            myEllipseGeometry.RadiusY = (RenderSize.Height - StrokeThickness) / 2.0

            dc.DrawGeometry(BackFill, New Pen(), myEllipseGeometry)
        End If

        '---

        Dim maxWidth = Math.Max(0.0, RenderSize.Width - Ellipse1.StrokeThickness)
        Dim maxHeight = Math.Max(0.0, RenderSize.Height - Ellipse1.StrokeThickness)

        Dim xStart = maxWidth / 2.0 * Math.Cos((StartAngle) * Math.PI / 180.0)
        Dim yStart = maxHeight / 2.0 * Math.Sin((StartAngle) * Math.PI / 180.0)

        Dim xEnd = maxWidth / 2.0 * Math.Cos((StartAngle + ArcAngle) * Math.PI / 180.0)
        Dim yEnd = maxHeight / 2.0 * Math.Sin((StartAngle + ArcAngle) * Math.PI / 180.0)

        'Dim geom = New StreamGeometry

        'If ArcAngle = 0 Then Return geom   ' Suppress start line

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


        'dc.DrawGeometry(Brushes.Green, New Pen(Brushes.Black, 1), geom)
        'dc.DrawGeometry(Foreground, New Pen(BorderBrush, BorderThickness.Left), geom)
        'dc.DrawGeometry(Fill, New Pen(BorderBrush, BorderThickness.Left), geom)
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
