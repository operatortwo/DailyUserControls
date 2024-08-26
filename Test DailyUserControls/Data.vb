Imports System.Collections.ObjectModel

Partial Public Class MainWindow

    '--- some Data for DataGrid ---

    Public Property LColorList As New List(Of RgbColor) From
    {
    New RgbColor With {.BaseIndex = 0, .Name = "White", .HexValue = "#FFFFFF", .R_Percent = 100, .G_Percent = 100, .B_Percent = 100},
    New RgbColor With {.BaseIndex = 1, .Name = "Silver", .HexValue = "#C0C0C0", .R_Percent = 75, .G_Percent = 75, .B_Percent = 75},
    New RgbColor With {.BaseIndex = 2, .Name = "Gray", .HexValue = "#808080", .R_Percent = 0, .G_Percent = 0, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 3, .Name = "Black", .HexValue = "#000000", .R_Percent = 0, .G_Percent = 0, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 4, .Name = "Red", .HexValue = "#FF0000", .R_Percent = 100, .G_Percent = 0, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 5, .Name = "Maroon", .HexValue = "#800000", .R_Percent = 50, .G_Percent = 0, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 6, .Name = "Yellow", .HexValue = "#FFFF00", .R_Percent = 100, .G_Percent = 100, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 7, .Name = "Olive", .HexValue = "#808000", .R_Percent = 50, .G_Percent = 50, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 8, .Name = "Lime", .HexValue = "#00FF00", .R_Percent = 0, .G_Percent = 100, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 9, .Name = "Green", .HexValue = "#008000", .R_Percent = 0, .G_Percent = 50, .B_Percent = 0},
    New RgbColor With {.BaseIndex = 10, .Name = "Aqua", .HexValue = "#00FFFF", .R_Percent = 0, .G_Percent = 100, .B_Percent = 100},
    New RgbColor With {.BaseIndex = 11, .Name = "Teal", .HexValue = "#008080", .R_Percent = 0, .G_Percent = 50, .B_Percent = 50},
    New RgbColor With {.BaseIndex = 12, .Name = "Blue", .HexValue = "#0000FF", .R_Percent = 0, .G_Percent = 0, .B_Percent = 100},
    New RgbColor With {.BaseIndex = 13, .Name = "Navy", .HexValue = "#000080", .R_Percent = 0, .G_Percent = 0, .B_Percent = 50},
    New RgbColor With {.BaseIndex = 14, .Name = "Fuchsia", .HexValue = "#FF00FF", .R_Percent = 100, .G_Percent = 0, .B_Percent = 100},
    New RgbColor With {.BaseIndex = 15, .Name = "Purple", .HexValue = "#800080", .R_Percent = 50, .G_Percent = 0, .B_Percent = 50}
}
    Public Property ColorList As New ObservableCollection(Of RgbColor)(LColorList)


    Public Class RgbColor
        Public Property BaseIndex As Integer
        Public Property Name As String = ""
        Public Property HexValue As String = ""
        Public Property R_Percent As Integer
        Public Property G_Percent As Integer
        Public Property B_Percent As Integer
    End Class

    '--- some Data for CheckboxFilterList ---

    Public Enum Months
        January = 1
        February = 2
        March = 3
        April = 4
        May = 5
        June = 6
        July = 7
        August = 8
        September = 9
        October = 10
        November = 11
        December = 12
    End Enum

    Public Property ValueList As New List(Of Integer) From
        {101, 102, 103, 104, 105, 106, 107, 108, 109, 110}

End Class
