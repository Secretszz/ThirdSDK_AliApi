// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IOSProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/26 18:23:11
// *******************************************

#if UNITY_IOS
namespace Bridge.AliApi
{
	using System.Text;
	using System.IO;
	using UnityEditor;
	using UnityEditor.Callbacks;
	using UnityEditor.iOS.Xcode;
	using Common;

	/// <summary>
	/// 
	/// </summary>
	internal static class IOSProcessor
	{
		[PostProcessBuild(10002)]
		public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
		{
			if (target == BuildTarget.iOS)
			{
				ThirdSDKSettings instance = ThirdSDKSettings.Instance;
				var projPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
				var proj = new PBXProject();
				proj.ReadFromFile(projPath);
#if UNITY_2019_3_OR_NEWER
				var targetGUID = proj.GetUnityFrameworkTargetGuid();
#else
				var targetGUID = proj.TargetGuidByName("Unity-iPhone");
#endif

				proj.AddFrameworkToProjectEx(targetGUID, "Security.framework", false);
				proj.AddFrameworkToProjectEx(targetGUID, "CoreGraphics.framework", false);
				proj.AddFrameworkToProjectEx(targetGUID, "WebKit.framework", false);

				proj.WriteToFile(projPath);

				var plistPath = Path.Combine(pathToBuildProject, "Info.plist");
				var plist = new PlistDocument();
				plist.ReadFromFile(plistPath);
				var rootDic = plist.root;

				var items = new[]
				{
						"weixin",
						"weixinULAPI",
						"weixinURLParamsAPI"
				};

				rootDic.AddApplicationQueriesSchemes(items);

				var array = rootDic.GetElementArray("CFBundleURLTypes");
				array.AddCFBundleURLTypes("Editor", "weixin", new[] { instance.WxAppId });
				plist.WriteToFile(plistPath);

				var sourcePath = ThirdSDKPackageManager.GetUnityPackagePath(PackageType.WeChat);
				string ApiPath;
				if (string.IsNullOrEmpty(sourcePath))
				{
					ApiPath = "Libraries/ThirdSDK/WxApi/Plugins/iOS/WeChatSDKManager.mm";
				}
				else
				{
					ApiPath = "Libraries/com.bridge.wxapi/Plugins/iOS/WeChatSDKManager.mm";
				}
				var objectiveCFilePath = Path.Combine(pathToBuildProject, ApiPath);
				StringBuilder objectiveCCode = new StringBuilder(File.ReadAllText(objectiveCFilePath));
				objectiveCCode.Replace("**APPID**", instance.WxAppId);
				objectiveCCode.Replace("**UNILINK**", instance.UniversalLink);
				// 将修改后的 Objective-C 代码写回文件中
				File.WriteAllText(objectiveCFilePath, objectiveCCode.ToString());
			}
		}
	}
}
#endif
