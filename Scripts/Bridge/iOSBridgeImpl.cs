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
	using System;
	using UnityEngine;
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
			wx_init();
		}

		/// <summary>
		/// 是否安装了微信客户端
		/// </summary>
		/// <returns></returns>
		bool IBridge.IsWXAppInstalled()
		{
			return wx_isWXAppInstalled();
		}

		/// <summary>
		/// 拉起微信客服
		/// </summary>
		/// <param name="groupId">企业ID</param>
		/// <param name="kfid">客服ID</param>
		bool IBridge.OpenCustomerServiceChat(string groupId, string kfid)
		{
			return wx_openCustomerServiceChat(groupId, kfid);
		}

		/// <summary>
		/// 拉起支付
		/// </summary>
		/// <param name="orderInfo">订单信息</param>
		/// <param name="listener">支付回调</param>
		void IBridge.OpenPay(string orderInfo, IBridgeListener listener)
		{
			PayCallback._payListener = listener;
			wx_Purchase(orderInfo, PayCallback.OnSuccess, PayCallback.OnCancel, PayCallback.OnError);
		}

		/// <summary>
		/// 分享图片到微信
		/// </summary>
		/// <param name="imagePath">图片路径</param>
		/// <param name="scene">分享场景</param>
		/// <param name="listener">分享回调</param>
		void IBridge.ShareImage(string imagePath, int scene, IBridgeListener listener)
		{
			ShareCallback._shareListener = listener;
			wx_shareImage(imagePath, scene, ShareCallback.OnSuccess, ShareCallback.OnCancel, ShareCallback.OnError);
		}

		/// <summary>
		/// 分享图片到微信
		/// </summary>
		/// <param name="imageData">图片数据</param>
		/// <param name="scene">分享场景</param>
		/// <param name="listener">分享回调</param>
		void IBridge.ShareImage(byte[] imageData, int scene, IBridgeListener listener)
		{
			try
			{
				ShareCallback._shareListener = listener;
				int length = imageData.Length;
				IntPtr buffer = Marshal.AllocHGlobal(length);
				Marshal.Copy(imageData, 0, buffer, length);
				wx_shareImageWithDatas(buffer, length, scene, ShareCallback.OnSuccess, ShareCallback.OnCancel, ShareCallback.OnError);
			}
			catch (Exception e)
			{
				Debug.LogError("字节流转指针解析错误：" + e.Message);
				listener?.OnError(-1, e.Message);
			}
		}
				
		/// <summary>
		/// 分享链接
		/// </summary>
		/// <param name="linkUrl">链接地址</param>
		/// <param name="scene">分享场景</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void IBridge.ShareLink(string linkUrl, int scene, IBridgeListener listener)
		{
			listener?.OnError(-1, "not support");
		}

		/// <summary>
		/// 分享视频
		/// </summary>
		/// <param name="videoUrl">视频地址</param>
		/// <param name="scene">分享场景</param>
		/// <param name="listener">拉起分享窗口事件</param>
		void IBridge.ShareVideo(string videoUrl, int scene, IBridgeListener listener)
		{
			listener?.OnError(-1, "not support");
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="listener">验证回调</param>
		void IBridge.WeChatAuth(IBridgeListener listener)
		{
			AuthCallback._authListener = listener;
			wx_Auth(AuthCallback.OnSuccess, AuthCallback.OnCancel, AuthCallback.OnError);
		}

		/// <summary>
		/// 初始化
		/// </summary>
		[DllImport("__Internal")]
		private static extern void wx_init();

		/// <summary>
		/// 打开客服界面
		/// </summary>
		/// <param name="groupId"></param>
		/// <param name="kfid"></param>
		/// <returns></returns>
		[DllImport("__Internal")]
		private static extern bool wx_openCustomerServiceChat(string groupId, string kfid);

		/// <summary>
		/// 是否下载了微信客户端
		/// </summary>
		/// <returns></returns>
		[DllImport("__Internal")]
		private static extern bool wx_isWXAppInstalled();

		/// <summary>
		/// 支付
		/// </summary>
		/// <param name="orderInfo"></param>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void wx_Purchase(string orderInfo, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 分享图片
		/// </summary>
		/// <param name="path">图片本地路径</param>
		/// <param name="scene">分享场景</param>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void wx_shareImage(string path, int scene, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 分享图片
		/// </summary>
		/// <param name="datas">图片数据字节流指针</param>
		/// <param name="length">图片数据长度</param>
		/// <param name="scene">分享场景</param>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void wx_shareImageWithDatas(IntPtr datas, int length, int scene, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 请求权限
		/// </summary>
		/// <param name="onSuccess">成功回调</param>
		/// <param name="onCancel">取消回调</param>
		/// <param name="onError">错误回调</param>
		[DllImport("__Internal")]
		private static extern void wx_Auth(U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Cancel onCancel, U3DBridgeCallback_Error onError);

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