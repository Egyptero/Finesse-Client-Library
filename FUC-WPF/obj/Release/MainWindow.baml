   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.4.1.1101, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  B >FUC-WPF, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:FUC_WPFFUC_WPF    FUC_WPF.MainWindow   FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     K GXamlAnimatedGif, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null<gif2https://github.com/XamlAnimatedGif/XamlAnimatedGif  !localclr-namespace:FUC_WPF  -     � Title$  FUC_WPF 1.0��$	��400��$	��400��$
��auto�� � 	Resources  �� MergedDictionaries �� Source    BooleanToVisibilityConverter FUC_WPF.VariableLocator  VariableLocator$  FUC_WPF.DialogExtensionLocator  DialogExtensionLocatorF   @MaterialDesignThemes.Wpf.Converters.BooleanToVisibilityConverter ( $InvertedBooleanToVisibilityConverter   	TrueValue   
FalseValue��  �� $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.CheckBox.xaml?��� $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ListBox.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.PopupBox.xaml?��� $k epack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.RadioButton.xaml?��� $i cpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.TextBlock.xaml?��� $l fpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ToggleButton.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ComboBox.xaml?��� $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Slider.xaml?��� $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.TextBox.xaml?��� $k epack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.PasswordBox.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Expander.xaml?��� $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Menu.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.DataGrid.xaml?��� $l fpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.ToggleButton.xaml?�%Z   &	        &	      &	 
     &	      ��       $ 	Collapsed=�$ Visible=�.��� $	��5,5q�   BoolToVisConverter��%   &	       �� .+��� -    ��Name$ MainForm��$y�Hidden=�.+�#   MaterialDesignThemes.Wpf.Chip  -   $��AgentInformation��
�������$��Stretch=���� -    �Name$ AgentInfoGrid��$	��2,2q� � ColumnDefinitions �� $��1* ��� $��1* �.+�� $��0��$��Center=�$
��Left=�.+���    MaterialDesignBody1TextBlock#��[ $
��Left=�$��Center=�$��Arial)�
���   ����� 	 �� Path$	 AgentInformation.Name��� #��[ $
��Left=�$��Center=�$	�� @ ��$��Arial)�
���   ��� #��[ $
��Left=�$��Center=�$��Arial)�
���   ����� $ 	 AgentInformation.Extension�
 �� Mode$
 OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�� $��1��$��Right=�$��Center=�.+�o�   MaterialDesignMenu#��[ .@�m�   MaterialDesignMenuItem#��[ $��30����'   !MaterialDesignThemes.Wpf.PackIcon  �   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.1.1101, Culture=neutral, PublicKeyToken=null]]   Kind$ DotsVertical=�$��24��$��24��
���   �.@�m� -   $��MakeCallMenu��#��[ $��	Make call��
�������y��� $&	  AgentInformation.MakeCallVisible� �� 	Converter# [  m� -   $��	QueueMenu��#��[ $��Queues��
�������m� -   $��	AboutMenu��#��[ $��About��
�������� -   $ 
StatusGrid��$	��5,5q� �� $��1* ��� $
��0.4* �.+��� -	   $��StatusComboBox��  MaterialDesignComboBox#��[ $��0��$��Stretch=�$��Center=����#3D687C���#3D687C���� $&	  AgentInformation.VoiceStatusList� �� SelectedValue �� $*	 $AgentInformation.SelectedVoiceStatus�$
 TwoWay=�$ PropertyChanged=����� .R�� .+��� $��22������ *StatusImage+�� ���� *StatusLabel+��   	 MaterialDesignTitleTextBlock#��[	 $��1��$��Right=�$��Center=�
������� �� Padding$	 0,0q����� $	 StateTimer.TimerLabel�� .+�� -
   $��	Telephony��$y�Hidden=�.+��� -   $��Calls��
������ $��0q�$��Disabled=�$	��0,0q����� $	 AgentInformation.Dialogs� �� $%	 AgentInformation.SelectedDialog�$
 TwoWay=�$ PropertyChanged=�  �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName��%\  '��       �� " ��.���  � �� �� " ��.R�� $��Panel��$	��1,0q�.+��� $ ContentSite��$��Center=�$��Center=�$��Content��$	��5,5q� P�  �@��
IsSelected .  . ���  � ��
 �   ��� $ Panel�� � ��
 �����P�  �@��
IsSelected . . ���  � ��
 �   ��� $ Panel�� � ��
 ��������� .R�#   MaterialDesignThemes.Wpf.Card  
�������$��1q�
�������.���� $��CallDetailsCard��.+�� $��
CallHeader�����#3D687C �� $��3* ��� $��1* �.+��� $	��5,5q�$��1��  
 MaterialDesignBody2TextBlock#��[
 
�������$��Right=����� $!	 DialogStateTimer.TimerLabel��� $	��5,5q�$��0��#��[
 
����������� *Header+� $��CallButtons��.+�� $��CallButtonsGrid��y��� $	 ReservedObject�# [  �� ConverterParameter$ CallButtons��.+��� $��Center=�$��Right=� �� Orientation$ 
Horizontal=�$	��0,5q�.+��� $
��Hold��-    . *MaterialDesignFloatingActionMiniDarkButton#��[ $��36��$��36�� � ToolTip$
 Hold��$��Center=�$��Center=�$	��5,2q����#3D687C
�������
 � Tag �� *ID+y��� *Hold+# [ {��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ PhonePaused=�$��24��$��24���� $��Resume��-    / +MaterialDesignFloatingActionMiniLightButton#��[ $��36��$��36��$ Resume Call��$��Center=�$��Center=�$	��5,2q����#3D687C
������� �� *ID+y��� *Resume+# [ {��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ PhoneReturn=�$��24��$��24��'   !MaterialDesignThemes.Wpf.PopupBox   - )MaterialDesignMultiFloatingActionPopupBox#��[ $��36��$��36�����#3D687C
�������   PlacementMode$ BottomAndAlignCentres=� ^� 	Placement$ Right=�$ Transfer Menu��$	��5,2q�   	StaysOpen .  y��� *
Transfer+# [ {��� $	 ReservedObject�# [ $ CallButtonItem��    ToggleContent    $ PhoneForward=�$��24��$��24��
�������!   PopupContent.! �� .+�� .+��� $��TransferDialPad��-   $	��0,1q�#��[ $��36��$��36��$ Dialpad�����#3D687C
�������" J� Opacity$	" 0.5Q� �� *ID+{��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ Dialpad=�$��24��$��24��� .+��� $��TransferSurvey��-   $	��0,1q�#��[ $��36��$��36��$ Survey�����#3D687C
�������$	" 0.5Q� �� *ID+{��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ EmoticonExcited=�$��24��$��24��� y��� # [ $ SendToIVRButton��.+��� $��TransferIVR��-   $	��0,1q�#��[ $��36��$��36��$	 IVR�����#3D687C
�������$	" 0.5Q� �� *ID+{��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ PhoneSettings=�$��24��$��24���� $��Answer��-   #��[ $��36��$��36��$ Answer Call��$��Center=�$��Center=�$	��5,2q����#3D687C
������� �� *ID+y��� *Answer+# [ {��� $	 ReservedObject�# [ $ CallButtonItem��.��  $ Phone=�$��24��$��24��� $��CallButtonsWaiting��y��� $	 ReservedObject�# [ $ CallButtonsWaiting��.+�� $��Center=�.+��� '	  !XamlAnimatedGif.AnimationBehavior# 	  	SourceUri$# Images/Loader.gif?�$	��0,5q�$��30���� #��[
 $��10,15q����� $	 ReservedObject�# [ $ WaitingMessage���� 
�������$ �� Stretch$
$ Fill=�	% �� X2$% 1���� $��UpdateRoutingDecision��$
��30,5q�$��Center=�$��Center=�y��� $	 ReservedObject�# [ $ UpdateRoutingData��.+��� $ 
Horizontal=�$	��0,5q�.+���   MaterialDesignCheckBox#��[ $
��Male�����#3D687C���#3D687C& d� 	IsChecked& �� $	 ReservedObject.IsMaleOnly�� $��Right=�$
��10,0q�$��Center=�.+�	�   MaterialDesignRadioButton#��[ $��Inquiry�����#3D687C���#3D687C$	��2,0q�& �� $	 ReservedObject.IsInquiry�	� #��[ $��	Complaint�����#3D687C���#3D687C$	��2,0q�& �� $ 	 ReservedObject.IsComplaint��� #��[ $��Center=����#3D687C���#3D687C���� $$	 ReservedObject.TransferIVRMenu� �� $&	  ReservedObject.SelectedIVROption�$
 TwoWay=�$ PropertyChanged=�� $	��0,5q� �� $��1* ��� $��1* �.+��� $��	callRoute��-   $��0��$
��Left=�$
��Call�� " MaterialDesignRaisedDarkButton#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��
closeRoute��-   $��1��$��Right=�$��Close��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+� $��DialPad��$��
80,0,80,10q� �� *ID+y��� $	 ReservedObject�# [ $ DialPad��' � RowDefinitions' �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $��1* ��� $��1* ��� $��1* �.+��� $��NumberToDial��$��0��$��0��$��3��  MaterialDesignTextBox#��[ $	��0,5q�)
   #MaterialDesignThemes.Wpf.HintAssist( 
  Hint$( Type number��$��Center=� �� *ID+���� $	 ReservedObject.DialNumber��� $	��one��-   $��1��$��0��$��1��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $	��two��-   $��1��$��1��$��2��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��three��-   $��1��$��2��$��3��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��four��-   $��2��$��0��$��4��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��five��-   $��2��$��1��$��5��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $	��six��-   $��2��$��2��$��6��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��seven��-   $��3��$��0��$��7��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��eight��-   $��3��$��1��$��8��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��nine��-   $��3��$��2��$��9��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��start��-   $��4��$��0��$��*��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��zero��-   $��4��$��1��$��0��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��hash��-   $��4��$��2��$��#��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $
��call��-    $��5��$��0��$
��Call��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��clear��-!   $��5��$��1��$��Clear��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+�� $��close��-"   $��5��$��2��$��Close��#��[ ���#3D687C
�������$	��1,1q�$��28�� �� *ID+9� $��CallVariablesData��  MaterialDesignExpander#��[ $��Stretch=�y��� # [ $ CallVariablesStatus����� $��Stretch=� �� $��3* ��� $��1* ��� $��1* �.+��� $	��2,5q�#��[
 $��0��$��Stretch=����� # [ $ callVariable5���� $	��2,5q�#��[
 $��1��$��Stretch=����� # [ $ callVariable2���� $	��2,5q�#��[
 $��2��$��Stretch=����� # [ $ callVariable6��.��� $ RJBCallInformation��$
��15,2q�.+��� .+�  $	��5,1q�$ Skill Label��)   IconBackground) �#3D687C*   IconForeground
* �����
�������$��Stretch=����� # [ $ callVariable5��+   Icon+   $ Group=�  $	��5,1q�$	 POS��) �#3D687C
* �����
�������$��Stretch=����� # [ $ callVariable1��+   $	 Pen=�  $	��5,1q�$ Language��) �#3D687C
* �����
�������$��Stretch=����� # [ $ callVariable2��+   $ 
FormatText=�  $	��5,1q�$	 CIC��) �#3D687C
* �����
�������$��Stretch=����� # [ $ callVariable4��+   $ 
AccountKey=�  $	��5,1q�$ 	IVR Label��) �#3D687C
* �����
�������$��Stretch=����� # [ $ callVariable3��+   $ Application=�  $	��5,1q�$ Dialed Number��) �#3D687C
* �����
�������$��Stretch=����� # [ $ callVariable7��+   $ PhoneInTalk=�� -#   $��Queues��$y�Visible=�.+�  $	��5,5q�
�������$��1q�
�������.���� .+�&   System.Windows.Controls.DataGrid  $��2 8 2 0q�  MaterialDesignDataGrid#��[ ,   CanUserSortColumns, . -   CanUserAddRows- .  -   'MaterialDesignThemes.Wpf.DataGridAssist.   CellPadding$. 4 2 2 2q�/   ColumnHeaderPadding$/ 4 2 2 2q�0   AutoGenerateColumns0 .  ���� $	 AgentInformation.Queues�1   ColumnHeaderStyle1 �� =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  �@�LayoutTransform V� .~��� 2 �� Angle$2 90Q�3   Columns3 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn4   Header$4 
Queue name��1  +System.Windows.Controls.DataGridBoundColumn5   Binding5 �� *Name+  $4 Calls in queue��5 �� *CallsInQueue+  $4 Agents ready��5 �� *AgentsReady+  $4 Agents not ready��5 �� *AgentsNotReady+  $4 Agents talking inbound��5 �� *AgentsTalkingInbound+  $4 Agents talking outbound��5 �� *AgentsTalkingOutbound+  $4 Agents talking internal��5 �� *AgentsTalkingInternal+� $��Center=�.+��� -$   $
��0,20q�  MaterialDesignRaisedButton#��[ $��Close��$��Center=�$��Center=����#3D687C
�������� -%   $��About��$y�Hidden=�.+�  $��50,30q�
�������$��1q�
�������.���� $��Center=�.+��� $��0,20,0,0q�#��[	 $��Finesse USD Connector��$��Center=��� #��[ $��Version 2.0��$��Center=��� $
��0,20q�#��[
 $��ISTNetworks - CXiC��$��Center=��� #��[
 $��Author��$��Center=��� #��[ $��Mamdouh Aref��$��Center=��� #��[
 $��Designer��$��Center=��� #��[ $��Mamdouh Aref��$��Center=��� #��[
 $��	Developer��$��Center=��� #��[ $��Mamdouh Aref��$��Center=�� $��Center=�.+��� -&   $
��0,20q�#��[ $��Close��$��Center=�$��Center=����#3D687C
��������� -'   $ 	LoginForm��$y�Visible=�.+��� $
��0,40q�.+�� $��Stretch=�$��Center=�.+�� .+�  $ Account=����#3D687C�� -(   6 ��Name$6 AgentID��#��[ $( Agent ID��$	��200��$	��5,3q����#3D687C���#3D687C���� $	 AgentInformation.AgentID�$
 TwoWay=�$ PropertyChanged=�� $��Stretch=�$��Center=�.+�� .+�  $	 Key=����#3D687CG� -)   7 G�Name$7 AgentPassword�� ) %MaterialDesignFloatingHintPasswordBox#��[ $( Password��$	��200��$	��5,3q����#3D687C���#3D687C� $��Stretch=�$��Center=�.+�� .+�  $ Phone=����#3D687C�� -*   $6 AgentExtension��#��[ $( 	Extension��$	��200��$	��5,3q����#3D687C���#3D687C���� $ 	 AgentInformation.Extension�$
 TwoWay=�$ PropertyChanged=�9� #��[ $��Center=�
�������
���   �$
��0,20q�y��� $6	 0AgentInformation.ReservedObject.ConfigureDetails�# [  ��� .+��� #��[
 $��More Details��.��� 
�������$	��1,1q�.+��� $
��5,10q�.+�� $��Stretch=�$��Center=�.+�� .+�  $ Server=����#3D687C�� -+   $6 DomainA��#��[ $( 	Finesse A��$y�Visible=�$	��200��$	��5,3q����#3D687C���#3D687C���� $	 AgentInformation.DomainA�$
 TwoWay=�$ PropertyChanged=�� $��Center=�$��Center=�.+�� .+�  $ Server=����#3D687C�� -,   $6 DomainB��#��[ $( 	Finesse B��$y�Visible=�$	��200��$	��5,3q����#3D687C���#3D687C���� $	 AgentInformation.DomainB�$
 TwoWay=�$ PropertyChanged=�� $��Center=� �� $��1* ��� $��1* �.+��� $��0��$ 
Horizontal=�.+�  $ Security=����#3D687Cd� --   8 d�Name$	8 SSL�� ( $MaterialDesignSwitchDarkToggleButton#��[ $	 SSL��$	��5,3q����#3D687C
�������$	��SSL��& �� $	 AgentInformation.SSL��� $��1��$ 
Horizontal=�.+�  $
 File=����#3D687Cd� -.   $8 TraceStatus��#��[ $ Trace��$	��5,3q����#3D687C
�������$	��Trc��& �� $	 TraceStatus��� 
�������$
$ Fill=�$% 1��$��2,20,2,5q�$	��200��� $��Center=�$��Center=�.+�� .+�  $ Server=����#3D687C�� -/   $6 CTIWEB��#��[ $( CTIWeb Server��$y�Visible=�$	��200��$	��5,3q����#3D687C���#3D687C���� $,	 &AgentInformation.ReservedObject.CtiWeb�$
 TwoWay=�$ PropertyChanged=�� $��Center=� �� $��1* ��� $��1* �.+��� $��0��$ 
Horizontal=�.+�  $ ArrowLeftBoldCircle=����#3D687Cd� -0   $8 LogAUDStart��#��[ $ Call AUD Start��$	��5,3q����#3D687C
�������$��AS��& �� $2	 ,AgentInformation.ReservedObject.CallAUDStart��� $��1��$ 
Horizontal=�.+�  $  ArrowLeftBoldCircleOutline=����#3D687Cd� -1   $8 	LogAUDEnd��#��[ $ Call AUD End��$	��5,3q����#3D687C
�������$��AE��& �� $0	 *AgentInformation.ReservedObject.CallAUDEnd�� $��Center=� �� $��1* ��� $��1* �.+��� $��0��$ 
Horizontal=�.+�  $ AlertCircle=����#3D687Cd� -2   $8 FireCallStart��#��[ $ Fire Call Start��$	��5,3q����#3D687C
�������$��FS��& �� $8	 2AgentInformation.ReservedObject.FireCallStartEvent��� $��1��$ 
Horizontal=�.+�  $ AlertCircleOutline=����#3D687Cd� -3   $8 FireCallEnd��#��[ $ Fire Call End��$	��5,3q����#3D687C
�������$��FE��& �� $6	 0AgentInformation.ReservedObject.FireCallEndEvent�� $��Center=�$
��0,10q� �� $��1* ��� $��1* ��� $��1* �.+��� $��0��$ 
Horizontal=�.+�  $ Phone=����#3D687Cd� -4   $8 LogCallStart��#��[ $ Log Call Start��$	��5,3q����#3D687C
�������$��LS��& �� $2	 ,AgentInformation.ReservedObject.LogCallStart��� $��1��$ 
Horizontal=�.+�  $ PhoneHangup=����#3D687Cd� -5   $8 
LogCallEnd��#��[ $ Log Call End��$	��5,3q����#3D687C
�������$��LE��& �� $0	 *AgentInformation.ReservedObject.LogCallEnd��� $��2��$ 
Horizontal=�.+�  $ PhoneOutgoing=����#3D687Cd� -6   $8 LogCallTransfer��#��[ $ Log Call Transfer��$	��5,3q����#3D687C
�������$��LT��& �� $5	 /AgentInformation.ReservedObject.LogCallTransfer�� $��Center=�$��Center=�.+��� -7   $��5��$��0��$��2��#��[ $��Start��$��Center=�$��Center=����#3D687C
�������$	��100��$
��0,20q��� #��[ $��FUC 2.0��$��Bottom=�
�������$��Center=��� -8   $ LoadingForm��$y�Hidden=�.+��� $# Images/Loader.gif?�$	��150��$	��150��$
��0,50q��� -9   9 ��Name$9 CurrentLoadingStep��#��[ 
�������$��Center=����� *LoadingMessage+'   !MaterialDesignThemes.Wpf.Snackbar  -:   :  Name$: ErrorBar��$	��Top=�
�������
���\\��$��Stretch=�;   MessageQueue; 4   .MaterialDesignThemes.Wpf.MessageQueueExtension  �� -;   $��
LoadingBar��$��Bottom=�.@��� 
����������� *LoadingMessage+