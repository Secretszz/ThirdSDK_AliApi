//
//  AlipayApiManager.m
//  Unity-iPhone
//
//  Created by 晴天网络 on 2025/2/11.
//

@implementation AlipayApiManager
+(instancetype)sharedManager {
    static dispatch_once_t onceToken;
    static AlipayApiManager *instance;
    dispatch_once(&onceToken, ^{
        instance = [[AlipayApiManager alloc] init];
    });
    return instance;
}

// 初始化
-(void) init{
}

-(void) doPay:(NSString *)appScheme
  OrderString:(NSString *)orderString{
    [[AlipaySDK defaultService] payOrder:orderString fromScheme:appScheme callback:^(NSDictionary *resultDic) {
                NSLog(@"reslut = %@",resultDic);
    }];
}

-(void) doAuth:(NSString *)appScheme
    AuthString:(NSString *)authString{
    [[AlipaySDK defaultService] auth_V2WithInfo:authInfoStr
                                     fromScheme:appScheme
                                       callback:^(NSDictionary *resultDic) {
                                           NSLog(@"result = %@",resultDic);
                                           // 解析 auth code
                                           NSString *result = resultDic[@"result"];
                                           NSString *authCode = nil;
                                           if (result.length>0) {
                                               NSArray *resultArr = [result componentsSeparatedByString:@"&"];
                                               for (NSString *subResult in resultArr) {
                                                   if (subResult.length > 10 && [subResult hasPrefix:@"auth_code="]) {
                                                       authCode = [subResult substringFromIndex:10];
                                                       break;
                                                   }
                                               }
                                           }
                                           NSLog(@"授权结果 authCode = %@", authCode?:@"");
                                       }];
}
    
-(BOOL)application:(UIApplication *)application
            openURL:(NSURL *)url
  sourceApplication:(NSString *)sourceApplication
         annotation:(id)annotation {
    if ([url.host isEqualToString:@"safepay"]) {
        //跳转支付宝客户端进行支付，处理支付结果
        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {
            NSLog(@"result = %@",resultDic);
        }];
    }
    return YES;
}

-(BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString*, id> *)options
{
    if ([url.host isEqualToString:@"safepay"]) {
        //跳转支付宝客户端进行支付，处理支付结果
        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {
            NSLog(@"result = %@",resultDic);
        }];
    }
    return YES;
}

@end