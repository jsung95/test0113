   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:EasyProjectEasyProject  5   	       EasyProject.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:EasyProject     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$   재고 관리 프로그램��5	   	   
�������$��2q�64   $
��1300��6H    � WindowStyle$
 None=�6U    � AllowsTransparency .  � 
ResizeMode$ CanResizeWithGrip=�6�    J� InputBindings 5
      �� 5   
    �� 	Modifiers$	 Altf�6   
 �� Key$ F4��6%    �� Command 6.   �� *Exit_Command+.��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   � $��30��6   $	��Top=�6   
�������	 � ColumnDefinitions	 5      �� 5      $��2* �6#   60   �� 5      $	��10* �6#   61   �� 5      $��2* �6#   60   .+�5      �� $)��#ezCaretech Inventory Management 1.0��6   $��0��6D   
�������$
��Left=�6g   $��Center=�6�   $��15(�6�   $��Champagne & Limousines)�6�   $	��264��6�   $��10,0,0,0q�6�   $��2��6�   � 5      $��2��6   .+�5      �� 
 �� Orientation$
 
Horizontal=�6   $
��10 0q�66   $��Center=�6D   $��Right=�6_   .+�5      �� -   5        $��Center=�5   2   $��4��6O   $��Center=�6_   ��6z   Z� ��6�   Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ WindowMinimize=�62   
�������6H   5      �� 5      -   5        $��Center=�5   0   $��4��6M   $��Center=�6]   ��6x   Z� ��6�   Z� .��5         $ 
DockWindow=�62   
�������6D   5!      �� 5"      -   5        $��Center=�5"   /   $��4��6L   $��Center=�6\   ��6w   Z� ��6�   Z� .��5#        $ Power=�62   
�������6?   5$      5%      5&      5'      � 5(   
   $��1��6   .+�5)       �   � NavigationUIVisibility$ Hidden=�6     � Source$ LoginPage.xaml?�64   5*      5+      5,      