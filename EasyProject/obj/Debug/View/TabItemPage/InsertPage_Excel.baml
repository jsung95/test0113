   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EasyProject.View.TabItemPageEasyProject.View.TabItemPage  5      3    -EasyProject.View.TabItemPage.InsertPage_Excel   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:EasyProject.View.TabItemPage     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  InsertPage_Excel��5	      .��5      � $	��450��6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��10,10,10,10q�6.      UniformCornerRadius$ 20Q�6D   
�������.��5      �  � RowDefinitions 5      �� 5      $��2* �6$   .+�5      � $��20,20,20,20q�6   ���	#FFE5E5E5 J� 	AllowDrop .  5      �� 5      $
��0.3* �6(   �� 5      $��* �6(   �� 5      $
��0.3* �6(   .+�5      �� $��1��6!   $��5q�6.    �� Orientation$ Vertical=�69   $��Center=�6P   $��Center=�6k   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  $��0��62      Kind$ filecsv=�6?   $��Bottom=�6N   $��Center=�6i   $��60��6�   $��60��6�   �� 5      $��Center=�6    $��Center=�6;   $��SemiBold"�6X   .��6n   !csv파일을 선택해주세요6}   �� 5      $
��0 20q�6%   $ 
Horizontal=�63   $��Center=�6L   $��Center=�6g   .+�5      �� -   5         ��Name$ fileUploadBtn��5   %   $
��20 0q�6<   $��1��6K   
����ح�$��13(�6o   $
��Bold"�6}   $��Center=�6�   $	��Top=�5      $��0q�60   $��30��6D   $	��120��6P    � Cursor$
 Hand��6\   .��5   "   �� $ 
Horizontal=�6-   .+�5    &     $ FileDownloadOutline=�6>   $��Center=�6Y   $��20��6t   $��20��6�   �� 5!   &   $��파일 다운받기��60   $��10(�6?   $��Center=�6M   5"   #   5#      �� 5$      -   5        $ fileDownLoadBtn��5$   %   $
��20 0q�6>   $��1��6L   
������$��13(�6q   $
��Bold"�6   $��Center=�6�   $	��Top=�5%      $��0q�60   $��30��6D   $	��120��6P   $
 Hand��6\   .��5&   "   �� $ 
Horizontal=�6-   .+�5'   &     $ FileUploadOutline=�6>   $��Center=�6W   $��20��6r   $��20��6~   �� 5(   &   $��파일 선택하기��60   $��10(�6?   $��Center=�6M   5)   #   5*      5+      5-      5.      5/      50      51      52      