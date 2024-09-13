
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IBridge.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 17:26:08
// *******************************************

namespace Bridge.AliApi
{
	using Common;
	
	/// <summary>
	/// 
	/// </summary>
	internal interface IBridge
	{
		/// <summary>
		/// 初始化sdk
		/// </summary>
		void InitBridge();

		/// <summary>
		/// 是否安装了支付宝客户端
		/// </summary>
		/// <returns></returns>
		bool IsAliPayAppInstalled();

		/// <summary>
		/// 拉起支付
		/// </summary>
		/// <param name="orderInfo">订单信息</param>
		/// <param name="listener">支付回调</param>
		void OpenPay(string orderInfo, IBridgeListener listener);

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="listener">验证回调</param>
		void AliPayAuth(IBridgeListener listener);
	}
}