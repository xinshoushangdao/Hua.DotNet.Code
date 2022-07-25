using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hua.DotNet.Code.Test.Ex.Model
{
    /// <summary>
    /// 性别
    /// </summary>
    [Description("性别")]
    public enum Gender
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Display(Name = "未知")] [Description("未知")]
        UnKnow,


        /// <summary>
        /// 男
        /// </summary>
        [Display(Name = "男")] [Description("男")]
        Man,


        /// <summary>
        /// 女
        /// </summary>
        [Display(Name = "女")] [Description("女")]
        WoMan,
    }
}