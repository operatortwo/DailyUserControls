Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Controls.Primitives
Public Class CheckboxFilterList
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
        'Margin = New Thickness(90, 90, 0, 0)

    End Sub

    Private ScreenScale As Double = 1.0

    Private Sub userControl_Loaded(sender As Object, e As RoutedEventArgs) Handles userControl.Loaded
        If Me.ActualHeight <> 0 Then
            ScreenScale = GetScreenScale(Me)
        End If
    End Sub

    Private Const DefaultText As String = "Filter List"

    Public Enum Location
        Left
        Right
    End Enum

#Region "Appearance"

    Public Shared Shadows ReadOnly BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(CheckboxFilterList), New UIPropertyMetadata(Brushes.LightGray))

    Public Overloads Property Background As Brush
        Get
            Return CType(GetValue(BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(BackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ImgBtn_ForegroundProperty As DependencyProperty = DependencyProperty.Register("Foreground", GetType(Brush), GetType(CheckboxFilterList), New UIPropertyMetadata(Brushes.Black))

    Public Overloads Property Foreground As Brush
        Get
            Return CType(GetValue(ImgBtn_ForegroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(ImgBtn_ForegroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ButtonPressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBackground", GetType(Color), GetType(CheckboxFilterList), New UIPropertyMetadata(Colors.LightGreen))
    ' appears in code
    ''' <summary>
    ''' Color when the Button is pressed
    ''' </summary>
    <Description("Color when the Button is pressed"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Overloads Property ButtonPressedBackground As Color
        Get
            Return CType(GetValue(ButtonPressedBackgroundProperty), Color)
        End Get
        Set(ByVal value As Color)
            SetValue(ButtonPressedBackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ButtonPressedBorderProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBorder", GetType(Color), GetType(CheckboxFilterList), New UIPropertyMetadata(Colors.LightSalmon))
    ' appears in code
    ''' <summary>
    ''' Border color when the button is pressed
    ''' </summary>
    <Description("Border color when the button is pressed"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Overloads Property ButtonPressedBorder As Color
        Get
            Return CType(GetValue(ButtonPressedBorderProperty), Color)
        End Get
        Set(ByVal value As Color)
            SetValue(ButtonPressedBorderProperty, value)
        End Set
    End Property

#End Region

#Region "List-Window Appearance"

    Private Const MinListWinWidth = 80
    Private Const MaxListWinWidth = 250
    Private Const DefaultListWinWidth = 150.0
    Public Shared ReadOnly ListWindowWidthProperty As DependencyProperty = DependencyProperty.Register("ListWindowWidth", GetType(Double), GetType(CheckboxFilterList), New FrameworkPropertyMetadata(DefaultListWinWidth, Nothing, AddressOf CoerceListWindowWidth))
    ' appears in code
    ''' <summary>
    ''' Width of the FilterListWindow. (80 - 250)
    ''' </summary>
    <Description("Width of the FilterListWindow. (80 - 250)"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property ListWindowWidth As Double
        Get
            Return CType(GetValue(ListWindowWidthProperty), Double)
        End Get
        Set(ByVal value As Double)
            SetValue(ListWindowWidthProperty, value)
        End Set
    End Property

    Private Shared Function CoerceListWindowWidth(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim val As Double = value
        If val < MinListWinWidth Then
            val = MinListWinWidth
        ElseIf val > MaxListWinWidth Then
            val = MaxListWinWidth
        End If
        Return val
    End Function

    Private Const MinListWinHeight = 100
    Private Const MaxListWinHeight = 300
    Private Const DefaultListWinHeight = 240.0
    Public Shared ReadOnly ListWindowHeightProperty As DependencyProperty = DependencyProperty.Register("ListWindowHeight", GetType(Double), GetType(CheckboxFilterList), New FrameworkPropertyMetadata(DefaultListWinHeight, Nothing, AddressOf CoerceListWindowHeight))
    ' appears in code
    ''' <summary>
    ''' Height of the FilterListWindow. (100 - 300)
    ''' </summary>
    <Description("Height of the FilterListWindow. (100 - 300)"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property ListWindowHeight As Double
        Get
            Return CType(GetValue(ListWindowHeightProperty), Double)
        End Get
        Set(ByVal value As Double)
            SetValue(ListWindowHeightProperty, value)
        End Set
    End Property

    Private Shared Function CoerceListWindowHeight(ByVal d As DependencyObject, ByVal value As Object) As Object
        Dim val As Double = value
        If val < MinListWinHeight Then
            val = MinListWinHeight
        ElseIf val > MaxListWinHeight Then
            val = MaxListWinHeight
        End If
        Return val
    End Function

#End Region

#Region "List"
    ''' <summary>
    ''' Required when the existing item list object is used, but one or more items in the list have been added or removed. 
    ''' This has the same effect as consecutively detach and attach the ItemList object to the control.
    ''' </summary>
    Public Sub ItemListUpdate()
        ItemListChanged(Me, ItemList)
    End Sub

    ' appears in code
    ''' <summary>
    ''' List of Items for Select List
    ''' </summary>
    Public Shared ReadOnly ItemListProperty As DependencyProperty = DependencyProperty.Register("ItemList", GetType(IEnumerable), GetType(CheckboxFilterList), New FrameworkPropertyMetadata(AddressOf OnItemListChanged))
    <Description("List of Items for Select List"), Category("CheckboxFilterList")>
    Public Property ItemList As IEnumerable
        Get
            Return GetValue(ItemListProperty)
        End Get
        Set(value As IEnumerable)
            SetValue(ItemListProperty, value)
        End Set
    End Property

    Private Shared Sub OnItemListChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As CheckboxFilterList = CType(d, CheckboxFilterList)
        'Dim loc As Location = CType((d.GetValue(ImageLocationProperty)), Location)
        ItemListChanged(control, args.NewValue)                 ' Input Items were changed
    End Sub

    Private Const EmptyString As String = ""
    Public Shared ReadOnly DisplayMemberProperty As DependencyProperty = DependencyProperty.Register("DisplayMember", GetType(String), GetType(CheckboxFilterList), New FrameworkPropertyMetadata(EmptyString))
    <Description("Select a Property in a class to display in Listbox. Not needed for Enum or value list."), Category("CheckboxFilterList")>
    Public Property DisplayMember As String
        Get
            Return GetValue(DisplayMemberProperty)
        End Get
        Set(value As String)
            SetValue(DisplayMemberProperty, value)
        End Set
    End Property

    Friend Shared ReadOnly IsFilteredKey As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("IsFiltered", GetType(Boolean), GetType(CheckboxFilterList), New PropertyMetadata(False))
    Public Shared ReadOnly IsFilteredProperty As DependencyProperty = IsFilteredKey.DependencyProperty
    <Description("a Filter is set"), Category("CheckboxFilterList")>
    Public ReadOnly Property IsFiltered As Boolean
        Get
            Return GetValue(IsFilteredProperty)
        End Get
    End Property

#End Region

#Region "Image"

    Private Shared urcfi = New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/Filter_x32.png")
    Private Shared CanFilterImage = New BitmapImage(urcfi)

    Private Shared urisfi = New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/FilterDropdown_x32.png")
    Private Shared IsFilteredImage = New BitmapImage(urisfi)

    Private _DoFilter As Boolean
    ''' <summary>
    ''' Determines which image is displayed on the button. True: IsFilteredImage, False: CanFilterImage
    ''' </summary>    
    Friend Property DoFilter As Boolean
        Get
            Return _DoFilter
        End Get
        Set(value As Boolean)
            If value = True Then
                _DoFilter = True
                Image = IsFilteredImage
                SetValue(IsFilteredKey, True)
            Else
                _DoFilter = False
                Image = CanFilterImage
                SetValue(IsFilteredKey, False)
            End If
        End Set
    End Property

    <Browsable(False)>
    Public Shared ReadOnly ImageProperty As DependencyProperty = DependencyProperty.Register("Image", GetType(ImageSource), GetType(CheckboxFilterList),
    New UIPropertyMetadata(CanFilterImage))
    ' appears in code
    ''' <summary>
    ''' The Image on the button
    ''' </summary>
    <Description("The Image on the button"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property Image As ImageSource
        Get
            Return CType(GetValue(ImageProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ImageMarginProperty As DependencyProperty = DependencyProperty.Register("ImageMargin", GetType(Thickness), GetType(CheckboxFilterList), New UIPropertyMetadata(New Thickness(5, 0, 5, 0)))
    ' appears in code
    ''' <summary>
    ''' Margins of the Image
    ''' </summary>
    <Description("Margins of the Image"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property ImageMargin As Thickness

        Get
            Return CType(GetValue(ImageMarginProperty), Thickness)
        End Get
        Set(ByVal value As Thickness)
            SetValue(ImageMarginProperty, value)
        End Set

    End Property

    Public Shared ReadOnly ImageLocationProperty As DependencyProperty = DependencyProperty.Register("ImageLocation", GetType(Location), GetType(CheckboxFilterList), New FrameworkPropertyMetadata(Location.Left, New PropertyChangedCallback(AddressOf OnImageLocationChanged), New CoerceValueCallback(AddressOf CoerceImageLocation)))
    <Description("Location of the Image"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property ImageLocation() As Location
        Get
            Return CType(Me.GetValue(ImageButton.ImageLocationProperty), Location)
        End Get
        Set
            Me.SetValue(ImageButton.ImageLocationProperty, Value)
        End Set
    End Property

    Private Shared Sub OnImageLocationChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As CheckboxFilterList = CType(d, CheckboxFilterList)
        Dim loc As Location = CType((d.GetValue(ImageLocationProperty)), Location)

        If loc = Location.Left Then
            control.Image1.SetValue(Grid.ColumnProperty, 0)
            control.Viewbox1.SetValue(Grid.ColumnProperty, 1)

            control.Grid2.ColumnDefinitions(1).Width = New GridLength(1, GridUnitType.Star)
            control.Grid2.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Auto)

        Else
            control.Image1.SetValue(Grid.ColumnProperty, 1)
            control.Viewbox1.SetValue(Grid.ColumnProperty, 0)

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
    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(CheckboxFilterList), TextPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Text on the button
    ''' </summary>
    <Description("The Text on the button"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property Text As String

        Get
            Return CType(GetValue(TextProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(TextProperty, value)
        End Set

    End Property


    Public Shared ReadOnly TextAlignmentProperty As DependencyProperty = DependencyProperty.Register("TextAlignment", GetType(HorizontalAlignment), GetType(CheckboxFilterList), New UIPropertyMetadata(HorizontalAlignment.Stretch))
    ' appears in code
    ''' <summary>
    ''' Horizontal alignment of the text
    ''' </summary>
    <Description("Horizontal text alignment"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Property TextAlignment As HorizontalAlignment

        Get
            Return CType(GetValue(TextAlignmentProperty), HorizontalAlignment)
        End Get
        Set(ByVal value As HorizontalAlignment)
            SetValue(TextAlignmentProperty, value)
        End Set

    End Property

    Public Shared ReadOnly TextPaddingProperty As DependencyProperty = DependencyProperty.Register("TextPadding", GetType(Thickness), GetType(CheckboxFilterList), New UIPropertyMetadata(New Thickness()))
    ' appears in code
    ''' <summary>
    ''' Padding of textblock
    ''' </summary>
    <Description("Padding of textblock"), Category("CheckboxFilterList")>   ' appears in VS property
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

    'Public Event Click As RoutedEventHandler
    'Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles Button.Click

        '--- test

        Dim pt As Point
        pt = Me.PointToScreen(New Point)

        Dim win As New FilterListWindow(Me)

        ' Fix if the control runs in a WinForms ElementHost (owner will be nothing)
        If Application.Current IsNot Nothing Then
            Dim mwin As Window = Application.Current.MainWindow
            If mwin IsNot Nothing Then
                win.Owner = Application.Current.MainWindow
            End If
        End If

        win.WindowStartupLocation = WindowStartupLocation.Manual
        win.Left = pt.X / ScreenScale
        win.Top = (pt.Y + Me.ActualHeight + 2) / ScreenScale
        win.Width = ListWindowWidth
        win.Height = ListWindowHeight
        win.Show()

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
