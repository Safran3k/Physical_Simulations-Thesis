   M S B A M L   `   `   `  ����O  KPhysical_Simulations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Physical_Simulations.WindowsPhysical_Simulations.Windows  5   	   7    1Physical_Simulations.Windows.CustomVelocityWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:Physical_Simulations.Windows     5   	     � Title$  Custom velocity values��5   	   $��162.4��6(   $��333.6��67    � WindowStartupLocation$ CenterScreen=�6E   
���   � � 
ResizeMode$ NoResize=�6}   .��5	      � .+�5
   
   �� $��0 5 0 5q�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Velocity X₀:��6   
�������$��Center=�6B   �� 5      -   5        $��sliderX��5       � Minimum$ -1000Q�6(    � Maximum$
 1000Q�68    �� TickFrequency$ 1Q�6G    �� IsSnapToTickEnabled . $	��220��6t   �� 5      $��25��6k   $��20��6v    �� 
IsReadOnly . ��6   �� *Value+	 �� ElementName$	 sliderX��
 �� UpdateSourceTrigger$
 PropertyChanged=�6�   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Velocity Y₀:��6   
�������$��Center=�6B   �� 5      -   5        $��sliderY��5      $ -1000Q�6(   $
 1000Q�68   $ 1Q�6G    . $	��220��6t   �� 5      $��25��6k   $��20��6v    . ��6   �� *Value+$	 sliderY��$
 PropertyChanged=�6�   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Velocity Z₀:��6   
�������$��Center=�6B   �� 5      -   5        $��sliderZ��5      $ -1000Q�6(   $
 1000Q�68   $ 1Q�6G    . $	��220��6t   �� 5      $��25��6k   $��20��6v    . ��6   �� *Value+$	 sliderZ��$
 PropertyChanged=�6�   5      �� 5      $ 
Horizontal=�6   $��Center=�62   .+�5      �� -   5        $��btnSave��5      $
��Save��6(   $��10q�67   $��70��6Y   �� 5      -   5        $��	btnCancel��5      $��Cancel��6*   $��10q�6;   $��70��6_   5      5      5      5       