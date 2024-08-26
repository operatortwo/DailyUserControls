Imports System.ComponentModel

Public Class SidePanelButton

    Public Sub New()

        ' this call is required for the Designer
        InitializeComponent()

        ' initial values
        'Button1.Background = New SolidColorBrush(Color.FromArgb(&HFF, &HCC, &HD5, &HF0))
        'Button1.Background = New SolidColorBrush(Color.FromArgb(&HFF, &HCC, &H5, &HF0))
        'Button1.ClickMode = ClickMode.Press

        'Background = BackgroundInactive
        'BackgroundInactive = Background
        'bgInactive = Brushes.CadetBlue

        'bgInactive = BackgroundInactive.Clone
    End Sub

    Friend bgInactive As Brush

    Public Shared DefaultBgActiveBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HED, &HF0, &HCC))
    Public Shared DefaultBgInactiveBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HCC, &HD5, &HF0))

    '' hide unused Properties of UserControl in designer   
    '<Browsable(False)>
    'Public Overloads Property Background As Brush


    Public Shared ReadOnly SPB_BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(SidePanelButton), New UIPropertyMetadata(DefaultBgInactiveBrush))

    Public Overloads Property Background As Brush
        Get
            Return CType(GetValue(SPB_BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(SPB_BackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BackgroundActiveProperty As DependencyProperty = DependencyProperty.Register("BackgroundActive", GetType(Brush), GetType(SidePanelButton), New UIPropertyMetadata(DefaultBgActiveBrush))

    Public Property BackgroundActive As Brush
        Get
            Return CType(GetValue(BackgroundActiveProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundActiveProperty, value)
        End Set
    End Property


    Public Shared ReadOnly BackgroundInactiveProperty As DependencyProperty = DependencyProperty.Register("BackgroundInactive", GetType(Brush), GetType(SidePanelButton), New UIPropertyMetadata(DefaultBgInactiveBrush, AddressOf OnBackgroundInactiveChanged))

    Public Property BackgroundInactive As Brush
        Get
            Return CType(GetValue(BackgroundInactiveProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundInactiveProperty, value)
        End Set
    End Property

    Private Shared Sub OnBackgroundInactiveChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As SidePanelButton = CType(d, SidePanelButton)

        'control.TextBox1.Text = CStr(CDbl(d.GetValue(ValueProperty)))        
        'control.ShowValue()

        control.Background = control.BackgroundInactive

    End Sub


    Public Shared ReadOnly SPB_ContentProperty As DependencyProperty = DependencyProperty.Register("Content", GetType(Object), GetType(SidePanelButton), New PropertyMetadata("S Button"))

    Public Overloads Property Content As Object
        Get
            Return CType(GetValue(SPB_ContentProperty), Object)
        End Get
        Set(ByVal value As Object)
            SetValue(SPB_ContentProperty, value)
        End Set
    End Property

    'Public Shared ReadOnly TextPaddingProperty As DependencyProperty = DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(ImageButton2), New UIPropertyMetadata(New Thickness()))
    Public Shared ReadOnly SPB_PaddingProperty As DependencyProperty = DependencyProperty.Register("Padding", GetType(Thickness), GetType(SidePanelButton), New UIPropertyMetadata(New Thickness()))

    Public Overloads Property Padding As Thickness
        Get
            Return CType(GetValue(SPB_PaddingProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(SPB_PaddingProperty, value)
        End Set
    End Property

    Public Shared ReadOnly AssociatedWindowProperty As DependencyProperty = DependencyProperty.Register("AssociatedWindow", GetType(Window), GetType(SidePanelButton), New PropertyMetadata)

    ' appears in code
    ''' <summary>
    ''' Name of the associated Window
    ''' </summary>
    <Description("The associated Window"), Category("SidePanelButton")>   ' appears in VS property
    Public Property AssociatedWindow As Window
        Get
            Return CType(GetValue(AssociatedWindowProperty), Window)
        End Get
        Set(ByVal value As Window)
            SetValue(AssociatedWindowProperty, value)
        End Set
    End Property

    Public Event Click(sender As Object, e As RoutedEventArgs)

    Private Sub Button1_Click(sender As Object, e As RoutedEventArgs) Handles Button1.Click
        RaiseEvent Click(Me, e)
    End Sub
End Class
