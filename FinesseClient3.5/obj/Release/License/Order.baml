   M S B A M L   `   `   `  ���� T  PFinesseClient, Version=3.0.0.1, Culture=neutral, PublicKeyToken=179d4bde56bd90a5=#clr-namespace:FinesseClient.LicenseFinesseClient.License  !    FinesseClient.License.Order   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FinesseClient.License       � WindowStartupLocation$  CenterScreen=� � Title$ FCL Ordering�� � 
ResizeMode$ NoResize=� � SizeToContent$ WidthAndHeight=�.���� .+�� $��10,10q� � ColumnDefinitions �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��10,10q�$��1q�
���   � �� Padding$ 10,10q�._��� .+��� $��	Developer��$��Center=�$��24(�$��Black"��� $��1 User��$��Center=�$��20(��� $��1000 USD / Annual��$��Center=�$��16(�	� -   $��	Developer��$��Center=�$
��5,10q� 	� 	GroupName$ Package�� d� 	IsChecked$
 TrueY��� $��1��$��10,10q�$��1q�
���   �$ 10,10q�._��� .+��� $��Basic��$��Center=�$��24(�$��Black"��� $��10 Users��$��Center=�$��20(��� $��2500 USD / Annual��$��Center=�$��16(�	� -   $��Basic��$��Center=�$
��5,10q�$ Package���� $��2��$��10,10q�$��1q�
���   �$ 10,10q�._��� .+��� $��
Enterprise��$��Center=�$��24(�$��Black"��� $��	100 Users��$��Center=�$��20(��� $��5000 USD / Annual��$��Center=�$��16(�	� -   $��
Enterprise��$��Center=�$
��5,10q�$ Package���� $��3��$��10,10q�$��1q�
���   �$ 10,10q�._��� .+��� $��	Unlimited��$��Center=�$��24(�$��Black"��� $��	Unlimited��$��Center=�$��20(��� $��10,000 USD / Annual��$��Center=�$��16(�	� -   $��	Unlimited��$��Center=�$
��5,10q�$ Package��� $��Center=�$��0,0,0,20q�.+��� -   $��Submit��$
��30,5q�����  �� Path$ SubmitCommand�