#if UNITY_ANDROID && TOAJIN_OAID_ENABLE

using System;
using System.Collections;
using System.Collections.Generic;
using PluginSet.Core;
using UnityEngine;
using Logger = PluginSet.Core.Logger;

namespace PluginSet.Taojin
{
    [PluginRegister]
    public class PluginTaojin: PluginBase, IPrivacyAuthorizationCallback, ICustomPlugin
    {
        private static Logger Logger = LoggerManager.GetLogger("Taojin");
        
        private static PluginTaojin _instance;
        
        public static string GetOAID()
        {
            if (!_instance)
            {
                return String.Empty;
            }
            
            return _instance.GetOaid();
        }
        
        private string _oaidResult;

        private string GetOaid()
        {
            if (string.IsNullOrEmpty(_oaidResult))
            {
                GetOaidAsync();
            }
            
            return _oaidResult;
        }
        
        public override string Name => "Taojin";

        private PluginSetCallback _callback;
        private List<SuccessCallback> _successCallbacks = new List<SuccessCallback>();
        private List<FailedCallback> _failedCallbacks = new List<FailedCallback>();

        protected override void Init(PluginSetConfig config)
        {
            base.Init(config);
            _instance = this;
        }

        public void CustomCall(string func, Action<Result> callback = null, string json = null)
        {
            if (func.ToLower() == "getoaid")
            {
                if (!string.IsNullOrEmpty(_oaidResult))
                {
                    callback?.Invoke(new Result()
                    {
                        Success = true,
                        Code = PluginConstants.SuccessCode,
                        PluginName = Name,
                        Data = _oaidResult,
                    });
                    return;
                }
                
                GetOaidAsync((result) =>
                {
                    callback?.Invoke(new Result()
                    {
                        Success = true,
                        Code = PluginConstants.SuccessCode,
                        PluginName = Name,
                        Data = result,
                    });
                }, (code, message) =>
                {
                    callback?.Invoke(new Result()
                    {
                        Success = false,
                        Code = code,
                        PluginName = Name,
                        Error = message,
                        Data = message,
                    });
                });
            }
        }

        public void OnPrivacyAuthorization()
        {
            GetOaidAsync();
        }

        private void GetOaidAsync(SuccessCallback success = null, FailedCallback failed = null)
        {
            if (success != null)
            {
                _successCallbacks.Add(success);
            }
                
            if (failed != null)
            {
                _failedCallbacks.Add(failed);
            }
            
            if (_callback != null)
            {
                return;
            }

            _callback = new PluginSetCallback(OnOaidGetSuccess, OnOaidGetFailed);
            StartCoroutine(GetAoidAsyncTimeout(3));
#if UNITY_EDITOR
            OnOaidGetFailed(PluginConstants.FailDefaultCode, "not support in editor");
#else
            TaojinAndroidApi.GetOAIDAsync(_callback);
#endif
        }

        private IEnumerator GetAoidAsyncTimeout(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            CancelOaidAsyncCallback("Get OAID timeout");
        }
        
        private void CancelOaidAsyncCallback(string reason)
        {
            if (_callback == null) return;
            _callback.onFailed(PluginConstants.CancelCode, reason);
            _callback = null;
        }
        
        private void OnOaidGetSuccess(string oaid)
        {
            _callback = null;
            _oaidResult = oaid;
            Logger.Debug($"OnOaidGetSuccess: {_oaidResult}");
            foreach (var successCallback in _successCallbacks)
            {
                successCallback.Invoke(oaid);
            }
        }
        
        private void OnOaidGetFailed(int code, string message)
        {
            _callback = null;
            Logger.Debug($"OnOaidGetFailed: {message}");
            foreach (var failedCallback in _failedCallbacks)
            {
                failedCallback.Invoke(code, message);
            }
        }
    }
}
#endif