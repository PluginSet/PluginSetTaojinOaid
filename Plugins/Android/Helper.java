package com.pluginset.taojin;

import android.content.Context;

import com.lk.oaid.ErrorCode;
import com.lk.oaid.IGetter;
import com.lk.oaid.OAIDHelper;
import com.pluginset.core.IPluginSetCallback;

public class Helper {
    public static void GetOAIDAsync(Context context, IPluginSetCallback callback)
    {
        new OAIDHelper().getOaid(context, new IGetter() {
            @Override
            public void onOAIDGetComplete(String s) {
                callback.onSuccess(s);
            }

            @Override
            public void onOAIDGetError(ErrorCode errorCode, Exception e) {
                callback.onFailed(errorCode.ordinal(), e.toString());
            }
        });
    }
}