   M S B A M L   `   `   `  ����H  DFD_MainWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:FD_MainWindowFD_MainWindow  5          FD_MainWindow.UCCard   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:FD_MainWindow     5      -   5          >�Name$  UCcard��5       � 	Resources 5
       �� Source�� 5   
   $( "/GameplayResources/CardStyles.xaml?�6   %    .��5   
   (� -   5         (�Name$ cardVB��5      $	��100��6"   $	��160��6.    (� Stretch$ UniformToFill=�6;   .��5   
   � -   5         �Name$ CardGrid��5      $	��100��6!   $	��160��6-   ��5      �� 5      -   5        CardBackgound�� j� Stretch$ UniformToFill=�5   4    � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��90 �6    �� 5      $��30 �6     � ColumnDefinitions 5      �� 5      $��29 �6#   �� 5      $��42 �6#   �� 5      $��29 �6#   .+�5      �� -   5        	 ��Name$	 cardBorderAV��5      $��26��6+   $��32��66   $��Right=�6B   $��Center=�6^   ��5      �� 5       -   5        CardClassFrame��$ UniformToFill=�5    9   ._�5"      �� -   5        
 ��Name$
 cardAV��5"         
CardValueT#��[  6,   
����� �$��30��6b   $��26��6n   $��Center=�6y   $��Center=�6�   5#      �� 5$      $��1��6   �� 5%      -   5        $
 cardHP��5%      #��[  6(   
�������$��26��6e   $��30��6p   $��2��6|   $
��Left=�6�   $��Center=�6�   �� 5'      -	   5         ��Name$ 	CardImage��5'      $��90��6'   $��90��63   $��Uniform=�6>   $��1��6P   $��3��6]   $��Center=�6q   �� 5(      -
   5        $
 cardName��5(        	CardNameT#��[ 5)      $��2��68   $��3��6E   ��5(   *   ��  �� ElementName$ UCcard�� �� Path$ BoardCard.name�5)   E   �� 5*      -   5         ��Name$ cardB��5*      $	��100��6$   $	��160��60   $��3��6=   $��3��6Q   
������   
CardButton#��[ 6�   $��Center=�6�   $��Center=�6�   5+      5,      5-      