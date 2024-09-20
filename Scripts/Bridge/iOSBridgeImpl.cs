// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IOSBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 17:24:53
// *******************************************

#if UNITY_IOS
namespace Bridge.AliApi
{
	using Common;
	using System.Runtime.InteropServices;
	using AOT;

	/// <summary>
	/// 
	/// </summary>
	internal class iOSBridgeImpl : IBridge
	{
		/// <summary>
		/// 初始化sdk
		/// </summary>
		void IBridge.InitBridge()
		{
			ali_init();
		}

		/// <summary>
		/// 是否安装了微信客户端
		/// </summary>
		/// <returns></returns>
		bool IBridge.IsAliPayAppInstalled()
		{
			return ali_isAliPayAppInstalled();
		}

		/// <summary>
		/// 拉起支付
		/// </summary>
		/// <param name="orderInfo">订单信息</param>
		/// <param name="listener">支付回调</param>
		void IBridge.OpenPay(string orderInfo, IBridgeListener listener)
		{
			PayCallback._payListener = listener;
			ali_Purchase(orderInfo, PayCallback.OnSuccess, PayCallback.OnCancel, PayCallback.OnError);
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="authInfo">认证信息</param>
		/// <param name="listener">认证回调</param>
		void IBridge.AliPayAuth(string authInfo, IBridgeListener listener)
		{
			AuthCallback._authListener = listener;
			ali_Auth(authInfo, AuthCallback.OnSuccess, AuthCallback.OnCancel, AuthCallback.OnError);
		}

		/// <summary>
		/// 初始化
		/// </summary>
		[DllImport("__Internal")]
		private static extern void ali_init();

		/// <summary>
		/// 是否下载了微信客户端
		/// </summary>
		/// <returns></returns>
		[DllImport("__Internal")]
		private static extern bool ali_isAliPayAppInstalled();

		/// <summary>
		/// 支付
		/// </summary>
		/// <param name="orderInfo"></param>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void ali_Purchase(string orderInfo, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 请求权限
		/// </summary>
		/// <param name="authInfo">认证信息</param>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void ali_Auth(string authInfo, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		private static class PayCallback
		{
			/// <summary>
			/// 支付回调监听
			/// </summary>
			public static IBridgeListener _payListener;

			/// <summary>
			/// 支付成功回调桥接函数
			/// </summary>
			/// <param name="result"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Success))]
			public static void OnSuccess(string result)
			{
				_payListener?.OnSuccess(result);
			}

			/// <summary>
			/// 支付用户取消回调桥接函数
			/// </summary>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Cancel))]
			public static void OnCancel()
			{
				_payListener?.OnCancel();
			}

			/// <summary>
			/// 支付错误回调桥接函数
			/// </summary>
			/// <param name="errCode"></param>
			/// <param name="errMsg"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Error))]
			public static void OnError(int errCode, string errMsg)
			{
				_payListener?.OnError(errCode, errMsg);
			}
		}
		
		private static class ShareCallback
		{
			/// <summary>
			/// 分享回调监听
			/// </summary>
			public static IBridgeListener _shareListener;

			/// <summary>
			/// 支付成功回调桥接函数
			/// </summary>
			/// <param name="result"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Success))]
			public static void OnSuccess(string result)
			{
				_shareListener?.OnSuccess(result);
			}

			/// <summary>
			/// 支付用户取消回调桥接函数
			/// </summary>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Cancel))]
			public static void OnCancel()
			{
				_shareListener?.OnCancel();
			}

			/// <summary>
			/// 支付错误回调桥接函数
			/// </summary>
			/// <param name="errCode"></param>
			/// <param name="errMsg"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Error))]
			public static void OnError(int errCode, string errMsg)
			{
				_shareListener?.OnError(errCode, errMsg);
			}
		}
		
		private static class AuthCallback
		{
			/// <summary>
			/// 权限回调监听
			/// </summary>
			public static IBridgeListener _authListener;

			/// <summary>
			/// 支付成功回调桥接函数
			/// </summary>
			/// <param name="result"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Success))]
			public static void OnSuccess(string result)
			{
				_authListener?.OnSuccess(result);
			}

			/// <summary>
			/// 支付用户取消回调桥接函数
			/// </summary>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Cancel))]
			public static void OnCancel()
			{
				_authListener?.OnCancel();
			}

			/// <summary>
			/// 支付错误回调桥接函数
			/// </summary>
			/// <param name="errCode"></param>
			/// <param name="errMsg"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Error))]
			public static void OnError(int errCode, string errMsg)
			{
				_authListener?.OnError(errCode, errMsg);
			}
		}
	}
}
#endif