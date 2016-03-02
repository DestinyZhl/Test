using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using SPP.Common.Constants;
using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels.Settings;
using System.Data.Entity;
using System.Collections.Generic;

namespace SPP.Data.Repository
{
    public class EnumerationRepository : RepositoryBase<Enumeration>,IEnumerationRepository
    {
        public EnumerationRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
        /// <summary>
        /// 获取当前时段及日期
        /// </summary>
        /// <param name="opType"></param>
        /// <param name="Time_Interval"></param>
        /// <returns></returns>
        public List<IntervalEnum> GetIntervalInfo(string opType,string InputPut_Interval = "")
        {
            var opEnumType = "Time_Interval_" + opType;
            var nowTime = DateTime.Now.ToString("yyyy-MM-dd");           
            //获取当前时间所在时段
            var strSql = @"DECLARE @AddLeftTime INT,@AddRightTime INT ,@NowDate NVARCHAR(20),@GETDATE DATETIME

SET @GETDATE=GETDATE()

SET @AddLeftTime=(select CONVERT(INT, Enum_Value)Enum_Value from
              dbo.Enumeration
              WHERE Enum_Type='Time_InterVal_ADD'
              AND Enum_Name='Time_InterVal_LEFTOP1')
SET @AddRightTime=(select CONVERT(INT, Enum_Value)Enum_Value from
              dbo.Enumeration
              WHERE Enum_Type='Time_InterVal_ADD'
              AND Enum_Name='Time_InterVal_RIGHTOP1')
SET @NowDate=( SELECT CASE WHEN DATEDIFF(MINUTE,@GETDATE,DivTime)>0 THEN CONVERT(varchar(100), DATEADD(DAY,-1,@GETDATE), 23) ELSE CONVERT(varchar(100), @GETDATE, 23) END AS nowDate
FROM (SELECT DATEADD(MINUTE,@AddLeftTime,CONVERT(DATETIME,CONVERT(varchar(100), @GETDATE, 23)+' '+SUBSTRING(Enum_Value,0,6) +':00'))DivTime from
dbo.Enumeration AS e WHERE Enum_Type='Time_InterVal_OP1' AND Enum_Name='1' )divtemp)           
 
 select Enum_Type OpEnumType,@NowDate NowDate,CAST(ORDERID AS NVARCHAR(20)) IntervalNo,Enum_Value Time_Interval,@GETDATE BeginTime,@GETDATE EndTime from
 (
 select beginminnute,CASE WHEN beginminnute>endminnute THEN endminnute+60*24 ELSE endminnute END endminnute,nowMinute,Enum_Value,orderID,Enum_Type from
 (
 select CASE WHEN DATEDIFF(MINUTE,nowTime,beginTime)-60<0 THEN 24*60+DATEDIFF(MINUTE,nowTime,beginTime) ELSE DATEDIFF(MINUTE,nowTime,beginTime) END beginminnute,
 CASE WHEN DATEDIFF(MINUTE,nowTime,endTime)-60<0 THEN 24*60+DATEDIFF(MINUTE,nowTime,endTime) ELSE DATEDIFF(MINUTE,nowTime,endTime) END endminnute,
 CASE WHEN DATEDIFF(MINUTE,nowTime,@GETDATE)-60<0 THEN 24*60+DATEDIFF(MINUTE,nowTime,@GETDATE)ELSE DATEDIFF(MINUTE,nowTime,@GETDATE) END nowMinute,Enum_Value,orderID,Enum_Type from
 (          
select DATEADD(MINUTE,@AddLeftTime,CONVERT(DATETIME,CONVERT(varchar(100), @GETDATE, 23)+' '+SUBSTRING(Enum_Value,0,6) +':00'))beginTime,
DATEADD(MINUTE,@AddRightTime,CONVERT(DATETIME,CONVERT(varchar(100), @GETDATE, 23)+' '+SUBSTRING(Enum_Value,7,6) +':00')) endTime
,Enum_Value,CONVERT(INT, enum_name) orderID,Enum_Type,
CONVERT(DATETIME,CONVERT(varchar(100), @GETDATE, 20))nowTime
from
dbo.Enumeration AS e
WHERE Enum_Type='Time_Interval_OP1') m)mm)temp
WHERE temp.nowMinute>=temp.beginminnute
AND temp.nowMinute<temp.endminnute
ORDER BY orderID";
            strSql = string.Format(strSql, opEnumType);
            var dbList = DataContext.Database.SqlQuery<IntervalEnum>(strSql).ToList();
            return dbList;
        }

        public List<Enumeration> GetIntervalOrder()
        {
            var strSql = @"select * from
                            dbo.Enumeration AS e
                            WHERE Enum_Type='Time_InterVal_OP1'
                            ORDER BY CONVERT(INT, enum_name)";
            var dbList = DataContext.Database.SqlQuery<Enumeration>(strSql).ToList();
            return dbList;
        }

        public List<string> GetEnumNameForKeyProcess()
        {
            var query = from enumertion in DataContext.Enumeration
                        where enumertion.Enum_Type == "Report_Key_Process"
                        select enumertion.Enum_Name;

            return query.Distinct().ToList();

        }

        public IQueryable<EnumerationDTO> GetEnumValueForKeyProcess(string partTypes)
        {
            var query = from enumertion in DataContext.Enumeration
                        where enumertion.Enum_Type == "Report_Key_Process" && enumertion.Enum_Name == partTypes
                        select new EnumerationDTO() {
                            Enum_Name= enumertion.Enum_Name,
                            Enum_Value= enumertion.Enum_Value,
                            Enum_UID=enumertion.Enum_UID
                        };
              return query.Distinct();
        }
    }
    public interface IEnumerationRepository : IRepository<Enumeration>
    {
        List<IntervalEnum> GetIntervalInfo(string opType,string InputPut_Interval="");
        List<Enumeration> GetIntervalOrder();
        IQueryable<EnumerationDTO> GetEnumValueForKeyProcess(string partTypes);
        List<string> GetEnumNameForKeyProcess();
    }
}
