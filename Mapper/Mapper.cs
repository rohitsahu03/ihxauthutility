using AuthUtility.Constants;
using AuthUtility.Models;
using System;
using System.Linq;

namespace AuthUtility.Mapper
{
    public static class Mapper
    {
        public static UserBasicDetails ToBasicDetails(this UserDetailModel Model)
        {
            if (Model == null || Model.User == null || Model.UserProperties == null)
                return null;

            UserBasicDetails response = new UserBasicDetails();
            response.UserId = Model.User.TAU_Id;
            response.UserName = Model.User.TAU_LoginName;
            response.Email = Model.User.TAU_EmailId;
            response.FirstName = Model.User.TAU_FirstName;
            response.PhoneNumber = Model.User.TAU_PhoneNumber;
            response.ProviderMasterEntityId = Model.User.TAU_ProviderMasterEntityId;
            var maid = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.MAID && x.TAUP_IsActive && x.TAUP_Value != null);
            response.MAID = maid != null ? long.Parse(maid.TAUP_Value) : default(long);
            var gender = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.Gender && x.TAUP_IsActive && x.TAUP_Value != null);
            response.Gender = gender != null ? gender.TAUP_Value : null;
            var empId = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.IWP_EmpID && x.TAUP_IsActive && x.TAUP_Value != null);
            response.EmployeeId = empId != null ? empId.TAUP_Value : null;
            var dob = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.DOB && x.TAUP_IsActive && x.TAUP_Value != null);
            response.DOB = dob != null ? DateTime.Parse(dob.TAUP_Value) : DateTime.MinValue;
            var doj = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.DateOfJoining && x.TAUP_IsActive && x.TAUP_Value != null);
            response.DateOfJoining = doj != null ? DateTime.Parse(doj.TAUP_Value) : DateTime.MinValue;
            var dom = Model.UserProperties.FirstOrDefault(x => x.TAUP_Name == UserPropertyConstants.DateOfMarriage && x.TAUP_IsActive && x.TAUP_Value != null);
            response.DateOfMarriage = dom != null ? DateTime.Parse(dom.TAUP_Value) : DateTime.MinValue;
            return response;
        }
    }
}
