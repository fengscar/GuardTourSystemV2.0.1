   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:GuardTourSystem.UtilsGuardTourSystem.Utils  5      ;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5      .    (GuardTourSystem.View.SharePatrolDataView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4Util#clr-namespace:GuardTourSystem.Utils     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      R NMahApps.Metro.IconPacks, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      l hMicrosoft.Practices.Prism.Composition, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35m iMicrosoft.Practices.Prism.Mvvm.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n	 jMicrosoft.Practices.Prism.Interactivity, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35-prismhttp://www.codeplex.com/prism   	 5
      l
 hTelerik.Windows.Controls.Chart, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Documents.Core, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
 
          5      ?view"clr-namespace:GuardTourSystem.View
 
          5      $	��500��5        +� FocusedElement  5      ��  �� ElementName$ 
ConfirmBtn��.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   9� $��3q�5      $W��Q用户名与密码   (如果共享文件夹没有设置密码,则不需要填写)��5       9� 
IsExpanded .  .��5      �� .+�5      �� $��10q�6    �� Orientation$ 
Horizontal=�6)   .+�5      �� $	��100��6   $��	用户名��6(   �� 5      $	��300��6   ��6*   �� *
Username+5       �� 5!      $��10q�6   $ 
Horizontal=�6)   .+�5"      �� $	��100��6   $��密码��6(   �� 5#      $	��300��6   ��6*   �� *
Password+5$      5%      5&      �� 5'   
   $��1��5(      $��3q�5)      $��16(�5*      $*��$请选择巡检数据的共享位置��5+      .��5,      � .+�5-      �� $	��430��5.      $��14(�5/      ��50      �� *ShareFilePath+ �� UpdateSourceTrigger$ PropertyChanged=��� 51      $��30��52      $��20��53      $��Center=�54      
������ $��0q�56      $��Right=�58       � ToolTip$ 选择共享位置��59      ��57      �� *CSelectShareFilePath+.��5:      .  (MahApps.Metro.IconPacks.PackIconMaterial  $��12��5;      
��� ����  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null]]   Kind$ Folder=�5=      5>      5?      5@      �� 5A   
   $��2��5B      $��10q�5C      $7��1当数据更新时,自动上传到共享文件夹��5D      $��16(�5E       d� 	IsChecked 5F      �� *	AutoShare+�� 5G   
   $��3��5H         
AlarmLabel#��[  5J      ��5I      �� *	ErrorInfo+5J      A� 5K   
   $��4��5L      $��1��5M      $
��0,10q�5N      	 A� Columns$	 2��5O      .+�5P      �� -   5        
 ��Name$
 
ConfirmBtn��5Q      $��10q�5R        
ConfirmBtn#��[ 5S      �� 5T      $��10q�6     CloseBtn#��[ 6!   5U      5V      5W      