   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EasyProject.ViewEasyProject.View  5      !    EasyProject.View.InsertPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      *localclr-namespace:EasyProject.View  5        N� Title$  
InsertPage��5       � Cursor$
 Hand��6    � DataContext 5
      �� *PVM+   Locator �� Source# [  .��5      )  #MaterialDesignThemes.Wpf.DialogHost  -   5          Name$ InsertDialogHost��5          IsOpen 6:   �� *IsInsertDialogHostOpen+   DialogContent 5   
   �� 5      $��20q�6   .+�5      �  � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $
��Auto �6+   6:   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Error=�6.   ���#4472C4$��48��5   2   $��48��5   2   $��20, 25, 20, 20q�5   2   $��0��5   2   �� 5      $��20q�6!   $��1��6-   $	��180��6>   .+�5      �� $��추가��6$   $��15(�6.   $��	UltraBold"�6<   6U   �� 5      $)��#파일을 추가하시겠습니까?��6$   $��13(�69   $��SemiBold"�6G   $��0,20,0,0q�5   (   6<   5      5      �� 5      	 �� Orientation$	 
Horizontal=�6   $��Center=�66   $��Center=�6S   .+�5       �� $��취소��6   ���#EAEAEA
���   �
   CloseDialogCommand#��Z
 5!      6a   �� 5"      $��확인��6   ���#4472C4
�������$��30,0,0,0q�6R   $
 Hand��5#   =   ��6   �� *ListCommand+6N   5%      5&      .��5(   
   � $	��500��6    � RowDefinitions 5)   
   �� 5*      $��1* �6   �� 5+      $��9* �6   .+�5.   
   � $��0��6    5/      �� 50      $��* �6#   �� 51      $��* �6#   �� 52      $��4* �6#   .+�55      �� -   5         ��Name$ formBtn��55      $��
폼 입력��6*   $��0��69   ���#4472C4
�������$��13(�6q   $
��Bold"�6   $��Center=�6�   $��Center=�56      $��10,0,0,0q�63   $��0q�6E   $��30��6Y   $	��150��6e   $
 Hand��6q   �� 57      -   5        $ excelBtn��57      $��파일 업로드��6+   $��1��6<   
�������
���   �$��13(�6v   $
��Bold"�6�   $��Center=�6�   $��Center=�58      $��10,0,0,0q�63   $��0q�6E   $��30��6Y   $	��150��6e   $
 Hand��6q   �� 59      $��내가 추가한 목록��6   $��2��6*   $��20,0,0,0q�6:   $
��Left=�6L   $��Center=�6g   $��SemiBold"�6�   $��13(�6�   6�   � 5;      $��2��6    5<      �� 5=      $��2* �6+   �� 5>      $��2* �6+   �� 5?      $��4* �6+   .+�5B      � $��2��6   $
��10 0q�6'    5C      �� 5D      $��* �6/   �� 5E      $
��0.3* �6/   �� 5F      $��* �6/   .+�5I      (  "System.Windows.Controls.DatePicker  -   5          Name$ expirationDate_DatePicker��5I   %   $
��Left=�6H   $��Center=�5J   *   |�.  $	��100��6W      SelectedDate 5K   )   �� *	StartDate+   DisplayDateEnd 6L   �� *	EndDate+ � 	Resources 5M        CustomCalendarDayButton �� 
TargetType:  4System.Windows.Controls.Primitives.CalendarDayButton # MaterialDesignCalendarDayButton �� BasedOn-  'MaterialDesignThemes.Wpf.CalendarAssist �� Property �� Value  CustomCalendarButton7  1System.Windows.Controls.Primitives.CalendarButton   MaterialDesignCalendarButton  CustomCalendar&   System.Windows.Controls.Calendar , (MaterialDesignDatePickerCalendarPortrait ( $MaterialDesignFloatingHintDatePicker��%/  &	       7  &	 �     7  &	 F    7  7  7  '  �    7  7  �� 5N   "   "  6H   8   .�5O   &   ��  �@ SelectionColor �	#ff2196f36e   �� 5P   &     �@ SelectionForegroundColor
 �����6o   5Q   #   �� 5R   "   "  6E   8   .�5S   &   ��  �@ SelectionColor �	#ff2196f36e   �� 5T   &     �@ SelectionForegroundColor
 �����6o   5U   #   �� 5V   "   "  6?   8   .�5W   &   ��  � ��
 �����6C   �� 5X   &    � ��
 �����6C   �� 5Y   &    �@ HeaderBackground
 �����6g   �� 5Z   &    �@ HeaderForeground
 �����6g   �� 5[   &    �@ CalendarDayButtonStyle8  6O   �� 5\   &    �@ CalendarButtonStyle8  6L   5]   #   �� 5^   "   "  6(   8   .�5_   &   ��  �@ CalendarStyle8  6F   5`   #   5a      5b      �� 5d      $��Center=�6$   $��1��6A   $��Center=�6Q   $��~��6l   $��	UltraBold"�6u   $��20(�6�     5e      -   5        $  expirationDate_DatePicker2��5e   %   $��2��6I   $��Center=�6Y   |�.  $	��100��6�    5f   )   �� *	EndDate+   DisplayDateStart 6J   �� *	StartDate+ 5g      ��%/  &	       7  &	 �     7  &	 F    7  7  7  '  �    7  7  �� 5h   "   "  6H   8   .�5i   &   ��  �@ SelectionColor �	#ff2196f36e   �� 5j   &     �@ SelectionForegroundColor
 �����6o   5k   #   �� 5l   "   "  6E   8   .�5m   &   ��  �@ SelectionColor �	#ff2196f36e   �� 5n   &     �@ SelectionForegroundColor
 �����6o   5o   #   �� 5p   "   "  6?   8   .�5q   &   ��  � ��
 �����6C   �� 5r   &    � ��
 �����6C   �� 5s   &    �@ HeaderBackground
 �����6g   �� 5t   &    �@ HeaderForeground
 �����6g   �� 5u   &    �@ CalendarDayButtonStyle8  6O   �� 5v   &    �@ CalendarButtonStyle8  6L   5w   #   �� 5x   "   "  6(   8   .�5y   &   ��  �@ CalendarStyle8  6F   5z   #   5{      5|      5}      5      5�      � 5�   
   $��1��6    5�      �� 5�      $��2* �6#   �� 5�      $��4* �6#   .+�5�       � -   5          �Name$ InsertPageFrame��5�      $��0��6-     � Source$ InsertPage_Form.xaml?�6=    � 5�      -   5        $ InsertListPageFrame��5�      $��1��61   $ InsertListPage.xaml?�6A   5�      � 5�   
   $��1��6   .+�5�      '  !MaterialDesignThemes.Wpf.Snackbar  -   5          Name$ Snackbar��5�   &   $��Bottom=�68   $��Stretch=�6S   ��5�      �� 5�      "  6    �� Triggers 5�      �� 5�       �� Binding 6*   �� *IsDuplicatedProduct+ �� Value True6R   .a�5�   "   ��  �@ MessageQueue 6A   �� *MessageQueue+6b   �� 5�   "    � �� �#Ea4a2d6Q   5�      �� 5�       6*   �� *IsDuplicatedProduct+	 False6R   .a�5�   "   ��  �@ MessageQueue 6A   �� *MessageQueue+6b   �� 5�   "    � ��
 � � �6O   5�      5�      5�      5�      5�      5�      5�      