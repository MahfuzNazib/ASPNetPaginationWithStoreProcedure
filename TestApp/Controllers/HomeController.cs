using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestApp.Models;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult GetAllData(int page = 1, int per_page = 10)
        {
            string connectionString = "Server=HBIZ-BD-LP007;Database=V91_NEONE2_QA_2_Base;User ID=V91_NEONE2_QA_2;Password=V91_NEONE2_QA_2;TrustServerCertificate=True;";

            string sql = "EXEC SP_LV_COMPANY_LEAVE_INFO_EMP '2021-01-01','2023-12-12','001995#000016', NULL, 'EMP_NUMBER ASC', @page, @per_page";

            DataBindModel responseModel = new DataBindModel();

            List<TestModel> data = new List<TestModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@page", page);
                    command.Parameters.AddWithValue("@per_page", per_page);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TestModel item = new TestModel
                            {
                                LEAVE_DAYS = reader["LEAVE_DAYS"].ToString(),
                                LEAVE_TYPE_NAME = reader["LEAVE_TYPE_NAME"].ToString(),
                                LEAVE_COMMENTS = reader["LEAVE_COMMENTS"].ToString(),
                                LEAVE_STATUS = reader["LEAVE_STATUS"].ToString(),
                                TOTAL_COUNT = Convert.ToInt32(reader["TOTAL_COUNT"])

                                // Set properties for other columns as needed
                            };
                            data.Add(item);
                        }
                    }
                }
            }
            int totalCount = data.FirstOrDefault()?.TOTAL_COUNT ?? 0;
            decimal last_page = totalCount / per_page;
            #region PageSummary
            PageSummary pageSummary = new PageSummary
            {
                page = page,
                per_page = per_page,
                first_page = 1,
                last_page = (int)Math.Ceiling(last_page),
                RedirectUrlMethodName = MethodBase.GetCurrentMethod().Name
            };

            #endregion
            responseModel.data = data;
            responseModel.pageSummary = pageSummary;

            return View(responseModel);
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
    }
}