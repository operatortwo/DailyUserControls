Public Class QuestionWindow

    Private Shared ReturnCode As QuestionWindowResult
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        btnYes.Focus()
        ReturnCode = QuestionWindowResult.Cancel            ' preset for closing the window with 'x' or alt+F4
    End Sub
    ''' <summary>
    ''' Simple question.
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <param name="message"></param>
    ''' <returns></returns>
    Public Overloads Shared Function Show(owner As Window, message As String) As QuestionWindowResult
        Dim dlg As New QuestionWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.btnCancel.Visibility = Visibility.Collapsed
        dlg.btnNo.IsCancel = True
        dlg.ShowDialog()
        Return ReturnCode
    End Function
    ''' <summary>
    ''' Simple question with caption.
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <returns></returns>
    Public Overloads Shared Function Show(owner As Window, message As String, caption As String) As QuestionWindowResult
        Dim dlg As New QuestionWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.btnCancel.Visibility = Visibility.Collapsed
        dlg.btnNo.IsCancel = True
        dlg.ShowDialog()
        Return ReturnCode
    End Function

    ''' <summary>
    ''' Question with caption and selectable buttons.
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <param name="buttons"></param>
    ''' <returns></returns>
    Public Overloads Shared Function Show(owner As Window, message As String, caption As String, buttons As QuestionWindowButton) As QuestionWindowResult
        Dim dlg As New QuestionWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        SetButtons(dlg, buttons)
        dlg.ShowDialog()
        Return ReturnCode
    End Function

    ''' <summary>
    ''' Question with caption, selectable buttons and background brush.
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <param name="buttons"></param>
    ''' <returns></returns>
    Public Overloads Shared Function Show(owner As Window, message As String, caption As String, buttons As QuestionWindowButton, background As Brush) As QuestionWindowResult
        Dim dlg As New QuestionWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Grid1.Background = background
        SetButtons(dlg, buttons)
        dlg.ShowDialog()
        Return ReturnCode
    End Function

    ''' <summary>
    ''' Question with caption, selectable buttons, background brush and font size.
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <param name="message"></param>
    ''' <param name="caption"></param>
    ''' <param name="buttons"></param>
    ''' <returns></returns>
    Public Overloads Shared Function Show(owner As Window, message As String, caption As String, buttons As QuestionWindowButton, background As Brush, FontSize As Double) As QuestionWindowResult
        Dim dlg As New QuestionWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Grid1.Background = background
        SetButtons(dlg, buttons)
        SetFontSize(dlg, FontSize)
        dlg.ShowDialog()
        Return ReturnCode
    End Function


    Private Shared Sub SetButtons(dlg As QuestionWindow, button As QuestionWindowButton)
        If button = QuestionWindowButton.YesNo Then
            dlg.btnCancel.Visibility = Visibility.Collapsed
            dlg.btnNo.IsCancel = True
        ElseIf button = QuestionWindowButton.YesNoCancel Then

        ElseIf button = QuestionWindowButton.OkCancel Then
            dlg.btnYes.Text = "Ok"
            dlg.btnCancel.Visibility = Visibility.Collapsed
            dlg.btnNo.IsCancel = True
        End If
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dlg"></param>
    ''' <param name="desiredSize">between 10 and 20, else the default size is set</param>
    Private Shared Sub SetFontSize(dlg As QuestionWindow, desiredSize As Double)
        If desiredSize = 0 Then Exit Sub            ' use the default size (SystemFonts.MessageFontSize)

        If desiredSize < 10 Then
            desiredSize = 10                              ' minimal size
        ElseIf desiredSize > 20 Then
            desiredSize = 20                              ' maximal size
        End If

        dlg.txblkMessage.FontSize = desiredSize
    End Sub

    Private Sub btnYes_Click(sender As Object, e As RoutedEventArgs) Handles btnYes.Click
        ReturnCode = QuestionWindowResult.Yes
        Close()
    End Sub

    Private Sub btnNo_Click(sender As Object, e As RoutedEventArgs) Handles btnNo.Click
        ReturnCode = QuestionWindowResult.No
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs) Handles btnCancel.Click
        ReturnCode = QuestionWindowResult.Cancel
        Close()
    End Sub
End Class

Public Enum QuestionWindowButton
    YesNo
    YesNoCancel
    OkCancel
End Enum

Public Enum QuestionWindowResult
    Yes
    No
    Cancel
End Enum