   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   APP_NAME 
 CANCEL  LOGIN_WINDOW  LOGIN_BUTTON  LOGIN_DEFAULT_PASSWORD  SETTING_WINDOW  SETTING_SELECT_LANGUAGE  TEST_LED  TEST_BUZZER 	 TEST_RECORD_COUNT 
 RoleManager  RoleOperator 
 Device  DeviceID 	 Route  	RouteName 	 Place  
PlaceOrder  	PlaceName  	PlaceCard 
 Worker  
WorkerName  
WorkerCard 	 Event  	EventName  	EventCard  	Frequence  FrequenceRegular  FrequenceIrregular  
PatrolData  
PatrolTime  Add   Edit 
! Delete 	" Query # Confirm 
$ Cancel 	% Close & Quit ' 	QueryDate ( QueryDateTo ) ExportExcel 	* Print + 	SelectAll , Reading - ReadingFail . ReadingSuccess / Success 0 Fail 1 	MenuQuery 2 MenuQueryReadPatrol 3 MenuQueryRawData 4 MenuQueryReadHit 5 MenuQueryResult 6 MenuQueryChart 7 MenuQueryReanalysis 8 MenuPatrolSetting 9 MenuPatrolSettingRoute : MenuPatrolSettingWorker ; MenuPatrolSettingEvent < MenuPatrolSettingFrequence = MenuPatrolSettingRegular > MenuPatrolSettingIrregular ? MenuDataManage #@ MenuDataManageBackupAndRecovery !A MenuDataManageClearPatrolData "B MenuDataManageImportPatrolData "C MenuDataManageExportPatrolData D 
MenuSystem E MenuSystemInit F MenuSystemUserManage G MenuSystemModifyPassword H MenuSystemLanguage I MenuSystemDeviceTest J MenuSystemLog K MenuHelp L MenuHelpHowToUse M MenuHelpHowToStart N MenuHelpAboutUs O ShortcutReceive P ShortcutRawdata Q ShortcutResult R ShortcutCharts S ShortcutDeviceTest T PatrolDataRead U PatrolDataClear V PatrolDataReading W PatrolDataReadFail X PatrolDataReadSuccess Y PatrolDataEmptyData Z PatrolDataHandling [ PatrolDataHandleFail \ PatrolDataHandleSuccess ] PatrolDataSaveSuccess ^ PatrolDataSaveFail _ PatrolDataDutyUpdating ` PatrolDataDutyUpdateFail a PatrolDataDutyUpdateSuccess b PatrolDataClearing c PatrolDataClearFail d PatrolDataClearSuccess�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      ,s&clr-namespace:System;assembly=mscorlib  5      %a  &	        &	 2     &	 P     &	 r     &	 �     &	 �     &	 �     &	     &	     &		 ;    &	
 _    &	 ~    &	 �    &	 �    &	 �    &	 �    &	     &	 9    &	 \    &	 ~    &	 �    &	 �    &	 �    &	 
    &	 '    &	 I    &	 k    &	 �    &	 �    &	 �    &	     &	 %    &	  @    &	! \    &	" w    &	# �    &	$ �    &	% �    &	& �    &	' 
    &	( +    &	) E    &	* i    &	+ �    &	, �    &	- �    &	. �    &	/     &	0 1    &	1 M    &	2 o    &	3 �    &	4 �    &	5 �    &	6     &	7 3    &	8 U    &	9 {    &	: �    &	; �    &	< �    &	=     &	> C    &	? q    &	@ �    &	A �    &	B �    &	C 	    &	D ;	    &	E `	    &	F �	    &	G �	    &	H �	    &	I �	    &	J 
    &	K ;
    &	L W
    &	M y
    &	N �
    &	O �
    &	P �
    &	Q     &	R %    &	S C    &	T f    &	U �    &	V �    &	W �    &	X     &	Y A    &	Z r    &	[ �    &	\ �    &	] �    &	^ !    &	_ S    &	` }    &	a �    &	b �    &	c     &	d =    ��5      KaiHe GuardTourSystem V0.1��6<   ��5      
Cancel��6&   ��5	      
User Login��60   ��5
      	Login��6+   ��5      Use default password��6D   ��5      System Settings��67   ��5      Language��69   ��5      LED��6%   ��5      
Buzzer��6+   ��5      Record Count��67   ��5      Manager��6,   ��5      Operator��6.   ��5      
Device��6&   ��5      	Device ID��6+   ��5      	Route��6$   ��5      
Route Name��6-   ��5      	Place��6$   ��5       Place Order��6/   ��5!      
Place Name��6-   ��5"      
Place Card��6-   ��5$      Employee��6(   ��5%      Employee Name��61   ��5&      Employee Card��61   ��5(      	Event��6$   ��5)      
Event Name��6-   ��5*      
Event Card��6-   ��5,      	Frequence��6,   ��5-      Weekly Frequence��6:   ��5.      Irregular Frequence��6?   ��50      Patrol Data��6/   ��51      Patrol Time��6/   ��53      Add��6    ��54      Edit��6"   ��55      Del��6#   ��56      	Query��6$   ��58      Confirm��6(   ��59      
Cancel��6&   ��5:      	Close��6$   ��5;      Quit��6"   ��5<      	QueryDate��6,   ��5=      To��6'   ��5>      Export Excel��61   ��5?      	Print��6$   ��5C      
Select All��6-   ��5E      Reading ��6>   ��5F       获取失败��6?   ��5G       获取成功��6B   ��5H      Success��6(   ��5I      Fail��6"   ��5N      
Data Query��6-   ��5O      Read Patrol-records��6@   ��5P      Patrol Records��68   ��5Q      Read Hit-records��6:   ��5R      Patrol Results��67   ��5S      Patrol Charts��65   ��5T      
Reanalysis��67   ��5V      Patrol Setting��69   ��5W      Routes Setting��6>   ��5X      Workers Setting��6@   ��5Y      Events Setting��6>   ��5Z      Frequences Setting��6F   ��5[      Weekly Duty Setting��6E   ��5\      Irregular Duty Setting��6J   ��5^      Data Manage��63   ��5_      Backup and Recorvy��6K   ��5`      Clear Patrol Data��6H   ��5a      Import Patrol Data��6J   ��5b      Export Patrol Data��6J   ��5d      System Manage��61   ��5e      System Data Init��68   ��5f      Operator Manage��6=   ��5g      Modify Password��6A   ��5h      Language��64   ��5i      Device Test��69   ��5j      
System Log��61   ��5l      Help��6&   ��5m      
How to Use��64   ��5n      How to Start��68   ��5o      About Us��61   ��5r      Receive��60   ��5s      PatrolRecords��66   ��5t      AssessResult��64   ��5u      
Charts��6.   ��5v      Device Test��67   ��5z      Receive��6/   ��5{      	Clear��6.   ��5|      Receiving patrol datas...��6D   ��5}      &"Receive patrol data failed : {0} !��6N   ��5~       Receive patrol data success!��6K   ��5      The device data is empty!��6F   ��5�      Handling data...��6<   ��5�      Handle data failed!��6A   ��5�      Handle data success!��6E   ��5�      Saved {0} patrol record!��6G   ��5�      Patrol record save failed!��6F   ��5�      Updating result...��6B   ��5�      Update result failed!��6G   ��5�      Update result success!��6K   ��5�      Clearing device data...��6C   ��5�      "Clear device data failed: {0}!��6K   ��5�      Clear device data success!��6J   5�      