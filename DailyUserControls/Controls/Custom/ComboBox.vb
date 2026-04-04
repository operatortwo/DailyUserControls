Public Class ComboBox
    Inherits Controls.ComboBox

    Shared Sub New()
        ' get existing (info)
        Dim bgMet As PropertyMetadata = BackgroundProperty.GetMetadata(GetType(ComboBox))
        Dim bbMet As PropertyMetadata = BorderBrushProperty.GetMetadata(GetType(ComboBox))

        ' can not overwrite(add) PropertyMetadata directly
        ' WPF will merge the PropertyMetadata, no need to pass the original Metadata object
        ' must be appropriate type (FrameworkPropertyMetadata not PropertyMetadata,..)
        ' Calls to OverrideMetadata should only be performed within the static constructors ...
        BackgroundProperty.OverrideMetadata(GetType(ComboBox), New FrameworkPropertyMetadata(New PropertyChangedCallback(AddressOf OnBackgroundChanged)))
        BorderBrushProperty.OverrideMetadata(GetType(ComboBox), New FrameworkPropertyMetadata(New PropertyChangedCallback(AddressOf OnBorderBrushChanged)))

    End Sub

    Private Sub CmbLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles Me.Loaded
        If Me.Template IsNot Nothing Then
            Dim tgbtn As Primitives.ToggleButton = TryCast(Me.Template.FindName("toggleButton", Me), Primitives.ToggleButton)
            If tgbtn IsNot Nothing Then
                Dim cnt As Integer = VisualTreeHelper.GetChildrenCount(tgbtn)
                If cnt > 0 Then
                    Dim obj = VisualTreeHelper.GetChild(tgbtn, 0)
                    Dim bor As Border = TryCast(obj, Border)
                    If bor IsNot Nothing Then
                        bor.Background = tgbtn.Background
                        bor.BorderBrush = tgbtn.BorderBrush
                    End If
                End If
            End If
        End If
    End Sub

    ' It looks like we have
    ' <ToggleButton/>
    ' <Border/>
    ' and Togglebutton is bound to ComboBox Background, but border is set to Static grey Color, an paints over it.
    ' In the fix above we assign Border.Background and Border.BorderBrush to the ToggleButton values.

    ' The -Changed Callbacks are mainly needed to update the control during design-time.
    Private Shared Sub OnBackgroundChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As ComboBox = CType(d, ComboBox)
        control.CmbLoaded(control, New RoutedEventArgs)
    End Sub

    Private Shared Sub OnBorderBrushChanged(ByVal d As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        Dim control As ComboBox = CType(d, ComboBox)
        control.CmbLoaded(control, New RoutedEventArgs)
    End Sub

End Class
