   M S B A M L   `   `   `  ����L  HFuelCellStoreTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:FuelCellStoreToolFuelCellStoreTool  5   	   "    FuelCellStoreTool.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:FuelCellStoreTool     5   	     � Title$  Fuel Cell Store Tool��5   	   $	��450��6&   $	��800��63   .��5	      � .+�5
   
   �� $��
Add filter��6   $
��Left=�6%   $��8,10,0,0q�6@   $	��Top=�6R   $��14(�6j   $
��Bold"�6x   �� 5   
   -   5         ��Name$ NameBox��5      $
��Left=�6#   $��	10,46,0,0q�6>    �� TextWrapping$
 Wrap=�6Q   $	��Top=�6e   $	��360��6}   �� 5   
    J� IsHitTestVisible$ False��6   $��Enter Property Name��6-   $	��Top=�6H   $
��Left=�6`   $��	10,46,0,0q�6{    �� Padding$ 2,1,0,0q�6�   
���������5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � y� �� Value$ 	Collapsed=�63    �� Triggers 5      �� 5      	 �� Binding	 6&   �� *Text+
 �� ElementName$
 NameBox�� �� Value  6T   .a�5      ��  � y�$ Visible=�6;   5      5      5      �� 5   
   -   5        $ ValueBox��5      $
��Left=�6$   $��
441,46,0,0q�6?   $
 Wrap=�6S   $	��Top=�6g   $��80��6   �� 5   
   $ False��6   $��Enter Value��6-   $	��Top=�6@   $
��Left=�6X   $��
441,46,0,0q�6s   $ 2,1,0,0q�6�   
���������5      �� 5      " ��6   .�5      ��  � y�$ 	Collapsed=�63    5      �� 5      	 6&   �� *Text+$
 ValueBox��  6U   .a�5      ��  � y�$ Visible=�6;   5       5"      5$      �� 5%   
   -   5         ��Name$ 	AddButton��5%      $	��Add��6$   $
��Left=�62   $��
551,45,0,0q�6M   $	��Top=�6a   6y   �� 5&   
   -   5         ��Name$ OperatorBox��5&      $
��Left=�6(   $��
399,46,0,0q�6C   $	��Top=�6W   $��37��6o   $��18��6z   �� 5'   
   -   5         ��Name$ ProductList��5'      $��
8,85,10,10q�6S    �� View 5(      �� 5)      .P�5*      �� 5+      5-      5/      50      