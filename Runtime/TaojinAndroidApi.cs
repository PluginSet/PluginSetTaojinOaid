#if UNITY_ANDROID && TOAJIN_OAID_ENABLE

using PluginSet.Core;
using UnityEngine;

namespace PluginSet.Taojin
{
    public static class TaojinAndroidApi
    {
        private static AndroidJavaClass _helper;

        private static AndroidJavaClass Helper
        {
            get
            {
                if (_helper == null)
                    _helper = new AndroidJavaClass("com.pluginset.taojin.Helper");
                
                return _helper;
            }
        }
        

        private static AndroidJavaObject _currentActivity;
        
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (_currentActivity == null)
                    _currentActivity = AndroidClasses.UnityPlayer?.GetStatic<AndroidJavaObject>("currentActivity");
                
                return _currentActivity;
            }
        }
        
        public static void GetOAIDAsync(PluginSetCallback callback)
        {
            Helper.CallStatic("GetOAIDAsync", CurrentActivity, callback);
        }
    }
}
#endif