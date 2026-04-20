' Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
'
' Step 1a) Using this custom control in a XAML file that exists in the current project.
' Add this XmlNamespace attribute to the root element of the markup file where it is 
' to be used:
'
'     xmlns:MyNamespace="clr-namespace:DailyUserControls"
'
'
' Step 1b) Using this custom control in a XAML file that exists in a different project.
' Add this XmlNamespace attribute to the root element of the markup file where it is 
' to be used:
'
'     xmlns:MyNamespace="clr-namespace:DailyUserControls;assembly=DailyUserControls"
'
' You will also need to add a project reference from the project where the XAML file lives
' to this project and Rebuild to avoid compilation errors:
'
'     Right click on the target project in the Solution Explorer and
'     "Add Reference"->"Projects"->[Browse to and select this project]
'
'
' Step 2)
' Go ahead and use your control in the XAML file. Note that Intellisense in the
' XML editor does not currently work on custom controls and its child elements.
'
'     <MyNamespace:TreeViewItemX/>
'

Imports System.ComponentModel


Public Class TreeViewItemX
    Inherits TreeViewItem

    Private Shared DefaultMouseOverBackgroundBrush As Brush = New SolidColorBrush(Colors.Azure)

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(GetType(TreeViewItemX), New FrameworkPropertyMetadata(GetType(TreeViewItemX)))
    End Sub

    Public Shared ReadOnly ImageProperty As DependencyProperty = DependencyProperty.Register("Image", GetType(ImageSource), GetType(TreeViewItemX), New UIPropertyMetadata)
    ' appears in code
    ''' <summary>
    ''' The Image on the button
    ''' </summary>
    <Description("TreeViewItem Image"), Category("TreeViewItemX")>   ' appears in VS property
    Public Property Image As ImageSource
        Get
            Return CType(GetValue(ImageProperty), ImageSource)
        End Get
        Set(ByVal value As ImageSource)
            SetValue(ImageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ImageMaxHeightProperty As DependencyProperty = DependencyProperty.Register("ImageMaxHeight", GetType(Double), GetType(TreeViewItemX), New UIPropertyMetadata(14.0))
    ' appears in code
    ''' <summary>
    ''' The Image on the button
    ''' </summary>
    <Description("TreeViewItem Image MaxHeight"), Category("TreeViewItemX")>   ' appears in VS property
    Public Property ImageMaxHeight As Double
        Get
            Return CType(GetValue(ImageMaxHeightProperty), Double)
        End Get
        Set(ByVal value As Double)
            SetValue(ImageProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SelectedBackgroundProperty As DependencyProperty = DependencyProperty.Register("SelectedBackground",
           GetType(SolidColorBrush), GetType(TreeViewItemX),
           New FrameworkPropertyMetadata(SystemColors.HighlightBrush))

    <Description("Background brush when item is selected")>   ' appears in VS property
    Public Property SelectedBackground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(SelectedBackgroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(SelectedBackgroundProperty, Value)
        End Set
    End Property

    Public Shared ReadOnly SelectedForegroundProperty As DependencyProperty = DependencyProperty.Register("SelectedForeground",
           GetType(SolidColorBrush), GetType(TreeViewItemX),
           New FrameworkPropertyMetadata(SystemColors.HighlightTextBrush))

    <Description("Foreground brush when item is selected")>   ' appears in VS property
    Public Property SelectedForeground() As SolidColorBrush
        Get
            Return CType(Me.GetValue(SelectedForegroundProperty), SolidColorBrush)
        End Get
        Set
            Me.SetValue(SelectedForegroundProperty, Value)
        End Set
    End Property

    Public Shared ReadOnly MouseOverBackgroundProperty As DependencyProperty = DependencyProperty.Register("MouseOverBackground",
            GetType(Brush), GetType(TreeViewItemX),
            New FrameworkPropertyMetadata(DefaultMouseOverBackgroundBrush))
    ' appears in code
    ''' <summary>
    ''' Background Brush when Mouse is over the header or the Image, except when Item IsSelected
    ''' </summary>
    <Description("Background Brush when Mouse is over the the header or the Image, except when Item IsSelected")>   ' appears in VS property
    Public Overloads Property MouseOverBackground As Brush
        Get
            Return CType(GetValue(MouseOverBackgroundProperty), Brush)
        End Get
        Set(ByVal value As Brush)
            SetValue(MouseOverBackgroundProperty, value)
        End Set
    End Property

#Region "Render"

    Private TemplateBd As Border
    Private ConnectingLineBrush As Brush = New SolidColorBrush(Color.FromArgb(&HFF, &H5D, &H5D, &H5D))
    Private ConnectingLinePen As New Pen(ConnectingLineBrush, 1.0)   ' With {.DashStyle = DashStyles.Dot}

    Protected Overrides Sub OnRender(dc As DrawingContext)
        MyBase.OnRender(dc)

        '--- connectig lines ---

        ' RenderOptions.SetEdgeMode(Me, EdgeMode.Aliased)         ' sharp connecting lines
        ' Different thicknesses at high dpi / scaled screens with EdgeMode.Aliased

        Dim posx As Double = 8
        Dim posy As Double = 1

        TemplateBd = TryCast(Me.Template.FindName("Bd", Me), Border)
        If TemplateBd IsNot Nothing Then
            posy = TemplateBd.ActualHeight / 2
        End If

        '--- horizontal line ---

        dc.DrawLine(ConnectingLinePen, New Point(posx, posy), New Point(19, posy))

        '--- vertical line ---

        Dim ic As ItemsControl = ItemsControl.ItemsControlFromItemContainer(Me)
        If ic Is Nothing Then Exit Sub              ' when 1 TreeViewItemX is inserted outside a TreeView
        Dim ndx = ic.ItemContainerGenerator.IndexFromContainer(Me)

        Dim iccount = ic.Items.Count
        Dim islast As Boolean

        If ndx = iccount - 1 Then islast = True

        If islast = True Then
            If HasItems = True Then
                If IsRootItem() = True Then
                    '--- no v-line for root-item ---
                Else
                    '--- last item with node-box, v-line from top to node-box ---                    
                    dc.DrawLine(ConnectingLinePen, New Point(posx, 0), New Point(posx, posy))
                End If
            Else
                '--- last sub-item, v-line in the upper half ---                
                dc.DrawLine(ConnectingLinePen, New Point(posx, 0), New Point(posx, ActualHeight / 2))
            End If
        Else
            '--- common sub-item with simple v-line ---
            dc.DrawLine(ConnectingLinePen, New Point(posx, 0), New Point(posx, ActualHeight))
        End If

    End Sub

    Private Function IsRootItem() As Boolean
        If Parent Is Nothing Then Return False
        If Parent.GetType Is GetType(Controls.TreeView) Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
