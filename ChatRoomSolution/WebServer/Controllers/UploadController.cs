using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult UploadChatFile()
        {

            UploadFileDTO model = new UploadFileDTO();
            HttpPostedFileBase file = Request.Files["uploadfile_ant"];

            if (file != null)
            {

                model.Catalog = DateTime.Now.ToString("yyyyMMdd");
                model.ImgIndex = Convert.ToInt32(Request.Form["imgIndex"]);
                string extensionName = System.IO.Path.GetExtension(file.FileName);
                model.FileName = Utils.GetRamCode() + extensionName;
                string filePathName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + ConfigurationManager.AppSettings["ImageAbsoluteFolderTemp"], model.Catalog);

                if (!System.IO.Directory.Exists(filePathName))
                {
                    System.IO.Directory.CreateDirectory(filePathName);
                }

                string relativeUrl = Request.RawUrl + ":" + Request.Url.Port + ConfigurationManager.AppSettings["ImageAbsoluteFolderTemp"];
                file.SaveAs(System.IO.Path.Combine(filePathName, model.FileName));
                string url = System.IO.Path.Combine(relativeUrl, model.Catalog, model.FileName).Replace("\\", "/");
                model.Url = url;

            }


            return Json(model);

        }
    }
}