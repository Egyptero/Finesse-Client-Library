   M S B A M L   `   `   `  ����D  @FinesseDE, Version=2.2.8.1, Culture=neutral, PublicKeyToken=null%clr-namespace:FinesseDE	FinesseDE  5   	       FinesseDE.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:FinesseDE     5   	   -   5          � Title$  FGDP V2.281��5	   	   $	��350��6   $	��350��6,    � 
ResizeMode$ NoResize=�6;    � WindowStartupLocation$ CenterScreen=�6Q    � SizeToContent$ WidthAndHeight=�6v   ��5
      �� 5   
    �� ImageSource$ Images/background.jpg��6    j� Stretch$
 Fill=�69   .��5      �  � RowDefinitions 5   
   �� 5      $��4* �6   �� 5      $��1* �6   .+�5   
   � $��0��6   $��10,10q�6   .+�5      �� .+�5      � $��
10,10,10,2q�6    � ColumnDefinitions 5      �� 5      $��1* �6+   �� 5      $��2* �6+   .+�5      �� $��0��6   $��Agent ID��6,   
�������6?   �� 5      $��1��6   $��22��6]   ��6.   ��  �� Path$ AgentInformation.AgentID�6]   5      � 5      $
��10,2q�6    5      �� 5      $��1* �6+   �� 5      $��2* �6+   .+�5!      �� $��0��6   $��Password��6,   
�������6?   G� 5"      -   5        $��Password��5"   2   $��1��6"   $��22��6B   5#      � 5$      $
��10,2q�6    5%      �� 5&      $��1* �6+   �� 5'      $��2* �6+   .+�5)      �� $��0��6   $��	Extension��6,   
�������6@   �� 5*      $��1��6   $��22��6_   ��6.   �� $  AgentInformation.Extension�6_   5+      9� 5,      $��Details��6   $��
10,5,10,10q�6,   
������ 
�������.��5-      �� 
������ .+�5.      � $	��0,2q�6    5/      �� 50   "   �� 51   "   �� 52   "   �� 53   "   �� 54   "   �� 55   "    57      �� 58   "   $��1* �63   �� 59   "   $��2* �63   .+�5;      �� $��0��6$   $��0��64   $��Server A��6A   
�������6T   �� 5<      $��1��6&   $��0��66   $��22��6s   ��6D   �� $ AgentInformation.DomainA�6s   �� 5=      $��0��6$   $��1��64   $��Server B��6A   
�������6T   �� 5>      $��1��6&   $��1��66   $��22��6r   ��6C   �� $ AgentInformation.DomainB�6r   �� 5?      $��0��6$   $��2��64   $��	XMPP Port��6A   
�������6U   �� 5@      $��1��6&   $��2��66   $��22��6s   ��6C   �� $ AgentInformation.XMPPPort�6s   �� 5A      $��0��6$   $��3��64   $��XMPP Url Suffix��6A   
�������6[   �� 5B      $��1��6&   $��3��66   $��22��6r   ��6C   �� $ AgentInformation.XMPPURL�6r   �� 5C      $��0��6$   $��4��64   $��	Http Port��6A   
�������6U   �� 5D      $��1��6&   $��4��66   $��22��6s   ��6C   �� $ AgentInformation.HTTPPort�6s   �� 5E      $��0��6$   $��5��64   $��Http Url Suffix��6A   
�������6[   �� 5F      $��1��6&   $��5��66   $��22��6r   ��6C   �� $ AgentInformation.HTTPURL�6r   5G      �� 5H      -   5        $	��SSL��5H   #   $	��SSL��6.   $��	10,2,10,5q�6<   
�������	 d� 	IsChecked	 6b   �� $ AgentInformation.SSL�� 5I      $��	10,2,10,5q�6$   y�67   �� $ AgentInformation.SSL�   BoolToVisConverter
 �� 	Converter#
 [  .+�5J      �� $��XMPP SSL��6$   
�������$	��100��6J   	� 5K      $��Ssl3 ��6*   
�������$	��0,5q�6M   	 6Z   �� $) #AgentInformation.XMPPConnectionType�  ParamToStringConverter#
 [  �� ConverterParameter$
 Ssl3��	� 5L      $
��Tls ��6*   
�������$	��0,5q�6L   	 6Y   �� $) #AgentInformation.XMPPConnectionType�#
 [ $	 Tls��	� 5M      $��Tls11 ��6*   
�������$	��0,5q�6N   	 6[   �� $) #AgentInformation.XMPPConnectionType�#
 [ $ Tls11��	� 5N      $��Tls12 ��6*   
�������$	��0,5q�6N   	 6[   �� $) #AgentInformation.XMPPConnectionType�#
 [ $ Tls12��5O      � 5P      $��
10,2,10,10q�6$   y�68   �� $ AgentInformation.SSL�#
 [  .+�5Q      �� $��HTTP SSL��6$   
�������$	��100��6J   	� 5R      $��Ssl3 ��6*   
�������$	��0,5q�6M   	 6Z   �� $) #AgentInformation.HTTPConnectionType�#
 [ $
 Ssl3��	� 5S      $
��Tls ��6*   
�������$	��0,5q�6L   	 6Y   �� $) #AgentInformation.HTTPConnectionType�#
 [ $	 Tls��	� 5T      $��Tls11 ��6*   
�������$	��0,5q�6N   	 6[   �� $) #AgentInformation.HTTPConnectionType�#
 [ $ Tls11��	� 5U      $��Tls12 ��6*   
�������$	��0,5q�6N   	 6[   �� $) #AgentInformation.HTTPConnectionType�#
 [ $ Tls12��5V      5W      5X      �� 5Y      -   5        $��SavePassword��5Y      $��Save password��6/   $��
10,2,10,10q�6G   
�������6[   5Z      5[      � 5\   
   $��1��6   $��30,10q�6    5]      �� 5^      $��1* �6#   �� 5_      $��1* �6#   .+�5a      �� -   5        $��0��5a      $��Close��6%   $
��left=�65   $��Center=�6P   $
��20,3q�6k   $��	ExtraBold"�6G  ��6z   ��   StatusbarBackground �� Source# [  �� UpdateSourceTrigger$ PropertyChanged=���6�   ��   StatusbarForground# [ $ PropertyChanged=�6^  �� 5b      -   5        $��1��5b      $��Login��6%   $��Right=�65   $��Center=�6Q   $
��20,3q�6l   $��	ExtraBold"�6H  ��6{   �� # [ $ PropertyChanged=���6�   �� # [ $ PropertyChanged=�6_  5c      5d      5e      