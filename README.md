# DailyUserControls
This side work contains some usercontrols that I use in other **.NET Framework** projects.  
The goal is to collect some common controls in a library to be able to maintain them in one place.  
There's a compromise between looking good, working well, and acceptable programming effort.
The current version offers quick and easy usability, while the graphics and the user input are kept rather simple.  
The test application is an important part of the project. On the one hand to present and describe the controls, on the other hand for debugging and development.  
During development, it became clear that not all ideas can be solved with **UserControls**. Where necessary, **CustomControls** were added to the library.


##
### The Controls

##
#### Image Button
![ImageButton](https://user-images.githubusercontent.com/88147904/236563904-b4d02a48-60af-43fd-9e03-8d6ee09edc90.PNG)  
 Allows placing a text and an image on a button. The image location can be set to left, right, top or bottom.
 The view can be adjusted using *ImageMargin* and *TextPadding*.
 
##
#### Numeric Up Down
![NumericUpDown](https://user-images.githubusercontent.com/88147904/236563999-ddb60ac5-b0bd-439d-b84a-3d27cfa4fd18.PNG)  
Can be used to increase, decrease or enter a numeric value.
The value can have decimal places, can be negative and can be restricted to step values.

##
#### Selector Button
![SelectorButton](https://user-images.githubusercontent.com/88147904/236564028-7c245dec-0e85-4ff0-a13d-e3afea0b53d5.PNG)  
Is a combination of a button and an up/down control. The control contains an integer value which can be used as an index to a list of items.
The user can press the button to open a dialog to choose an item from a list, or can scroll through the list with the up/down buttons and the mouse wheel.  
The control itself only manages button click, increasing and decreasing the value. Additional code is required for handling the list and displaying the dialog

##
#### Small Slider
![SmallSlider](https://user-images.githubusercontent.com/88147904/236564707-593c6bf1-cb88-48b1-bbfd-1c15aa76675a.PNG)  
Allows increasing and decreasing a numeric value. Responds to mouse wheel and needs only little space.

##
#### Toggle Button
![ToggleButton](https://user-images.githubusercontent.com/88147904/236564734-5005cba6-acdb-4b5d-9930-59057779eb31.PNG)  
Based on the original ToggleButton, it has additional properties for text, background and image, each for checked and unchecked state.
This allows the control to be used flexibly. For example, an on-off push button switch can be shown that is backlit when ON.

##
#### VU Bar
![VU_Bar](https://user-images.githubusercontent.com/88147904/236564761-2143aa9c-e7e7-46b4-96c7-acfc371fe6e4.PNG)  
Is a very simple indicator showing activity on an audio track.
Some additional code for timed value decrease is necessary.  
Bar can be SolidColorBrush, LinearGradientBrush or ImageBrush.

##
#### Progress Circle
![ProgressCircle](https://user-images.githubusercontent.com/88147904/236564902-81b50848-d7fb-4400-aa28-65f055ec450d.PNG)  
Visualizes progress or angle.

##
#### Progress Ring

Visualizes progress in ring- / donut style

##
#### Knob
![Knob](https://user-images.githubusercontent.com/88147904/236564934-270e07cd-a5e8-4b54-b6fe-ae467bf36556.PNG)  
Is a value control that simulates a potentiometer.

##
#### DataGridTextColumnX
A CustomControl that adds TextAlignment and Color Properties to a DataGridTextColumn

##
#### ComboBox
A custom control that inherits from ComboBox and allows to set Background and BorderBrush. This is because with the original ComboBox, changing the Background or BorderBrush has no effect.

##
#### CheckboxFilterList
![CheckboxFilterList](https://github.com/operatortwo/DailyUserControls/assets/88147904/7db2466d-a989-47b9-a2f4-1dbec5e7aa4f)  
Based on a List or an Enum the user can select items in a Listbox Window.
The control then returns a list of the selected items.

##
#### MessageWindow
![MessageWindowSmall](https://github.com/user-attachments/assets/e690ce65-17fa-472f-96ad-393571c4e0e8)  
Works similarly to MessageBox. Allows to display messages, for example whether an action was successful or not.  
The built-in icons **Error**, **Information**, **StatusOk** and **Warning** can be used to clarify the meaning of the message.
Because SizeToContent is used, both short and long messages can be displayed.  
A subset is **QuestionWindow** which returns Yes, No or Cancel.

##
#### Side Panel
![SidePanel](https://user-images.githubusercontent.com/88147904/236564981-e5dd04d4-f29e-4a35-98a6-0283e260c5d0.PNG)  
A panel on the left side of the window which can contain SidePanelButtons, each associated with a tool window.
There are no docking or other functions. Such projects can be found by lookin for *wpf avalondock*.

##
#### TabControlExpander
![TabControlExpander](https://github.com/operatortwo/DailyUserControls/assets/88147904/1f424a6b-2d52-471d-a7a6-372abc70c024)  
A combination of TabControl and Expander.
Since it wasn't possible to have named TabItem's
(such as x:Name="Tab 1") in a UserControl, this control was created as a CustomControl.
The basic function is inspired by the Ribbon Control: TabItems can be filled with controls and, if desired, the height of the TabControl can be reduced to the height of the header.  
It is necessary to set a fixed height like: Height="150" and currently only TabStripPlacement TOP can be used.

##
### Notes

#### Using the controls

- References / Add Reference -> DailyUserControls
- In XAML header add namespace:  
````xmlns:duc="clr-namespace:DailyUserControls;assembly=DailyUserControls"````
- In XAML body (Window Content / Grid etc.) insert the control: ```<duc:NumericUpDown/>```
- Change the properties in the Properties Window. Brushes can be found at the top while special properties are grouped together in one category at the end.  

![Category](https://github.com/operatortwo/DailyUserControls/assets/88147904/d96d9bc4-ab6c-4be6-8173-6c0fda000a1b)


#### SetValueSilent
Some value controls have a *SetValueSilent* method. This is useful in some special cases, generally when user and program code can set the value of the control.
*SetValueSilent* causes the control's value to be updated, but does not raises a ValueChanged event.

##
### Known issues
#### ValueChanged

There seems to be a bug in VS 2022 17.5.5+  
and also in VS 2019 when *Preview  features* */New WPF-XAML Designer for .NET Framework* is checked:  
When creating a ValueChanged handler by double-clicking the event in the designer, an incomplete handler for ValueChanged is inserted.
It is not due to the user controls but can also be observed with the built-in slider control.

```
Private Sub Slider1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of T))

End Sub
```
> Error BC30002 Type ‘T’ is not defined.

This can be corrected by completing the handler to:
```
Private Sub Slider1_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles Slider1.ValueChanged

End Sub
```
There is now also an event in XAML:
> ValueChanged="Slider1_ValueChanged"  

This should be deleted, else ValueChanged is called twice.  

*'Handles' seems to be VB specific, it might work a little differently in C#.*

#### Initial sizing

Until version 1.0.5.9, initial values ​​for Width, Height, HorizontalAlignment and VerticalAlignment were set in the constructor.
The intention was to give the controls an initial size to simplify the design process.
Over time, however, it became apparent that this made designing more difficult and some settings for sizing and alignment could no longer be made.  

Therefore, in version 1.0.6.0 the initial settings for Width, Height, HorizontalAlignment and VerticalAlignment were removed. 
The behavior when adding UserControls to the design now corresponds to that of the built-in controls, such as the button. Width and Height are set to Stretch and so the initial size can be, for example, 600 x 400.
The designer must now first limit the size, but in return he regains full control over the design.
