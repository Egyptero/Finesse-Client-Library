   M S B A M L   `   `   `  ���� T  PFinesseClient, Version=3.0.0.1, Culture=neutral, PublicKeyToken=179d4bde56bd90a5=#clr-namespace:FinesseClient.LicenseFinesseClient.License  #    FinesseClient.License.License   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FinesseClient.License       � WindowStartupLocation$  CenterScreen=� � Title$ Finesse Client Connector�� � 
ResizeMode$ NoResize=� � SizeToContent$ WidthAndHeight=�.���  � ColumnDefinitions �� $	��77* ��� $	��18* �.+��� $��10,10q�$��2��.+��� $��Center=�.+��� $��Finesse Client Library��$
��Bold"�$��24(��� $��Center=�.+��� $��Version 2.2.9.6���� $��Center=�.+��� $��Created by FIRE M.I.S.C��$
��Bold"��� $��Center=�.+��� $��www.firemisc.com��� -   $��UserInformation��$��10,10q� �� $
��auto ��� $��1* ��� $
��auto ��� $��1* � � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto �.+��� $
��Name��$��0��$��0��$	��5,5q��� $��1��$��3��$��0��$	��5,5q����� *Name+�� $��Email��$��0��$��1��$	��5,5q��� $��1��$��1��$	��5,5q����� *Email+�� $��Phone��$��2��$��1��$	��5,5q��� $��3��$��1��$	��5,5q����� *Phone+�� $��Company Name��$��0��$��2��$	��5,5q��� $��1��$��3��$��2��$	��5,5q����� *CompanyName+� $
��40,0q� �� $��* ��� $��* �.+�� $��0��� $��1��$��Right=�.+��� $��
Free trial��$	��5,5q�$	��100������  �� Path$ FreeTrialCommand��� $��Order License��$��1��$	��5,5q�$	��100������ $ OrderCommand��� $��Center=�.+��� ${��uYou can request 30 days free trials. To extend your trial , you may contact us by sending email to maref@firemisc.com��� $
��10,5q� �� $
��auto ��� $��1* ��� $
��auto � �� $
��auto ��� $
��auto �.+��� $	��Key��$��0��$��0��$	��5,5q��� $��0��$��1�� �� MaxLines$ 4��$	��5,5q� � ToolTip$ User Key��$	��500��	 �� TextWrapping$
	 Wrap=����� *Token+� $��0��$��2��.+��� $��Activate��$	��5,5q�$��80������ $ ActivateCommand��� $��License��$��1��$��0��$	��5,5q��� $��1��$��1��$ 4��$	��5,5q�$ License key��$	��500��$
	 Wrap=����� *	License+� $��1��$��2��.+��� -   $
��Save��$	��5,5q�$��80������ $ SaveCommand�