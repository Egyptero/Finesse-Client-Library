   M S B A M L   `   `   `  ���� D  @FinesseDE, Version=2.2.8.1, Culture=neutral, PublicKeyToken=null%clr-namespace:FinesseDE	FinesseDE      FinesseDE.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     J FWpfAnimatedGif, Version=1.4.17.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  #localclr-namespace:FinesseDE  -   "  WpfAnimatedGif.ImageBehavior    AnimateInDesignMode  .  � Title$- 'Finesse Gadget Desktop Platform V 2.281��$	��600�� � Icon$ Finesse.ico��$	��0,0q� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=�����  j� Stretch$
 Fill=� �� ImageSource ��    CXProperties �� Source# [   �� Path$ BackgroundImage�.��� .+�� -   $��	Connected������   PageBackground# [ 	 �� UpdateSourceTrigger$	 PropertyChanged=�
 � 	Resources
   ComboBoxTextBox �� 
TargetType ��Name  ComboBoxToggleButton �� 
TargetType �� Property �� Value � ColumnDefinitions �� CornerRadius F�Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	       &	 0     �� " ��.R��� $ PART_ContentHost��|�. �� " d�.���  � �� �� " d�.R��  �� �� $��32 �.+��� $ Border��$��2��$ 0��#��z��#��z��$��2q��� $��0��$ 0��
������ 
������ $��10q�F� $ Arrow��$��1��#��z��#��z��$��Center=�$��Center=�"����        � z  � � $�     	 P�  � {� .  . ��� $ Border�� � ��
 ������� $ Border�� � ��
 �������  � ��
 ������� $ Arrow�� � ��
 ����� � RowDefinitions �� $
��auto ��� $
��auto ��� $��* ��� $
��auto �.+��� -   $��InteractionsToolBar��$��0��
������ $��0q�$��Disabled=�$
��10,5q�$	��135������ $ AgentInformation.Dialogs� �� SelectedValue �� $% AgentInformation.SelectedDialog� �� Mode$ TwoWay=�$	 PropertyChanged=����� .R��  � Orientation$ 
Horizontal=� K� IsItemsHost .  $	��Top=�
  ��Name  DialogForground  DialogBackground  SelectedDialogForground  SelectedDialogBackground��%   '��       7  7  7  7  �� " ��.���  � �� �� " ��.R�� $��Panel��$	��1,0q�.+��� $ ContentSite��$��Center=�$��Center=�$��Content��$	��5,5q� P�  �@��
IsSelected .  . ���  �� 8   $	 PropertyChanged=� � ����  � ��$
 Bold"��� $ Panel�� �� 8  $	 PropertyChanged=� � ��P�  �@��
IsSelected . . ���  �� 8  $	 PropertyChanged=� � ����  � ��$
 Bold"��� $ Panel�� �� 8  $	 PropertyChanged=� � ������ .R�� $	��380�� �� $��1* ��� $��1* �.+�� $��0��.+��� $��(��$	��0,5q��� $	��2,5q����� $! DialogStateTimer.TimerLabel��� $��)��$	��0,5q��� $	��0,5q����� *Header+� $��1��$��Right=�.+��� $��Answer��-    � ToolTip$ Answer��
������ 
������ 
 � Tag �� *ID+y��� *Answer+  BoolToVisConverter �� 	Converter# [ .���� $��Images/answercall.png��$��25��$��25���� $��Transfer��-   $ Direct Transfer��
������ 
������  �� *ID+y��� *
Transfer+# [ .���� $��Images/transfercall.png��$��25��$��25���� $��	CTransfer��-   $ Transfer��
������ 
������  �� *ID+y��� *	CTransfer+# [ .���� $��Images/transfercall.png��$��25��$��25���� $��Consult��-   $ Consult��
������ 
������  �� *ID+y��� *	Consult+# [ .���� $��Images/consultcall.png��$��25��$��25���� $��
Conference��-   $ 
Conference��
������ 
������  �� *ID+y��� *
Conference+# [ .���� $��Images/conferencecall.png��$��25��$��25���� $��Keypad��-	   $ Keypad��
������ 
������  �� *ID+y��� *
SendDTMF+# [ .���� $��Images/keypadcall.png��$��25��$��25���� $
��Hold��-
   $
 Hold��
������ 
������  �� *ID+y��� *Hold+# [ .���� $��Images/holdcall.png��$��25��$��25���� $��Resume��-   $ Resume��
������ 
������  �� *ID+y��� *Resume+# [ .���� $��Images/resumecall.png��$��25��$��25���� $��Release��-   $ Release��
������ 
������  �� *ID+y��� *	Release+# [ .���� $��Images/releasecall.png��$��25��$��25��� $��1������  	 AgentStatusbarBackground# [	 $	 PropertyChanged=� �� $
��auto ��� $
��auto � �� $��* ��� $
��auto �.+�� -   $��DialPad��$��0��$��0��$��2��$��Center=�$
��auto��$y�	Collapsed=�����  
 DialpadBackground# [
 $	 PropertyChanged=�.+��  �� $
��auto ��� $
��auto � �� $��* ��� $
��auto �.+�� $��0��$��0��$��Center=�
������ .+��� -   $��
DialNumber��$	��150��$	��150��$	��5,2q�$	��5,5q��� -   $��Keypad_1��$��1��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������   DialpadForground# [ $	 PropertyChanged=��� -   $��Keypad_2��$��2��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_3��$��3��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_4��$��4��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_5��$��5��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_6��$��6��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_7��$��7��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_8��$��8��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_9��$��9��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_0��$��0��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_start��$��*��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� -   $��Keypad_hash��$��#��$��18(�$��Black"�
������ $	��5,0q�$	��2,2q�
�������$��25������ # [ $	 PropertyChanged=��� $��2��$��Stretch=�$	��5,2q�����   SparatorColor# [ $	 PropertyChanged=��� -   $��Clear��$��Clear��$
��10,3q�$	��5,5q�$
��Bold"�$��55������ # [
 $	 PropertyChanged=����� # [ $	 PropertyChanged=����� # [ $	 PropertyChanged=��� -   $
��Dial��$
��Dial��$
��10,3q�$	��5,5q�$
��Bold"�$��55������ # [
 $	 PropertyChanged=����� # [ $	 PropertyChanged=����� # [ $	 PropertyChanged=�� $��Right=�$��1��$��0��$��Center=�.+��� -   $��CloseDialPad��$��×��
������ $��20(�$	��8,0q�$	��0,0q����� # [
 $	 PropertyChanged=����� # [ $	 PropertyChanged=��� $��1��$��0��$��2��$��2��$��Stretch=�$	��5,2q����� # [  $ SplitterBackground�$	 PropertyChanged=�� -   $��StatusPanel��$��0��$��1��$
��Left=�$��Center=����� # [	 $	 PropertyChanged=�.+��� -   $��StatusComboBox��$
��10,2q�$��Center=�$��Center=����� $&  AgentInformation.VoiceStatusList� �� $* $AgentInformation.SelectedVoiceStatus�$ TwoWay=�$	 PropertyChanged=�
   �� 	ClickMode! d� 	IsChecked" �� RelativeSource# J� IsHitTestVisible$ �� SelectionBoxItem% �� SelectionBoxItemTemplate& ��Name' �� 
IsReadOnly( �� 
IsReadOnly) �� IsDropDownOpen* � AllowsTransparency+ J� SnapsToDevicePixels, � ActualWidth- �� MaxDropDownHeight-  'System.Windows.Input.KeyboardNavigation.   DirectionalNavigation/ P� 
SourceName0 �� Triggers��%�  '��       7 	 7 	 7  7  7 	 �� " ��.���  � �� �� 8   $	 PropertyChanged=���  � �� �� 8  $	 PropertyChanged=���  � ��
 �������  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��$ Normal"���  � ��$	 300����  � ��$ 32����  � ��$ 32����  � �� �� " ��.R�� .+�d� $��ToggleButton��#��z��#��z��#��z��8�� $��2��|�.  $  Press=�! �� $ IsDropDownOpen�$ TwoWay=�" �� *TemplatedParent+�� $��ContentSite��# .  #��z$ #��z% #��z��$��	10,3,30,3q�$��Center=�$
��Left=��� $& PART_EditableTextBox��8�� $
��Left=�$��Center=�$��3,3,23,3q�|�. $y�Hidden=�#( z' ��Z� � $��Popup��$��Bottom=�#��z) * . |�.  $��Slide=�.��� $��DropDown��+ . #��z, #��z- .+��� $ DropDownBorder��
�������$��2q����� 8  $	 PropertyChanged=��� $��4,6,4,6q�+ . .����  . $. 	Contained=� P�  �@��HasItems .  . ��� $ DropDownBorder�� � ��$ 95��P�  �@��
IsGrouping . . ���  � �� .  P� $/ Popup�� �@�AllowsTransparency . . ��� $ DropDownBorder�� �@��CornerRadius$ 0���� $ DropDownBorder�� � ��$ 0,2,0,0q�P�  �@��
IsEditable . . ���  � �� .  �� $ PART_EditableTextBox�� � y�$ Visible=��� $ ContentSite�� � y�$ Hidden=�0 ���� .R�� .+��� $��22������ *StatusImage+�� ���� *StatusLabel+�� $��2��$��Stretch=�����   SplitterBackground# [ $	 PropertyChanged=��� $
��Bold"�$��18(�$	��5,0q�$	��0,5q�$��Center=�$��Center=����� $ StateTimer.TimerLabel�����   AgentStatusbarForground# [ $	 PropertyChanged=��� $��2��$��Stretch=����� # [ $	 PropertyChanged=��� $	��Ext��$
��Bold"�$	��5,0q�$	��0,5q�$��Center=�$��Center=����� # [ $	 PropertyChanged=��� $
��Bold"�$	��5,0q�$	��0,5q�$��Center=�$��Center=����� $  AgentInformation.Extension����� # [ $	 PropertyChanged=�� $��1��$��1��$��Right=����� # [	 $	 PropertyChanged=�.+��� -    $��MakeCall��$��	Make Call��$
��10,0q�$	��5,5q�$
��Bold"����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=����� # [ $	 PropertyChanged=�y��� *" AgentInformation.MakeCallVisible+# [ $	 PropertyChanged=��� $��2��$��Stretch=����� # [ $	 PropertyChanged=�o� ���� # [	 $	 PropertyChanged=�.@�m� ���� $	��≡��$��	ExtraBold"�$��26(����� # [ $	 PropertyChanged=�.@�m� -!   $ 0��$��Teams��1 J� Opacity$1 1Q����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=�m� -"   $ 1��$��Queues��$	1 0.8Q����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=�m� -#   $ 2��$��
Activities��$	1 0.8Q����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=�m� -$   $ 3��$��Setting��$	1 0.8Q����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=�m� -%   $ 4��$��Debug��$	1 0.8Q����� # [	 $	 PropertyChanged=����� # [ $	 PropertyChanged=�� -&   $��MainGrid��$��2���� -'   $��	Statusbar��$��3��$��30��$��Bottom=�����   StatusbarBackground# [ $	 PropertyChanged=�����   StatusbarForground# [ $	 PropertyChanged=�.@��� -(   $��CurrentStatus��$��Agent logged in ...��$��NoWrap=�� -)   $��	Reconnect��$1 1Q�$y�	Collapsed=�$��Center=�$��Center=�$	��400��$	��600��.+��� 2   AnimatedSource$2 Images/Somloading.gif��$��Center=�$��Center=��� $��Center=�$��Bottom=�
���   �$
��Bold"�
�������$
��0,10q����� *LoadingMessage+