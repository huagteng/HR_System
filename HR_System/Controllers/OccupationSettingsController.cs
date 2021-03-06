﻿using HR_System.Filters;
using HR_SystemBLL;
using HR_SystemIBLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    [LoginUserAuthorization]
    public class OccupationSettingsController : Controller
    {

        // 处理编辑职位类型请求
        [SystemManagerAuthorization]
        public ActionResult EditOccupationClass(string id)
        {

            IOccupationBLL bLL = new OccupationBLL();

            OccupationClass occupationClass = bLL.GetOccupationClassById(Convert.ToInt32(id));

            Models.OccupationClass occupationClassView = new Models.OccupationClass
            {
                Id = occupationClass.Id,
                Name = occupationClass.Name
            };

            ViewData["occupationClassView"] = occupationClassView;

            return View();
        }

        //处理保存职位类型请求
        [SystemManagerAuthorization]
        public ActionResult SaveOccupationClass(string OccpationClassId, string OccpationClassName)
        {
            IOccupationBLL bLL = new OccupationBLL();

            OccupationClass occupationClass = new OccupationClass { Id = Convert.ToInt32(OccpationClassId), Name = OccpationClassName };

            if (bLL.SaveOccupationClass(occupationClass))
            {
                TempData["info"] = "保存成功";
                return Redirect("/Settings/OccupationSettings");
            }
            else
            {
                TempData["error"] = "保存失败";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }
        }



        //处理编辑职位请求
        [SystemManagerAuthorization]
        public ActionResult EditOccupationName(string id)
        {

            IOccupationBLL bLL = new OccupationBLL();

            OccupationName occupationName = bLL.GetOccupationNameById(Convert.ToInt32(id));

            Models.OccupationName occupationNameView = new Models.OccupationName
            {
                Id = occupationName.Id,
                Name = occupationName.Name
            };

            OccupationClass occupationClass = bLL.GetOccupationClassById(occupationName.ClassId);

            Models.OccupationClass occupationClassView = new Models.OccupationClass
            {
                Id = occupationClass.Id,
                Name = occupationClass.Name
            };

            occupationNameView.OccupationClass = occupationClassView;

            ViewData["occupationNameView"] = occupationNameView;


            //装载所有的职位类型，用于职位类型选择的下拉框
            List<Models.OccupationClass> occupationClassList = new List<Models.OccupationClass>();

            foreach (var oc in bLL.GetAllOccupationClass())
            {
                Models.OccupationClass tempOccupationClass = new Models.OccupationClass
                {
                    Id = oc.Id,
                    Name = oc.Name
                };
                occupationClassList.Add(tempOccupationClass);
            }

            ViewData["occupationClassList"] = occupationClassList;

            return View();
        }

        //处理保存职位请求
        [SystemManagerAuthorization]
        public ActionResult SaveOccupationName(string OccpationNameId, string OccpationName, string classId, string standardId)
        {

            IOccupationBLL bLL = new OccupationBLL();

            ISalaryBLL salaryBLL = new SalaryBLL();

            OccupationName occupationName = new OccupationName { Id = Convert.ToInt32(OccpationNameId), Name = OccpationName, ClassId = Convert.ToInt32(classId) };

            if (bLL.SaveOccupationName(occupationName))
            {

                occupationName = bLL.GetOccupationNameByNameAndClass(OccpationName, Convert.ToInt32(classId));

                StandardMapOccupationName mapOcc = new StandardMapOccupationName { OccupationNameId = occupationName.Id, StandardId = Convert.ToInt32(standardId) };

                if (salaryBLL.SaveMapOcc(mapOcc))
                {
                    TempData["info"] = "保存成功";
                    return Redirect("/Settings/OccupationSettings");
                }
                else
                {
                    TempData["error"] = "保存失败";
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                }


                
            }
            else
            {
                TempData["error"] = "保存失败";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

        }


        //处理编辑职称请求
        [SystemManagerAuthorization]
        public ActionResult EditTechnicalTitle(string id)
        {

            IOccupationBLL bLL = new OccupationBLL();

            TechnicalTitle technicalTitle = bLL.GetTechnicalTitleById(Convert.ToInt32(id));

            Models.TechnicalTitle technicalTitleView = new Models.TechnicalTitle
            {
                Id = technicalTitle.Id,
                Name = technicalTitle.Name
            };

            ViewData["technicalTitleView"] = technicalTitleView;

            return View();

        }

        //处理保存职称请求
        [SystemManagerAuthorization]
        public ActionResult SaveTechnicalTitle(string TitleId, string TitleName)
        {

            IOccupationBLL bLL = new OccupationBLL();

            TechnicalTitle technicalTitle = new TechnicalTitle { Id = Convert.ToInt32(TitleId), Name = TitleName };

            if (bLL.SaveTechnicalTitle(technicalTitle))
            {
                TempData["info"] = "保存成功";
                return Redirect("/Settings/TitleSettings");
            }
            else
            {
                TempData["error"] = "保存失败";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

        }




        //删除职位
        [SystemManagerAuthorization]
        public ActionResult DeleteOccuptaionName(string id)
        {

            IOccupationBLL bLL = new OccupationBLL();

            if (bLL.DeleteOccupationNameById(Convert.ToInt32(id)))
            {
                TempData["info"] = "已删除";
                return Redirect("/Settings/OccupationSettings");
            }
            else
            {
                TempData["error"] = "删除失败";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

        }

        //删除职位类型
        [SystemManagerAuthorization]
        public ActionResult DeleteOccupationClass(string id)
        {
            IOccupationBLL bLL = new OccupationBLL();

            if (bLL.DeleteOccupationClassById(Convert.ToInt32(id)))
            {
                TempData["info"] = "已删除";
                return Redirect("/Settings/OccupationSettings");
            }
            else
            {
                TempData["error"] = "删除失败，该职位类型下有职位，无法直接删除";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

        }

        //删除职称
        [SystemManagerAuthorization]
        public ActionResult DeleteTechnicalTitle(string id)
        {

            IOccupationBLL bLL = new OccupationBLL();

            if (bLL.DeleteTechnicalTitle(Convert.ToInt32(id)))
            {
                TempData["info"] = "已删除";
                return Redirect("/Settings/TitleSettings");
            }
            else
            {
                TempData["error"] = "删除失败";
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }

        }



        //添加职位类型
        [SystemManagerAuthorization]
        public ActionResult AddOccupationClass()
        {
            return View();
        }

        //添加职位
        [SystemManagerAuthorization]
        public ActionResult AddOccupationName()
        {

            IOccupationBLL bLL = new OccupationBLL();

            ISalaryBLL salaryBLL = new SalaryBLL();

            //装载所有职位类型，作为下拉选择框
            List<Models.OccupationClass> occupationgClassList = new List<Models.OccupationClass>();

            foreach (var oc in bLL.GetAllOccupationClass())
            {
                Models.OccupationClass tempClass = new Models.OccupationClass
                {
                    Id = oc.Id,
                    Name = oc.Name
                };
                occupationgClassList.Add(tempClass);
            }

            ViewData["occupationClassList"] = occupationgClassList;


            //装载所有薪酬标准
            List<Models.SalaryStandard> standardList = new List<Models.SalaryStandard>();
            foreach (var s in salaryBLL.GetAllSalaryStandard())
            {
                Models.SalaryStandard tempS = new Models.SalaryStandard
                {
                    Id = s.Id,
                    StandardName = s.StandardName,
                    Total = s.Total
                };
                standardList.Add(tempS);
            }
            ViewData["standardList"] = standardList;


            return View();
        }

        [SystemManagerAuthorization]
        public ActionResult AddTechnicalTitle()
        {
            return View();
        }



        /// <summary>
        /// 处理根据职位类型id异步请求职位
        /// </summary>
        /// <param name="id">职位类型的id</param>
        /// <returns>返回json数据</returns>
        [StaffNormalAuthorization]
        public ActionResult DynamicOccName(string id)
        {
            IOccupationBLL bLL = new OccupationBLL();
            List<OccupationName> list = new List<OccupationName>();
            List<OccupationName>  tempList = bLL.GetAllOccNameByClassId(Convert.ToInt32(id));
            if (tempList != null)
            {
                list = tempList;
            }
            return Json(list);
        }

    }
}