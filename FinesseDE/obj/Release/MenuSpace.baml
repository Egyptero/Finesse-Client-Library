   M S B A M L   `   `   `  ���� D  @FinesseDE, Version=2.1.1.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FinesseDE	FinesseDE      FinesseDE.MenuSpace   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     X TXceed.Wpf.Toolkit, Version=3.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  #localclr-namespace:FinesseDE    � 	Resources     BoolToVisConverter��%   &	        �� .���� -   $��MyMenuSpace��$	��5,5q�$��2q�����   GadgetHeaderBackground �� Source# [  �� UpdateSourceTrigger$ PropertyChanged=�y���  �� Path$ IsMenuSpace� �� 	Converter# [  ._�� ����   GadgetBackground# [ $ PropertyChanged=� � RowDefinitions �� $
��auto ��� $��* �.+�� $��0������ # [ $ PropertyChanged=�.+�� $��Right=�.+��� -   $��CloseMenuSpaceBtn��$��×��
������ $��20(�$	��8,0q�$	��0,0q����� # [ $ PropertyChanged=�����   GadgetHeaderForground# [ $ PropertyChanged=��� -   $��MenuTabs��$��1��$��Arial)�$
��Bold"�$��20,20q�$	��400����.    �� 
TargetType �� Property �� Value	 �� 
TargetType
 ��Name �� Triggers P� Property P� Value  TabHeaderForground �� 
TargetName  TabHeaderBackground  SelectedHeaderTabForground  SelectedHeaderTabBackground��%�  '��       7  7  7  7  �� " ��.���  � �� �� "	 ��.R�� $��Panel��$	��1,0q�.+��� $
 ContentSite��$��Center=�$��Center=�$��Header��$��20,12q� P�  �@��
IsSelected .  . ���  �� 8   $ PropertyChanged=� � ���� $ Panel�� �� 8  $ PropertyChanged=� � ��P�  �@��
IsSelected . . ���  �� 8  $ PropertyChanged=� � ���� $ Panel�� �� 8  $ PropertyChanged=� � ��.@��� $��Teams��.��&   System.Windows.Controls.DataGrid  ���� $ AgentInformation.Teams��� $��
Activities���� $��Setting��.���� ��. .+�9� $��Color Setting����.  9� 
IsExpanded . .���� .+��� $��Desktop��$��Light"�$
��10,5q�.��� .+��� $
��0,10q�.+��� $!��/Images/Desktop/Default.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Default��$	��5,5q� d� 	IsChecked ��   CXProperties# [ $ SelectedBackgroundImage� 	 ParamToStringConverter# [	  �� ConverterParameter$ Default���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop1.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Natural��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop1���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop2.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $
��Snow��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop2���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop3.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Seattle��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop3���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop4.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Mountain��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop4���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop5.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $	��Sky��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop5���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop6.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Trees��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop6���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop7.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $
��Lion��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop7���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop8.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Greeny��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop8���� $
��0,10q�.+��� $"��/Images/Desktop/Desktop9.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Turtile��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ Desktop9���� $
��0,10q�.+��� $#��/Images/Desktop/Desktop10.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Jungle��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ 	Desktop10���� $
��0,10q�.+��� $#��/Images/Desktop/Desktop11.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Bridge��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ 	Desktop11���� $
��0,10q�.+��� $#��/Images/Desktop/Desktop12.jpg��$��50��$��50��$
��Fill=�$	��5,5q�	� $��Heights��$	��5,5q� �� # [ $ SelectedBackgroundImage�# [	 $ 	Desktop12���� $��Background Color Setting��$��Light"�$
��10,5q�.��� .+�#  Xceed.Wpf.Toolkit.ColorPicker   � ToolTip$ Page background��$��50��$	��5,2q�   SelectedColor �� # [ $ PageBackground�$ PropertyChanged=� �� Mode$ TwoWay=�  $" Bottom status bar background��$��50��$	��5,2q� �� # [ $ StatusbarBackground�$ PropertyChanged=�$ TwoWay=�  $ Dialpad background��$��50��$	��5,2q� �� # [ $ DialpadBackground�$ PropertyChanged=�$ TwoWay=�  $! Agent status bar background��$��50��$	��5,2q� �� # [ $ AgentStatusbarBackground�$ PropertyChanged=�$ TwoWay=�  $ Dialog tab background��$��50��$	��5,2q� �� # [ $ DialogBackground�$ PropertyChanged=�$ TwoWay=�  $&  Dialog tab background (Selected)��$��50��$	��5,2q� �� # [ $ SelectedDialogBackground�$ PropertyChanged=�$ TwoWay=�  $ Menu Tab background��$��50��$	��5,2q� �� # [ $ TabBackground�$ PropertyChanged=�$ TwoWay=�  $  Menu Tab header background��$��50��$	��5,2q� �� # [ $ TabHeaderBackground�$ PropertyChanged=�$ TwoWay=�  $+ %Menu Tab header background (Selected)��$��50��$	��5,2q� �� # [ $! SelectedHeaderTabBackground�$ PropertyChanged=�$ TwoWay=�  $ Splitter background��$��50��$	��5,2q� �� # [ $ SplitterBackground�$ PropertyChanged=�$ TwoWay=�  $ Separator background��$��50��$	��5,2q� �� # [ $ SparatorColor�$ PropertyChanged=�$ TwoWay=�  $ Gedget background��$��50��$	��5,2q� �� # [ $ GadgetBackground�$ PropertyChanged=�$ TwoWay=�  $ Gedget header background��$��50��$	��5,2q� �� # [ $ GadgetHeaderBackground�$ PropertyChanged=�$ TwoWay=��� $��Text Color Setting��$��Light"�$
��10,5q�.��� .+�  $ Page forground��$��50��$	��5,2q� �� # [ $ PageForground�$ PropertyChanged=�$ TwoWay=�  $! Bottom status bar forground��$��50��$	��5,2q� �� # [ $ StatusbarForground�$ PropertyChanged=�$ TwoWay=�  $ Dialpad forground��$��50��$	��5,2q� �� # [ $ DialpadForground�$ PropertyChanged=�$ TwoWay=�  $  Agent status bar forground��$��50��$	��5,2q� �� # [ $ AgentStatusbarForground�$ PropertyChanged=�$ TwoWay=�  $ Dialog tab forground��$��50��$	��5,2q� �� # [ $ DialogForground�$ PropertyChanged=�$ TwoWay=�  $% Dialog tab forground (Selected)��$��50��$	��5,2q� �� # [ $ SelectedDialogForground�$ PropertyChanged=�$ TwoWay=�  $ Menu Tab forground��$��50��$	��5,2q� �� # [ $ TabForground�$ PropertyChanged=�$ TwoWay=�  $ Menu Tab header forground��$��50��$	��5,2q� �� # [ $ TabHeaderForground�$ PropertyChanged=�$ TwoWay=�  $* $Menu Tab header forground (Selected)��$��50��$	��5,2q� �� # [ $  SelectedHeaderTabForground�$ PropertyChanged=�$ TwoWay=�  $ Gedget forground��$��50��$	��5,2q� �� # [ $ GadgetForground�$ PropertyChanged=�$ TwoWay=�  $ Gedget header forground��$��50��$	��5,2q� �� # [ $ GadgetHeaderForground�$ PropertyChanged=�$ TwoWay=�� $��Right=�$��10,10q�.+��� -   $��Cancel��$
��15,5q�$
��10,5q�$��60���� -   $
��Save��$
��15,5q�$
��10,5q�$��60���� $��Debug��.��� ���� # [  �� $
��auto ��� $��* �.+�� $��0��$
��10,3q�.+��� .+��� $��Advanced�� �� *TraceStatus+�� $��
Engine Log�� �� *	SaveLog+�� $��Right=�.+��� -   $��ClearLog��$��	Clear Log��  $��1����.    AutoGenerateColumns .  $��Stretch=�$
��10,5q����� *LogMessages+   AlternatingRowBackground �� # [ $ PropertyChanged=�  =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn��%�   '        7 7  �� 8   "  .���  � �� �� 8  $ PropertyChanged=���  � ��
 �������  � ��$	 3,3q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Message��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� 