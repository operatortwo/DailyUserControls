Class MainWindow
    Public Property WindowRed1 As New WindowRed             ' New is required for SidePanel initialisation
    Public Property WindowGreen1 As New WindowGreen
    Public Property WindowBlue1 As New WindowBlue
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' set owner, else Application is still running after MainWin was closed        
        WindowRed1.Owner = Me
        WindowGreen1.Owner = Me
        WindowBlue1.Owner = Me
    End Sub

    Private Sub btnCloseOpenWindow_Click(sender As Object, e As RoutedEventArgs) Handles btnCloseOpenWindow.Click
        sdp1.CloseOpenWindow()
    End Sub

    Private Sub btnRedWindow_Click(sender As Object, e As RoutedEventArgs) Handles btnRedWindow.Click
        sdp1.OpenOrActivateWindow("Red Win")
    End Sub

    Private Sub btnGreenWindow_Click(sender As Object, e As RoutedEventArgs) Handles btnGreenWindow.Click
        sdp1.OpenOrActivateWindow("Green Win")
    End Sub

    Private Sub btnBlueWindow_Click(sender As Object, e As RoutedEventArgs) Handles btnBlueWindow.Click
        sdp1.OpenOrActivateWindow("Blue Window")
    End Sub
End Class
