Imports System.ComponentModel
Public Class ProgressRing
    Inherits UserControl

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.        
    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
        '--- need to update ArcAngle if ProgressValue is DefaultValue ---
        If ProgressValue = DefaultProgressValue Then
            Dim args As New DependencyPropertyChangedEventArgs(ProgressValueProperty, DefaultProgressValue, DefaultProgressValue)
            OnProgressValueChanged(Me, args)
        End If
    End Sub

    Private Const DefaultMinValue As Double = 0
    Private Const DefaultMaxValue As Double = 100
    Private Const DefaultProgressValue As Double = 0.0F

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

    Private Shared ReadOnly DefaultRingBackFillBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HE6, &HE6, &HE6))

    Private Shared ReadOnly RingFillPropertyMetadata As New FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly RingFillProperty As DependencyProperty = DependencyProperty.Register("RingFill", GetType(Brush), GetType(ProgressRing), RingFillPropertyMetadata)

    '<Description("Ring Fill"), Category("Progress Ring")>   ' appears in VS property
    Public Property RingFill As Brush
        Get
            Return CType((GetValue(RingFillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(RingFillProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokePropertyMetadata As New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeProperty As DependencyProperty = DependencyProperty.Register("Stroke", GetType(Brush), GetType(ProgressRing), StrokePropertyMetadata)

    '<Description("Ring Fill"), Category("Progress Ring")>   ' appears in VS property
    Public Property Stroke As Brush
        Get
            Return CType((GetValue(StrokeProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(StrokeProperty, value)
        End Set
    End Property

    Private Shared ReadOnly StrokeThicknessPropertyMetadata As New FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly StrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("StrokeThickness", GetType(Double), GetType(ProgressRing), StrokeThicknessPropertyMetadata)

    <Description("Ellipse Stroke"), Category("Progress Ring")>   ' appears in VS property
    Public Property StrokeThickness As Double
        Get
            Return CDbl(GetValue(StrokeThicknessProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(StrokeThicknessProperty, value)
        End Set
    End Property

    Private Shared ReadOnly RingBackFillPropertyMetadata As New FrameworkPropertyMetadata(DefaultRingBackFillBrush, FrameworkPropertyMetadataOptions.AffectsRender)
    Public Shared ReadOnly RingBackFillProperty As DependencyProperty = DependencyProperty.Register("RingBackFill", GetType(Brush), GetType(ProgressRing), RingBackFillPropertyMetadata)

    '<Description("Ellipse Fill"), Category("Progress Ring")>   ' appears in VS property
    Public Property RingBackFill As Brush
        Get
            Return CType((GetValue(RingBackFillProperty)), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(RingBackFillProperty, value)
        End Set
    End Property

    Public Shared ReadOnly RingWidthRatioProperty As DependencyProperty = DependencyProperty.Register("RingWidthRatio",
                GetType(Integer), GetType(ProgressRing), New FrameworkPropertyMetadata(30,
                FrameworkPropertyMetadataOptions.AffectsRender, Nothing,
                New CoerceValueCallback(AddressOf CoerceRingWidthRatio)))
    <Description("Ratio of ring-width to control-size"), Category("Progress Ring")>   ' appears in VS property
    Public Property RingWidthRatio As Integer
        Get
            Return CInt(GetValue(RingWidthRatioProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(RingWidthRatioProperty, value)
        End Set
    End Property
    Private Shared Function CoerceRingWidthRatio(ByVal depObj As DependencyObject, ByVal baseVal As Object) As Object
        Dim ratio As Integer = CInt(baseVal)
        If ratio > 100 Then
            ratio = 100
        ElseIf ratio < 1 Then
            ratio = 1
        End If
        Return ratio
    End Function


#End Region

#Region "Angle and Value"
    'Private Const StartAngle As Single = 270.0          ' from 3 o'clock, clockwise (270 = 12 o'clock)

    Private Shared ReadOnly StartAnglePropertyMetadata As New FrameworkPropertyMetadata(270.0F,
                FrameworkPropertyMetadataOptions.AffectsRender, Nothing,
                New CoerceValueCallback(AddressOf CoerceStartAngle))
    Public Shared ReadOnly StartAngleProperty As DependencyProperty = DependencyProperty.Register("StartAngle",
                GetType(Single), GetType(ProgressRing), StartAnglePropertyMetadata)
    <Description("Start of the Arc, relative to three o'clock, clockwise"), Category("Progress Ring")>   ' appears in VS property
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

    Private Shared ReadOnly ProgressValuePropertyMetadata As New FrameworkPropertyMetadata(DefaultProgressValue,
                FrameworkPropertyMetadataOptions.AffectsRender,
                New PropertyChangedCallback(AddressOf OnProgressValueChanged),
                New CoerceValueCallback(AddressOf CoerceProgressValue))
    Public Shared ProgressValueProperty As DependencyProperty = DependencyProperty.Register("ProgressValue",
                GetType(Double), GetType(ProgressRing), ProgressValuePropertyMetadata)

    <Description("The current Progress Value"), Category("Progress Ring")>   ' appears in VS property
    Public Property ProgressValue As Double
        Get
            Return GetValue(ProgressValueProperty)
        End Get
        Set(value As Double)
            SetValue(ProgressValueProperty, value)
        End Set
    End Property
    Private Shared Function CoerceProgressValue(ByVal d As DependencyObject, ByVal baseVal As Object) As Object
        Dim value As Double = baseVal
        If value > d.GetValue(MaximumValueProperty) Then
            value = d.GetValue(MaximumValueProperty)
        ElseIf value < d.GetValue(MinimumValueProperty) Then
            value = d.GetValue(MinimumValueProperty)
        End If
        Return value
    End Function
    Private Shared Sub OnProgressValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ProgressRing = CType(d, ProgressRing)

        Dim minval As Double = d.GetValue(MinimumValueProperty)
        Dim maxval As Double = d.GetValue(MaximumValueProperty)
        Dim progval As Double = d.GetValue(ProgressValueProperty)
        Dim range As Double = Math.Abs(maxval - minval)

        Dim fact As Double = Math.Abs(progval - minval) / range

        '--- set ArcAngle for rendering ---
        d.SetValue(ArcAnglePropertyKey, CSng(fact * 360))

        '--- set Percent value for Information ---
        d.SetValue(ProgressPercentPropertyKey, CInt(fact * 100))
    End Sub

    '--- ArcAngle is needed for rendering ---
    '--- It is defined as DependencyProperty so that it can also be used as information outside of the control ---
    Friend Shared ReadOnly ArcAnglePropertyKey As DependencyPropertyKey =
            DependencyProperty.RegisterReadOnly("ArcAngle", GetType(Single), GetType(ProgressRing),
            New PropertyMetadata(0.0F))
    Public Shared ReadOnly ArcAngleProperty As DependencyProperty = ArcAnglePropertyKey.DependencyProperty
    ' ReadOnly properties are not shown in designer:
    '<Description("Get current value of ArcAngle"), Category("Progress Ring")>
    Public ReadOnly Property ArcAngle As Single
        Get
            Return GetValue(ArcAngleProperty)
        End Get
    End Property

    '--- ProgressPercent is defined as DependencyProperty so that it can be used as information outside of the control ---
    Friend Shared ReadOnly ProgressPercentPropertyKey As DependencyPropertyKey =
            DependencyProperty.RegisterReadOnly("ProgressPercent", GetType(Integer), GetType(ProgressRing),
            New PropertyMetadata(0))
    Public Shared ReadOnly ProgressPercentProperty As DependencyProperty = ProgressPercentPropertyKey.DependencyProperty
    ' ReadOnly properties are not shown in designer:
    '<Description("Get current value of ProgressPercent"), Category("Progress Ring")>
    Public ReadOnly Property ProgressPercent As Integer
        Get
            Return GetValue(ProgressPercentProperty)
        End Get
    End Property

#End Region

#Region "Minimum and Maximum"

    Public Shared ReadOnly MinimumValueProperty As DependencyProperty = DependencyProperty.Register("MinimumValue",
                GetType(Double), GetType(ProgressRing), New FrameworkPropertyMetadata(DefaultMinValue,
                New PropertyChangedCallback(AddressOf OnMinimumValueChanged),
                New CoerceValueCallback(AddressOf CoerceMinimumValue)))

    <Description("Lowest valid value"), Category("Progress Ring")>
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

    Public Shared ReadOnly MaximumValueProperty As DependencyProperty = DependencyProperty.Register("MaximumValue",
                GetType(Double), GetType(ProgressRing), New FrameworkPropertyMetadata(DefaultMaxValue,
                New PropertyChangedCallback(AddressOf OnMaximumValueChanged),
                New CoerceValueCallback(AddressOf CoerceMaximumValue)))

    <Description("Highest valid value"), Category("Progress Ring")>
    Public Property MaximumValue() As Double
        Get
            Return CDbl(GetValue(MaximumValueProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(MaximumValueProperty, value)
        End Set
    End Property

    Private Overloads Shared Function CoerceMaximumValue(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim newValue As Double = value
        Dim minValue As Double = d.GetValue(MinimumValueProperty)

        If newValue < minValue Then
            Return minValue + 1
        End If

        Return newValue
    End Function

    Private Shared Sub OnMaximumValueChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim maxValue As Double = args.NewValue
        Dim minValue As Double = d.GetValue(MinimumValueProperty)

        If minValue >= maxValue Then
            Dim control As ProgressRing = CType(d, ProgressRing)
            control.SetValue(MinimumValueProperty, maxValue - 1)
            Beep()
        End If

    End Sub


#End Region

#Region "Render"
    '
    ' Calculate points on an arc
    '
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
    '            --- to radians * Pi / 180 ---
    '
    '   ArcTo   Creates an elliptical arc between the current point and the specified end point.
    '
    '   size	        System.Windows.Size     The x- And y-radius Of the arc
    '   rotationAngle	System.Double           The rotation Of the ellipse, In degrees
    '   isLargeArc      Flag	                Set to 1 if the angle of the arc should be 180 degrees or greater;
    '                                           otherwise, set to 0.
    '   sweepDirection  Flag	                Set to 1 if the arc is drawn in a positive-angle direction;
    '                                           otherwise, set to 0.
    '   endPoint	    System.Windows.Point    The point To which the arc Is drawn.
    '
    '   
    ' ArcTo(Point, Size, rotationAngle, isLargeArc, SweepDirection, isStroked,isSmoothJoin)
    ' ArcTo(StartPoint, x and y radius, rotation in degrees (? 0.0), is > 180 degrees?, Clockwise or Couterclockwise ?,is Straked ?,is Smoothjoin ?)
    '

    Protected Overrides Sub OnRender(dc As DrawingContext)
        MyBase.OnRender(dc)

        Dim isLargeArc As Boolean = ArcAngle > 180.0

        '--- the control's drawing rectangle ---
        Dim maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness)
        Dim maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness)

        '--- Ring width ---
        'Dim RingWidthRatio As Integer = 30         ' ratio of control's diameter to RingWidth (1 to 100, percent)

        Dim RingWidth As Integer = maxWidth / 100 * RingWidthRatio
        Dim RingHeight As Integer = maxHeight / 100 * RingWidthRatio

        '--- outer arc ---
        Dim xStartOuter = maxWidth / 2.0 * Math.Cos((StartAngle) * Math.PI / 180.0)
        Dim yStartOuter = maxHeight / 2.0 * Math.Sin((StartAngle) * Math.PI / 180.0)

        Dim xEndOuter = maxWidth / 2.0 * Math.Cos((StartAngle + ArcAngle) * Math.PI / 180.0)
        Dim yEndOuter = maxHeight / 2.0 * Math.Sin((StartAngle + ArcAngle) * Math.PI / 180.0)

        '--- iner arc ---
        Dim xStartInner = (maxWidth - RingWidth) / 2.0 * Math.Cos((StartAngle) * Math.PI / 180.0)
        Dim yStartInner = (maxHeight - RingHeight) / 2.0 * Math.Sin((StartAngle) * Math.PI / 180.0)

        Dim xEndInner = (maxWidth - RingWidth) / 2.0 * Math.Cos((StartAngle + ArcAngle) * Math.PI / 180.0)
        Dim yEndInner = (maxHeight - RingHeight) / 2.0 * Math.Sin((StartAngle + ArcAngle) * Math.PI / 180.0)

        Dim centerX = RenderSize.Width / 2.0
        Dim centerY = RenderSize.Height / 2.0

        '--- for 2* 180 degrees ring ---
        Dim xEndOuterHalf = maxWidth / 2.0 * Math.Cos((StartAngle + 180) * Math.PI / 180.0)
        Dim yEndOuterHalf = maxHeight / 2.0 * Math.Sin((StartAngle + 180) * Math.PI / 180.0)

        Dim xEndInnerHalf = (maxWidth - RingWidth) / 2.0 * Math.Cos((StartAngle + 180) * Math.PI / 180.0)
        Dim yEndInnerHalf = (maxHeight - RingHeight) / 2.0 * Math.Sin((StartAngle + 180) * Math.PI / 180.0)


        '--- draw background Ring --        
        Dim geomb = New StreamGeometry

        Using ctx = geomb.Open
            ctx.BeginFigure(New Point(centerX + xStartOuter, centerY + yStartOuter), True, True)
            '--- arc does not draw a full circle, so we use 2* 180 degrees ---
            ctx.ArcTo(New Point(centerX + xEndOuterHalf, centerY + yEndOuterHalf), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, False, SweepDirection.Clockwise, True, False)
            ctx.ArcTo(New Point(centerX + xStartOuter, centerY + yStartOuter), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, False, SweepDirection.Clockwise, True, False)
            '--- inner arc ---
            ctx.LineTo(New Point(centerX + xStartInner, centerY + yStartInner), False, False)
            ctx.ArcTo(New Point(centerX + xEndInnerHalf, centerY + yEndInnerHalf), New Size((maxWidth - RingWidth) / 2.0, (maxHeight - RingHeight) / 2), 0.0, False, SweepDirection.Clockwise, True, False)
            ctx.ArcTo(New Point(centerX + xStartInner, centerY + yStartInner), New Size((maxWidth - RingWidth) / 2.0, (maxHeight - RingHeight) / 2), 0.0, False, SweepDirection.Clockwise, True, False)
        End Using

        dc.DrawGeometry(RingBackFill, Nothing, geomb)

        '--- draw Value Ring --
        If ArcAngle < 360 Then

            Dim geom = New StreamGeometry

            Using ctx = geom.Open
                ctx.BeginFigure(New Point(centerX + xStartOuter, centerY + yStartOuter), True, True)
                '--- outer arc, Clockwise ---
                ctx.ArcTo(New Point(centerX + xEndOuter, centerY + yEndOuter), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, isLargeArc, SweepDirection.Clockwise, True, False)
                '--- line to endpoint of inner arc ---
                ctx.LineTo(New Point(centerX + xEndInner, centerY + yEndInner), True, False)
                '--- inner arc, Counterclockwise ---
                ctx.ArcTo(New Point(centerX + xStartInner, centerY + yStartInner), New Size((maxWidth - RingWidth) / 2.0, (maxHeight - RingHeight) / 2), 0.0, isLargeArc, SweepDirection.Counterclockwise, True, False)
            End Using

            dc.DrawGeometry(RingFill, New Pen(Stroke, StrokeThickness), geom)

        Else
            '--- draw a full ring ---
            Dim geom = New StreamGeometry

            Using ctx = geom.Open
                ctx.BeginFigure(New Point(centerX + xStartOuter, centerY + yStartOuter), True, True)
                '--- arc does not draw a full circle, so we use 2* 180 degrees ---
                ctx.ArcTo(New Point(centerX + xEndOuterHalf, centerY + yEndOuterHalf), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, False, SweepDirection.Clockwise, True, False)
                ctx.ArcTo(New Point(centerX + xStartOuter, centerY + yStartOuter), New Size(maxWidth / 2.0, maxHeight / 2), 0.0, False, SweepDirection.Clockwise, True, False)
                '--- inner arc ---
                ctx.LineTo(New Point(centerX + xStartInner, centerY + yStartInner), False, False)
                ctx.ArcTo(New Point(centerX + xEndInnerHalf, centerY + yEndInnerHalf), New Size((maxWidth - RingWidth) / 2.0, (maxHeight - RingHeight) / 2), 0.0, False, SweepDirection.Clockwise, True, False)
                ctx.ArcTo(New Point(centerX + xStartInner, centerY + yStartInner), New Size((maxWidth - RingWidth) / 2.0, (maxHeight - RingHeight) / 2), 0.0, False, SweepDirection.Clockwise, True, False)
            End Using

            dc.DrawGeometry(RingFill, New Pen(Stroke, StrokeThickness), geom)
        End If






    End Sub





#End Region

End Class
