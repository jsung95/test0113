   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EasyProject.View.TabItemPageEasyProject.View.TabItemPage  5      6    0EasyProject.View.TabItemPage.InsertPage_Category   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      6local*clr-namespace:EasyProject.View.TabItemPage  5        N� Title$  InsertPage_Category��5       � DataContext 5
      �� *PSVM+   Locator �� Source# [   � 	Resources 5      &   System.Windows.Controls.DataGrid �� 
TargetType �� Property �� Value��%J  '        �� 5   
   "  6   .�5      ��  � ��
 �����6+   �� 5       �@ VerticalGridLinesBrush �#4472C467   �� 5        �@ HorizontalGridLinesBrush �#4472C469   �� 5       �@ 	RowHeight$ 32Q�6*   �� 5       �@ RowHeaderWidth$ 0Q�6/   �� 5       � ��
 �   �6+   �� 5       � ��$ Center=�62   �� 5       � ��$ Center=�69   �� 5      # �@ VerticalScrollBarVisibility$
 Auto=�6<   �� 5       �@�Cursor$
 Hand��6'   �� 5       �@ 
IsReadOnly .  6;   5      5       .��5"      � $	��450��6   .+�5#   
   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��10,10,10,10q�6.      UniformCornerRadius$ 20Q�6D   
�������.��5$      �  � RowDefinitions 5%      �� 5&      $��2* �6$   .+�5)      � $��20,20,20,20q�6   ���	#FFE5E5E5	 J� 	AllowDrop	 . .+�5*      �� 
 �� Orientation$
 Vertical=�6!   .+�5+      �� -   5         ��Name$ CategoryAddBtn��5+   !   $��Right=�69   $
��10 5q�6U    & "MaterialDesignIconForegroundButton#��[ 6c   $��20��6�   $��20��6�   .��5,      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 plus=�66   $��20��6B   $��20��6N   $��Center=�6Y   $��Center=�6v   
���   �6�   5-        5/      -   5          Name$ dataGrid��5/   #   $��1��65   $
��Left=�6B   $��10 10q�6]   $	��330��6l   	 .  $	��Top=�50   /      AutoGenerateColumns .   � Cursor$
 Hand��51   p   ��6   �� *
Categories+ �� SelectedItem 6@   �� *SelectedModifyCategory+   Columns 53      4  .System.Windows.Controls.DataGridTemplateColumn  54   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 품목/종류명��69      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6I      CanUserSort .     
IsReadOnly .     CellTemplate 55   &   �� 56   *   .R�57   .   �� $��Center=�6W   $
��10 0q�6r   ��68   �� *Category_name+6�   58   +      CellEditingTemplate 5:   &   �� 5;   *   .R�5<   .   �� ��66   �� *+)PSVM.SelectedModifyCategory.Category_name+# [  6�   5=   +      	CellStyle 5@   &   �� 5A   *   *  $System.Windows.Controls.DataGridCell"  60    �� Triggers 5C   .   ^� 5D   2    ^� 
Conditions 5E   6   �� 5F   :    �� Binding 6D   �� *
IsSelected+ �� RelativeSource �� *Self+ �� Value true6�   .0�5H   6   ��  �@ 	IsEditing . 6R   5I   3   5K   +   5M   #     5O   "   -   5        buttonColumn��$ * 5O   O    .  5P   &   �� 5Q   *   .R�5R   .   �� $ CategoryNameModifyBtn��65   #��[ 6T   $��20��6�   $��20��6�   ��6�   �� *PSVM.CategoryModifyCommand+# [  .��5S   2     $ pencil=�6J   $��20��6X   $��20��6d   $��Center=�6o   $��Center=�6�   
�������6�   5T   /   5U   +   5W   #   5Y      5Z      5[      5\      5]      5^      5_      