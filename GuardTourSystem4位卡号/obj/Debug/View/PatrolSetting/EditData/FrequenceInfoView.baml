   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GuardTourSystem.ConverterGuardTourSystem.Converter  5      ;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5
      ,    &GuardTourSystem.View.FrequenceInfoView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      T PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5      7	Converter'clr-namespace:GuardTourSystem.Converter  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      l hTelerik.Windows.Controls.Chart, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k	 gTelerik.Windows.Controls.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Documents.Core, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
    	 
      5	      ?view"clr-namespace:GuardTourSystem.View
    	 
      5
        +� FocusedElement  5      ��  �� ElementName$ RouteNameTextBox�� � 	Resources 5      4  .GuardTourSystem.Converter.BoolReverseConverter   BoolReverser��%   &	          5   
   6)   5      .��5      � $
��Auto��5   	   $��
20,20,20,0q�5   	   $��Center=�5   	    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5    
   �� $��0��5!      $��5,5,5,0q�5"      $��Center=�5#       �� Orientation$ 
Horizontal=�5$      .+�5%      �� $��部门名称��6     ContentLabel#��[ 6#   �� 5&      $	��150��5'      $��3q�5(       �� DisplayMemberPath$ 	RouteName��5)      ��5*      �� *Routes+ �� SelectedItem 5+      �� *Route+ �� Mode$ TwoWay=�5,      �� 5.   
   $��1��5/      $��5,5,5,0q�50      $��Center=�51      $ 
Horizontal=�52      .+�53      �� $��班次名称��6   #��[ 6#   �� 54      -   5         ��Name$ RouteNameTextBox��55      	 �� 	MaxLength$	 5��56      
 �� MaxLines$
 1��57        ContentTextBox#��[ 58      ��59      �� *Frequence.Name+5:      �� 5<   
   $��2��5=      $��5,5,5,0q�5>      $��Center=�5?      $ 
Horizontal=�5@      .+�5A      �� $��上班时间��6   #��[ 6#   ,  &Telerik.Windows.Controls.RadTimePicker  5B      $	��150��5C      $��3q�5D      0  *Telerik.Windows.Controls.RadDateTimePicker   SelectedTime 5E      �� *Frequence.StartTime+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5F      �� 5I   
   $��3��5J      $��5,5,5,0q�5K      $��Center=�5L      $ 
Horizontal=�5M      .+�5N      �� $��下班时间��6   #��[ 6#   � 5O      $��3q�6    � ColumnDefinitions 5P      �� 5Q      $
��Auto �6'   �� 5R      $
��Auto �6'   .+�5T      �� $��0��5U      $��57��5V      $��0,0,3,0q�5W      $��次日��5X       d� 	IsChecked 5Y      �� *Frequence.NextDay+  5Z      $��2��5[      $��90��5\       5]      �� *Frequence.EndTime+$ TwoWay=�5^      5_      �� 5`   
   $��4��5a      $��5,5,5,0q�5b      $��Center=�5c      $ 
Horizontal=�5d      .+�5e      �� $��工作时间��6   #��[ 6#   �� 5f      -   5        $	��150��5g      $	 3��5h      #��[ 5j       � ToolTip$ 单位-分钟��5l      ��5k      �� *Frequence.PatrolTime+5l      5m      �� 5o   
   $��5��5p      $��5,5,5,0q�5q      $��Center=�5r      $ 
Horizontal=�5s      .+�5t      �� $��间休时间��6   #��[ 6#   �� 5u      -   5        $	��150��5v      $	 3��5w      #��[ 5y      $ 单位-分钟��5{      ��5z      �� *Frequence.RestTime+5{      5|      �� 5   
   $��6��5�      $��30,5,0,0q�5�      $��Center=�5�      $ 
Horizontal=�5�      .+�5�      �� $��排班方式��6   #��[ 6#   A� 5�      $	��180��5�      $��3q�5�       A� Columns$ 2��5�      .+�5�      	� $��按周排班��6    6-   �� *Frequence.IsRegular+$ TwoWay=�	� 5�      $��无规律排班��6    6.   �� *Frequence.IsRegular+ �� 	Converter# [  5�      5�      �� 5�   
   $��7��5�      $��5,5,5,0q�5�      $��Center=�5�      $ 
Horizontal=�5�      .+�5�      �� $��打卡次数��6   #��[ 6#   �� 5�      {�.  #��[ 5�      ��5�      �� *Frequence.PatrolCount+5�      �� 5�   
   $��8��5�      $��2��5�        
AlarmLabel#��[ 5�      ��5�      �� *	ErrorInfo+5�      A� 5�   
   $��9��5�      $��0,10,0,0q�5�      $ 2��5�      .+�5�      �� $��10q�5�      $��确定��5�        
ConfirmBtn#��[ 5�      �� 5�      $��10q�5�      $��取消��5�        CloseBtn#��[ 5�      5�      5�      5�      