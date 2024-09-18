// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		AliApi.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/13 10:26:12
// *******************************************

namespace Bridge.AliApi
{
	using Common;

	/// <summary>
	/// 
	/// </summary>
	public static class AliApi
	{
		private static IBridge _bridge;

		/// <summary>
		/// SDK桥接文件
		/// </summary>
		private static IBridge bridgeImpl
		{
			get
			{
				if (_bridge == null)
				{
#if UNITY_IOS && !UNITY_EDITOR
					_bridge = new iOSBridgeImpl();
#elif UNITY_ANDROID && !UNITY_EDITOR
					_bridge = new AndroidBridgeImpl();
#else
					_bridge = new EditorBridge();
#endif
				}

				return _bridge;
			}
		}

		/// <summary>
		/// 初始化sdk
		/// </summary>
		public static void InitAliApi()
		{
			bridgeImpl.InitBridge();
		}

		/// <summary>
		/// 是否下载了微信
		/// </summary>
		/// <returns></returns>
		public static bool IsAliPayAppInstalled()
		{
			return bridgeImpl.IsAliPayAppInstalled();
		}

		/// <summary>
		/// 拉起支付
		/// </summary>
		/// <param name="orderInfo">订单信息</param>
		/// <param name="listener">支付回调</param>
		public static void OpenPay(string orderInfo, IBridgeListener listener)
		{
			bridgeImpl.OpenPay(orderInfo, listener);
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="authInfo">认证信息</param>
		/// <param name="listener">认证回调</param>
		public static void AliPayAuth(string authInfo, IBridgeListener listener)
		{
			bridgeImpl.AliPayAuth(authInfo, listener);
		}
	}
}