   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5      )    #GuardTourSystem.View.ReanalysisView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      l hTelerik.Windows.Controls.Chart, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l	 hTelerik.Windows.Documents.Core, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Controls.Input, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
     	 
     5      ?view"clr-namespace:GuardTourSystem.View
     	 
     5      .��5      � $��10q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��5      $��2��5      $��0,5,0,10q�5      $-��'选择需要进行重新分析的时间��5         
TitleLabel#��[  5      �� 5.   
   $��1��5/      $
��5,10q�50      $��Center=�51       �� Orientation$ 
Horizontal=�52      .+�57      0 
 *Telerik.Windows.Controls.RadDateTimePicker  -   5          Name$ StartDatePicker��58      $	��100��59        RadDatePicker#��[ 5;         SelectedDate 5:      �� *DateQueryInfo.Begin+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� StringFormat$ 
yyyy-mm-dd��5;      �� 5<      $	��至��6   #��[  6      5=      $	��100��5>      #��[ 5@       5?      �� *DateQueryInfo.End+$ TwoWay=�$ PropertyChanged=�$ 
yyyy-MM-dd��5@      5B      �� 5N   
   $��2��5O      $��2��5P        
AlarmLabel#��[ 5R      ��5Q      �� *	ErrorInfo+5R      A� 5S   
   $��3��5T      $��1��5U       A� Columns$ 2��5V      .+�5W      �� $��10q�6     
ConfirmBtn#��[ 6!   �� 5X      $��10q�6     CloseBtn#��[ 6!   5Y      5Z      5[      