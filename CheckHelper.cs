using System.Text.RegularExpressions;

namespace JFLibrary
{
    /// <summary>
    /// 正则校验类
    /// Author:CJF
    /// Describe:使用正则表达式对校验字符串格式
    /// </summary>
    public static class CheckHelper
    {
        #region [判断电话号码]

        /// <summary>
        ///     判断电话号码格式是否正确
        ///     Author:蔡嘉福
        ///     利用正则表达式,判断电话号码格式是否正确
        /// </summary>
        /// <param name="MobilePhone">string类型字符串</param>
        /// <returns></returns>
        public static bool IsPhoneFormatTrue(this string MobilePhone)
        {
            if (string.IsNullOrEmpty(MobilePhone)) return false;
            var a = Regex.IsMatch(MobilePhone, @"^(0\d{2,3})-?(\d{7,8})$");
            var b = Regex.IsMatch(MobilePhone,
                @"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$");
            if (a || b)
                return true;
            return false;
        }

        #endregion

        #region [判断--车牌号]

        /// <summary>
        ///     判断车牌号格式是否正确
        ///     Author:蔡嘉福
        ///     利用正则表达式,判断车牌号格式是否正确
        /// </summary>
        /// <param name="CarNumber">string类型字符串</param>
        /// <returns></returns>
        public static bool IsCarNumberFormatTrue(this string CarNumber)
        {
            if (string.IsNullOrEmpty(CarNumber)) return false;
            return Regex.IsMatch(CarNumber,
                @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4,5}[A-Z0-9挂学警港澳]");
        }

        #endregion

        #region [判断--IP地址]

        /// <summary>
        ///     判断IP地址格式是否正确
        ///     Author:蔡嘉福
        ///     利用正则表达式,判断IP地址格式是否正确
        /// </summary>
        /// <param name="IPAdress">string类型字符串</param>
        /// <returns></returns>
        public static bool IsIPAdressFormatTrue(this string IPAdress)
        {
            if (string.IsNullOrEmpty(IPAdress)) return false;
            return Regex.IsMatch(IPAdress, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #endregion
    }
}