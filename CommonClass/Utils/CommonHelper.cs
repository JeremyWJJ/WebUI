using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Mvc
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取header中的hospitalId和hinFacilityIdent
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static (string hospitalId, string hinFacilityIdent) GetHospitalId(this ControllerBase controllerBase)
        {
            StringValues _hospitalId = "";
            StringValues _hinFacilityIdent = "";
            // 验证医院信息是否完整
            controllerBase.HttpContext.Request.Headers.TryGetValue("__hospitalId", out _hospitalId);
            controllerBase.HttpContext.Request.Headers.TryGetValue("__hinCode", out _hinFacilityIdent);

            return (_hospitalId, _hinFacilityIdent);
        }
    }
}