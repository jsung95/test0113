   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:EasyProjectEasyProject  5	      =#clr-namespace:EasyProject.ViewModelEasyProject.ViewModel  5
      3clr-namespace:EasyProject.UtilEasyProject.Util  5          EasyProject.DialogPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Idxwui7http://schemas.devexpress.com/winfx/2008/xaml/windowsui     5      UdxwuinBhttp://schemas.devexpress.com/winfx/2008/xaml/windowsui/navigation     5      Edxmvvm2http://schemas.devexpress.com/winfx/2008/xaml/mvvm     5      +localclr-namespace:EasyProject     5	      2vm#clr-namespace:EasyProject.ViewModel     5
      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      &wclr-namespace:EasyProject.Util  5        N� Title$  
DialogPage��5      .��5      � 
�������.+�5*   
   #  MaterialDesignThemes.Wpf.Card  $	��250��6   $	��500��6+   $��10q�67      UniformCornerRadius$ 20Q�6D   
�������.��5+   
   � $	��250��6   $	��500��6    � ColumnDefinitions 5-      �� 5.      $
��0.3* �6'   �� 5/      $
��0.3* �6'   �� 50      $
��0.3* �6'   .+�53      � $��0��6    � RowDefinitions 54      �� 55      $
��0.3* �6(   �� 56      $
��0.5* �6(   �� 57      $
��0.3* �6(   .+�59      �� -   5        $��InventoryRevise_Btn��59      $��1��68   $��Center=�6r   $��Center=�6�   $��80��6�   $��80��6�   ��6E   Z� ��6[   Z� .��5:      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 Gear=�62   
���   �$��60��6Q   $��60��6]   $��Center=�6h   $��Center=�6�   5;      �� 5<      -   5        $��modify��5<   -   $��2��6    $��재고수정��6;   $��Center=�6G   $��Center=�6b   $��15(�6   6�   5=      � 5?      $��1��6    5@      �� 5A      $
��0.3* �6(   �� 5B      $
��0.5* �6(   �� 5C      $
��0.3* �6(   .+�5E      �� -   5        $��Release_Btn��5E      $��1��60   $��Center=�6j   $��Center=�6�   $��80��6�   $��80��6�   ��6=   Z� ��6S   Z� .��5F        $ Arrow=�62   
���   �$��60��6R   $��60��6^   $��Center=�6i   $��Center=�6�   5G      �� 5H      -   5        $��release��5H   -   $��2��6    $��출고(사용)��6<   $��Center=�6J   $��Center=�6e   $��15(�6�   6�   5I      � 5K      $��2��6    5L      �� 5M      $
��0.3* �6(   �� 5N      $
��0.5* �6(   �� 5O      $
��0.3* �6(   .+�5Q      �� -   5        $��goOrder_Btn��5Q      $��1��60   $��Center=�6j   $��Center=�6�   $��80��6�   $��80��6�   ��6=   Z� ��6S   Z� .��5R        $ pencil=�62   
���   �$��60��6S   $��60��6_   $��Center=�6j   $��Center=�6�   5S      �� 5T      -   5        $��
order_form��5T   -   $��2��6    $��발주신청서 작성��6?   $��Center=�6O   $��Center=�6j   $��15(�6�   6�   5U      F� 5W      
���   �$��1��6&   ��5X      �� 5Y       �� 
StartPoint$ 750,10&�6'    �� EndPoint$ 750,1148&�6;   5[      5]      5^      5_      5`      