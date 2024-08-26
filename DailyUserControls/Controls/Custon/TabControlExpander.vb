' Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
'
' Step 1b) Using this custom control in a XAML file that exists in a different project.
' Add this XmlNamespace attribute to the root element of the markup file where it is 
' to be used:
'
'     xmlns:MyNamespace="clr-namespace:DailyUserControls;assembly=DailyUserControls"
'
' Step 2)
' Go ahead and use your control in the XAML file. Note that Intellisense in the
' XML editor does not currently work on custom controls and its child elements.
'
'     <MyNamespace:TabControlExpander/>
'

Imports System.ComponentModel
Imports System.Windows.Controls.Primitives


Public Class TabControlExpander
    Inherits TabControl

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(GetType(TabControlExpander), new FrameworkPropertyMetadata(GetType(TabControlExpander)))
    End Sub

    Private HeaderPanel As TabPanel
    Private ContentHost As New ContentPresenter
    Private TgBtnCollapse As Primitives.ToggleButton
    Private CollapseWhenNotFocused As Boolean
    Private InitialHeight As Double

    Private HelpButton As Button

    Private Sub TcInitialized() Handles Me.Initialized
        InitialHeight = Me.Height
    End Sub

    Private Sub TcLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' _Loaded can be called more than once, f.e. when switching between TabItems in a parent TabControl
        ' AddHandler / RemoveHandler should be balanced in Loaded/Unladed to prevent multiple Handlers
        ' and multiple EventHandler calls

        ' alternative to find elements:
        ' Dim tgbtn As ToggleButton = Me.GetTemplateChild("tgbtnCollapse")

        ' qualify ToggleButton, else DailyUserControls.ToggleButton is used

        TgBtnCollapse = TryCast(Me.Template.FindName("tgbtnCollapse", Me), Primitives.ToggleButton)
        HeaderPanel = TryCast(Me.Template.FindName("headerPanel", Me), TabPanel)
        ContentHost = TryCast(Me.Template.FindName("PART_SelectedContentHost", Me), ContentPresenter)

        If TgBtnCollapse IsNot Nothing Then
            AddHandler TgBtnCollapse.Checked, AddressOf TgBtnCollapse_checked
            AddHandler TgBtnCollapse.Unchecked, AddressOf TgBtnCollapse_unchecked
        End If

        '--- Help Button ---
        HelpButton = TryCast(Me.Template.FindName("HelpButton", Me), Button)
        If HelpButton IsNot Nothing Then
            AddHandler HelpButton.Click, AddressOf HelpButton_Click
        End If
    End Sub

    Private Sub TcUnloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        If TgBtnCollapse IsNot Nothing Then
            RemoveHandler TgBtnCollapse.Checked, AddressOf TgBtnCollapse_checked
            RemoveHandler TgBtnCollapse.Unchecked, AddressOf TgBtnCollapse_unchecked
        End If
        '--- Help Button ---
        If HelpButton IsNot Nothing Then
            RemoveHandler HelpButton.Click, AddressOf HelpButton_Click
        End If
    End Sub

    Private Sub TgBtnCollapse_checked(sender As Object, e As RoutedEventArgs)
        CollapseWhenNotFocused = True
        Collapse()
    End Sub

    Private Sub TgBtnCollapse_unchecked(sender As Object, e As RoutedEventArgs)
        CollapseWhenNotFocused = False
        Expand()
    End Sub

    Private Sub Collapse()
        ContentHost.Visibility = Visibility.Collapsed
        If HeaderPanel IsNot Nothing Then
            Me.Height = HeaderPanel.Height      ' NaN (Auto)
        End If
    End Sub

    Private Sub Expand()
        ContentHost.Visibility = Visibility.Visible
        Me.Height = InitialHeight
    End Sub

    Private Sub TcGotFocus(sender As Object, e As RoutedEventArgs) Handles Me.GotFocus
        If CollapseWhenNotFocused = True Then
            Expand()
        End If
    End Sub

    Private Sub TcLostFocus(sender As Object, e As RoutedEventArgs) Handles Me.LostFocus
        If CollapseWhenNotFocused = True Then
            Collapse()
        End If
    End Sub

    <Description("Occurs when the help button on the header panel was clicked")>
    Public Event HelpButtonClick As RoutedEventHandler
    Private Sub HelpButton_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent HelpButtonClick(sender, e)
    End Sub

#Region "Appearance"

    Public Shared ReadOnly HeaderPanelBackgroundProperty As DependencyProperty = DependencyProperty.Register("HeaderPanelBackground", GetType(Brush), GetType(TabControlExpander), New UIPropertyMetadata())
    ' appears in code
    ''' <summary>
    ''' Background brush when checked
    ''' </summary>    
    <Description("Background of the entire header panel")>   ' appears in VS property
    Public Property HeaderPanelBackground As Brush
        Get
            Return CType(GetValue(HeaderPanelBackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(HeaderPanelBackgroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly HelpButtonVisibilityProperty As DependencyProperty = DependencyProperty.Register("HelpButtonVisibility", GetType(Visibility), GetType(TabControlExpander), New UIPropertyMetadata(Visibility.Hidden))
    ' appears in code
    ''' <summary>
    ''' Background brush when checked
    ''' </summary>    
    <Description("Visibility of the Help button on the right side of the header panel"), Category("TabControlExpander")>   ' appears in VS property
    Public Property HelpButtonVisibility As Visibility
        Get
            Return CType(GetValue(HelpButtonVisibilityProperty), Visibility)
        End Get
        Set(ByVal value As Visibility)
            SetValue(HelpButtonVisibilityProperty, value)
        End Set
    End Property

#End Region

End Class
