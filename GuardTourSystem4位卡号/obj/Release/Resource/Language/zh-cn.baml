   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   RoleManager  RoleOperator 
 Device  DeviceID 	 Route  	RouteName 	 Place  
PlaceOrder  	PlaceName 	 	PlaceCard 

 Worker  
WorkerName  
WorkerCard 	 Event  	EventName  	EventCard  	Frequence  FrequenceRegular  FrequenceIrregular  
PatrolData  
PatrolTime  Add  Edit 
 Delete 	 Query  Confirm 
 Cancel 	 Close  Quit 	 Clear 	 Reset  ExportExcel 	  Print ! 	SelectAll " Reading # ReadingFail $ ReadingSuccess % Success & Fail ' PortErrorWakeUp ( PortErrorInit ) PortErrorNoDevice * PortErrorNoDeviceOrOccupy + PortErrorMultiDevice , PortErrorWrongData - PortErrorTimeOut . PortSuccess / InfoViewDefaultTitle 0 	QueryDate 1 QueryDateTo 2 QueryComplete 3 QueryDateErrorDate 4 QueryDateErrorBegin 5 QueryDateErrorEnd 6 ApplyFilter 7 ClearFilter 8 FilterValueEmpty 9 FilterValueNull : LoginWindowTitle ; LoginWindowConfirmText < LoginWindowPasswordHint = LoginWindowPasswordTootTip !> LoginWindowErrorEmptyPassword !? LoginWindowErrorWrongPassword @ 	MenuQuery A MenuQueryReadPatrol B MenuQueryRawData C MenuQueryRawCount D MenuQueryReadHit E MenuQueryResult F MenuQueryChart G MenuQueryReanalysis H MenuPatrolSetting I MenuPatrolSettingRoute J MenuPatrolSettingWorker K MenuPatrolSettingEvent L MenuPatrolSettingFrequence M MenuPatrolSettingRegular N MenuPatrolSettingIrregular O MenuDataManage #P MenuDataManageBackupAndRecovery !Q MenuDataManageClearPatrolData "R MenuDataManageImportPatrolData "S MenuDataManageExportPatrolData T 
MenuSystem U MenuSystemInit V MenuSystemUserManage W MenuSystemModifyPassword X MenuSystemLanguage Y MenuSystemDeviceTest Z MenuSystemLog [ MenuHelp \ MenuHelpHowToUse ] MenuHelpHowToStart ^ MenuHelpAboutUs _ ShortcutReceive ` ShortcutRawdata a ShortcutRawCount b ShortcutResult c ShortcutCharts d ShortcutDeviceTest e PatrolDataRead f PatrolDataClear g PatrolDataReading h PatrolDataReadFail i PatrolDataReadSuccess j PatrolDataEmptyData k PatrolDataHandling l PatrolDataHandleFail m PatrolDataHandleSuccess n PatrolDataSaveSuccess o PatrolDataSaveFail p PatrolDataDutyUpdating q PatrolDataDutyUpdateFail r PatrolDataDutyUpdateSuccess s PatrolDataClearing t PatrolDataClearFail u PatrolDataClearSuccess v RawData w QueryResultShowOnDuty x QueryResultShowMissDuty y Card z BatchAdd { BatchAddClearAfterRead | BatchAddRead } BatchAddReadSingle ~ BatchAddReadEvent  BatchAddReadPlace � BatchAddReadWorker � BatchAddReadErrorConnect � BatchAddReadErrorConnectExp � BatchAddReadErrorEmpty � BatchAddReadErrorEmptyExp � BatchAddReadErrorExists "� BatchAddReadErrorNoRouteSelect !� BatchAddSingleReadErrorNoCard $�  BatchAddSingleReadErrorNoCardExp "� BatchAddSingleReadErrorCantAdd %� !BatchAddSingleReadErrorCantAddExp � RouteSettingAddRoute � RouteSettingAddPlace � RouteSettingDelRouteConfirm "� RouteSettingDelRouteConfirmExp � RouteSettingDelRouteError � RouteSettingDelPlaceConfirm "� RouteSettingDelPlaceConfirmExp � RouteSettingDelPlaceError � RouteSettingSendToDevice #� RouteSettingSendToDeviceConfirm &� "RouteSettingSendToDeviceConfirmExp � RouteSettingSendindToDevice #� RouteSettingSendToDeviceSuccess  � RouteSettingSendToDeviceFail � RouteSettingEditRoute � Backup_DefaultFileName  � ExportPatrol_DefaultFileName � DeviceTestVerifingTime � DeviceTestVerifyTimeSuccess � DeviceTestVerifyTimeFail 
� Monday � Tuesday � 	Wednesday � Thursday 
� Friday � Saturday 
� Sunday � APP_NAME � APP_VERSION � COMPANY_NAME � COMPANY_TEL � COMPANY_ADDRESS � COMPANY_WEBSITE � COMPANY_COPYRIGHT � LOGIN_WINDOW � LOGIN_BUTTON � LOGIN_DEFAULT_PASSWORD � SETTING_WINDOW � SETTING_SELECT_LANGUAGE � SEARCH_WORKER_HINT � SEARCH_LINE_HINT � SEARCH_PLAN_HINT � TEST_LED � TEST_BUZZER � TEST_DEVICE_ID � TEST_DEVICE_TIME � TEST_PATROL_COUNT � TEST_HIT_COUNT � TEST_CONNECTING � TEST_SUCCESS � 	TEST_Fail � TEST_TIME_ADJUST � TEST_TIME_SUCCESS � TEST_GET_PATROL_COUNT � TEST_GET_MACHINE_ID � SYSTEM_ERROR � Export_RouteAndPlace � Export_Worker � Export_Event � Export_Frequence � WorkerCountInfo � RouteCountInfo�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     8d2http://schemas.microsoft.com/expression/blend/2008  Bmc;http://schemas.openxmlformats.org/markup-compatibility/2006  ,s&clr-namespace:System;assembly=mscorlib  %Q  &	        &	      &	 &     &	 9     &	 I     &	 Y     &	 o     &	      &	 �     &		 �     &	
 �     &	 �     &	 �     &	     &	     &	 2    &	 H    &	 X    &	 n    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	     &	     &	 #    &	 3    &	 C    &	 S    &	  h    &	! x    &	" �    &	# �    &	$ �    &	% �    &	& �    &	' �    &	( 	    &	) (    &	* G    &	+ {    &	, �    &	- �    &	. �    &	/ �    &	0     &	1 !    &	2 .    &	3 D    &	4 r    &	5 �    &	6 �    &	7 �    &	8 �    &	9 �    &	:     &	;     &	< ,    &	= T    &	> �    &	? �    &	@ �    &	A     &	B     &	C 3    &	D I    &	E _    &	F u    &	G �    &	H �    &	I �    &	J �    &	K �    &	L     &	M *    &	N F    &	O e    &	P {    &	Q �    &	R �    &	S �    &	T �    &	U �    &	V     &	W 0    &	X F    &	Y \    &	Z r    &	[ �    &	\ �    &	] �    &	^ �    &	_ �    &	` �    &	a 	    &	b "	    &	c 8	    &	d N	    &	e d	    &	f �	    &	g �	    &	h �	    &	i �	    &	j 
    &	k 0
    &	l U
    &	m x
    &	n �
    &	o �
    &	p �
    &	q     &	r 4    &	s W    &	t     &	u �    &	v �    &	w �    &	x     &	y     &	��.    &	z >    &	{ T    &	| v    &	} �    &	~ �    &	 �    &	� �    &	� �    &	�     &	� x    &	� �    &	� �    &	� 	    &	� 7    &	� Y    &	� �    &	� �    &	� #    &	� 9    &	� O    &	� r    &	� �    &	� �    &	� �    &	� ,    &	� I    &	� e    &	� �    &	� �    &	�     &	� >    &	� n    &	� �    &	� �    &	� �    &	� �    &	� �    &	� (    &	� 8    &	� H    &	� X    &	� h    &	� x    &	� �    &	� �    &	� �    &	� �    &	� �    &	� 	    &	� J    &	� b    &	� }    &	� �    &	� �    &	� �    &	� �    &	� �    &	�     &	� ;    &	� ]    &	� m    &	� �    &	� �    &	� �    &	� �    &	� �    &	� 	    &	�     &	� 5    &	� W    &	� v    &	� �    &	� �    &	� �    &	� �    &	�     &	�     &	� 2    &	� Q    ��	管理员����	操作员����	计数机����
机号����
部门����部门名称����
人员����人员序号����人员名称����人员钮号����	管理卡����管理卡名称����管理卡钮号����计数事件����事件名称����事件钮号����
班次����按周排班����无规律排班����打卡数据����打卡时间����
添加����
修改����
删除����
查询����
确认����
取消����
关闭����
退出����
清空����
重置����导出Excel����
打印����
全选����正在获取 ���� 获取失败���� 获取成功����
成功����
失败����计数机未响应����USB口初始化失败����未找到计数设备����.*没有计数设备或设备USB口被占用����)%计数设备不唯一,请单独连接����错误的响应数据����通信超时����通信成功����通信信息����查询日期����至����查询完成����($开始日期要在结束日期之前����"请选择正确的开始日期����"请选择正确的结束日期����
筛选����清空筛选����	[空]����	[空]����用户登录����
登录����"获取默认密码请点击[?]����b^获取当前用户默认密码.

管理员的默认密码为{0}
操作员的默认密码为{1}����请输入密码...����密码有误...����数据查询����数据接收����打卡记录����打卡总数����敲击记录����考核结果����统计图表����重新分析����信息录入����设置人员信息����设置管理卡信息����设置打卡事件����设置打卡班次����设置按周排班����设置无规律排班����数据维护����备份与还原����清理计数数据����导入计数数据����导出计数数据����系统管理����系统初始化����操作员管理����修改密码����修改语言����通讯管理����系统操作日志����
帮助����使用帮助����如何开始����关于我们����数据接收����打卡记录����打卡总数����考核结果����统计图表����通讯管理����接收打卡数据����清空打卡数据����正在接收计数数据...����#计数数据接收失败: {0} !����计数数据接收成功!����计数机中没有数据!����正在处理计数数据...����计数数据处理失败!����计数数据处理成功!����($成功保存了 {0} 条计数数据!����计数数据保存失败!����正在更新考核结果...����考核结果更新失败!����考核结果更新成功!����"正在清空计数机数据...����%!计数机数据清空失败: {0}!���� 计数机数据清空成功!����计数记录����显示准时数据����显示漏巡数据����
钮号����
名称����批量添加����读取后清空打卡机����批量读取����单个读取����批量添加事件����批量添加人员����批量添加管理卡����读取打卡记录失败����WS1.请检查计数机是否连接到电脑
2.请确认驱动程序是否正确安装����没有可用的数据����>:已连接到计数机,但计数机中没有可用的数据����($该名称在选中项中出现多次����($请在下方选择员工所在部门����计数机没有读到卡����zv请在点击 [单个读取] 按键后的5秒内使用计数机读卡,读卡结果将显示在批量添加的列表中.����无法添加该卡����($该卡已被读取到批量添加中����新增部门����新增人员����确定要删除该部门?����73将同时删除所有与该部门有关的数据...����部门删除失败!����确定要删除该人员?����73将同时删除所有与该人员有关的数据...����人员删除失败!����发送到计数机����.*将重新设置计数机中的路线信息����B>该操作预计耗时1分钟,请保持计数机的正常连接����+'正在设置计数机的路线信息...����&"计数机路线信息设置成功!����*&计数机路线信息设置失败: {0}����编辑部门����数据备份����计数数据����"正在校准计数机时间...���� 计数机时间校准成功!����$ 计数机时间校准失败: {0}����
周一����
周二����
周三����
周四����
周五����
周六����
周天����智能计数管理系统����
V2.4.0����"福州凯和电子有限公司����0591-83308208����;7福建省福州市六一中路115号机械研究院2楼����www.kai-he.com����版权所有 2017����用户登录����
登录����使用默认密码����系统设置����
语言����.*查找计数员姓名、钮号、设备号����查找线路、地点卡����查找线路、计数员����
LED灯����振动或蜂鸣����读计数机机号����读计数机时间����读计数记录数����读敲击记录数����	连接中����测试成功����测试失败����计数机时间已校准����计数机时间正常����当前有 {0} 条数据����计数机机号为: {0}����系统错误����部门与人员信息����管理卡信息����事件信息����班次信息����管理卡统计信息����部门统计信息��