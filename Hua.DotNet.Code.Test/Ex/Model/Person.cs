using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hua.DotNet.Code.Test.Ex.Model
{
    /// <summary>
    /// 人 实体
    /// </summary>
    [Description("人")]
    public class Person
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Display(Name = "唯一标识")]
        [Description("唯一标识")]
        public long PersonId { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Description("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "电话")]
        [Description("电话")]
        public string Tel;

    }
}