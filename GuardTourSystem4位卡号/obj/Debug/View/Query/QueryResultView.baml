   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GuardTourSystem.ConverterGuardTourSystem.Converter  5      C&clr-namespace:GuardTourSystem.SelectorGuardTourSystem.Selector  5      ;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5
      *    $GuardTourSystem.View.QueryResultView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      =	converter'clr-namespace:GuardTourSystem.Converter     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ;selector&clr-namespace:GuardTourSystem.Selector     5      l hTelerik.Windows.Controls.Chart, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l	 hTelerik.Windows.Documents.Core, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Controls.Input, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
     	 
     5	      ?view"clr-namespace:GuardTourSystem.View
     	 
     5
        � 	Resources  5      8  2GuardTourSystem.Selector.PatrolResultStyleSelector   PatrolResultStyleSelector   MissDutyStyle �� 
TargetType3  -Telerik.Windows.Controls.GridView.GridViewRow �� Property  FlatRedSolidBrush �� Value   OnDutyStyle��%�   &	        7    5   
    5      �� 5      "  6   .�5      ��  � ��8   63   5       5      �� 5      "  6   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5!   
   � $��0��6    � ColumnDefinitions 5"      �� 5#      $
��Auto �6#   �� 5$      $
��Auto �6#   �� 5%      $
��Auto �6#   �� 5&      $
��Auto �6#   �� 5'      $
��Auto �6#   �� 5(      $
��Auto �6#   �� 5*      $
��Auto �6#   �� 5+      $��* �6#   �� 5,      $
��Auto �6#   �� 5-      $
��Auto �6#   �� 5.      $
��Auto �6#   .+�51      �� $��0��52        	QueryDate#���  53        
TitleLabel#��[ 54      0 
 *Telerik.Windows.Controls.RadDateTimePicker  55      $��1��56      $��10,3,0,3q�57         DisplayFormat$
 Long=�58        RadDatePicker#��[ 5:      	   SelectedDate	 59      �� *DateQueryInfo.Begin+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� StringFormat$ 
yyyy/MM/dd��5:      �� 5;      $��2��5<        QueryDateTo#���  5=      #��[ 5>        5?      $��3��5@      $��0,3,10,3q�5A      $
 Long=�5B      #��[ 5D      	 5C      �� *DateQueryInfo.End+$
 TwoWay=�$ PropertyChanged=�$ 
yyyy/MM/dd��5D      �� 5E      $��4��5F      $��5,0,5,0q�5G        
AlarmLabel#��[ 5I      ��5H      �� *Error+5I      �� 5J      $��5��5K      $��80��5L       	 Query#���  5N        BlueBtn#��[ 5O      ��5M      �� *CQuery+5O      �� 5Q      $��6��5R      $��80��5S       		 Reset#��� 	 5U      #��[ 5V      ��5T      �� *CReset+5V      �� 5X      $��8��5Y      $��3,0,30,0q�5Z      $��5,0,5,0q�5[       
 QueryResultShowOnDuty#��� 
 5\      $��16(�5]       d� 	IsChecked 5^      �� *
ShowInDuty+�� 5_      $��9��5`      $��3q�5a      $��5,0,5,0q�5b      #��[ 5c        QueryResultShowMissDuty#���  5d      $��16(�5e        WhiteCheckBoxStyle#���  5g       5f      �� *ShowOutDuty+5g      5h      *  $Telerik.Windows.Controls.RadGridView  5i   
   -   5          Name$ GridView��5j      $��1��5k      ;  5Telerik.Windows.Controls.GridView.GridViewDataControl   CanUserFreezeColumns .  8  2Telerik.Windows.Controls.GridView.BaseItemsControl   GroupRenderMode$
 Flat=�5m         MergedCellsDirection$ Vertical=�5o         RowStyleSelector# [  5p        ContentGridView#��[ 5q      *  $Telerik.Windows.Controls.DataControl   ItemsSource 5n      �� *	Results+   Columns 5s      1  +Telerik.Windows.Controls.GridViewDataColumn  5t      -	  'Telerik.Windows.Controls.GridViewColumn 	  Width?
  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 1*
 5u       	 Route6  0Telerik.Windows.Controls.GridViewBoundColumnBase   Header# �  5w         DataMemberBinding 5v      �� *	RouteName+5w        5x      $ 1*
 5y        	Frequence# �  5{       5z      �� *FrequenceName+5{        5|      $
 1.5*
 5}      $ 计划时间��5       5~      �� *
PlanTime+5        5�      $ 1*
 5�      $ 	计数员��5�         IsCellMergingEnabled .   5�      �� *
WorkerName+5�        5�      $ 1*
 5�      $ 	计数点��5�       .   5�      �� *	PlaceName+5�        5�      $
 1.5*
 5�      $ 实巡时间��5�       .   	  IsFilterable .   5�      �� *
PatrolTime+$  yyyy年MM月dd日 HH:mm:ss��5�        5�      $ 0.75*
 5�      $ 考核结果��5�       .   5�      �� *PatrolResult+5�        5�      $ 1*
 5�      $ 计数事件��5�       .   5�      �� *	EventName+5�      5�      �� 5�      $��2��5�      $��RightToLeft=�5�       �� Orientation$ 
Horizontal=�5�      .+�5�      ��   CloseBtn#��[ 6   �� 5�      -   5          ExportExcelBtn#��[ 5�   1   �� 5�      -   5          PrintBtn#��[ 5�   0   5�      5�      5�      