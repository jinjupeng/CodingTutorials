using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCSample.Common;
using MVCSample.Context;
using MVCSample.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MVCSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly MVCContext _context;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, MVCContext context)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Import(IFormFile excelfile)
        {
            string sWebRootFolder = _webHostEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (FileStream fileStream = new FileStream(file.ToString(), FileMode.Create))
                {
                    excelfile.CopyTo(fileStream);
                    fileStream.Flush();
                }
                Stream fs = excelfile.OpenReadStream();
                //此处泛型为需要隐射的实体类
                Import2Excel<FundModel> it = new Import2Excel<FundModel>();
                //这里自定义将excel的表头或列名字映射到对应实体
                it.ForMember("日期", e => e.CurrentDateTime);
                it.ForMember("上证指数", e => e.ShanghaiCompositeIndex);
                it.ForMember("平安银行(000001)", e => e.PinganBank);
                it.ForMember("贵州茅台(600519)", e => e.MoutaiChina);
                it.ForMember("中信建投(601066)", e => e.CiciCitic);
                it.ForMember("华兴源创(688001)", e => e.SuzhouHYCTechnology);
                it.ForMember("同达创业(600647)", e => e.ShanghaiTongdaVentureCapitalCo);
                //通过此方法直接将流文件加载到内存
                IList<FundModel> list = it.LoadFromExcel(fs);
                _context.AddRangeAsync(list);
                var count = _context.SaveChangesAsync();
                return Content("上传成功，请返回到上一页进行相对收益计算！");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Compute()
        {
            var fundDto = new FundDto
            {
                StartDate = Convert.ToDateTime(Request.Form["startdate"]),
                EndDate = Convert.ToDateTime(Request.Form["enddate"])
            };
            var fundsList = _context.Set<FundModel>().Where(a => a.CurrentDateTime >= fundDto.StartDate && a.CurrentDateTime <= fundDto.EndDate).ToList();
            //var fundDict = new Dictionary<DateTime, FundModel>();
            // 注意：查询起始时间的累计涨跌幅 = 1
            for (var i = 0; i < fundsList.Count; i++)
            {
                var fundModel = fundsList[i];
                if(i == 0)
                {
                    // 上证指数涨跌幅
                    fundModel.ShanghaiCompositeIndexCompute = 0;
                    // 平安银行相对收益
                    fundModel.PinganBankRelativeGain = 1;
                    // 贵州茅台相对收益
                    fundModel.MoutaiChinaRelativeGain = 1;
                    // 中信建设相对收益
                    fundModel.CiciCiticRelativeGain = 1;
                    // 华兴源创相对收益
                    fundModel.SuzhouHYCTechnologyRelativeGain = 1;
                    // 同达创业相对收益
                    fundModel.ShanghaiTongdaVentureCapitalCoRelativeGain = 1;
                    fundModel.ShanghaiCompositeIndex = Math.Round(fundModel.ShanghaiCompositeIndex, 2);

                }
                else
                {
                    var lastFundModel = fundsList[i - 1];
                    // 上证指数涨跌幅
                    fundModel.ShanghaiCompositeIndexCompute = fundModel.ShanghaiCompositeIndex / lastFundModel.ShanghaiCompositeIndex - 1;
                    if(fundModel.PinganBank == 0 || lastFundModel.PinganBank == 0)
                    {
                        fundModel.PinganBankRelativeGain = 1;
                    }
                    else
                    {
                        fundModel.PinganBankRelativeGain = (((fundModel.PinganBank / lastFundModel.PinganBank - 1) - fundModel.ShanghaiCompositeIndexCompute) + 1) * lastFundModel.PinganBankRelativeGain;

                    }
                    if(fundModel.MoutaiChina == 0 || lastFundModel.MoutaiChina == 0)
                    {
                        fundModel.MoutaiChinaRelativeGain = 1;
                    }
                    else
                    {

                        fundModel.MoutaiChinaRelativeGain = (((fundModel.MoutaiChina / lastFundModel.MoutaiChina - 1) - fundModel.ShanghaiCompositeIndexCompute) + 1) * lastFundModel.MoutaiChinaRelativeGain;

                    }
                    if(fundModel.CiciCitic == 0 || lastFundModel.CiciCitic == 0)
                    {
                        fundModel.CiciCiticRelativeGain = 1;
                    }
                    else
                    {

                        fundModel.CiciCiticRelativeGain = (((fundModel.CiciCitic / lastFundModel.CiciCitic - 1) - fundModel.ShanghaiCompositeIndexCompute) + 1) * lastFundModel.CiciCiticRelativeGain;

                    }
                    if(fundModel.SuzhouHYCTechnology == 0 || lastFundModel.SuzhouHYCTechnology == 0)
                    {
                        fundModel.SuzhouHYCTechnologyRelativeGain = 1;
                    }
                    else
                    {

                        fundModel.SuzhouHYCTechnologyRelativeGain = (((fundModel.SuzhouHYCTechnology / lastFundModel.SuzhouHYCTechnology - 1) - fundModel.ShanghaiCompositeIndexCompute) + 1) * lastFundModel.SuzhouHYCTechnologyRelativeGain;

                    }
                    if (fundModel.ShanghaiTongdaVentureCapitalCo == 0 || lastFundModel.ShanghaiTongdaVentureCapitalCo == 0)
                    {
                        fundModel.ShanghaiTongdaVentureCapitalCoRelativeGain = 1;
                    }
                    else
                    {

                        fundModel.ShanghaiTongdaVentureCapitalCoRelativeGain = (((fundModel.ShanghaiTongdaVentureCapitalCo / lastFundModel.ShanghaiTongdaVentureCapitalCo - 1) - fundModel.ShanghaiCompositeIndexCompute) + 1) * lastFundModel.ShanghaiTongdaVentureCapitalCoRelativeGain;

                    }
                    fundModel.ShanghaiCompositeIndex = Math.Round(fundModel.ShanghaiCompositeIndex, 2);
                    fundModel.ShanghaiCompositeIndexCompute = Math.Round(fundModel.ShanghaiCompositeIndexCompute * 100, 2);
                    fundModel.PinganBankRelativeGain = Math.Round(fundModel.PinganBankRelativeGain, 2);
                    fundModel.MoutaiChinaRelativeGain = Math.Round(fundModel.MoutaiChinaRelativeGain, 2);
                    fundModel.CiciCiticRelativeGain = Math.Round(fundModel.CiciCiticRelativeGain, 2);
                    fundModel.SuzhouHYCTechnologyRelativeGain = Math.Round(fundModel.SuzhouHYCTechnologyRelativeGain, 2);
                    fundModel.ShanghaiTongdaVentureCapitalCoRelativeGain = Math.Round(fundModel.ShanghaiTongdaVentureCapitalCoRelativeGain, 2);
                }
            }
            // 计算单日上证指数涨跌幅 = 当日上证指数 / 上日上证指数 - 1


            // 计算单日贵州茅台单日涨跌幅 = 当日股票价 / 上日股票价 - 1

            // 计算相对收益 = 累计涨跌幅 = ((股票单日涨跌幅-上证指数涨跌幅) + 1) * 累计涨跌幅

            return View(fundsList);
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <returns></returns>
        public IActionResult Export()
        {
            string sWebRootFolder = _webHostEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("aspnetcore");
                //添加头
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Url";
                //添加值
                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "111";
                worksheet.Cells["C2"].Value = "http://1111";

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "222";
                worksheet.Cells["C3"].Value = "http://222";
                worksheet.Cells["C3"].Style.Font.Bold = true;

                package.Save();
            }
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
