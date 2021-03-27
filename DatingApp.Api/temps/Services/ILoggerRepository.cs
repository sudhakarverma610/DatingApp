using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Model;

namespace WebAPI.Services
{
   public interface ILoggerRepository
    {
        void Information(string message, Exception exception = null, int? customer = null);
        void Warning(string message, Exception exception = null, int? customer = null);
        void Error(string message, Exception exception = null, int? customer = null);
    }
    public class LoggerRepository: ILoggerRepository
    {

 
        public virtual void Information(string message, Exception exception = null, int? customer = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;
            InsertLog(LogLevel.Information, message, exception?.ToString() ?? string.Empty, customer);
        }
        public virtual void Warning(string message, Exception exception = null, int? customer = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            InsertLog(LogLevel.Warning, message, exception?.ToString() ?? string.Empty, customer);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="customer">Customer</param>
        public virtual void Error(string message, Exception exception = null, int? customer = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            InsertLog(LogLevel.Error, message, exception?.ToString() ?? string.Empty, customer);
        }
        private void InsertLog(LogLevel logLevel, string shortMessage, string fullMessage, int? customer)
        {
            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress ="", //_webHelper.GetCurrentIpAddress(),
                CustomerId = customer,
                PageUrl = "",//_webHelper.GetThisPageUrl(true),
                ReferrerUrl = "",//_webHelper.GetUrlReferrer(),
                CreatedOnUtc = DateTime.UtcNow
            };
            using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.IpAddress), log.IpAddress));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.LogLevelId), (int)log.LogLevel));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.ShortMessage), log.ShortMessage));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.FullMessage), log.FullMessage));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.CustomerId), log.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.PageUrl), log.PageUrl));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.ReferrerUrl), log.ReferrerUrl));
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.CreatedOnUtc), log.CreatedOnUtc));
                    int rowsAffected = cmd.ExecuteNonQuery();
                }

 
            }
        }
        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            if (string.IsNullOrEmpty(CommonSetting.LogByIdsLoggerStoreProcedureName)){
                throw new Exception("Please Provide LogByIdsLoggerStoreProcedureName ");
            }
            IList<Log> logs = new List<Log>();
            using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Ids", string.Join(",",logIds)));
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var log = new Log();
                        log.Id = Convert.ToInt32(reader["@"+nameof(log.Id)]);
                        log.LogLevel =(LogLevel) Convert.ToInt32(reader["@"+nameof(log.LogLevel)]);
                        log.ShortMessage = Convert.ToString(reader["@"+nameof(log.ShortMessage)]);
                        log.FullMessage = Convert.ToString(reader["@"+nameof(log.FullMessage)]);
                        log.IpAddress = Convert.ToString(reader["@"+nameof(log.IpAddress)]);
                        log.CustomerId = Convert.ToInt32(reader["@"+nameof(log.CustomerId)]);
                        log.PageUrl = Convert.ToString(reader["@"+nameof(log.PageUrl)]);
                        log.ReferrerUrl = Convert.ToString(reader["@"+nameof(log.ReferrerUrl)]);
                        log.CreatedOnUtc =Convert.ToDateTime((reader["@"+nameof(log.CreatedOnUtc)]));
                        logs.Add(log);


                    }
                }


            }
            return logs;
        }
        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;
            var log = new Log();

            using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.Id), logId));
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows&& reader.Read())
                    {
                        log.Id = Convert.ToInt32(reader["@"+nameof(log.Id)]);
                        log.LogLevel = (LogLevel)Convert.ToInt32(reader["@"+nameof(log.LogLevel)]);
                        log.ShortMessage = Convert.ToString(reader["@"+nameof(log.ShortMessage)]);
                        log.FullMessage = Convert.ToString(reader["@"+nameof(log.FullMessage)]);
                        log.IpAddress = Convert.ToString(reader["@"+nameof(log.IpAddress)]);
                        log.CustomerId = Convert.ToInt32(reader["@"+nameof(log.CustomerId)]);
                        log.PageUrl = Convert.ToString(reader["@"+nameof(log.PageUrl)]);
                        log.ReferrerUrl = Convert.ToString(reader["@"+nameof(log.ReferrerUrl)]);
                        log.CreatedOnUtc = Convert.ToDateTime((reader["@"+nameof(log.CreatedOnUtc)])); 

                    }
                }


            }
            return log;
        }
        public virtual LogModelView GetAllLogs(DateTime? fromUtc = null, DateTime? toUtc = null,
          string message = "", LogLevel? logLevel = null,
          int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var logfilter = new LogModelView()
            {
                FromDate = fromUtc,
                ToDate = toUtc,
                Message = message,
                Page = pageIndex + 1,
                Size = pageSize
            };

            IList<Log> logs = new List<Log>();
            using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (fromUtc.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@"+nameof(logfilter.FromDate), fromUtc.Value));
                    if (toUtc.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@"+nameof(logfilter.ToDate), toUtc.Value));

                     if (logLevel.HasValue)
                    {
                        var logLevelId = (int)logLevel.Value;
                        cmd.Parameters.Add(new SqlParameter("@"+nameof(logfilter.logLevelId), logLevelId));
                    }
                    if (!string.IsNullOrEmpty(message))
                        cmd.Parameters.Add(new SqlParameter("@"+nameof(logfilter.Message), message));
                    cmd.Parameters.Add("@"+nameof(logfilter.TotalPages), SqlDbType.Int);
                    cmd.Parameters["@"+nameof(logfilter.TotalPages)].Direction = ParameterDirection.Output;

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var log = new Log();
                        log.Id = Convert.ToInt32(reader["@"+nameof(log.Id)]);
                        log.LogLevel = (LogLevel)Convert.ToInt32(reader["@"+nameof(log.LogLevel)]);
                        log.ShortMessage = Convert.ToString(reader["@"+nameof(log.ShortMessage)]);
                        log.FullMessage = Convert.ToString(reader["@"+nameof(log.FullMessage)]);
                        log.IpAddress = Convert.ToString(reader["@"+nameof(log.IpAddress)]);
                        log.CustomerId = Convert.ToInt32(reader["@"+nameof(log.CustomerId)]);
                        log.PageUrl = Convert.ToString(reader["@"+nameof(log.PageUrl)]);
                        log.ReferrerUrl = Convert.ToString(reader["@"+nameof(log.ReferrerUrl)]);
                        log.CreatedOnUtc = Convert.ToDateTime((reader["@"+nameof(log.CreatedOnUtc)]));
                        logs.Add(log);
                    }
                    logfilter.TotalPages =Convert.ToInt32(cmd.Parameters["@"+nameof(logfilter.TotalPages)].Value);
                }


            }
            logfilter.logs = logs;
            return logfilter;
         
        }
        public virtual void DeleteLogs(IList<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException("@"+nameof(logs));

            DeleteLogs(logs.Select(x=>x.Id).ToArray());
        }
        public virtual void DeleteLogs(int[] logIds)
        {
             using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Ids", string.Join(",", logIds)));
                    var rowAffected =Task.Run(async()=>await cmd.ExecuteNonQueryAsync());
                }


            }
         }
        public virtual void DeleteLog(int logId)
        {
            Log log = new Log() { Id = logId };
             using (SqlConnection connection = new SqlConnection(CommonSetting.ConnetionString))
            {
                using (SqlCommand cmd = new SqlCommand(CommonSetting.InsertLoggerStoreProcedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@"+nameof(log.Id), log.Id));
                    var rowAffected = Task.Run(async () => await cmd.ExecuteNonQueryAsync());
                }


            }
        }

    }
}
