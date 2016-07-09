using GeetestSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Geetest.Controllers
{
    /// <summary>
    ///  add by smallant QQ技术群：小蚂蚁技术联盟 247844859
    /// </summary>
    public class GeetestController : Controller
    {
        // GET: Geetest
        public ActionResult Index()
        {
            string result = getCaptcha();

            return Content(result, "application/json");
        }

        private string getCaptcha()
        {
            try
            {
                GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
                Byte gtServerStatus = geetest.preProcess();
                Session[GeetestLib.gtServerStatusSessionKey] = gtServerStatus;
                return geetest.getResponseStr();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public JsonResult CheckGeeTestResult()
        {
            GeetestLib geetest = new GeetestLib("b46d1900d0a894591916ea94ea91bd2c", "36fc3fe98530eea08dfc6ce76e3d24c4");
            Byte gt_server_status_code = (Byte)Session[GeetestLib.gtServerStatusSessionKey];
            String userID = (String)Session["userID"];
            var stre = HttpContext.Request.InputStream;
            var jsonstr = new StreamReader(stre).ReadToEnd();
            var geetest_challenge = JSONHelper.JsonToString(jsonstr, "geetest_challenge");
            var geetest_validate = JSONHelper.JsonToString(jsonstr, "geetest_validate");
            var geetest_seccode = JSONHelper.JsonToString(jsonstr, "geetest_seccode");
            int result = 0;
            String challenge = geetest_challenge;
            String validate = geetest_validate;
            String seccode = geetest_seccode;
            if (gt_server_status_code == 1)
            {
                result = geetest.enhencedValidateRequest(challenge, validate, seccode, userID);
            }
            else
            {
                result = geetest.failbackValidateRequest(challenge, validate, seccode);
            }
            if (result != 1)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}