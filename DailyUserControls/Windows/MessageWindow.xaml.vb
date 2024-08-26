Public Class MessageWindow

    Private Shared uri_Error As New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/MessageWindow/Error_x32.png")
    Private Shared uri_Information As New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/MessageWindow/Information_x32.png")
    Private Shared uri_StatusOk As New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/MessageWindow/StatusOk_x32.png")
    Private Shared uri_Warning As New Uri("pack://application:,,,/DailyUserControls;component/Resources/Images/MessageWindow/Warning_x32.png")

    'Private ImgUriBase As String = "pack://application:,,,/DailyUserControls;component/Resources/Images/"

    'Private ImgSubUriError As String = "MessageWindow/Error_x32.png"
    'Private ImgSubUriInfo As String = "MessageWindow/Information_x32.png"
    'Private ImgSubUriStatOk As String = "MessageWindow/StatusOk_x32.png"
    'Private ImgSubUriWarnimg As String = "MessageWindow/Warning_x32.png"

    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>   
    ''' <param name="FontSize">Must be between 10 and 20</param>
    Public Overloads Shared Sub Show(owner As Window, message As String, caption As String, icon As MessageIcon, background As Brush, FontSize As Double)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Grid1.Background = background
        SetIcon(dlg, icon)
        SetFontSize(dlg, FontSize)
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(owner As Window, message As String, caption As String, icon As MessageIcon, background As Brush)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Grid1.Background = background
        SetIcon(dlg, icon)
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>   
    ''' <param name="FontSize">Must be between 10 and 20</param>
    Public Overloads Shared Sub Show(owner As Window, message As String, caption As String, icon As MessageIcon, FontSize As Double)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        SetIcon(dlg, icon)
        SetFontSize(dlg, FontSize)
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(owner As Window, message As String, caption As String, icon As MessageIcon)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner       
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        SetIcon(dlg, icon)
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(owner As Window, message As String, icon As MessageIcon)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner
        dlg.txblkMessage.Text = message
        SetIcon(dlg, icon)
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(owner As Window, message As String, caption As String)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Image1.Source = Nothing                                     ' no Icon
        dlg.Grid1.ColumnDefinitions(0).Width = New GridLength(20)       ' resize column 0
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(owner As Window, message As String)
        Dim dlg As New MessageWindow
        dlg.Owner = owner                                               ' Startup Location: center owner
        dlg.txblkMessage.Text = message
        dlg.Image1.Source = Nothing                                     ' no Icon
        dlg.Grid1.ColumnDefinitions(0).Width = New GridLength(20)       ' resize column 0
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(message As String, caption As String)
        Dim dlg As New MessageWindow
        dlg.Owner = Windows.Application.Current.MainWindow
        dlg.txblkMessage.Text = message
        dlg.Title = caption
        dlg.Image1.Source = Nothing                                     ' no Icon
        dlg.Grid1.ColumnDefinitions(0).Width = New GridLength(20)       ' resize column 0
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Opens a dialog window and waits until the user presses ESC or ENTER or closes the window.
    ''' </summary>    
    Public Overloads Shared Sub Show(message As String)
        Dim dlg As New MessageWindow
        dlg.Owner = Windows.Application.Current.MainWindow
        dlg.txblkMessage.Text = message
        dlg.Image1.Source = Nothing                                     ' no Icon (default icon is used for design)
        dlg.Grid1.ColumnDefinitions(0).Width = New GridLength(20)       ' resize column 0
        dlg.ShowDialog()
    End Sub
    ''' <summary>
    ''' Inherited from Window. No meaningful use here.
    ''' </summary>
    Private Overloads Shared Sub Show()
        Dim dlg As New MessageWindow
        dlg.Owner = Windows.Application.Current.MainWindow
        dlg.ShowDialog()
    End Sub

    Private Shared Sub SetIcon(dlg As MessageWindow, icon As MessageIcon)
        Try
            Dim bi As New BitmapImage
            bi.BeginInit()
            bi.UriSource = GetUri(icon)
            bi.EndInit()
            dlg.Image1.Source = bi
        Catch ex As Exception
            ' catches dlg Is Nothing and UriSource not found
            dlg.Image1.Source = Nothing                                     ' no Icon
            dlg.Grid1.ColumnDefinitions(0).Width = New GridLength(20)       ' resize column 0
        End Try
    End Sub

    Private Shared Function GetUri(icon As MessageIcon) As Uri
        Select Case icon
            Case MessageIcon.Information
                Return uri_Information
            Case MessageIcon.StatusOk
                Return uri_StatusOk
            Case MessageIcon.Error
                Return uri_Error
            Case MessageIcon.Warning
                Return uri_Warning
            Case Else
                Return Nothing
        End Select
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dlg"></param>
    ''' <param name="desiredSize">between 10 and 20, else the default size is set</param>
    Private Shared Sub SetFontSize(dlg As MessageWindow, desiredSize As Double)
        If desiredSize = 0 Then Exit Sub            ' use the default size (SystemFonts.MessageFontSize)

        If desiredSize < 10 Then
            desiredSize = 10                              ' minimal size
        ElseIf desiredSize > 20 Then
            desiredSize = 20                              ' maximal size
        End If

        dlg.txblkMessage.FontSize = desiredSize
    End Sub


End Class

Public Enum MessageIcon
    [Error]
    Information
    StatusOk
    Warning
End Enum