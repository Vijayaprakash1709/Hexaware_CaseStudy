
using System.Data.SqlClient;
using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;


namespace Pay_Xpert.Repository.Implementations
{
  
        internal class FinancialRecordService : IFinancialRecordService
        {
            public void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType)
            {
                try
                {
                    using (var connection = DBConnUtil.GetConnection())
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand(@"
                        INSERT INTO FinancialRecord (EmployeeID, RecordDate, Description, Amount, RecordType)
                        VALUES (@EmployeeID, @RecordDate, @Description, @Amount, @RecordType)", connection))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                            cmd.Parameters.AddWithValue("@RecordDate", DateTime.Now); // Current timestamp
                            cmd.Parameters.AddWithValue("@Description", description);
                            cmd.Parameters.AddWithValue("@Amount", amount);
                            cmd.Parameters.AddWithValue("@RecordType", recordType);

                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Financial record added successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding financial record: {ex.Message}");
                }
            }

            public FinancialRecord GetFinancialRecordById(int recordId)
            {
                try
                {
                    using (var connection = DBConnUtil.GetConnection())
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand(@"
                        SELECT RecordID, EmployeeID, RecordDate, Description, Amount, RecordType
                        FROM FinancialRecord
                        WHERE RecordID = @RecordID", connection))
                        {
                            cmd.Parameters.AddWithValue("@RecordID", recordId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new FinancialRecord
                                    {
                                        RecordID = reader.GetInt32(reader.GetOrdinal("RecordID")),
                                        EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                        RecordDate = reader.GetDateTime(reader.GetOrdinal("RecordDate")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                        RecordType = reader.GetString(reader.GetOrdinal("RecordType"))
                                    };
                                }
                                else
                                {
                                    throw new Exception("No financial record found with the given Record ID.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving financial record by ID: {ex.Message}");
                }
            }

            public List<FinancialRecord> GetFinancialRecordsForEmployee(int employeeId)
            {
                try
                {
                    var financialRecords = new List<FinancialRecord>();
                    using (var connection = DBConnUtil.GetConnection())
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand(@"
                        SELECT RecordID, EmployeeID, RecordDate, Description, Amount, RecordType
                        FROM FinancialRecord
                        WHERE EmployeeID = @EmployeeID", connection))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    financialRecords.Add(new FinancialRecord
                                    {
                                        RecordID = reader.GetInt32(reader.GetOrdinal("RecordID")),
                                        EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                        RecordDate = reader.GetDateTime(reader.GetOrdinal("RecordDate")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                        RecordType = reader.GetString(reader.GetOrdinal("RecordType"))
                                    });
                                }
                            }
                        }
                    }
                    return financialRecords;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving financial records for employee: {ex.Message}");
                }
            }

            public List<FinancialRecord> GetFinancialRecordsForDate(DateTime recordDate)
            {
                try
                {
                    var financialRecords = new List<FinancialRecord>();
                    using (var connection = DBConnUtil.GetConnection())
                    {
                        connection.Open();
                        using (var cmd = new SqlCommand(@"
                        SELECT RecordID, EmployeeID, RecordDate, Description, Amount, RecordType
                        FROM FinancialRecord
                        WHERE CAST(RecordDate AS DATE) = @RecordDate", connection))
                        {
                            cmd.Parameters.AddWithValue("@RecordDate", recordDate.Date);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    financialRecords.Add(new FinancialRecord
                                    {
                                        RecordID = reader.GetInt32(reader.GetOrdinal("RecordID")),
                                        EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                        RecordDate = reader.GetDateTime(reader.GetOrdinal("RecordDate")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                        RecordType = reader.GetString(reader.GetOrdinal("RecordType"))
                                    });
                                }
                            }
                        }
                    }
                    return financialRecords;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving financial records for date: {ex.Message}");
                }
            }
        }
    }


