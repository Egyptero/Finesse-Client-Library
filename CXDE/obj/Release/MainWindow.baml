   M S B A M L   `   `   `  ���� @  <CXDE, Version=1.0.36.1, Culture=neutral, PublicKeyToken=null1clr-namespace:CXDE.PropertiesCXDE.Properties  clr-namespace:CXDECXDE      CXDE.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     K GXamlAnimatedGif, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null<gif2https://github.com/XamlAnimatedGif/XamlAnimatedGif  (propclr-namespace:CXDE.Properties  localclr-namespace:CXDE  -     � Title$  Finesse USD��$	��400��$	��800��.��� -    �Name$ MainGrid��$��10,10,10,10q�
�������.+��� -    ��Name$ 
MainScreen��$y�Visible=�.+�� $��Stretch=�$y�Visible=�.+��� $��Arial)�
�������$��Center=�$��Connector Version 1.80��� -   $ 
MessageBar��$��Stretch=�$y�Hidden=� � ColumnDefinitions �� $��1* ��� $��6* ��� $��1* �.+��� -    ��Name$ UserMessageImage��$��0��$��Images/Error.png��$��20��$��20���� -    ��Name$ UserMessageText��$��1��$��Stretch=�
���  ��
�������$ ��Error connecting to server��$	��5,0q��� -   $��2�� � ToolTip$ Close the error bar������  �� 
TargetType" ��.R�� .+��� $��Center=�$��Center=�.���� -   $ UserCloseImage��$��Images/close.png��$��20��$��20��$��Right=�� -	   $ 
StatusGrid�� �� $��1* ��� $��1* �.+��� -
   $ InStatusTimer��$��0��$
��Left=�$��Center=�$��In state since: 00:00��$��Arial)�
��������� -    ��Name$ StatusDropDown��$��1��$��Right=�$��Center=��� 	 �� Stretch$
	 Fill=�
�������	
 �� X2$
 2��$
��0,10q�$��1�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 1Q�   	Direction$	 330Q�   Color$ Black��   Opacity$	 0.5Q�   
BlurRadius$ 1Q�� -   $ AgentInfoGrid�� �� $��1* ��� $��1* � � RowDefinitions �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� $��0��$��0��$
��Left=�$��Center=�$��My Information��$��Arial)�
�������$��Black"��� -   $ AgentNameInfo��$��1��$��0��$
��Left=�$��Center=�$��Name: Mamdouh Aref��$��Arial)�
��������� -   $ AgentExtensionInfo��$��2��$��0��$��Center=�$��Extension: 1072��$��Arial)�
��������� $��0��$��1��$��Center=�$��My Statistics��$��Arial)�
�������$��Black"��� -   $ LoginDuration��$��1��$��1��$��Login Duration: 00:00:00��$��Center=�
��������� -   $ AverageTalkTime��$��2��$��1��$��Avg Talk Time: 00:00:00��$
��Left=�$��Center=�
��������� -   $ NotReadyTime��$��3��$��1��$��Not Ready Time: 00:00:00��$
��Left=�$��Center=�
��������� -   $ HandledCall��$��5��$��1��$��Handled Calls: 00��$
��Left=�$��Center=�
��������� $
	 Fill=�
�������$
 2��$
��0,10q�$��1��   $ 1Q�$	 330Q�$ Black��$	 0.5Q�$ 1Q�� -   $ MakeCall��$��Center=�.+��� .+�� $��Center=�.+��� -    ��Name$ 
DialNumber��$��Type Number��$��Center=�$	��200��$��Center=�
�������
��������� $	��0,3q��� -    ��Name$ MakeCallButton��$
��Dial��$��Center=�
�������
�������$��Black"�$
��14px(�$��50���� $	��400��$
��Auto=�$��Stretch=�.���� -   $ Calls��$
��0,10q�.+��� -    ��Name$ CallDialog1��
�������$��1q� �� Padding$	 5,5q�$	��5,5q�._��� .+�� -   $ 
CallVarBar��$��Stretch=�$y�Visible=� �� $��4* ��� $��2* ��� $��1* �.+��� -   $ SkillHighLight��$��0��$��Stretch=� �� TextAlignment$ Center=�
��� � �
�������$��CurrentAcct_VIP_Inq��$	��0,0q�$��14(��� -   $ LangHighLight��$��1��$��Stretch=�$ Center=�
��� � �
�������$��Ar��$	��0,0q�$��14(��� -   $ CTIHighLight��$��2��$��Stretch=�$ Center=�
��� � �
�������$	��CTI��$	��0,0q�$��14(��� ���� *HorizontalAlignment+ �� Mode$ OneWayToSource=� �� RelativeSource �� *FindAncestor+ �� AncestorType" ��.+�� -   $ CallStatusBar��$��10,10q� �� $��1* ��� $��1* �.+��� $��0��$
��Left=�$��Call Status: Active��
��������� $��1��$��Right=�$��Call Duration: 00:00��
�������� -   $ CallDetails�� �� $��1* ��� $
��1.5* � �� $��1* ��� $��1* �.+��� $��0��$��0��$��Center=�$	��Top=�$
��18px(�
�������$��+966 5651 51517��$��Arial)�� -   $ CallActions��$��1��$��1��$��Bottom=� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� -   $ KeypadCallButton��$��0��$ 	Send DTMF��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.����  �� Orientation$ 
Horizontal=�.+��� $��Stretch=�$��Images/keypadcall.png���� -    $ HoldCallButton��$��1��$ 	Hold Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Stretch=�$��Images/holdcall.png���� -!   $ ResumeCallButton��$��1��$ Resume Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/resumecall.png���� -"   $ ConsultCallButton��$��2��$ Conference Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/conferencecall.png���� -#   $ TransferCallButton��$��3��$ Tranfer Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/transfercall.png���� -$   $ EndCallButton��$��4��$ End Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/releasecall.png���� -%   $ AnswerCallButton��$��4��$ Answer Call��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/answercall.png���� 
�������$��10,10q�.+��� 
�������$
	 Fill=�$
 1��� -&   $ SendCallToIVR��$	��0,5q� �� $��1* ��� $��2* ��� $
��0.4* � �� $��1* ��� $��1* �.+��� $��0��$��0��$��Send to IVR��
�������$��Center=��� $��0��$��1��$��Center=�$��Center=�
�������.@��� $��Please Make Selection�� �� 
IsSelected . �� $��	Main Menu���� $��Billing Menu���� -'   $ IVRCallButton��$��0��$��2��$ Send Call To IVR��$��Center=�$��Center=�$��35��$��35������ " ��.R�� .+��� $��Center=�$��Center=�.���� $ 
Horizontal=�.+��� $��Images/ivrcall.png���� $��1��$��0��$��	Male Only��
������� d� 	IsChecked$
 TrueY��� $��1��$��1��$��2��$
��Left=�.+�	� $��Inquiry��
�������$
 TrueY�	� $��	Complaint��
��������� 
�������$
	 Fill=�$
 1���� -(   $ RJBCallInformation��$	��0,5q�.+��� -)   $ 
SkillLabel��$��Skill: ��$
��Left=�$��Center=�
��������� -*   $ POSLabel��$��POS: ��$
��Left=�$��Center=�
��������� -+   $ LanguageLabel��$��
Language: ��$
��Left=�$��Center=�
��������� -,   $ IVRLabel��$��IVR: ��$
��Left=�$��Center=�
��������� --   $ CICLabel��$��CIC: ��$��Stretch=�$��Center=�
��������� -.   $ DialedNumberLabel��$��Dialed Number: ��$
��Left=�$��Center=�
��������� -/   $ LoginScreen��$y�Hidden=�.+�� -0   $ LoginMessageBar��$��Stretch=�$y�Hidden=� �� $��1* ��� $��6* ��� $��1* �.+��� -1   $ UserMessageImageLogin��$��0��$��Images/Error.png��$��20��$��20���� -2   $ UserMessageTextLogin��$��1��$��Stretch=�
���  ��
�������$ ��Error connecting to server��$	��5,0q��� -3   $��2��$ Close the error bar������ " ��.R�� .+��� $��Center=�$��Center=�.���� -4   $ UserCloseImageLogin��$��Images/close.png��$��20��$��20��$��Right=�� -5   $ LoginScreenForm��$��10,30q� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* � �� $
��0.5* ��� $��1* �.+��� -6    ��Name$ AgentIDLabel��$��0��$��0��$��Agent ID��$
��Left=�$��Center=��� -7   $ AgentIDInfo��$��0��$��1��$�� ��$��Stretch=�$��Center=��� -8   $ DomainLabel��$��1��$��0��$��Password��$
��Left=�$��Center=�$��26��G� -9    G�Name$ PasswordInfo��$��1��$��1�� G� Password$  ��$��Stretch=�$��Center=��� -:   $ AgentExtLabel��$��2��$��0��$��	Extension��$
��Left=�$��Center=��� -;   $ AgentExtInfo��$��2��$��1��$�� ��$��Stretch=�$��Center=��� -<   $ ServerSideA��$��3��$��0��$��Server Side A��$
��Left=�$��Center=�$y�Hidden=��� -=   $ DomainAInfo��$��3��$��1��$ ��hofinesseaprd.alrajhi.bank��$��Stretch=�$��Center=�$y�Hidden=��� ->   $ ServerSideB��$��4��$��0��$��Server Side B��$
��Left=�$��Center=�$y�Hidden=��� -?   $ DomainBInfo��$��4��$��1��$ ��hofinessebprd.alrajhi.bank��$��Stretch=�$��Center=�$y�Hidden=��� -@   $ LoginButton��$��5��$��0��$��2��$��Login��$��Center=�$��Center=�
���   �
�������$��Black"�$
��14px(�$��60��$
��0,20q��� $��Production Version 1.80��$��Bottom=�
�������$��Center=��� -A   $ LoginLoadingScreen��$y�Hidden=�.+��� '  !XamlAnimatedGif.AnimationBehavior    	SourceUri$  Images/Loading.gif?�$	��150��$	��150��$
��0,50q��� -B   $ CurrentLoadingStep��
�������$��XMPP Connection��$��Center=�