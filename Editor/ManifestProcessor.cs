// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ManifestProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/11 19:46:27
// *******************************************

namespace Bridge.AliApi
{
	using UnityEngine;
	using System.IO;
	using Editor;
	using UnityEditor;
	using UnityEditor.Callbacks;

	/// <summary>
	/// 
	/// </summary>
	internal static class ManifestProcessor
	{
		[PostProcessBuild(10001)]
		public static void OnPostprocessBuild(BuildTarget target, string projectPath)
		{
			CopyNativeCode(projectPath);
			Common.ManifestProcessor.ReplaceBuildDefinedCache[Common.ManifestProcessor.ALI_DEPENDENCIES] = "api 'com.alipay.sdk:alipaysdk-android:+@aar'";
		}
        
		private static void CopyNativeCode(string projectPath)
		{
			var sourcePath = ThirdSDKPackageManager.GetUnityPackagePath(ThirdSDKPackageManager.XhsApiPackageName);
			if (string.IsNullOrEmpty(sourcePath))
			{
				// 这个不是通过ump下载的包，查找工程内部文件夹
				sourcePath = "Assets/ThirdSDK/AliApi";
			}

			sourcePath += "/Plugins/Android";
			Debug.Log("remotePackagePath===" + sourcePath);
			string targetPath = Path.Combine(projectPath, Common.ManifestProcessor.NATIVE_CODE_DIR, "aliapi");
			Debug.Log("targetPath===" + targetPath);
			FileTool.DirectoryCopy(sourcePath + "/aliapi", targetPath);
		}
	}
}