Public Class FilterListWindow
    Private control As CheckboxFilterList

    Private SelectedItemsBefore As New List(Of Object)

    Public Sub New(caller As CheckboxFilterList)

        ' required for the designer
        InitializeComponent()

        control = caller

        ListBox1.ItemsSource = control.AllItems
        ' this will raise SelectionChanged several times
        ' it is not guaranteened that all Items are added to the Listbox at this moment,
        ' SelectionChanged can also occur afterWindow_Loaded (when scrolling Listbox down)

        SelectedItemsBefore.Clear()

        For Each item In control.AllItems
            If item.IsChecked = True Then
                SelectedItemsBefore.Add(item.Value)       ' add object
            End If
        Next

    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim lb = ListBox1

    End Sub
    Private Sub Window_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        ' there should be also a way to leave the window with the keyboard
        ' when the visibility is set to Hidden, Window_Deactivated is called
        If e.Key = Key.Escape Then
            Me.Visibility = Visibility.Hidden
        End If
    End Sub
    Private Sub Window_Deactivated(sender As Object, e As EventArgs)

        Dim selcount As Integer

        For Each item In control.AllItems
            If item.IsChecked = True Then selcount += 1
        Next

        If selcount < control.AllItems.Count Then
            control.DoFilter = True
        Else
            control.DoFilter = False
        End If


        Dim selchanged As Boolean = WasSelectionChanged()

        If selchanged = True Then
            ' create SelectedItems List
            control.SelectedItems.Clear()

            For Each item In control.AllItems
                If item.IsChecked = True Then
                    control.SelectedItems.Add(item.Value)
                End If
            Next

            ' set ShortCut Flags
            If control.SelectedItems.Count = control.AllItems.Count Then
                control.SetSelectedAll(True)
            Else
                control.SetSelectedAll(False)
            End If

            If control.SelectedItems.Count = 0 Then
                control.SetSelectedNone(True)
            Else
                control.SetSelectedNone(False)
            End If

        End If

        '--- nice in some cases, but here it shows the ListWindow again when the Button is pressed again
        'Me.Visibility = Visibility.Hidden
        Close()

        If selchanged = True Then
            ' inform caller about update (after close)
            control.RaiseSelectionChangedEvent()
        End If

        If Owner IsNot Nothing Then
            Owner.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, New SetFocusDelegate(AddressOf MySetFocus))
        End If

    End Sub


    Public Delegate Sub SetFocusDelegate()

    Public Sub MySetFocus()
        If Owner IsNot Nothing Then
            Owner.Activate()
            Owner.Focus()
        End If
    End Sub

    Private Function WasSelectionChanged() As Boolean
        Dim selcount As Integer

        For Each item In control.AllItems
            If item.IsChecked = True Then selcount += 1
        Next

        If SelectedItemsBefore.Count <> selcount Then Return True

        For Each item In control.AllItems
            If item.IsChecked Then
                If SelectedItemsBefore.Contains(item.Value) = False Then Return True
            End If
        Next

        Return False
    End Function


    Private Sub btnAllNone_Click(sender As Object, e As RoutedEventArgs) Handles btnAllNone.Click

        Dim allcount As Integer = control.AllItems.Count
        Dim selcount As Integer

        For Each item In control.AllItems
            If item.IsChecked = True Then selcount += 1
        Next

        If selcount = allcount Then
            ' all selected -> select none
            For Each item In control.AllItems
                item.IsChecked = False
            Next
        Else
            ' none or some selected -> select all
            For Each item In control.AllItems
                item.IsChecked = True
            Next

        End If


    End Sub
End Class
