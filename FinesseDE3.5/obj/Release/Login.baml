   M S B A M L   `   `   `  ���� D  @FinesseDE, Version=2.2.8.1, Culture=neutral, PublicKeyToken=null%clr-namespace:FinesseDE	FinesseDE      FinesseDE.Login   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:FinesseDE     -     � Title$  FGDP V2.281��$	��350��$	��350�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � SizeToContent$ WidthAndHeight=�����  �� ImageSource$ Images/background.jpg�� j� Stretch$
 Fill=�.���  � RowDefinitions �� $��4* ��� $��1* �.+�� $��0��$��10,10q�.+��� .+�� $��
10,10,10,2q� � ColumnDefinitions �� $��1* ��� $��2* �.+��� $��0��$��Agent ID��
��������� $��1��$��22������  �� Path$ AgentInformation.AgentID�� $
��10,2q� �� $��1* ��� $��2* �.+��� $��0��$��Password��
�������G� -   $��Password��$��1��$��22��� $
��10,2q� �� $��1* ��� $��2* �.+��� $��0��$��	Extension��
��������� $��1��$��22������ $  AgentInformation.Extension�9� $��Details��$��
10,5,10,10q�
������ 
�������.���� 
������ .+�� $	��0,2q� �� �� �� �� �� ��  �� $��1* ��� $��2* �.+��� $��0��$��0��$��Server A��
��������� $��1��$��0��$��22������ $ AgentInformation.DomainA��� $��0��$��1��$��Server B��
��������� $��1��$��1��$��22������ $ AgentInformation.DomainB��� $��0��$��2��$��	XMPP Port��
��������� $��1��$��2��$��22������ $ AgentInformation.XMPPPort��� $��0��$��3��$��XMPP Url Suffix��
��������� $��1��$��3��$��22������ $ AgentInformation.XMPPURL��� $��0��$��4��$��	Http Port��
��������� $��1��$��4��$��22������ $ AgentInformation.HTTPPort��� $��0��$��5��$��Http Url Suffix��
��������� $��1��$��5��$��22������ $ AgentInformation.HTTPURL��� -   $	��SSL��$	��SSL��$��	10,2,10,5q�
�������	 d� 	IsChecked	 �� $ AgentInformation.SSL�� $��	10,2,10,5q�y��� $ AgentInformation.SSL�   BoolToVisConverter
 �� 	Converter#
 [  .+��� $��XMPP SSL��
�������$	��100��	� $��Ssl3 ��
�������$	��0,5q�	 �� $) #AgentInformation.XMPPConnectionType�  ParamToStringConverter#
 [  �� ConverterParameter$
 Ssl3��	� $
��Tls ��
�������$	��0,5q�	 �� $) #AgentInformation.XMPPConnectionType�#
 [ $	 Tls��	� $��Tls11 ��
�������$	��0,5q�	 �� $) #AgentInformation.XMPPConnectionType�#
 [ $ Tls11��	� $��Tls12 ��
�������$	��0,5q�	 �� $) #AgentInformation.XMPPConnectionType�#
 [ $ Tls12��� $��
10,2,10,10q�y��� $ AgentInformation.SSL�#
 [  .+��� $��HTTP SSL��
�������$	��100��	� $��Ssl3 ��
�������$	��0,5q�	 �� $) #AgentInformation.HTTPConnectionType�#
 [ $
 Ssl3��	� $
��Tls ��
�������$	��0,5q�	 �� $) #AgentInformation.HTTPConnectionType�#
 [ $	 Tls��	� $��Tls11 ��
�������$	��0,5q�	 �� $) #AgentInformation.HTTPConnectionType�#
 [ $ Tls11��	� $��Tls12 ��
�������$	��0,5q�	 �� $) #AgentInformation.HTTPConnectionType�#
 [ $ Tls12���� -   $��SavePassword��$��Save password��$��
10,2,10,10q�
�������� $��1��$��30,10q� �� $��1* ��� $��1* �.+��� -   $��0��$��Close��$
��left=�$��Center=�$
��20,3q�$��	ExtraBold"�����   StatusbarBackground �� Source# [  �� UpdateSourceTrigger$ PropertyChanged=�����   StatusbarForground# [ $ PropertyChanged=��� -   $��1��$��Login��$��Right=�$��Center=�$
��20,3q�$��	ExtraBold"����� # [ $ PropertyChanged=����� # [ $ PropertyChanged=�