Imports System.ComponentModel
Imports System.Windows.Controls.Primitives

Public Class DataGridTextColumnX
    Inherits DataGridTextColumn

    Public Sub New()
        Dim col As DataGridTextColumn = Me

        'Set all Cell Style items for columns which are using default values
        Dim stl As New Style(GetType(DataGridCell))
        CollectCellProperties(Me)
    End Sub

    ' Move the property to user defined Category
    <Description("Text (Foreground) brush"), Category("Column Options")>
    Public Overloads Property Foreground As Brush
        Get
            Return CType(GetValue(ForegroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(ForegroundProperty, value)
        End Set
    End Property

    Public Shared Shadows ReadOnly BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background",
            GetType(Brush), GetType(DataGridTextColumnX),
            New UIPropertyMetadata(New PropertyChangedCallback(AddressOf OnBackgroundChanged)))
    <Description("Background brush"), Category("Column Options")>
    Public Property Background As Brush
        Get
            Return CType(GetValue(BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundProperty, value)
        End Set
    End Property

    Private Shared Sub OnBackgroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub

#Region "Header TextAlignment"
    Public Shared ReadOnly HeaderTextAlignmentProperty As DependencyProperty = DependencyProperty.Register("HeaderTextAlignment",
            GetType(HorizontalAlignment), GetType(DataGridTextColumnX),
            New FrameworkPropertyMetadata(HorizontalAlignment.Left,
            New PropertyChangedCallback(AddressOf OnHeaderTextAlignmentChanged)))
    'New CoerceValueCallback(AddressOf CoerceHeaderTextAlignmentChanged)))
    <Description("Alignment of the Header text"), Category("Column Options")>   ' appears in VS property
    Public Property HeaderTextAlignment() As HorizontalAlignment
        Get
            Return CType(Me.GetValue(HeaderTextAlignmentProperty), HorizontalAlignment)
        End Get
        Set
            Me.SetValue(HeaderTextAlignmentProperty, Value)
        End Set
    End Property

    Private Shared Sub OnHeaderTextAlignmentChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        Dim hs As Style = CType(d.GetValue(HeaderStyleProperty), Style)

        If hs Is Nothing Then
            ' set a new style
            hs = New Style(GetType(DataGridColumnHeader))
            hs.Setters.Add(New Setter(TextBox.HorizontalContentAlignmentProperty, control.HeaderTextAlignment))
            control.HeaderStyle = hs
        Else
            ' add setter to existing style
            If hs.IsSealed = False Then
                hs.Setters.Add(New Setter(TextBox.HorizontalContentAlignmentProperty, control.HeaderTextAlignment))
            Else
                'create a new style based on the existing (but sealed) style
                Dim nhs As New Style(GetType(DataGridColumnHeader), hs)
                nhs.Setters.Add(New Setter(TextBox.HorizontalContentAlignmentProperty, control.HeaderTextAlignment))
                control.HeaderStyle = nhs
            End If
        End If
    End Sub
#End Region

#Region "Text Alignment"
    Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment",
            GetType(TextAlignment), GetType(DataGridTextColumnX),
            New FrameworkPropertyMetadata(TextAlignment.Left,
            New PropertyChangedCallback(AddressOf OnTextAlignmentChanged)))
    <Description("Alignment of the text"), Category("Column Options")>   ' appears in VS property
    Public Property TextAlignment() As TextAlignment
        Get
            Return CType(Me.GetValue(TextAlignmentProperty), TextAlignment)
        End Get
        Set
            Me.SetValue(TextAlignmentProperty, Value)
        End Set
    End Property

    Private Shared Sub OnTextAlignmentChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#Region "Cell Colors"

#Region "IsSelected"

#Region "IsSelected Background"
    Public Shared ReadOnly SelectedBackgroundProperty As DependencyProperty = DependencyProperty.Register("SelectedBackground",
           GetType(SolidColorBrush), GetType(DataGridTextColumnX),
           New FrameworkPropertyMetadata(SystemColors.HighlightBrush,
           New PropertyChangedCallback(AddressOf OnSelectedBackgroundChanged)))
    <Description("Background brush when cell is selected"), Category("Column Options")>   ' appears in VS property
    Public Property SelectedBackground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(SelectedBackgroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(SelectedBackgroundProperty, Value)
        End Set
    End Property

    Private Shared Sub OnSelectedBackgroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#Region "IsSelected Foreground"
    Public Shared ReadOnly SelectedForegroundProperty As DependencyProperty = DependencyProperty.Register("SelectedForeground",
           GetType(SolidColorBrush), GetType(DataGridTextColumnX),
           New FrameworkPropertyMetadata(SystemColors.HighlightTextBrush,
           New PropertyChangedCallback(AddressOf OnSelectedForegroundChanged)))
    <Description("Foreground brush when cell is selected"), Category("Column Options")>   ' appears in VS property
    Public Property SelectedForeground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(SelectedForegroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(SelectedForegroundProperty, Value)
        End Set
    End Property

    Private Shared Sub OnSelectedForegroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#Region "IsSelected Border"
    Public Shared ReadOnly SelectedBorderBrushProperty As DependencyProperty = DependencyProperty.Register("SelectedBorderBrush",
           GetType(SolidColorBrush), GetType(DataGridTextColumnX),
           New FrameworkPropertyMetadata(SystemColors.ActiveBorderBrush,
           New PropertyChangedCallback(AddressOf OnSelectedBorderBrushChanged)))
    <Description("Border brush when cell is selected"), Category("Column Options")>   ' appears in VS property
    Public Property SelectedBorderBrush() As SolidColorBrush
        Get
            Return CType(Me.GetValue(SelectedBorderBrushProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(SelectedBorderBrushProperty, Value)
        End Set
    End Property

    Private Shared Sub OnSelectedBorderBrushChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#End Region

#Region "IsFocused"

#Region "IsFocused Background"
    Public Shared ReadOnly FocusedBackgroundProperty As DependencyProperty = DependencyProperty.Register("FocusedBackground",
           GetType(SolidColorBrush), GetType(DataGridTextColumnX),
           New FrameworkPropertyMetadata(SystemColors.HighlightBrush,
           New PropertyChangedCallback(AddressOf OnFocusedBackgroundChanged)))
    <Description("Background brush when cell is focused"), Category("Column Options")>   ' appears in VS property
    Public Property FocusedBackground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(FocusedBackgroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(FocusedBackgroundProperty, Value)
        End Set
    End Property

    Private Shared Sub OnFocusedBackgroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#Region "IsFocused Foreground"
    Public Shared ReadOnly FocusedForegroundProperty As DependencyProperty = DependencyProperty.Register("FocusedForeground",
          GetType(SolidColorBrush), GetType(DataGridTextColumnX),
          New FrameworkPropertyMetadata(SystemColors.HighlightTextBrush,
          New PropertyChangedCallback(AddressOf OnFocusedForegroundChanged)))
    <Description("Foreground brush when cell is focused"), Category("Column Options")>   ' appears in VS property
    Public Property FocusedForeground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(FocusedForegroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(FocusedForegroundProperty, Value)
        End Set
    End Property

    Private Shared Sub OnFocusedForegroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#Region "IsFocused Border"
    Public Shared ReadOnly FocusedBorderBrushProperty As DependencyProperty = DependencyProperty.Register("FocusedBorderBrush",
           GetType(SolidColorBrush), GetType(DataGridTextColumnX),
           New FrameworkPropertyMetadata(SystemColors.ActiveBorderBrush,
           New PropertyChangedCallback(AddressOf OnFocusedBorderBrushChanged)))
    <Description("Border brush when cell is focused"), Category("Column Options")>   ' appears in VS property
    Public Property FocusedBorderBrush() As SolidColorBrush
        Get
            Return CType(Me.GetValue(FocusedBorderBrushProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(FocusedBorderBrushProperty, Value)
        End Set
    End Property

    Private Shared Sub OnFocusedBorderBrushChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As DataGridTextColumnX = CType(d, DataGridTextColumnX)
        CollectCellProperties(control)
    End Sub
#End Region

#End Region

    Private Shared Sub CollectCellProperties(col As DataGridTextColumnX)

        Dim stl As New Style(GetType(DataGridCell))

        stl.Setters.Add(New Setter(DataGridCell.BackgroundProperty, col.Background))
        ' Foreground exists in the base class, no Setter required

        stl.Setters.Add(New Setter(TextBox.TextAlignmentProperty, col.TextAlignment))

        Dim trig As New Trigger With {.Property = DataGridCell.IsSelectedProperty, .Value = True}
        trig.Setters.Add(New Setter(ForegroundProperty, col.SelectedForeground))
        trig.Setters.Add(New Setter(Control.BackgroundProperty, col.SelectedBackground))
        trig.Setters.Add(New Setter(Control.BorderBrushProperty, col.SelectedBorderBrush))
        stl.Triggers.Add(trig)

        trig = New Trigger With {.Property = DataGridCell.IsFocusedProperty, .Value = True}
        trig.Setters.Add(New Setter(DataGridCell.ForegroundProperty, col.FocusedForeground))
        trig.Setters.Add(New Setter(DataGridCell.BackgroundProperty, col.FocusedBackground))
        trig.Setters.Add(New Setter(Control.BorderBrushProperty, col.FocusedBorderBrush))
        stl.Triggers.Add(trig)

        col.CellStyle = stl

    End Sub

#End Region

End Class
