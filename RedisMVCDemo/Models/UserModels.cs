using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RedisMVCDemo.Models
{
    /// <summary>
    /// 输入-数据模型
    /// </summary>
    public class UserModels
    {
        [Display(Name = "唯一标识")]
        public long Id { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }
    }

   
}