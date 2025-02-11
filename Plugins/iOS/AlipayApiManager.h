//
//  AlipayApiManager.h
//  UnityFramework
//
//  Created by 晴天网络 on 2025/2/11.
//

#import "WXApiManager.h"
#import "WXApi.h"
#import "CommonApi.h"

@interface AlipayApiManager : NSObject

@property (nonatomic, assign) U3DBridgeCallback_Success onSuccess;
@property (nonatomic, assign) U3DBridgeCallback_Cancel onCancel;
@property (nonatomic, assign) U3DBridgeCallback_Error onError;

+(instancetype)sharedManager;

// 初始化
-(void) init;

-(void) doPay:(NSString *)appScheme
  OrderString:(NSString *)orderString;

-(void) doAuth:(NSString *)appScheme
    AuthString:(NSString *)authString;

-(BOOL)application:(UIApplication *)application
           openURL:(NSURL *)url
 sourceApplication:(NSString *)sourceApplication
        annotation:(id)annotation;

-(BOOL)application:(UIApplication *)app
           openURL:(NSURL *)url
           options:(NSDictionary<NSString*, id> *)options;
@end
