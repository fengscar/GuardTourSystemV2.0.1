using GuardTourSystem.ViewModel;
using KaiheSerialPortLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuardTourSystem.Utils
{
    enum LanKey
    {
         APP_NAME,
        APP_VERSION,
        APP_LANGUAGE,
        APP_LOGINUSER,
        COMPANY_NAME,
        COMPANY_TEL,
        COMPANY_ADDRESS,
        COMPANY_WEBSITE,
        COMPANY_COPYRIGHT,

        CANCEL,
        LOGIN_WINDOW, LOGIN_BUTTON, LOGIN_DEFAULT_PASSWORD,
        SETTING_WINDOW, SETTING_SELECT_LANGUAGE,
        SEARCH_WORKER_HINT, SEARCH_LINE_HINT, SEARCH_PLAN_HINT,
        TEST_LED, TEST_BUZZER, TEST_PATROL_COUNT, TEST_DEVICE_TIME, TEST_DEVICE_ID, TEST_HIT_COUNT,
        TEST_CONNECTING, TEST_SUCCESS, TEST_Fail, TEST_TIME_ADJUST, TEST_TIME_SUCCESS, TEST_GET_PATROL_COUNT, TEST_GET_MACHINE_ID,
        SYSTEM_ERROR,

        #region 常用名称
        RoleManager,//管理员 
        RoleOperator,//操作员 

        Device,//巡检机 
        DeviceID,//机号 

        Route,//巡检线路 
        RouteName,//线路名称 

        Place,//巡检点 
        PlaceOrder,//巡检点序号 
        PlaceName,//巡检点名称 
        PlaceCard,//巡检点钮号 

        Worker,//巡检员 
        WorkerName,//巡检员名称 
        WorkerCard,//巡检员钮号 

        Event,//巡检事件 
        EventName,//事件名称 
        EventCard,//事件钮号 

        PatrolData,//巡检数据 
        PatrolTime,//巡检时间 

        Add,//添加 
        Edit,//修改 
        Delete,//删除 

        Confirm,//确认 
        Cancel,//取消 
        Close,//关闭 
        Quit,//退出 

        SelectAll,//全选 

        Reading,//正在获取 
        ReadingFail,//获取失败 
        ReadingSuccess,//获取成功 
        Success,//成功 
        Fail,//失败 

        #endregion

        #region 串口通信结果
        PortErrorWakeUp,//巡检机未响应 
        PortErrorInit,//USB口初始化失败 
        PortErrorNoDevice,//未找到巡检设备 
        PortErrorNoDeviceOrOccupy,//没有巡检设备或设备USB口被占用 
        PortErrorMultiDevice,//巡检设备不唯一,请单独连接 
        PortErrorWrongData,//错误的响应数据 
        PortErrorTimeOut,//通信超时 
        PortSuccess,//通信成功 
        #endregion

        #region 菜单
        MenuQuery,
        MenuQueryReadPatrol,
        MenuQueryRawData,
        MenuQueryReadHit,
        MenuQueryResult,
        MenuQueryChart,
        MenuQueryReanalysis,

        MenuPatrolSetting,//信息录入
        MenuPatrolSettingRoute,//设置巡检点
        MenuPatrolSettingWorker,//设置巡检员 
        MenuPatrolSettingEvent,//设置巡检事件 
        MenuPatrolSettingFrequence,//设置巡检班次 
        MenuPatrolSettingRegular,//设置按周排班 
        MenuPatrolSettingIrregular,//设置无规律排班   

        MenuDataManage,//数据维护 
        MenuDataManageBackupAndRecovery,//备份与还原 
        MenuDataManageClearPatrolData,//清理巡检数据  
        MenuDataManageImportPatrolData,//导入巡检数据 
        MenuDataManageExportPatrolData,//导出巡检数据 
        MenuDataManageSharePatrolData,//共享文件夹设置

        MenuSystem,//系统管理 
        MenuSystemInit,//系统初始化 
        MenuSystemUserManage,//操作员管理 
        MenuSystemModifyPassword,//修改密码 
        MenuSystemLanguage,//修改语言 
        MenuSystemDeviceTest,//通讯测试 
        MenuSystemLog,//系统操作日志 

        MenuHelp,//帮助 
        MenuHelpHowToUse,//使用帮助 
        MenuHelpHowToStart,//如何开始 
        MenuHelpAboutUs,//关于我们 
        #endregion

        #region 登录窗口
        LoginWindowTitle,
        LoginWindowConfirmText,
        LoginWindowPasswordHint,
        LoginWindowPasswordTootTip,
        LoginWindowErrorEmptyPassword,
        LoginWindowErrorWrongPassword,
        #endregion

        #region 数据查询-数据接收
        PatrolDataRead,//接收巡检数据
        PatrolDataClear,//清空巡检数据
        PatrolDataReading,//正在接收巡检数据
        PatrolDataReadFail,//巡检数据 接收失败: {0}
        PatrolDataReadSuccess,//巡检数据接收成功
        PatrolDataEmptyData,//巡检机中没有数据
        PatrolDataHandling,//正在处理巡检数据...
        PatrolDataHandleFail,//巡检数据 处理失败
        PatrolDataHandleSuccess,//巡检数据 处理成功
        PatrolDataSaveSuccess,//成功保存了 {0} 条巡检数据
        PatrolDataSaveFail,//巡检数据保存失败
        PatrolDataDutyUpdating,//正在更新考核结果...
        PatrolDataDutyUpdateFail,//考核结果 更新失败
        PatrolDataDutyUpdateSuccess,//考核结果 更新成功
        PatrolDataClearing,//正在清空巡检机数据...
        PatrolDataClearFail,//巡检机数据清空失败
        PatrolDataClearSuccess,//巡检机数据清空成功
        #endregion
        #region 数据查询-巡检记录
        RawData,//巡检记录

        #endregion

        #region 系统管理-通讯测试
        DeviceTestVerifingTime,//正在校准巡检机时间...
        DeviceTestVerifyTimeSuccess,//巡检机时间校准成功
        DeviceTestVerifyTimeFail,//巡检机时间校准失败 {0}
        #endregion

        Export_RouteAndPlace,
        Export_Worker,
        Export_Event,
        Export_Frequence,

        Backup_DefaultFileName,

        ExportPatrol_DefaultFileName,

        InfoViewDefaultTitle,

        QueryDateTo,
        QueryDateErrorDate,
        QueryDateErrorBegin,
        QueryDateErrorEnd,
        QueryComplete,
        BatchAddReadEvent,
        BatchAddReadPlace,
        BatchAddReadWorker,
        BatchAddReadErrorConnectExp,
        BatchAddReadErrorConnect,
        BatchAddReadErrorEmptyExp,
        BatchAddReadErrorEmpty,
        BatchAddReadErrorExists,
        BatchAddSingleReadErrorNoCardExp,
        BatchAddSingleReadErrorNoCard,
        BatchAddSingleReadErrorCantAddExp,
        BatchAddSingleReadErrorCantAdd,
        RouteSettingDelRouteConfirm,
        RouteSettingDelRouteError,
        RouteSettingDelRouteConfirmExp,
        RouteSettingDelPlaceConfirmExp,
        RouteSettingDelPlaceConfirm,
        RouteSettingSendindToDevice,
        RouteSettingSendToDeviceConfirm,
        RouteSettingSendToDeviceConfirmExp,
        RouteSettingSendToDeviceSuccess,
        RouteSettingDelPlaceError,
        RouteSettingSendToDeviceFail,
        BatchAddReadErrorNoRouteSelect,
        RouteSettingAddRoute,
        RouteSettingEditRoute,
        FilterValueEmpty,
        FilterValueNull,

        WorkerCountInfo,
        RouteCountInfo,




    }

    /// <summary>
    /// XAML Resource Language String Loader
    /// </summary>
    class LanLoader
    {
        public static string Load(LanKey key, params object[] param)
        {
            try
            {
                var format = LoadString(key.ToString());
                if (format != null && param != null)
                {
                    return String.Format(format, param);
                }
                else
                {
                    return format;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e + "载入语言:" + key + "时出错");
                return null;
            }
        }



        ///
        /// 得到当前语言下 参数KEY对应的字符串
        /// 若没有,则返回NULL
        private static string LoadString(string key)
        {
            var md = Application.Current.Resources.MergedDictionaries[0];
            if (md.Contains(key))
            {
                return (string)md[key];
            }
            else
            {
                return null;
            }
        }

        // 根据 语言编码名称找到对应文件夹下的资源文件,并重新载入...
        public static void ChangeLanguage(string lanCode)
        {
            if (String.IsNullOrEmpty(lanCode))
            {
                lanCode = "zh-cn";
            }

            string filePath = "pack://application:,,,/Resource/Language/" + lanCode + ".xaml";

            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(filePath, UriKind.RelativeOrAbsolute)
            };
            //菜单不是绑定的,要主动改变
            AppMenuViewModel.Instance.InitMenu();
        }

    }
}
