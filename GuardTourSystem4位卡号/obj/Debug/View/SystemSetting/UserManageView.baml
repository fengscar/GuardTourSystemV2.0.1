   M S B A M L   `   `   `  ����A  =Patrol, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:GuardTourSystem.UtilsGuardTourSystem.Utils  5      ;"clr-namespace:GuardTourSystem.ViewGuardTourSystem.View  5	      )    #GuardTourSystem.View.UserManageView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4Util#clr-namespace:GuardTourSystem.Utils     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      l hTelerik.Windows.Controls.Chart, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l	 hTelerik.Windows.Documents.Core, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Controls.Input, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7g cTelerik.Windows.Documents, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7| xTelerik.Windows.Documents.FormatProviders.Html, Version=2014.3.1202.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Qtelerik1http://schemas.telerik.com/2008/xaml/presentation
     	 
     5      ?view"clr-namespace:GuardTourSystem.View
     	 
     5	        +� FocusedElement  5      ��  �� ElementName$ NewPasswordBox��.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $
��Left=�5      $%��请输入新的操作员密码:��5         
TitleLabel#��[  5      �� 5   
   $��1��5      $
��5,10q�5      $��Center=�5       �� Orientation$ 
Horizontal=�5      .+�5      �� $��80��5       $��0,0,10,0q�5!      $��	新密码��5"        ContentLabel#��[ 5#      G� 5$      -   5         G�Name$ NewPasswordBox��5%      $	��150��5&      $��16(�5(       G� 	MaxLength$ 10��5)      *   $GuardTourSystem.Utils.PasswordHelper   Password 5'      �� *NewPassword1+ �� Mode$ TwoWay=�5)      5*      �� 5+   
   $��2��5,      $
��5,10q�5-      $��Center=�5.      $ 
Horizontal=�5/      .+�50      �� $��80��51      $��0,0,10,0q�52      $��确认新密码��53      #��[ 54      G� 55      $	��150��56      $��16(�58      $ 10��59       57      �� *NewPassword2+$ TwoWay=�59      5:      �� 5;   
   $��3��5<        
AlarmLabel#��[ 5>      ��5=      �� *	ErrorInfo+5>      A� 5?   
   $��4��5@      $
��0,10q�5A       A� Columns$ 2��5B      .+�5C      �� $��10q�6     
ConfirmBtn#��[ 6!   �� 5D      $��10q�6     CloseBtn#��[ 6!   5E      5F      5G      