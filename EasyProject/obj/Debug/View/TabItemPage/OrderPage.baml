   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:EasyProject.ViewModelEasyProject.ViewModel  5      3clr-namespace:EasyProject.ViewEasyProject.View  5           EasyProject.View.OrderPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2vm#clr-namespace:EasyProject.ViewModel     5      0localclr-namespace:EasyProject.View     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	        N� Title$  	OrderPage��5       � DataContext 5      �� *PSVM+   Locator �� Source# [  .��5      � -   5         �Name$ NewPlaceOrder��5   $   $	��450��6   $	��800��6    � ColumnDefinitions 5   
   �� 5      $	��89* �6   �� 5      $
��311* �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��10,10,10,10q�6      UniformCornerRadius$ 20Q�65   
�������$��2��6a   .��5      � $	��400��6   $	��750��6    
������� � RowDefinitions 5      �� 5      $��3.6028797018964E+15* �6$   �� 5      $��9.00719925474099E+15* �6$   �� 5      $��1.08086391056892E+16* �6$   �� 5      $��1.44115188075856E+16* �6$   �� 5      $��3.6028797018964E+15* �6$   .+�5      � $��3��6   ���#4472C4$��	0,139,0,0q�69   $��2��6L   �� 5       -   5         ��Name$ printBtn��5       $��인쇄��6+   ���#4472C4
�������$��12(�6a   $
��Bold"�6o   $
��Left=�6�   $	��Top=�5!      $��672,136,0,0q�6-   $��0q�6B   $��28��6V   $��58��6b    � Cursor$
 Hand��6m   $��3��6{   $��2��6�   6�   � 5$      $��0��6   ���#4472C4$��0,0,0,87q�69   $��2��6K   .+�5%      �� $
��Bold"�6    $��15(�62   	 �� TextAlignment$
	 left=�6@   $��
23,0,628,0q�6U   $��Center=�6i   �� 6�   
�������$�발주 신청서��6�   
 �� 	IsEnabled
 .  6�   6�   �� 5(      $��5q�6"    �� Orientation$ Vertical=�6-   $��Center=�6D   $��Center=�6_   .+�5)      �� -   5        $ resetBtn��5)   !   
����������#4472C4$��12(�6\   $
��Bold"�6j   $
��Left=�6|   $��Center=�5*      $��625,5,0,2.2q�64   $��0q�6I   $��28��6]   $��58��6i   $
 Hand��6t   .��5+      '  !MaterialDesignThemes.Wpf.PackIcon  $��0��66      Kind$ Restore=�6C   $��Bottom=�6R   $��Center=�6m   $��15��6�   $��15��6�   5,      5-      5/      � 50      $��1��6    51      �� 52      $
��0.6* �6(   �� 53      $
��0.4* �6(   .+�55      � $��0��6   .+�56      �� 
�������$
��Left=�61   $��10��6L   $��0,26,0,0q�6X   ���#4472C4$	��Top=�6{   $��10��6�   �� 57      $��12(�6%   $
��Bold"�63   $
��Left=�6E   $��	22,23,0,0q�6`   $
��Wrap=�6s   $��신청자 정보��6�   $	��Top=�6�   ���#4472C46�   �� 59      $ 
Horizontal=�6%   $��Right=�6>   .+�5:      �� $��	담당자��6$   ���#4472C4$��0��6G   $��12(�6W   $
��Bold"�6e   $
��Left=�6w   $��Center=�5;      $��10,0,0,0q�63   �� 5<      -   5         ��Name$ 	id_TxtBox��5<   (      MaterialDesignBody1TextBlock#���  6;   
 � Tag$ 	아이디��6r   $��1��6}   $��12(�5=      $��Center=�6%   $��	10,5,10,0q�6@   $��30��6S   $
��Bold"�6_   ��5>      �� *SelectedUser.Nurse_name+�� 5A      $��부서��6$   $��2��61   ���#4472C4$��12(�6V   $
��Bold"�6d   $
��Left=�6v   $��Center=�5B      $��10,0,0,0q�63   �� 5C      -   5        $ dept_TxtBox��5C   (   #���  6=   $ 부서��6t   $��3��6~   $��12(�5D      $��Center=�6#   $��	10,5,10,0q�6>   $��30��6Q   $
��Bold"�6]   ��5E      �� *SelectedUser.Dept_name+�� 5H      $��	연락처��6$   $��4��62   ���#4472C4$��12(�6W   $
��Bold"�6e   $
��Left=�6w   $��Center=�5I      $��10,0,0,0q�63   �� 5J      -   5        $ phone_TxtBox��5J   (   #���  6>   $ - 빼고입력��6u   $��5��6�   $��12(�5K      $��Center=�6'   $��	10,5,10,0q�6B   $��30��6U   $
��Bold"�6a   ��5L      �� *SelectedUser.Dept_phone+5O      5Q      5S      � 5U      $��2��6    5V      �� 5W      $
��0.3* �6(   �� 5X      $
��0.7* �6(   .+�5Z      � $��0��6   .+�5\      �� $��12(�6%   $
��Bold"�63   $
��Left=�6E   $��	23,13,0,0q�6`   $
��Wrap=�5]   !   $��발주 정보��65   $	��Top=�6B   ���#4472C46Z   �� 5^      
�������$
��Left=�61   $��10��6L   $��0,16,0,0q�5_       ���#4472C4$	��Top=�6C   $��10��6[   5`      � 5b      $��1��6    5c      �� 5d      $	��50* �6/   �� 5e      $	��50* �6/   �� 5f      $	��50* �6/   �� 5g      $	��50* �6/   �� 5h      $	��50* �6/   �� 5i      $	��50* �6/   �� 5j      $	��50* �6/   �� 5k      $	��50* �6/   �� 5l      $	��40* �6/   �� 5m      $	��50* �6/   .+�5q      �� $��제품코드��6    ���#4472C4$��12(�6D   $
��Bold"�6R   $
��Left=�6d   $��Center=�5r      $��55��64   $��22,0,0,0q�6?   $��2��6Q   �� 5s      -   5         ��Name$ prodcode_TxtBox��5s   "     WatermarkTextBox#���  6;   $ 제품코드��6f   $��1��6r   $��Center=�5t      $��12(�6:   $��Center=�6H   $��30��6c   $��10,0,0,0q�6o   �� 5u      $��	제품명��6    ���#4472C4$��2��6C   $��12(�6S   $
��Bold"�6a   $��Center=�6s   $��Center=�5v      �� 5w      -   5        $ prodname_TxtBox��5w   "   #���  6;   $ 	제품명��6f   $��3��6q   $��Center=�6�   $��Center=�5x      $��12(�6:   $��Center=�6H   $��30��6c   �� 5y      $��품목종류��6    ���#4472C4$��4��6D   $��12(�6T   $
��Bold"�6b   $��Center=�6t   $��Center=�5z      �� 5{      -	   5        $ categoryname_TxtBox��5{   "   #���  6?   $ 품목종류��6j   $��5��6v   $��Center=�6�   $��Center=�5|      $��12(�6:   $��Center=�6H   $��30��6c   �� 5}      $��제품가격��6    ���#4472C4$��6��6D   $��12(�6T   $
��Bold"�6b   $��Center=�6t   $��Center=�5~      �� 5      -
   5        $ prodprice_TxtBox��5   "   #���  6<   $ 제품가격��6g   $��7��6s   $��Center=�6�   $��Center=�5�      $��12(�6:   $��Center=�6H   $��30��6c   �� 5�      $��수량��6    ���#4472C4$��8��6B   $��12(�6R   $
��Bold"�6`   $��Center=�6r   $��Center=�5�      �� 5�      -   5        $ prodcount_TxtBox��5�   "   #���  6<   $ 수량��6g   $��9��6q   $��Center=�6�   $��Center=�5�      $��12(�6:   $��Center=�6H   $��30��6c   $��5,0,0,0q�6o   $
��Left=�6�   $��72��6�   5�      5�      � 5�      $��3��6    5�      �� 5�      $
��0.4* �6(   �� 5�      $
��0.6* �6(   .+�5�      � $��0��6   .+�5�      �� $��12(�6%   $
��Bold"�63   $
��Left=�6E   $��	25,25,0,0q�6`   $
��Wrap=�6s   $��메모��6�   $	��Top=�6�   ���#4472C46�   �� 5�      
�������$
��Left=�61   $��10��6L   $��0,28,0,0q�6X   ���#4472C4$	��Top=�6{   $��10��6�   5�      � 5�      $��1��6   .+�5�      �� -   5        $ memo_TxtBox��5�   "   #���  67   $" 내용을 입력해주세요��6b   $
��Left=�6u   $��Center=�5�      $��9(�6:   $	��Top=�6G   $��65��6_   $��0,5,0,0q�6k   5�      5�      �� 5�      -   5        $ pdfBtn��5�      $	��PDF��6)   ���#4472C4
�������$��12(�6`   $
��Bold"�6n   $
��Left=�6�   $	��Top=�5�      $��596,136,0,0q�6)   $��0q�6>   $��28��6R   $��58��6^   $
 Hand��6i   $��3��6w   $��2��6�   6�   5�      5�      F� 5�   
   
���   �$��1��6   $��2��62   ��5�      �� 5�       �� 
StartPoint$ 25,130&�6    �� EndPoint$ 775,130&�63   5�      F� 5�   
   
���   �$��1��6   $��2��62   ��5�      �� 5�      $ 25,260&�6   $ 775,260&�63   5�      '  !MaterialDesignThemes.Wpf.Snackbar  5�   
   -   5          Name$ Snackbar��5�   "   $��Stretch=�64   $��2��6R   $��8,0,8,0q�6f   ��5�      �� 5�       �� 
TargetType"  6    �� Triggers 5�      �� 5�       �� Binding 6&   �� *IsInOutEnabled+ �� Value True6I   .a�5�      ��  �� Property �@ MessageQueue �� Value 6=   �� *MessageQueue+6^   �� 5�       � �� �#Ea4a2d6M   5�      �� 5�       6&   �� *IsInOutEnabled+	 False6I   .a�5�      ��  �@ MessageQueue 6=   �� *MessageQueue+6^   �� 5�       � ��
 � � �6K   5�      5�      5�      5�      5�      