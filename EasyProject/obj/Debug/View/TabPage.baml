   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:EasyProjectEasyProject  5      E ADragablz, Version=0.0.3.223, Culture=neutral, PublicKeyToken=null5(clr-namespace:Dragablz;assembly=DragablzDragablz 5      =#clr-namespace:EasyProject.ViewModelEasyProject.ViewModel  5      5clr-namespace:EasyProject.ModelEasyProject.Model  66       EasyProject.TabPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:EasyProject     5      =dragablz(clr-namespace:Dragablz;assembly=Dragablz     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5
      ,vm#clr-namespace:EasyProject.ViewModel  5      +modelclr-namespace:EasyProject.Model  66     N� Title$  TabPage��5       � DataContext 5      *   $EasyProject.ViewModel.LoginViewModel  5   
   .��5      )  #MaterialDesignThemes.Wpf.DialogHost     DialogContent 5   
   �� 5      $��20q�6   .+�5      �  � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $
��Auto �6+   6:   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Error=�6.   ���#4472C4$��48��5   2   $��48��5   2   $��20, 25, 20, 20q�5   2   $��0��5   2   �� 5      $��20q�6!   $��1��6-   $	��180��6>   .+�5      �� $��로그아웃��6$   $��15(�60   $��	UltraBold"�6>   6W   �� 5       $&�� 로그아웃 하시겠습니까?��6$   $��13(�68   $��SemiBold"�6F   $��0,20,0,0q�5!   (   6<   5"      5#      �� 5$       �� Orientation$ 
Horizontal=�6   $��Center=�66   $��Center=�6S   .+�5%      �� $��취소��6   ���#EAEAEA
���   �   CloseDialogCommand#��Z 5&      6a   �� 5'      $��확인��6   ���#4472C4
�������$��30,0,0,0q�6R   ��5(      �� *LogoutCommand+6A   5*      5+      .��5-   
   �� $ Vertical=�6   
�������.+�5.      � $��40��6   .+�5/      �� $ 
Horizontal=�6   $	��Top=�66   .+�50      �� -   5         ��Name$ StatusPageTabButton��50   %   
 J� Uid$ 0��6   $��40��6B   $	��150��6N   ���	#ff2196f3$��재고 현황��6�   ���#F0EBE9	 � Cursor$
	 Hand��6�      MaterialDesignFlatButton#��[  6�   ��6�   Z� 6�   �� 51      -   5        $ GraphPageTabButton��51   %   $ 1��6   $��40��6A   $	��150��6M   ���	#ff2196f3$��그래프 현황��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   �� 52      -   5        $# IncomingOutgoingPageTabButton��52   %   $ 3��6   $��40��6L   $	��150��6X   ���	#ff2196f3$��입출고 현황��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   �� 53      -   5        $ InsertPageTabButton��53   %   $ 2��6   $��40��6B   $	��150��6N   ���	#ff2196f3$��재고 등록��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   �� 54      -   5        $ OrderPageTabButton��54   %   $ 4��6   $��40��6A   $	��150��6M   ���	#ff2196f3$��신규발주 신청��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   �� 55      -   5        $ AuthorityPageTabButton��55   %   $ 5��6   $��40��6E   $	��150��6Q   ���	#ff2196f3$��권한 변경��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   �� 56      -   5        $ LogPageTabButton��56   %   $ 6��6   $��40��6?   $	��150��6K   ���	#ff2196f3$��로그 현황��6�   $
	 Hand��6�   #��[  6�   ��6�   Z� ��6�   Z� 6�   57      �� 59      $ 
Horizontal=�6   $
��10 0q�66   $��Center=�6D   $��Right=�6_   .+�5>      �� -   5        $ 	logoutBtn��5>      $��35��60   $��90��6<   $��13(�6G   $��DemiBold"�6U   $��Center=�6k   $��Center=�5?      ���	#ff2196f3$��로그아웃��6Q   $
	 Hand��5@   4   
   OpenDialogCommand#��Z
 5A      ��5?   `   Z� ��5@      Z� 5A      �� 5C      $��SemiBold"�6    $��Center=�66   $��Center=�6Q   ���	#ff2196f36n   �� 5D      $ 
Horizontal=�6!   $��Center=�6:   $��Center=�6W   .+�5E      '  !MaterialDesignThemes.Wpf.PopupBox  -	   5          Name$ userInfo_PopupBox��5E   2      	StaysOpen .    PlacementMode$ BottomAndAlignRightEdges=�6^      ToggleContent 5F      �� 5G   "   -
   5         ��Name$ userNameTxtBox��5G   ,   $��SemiBold"�6D   $��Center=�6Z   $��Center=�6u   ���	#ff2196f36�      PopupContent. 5I      � $	��250��6#   $	��170��6/   $��8,8,8,8q�6<    � 	Resources 5J   "    �� 
TargetType  MaterialDesignCheckBox �� BasedOn �� Property �� Value  MaterialDesignTextBox)  #MaterialDesignThemes.Wpf.HintAssist��%�   '��       7  '�� S     7  �� 5K   &   " ��6,   8   .�5L   *   ��  � ��$ 8,8,10,8q�6C   5M   '   �� 5N   &   " ��6,   8   .�5O   *   ��  �@ 
IsFloating . 6a   �� 5P   *    � ��$ 8,8,8,8q�6C   5Q   '   5R   #    � RowDefinitions 5S   "   �� 5T   &   $��* �64   �� 5U   &   $��* �64   �� 5V   &   $��* �64   �� 5W   &   $��* �64   �� 5X   &   $��* �64   .+�5[   "   �� $��0��6-    $  MaterialDesignHeadline6TextBlock#��[ 6:   $��Center=�6t   $��Center=�6�   $��SemiBold"�5\   %   $��15(�6;   ���	#ff2196f3�� 5]   &   -   5         ��Name$ 	user_name��5]   *    �� 5^   &   $�반갑습니다.��6*   5_   #   �� 5a   "   $��1��6-   #��[ 6:   $
��Left=�6t   $��Center=�6�   $��SemiBold"�5b   %   $��12(�6;   ���	#ff2196f3$��10,0,0,0q�6`   �� 5c   &   $�사번 :��6*    �� 5d   &   -   5        $ user_no��5d   *   
���   �6;   5e   #   �� 5g   "   $��2��6-   #��[ 6:   $
��Left=�6t   $��Center=�6�   $��SemiBold"�5h   %   $��12(�6;   ���	#ff2196f3$��10,0,0,0q�6`   �� 5i   &   $�권한 :��6*    �� 5j   &   -   5        $ 	user_auth��5j   *   
���   �6=   5k   #   �� 5m   "   $��3��6-   #��[ 6:   $
��Left=�6t   $��Center=�6�   $��SemiBold"�5n   %   $��12(�6;   ���	#ff2196f3$��10,0,0,0q�6`   �� 5o   &   $�부서 :��6*    �� 5p   &   -   5        $ 	user_dept��5p   *   
���   �6=   5q   #   � 5s   "   $��4��6'    5t   &   �� 5u   *   $
��0.5* �6;   �� 5v   *   $��* �6;   .+�5x   &   �� -   5        $# user_info_popupBox_cancel_Btn��5x   =   $��0��6-   $��닫기��6d      ClosePopupCommand#��Z 6q   $��10(�5y   1   $
��Bold"�6?   $��Center=�6Q   $��Center=�5z   1   $��0q�6M   $��28��6a   $��55��6m   $
	 Hand��6x   
���������5{   *   �� 5|   .   " ��64   # [B��6I   .�5}   2   ��  � ��
 �����6P   5~   /   5�   '   �� 5�   &   -   5        $ pw_Change_Btn��5�   =   $��1��6-   $��비밀번호 변경��6T   $��10(�5�   1   $
��Bold"�6?   $��Center=�6Q   $��Center=�5�   1   $��0q�6M   $��28��6a   $	��155��6m   $
	 Hand��6y   
���������5�   *   �� 5�   .   " ��64   # [B��6I   .�5�   2   ��  � �� �#4472C46P   5�   /   5�   '   5�   #   5�      5�      5�      �� 5�      -   5        $ userNamePlusTxtBlock��5�       $	��님��6>   $
��Bold"�6G   $��Center=�6Y   $��Center=�6t   ���	#ff2196f36�   5�      5�      � 5�      -   5         �Name$ 	TabBorder��5�      $
��1300��6&   $��1��63   
�������$��Bottom=�6U   $
��Left=�6p   6�   � 5�      .+�5�       � -   5          �Name$ TabFrame��5�        � NavigationUIVisibility$ Hidden=�6*     � Source$' !/View/TabItemPage/StatusPage.xaml?�6J   $	��550��6u   $��Bottom=�6�   5�      5�      5�      5�      