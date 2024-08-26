Imports System.ComponentModel           ' for attributes (<Description>)
Public Class ToggleButton
    Inherits UserControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        '- initial values when inserted to the window (until 1.0.5.9)
        ' default initial values concerning sizing should not be set here, it can restrict the use of the control,
        ' f.e. it can prevent setting Alignment to stretch
        'Width = 140
        'Height = 30
        'HorizontalAlignment = HorizontalAlignment.Left
        'VerticalAlignment = VerticalAlignment.Top
        'Margin = New Thickness(100, 100, 0, 0)

    End Sub

    Private Const DefaultText As String = "ToggleButton"

    Public Shared DefaultBgCheckedBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HD2, &HE8, &H92))
    Public Shared DefaultBgUncheckedBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HE6, &HE6, &HE6))


    Public Shared DefaultBgDisabledBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H70, &H70, &H70))

    Private Shared DefaultPressedBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HDC, &HF2, &HFF))

    Public FocusBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H56, &H9D, &HE5))


    Public Enum Location
        Left
        Right
    End Enum

#Region "Appearance"

    Public Shared ReadOnly TgBtn_BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(Brushes.LightGray))

    <Browsable(False)>                                      ' hide it in designer
    Public Overloads Property Background As Brush
        Get
            Return CType(GetValue(TgBtn_BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(TgBtn_BackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BackgroundCheckedProperty As DependencyProperty = DependencyProperty.Register("BackgroundChecked", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(DefaultBgCheckedBrush, AddressOf OnBackgroundCheckedChanged))
    ' appears in code
    ''' <summary>
    ''' Background brush when checked
    ''' </summary>    
    <Description("Background when checked"), Category("Toggle Button")>   ' appears in VS property
    Public Property BackgroundChecked As Brush
        Get
            Return CType(GetValue(BackgroundCheckedProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundCheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnBackgroundCheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateBackground(control)
    End Sub

    Public Shared ReadOnly BackgroundUncheckedProperty As DependencyProperty = DependencyProperty.Register("BackgroundUnchecked", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(DefaultBgUncheckedBrush, AddressOf OnBackgroundUncheckedChanged))
    ' appears in code
    ''' <summary>
    ''' Background brush when unchecked
    ''' </summary>    
    <Description("Background when unchecked"), Category("Toggle Button")>   ' appears in VS property
    Public Property BackgroundUnchecked As Brush
        Get
            Return CType(GetValue(BackgroundUncheckedProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundUncheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnBackgroundUncheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateBackground(control)
    End Sub

    Public Shared ReadOnly BackgroundDisabledProperty As DependencyProperty = DependencyProperty.Register("BackgroundDisabled", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(DefaultBgDisabledBrush))
    ' appears in code
    ''' <summary>
    ''' Background brush when disabled, IsEnabled = FALSE
    ''' </summary>    
    <Description("Background when disabled")>   ' appears in VS property
    Public Property BackgroundDisabled As Brush
        Get
            Return CType(GetValue(BackgroundDisabledProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundDisabledProperty, value)
        End Set
    End Property

    Public Shared Shadows ReadOnly ForegroundProperty As DependencyProperty = DependencyProperty.Register("Foreground", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(Brushes.Black))

    Public Overloads Property Foreground As Brush
        Get
            Return CType(GetValue(ForegroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(ForegroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ButtonPressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBackground", GetType(Brush), GetType(ToggleButton), New UIPropertyMetadata(DefaultPressedBrush))
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


#Region "Image"

    Public Shared ReadOnly ImageProperty As DependencyProperty = DependencyProperty.Register("Image", GetType(ImageSource), GetType(ToggleButton), New UIPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Text on the button when checked
    ''' </summary>
    <Browsable(False)>
    Public Property Image As ImageSource
        Get
            Return CType(GetValue(ImageProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ImageCheckedProperty As DependencyProperty = DependencyProperty.Register("ImageChecked", GetType(ImageSource), GetType(ToggleButton), New UIPropertyMetadata(AddressOf OnImageCheckedChanged))
    ' appears in code
    ''' <summary>
    ''' The Image on the button when checked
    ''' </summary>
    <Description("The Image on the button when checked"), Category("Toggle Button")>   ' appears in VS property
    Public Property ImageChecked As ImageSource
        Get
            Return CType(GetValue(ImageCheckedProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageCheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnImageCheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateImage(control)
    End Sub

    Public Shared ReadOnly ImageUncheckedProperty As DependencyProperty = DependencyProperty.Register("ImageUnchecked", GetType(ImageSource), GetType(ToggleButton), New UIPropertyMetadata(AddressOf OnImageUncheckedChanged))
    ' appears in code
    ''' <summary>
    ''' The Image on the button when unchecked
    ''' </summary>
    <Description("The Image on the button when unchecked"), Category("Toggle Button")>   ' appears in VS property
    Public Property ImageUnchecked As ImageSource
        Get
            Return CType(GetValue(ImageUncheckedProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageUncheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnImageUncheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateImage(control)
    End Sub

    Public Shared ReadOnly ImageMarginProperty As DependencyProperty = DependencyProperty.Register("ImageMargin", GetType(Thickness), GetType(ToggleButton), New UIPropertyMetadata(New Thickness(0, 0, 0, 0)))
    ' appears in code
    ''' <summary>
    ''' Margins of the Image
    ''' </summary>
    <Description("Margins of the Image"), Category("Toggle Button")>   ' appears in VS property
    Public Property ImageMargin As Thickness

        Get
            Return CType(GetValue(ImageMarginProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(ImageMarginProperty, value)
        End Set

    End Property

    Public Shared ReadOnly ImageLocationProperty As DependencyProperty = DependencyProperty.Register("ImageLocation", GetType(Location), GetType(ToggleButton), New FrameworkPropertyMetadata(Location.Left, New PropertyChangedCallback(AddressOf OnImageLocationChanged), New CoerceValueCallback(AddressOf CoerceImageLocation)))
    <Description("Location of the Image"), Category("Toggle Button")>   ' appears in VS property
    Public Property ImageLocation() As Location
        Get
            Return CType(Me.GetValue(ImageLocationProperty), Location)
        End Get
        Set
            Me.SetValue(ImageLocationProperty, Value)
        End Set
    End Property

    Private Shared Sub OnImageLocationChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        Dim loc As Location = CType(d.GetValue(ImageLocationProperty), Location)

        If loc = Location.Left Then
            control.Image1.SetValue(Grid.ColumnProperty, 0)
            control.TextBlock1.SetValue(Grid.ColumnProperty, 1)

            control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Star)
            control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Auto)

        Else
            control.Image1.SetValue(Grid.ColumnProperty, 1)
            control.TextBlock1.SetValue(Grid.ColumnProperty, 0)

            control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Star)
            control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Auto)

        End If

        '--- or
        'Grid.SetColumn(control.Image1, 0)
        'Grid.SetColumn(control.Viewbox1, 1)

    End Sub

    Private Overloads Shared Function CoerceImageLocation(ByVal d As DependencyObject, ByVal value As Object) As Object
        Return value        ' no changes
    End Function

#End Region


#Region "Text"

    Private Shared ReadOnly TextPropertyMetadata As New FrameworkPropertyMetadata(DefaultText)
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(ToggleButton), TextPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Text on the button when checked
    ''' </summary>
    <Browsable(False)>
    Public Property Text As String
        Get
            Return CType(GetValue(TextProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TextCheckedProperty As DependencyProperty = DependencyProperty.Register("TextChecked", GetType(String), GetType(ToggleButton), New FrameworkPropertyMetadata(DefaultText, AddressOf OnTextCheckedChanged))
    ' appears in code
    ''' <summary>
    ''' The Text on the button when checked
    ''' </summary>
    <Description("The Text on the button when checked"), Category("Toggle Button")>   ' appears in VS property
    Public Property TextChecked As String
        Get
            Return CType(GetValue(TextCheckedProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextCheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnTextCheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateText(control)
    End Sub

    Public Shared ReadOnly TextUncheckedProperty As DependencyProperty = DependencyProperty.Register("TextUnchecked", GetType(String), GetType(ToggleButton), New FrameworkPropertyMetadata(DefaultText, AddressOf OnTextUncheckedChanged))
    ' appears in code
    ''' <summary>
    ''' The Text on the button when unchecked
    ''' </summary>
    <Description("The Text on the button when unchecked"), Category("Toggle Button")>   ' appears in VS property
    Public Property TextUnchecked As String
        Get
            Return CType(GetValue(TextUncheckedProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextUncheckedProperty, value)
        End Set
    End Property

    Private Shared Sub OnTextUncheckedChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)
        UpdateText(control)
    End Sub

    Public Shared ReadOnly TextAlignmentHorizontalProperty As DependencyProperty = DependencyProperty.Register("TextAlignmentHorizontal", GetType(HorizontalAlignment), GetType(ToggleButton), New UIPropertyMetadata(HorizontalAlignment.Center))
    ' appears in code
    ''' <summary>
    ''' Horizontal alignment of the text
    ''' </summary>
    <Description("Horizontal text alignment"), Category("Toggle Button")>   ' appears in VS property
    Public Property TextAlignmentHorizontal As HorizontalAlignment

        Get
            Return CType(GetValue(TextAlignmentHorizontalProperty), HorizontalAlignment)
        End Get
        Set(ByVal value As HorizontalAlignment)
            SetValue(TextAlignmentHorizontalProperty, value)
        End Set

    End Property

    Public Shared ReadOnly TextAlignmentVerticalProperty As DependencyProperty = DependencyProperty.Register("TextAlignmentVertical", GetType(VerticalAlignment), GetType(ToggleButton), New UIPropertyMetadata(VerticalAlignment.Center))
    ' appears in code
    ''' <summary>
    ''' Vertical alignment of the text
    ''' </summary>
    <Description("Vertical text alignment"), Category("Toggle Button")>   ' appears in VS property
    Public Property TextAlignmentVertical As VerticalAlignment

        Get
            Return CType(GetValue(TextAlignmentVerticalProperty), VerticalAlignment)
        End Get
        Set(ByVal value As VerticalAlignment)
            SetValue(TextAlignmentVerticalProperty, value)
        End Set

    End Property

    Public Shared ReadOnly TextPaddingProperty As DependencyProperty = DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(ToggleButton), New UIPropertyMetadata(New Thickness()))
    ' appears in code
    ''' <summary>
    ''' Padding of textblock
    ''' </summary>
    <Description("Padding of text"), Category("Toggle Button")>   ' appears in VS property
    Public Property TextPadding As Thickness

        Get
            Return CType(GetValue(TextPaddingProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(TextPaddingProperty, value)
        End Set

    End Property

#End Region


#Region "Control Properties"


    Public Shared ReadOnly IsCheckedProperty As DependencyProperty = DependencyProperty.Register("IsChecked", GetType(Boolean), GetType(ToggleButton), New FrameworkPropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsCheckedChanged)))
    ' appears in code
    ''' <summary>
    ''' IsChecked, is state ON or OFF
    ''' </summary>
    <Description("Is Checked, ON or OFF"), Category("Toggle Button")>   ' appears in VS property
    Public Property IsChecked As Boolean

        Get
            Return CType(GetValue(IsCheckedProperty), Boolean)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsCheckedProperty, value)
        End Set

    End Property

    Private Shared Sub OnIsCheckedChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As ToggleButton = CType(d, ToggleButton)

        If control.IsChecked = True Then
            ' fix: either call 'tgbtn_checked' OR use 'Handles tgbtn_checked', but not both,
            ' else the checked Event is raised twice
            ' control.ToggleButton_Checked(control, New RoutedEventArgs) -> not needed, Handles is used
            UpdateBackground(control)
            UpdateText(control)
            UpdateImage(control)
        Else
            'control.ToggleButton_Unchecked(control, New RoutedEventArgs)
            UpdateBackground(control)
            UpdateText(control)
            UpdateImage(control)
        End If

    End Sub

    Private Shared Sub UpdateBackground(control As ToggleButton)
        If control.IsChecked = True Then
            control.Background = control.BackgroundChecked
        Else
            control.Background = control.BackgroundUnchecked
        End If
    End Sub

    Private Shared Sub UpdateText(control As ToggleButton)
        If control.IsChecked = True Then
            control.Text = control.TextChecked
        Else
            control.Text = control.TextUnchecked
        End If
    End Sub

    Private Shared Sub UpdateImage(control As ToggleButton)
        If control.IsChecked = True Then
            control.Image = control.ImageChecked
        Else
            control.Image = control.ImageUnchecked
        End If
    End Sub


    ''' <summary>
    ''' Identifies the Checked routed event
    ''' </summary>
    <Description("Occurs when Checked")>                 ' appears in VS Property sheet (events)
    Public Event Checked As RoutedEventHandler
    Private Sub ToggleButton_Checked(sender As Object, e As RoutedEventArgs) Handles ToggleButton.Checked
        'ToggleButton.Background = BackgroundChecked        
        RaiseEvent Checked(Me, e)
    End Sub

    ''' <summary>
    ''' Identifies the Checked routed event
    ''' </summary>
    <Description("Occurs when Unchecked")>                 ' appears in VS Property sheet (events)
    Public Event Unchecked As RoutedEventHandler
    Private Sub ToggleButton_Unchecked(sender As Object, e As RoutedEventArgs) Handles ToggleButton.Unchecked
        'ToggleButton.Background = BackgroundUnchecked
        RaiseEvent Unchecked(Me, e)
    End Sub



    Public Shared ReadOnly ClickModeProperty As DependencyProperty = DependencyProperty.Register("ClickMode", GetType(ClickMode), GetType(ToggleButton), New UIPropertyMetadata(ClickMode.Release))
    ' appears in code
    ''' <summary>
    ''' Horizontal alignment of the text
    ''' </summary>
    <Description("Click Mode"), Category("Toggle Button")>   ' appears in VS property
    Public Property ClickMode As ClickMode

        Get
            Return CType(GetValue(ClickModeProperty), ClickMode)
        End Get
        Set(ByVal value As ClickMode)
            SetValue(ClickModeProperty, value)

        End Set


    End Property


#End Region

#Region "Control Region"


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
    Private Sub userControl_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles userControl.IsEnabledChanged
        If CBool(e.NewValue) = True Then
            Grid2.OpacityMask = Nothing
        Else
            Grid2.OpacityMask = DisabledOpacityMask
        End If
    End Sub

#End Region



End Class
