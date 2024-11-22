using System.Data.SqlClient;
using Pay_Xpert.Models;
using Pay_Xpert.Services.Interfaces;
using Pay_Xpert.Utility;
using Pay_Xpert.Exceptions;

namespace Pay_Xpert.Repository.Implementations
{
    internal class FinancialRecordService : IFinancialRecordService
    {
        public void AddFinancialRecord(int employeeId, string description, decimal amount, string recordType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(description) || amount <= 0 || string.IsNullOrWhiteSpace(recordType))
                {
                    throw new InvalidInputException("Invalid input: description, amount, and record type must be valid.");
                }

                using (var connection = DBConnUtil.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new SqlCommand(@"
                        INSERT INTO FinancialRecord (EmployeeID, RecordDate, Description, Amount, RecordType)
                        VALUES (@EmployeeID, @RecordDate, @Description, @Amount, @RecordType)", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@RecordDate", DateTime.Now); 
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@RecordType", recordType);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                        }

                        Console.WriteLine("Financial record added successfully.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while adding financial record: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"Error adding financial record: {ex.Message}");
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
                                throw new FinancialRecordException($"No financial record found with ID {recordId}.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving financial record by ID: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"Error retrieving financial record by ID: {ex.Message}");
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
                            if (!reader.HasRows)
                            {
                                throw new EmployeeNotFoundException($"No financial records found for employee with ID {employeeId}.");
                            }

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
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving financial records for employee: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"Error retrieving financial records for employee: {ex.Message}");
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
                            if (!reader.HasRows)
                            {
                                throw new FinancialRecordException($"No financial records found for date {recordDate.ToShortDateString()}.");
                            }

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
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException($"Database error while retrieving financial records for date: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new FinancialRecordException($"Error retrieving financial records for date: {ex.Message}");
            }
        }
    }
}
