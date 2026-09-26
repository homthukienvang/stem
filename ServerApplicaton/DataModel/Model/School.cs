using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public partial class School
    {
        public int SchoolId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public string DeploymentMethod { get; set; }
        public int NumberOfStudent { get; set; }
        public string PrintDocument { get; set; }
        public string Certificate { get; set; }
        public int TotalRowCount { get; set; }
        public int NumberOfTeacher { get; set; }

        public IEnumerable<SchoolLog> SchoolLogs { get; set; }

        public School OldSchool { get; set; }

        public string GetDiff()
        {
            StringBuilder str = new StringBuilder();
            if (OldSchool != null)
            {
                if (OldSchool.Code != Code)
                    str.Append("Mã trường: " + Code + ";");
                if (OldSchool.Name != Name)
                    str.Append("Tên trường: " + Name + ";");
                if (OldSchool.CityCode != CityCode)
                    str.Append("Mã tỉnh/thành phố: " + CityCode + ";");
                if (OldSchool.DistrictCode != DistrictCode)
                    str.Append("Mã quận/huyện: " + DistrictCode + ";");
                if (OldSchool.DeploymentMethod != DeploymentMethod)
                    str.Append("Phương thức triển khai: " + DeploymentMethod + ";");
                if (OldSchool.NumberOfStudent != NumberOfStudent)
                    str.Append("Số lượng giáo viên: " + NumberOfStudent + ";");
                if (OldSchool.PrintDocument != PrintDocument)
                    str.Append("Cho phép in giáo án: " + PrintDocument + ";");
                if (OldSchool.Certificate != Certificate)
                    str.Append("Giấy chứng nhận: " + Certificate + ";");
            }
            else
            {
                str.Append("Mã trường: " + Code + ";");
                str.Append("Tên trường: " + Name + ";");
                str.Append("Mã tỉnh/thành phố: " + CityCode + ";");
                str.Append("Mã quận/huyện: " + DistrictCode + ";");
                str.Append("Phương thức triển khai: " + DeploymentMethod + ";");
                str.Append("Số lượng giáo viên: " + NumberOfStudent + ";");
                str.Append("Cho phép in giáo án: " + PrintDocument + ";");
                str.Append("Giấy chứng nhận: " + Certificate + ";");
            }


            return str.ToString();
        }
    }
}
