   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5      (    "GuardTourSystem.View.EventInfoView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      3view"clr-namespace:GuardTourSystem.View     5        +� FocusedElement  5      ��  �� ElementName$ EventNameTextBox��.��5
      � $
��Auto��5   	   $��
20,20,20,0q�5   	   $��Center=�5   	    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��5      $
��5,10q�5      $��Center=�5       �� Orientation$ 
Horizontal=�5      .+�5      �� $
��Auto��5      $��事件名称��5         ContentLabel#��[  5      �� 5      -   5         ��Name$ EventNameTextBox��5      $	��200��5       �� 	MaxLength$ 10��5         ContentTextBox#��[ 5!      ��5"      �� *
Event.Name+ �� UpdateSourceTrigger$ PropertyChanged=�5#      �� 5$   
   $��1��5%      $	��5,0q�5&      $��Center=�5'      $ 
Horizontal=�5(      .+�5)      �� $
��Auto��5*      $��事件钮号��5+      #��[  5,      �� 5.      $	��200��5/      $ 4��50      #��[ 51      ��52      �� *
Event.Card+$ PropertyChanged=�53      �� 54   
   $��2��55        
AlarmLabel#��[ 57      ��56      �� *	ErrorInfo+57      A� 58   
   $��3��59      $��5q�5:       A� Columns$ 2��5;      .+�5<      �� $��10q�5=      $��确定��5>        
ConfirmBtn#��[ 5?      �� 5@      $��10q�5A      $��取消��5B        CloseBtn#��[ 5C      5D      5F      5G      