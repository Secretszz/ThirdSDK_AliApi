package com.bridge.aliapi;

import android.app.Activity;
import android.text.TextUtils;
import android.util.Log;

import com.alipay.sdk.app.AuthTask;
import com.alipay.sdk.app.PayTask;
import com.bridge.common.listener.IBridgeListener;

import java.util.Map;

public class AliApiManager {
    private static final String TAG = AliApiManager.class.getName();
    public static AliApiManager getInstance(){
        return Holder.INSTANCE;
    }

    /**
     * 成功
     */
    private final static String RESULT_STATUS_SUCCESS = "9000";
    /**
     * 正在处理中
     */
    private final static String RESULT_STATUS_PROCESSING = "8000";
    /**
     * 失败
     */
    private final static String RESULT_STATUS_FAILED = "4000";
    /**
     * 重复请求
     */
    private final static String RESULT_STATUS_REPEAT_REQUEST = "5000";
    /**
     * 用户中途取消
     */
    private final static String RESULT_STATUS_USER_CANCEL = "6001";
    /**
     * 网络连接出错
     */
    private final static String RESULT_STATUS_NETWORK_ERROR = "6002";
    /**
     * 结果未知
     */
    private final static String RESULT_STATUS_UNKNOWN = "6004";

    public void initAliApiManager(Activity activity){

    }

    /**
     * 拉起支付宝支付
     * @param activity 主程序
     * @param orderInfo 订单信息
     * @param payListener 支付回调
     */
    public void startAliPay(Activity activity, String orderInfo, IBridgeListener payListener){
        Thread payThread = new Thread(new PayRunnable(activity, orderInfo, payListener));
        payThread.start();
    }

    /**
     * 拉起支付宝认证
     * @param activity 主程序
     * @param authInfo 认证信息
     * @param authListener 认证回调
     */
    public void startAliAuth(Activity activity, String authInfo, IBridgeListener authListener){
        Thread authThread = new Thread(new AuthRunnable(activity, authInfo, authListener));
        authThread.start();
    }

    /**
     * 支付线程
     */
    private static class PayRunnable implements Runnable{

        private final Activity activity;
        private final String order;
        private final IBridgeListener payListener;

        private PayRunnable(Activity activity, String order, IBridgeListener payListener) {
            this.activity = activity;
            this.order = order;
            this.payListener = payListener;
        }

        /**
         * When an object implementing interface <code>Runnable</code> is used
         * to create a thread, starting the thread causes the object's
         * <code>run</code> method to be called in that separately executing
         * thread.
         * <p>
         * The general contract of the method <code>run</code> is that it may
         * take any action whatsoever.
         *
         * @see Thread#run()
         */
        @Override
        public void run() {
            PayTask aliPay = new PayTask(activity);
            Map<String, String> rawResult = aliPay.payV2(order, true);
            //LOG.debug(rawResult.toString());
            if (rawResult == null) {
                Log.e(TAG, "rawResult == null");
                return;
            }

            PayResult payResult = new PayResult(rawResult);
            String resultStatus = payResult.getResultStatus();
            String result = payResult.getResult();
            String memo = payResult.getMemo();

            if (TextUtils.equals(resultStatus, RESULT_STATUS_SUCCESS)
                    || TextUtils.equals(resultStatus, RESULT_STATUS_PROCESSING)
                    || TextUtils.equals(resultStatus, RESULT_STATUS_UNKNOWN)) {
                payListener.onSuccess(result);
            } else if (TextUtils.equals(resultStatus, RESULT_STATUS_USER_CANCEL)) {
                payListener.onCancel();
            } else {
                payListener.onError(Integer.parseInt(resultStatus), result);
            }
            Log.d(TAG, memo);
        }
    }

    private static class AuthRunnable implements Runnable{

        private final Activity activity;
        private final String authInfo;
        private final IBridgeListener authListener;

        private AuthRunnable(Activity activity, String authInfo, IBridgeListener authListener) {
            this.activity = activity;
            this.authInfo = authInfo;
            this.authListener = authListener;
        }

        /**
         * When an object implementing interface <code>Runnable</code> is used
         * to create a thread, starting the thread causes the object's
         * <code>run</code> method to be called in that separately executing
         * thread.
         * <p>
         * The general contract of the method <code>run</code> is that it may
         * take any action whatsoever.
         *
         * @see Thread#run()
         */
        @Override
        public void run() {
            AuthTask authTask = new AuthTask(this.activity);
            Map<String, String> rawResult = authTask.authV2(authInfo, true);
            if (rawResult == null) {
                Log.e(TAG, "rawResult == null");
                return;
            }

            AuthResult authResult = new AuthResult(rawResult, true);
            String resultStatus = authResult.getResultStatus();
            String result = authResult.getResult();
            String memo = authResult.getMemo();

            if (TextUtils.equals(resultStatus, RESULT_STATUS_SUCCESS)) {
                String resultCode = authResult.getResultCode();
                if (TextUtils.equals(resultCode, "200")){
                    authListener.onSuccess(result);
                } else {
                    authListener.onError(Integer.parseInt(resultCode), result);
                }
            } else if (TextUtils.equals(resultStatus, RESULT_STATUS_USER_CANCEL)) {
                authListener.onCancel();
            } else {
                authListener.onError(Integer.parseInt(resultStatus), result);
            }
            Log.d(TAG, memo);
        }
    }

    private static class Holder{
        private final static AliApiManager INSTANCE = new AliApiManager();
    }
}
