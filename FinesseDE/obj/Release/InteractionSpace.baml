   M S B A M L   `   `   `  ���� D  @FinesseDE, Version=2.1.1.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FinesseDE	FinesseDE       FinesseDE.InteractionSpace   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     )localclr-namespace:FinesseDE       � 	Resources     BoolToVisConverter��%   &	        �� .���� -   $��MySpace��$	��5,5q�$��2q�y���  �� Path$ IsInteractionSpace� �� 	Converter# [  ����   AgentStatusbarBackground �� Source# [  �� UpdateSourceTrigger$ PropertyChanged=�._��  � ColumnDefinitions �� -    �Name$ LeftCol��$��1* ��� -   $ RightRegion��$��3* �.+�� -   $��LeftArea��$��0�� � RowDefinitions �� $
��auto ��� $��2* ��� $
��auto ��� $��1* �.+�� -   $��LeftAreaUpper��$��0��$��Stretch=�.+��� $��
5,10,15,10q� � UseLayoutRounding . $��2q�	 �� CornerRadius$	 10������   GadgetBackground# [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�
 �� Padding$	
 2,2q�$	 10������   GadgetHeaderBackground# [ $ PropertyChanged=�._��� $��	Call Data��$
��Bold"�$	��5,2q�����   GadgetHeaderForground# [ $ PropertyChanged=�&   System.Windows.Controls.DataGrid     AutoGenerateColumns .  ��. $��0,0,0,5q�
������  J� Opacity$	 0.8Q�
������ ���� $C =AgentInformation.SelectedDialog.MediaProperties.CallVariables�  =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property �� Value��%�   '        7 7  7  �� 8   "  .���  � �� �� 8  $ PropertyChanged=���  � �� �� 8  $ PropertyChanged=���  � ��$	 3,3q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Call Variable��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $
 Name�  $ Value��$ *  �� $ Value�� -   $��LeftAreaMiddle��$��1��$��Stretch=�.+��� $��
5,10,15,10q� . $��2q�$	 10������ # [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Call Details��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=��� $	��2,2q�.+�� .+��� $
��DNIS��$	��100������   GadgetForground# [ $ PropertyChanged=��� ���� $: 4AgentInformation.SelectedDialog.MediaProperties.DNIS����� # [ $ PropertyChanged=�� .+��� $��	Call Type��$	��100������ # [ $ PropertyChanged=��� ���� $> 8AgentInformation.SelectedDialog.MediaProperties.CallType����� # [ $ PropertyChanged=�� .+��� $��Dialed Number��$	��100������ # [ $ PropertyChanged=��� ���� $B <AgentInformation.SelectedDialog.MediaProperties.DialedNumber����� # [ $ PropertyChanged=��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Participant��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=�   .  ��. $��0,0,0,5q�
������ $	 0.8Q�
������ ���� $2 ,AgentInformation.SelectedDialog.Participants�  ��%�   '        7 7  7  �� 8   "  .���  � �� �� 8  $ PropertyChanged=���  � �� �� 8  $ PropertyChanged=���  � ��$	 3,3q�   $ 	Extension��$
 auto  �� $ MediaAddress�  $ Status��$
 auto  �� $ State��� $��2��$
��auto��$��Stretch=� � Cursor$ SizeNS������  �� 
TargetType" ��.R�� .+��� ���	#FF407391$��Stretch=�$��10��$	 0.0Q���  �� EdgeMode$ Aliased=� . $��6��$��Center=�$��50��$��Stretch=�.+��� 	 �� X1$ 0��	 �� X2$ 50��	 �� Y1$ 0��	 �� Y2$ 0��
�������$��1���� $ 0��$ 50��$ 1��$ 1�����#A0A0A0$��1���� $ 0��$ 50��$ 4��$ 4��
�������$��1���� $ 0��$ 50��$ 5��$ 5�����#A0A0A0$��1��� -   $��LeftAreaBottom��$��3��$��Stretch=�.+��� $��
5,10,15,10q� . $��2q�$	 10������ # [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Customer Journey��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=��� $
��auto��$��Stretch=�$ SizeWE������ " ��.R�� .+��� 
������ $��Stretch=�$��10��$	 0.1Q��� $ Aliased=� . $��6��$��Center=�$��50��$��Stretch=�.+��� $ 0��$ 0��$ 0��$ 50��
�������$��1���� $ 1��$ 1��$ 0��$ 50�����#A0A0A0$��1���� $ 4��$ 4��$ 0��$ 50��
�������$��1���� $ 5��$ 5��$ 0��$ 50�����#A0A0A0$��1��� $��1�� �� -   $ 	MiddleCol��$��2* ��� -	   $ RightCol��$��1* �.+�� -
   $��
MiddleArea��$��0��.+�(  "System.Windows.Controls.WebBrowser  -   $��Browser��$y�Visible=�$��2,2,12,2q�$
��auto=�$
��Auto=�! �� IsDeferredScrollingEnabled .  ��.     Source$4 .https://www.google.com.eg/search?q=istnetworks?��� $
��auto��$��Stretch=�$ SizeWE������ " ��.R�� .+��� 
������ $��Stretch=�$��10��$	 0.1Q��� $ Aliased=� . $��6��$��Center=�$��50��$��Stretch=�.+��� $ 0��$ 0��$ 0��$ 50��
�������$��1���� $ 1��$ 1��$ 0��$ 50�����#A0A0A0$��1���� $ 4��$ 4��$ 0��$ 50��
�������$��1���� $ 5��$ 5��$ 0��$ 50�����#A0A0A0$��1��� -   $��	RightArea��$��1�� �� $
��auto ��� $��2* ��� $
��auto ��� $
��auto �.+�� -   $��RightAreaUpper��$��0��$��Stretch=�.+��� $��
5,10,15,10q� . $��2q�$	 10������ # [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Contact History��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=��� $��Light"�$��11(�   ��Name �� Triggers  P� Property! P� Value  TabHeaderForground" �� 
TargetName  TabHeaderBackground  SelectedHeaderTabForground 	 SelectedHeaderTabBackground��%�  '��       7  7  7  7 	 �� " ��.���  � �� �� " ��.R�� $��Panel��$	��1,0q�.+��� $ ContentSite��$��Center=�$��Center=�$��Header��$	��5,5q� P�   �@��
IsSelected! .  . ���  �� 8   $ PropertyChanged=� � ���� $" Panel�� �� 8  $ PropertyChanged=� � ��P�   �@��
IsSelected! . . ���  �� 8  $ PropertyChanged=� � ���� $" Panel�� �� 8  $ PropertyChanged=� � ��.@��� $��Contact Details��.��� ���� # [ $ PropertyChanged=�.+�� $��10,20q� �� $
��auto ��� $��* � �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0��$��
First Name��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��0��$�� ��$	��5,2q�$	��2,2q��� $��0��$��1��$��	Last Name��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��1��$�� ��$	��5,2q�$	��2,2q��� $��0��$��2��$��Phone��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��2��$�� ��$	��5,2q�$	��2,2q��� $��0��$��3��$��Mobile��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��3��$�� ��$	��5,2q�$	��2,2q��� $��0��$��4��$��Email��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��4��$�� ��$	��5,2q�$	��2,2q��� $��0��$��5��$��Facebook ID��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��5��$�� ��$	��5,2q�$	��2,2q��� $��0��$��6��$��
Twitter ID��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��6��$�� ��$	��5,2q�$	��2,2q��� $��0��$��7��$��Insta ID��$	��5,2q�$	��2,2q����� # [ $ PropertyChanged=��� $��1��$��7��$�� ��$	��5,2q�$	��2,2q��� $��History���� $��Search��.���� $��	Search...��� -   $��RightAreaMiddle��$��1��$��Stretch=�.+��� $��
5,10,15,10q� . $��2q�$	 10������ # [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Knowledge Base��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=��� $��2��$
��auto��$��Stretch=�$ SizeNS������ " ��.R�� .+��� ���	#FF407391$��Stretch=�$��10��$	 0.0Q��� $ Aliased=� . $��6��$��Center=�$��50��$��Stretch=�.+��� $ 0��$ 50��$ 0��$ 0��
�������$��1���� $ 0��$ 50��$ 1��$ 1�����#A0A0A0$��1���� $ 0��$ 50��$ 4��$ 4��
�������$��1���� $ 0��$ 50��$ 5��$ 5�����#A0A0A0$��1��� -   $��RightAreaBottom��$��3��$��Stretch=�.+��� $��
5,10,15,10q� . $��2q�$	 10������ # [ $ PropertyChanged=����� # [ $ PropertyChanged=�._��� .+��� $��0,0,0,5q�$	
 2,2q�$	 10������ # [ $ PropertyChanged=�._��� $��Notes��$
��Bold"�$	��5,2q����� # [ $ PropertyChanged=��� $�� ��# �� MinLines$# 5��$ �� MaxLines$$ 5��$��Stretch=�$��Stretch=�� $��Right=�$��10,10q�.+��� $
��Save��$	��5,0q�$��50������  
 StatusbarBackground# [
 $ PropertyChanged=�����   StatusbarForground# [ $ PropertyChanged=��� $��Cancel��$	��5,0q�$��50������ # [
 $ PropertyChanged=����� # [ $ PropertyChanged=�