   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EasyProject.View.TabItemPageEasyProject.View.TabItemPage  5      3    -EasyProject.View.TabItemPage.LogStatusPageBtn   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      6local*clr-namespace:EasyProject.View.TabItemPage  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5        N� Title$  LogStatusPageBtn��5
       � DataContext 5      �� *LGVM+   Locator �� Source# [  .��5      � $
��1200��6    � RowDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $
��2.5* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $��0 �6#   �� 5      $
��0.5* �6#   �� 5      $
��0.5* �6#   �� 5      $��* �6#   �� 5      $��0.65* �6#   �� 5      $��0.25* �6#   �� 5      $
��0.3* �6#   .+�5      �� -   5         ��Name$ EventLogBtn��5      $��전체 로그��6*   $��1��6:   ���#4472C4$��13(�6_   $
��Bold"�6m   
�������$��Center=�5       $
��Left=�60   $��0q�6L   $��30��6`   $	��130��6l    � Cursor$
 Hand��6x   �� 5!      -   5        $ LoginBtn��5!      $��접속 로그��6'   $��2��67   ���#4472C4$��13(�6\   $
��Bold"�6j   
�������$��Center=�5#      $
��Left=�60   $��10,0,0,0q�6K   $��0q�6]   $��30��6q   $	��130��6}   $
 Hand��6�   �� 5$      -   5        $ 	LogoutBtn��5$      $��종료 로그��6(   $��3��68   ���#4472C4$��13(�6]   $
��Bold"�6k   
�������$��Center=�5&      $
��Left=�60   $��10,0,0,0q�6K   $��0q�6]   $��30��6q   $	��130��6}   $
 Hand��6�   � 5'      $��4��6   $��40,0,0,0q�6#   5+      � 5,      $��5��6   .+�5-      �� -   5         ��Name$ DataGridCheckbox��5-       + 'MaterialDesignFilterChipOutlineCheckBox#��[ 65   $��4��6v   $��Right=�6�   $��	표보기��6�   $
 Hand��6�    d� 	IsChecked 6�   �� *IsDataGridCheckBoxChecked+6�   5.      � 5/      $��6��6   .+�50      �� -   5        $ GraphCheckbox��50      #��[ 62   $��5��6s   $��Right=�6�   $��그래프보기��6�   $
 Hand��6�    6�   �� *IsGraphCheckBoxChecked+6�   51      52      � 54   
   $��1��6   .+�55       � -   5        	  �Name$	 	ListFrame��55      
  � Source$
 LogStatusList1Page.xaml?�6(   $	��Top=�6I   56      57      58      