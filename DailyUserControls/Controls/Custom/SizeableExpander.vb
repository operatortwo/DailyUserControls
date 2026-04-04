Imports System.ComponentModel
Imports System.Windows.Markup

<ContentProperty("UserContent")> Public Class SizeableExpander
    Inherits System.Windows.Controls.Control

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(GetType(SizeableExpander), New FrameworkPropertyMetadata(GetType(SizeableExpander)))
    End Sub

    Private Sub Expander_Initalized() Handles Me.Initialized
        'SetInitialHeight()
    End Sub

    Private HeaderRow As New RowDefinition
    'Private SplitterRow As RowDefinition
    Private GridSplitter As New GridSplitter
    Private HeaderBackgroundRect As Rectangle
    Private ContentBackgroundRect As Rectangle
    Private HeaderBorder As New Border
    Private ContentBorder As New Border

    Private ExpanderIcon As New Expander

    Private CP1 As ContentPresenter

    Private Sub ExpanderLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If Me.Template IsNot Nothing Then

            HeaderRow = TryCast(Me.Template.FindName("HeaderRow", Me), RowDefinition)
            GridSplitter = TryCast(Me.Template.FindName("GridSplitter", Me), GridSplitter)

            HeaderBackgroundRect = TryCast(Me.Template.FindName("HeaderBackgroundRect", Me), Rectangle)
            ContentBackgroundRect = TryCast(Me.Template.FindName("ContentBackgroundRect", Me), Rectangle)

            HeaderBorder = TryCast(Me.Template.FindName("HeaderBorder", Me), Border)
            ContentBorder = TryCast(Me.Template.FindName("ContentBorder", Me), Border)

            ExpanderIcon = TryCast(Me.Template.FindName("ExpanderIcon", Me), Expander)

            If ExpanderIcon IsNot Nothing Then
                ExpanderIcon.IsExpanded = IsExpanded
            End If

            CP1 = TryCast(Me.Template.FindName("CP1", Me), ContentPresenter)

            If CP1 IsNot Nothing Then
                CP1.Content = UserContent
            End If

            If HeaderBorder IsNot Nothing Then
                HeaderBorder.CornerRadius = New CornerRadius(CornerUniformRadius)
                HeaderBorder.BorderThickness = New Thickness(BorderStrokeThickness)
            End If

            If ContentBorder IsNot Nothing Then
                ContentBorder.CornerRadius = New CornerRadius(CornerUniformRadius)
                ContentBorder.BorderThickness = New Thickness(BorderStrokeThickness)
                ContentBorder.Margin = New Thickness(0, 0 - BorderStrokeThickness, 0, 0)
            End If

            If GridSplitter IsNot Nothing Then
                If IsExpanded = False Then
                    ContentBorder.Visibility = Visibility.Collapsed
                    GridSplitter.Visibility = Visibility.Collapsed
                End If
            End If

            If ExpanderIcon IsNot Nothing Then
                AddHandler ExpanderIcon.Expanded, AddressOf ExpanderIcon_Expanded
                AddHandler ExpanderIcon.Collapsed, AddressOf ExpanderIcon_Collapsed
            End If

            If GridSplitter IsNot Nothing Then
                AddHandler GridSplitter.PreviewMouseLeftButtonDown, AddressOf GridSplitter_PreviewMouseLeftButtonDown
                AddHandler GridSplitter.PreviewMouseLeftButtonUp, AddressOf GridSplitter_PreviewMouseLeftButtonUp
                AddHandler GridSplitter.PreviewMouseMove, AddressOf GridSplitter_PreviewMouseMove
            End If

        End If

    End Sub

    Private Sub ExpanderUnloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        If ExpanderIcon IsNot Nothing Then
            RemoveHandler ExpanderIcon.Expanded, AddressOf ExpanderIcon_Expanded
            RemoveHandler ExpanderIcon.Collapsed, AddressOf ExpanderIcon_Collapsed
        End If
        If GridSplitter IsNot Nothing Then
            RemoveHandler GridSplitter.PreviewMouseLeftButtonDown, AddressOf GridSplitter_PreviewMouseLeftButtonDown
            RemoveHandler GridSplitter.PreviewMouseLeftButtonUp, AddressOf GridSplitter_PreviewMouseLeftButtonUp
            RemoveHandler GridSplitter.PreviewMouseMove, AddressOf GridSplitter_PreviewMouseMove
        End If
    End Sub


#Region "Appearance"

#Region "Hide some Properties"
    'hide unused Properties Of UserControl In designer   
    <Browsable(False)>
    Public Overloads Property Background As Brush
    '<Browsable(False)>
    'Public Overloads Property BorderBrush As Brush          ' would draw a border around the UserControl
    '<Browsable(False)>
    'Public Overloads Property BorderThickness As Thickness  ' would make userControl smaller
#End Region

    ' Foreground is used for Header Foreground


    Public Shared ReadOnly HeaderBackgroundProperty As DependencyProperty = DependencyProperty.Register("HeaderBackground", GetType(Brush), GetType(SizeableExpander), New UIPropertyMetadata(Brushes.LightGray))
    Public Property HeaderBackground As Brush
        Get
            Return CType(GetValue(HeaderBackgroundProperty), Brush)
        End Get
        Set(value As Brush)
            SetValue(HeaderBackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ContentBackgroundProperty As DependencyProperty = DependencyProperty.Register("ContentBackground", GetType(Brush), GetType(SizeableExpander), New UIPropertyMetadata(Brushes.WhiteSmoke))
    Public Property ContentBackground As Brush
        Get
            Return CType(GetValue(ContentBackgroundProperty), Brush)
        End Get
        Set(value As Brush)
            SetValue(ContentBackgroundProperty, value)
        End Set
    End Property

    Public Shared Shadows ReadOnly BorderBrushProperty As DependencyProperty = DependencyProperty.Register("BorderBrush", GetType(Brush), GetType(SizeableExpander), New UIPropertyMetadata(Brushes.Transparent))

    Public Overloads Property BorderBrush As Brush
        Get
            Return CType(GetValue(BorderBrushProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BorderBrushProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Header content
    ''' </summary>
    Public Shared ReadOnly HeaderProperty As DependencyProperty = DependencyProperty.Register("Header", GetType(Object), GetType(SizeableExpander), Nothing)
    <Description("Header content"), Category("SizeableExpander")>   ' appears in VS property
    Public Property Header As Object
        Get
            Return GetValue(HeaderProperty)
        End Get
        Set(value As Object)
            SetValue(HeaderProperty, value)
        End Set
    End Property


    ''' <summary>
    ''' Border Stroke-Thickness for header and content border
    ''' </summary>
    Public Shared ReadOnly BorderStrokeThicknessProperty As DependencyProperty = DependencyProperty.Register("BorderStrokeThickness",
            GetType(Double), GetType(SizeableExpander), New UIPropertyMetadata(1.0,
            New PropertyChangedCallback(AddressOf OnBorderStrokeThicknessChanged),
            New CoerceValueCallback(AddressOf CoerceBorderStrokeThickness)))
    <Description("Border Stroke-Thickness for header and content border"), Category("SizeableExpander")>   ' appears in VS property
    Public Property BorderStrokeThickness As Double
        Get
            Return GetValue(BorderStrokeThicknessProperty)
        End Get
        Set(value As Double)
            SetValue(BorderStrokeThicknessProperty, value)
        End Set
    End Property
    Private Shared Sub OnBorderStrokeThicknessChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SizeableExpander = CType(d, SizeableExpander)
        Dim value As Double = control.BorderStrokeThickness

        If control.HeaderBorder IsNot Nothing Then
            control.HeaderBorder.BorderThickness = New Thickness(value)
        End If

        If control.ContentBorder IsNot Nothing Then
            control.ContentBorder.BorderThickness = New Thickness(value)
            control.ContentBorder.Margin = New Thickness(0, 0 - control.BorderStrokeThickness, 0, 0)
        End If

    End Sub

    Private Overloads Shared Function CoerceBorderStrokeThickness(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim val As Double = CDbl(value)
        If val > 10 Then
            val = 10
        ElseIf val < 0.0 Then
            val = 0.0
        End If
        Return val
    End Function


    ''' <summary>
    ''' Corner Radius for Header and Content, Background and Border
    ''' </summary>
    Public Shared ReadOnly CornerUniformRadiusProperty As DependencyProperty = DependencyProperty.Register("CornerUniformRadius",
            GetType(Double), GetType(SizeableExpander), New UIPropertyMetadata(0.0,
            New PropertyChangedCallback(AddressOf OnCornerUniformRadiusChanged),
            New CoerceValueCallback(AddressOf CoerceCornerUniformRadius)))
    <Description("Corner Radius for header and content border"), Category("SizeableExpander")>   ' appears in VS property
    Public Property CornerUniformRadius As Double
        Get
            Return GetValue(CornerUniformRadiusProperty)
        End Get
        Set(value As Double)
            SetValue(CornerUniformRadiusProperty, value)
        End Set
    End Property

    Private Shared Sub OnCornerUniformRadiusChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SizeableExpander = CType(d, SizeableExpander)
        Dim value As Double = control.CornerUniformRadius

        If control.HeaderBorder IsNot Nothing Then
            control.HeaderBorder.CornerRadius = New CornerRadius(value)
        End If
        If control.ContentBorder IsNot Nothing Then
            control.ContentBorder.CornerRadius = New CornerRadius(value)
        End If
    End Sub

    Private Overloads Shared Function CoerceCornerUniformRadius(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim val As Double = CDbl(value)
        If val > 40 Then
            val = 40
        ElseIf val < 0.0 Then
            val = 0.0
        End If
        Return val
    End Function

#End Region

    Public Shared ReadOnly IsExpandedProperty As DependencyProperty = DependencyProperty.Register("IsExpanded", GetType(Boolean), GetType(SizeableExpander), New FrameworkPropertyMetadata(True, New PropertyChangedCallback(AddressOf OnIsExpandedChanged)))
    ' appears in code
    ''' <summary>
    ''' Is Expander expanded
    ''' </summary>
    <Description("Is Expander expanded"), Category("SizeableExpander")>   ' appears in VS property
    Public Property IsExpanded As Boolean

        Get
            Return CType(GetValue(IsExpandedProperty), Boolean)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsExpandedProperty, value)
        End Set

    End Property


    Private ExpandedHeight As Double = 100

    Private Shared Sub OnIsExpandedChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SizeableExpander = CType(d, SizeableExpander)

        If control.IsExpanded = True Then
            control.ExpanderIcon.IsExpanded = True
            control.ContentBorder.Visibility = Visibility.Visible
            control.GridSplitter.Visibility = Visibility.Visible
            control.Height = control.ExpandedHeight

            If control.ExpandedHeight > 40 Then
                control.Height = control.ExpandedHeight
            Else
                control.Height = 100
            End If
        Else
            control.ExpanderIcon.IsExpanded = False
            control.GridSplitter.Visibility = Visibility.Collapsed
            control.ContentBorder.Visibility = Visibility.Collapsed

            If control.HeaderRow.ActualHeight > 0 Then
                control.Height = control.HeaderRow.ActualHeight
                control.ExpandedHeight = control.ActualHeight
            Else
                control.Height = Double.NaN
            End If
        End If

    End Sub

    '--- Allow setting IsExpanded by code. This can also be done by setting the IsExpanded Property.
    ''' <summary>
    ''' Expand the TrackPanel
    ''' </summary>
    Public Sub Expand()
        IsExpanded = True
    End Sub

    ''' <summary>
    ''' Collapse the TrackPanel
    ''' </summary>
    Public Sub Collapse()
        IsExpanded = False
    End Sub

    Private Shared ReadOnly UserContentProperty As DependencyProperty =
        DependencyProperty.Register("UserContent", GetType(Object), GetType(SizeableExpander),
        New PropertyMetadata(Nothing))

    Public Property UserContent() As Object
        Get
            Return CType(GetValue(UserContentProperty), Object)
        End Get
        Set
            SetValue(UserContentProperty, Value)
        End Set
    End Property

    Private Sub ExpanderIcon_Collapsed(sender As Object, e As RoutedEventArgs)
        IsExpanded = False
    End Sub

    Private Sub ExpanderIcon_Expanded(sender As Object, e As RoutedEventArgs)
        IsExpanded = True
    End Sub


    Private Sub GridSplitter_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        el.CaptureMouse()
    End Sub

    Private Sub GridSplitter_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then
            el.ReleaseMouseCapture()
        End If
    End Sub

    Private Sub GridSplitter_PreviewMouseMove(sender As Object, e As MouseEventArgs)
        Dim el As UIElement = CType(sender, UIElement)
        If el.IsMouseCaptured Then

            If e.LeftButton = MouseButtonState.Pressed Then     ' dragging

                Dim pt As Point
                pt = e.GetPosition(Me)

                Dim minheight2 As Double = HeaderRow.ActualHeight + GridSplitter.ActualHeight
                Dim newheight2 As Double

                If pt.Y < minheight2 Then
                    newheight2 = minheight2
                Else
                    newheight2 = pt.Y
                End If

                Me.Height = newheight2

            End If
        End If
    End Sub



End Class