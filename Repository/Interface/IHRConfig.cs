

using Bhomes_ERP.Models.VM_Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bhomes_ERP.Repository.Interface
{
    public interface IHRConfig
    {
        #region View Table 
        public List<VM_Department> Get_HR_Department();
        public List<VM_SubDepartment> Get_HR_SubDepartment();

        #endregion

        #region Insert Functions
        public bool Save_to_HR_Department(VM_Department model);
        public bool Save_to_HR_SubDepartment(VM_SubDepartment model);
        public bool Save_to_HR_Shift(VM_Shift model);
        public bool Save_to_HR_Designation(VM_Designation model);
        public bool Save_to_HR_EmployeeType(VM_EmployeeType model);
        #endregion


        #region Update Functions
        public bool Update_to_HR_Department(VM_Department model);
        public bool Update_to_HR_Sub_Department(VM_SubDepartment model);
        public bool Update_to_HR_Shift(VM_Shift model);
        public bool Update_to_HR_Designation(VM_Designation model);
        public bool Update_to_HR_EmployeeType(VM_EmployeeType model);
        #endregion



    }
}
