Imports System.Windows.Markup

<ContentProperty("Children")>
Public Class SidePanel

    Public Sub New()

        ' This call is required for the Designer.
        InitializeComponent()

        Children = StackPanel1.Children

        HorizontalAlignment = HorizontalAlignment.Left

    End Sub


    Private Sub userControl_Loaded(sender As Object, e As RoutedEventArgs) Handles userControl.Loaded

        If ParentWindow Is Nothing Then
            ParentWindow = Window.GetWindow(Me)
            Initialize_SidePanel()
        End If

        ' _Loaded can be called more than once, f.e. when returning from another TabItem in a TabControl
        ' Checking 'ParentWindow Is Nothing' prevents that 'Initialize_SidePanel()' is only called once
        ' and prevents Handlers are added more than once (f.e. 'SidePanelButton_Clicked')


    End Sub

#Region "Properties"

    Private Shared ReadOnly DefaultBackgroundBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HCC, &HD5, &HF0))

    Public Shared ReadOnly SdPnl_BackgroundProperty As DependencyProperty = DependencyProperty.Register("Background", GetType(Brush), GetType(SidePanel), New UIPropertyMetadata(DefaultBackgroundBrush))

    Public Overloads Property Background As Brush
        Get
            Return CType(GetValue(SdPnl_BackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(SdPnl_BackgroundProperty, value)
        End Set
    End Property


    Public Shared ReadOnly ChildrenProperty As DependencyPropertyKey = DependencyProperty.RegisterReadOnly("Children", GetType(UIElementCollection), GetType(SidePanel), New PropertyMetadata())

    Public Property Children() As UIElementCollection
        Get
            Return CType(GetValue(ChildrenProperty.DependencyProperty), UIElementCollection)
        End Get
        Private Set
            SetValue(ChildrenProperty, Value)
        End Set
    End Property

#End Region

#Region "Code"

    Private ParentWindow As Window
    Private LastWindowPosition As New Point
    Private ButtonBackground_Active As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HED, &HF0, &HCC))
    Private ButtonBackground_Inactive As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &HCC, &HD5, &HF0))

    Private SidePanelItems As New List(Of SidePanelItem)

    Private Class SidePanelItem
        Public SidePanelButton As SidePanelButton
        Public AssociatedWindow As Window
    End Class

    Private Sub Initialize_SidePanel()

        If ParentWindow IsNot Nothing Then
            AddHandler ParentWindow.LocationChanged, AddressOf ParentWindow_LocationChanged
            AddHandler ParentWindow.SizeChanged, AddressOf ParentWindow_SizeChanged
        End If

        '--- create ListOf SidePanelItems

        Dim assem As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()

        SidePanelItems.Clear()      ' in case of repeated call, f.e. return from other TabItem: UC is loaded again!

        For Each element In Me.Children
            If TypeOf element Is SidePanelButton Then
                Dim btn As SidePanelButton = CType(element, SidePanelButton)
                Dim item As New SidePanelItem
                item.SidePanelButton = btn

                Dim win As Window = TryCast(btn.AssociatedWindow, Window)
                If win IsNot Nothing Then

                    Dim spi As New SidePanelItem
                    spi.SidePanelButton = btn
                    spi.AssociatedWindow = win
                    SidePanelItems.Add(spi)

                    AddHandler btn.Click, AddressOf SidePanelButton_Clicked
                End If
            End If
        Next

    End Sub

    Private Sub ParentWindow_LocationChanged(sender As Object, e As EventArgs)
        Dim newpos As New Point With {.X = ParentWindow.Left(), .Y = ParentWindow.Top}
        Dim pdiff As New Point

        pdiff.X = LastWindowPosition.X - ParentWindow.Left()
        pdiff.Y = LastWindowPosition.Y - ParentWindow.Top

        For Each item In SidePanelItems
            If IsWindowOpen(item.AssociatedWindow) Then
                item.AssociatedWindow.Left -= pdiff.X
                item.AssociatedWindow.Top -= pdiff.Y

                LastWindowPosition.X = ParentWindow.Left()
                LastWindowPosition.Y = ParentWindow.Top
                Exit For
            End If
        Next
    End Sub

    Private Sub ParentWindow_SizeChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub SidePanelButton_Clicked(sender As Object, e As RoutedEventArgs)
        'Dim btn As Button
        'btn = TryCast(sender, Button)
        Dim btn As SidePanelButton
        btn = TryCast(sender, SidePanelButton)

        SidePanelButton_Function(btn)

    End Sub

    ''' <summary>
    ''' Close Associated Window if open, else: close all other Associated Windows, open this Associated Window.
    ''' </summary>
    ''' <param name="btn"></param>
    Private Sub SidePanelButton_Function(btn As SidePanelButton)
        If btn Is Nothing Then Exit Sub

        Dim spi As SidePanelItem
        spi = SidePanelItems.Find(Function(x) x.SidePanelButton Is btn)

        If IsWindowOpen(spi.AssociatedWindow) Then
            spi.AssociatedWindow.Close()
        Else
            ' else: close all other windows, open this window

            For Each item In SidePanelItems
                If IsWindowOpen(item.AssociatedWindow) Then
                    item.AssociatedWindow.Close()
                End If
            Next

            '-- create + show associated window --
            '
            'btn.Background = ButtonBackground_Active
            btn.Background = btn.BackgroundActive

            LastWindowPosition.X = ParentWindow.Left()
            LastWindowPosition.Y = ParentWindow.Top

            Dim pt As Point
            pt = Me.PointToScreen(New Point)
            'pt = ParentWindow.PointToScreen(New Point)

            Dim tp As Type
            tp = spi.AssociatedWindow.GetType
            'tp.assembly.definedTypes / FullName

            Dim mywin As Window
            'Dim myass As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()
            'mywin = CType(myass.CreateInstance(myass.GetName().Name + "." + tp.Name), Window)

            mywin = CType(tp.Assembly.CreateInstance(tp.FullName), Window)

            'Dim obj As Object = Me
            Dim scale As Double = GetScreenScale(ParentWindow)

            ' -> pt1 = PointToScreen(0,0)  pt2 = PointToScreen(100,100)  ScaleX = pt2.X - pt1.X

            With mywin
                .Left = (pt.X + Me.ActualWidth) / scale
                .Top = pt.Y / scale
                .Owner = ParentWindow
            End With

            AddHandler mywin.Closed, AddressOf SidePanelWindow_Closed

            spi.AssociatedWindow = mywin
            spi.AssociatedWindow.Show()
        End If

    End Sub


    Private Sub SidePanelWindow_LostFocus(sender As Object, e As RoutedEventArgs)
        Dim win As Window
        win = TryCast(sender, Window)
        If win Is Nothing Then Exit Sub
        'win.Close()
    End Sub

    Private Sub SidePanelWindow_Deactivated(sender As Object, e As EventArgs)
        Dim win As Window
        win = TryCast(sender, Window)
        If win Is Nothing Then Exit Sub
        If win.IsVisible = False Then Exit Sub          ' closing

        'win.Close()

        Dim own As Window
        own = win.Owner

        own.Topmost = True
        Dim unused = own.Focus()
        own.Activate()

    End Sub

    Private Sub SidePanelWindow_Closed(sender As Object, e As EventArgs)
        Dim win As Window
        win = TryCast(sender, Window)
        If win IsNot Nothing Then
            Dim own As Window
            own = win.Owner
            'own.Topmost = True
            'own.Focus()
            'own.Activate()

            Dim spi As SidePanelItem
            spi = SidePanelItems.Find(Function(x) x.AssociatedWindow Is win)
            spi.SidePanelButton.Background = spi.SidePanelButton.BackgroundInactive
        End If
    End Sub

    ''' <summary>
    ''' Use Button Content (Text on Button) to find the associated Window
    ''' </summary>
    ''' <param name="ButtonContent"></param>
    Public Sub OpenOrActivateWindow(ButtonContent As String)
        For Each item In SidePanelItems

            If CStr(item.SidePanelButton.Content) = ButtonContent Then
                If IsWindowOpen(item.AssociatedWindow) Then
                    item.AssociatedWindow.Activate()
                    Exit For                                ' job is done
                Else
                    SidePanelButton_Function(item.SidePanelButton)
                    Exit For                                ' job is done
                End If
            End If
        Next
    End Sub


    ''' <summary>
    ''' Check associated Windows and close it if any of them is open
    ''' </summary>
    Public Sub CloseOpenWindow()
        For Each item In SidePanelItems
            If IsWindowOpen(item.AssociatedWindow) Then
                item.AssociatedWindow.Close()
            End If
        Next
    End Sub

    Private Function IsWindowOpen(win As Window) As Boolean
        If IsNothing(win) OrElse win.IsLoaded = False Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function GetScreenScale(visual As Visual) As Double
        ' assuming ScaleX = ScaleY

        Dim scale As Double
        Try
            Dim pt1 As Point
            Dim pt2 As Point

            pt1 = visual.PointToScreen(New Point(0, 0))
            pt2 = visual.PointToScreen(New Point(100, 100))
            scale = (pt2.X - pt1.X) / 100
        Catch
            Return 1.0                  ' in case of invalid visual
        End Try

        If scale <= 0 Then
            scale = 1                   ' reset to default in case of invalid value
        End If

        Return scale
    End Function

#End Region

End Class
