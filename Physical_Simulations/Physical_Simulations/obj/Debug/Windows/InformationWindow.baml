   M S B A M L   `   `   `  ����e  aMicrosoft.Web.WebView2.Wpf, Version=1.0.2420.47, Culture=neutral, PublicKeyToken=2a8ab48044d2601ekLclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.WpfMicrosoft.Web.WebView2.Wpf  5   	   O KPhysical_Simulations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Physical_Simulations.WindowsPhysical_Simulations.Windows 5   	   4   .Physical_Simulations.Windows.InformationWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   \wv2Lclr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf     5   	   <local*clr-namespace:Physical_Simulations.Windows     5   	     � Title$$  Information about this project��5	   	   $	��500��60   $	��900��6=    � 
ResizeMode$ NoResize=�6I    � WindowStartupLocation$ CenterScreen=�6_   
���   �.��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��200 �6   .+�5   
   �� $��0��6   
�������$��2q�64   ._�5      )   #Microsoft.Web.WebView2.Wpf.WebView2  -   5        $��webView��5      5      �� 5   
   $��1��6   $��10q�6%   .+�5      �� -   5        $��tbInformationText��5      
�������$
��Wrap=�6D   $	��390��6X   5      �� 5   
   -   5        $��btnPageLeft��5      $��40��6$   $��40��6/   $��59,0,101,10.4q�6;   $��1��6R   $��Center=�6b   $��Bottom=�6    � ToolTip$ 	Page left��6�   ��5      �� 5      .R�5      � .+�5       A� ���	#FFDDDDDD
���   �$��2��6B   F� 5!      
���   �$��2��6.   "���� -1��� р��  -1� -1@x}	$��Center=�6a   $��Center=�6~   $��
0,-10,10,0q�6�   5"      5#      5%      �� 5'   
   -   5        $��btnPageRight��5'      $��40��6%   $��40��60   $��104,0,56,10.4q�6<   $��1��6S   $��Center=�6c   $��Bottom=�6�   $ 
Page right��6�   ��5(      �� 5)      .R�5*      � .+�5+      A� ���	#FFDDDDDD
���   �$��2��6B   F� 5,      
���   �$��2��6.   "������� ��� � -1 -1р�� @x}	$��Center=�6a   $��Center=�6~   $��	0,-10,5,0q�6�   5-      5.      50      51      52      