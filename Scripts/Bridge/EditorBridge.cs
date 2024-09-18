// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		EditorBridge.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 17:33:45
// *******************************************

#if (UNITY_IOS || UNITY_ANDROID) && UNITY_EDITOR
namespace Bridge.AliApi
{
	using Common;
	
	/// <summary>
	/// 
	/// </summary>
	internal class EditorBridge : IBridge
	{
		/// <summary>
		/// 初始化sdk
		/// </summary>
		void IBridge.InitBridge()
		{
		}

		bool IBridge.IsAliPayAppInstalled()
		{
			return false;
		}

		void IBridge.OpenPay(string orderInfo, IBridgeListener listener)
		{
			listener?.OnSuccess("");
		}

		void IBridge.AliPayAuth(string authInfo, IBridgeListener listener)
		{
			listener?.OnCancel();
		}
	}
}
#endif