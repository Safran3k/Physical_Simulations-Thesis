   M S B A M L   `   `   `  ����O  KPhysical_Simulations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Physical_Simulations.WindowsPhysical_Simulations.Windows  5   	   3    -Physical_Simulations.Windows.CustomMassWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:Physical_Simulations.Windows     5   	     � Title$  Custom mass values��5   	   $��104.8��6$   $��344.8��63    � WindowStartupLocation$ CenterScreen=�6A   
���   � � 
ResizeMode$ NoResize=�6y   .��5	      � .+�5
   
   �� $��0 5 0 5q�6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��
Body mass:��6   
�������$��Center=�6@   �� 5      -   5        $��sliderBodyMass��5       � Minimum$ 1Q�6/    � Maximum$
 5000Q�6;    �� TickFrequency$ 1Q�6J    �� IsSnapToTickEnabled . $	��220��6w   �� 5      $��35��6r   $��20��6}    �� 
IsReadOnly . ��6   �� *Value+	 �� ElementName$	 sliderBodyMass��
 �� UpdateSourceTrigger$
 PropertyChanged=�6�   5      �� 5      $ 
Horizontal=�6   $��Center=�62   .+�5      �� -   5        $��btnSave��5      $
��Save��6(   $��10q�67   $��70��6Y   �� 5      -   5        $��	btnCancel��5      $��Cancel��6*   $��10q�6;   $��70��6_   5      5      5      5      