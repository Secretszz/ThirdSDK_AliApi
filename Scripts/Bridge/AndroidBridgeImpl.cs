// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		AndroidBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 17:57:18
// *******************************************

#if UNITY_ANDROID
namespace Bridge.AliApi
{
	using Common;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class AndroidBridgeImpl : IBridge
	{
		private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
		private const string ManagerClassName = "com.bridge.alipay.AliApiManager";
		private static AndroidJavaObject api;
		private static AndroidJavaObject currentActivity;

		/// <summary>
		/// 初始化sdk
		/// </summary>
		void IBridge.InitBridge()
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass(UnityPlayerClassName);
			currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaClass jc = new AndroidJavaClass(ManagerClassName);
			api = jc.CallStatic<AndroidJavaObject>("getInstance");
			api.Call("initAliApiManager", currentActivity);
		}

		/// <summary>
		/// 是否安装了微信客户端
		/// </summary>
		/// <returns></returns>
		bool IBridge.IsAliPayAppInstalled()
		{
			return api != null && api.Call<bool>("isAliPayAppInstalled");
		}

		/// <summary>
		/// 拉起支付
		/// </summary>
		/// <param name="orderInfo">订单信息</param>
		/// <param name="listener">支付回调</param>
		void IBridge.OpenPay(string orderInfo, IBridgeListener listener)
		{
			api?.Call("startAliPay", orderInfo, new BridgeCallback(listener));
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="listener">验证回调</param>
		void IBridge.AliPayAuth(IBridgeListener listener)
		{
			api?.Call("startAliAuth", new BridgeCallback(listener));
		}
	}
}
#endif