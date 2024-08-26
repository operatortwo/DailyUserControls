Imports System.Collections.ObjectModel
Imports System.ComponentModel

Partial Public Class CheckboxFilterList
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    'Public Event SelectionChanged As RoutedEventHandler

    Friend Property AllItems As New List(Of ListItemClass)
    Public ReadOnly Property SelectedItems As New ObservableCollection(Of Object)
    Public ReadOnly Property SelectedAll As Boolean         ' use SetSelectedAll to change
    Public ReadOnly Property SelectedNone As Boolean        ' use SetSelectedNone to change

    ''' <summary>
    ''' The base list was attached or removed from the control
    ''' </summary>
    ''' <param name="NewValue"></param>
    Private Shared Sub ItemListChanged(control As CheckboxFilterList, NewValue As Object)

        '--- remove old items, if any
        control.AllItems.Clear()
        control.SelectedItems.Clear()

        control.DoFilter = False            ' reset Image on the Button to 'not Filtered' (all selected)

        If NewValue Is Nothing Then Exit Sub                        ' exit when NewValue is Nothing

        Dim nv As IEnumerable = TryCast(NewValue, IEnumerable)
        If nv Is Nothing Then Exit Sub                              ' exit when TryCast failed (not IEnumerable)
        Dim enu As IEnumerator = nv.GetEnumerator
        If enu.MoveNext = False Then Exit Sub           ' exit when count = 0, else the next codeline fails

        ' get item Type
        Dim tp As Type = NewValue(0).GetType()

        If tp.IsClass = False Then
            ' for Enum and Value Type
            For Each item In NewValue
                control.AllItems.Add(New ListItemClass With {.Name = item.ToString, .Value = item})
                control.SelectedItems.Add(item)
            Next
        Else
            ' IsClass
            If control.DisplayMember <> "" Then
                Dim dm = tp.GetProperty(control.DisplayMember)
                If dm IsNot Nothing Then
                    For Each item In NewValue
                        control.AllItems.Add(New ListItemClass With {
                                             .Name = CallByName(item, control.DisplayMember, CallType.Get),
                                             .Value = item})
                        control.SelectedItems.Add(item)
                    Next
                Else
                    For Each item In NewValue
                        control.AllItems.Add(New ListItemClass With {.Name = item.ToString, .Value = item})
                        control.SelectedItems.Add(item)
                    Next
                End If
            End If
        End If

        control.SetSelectedAll(True)
        control.SetSelectedNone(False)

    End Sub


    Public Class ListItemClass
        Implements INotifyPropertyChanged

        ' DisplayMemberPath and SelectedValuePath need property not field
        Public Property Name As String
        Public Property Value As Object
        Public Property IsEnabled As Boolean = True
        Private _IsChecked As Boolean = True
        Public Property IsChecked As Boolean
            Get
                Return _IsChecked
            End Get
            Set(value As Boolean)
                If Not (value = _IsChecked) Then
                    Me._IsChecked = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsChecked"))
                End If

            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

    Friend Sub SetSelectedAll(value As Boolean)
        If value <> SelectedAll Then
            _SelectedAll = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SelectedAll"))
        End If
    End Sub
    Friend Sub SetSelectedNone(value As Boolean)
        If value <> SelectedNone Then
            _SelectedNone = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SelectedNone"))
        End If
    End Sub


    Public Shared ReadOnly SelectionChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, GetType(RoutedEventHandler), GetType(CheckboxFilterList))

    ' Provide CLR accessors for the event
    <Description("Occurs when the user selected/deselected one ore more items in the List"), Category("CheckboxFilterList")>   ' appears in VS property
    Public Custom Event SelectionChanged As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(SelectionChangedEvent, value)
        End AddHandler

        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(SelectionChangedEvent, value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    ' This method raises the SeelectionChanged event
    Friend Sub RaiseSelectionChangedEvent()
        Dim newEventArgs As New RoutedEventArgs(SelectionChangedEvent)
        MyBase.RaiseEvent(newEventArgs)
    End Sub

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


End Class
