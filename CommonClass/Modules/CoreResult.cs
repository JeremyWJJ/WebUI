using Newtonsoft.Json;

namespace SpdCommon.Modules
{
    public enum APIResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 未知的错误
        /// </summary>
        UnknownError = 10000,

        /// <summary>
        /// 参数错误
        /// </summary>
        ArgumentError = 10001,

        /// <summary>
        /// 数据库执行错误
        /// </summary>
        DatabaseError = 10002,

        /// <summary>
        /// 内部接口逻辑错误
        /// </summary>
        LogicError = 10003,

        /// <summary>
        /// 数据验证没通过
        /// </summary>
        ValidateError = 10004,

        /// <summary>
        /// 权限错误
        /// </summary>
        AuthError = 10005,

        /// <summary>
        /// 查询结果为空
        /// </summary>
        EmptyResult = 10006,

        /// <summary>
        /// 微信未授权，需要授权
        /// </summary>
        WechatAuthError = 10008,

        /// <summary>
        /// 其他错误
        /// </summary>
        OtherError = 10007
    }

    public class CoreResult
    {
        /// <summary>
        /// 返回值编码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; } = 0;

        /// <summary>
        /// 返回结果是否成功
        /// <list type="bullet">
        /// <item>
        /// <term>0</term>
        /// <description>失败</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>成功</description>
        /// </item>
        /// </list>
        /// </summary>
        [JsonProperty("result")]
        public int Result { get; set; } = 1;

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        public CoreResult()
        {
            Code = 0;
            Result = 1;
        }

        public CoreResult(APIResultCode code, string message)
        {
            Code = (int)code;
            switch (code)
            {
                case APIResultCode.Success:
                    Result = 1;
                    break;

                case APIResultCode.UnknownError:
                case APIResultCode.ArgumentError:
                case APIResultCode.DatabaseError:
                case APIResultCode.LogicError:
                case APIResultCode.ValidateError:
                case APIResultCode.AuthError:
                case APIResultCode.OtherError:
                case APIResultCode.WechatAuthError:
                    Result = 0;
                    Message = message;
                    break;

                case APIResultCode.EmptyResult:
                    Result = 1;
                    Message = message;
                    break;

                default:
                    Result = 1;
                    break;
            }
        }

        public CoreResult(object data)
        {
            Code = 0;
            Result = 1;
            Data = data;
        }
    }
}