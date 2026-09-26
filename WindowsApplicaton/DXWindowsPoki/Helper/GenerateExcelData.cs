using System;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace DXWindowsApplication.Helper
{
    public class GenerateExcelData
    {

        public static DataSet GenerateExcelDataFile(string path)
        {
            OleDbConnection _connection;
            OleDbCommand _command;
            OleDbDataAdapter _dataAdapter;
            DataSet _dataSet;

            try
            {
                string connectionString = "";
                if (Path.GetExtension(path) == ".xls")
                {
                    connectionString =
                        String.Format(
                            @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=2;""",
                            path);
                }
                else if (Path.GetExtension(path) == ".xlsx")
                {
                    connectionString =
                        String.Format(
                            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2\;""",
                            path);
                }

                string query = String.Format("select * from [{0}$]", "Sheet1");
                _connection = new OleDbConnection(connectionString);
                if (_connection.State == ConnectionState.Closed) _connection.Open();
                _command = new OleDbCommand(query, _connection);
                _dataAdapter = new OleDbDataAdapter(_command);
                _dataSet = new DataSet();
                _dataAdapter.Fill(_dataSet);

                return _dataSet;
            }
            catch            {
                return null;
            }
        }
    }
}