   M S B A M L   `   `   `  ����@  <ezIMP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:EasyProject.View.TabItemPageEasyProject.View.TabItemPage  5      =#clr-namespace:EasyProject.ViewModelEasyProject.ViewModel  5      )clr-namespace:EasyProjectEasyProject  5	      1    +EasyProject.View.TabItemPage.InsertListPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:EasyProject.View.TabItemPage     5      _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ,vm#clr-namespace:EasyProject.ViewModel  5      !lclr-namespace:EasyProject  5	        N� Title$  InsertListPage��5       � DataContext 5      �� *PVM+   Locator �� Source# [   � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value&   System.Windows.Controls.DataGrid   BooleanToVisibilityConverter��%  '        '      &	 �    �� 5   
   "  6   .�5      ��  � ��
 �����6+   �� 5       � ��$ 12(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�62   �� 5       � ��$ 20��6'   5      �� 5   
   "  6   .�5      ��  � ��
 �����6+   �� 5       �@ VerticalGridLinesBrush �#4472C467   �� 5        �@ HorizontalGridLinesBrush �#4472C469   �� 5       �@ 	RowHeight$ 32Q�6*   �� 5       �@ RowHeaderWidth$ 0Q�6/   �� 5        � ��
 �   �6+   �� 5!       � ��$ Center=�62   �� 5"       � ��$ Center=�69   �� 5#      # �@ VerticalScrollBarVisibility$
 Auto=�6<   5$      �� 5%   
   6'   5&      .��5(      � $	��450��6   .+�5)   
   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��10,10,10,10q�6.      UniformCornerRadius$ 20Q�6D   
�������.��5*        -   5          Name$ dataGrid��5*      $
��Left=�5+      $��10 10 10 10q�68   $	��Top=�5,      	   AutoGenerateColumns	 .  
   CanUserSortColumns
 .    
IsReadOnly .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5-   I   ��5.      �� *
Add_list+ �� Mode$ TwoWay=�   Columns 54      0  *System.Windows.Controls.DataGridTextColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header$ 입력일자��6J      Width$
 1.3* 6X      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� *Prod_in_date+6e     56      $ 제품코드��6G   $ * 6U    .  6)   �� *	Prod_code+6_     57      $ 	제품명��6G   $ * 6T    .  6)   �� *	Prod_name+6^     58      $ 품목/종류��6L   $
 1.3* 6[    .  6)   �� *Category_name+6i     59      $ 제품가격��6d   $
 1.5* 6r    .  6)   �� *
Prod_price+ �� StringFormat$ {0:#,0} ￦��   ElementStyle 5:      �� 5;      " ��6$   .�5<   "   ��  � ��$ Right=�6H   5=      5?        5@      $ 유통기한��6e   $
 2.5* 6s    .  6)   �� *Prod_expire+$ 
yyyy-MM-dd��6�     5A      $ 수량��6h   $ * 6t    .  6)   �� *Prod_in_count+$ {0:#,0} 개�� 5B      �� 5C      " ��6$   .�5D   "   ��  � ��$ Right=�6H   5E      5G        5H      $ 관리자명��6I   $ * 6W    .  6)   �� *
Nurse_name+6b   5K      5L      �� 5M   
   $1��+조회 가능한 데이터가 없습니다.��6    �� TextAlignment$ Center=�6-   $��
0,48,0,-48q�6�   y�6E   �� *Items.IsEmpty+ �� 	Converter# [  �� ElementName$ dataGrid��6�   5N      5O      