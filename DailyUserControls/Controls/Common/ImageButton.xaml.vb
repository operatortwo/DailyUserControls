Imports System.ComponentModel

Public Class ImageButton
    Private Shared DefaultBackgroundBrush As New SolidColorBrush(Color.FromArgb(&HFF, &HE6, &HE6, &HE6))
    Private Shared DefaultBorderBrush As New SolidColorBrush(Color.FromArgb(&HFF, &H70, &H70, &H70))
    Shared Sub New()
        BackgroundProperty.OverrideMetadata(GetType(ImageButton), New FrameworkPropertyMetadata(DefaultBackgroundBrush))
        BorderBrushProperty.OverrideMetadata(GetType(ImageButton), New FrameworkPropertyMetadata(DefaultBorderBrush))
        BorderThicknessProperty.OverrideMetadata(GetType(ImageButton), New FrameworkPropertyMetadata(New Thickness(1, 1, 1, 1)))
    End Sub
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
        'Margin = New Thickness(90, 90, 0, 0)

    End Sub

    Private Const DefaultText As String = "ImageButton"

    Public Enum Location
        Left
        Right
        Top
        Bottom
    End Enum

#Region "Appearance"

    ' V 1.0.5.1
    ' the following prevents updating the color in design-time with VS2019 --> commented out
    ' --> UserControl already has a Background and a Foreground Property, no sense to define it again.
    'Public Shared Shadows ReadOnly BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(ImageButton), New UIPropertyMetadata(Brushes.LightGray))

    'Public Overloads Property Background As Brush
    '    Get
    '        Return CType(GetValue(BackgroundProperty), Brush)
    '    End Get
    '    Set(ByVal value As Brush)
    '        SetValue(BackgroundProperty, value)
    '    End Set
    'End Property

    'Public Shared ReadOnly ImgBtn_ForegroundProperty As DependencyProperty = DependencyProperty.Register("Foreground", GetType(Brush), GetType(ImageButton), New UIPropertyMetadata(Brushes.Black))

    'Public Overloads Property Foreground As Brush
    '    Get
    '        Return CType(GetValue(ImgBtn_ForegroundProperty), Brush)
    '    End Get
    '    Set(ByVal value As Brush)
    '        SetValue(ImgBtn_ForegroundProperty, value)
    '    End Set
    'End Property

    Public Shared ReadOnly ButtonPressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBackground", GetType(Color), GetType(ImageButton), New UIPropertyMetadata(Colors.LightGreen))
    ' appears in code
    ''' <summary>
    ''' Color when the Button is pressed
    ''' </summary>
    <Description("Color when the Button is pressed"), Category("ImageButton")>   ' appears in VS property
    Public Overloads Property ButtonPressedBackground As Color
        Get
            Return CType(GetValue(ButtonPressedBackgroundProperty), Color)
        End Get
        Set(ByVal value As Color)
            SetValue(ButtonPressedBackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ButtonPressedBorderProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBorder", GetType(Color), GetType(ImageButton), New UIPropertyMetadata(Colors.LightSalmon))
    ' appears in code
    ''' <summary>
    ''' Border color when the button is pressed
    ''' </summary>
    <Description("Border color when the button is pressed"), Category("ImageButton")>   ' appears in VS property
    Public Overloads Property ButtonPressedBorder As Color
        Get
            Return CType(GetValue(ButtonPressedBorderProperty), Color)
        End Get
        Set(ByVal value As Color)
            SetValue(ButtonPressedBorderProperty, value)
        End Set
    End Property

#End Region

#Region "Role"

    Private Const IsCancelDescription = "Gets or sets a value that indicates whether a Button is a Cancel button. A user can activate the Cancel button by pressing the ESC key."

    Public Shared ReadOnly IsCancelProperty As DependencyProperty = DependencyProperty.Register("IsCancel", GetType(Boolean), GetType(ImageButton))
    ' appears in code
    ''' <summary>
    ''' Gets or sets a value that indicates whether a Button is a Cancel button. A user can activate the Cancel button by pressing the ESC key.
    ''' </summary>
    <Description(IsCancelDescription), Category("Common Properties")>   ' appears in VS property
    Public Overloads Property IsCancel As Boolean
        Get
            Return GetValue(IsCancelProperty)
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsCancelProperty, value)
        End Set
    End Property

    Private Const IsDefaultlDescription = "Gets or sets a value that indicates whether a Button is the default button. A user invokes the default button by pressing the ENTER key."

    Public Shared ReadOnly IsDefaultlProperty As DependencyProperty = DependencyProperty.Register("IsDefault", GetType(Boolean), GetType(ImageButton))
    ' appears in code
    ''' <summary>
    ''' Gets or sets a value that indicates whether a Button is the default button. A user invokes the default button by pressing the ENTER key.
    ''' </summary>
    <Description(IsDefaultlDescription), Category("Common Properties")>   ' appears in VS property
    Public Overloads Property IsDefault As Boolean
        Get
            Return GetValue(IsDefaultlProperty)
        End Get
        Set(value As Boolean)
            SetValue(IsDefaultlProperty, value)
        End Set
    End Property

#End Region

#Region "Image"

    Public Shared ReadOnly ImageProperty As DependencyProperty = DependencyProperty.Register("Image", GetType(ImageSource), GetType(ImageButton), New UIPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Image on the button
    ''' </summary>
    <Description("The Image on the button"), Category("ImageButton")>   ' appears in VS property
    Public Property Image As ImageSource
        Get
            Return CType(GetValue(ImageProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ImageMarginProperty As DependencyProperty = DependencyProperty.Register("ImageMargin", GetType(Thickness), GetType(ImageButton), New UIPropertyMetadata(New Thickness(2, 2, 0, 2)))
    ' appears in code
    ''' <summary>
    ''' Margins of the Image
    ''' </summary>
    <Description("Margins of the Image"), Category("ImageButton")>   ' appears in VS property
    Public Property ImageMargin As Thickness

        Get
            Return CType(GetValue(ImageMarginProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(ImageMarginProperty, value)
        End Set

    End Property

    Public Shared ReadOnly ImageLocationProperty As DependencyProperty = DependencyProperty.Register("ImageLocation", GetType(Location), GetType(ImageButton), New FrameworkPropertyMetadata(Location.Left, New PropertyChangedCallback(AddressOf OnImageLocationChanged), New CoerceValueCallback(AddressOf CoerceImageLocation)))
    <Description("Location of the Image"), Category("ImageButton")>   ' appears in VS property
    Public Property ImageLocation() As Location
        Get
            Return CType(Me.GetValue(ImageButton.ImageLocationProperty), Location)
        End Get
        Set
            Me.SetValue(ImageButton.ImageLocationProperty, Value)
        End Set
    End Property

    Private Shared Sub OnImageLocationChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As ImageButton = CType(d, ImageButton)
        Dim loc As Location = CType((d.GetValue(ImageLocationProperty)), Location)

        Select Case loc
            Case Location.Left
                control.Image1.SetValue(Grid.RowProperty, 0)
                control.Image1.SetValue(Grid.ColumnProperty, 0)
                control.Viewbox1.SetValue(Grid.RowProperty, 0)
                control.Viewbox1.SetValue(Grid.ColumnProperty, 1)

                control.Grid2.RowDefinitions(0).Height = New GridLength(1, GridUnitType.Star)
                control.Grid2.RowDefinitions(1).Height = New GridLength(1, GridUnitType.Auto)

                control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Auto)
                control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Star)

            Case Location.Right
                control.Image1.SetValue(Grid.RowProperty, 0)
                control.Image1.SetValue(Grid.ColumnProperty, 1)
                control.Viewbox1.SetValue(Grid.RowProperty, 0)
                control.Viewbox1.SetValue(Grid.ColumnProperty, 0)

                control.Grid2.RowDefinitions(0).Height = New GridLength(1, GridUnitType.Star)
                control.Grid2.RowDefinitions(1).Height = New GridLength(1, GridUnitType.Auto)

                control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Star)
                control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Auto)

            Case Location.Top
                control.Image1.SetValue(Grid.RowProperty, 0)
                control.Image1.SetValue(Grid.ColumnProperty, 0)
                control.Viewbox1.SetValue(Grid.RowProperty, 1)
                control.Viewbox1.SetValue(Grid.ColumnProperty, 0)

                control.Grid2.RowDefinitions(0).Height = New GridLength(1, GridUnitType.Star)
                control.Grid2.RowDefinitions(1).Height = New GridLength(1, GridUnitType.Star)

                control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Star)
                control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Auto)

            Case Location.Bottom
                control.Image1.SetValue(Grid.RowProperty, 1)
                control.Image1.SetValue(Grid.ColumnProperty, 0)
                control.Viewbox1.SetValue(Grid.RowProperty, 0)
                control.Viewbox1.SetValue(Grid.ColumnProperty, 0)

                control.Grid2.RowDefinitions(0).Height = New GridLength(1, GridUnitType.Star)
                control.Grid2.RowDefinitions(1).Height = New GridLength(1, GridUnitType.Star)

                control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Star)
                control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Auto)

        End Select

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
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(ImageButton), TextPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Text on the button
    ''' </summary>
    <Description("The Text on the button"), Category("ImageButton")>   ' appears in VS property
    Public Property Text As String

        Get
            Return CType(GetValue(TextProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set

    End Property


    Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment", GetType(HorizontalAlignment), GetType(ImageButton), New UIPropertyMetadata(HorizontalAlignment.Stretch))
    ' appears in code
    ''' <summary>
    ''' Horizontal alignment of the text
    ''' </summary>
    <Description("Horizontal text alignment"), Category("ImageButton")>   ' appears in VS property
    Public Property TextAlignment As HorizontalAlignment

        Get
            Return CType(GetValue(TextAlignmentProperty), HorizontalAlignment)
        End Get
        Set(ByVal value As HorizontalAlignment)
            SetValue(TextAlignmentProperty, value)
        End Set

    End Property

    Public Shared ReadOnly TextPaddingProperty As DependencyProperty = DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(ImageButton), New UIPropertyMetadata(New Thickness()))
    ' appears in code
    ''' <summary>
    ''' Padding of textblock
    ''' </summary>
    <Description("Padding of textblock"), Category("ImageButton")>   ' appears in VS property
    Public Property TextPadding As Thickness

        Get
            Return CType(GetValue(TextPaddingProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(TextPaddingProperty, value)
        End Set

    End Property


#End Region

#Region "Control"

    Public Event Click As RoutedEventHandler
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles Button.Click
        RaiseEvent Click(Me, e)
    End Sub


    Private ReadOnly DisabledOpacityMask As SolidColorBrush = New SolidColorBrush(Color.FromArgb(&H54, 0, 0, 0))
    'Private OriginalBorderBrush As Brush                                     ' for IsEnabled.Changed
    Private Sub userControl_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles userControl.IsEnabledChanged

        If CBool(e.NewValue) = True Then
            'Grid1.OpacityMask = Nothing
            OpacityMask = Nothing
        Else
            'Grid1.OpacityMask = DisabledOpacityMask
            OpacityMask = DisabledOpacityMask
        End If

        'If DesignerProperties.GetIsInDesignMode(Me) = False Then
        '    If CBool(e.NewValue) = True Then
        '        BorderBrush = OriginalBorderBrush
        '    Else
        '        OriginalBorderBrush = BorderBrush
        '        BorderBrush = Brushes.Transparent
        '    End If
        'End If

    End Sub

#End Region

End Class
