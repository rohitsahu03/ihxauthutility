using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AuthUtility.Controllers
{
    [AuthorizationFilter("9227")]
    public class BulkUpdateController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBulkUpdateProvider _bulkUpdateProvider;
        private readonly ConfigManager _configManager;
        private readonly IDBProvider _dbProvider;
        public BulkUpdateController(IHostingEnvironment hostingEnvironment, ILogger<BulkUpdateController> logger, IBulkUpdateProvider bulkUpdateProvider,
                                    ConfigManager configManager, IDBProvider dbProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _bulkUpdateProvider = bulkUpdateProvider;
            _configManager = configManager;
            _dbProvider = dbProvider;
        }
        public IActionResult UserDetails(string Message)
        {
            return View("UserNames");
        }

        public IActionResult DownloadSampleFile()
        {
            List<string> columnNames = new List<string> { UtilityConstants.UserId, UtilityConstants.OldUserName, UtilityConstants.NewUserName, UtilityConstants.EmployeeId, UtilityConstants.EntityId };
            string fileName = "Sample.xlsx";
            return File(System.IO.File.ReadAllBytes(DownloadFile(columnNames, fileName)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult DownloadSampleBulkUserCreation()
        {
            List<string> columnNames = typeof(UserCreationConstants).GetAllPublicConstantValues<string>();
            string fileName = "Sample_Bulk_User_Creation.xlsx";
            return File(System.IO.File.ReadAllBytes(DownloadFile(columnNames, fileName)), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        public BaseResponse UpdateUserDetails()
        {
            List<string> allowedHeaders = new List<string> { UtilityConstants.UserId, UtilityConstants.OldUserName, UtilityConstants.NewUserName, UtilityConstants.EmployeeId, UtilityConstants.EntityId };
            try
            {
                var data = GetDetailsFromSheet(Request.Form.Files[0], allowedHeaders);
                if (!string.IsNullOrEmpty(data.Message))
                    return new BaseResponse() { Message = data.Message };

                List<UserDetails> dataObj = data.Dt.AsEnumerable()
                                               .Where(x => !x.ItemArray.All(y => string.IsNullOrWhiteSpace(y.ToString())))
                                               .Select(row =>
                                                new UserDetails
                                                {
                                                    UserId = !string.IsNullOrEmpty(row.Field<string>(UtilityConstants.UserId)) ? int.Parse(row.Field<string>(UtilityConstants.UserId)) : default(int),
                                                    OldUserName = Helper.Instance.GetTrimmedValue(row.Field<string>(UtilityConstants.OldUserName)),
                                                    NewUserName = Helper.Instance.GetTrimmedValue(row.Field<string>(UtilityConstants.NewUserName)),
                                                    EmployeeId = Helper.Instance.GetTrimmedValue(row.Field<string>(UtilityConstants.EmployeeId)),
                                                    EntityId = !string.IsNullOrEmpty(row.Field<string>(UtilityConstants.EntityId)) ? int.Parse(row.Field<string>(UtilityConstants.EntityId)) : default(int)
                                                }).ToList();

                var validDataResponse = ValidateDataForUpdation(dataObj);
                if (validDataResponse.IsNotNull() && !validDataResponse.IsSuccess)
                    return validDataResponse;

                UserDetailsBulkUpdate userDetailsToUpdate = new UserDetailsBulkUpdate()
                {
                    UserDetails = dataObj,
                    ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext)
                };

                return _bulkUpdateProvider.UpdateBulkUserDetails(userDetailsToUpdate);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in UpdateUserDetails :" + Ex.Message + Ex.StackTrace);

                return new BaseResponse() { Message = MsgConstants.ErrorProcessingData + "|" + Ex.Message };
            }
        }

        public IActionResult RetailUserCreation()
        {
            BulkUserCreation viewResponse = new BulkUserCreation { AllRoles = _dbProvider.GetAllRoles(true) };
            return View("BulkUserCreation", viewResponse);
        }

        [HttpPost]
        public BaseResponse BulkUserCreation()
        {
            try
            {
                List<string> allowedHeaders = typeof(UserCreationConstants).GetAllPublicConstantValues<string>();
                var roles = Request.Form["Roles"];
                bool isVerifyContact = Convert.ToBoolean(Request.Form["VerifyContact"].ToString());
                var data = GetDetailsFromSheet(Request.Form.Files[0], allowedHeaders);
                if (!string.IsNullOrEmpty(data.Message))
                    return new BaseResponse() { Message = data.Message };

                List<UserCreation> dataObj = data.Dt.AsEnumerable()
                                     .Where(x => !x.ItemArray.All(y => string.IsNullOrWhiteSpace(y.ToString())))
                                     .Select(row =>
                                        new UserCreation
                                        {
                                            ApplicationId = UtilityConstants.AuthUtilityAppId,
                                            FirstName = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.FirstName)),
                                            LastName = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.LastName)),
                                            Email = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.Email)),
                                            PhoneNumber = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.PhoneNumber)),
                                            Gender = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.Gender)),
                                            UserName = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.UserName)),
                                            Password = Helper.Instance.GetTrimmedValue(row.Field<string>(UserCreationConstants.Password)),
                                            ProviderMasterEntityId = !string.IsNullOrEmpty(row.Field<string>(UserCreationConstants.EntityId)) ? Convert.ToInt32(row.Field<string>(UserCreationConstants.EntityId)) : default,
                                            DOB = Helper.Instance.ParseStringToDateTime(row.Field<string>(UserCreationConstants.DOB)),
                                            IgnoreAliasConfig = true,
                                            UserRoles = roles.IsNotNull() ? roles.ToString().Split(",").Select(x => Convert.ToInt32(x)).ToList() : default
                                        }).ToList();

                BaseResponse isDataValid = ValidateBulkCreationData(dataObj);
                if (isDataValid.IsNotNull() && !isDataValid.IsSuccess)
                    return isDataValid;

                return _bulkUpdateProvider.UserBulkCreation(dataObj, isVerifyContact, Helper.Instance.GetActionPerformedBy(HttpContext));
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in UpdateUserDetails :" + Ex.Message + Ex.StackTrace);

                return new BaseResponse() { Message = MsgConstants.ErrorProcessingData + "|" + Ex.Message };
            }
        }

        public IActionResult RemoveElasticBaseProfile(string Message)
        {
            if (string.IsNullOrEmpty(Message))
                return View("RemoveElasticBaseProfile");

            return View("RemoveElasticBaseProfile", new RemoveElasticBaseProfile() { Message = Message });
        }

        [HttpPost]
        public IActionResult RemoveElasticBaseProfile(RemoveElasticBaseProfile Model)
        {
            try
            {
                BaseResponse result = new BaseResponse();
                if (ModelState.IsValid)
                {
                    if (Model.IsNull() || !Model.IsValid())
                        return RedirectToAction("RemoveElasticBaseProfile", "BulkUpdate", new { @message = MsgConstants.InvalidData });

                    Model.ActionPerformedBy = Helper.Instance.GetActionPerformedBy(HttpContext);
                    if (_logger != null)
                        _logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Elastic base profile removal request of entityid : " + Model.EntityId + " by : " + Model.ActionPerformedBy);

                    result = _bulkUpdateProvider.RemoveElasticBaseProfile(Model);
                }

                if (!result.IsSuccess)
                {
                    Model.Message = string.IsNullOrEmpty(result.Message) ? MsgConstants.ErrorMsg : result.Message;
                    return View("RemoveElasticBaseProfile", Model);
                }

                return RedirectToAction("RemoveElasticBaseProfile", "BulkUpdate", new { @message = result.Message });

            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in RemoveElasticBaseProfile :" + ex.Message + ex.StackTrace);

                return RedirectToAction("RemoveElasticBaseProfile", "BulkUpdate", new { @message = ex.Message });
            }
        }

        private BaseResponse ValidateBulkCreationData(List<UserCreation> DataObj)
        {
            if (!DataObj.HasRecords())
                return new BaseResponse() { Message = MsgConstants.NoDataFound };

            List<UserCreation> incompleteData = DataObj.Where(x => !x.IsValid()).ToList();

            if (incompleteData.HasRecords())
            {
                List<int> incompleteDataIndexes = new List<int>();
                incompleteData.ForEach(x =>
                {
                    incompleteDataIndexes.Add(DataObj.IndexOf(x) + 2);
                });
                return new BaseResponse() { Message = "Incomplete data in row numbers => " + string.Join(",", incompleteDataIndexes) };
            }

            List<string> duplicateValues = new List<string>();
            duplicateValues.AddRange(DataObj.GroupBy(x => x.UserName).Where(c => c.Count() > 1).Select(x => x.Key).ToList());
            duplicateValues.AddRange(DataObj.Where(x => !string.IsNullOrWhiteSpace(x.Email)).GroupBy(x => x.Email).Where(c => c.Count() > 1).Select(x => x.Key).ToList());
            duplicateValues.AddRange(DataObj.Where(x => !string.IsNullOrWhiteSpace(x.PhoneNumber)).GroupBy(x => x.PhoneNumber).Where(c => c.Count() > 1).Select(x => x.Key).ToList());

            if (duplicateValues.HasRecords())
                return new BaseResponse() { Message = string.Format(MsgConstants.DuplicateData, string.Join(",", duplicateValues)) };

            return new BaseResponse() { IsSuccess = true };
        }

        private string DownloadFile(List<string> ColumnNames, string FileName)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            // string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            string filePath = Path.Combine(sWebRootFolder, FileName);
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    var memory = new MemoryStream();
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {

                        IWorkbook workbook;
                        workbook = new XSSFWorkbook();
                        ISheet excelSheet = workbook.CreateSheet("UserDetails");
                        IRow row = excelSheet.CreateRow(0);

                        // create font style
                        IFont myFont = workbook.CreateFont();
                        myFont.FontHeightInPoints = (short)10;
                        myFont.FontName = "Garamond";
                        myFont.Boldweight = (short)FontBoldWeight.Bold;

                        // create bordered cell style
                        ICellStyle borderedCellStyle = workbook.CreateCellStyle();
                        borderedCellStyle.SetFont(myFont);

                        for (int i = 0; i < ColumnNames.Count; i++)
                        {
                            ICell cell = row.CreateCell(i);
                            cell.SetCellValue(ColumnNames[i]);
                            cell.CellStyle = borderedCellStyle;
                            excelSheet.AutoSizeColumn(i);
                        }
                        workbook.Write(fs);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in DownloadSampleFile :" + Ex.Message + Ex.StackTrace);
            }
            return filePath;
        }

        private BaseResponse ValidateDataForUpdation(List<UserDetails> DataObj)
        {
            if (!DataObj.HasRecords())
                return new BaseResponse() { Message = MsgConstants.NoDataFound };

            if (DataObj.Any(x => string.IsNullOrEmpty(x.OldUserName) && string.IsNullOrEmpty(x.NewUserName) && x.UserId == 0)
               || DataObj.Any(x => (string.IsNullOrEmpty(x.NewUserName) && !string.IsNullOrEmpty(x.OldUserName)) || (!string.IsNullOrEmpty(x.NewUserName) && string.IsNullOrEmpty(x.OldUserName))))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            List<int> duplicateUserIds = DataObj
                                        .Where(x => x.UserId != 0)
                                        .GroupBy(x => x.UserId)
                                        .Where(x => x.Count() > 1)
                                        .Select(x => x.Key)
                                        .ToList();

            if (duplicateUserIds.HasRecords())
                return new BaseResponse() { Message = string.Format(MsgConstants.DuplicateData, string.Join(",", duplicateUserIds)) };

            List<string> sameOldAndNewUserNames = DataObj
                                                 .Where(x => !string.IsNullOrEmpty(x.NewUserName) && !string.IsNullOrEmpty(x.OldUserName))
                                                 .Where(x => x.OldUserName.ToLower().Trim() == x.NewUserName.ToLower().Trim()).Select(x => x.OldUserName).ToList();

            if (sameOldAndNewUserNames.HasRecords())
                return new BaseResponse() { Message = string.Format(MsgConstants.SameOldAndNewUserName, string.Join(",", sameOldAndNewUserNames)) };

            List<string> duplicateOldUserNames = DataObj
                                                .Where(x => !string.IsNullOrEmpty(x.NewUserName) && !string.IsNullOrEmpty(x.OldUserName))
                                                .GroupBy(x => x.OldUserName)
                                                .Where(x => x.Count() > 1)
                                                .Select(x => x.Key)
                                                .ToList();

            List<string> duplicateNewUserNames = DataObj
                                                .Where(x => !string.IsNullOrEmpty(x.NewUserName) && !string.IsNullOrEmpty(x.OldUserName))
                                                .GroupBy(x => x.NewUserName)
                                                .Where(x => x.Count() > 1)
                                                .Select(x => x.Key)
                                                .ToList();

            if (duplicateOldUserNames.HasRecords() || duplicateNewUserNames.HasRecords())
            {
                duplicateOldUserNames.AddRange(duplicateNewUserNames);
                return new BaseResponse() { Message = string.Format(MsgConstants.DuplicateData, string.Join(",", duplicateOldUserNames)) };
            }

            return new BaseResponse() { IsSuccess = true };
        }

        private (DataTable Dt, string Message) GetDetailsFromSheet(IFormFile File, List<string> AllowedHeaders)
        {
            try
            {
                if (File.Length > 0)
                {
                    //string folderName = "Upload";
                    //string webRootPath = _hostingEnvironment.WebRootPath;
                    //string newPath = Path.Combine(webRootPath, folderName);

                    //if (!Directory.Exists(newPath))
                    //    Directory.CreateDirectory(newPath);
                    //else
                    //    Array.ForEach(Directory.GetFiles(newPath), System.IO.File.Delete);

                    string sFileExtension = Path.GetExtension(File.FileName).ToLower();
                    //string fullPath = Path.Combine(newPath, File.FileName);

                    using (var stream = File.OpenReadStream())
                    {
                        //File.CopyTo(stream);
                        //stream.Position = 0;
                        ISheet sheet = GetSheetFromExcelType(sFileExtension, stream);
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        List<string> presentHeaders = new List<string>();
                        headerRow.Cells.ForEach(x =>
                        {
                            string header = x.ToString();
                            if (!string.IsNullOrEmpty(header))
                                presentHeaders.Add(header);
                        });

                        if (presentHeaders.Except(AllowedHeaders).Any() || sheet.LastRowNum == sheet.FirstRowNum)
                            return (null, MsgConstants.ErrorInExcel);

                        return (GetDatatableFromSheet(sheet), null);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in GetDataFromSheet :" + Ex.Message + Ex.StackTrace);

                return (null, Ex.Message);
            }
            return (null, MsgConstants.ErrorInReadingFile);
        }

        private ISheet GetSheetFromExcelType(string FileExtension, Stream Stream)
        {
            ISheet sheet;
            if (FileExtension == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(Stream); //This will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(Stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
            }
            return sheet;
        }

        private DataTable GetDatatableFromSheet(ISheet sheet)
        {
            if (sheet == null)
                return null;

            DataTable data = new DataTable();
            int startRow = 0;
            //bool isFirstRowColumn = true;
            try
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum;

                //if (isFirstRowColumn)
                //{
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    ICell cell = firstRow.GetCell(i);
                    if (cell != null)
                    {
                        string cellValue = cell.StringCellValue;
                        if (cellValue != null)
                        {
                            DataColumn column = new DataColumn(cellValue);
                            data.Columns.Add(column);
                        }
                    }
                }
                startRow = sheet.FirstRowNum + 1;
                //}
                //else
                //{
                //    startRow = sheet.FirstRowNum;
                //}

                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }

                return data;
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in GetDatatableFromSheet :" + Ex.Message + Ex.StackTrace);

                return null;
            }
        }
    }
}