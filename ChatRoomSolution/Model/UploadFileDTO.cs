using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UploadFileDTO
    {

        /// <summary>
            /// 目录名称
            /// </summary>
        public string Catalog { set; get; }
        /// <summary>
            /// 文件名称，包括扩展名
            /// </summary>
        public string FileName { set; get; }
        /// <summary>
            /// 浏览路径
            /// </summary>
        public string Url { set; get; }
        /// <summary>
            /// 上传的图片编号(提供给前端判断图片是否全部上传完)
            /// </summary>
        public int ImgIndex { get; set; }

    }
}
