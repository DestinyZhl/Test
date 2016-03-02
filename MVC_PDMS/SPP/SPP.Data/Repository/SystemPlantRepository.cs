﻿using SPP.Common.Enums;
using SPP.Data.Infrastructure;
using SPP.Model;
using SPP.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;

namespace SPP.Data.Repository
{
    public class SystemPlantRepository : RepositoryBase<System_Plant>, ISystemPlantRepository
    {
        public SystemPlantRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        #region 公用方法对比时间
        public Boolean CompareDateOK(DateTime StartDate, DateTime EndDate, DateTime Compare_StartDate, DateTime Compare_EndDate)
        {
            var mindate = System.DateTime.MinValue;
            if (Compare_StartDate == mindate && Compare_EndDate != mindate)
            {
                if (EndDate != mindate)
                {
                    if (EndDate < Compare_EndDate&& StartDate <= EndDate) return true; else return false;
                }
                else
                    return false;
            }
            else if (Compare_StartDate != mindate && Compare_EndDate == mindate)
            {
                if (StartDate != mindate)
                {
                    if (StartDate > Compare_StartDate && StartDate <= EndDate) return true; else return false;
                }
                else return false;
            }
            else if (Compare_StartDate != mindate && Compare_EndDate != mindate)
            {
                if (StartDate != mindate && EndDate != mindate)
                {
                    if (StartDate > Compare_StartDate && EndDate < Compare_EndDate && StartDate <= EndDate) return true; else return false;
                }
                else return false;
            }
            else 
            {
                return true;
            }
        }
        #endregion

        /// <summary>
        /// Query system users and order by account_id asc
        /// </summary>
        /// <param name="search">search model</param>
        /// <param name="page">page info</param>
        /// <param name="count">number of total records</param>
        /// <returns></returns>
        public IQueryable<System_Plant> QueryPlants(PlantModelSearch search, Page page, out int count)
        {
            var query = from plant in DataContext.System_Plant.Include("System_Users")
                        select plant;
            if (string.IsNullOrEmpty(search.ExportUIds))
            {
                #region Query_Types

                if (search.query_types != null && search.Reference_Date != null)
                {
                    EnumValidity queryType = (EnumValidity)Enum.ToObject(typeof(EnumValidity), search.query_types);

                    switch (queryType)
                    {
                        case EnumValidity.Valid:
                            query = query.Where(p => p.Begin_Date <= search.Reference_Date && (p.End_Date >= search.Reference_Date || p.End_Date == null));
                            break;
                        case EnumValidity.Invalid:
                            query = query.Where(p => p.Begin_Date > search.Reference_Date || (p.End_Date < search.Reference_Date && p.End_Date != null));
                            break;
                        default:
                            break;
                    }

                }
                #endregion
               

                #region Modified_Date
                if (search.Modified_Date_From != null)
                {
                    query = query.Where(m => m.Modified_Date >= search.Modified_Date_From);
                }
                if (search.Modified_Date_End != null)
                {
                    var endDate = ((DateTime)search.Modified_Date_End).AddDays(1);
                    query = query.Where(m => m.Modified_Date < endDate);
                }
                #endregion

                #region 查询Modified_NTID
                if (!string.IsNullOrWhiteSpace(search.Modified_By_NTID))
                {
                    query = query.Where(q => q.System_Users.User_NTID == search.Modified_By_NTID);
                }
                #endregion


                if (!string.IsNullOrWhiteSpace(search.Plant))
                {
                    query = query.Where(p => p.Plant == search.Plant);

                }
                if (!string.IsNullOrWhiteSpace(search.Location))
                {
                    query = query.Where(p => p.Location.Contains(search.Location));
                }
                //Name_0,公司代碼。
                if (!string.IsNullOrWhiteSpace(search.Name_0))
                {
                    query = query.Where(p => p.Name_0 == search.Name_0);
                }
                //Name_1,公司中文名稱模糊查詢。
                if (!string.IsNullOrWhiteSpace(search.Name_1))
                {
                    query = query.Where(p => p.Name_1.Contains(search.Name_1));
                }
                if (!string.IsNullOrWhiteSpace(search.PlantManager_Name))
                {
                    query = query.Where(p => p.PlantManager_Name.Contains(search.PlantManager_Name));
                }

                if (!string.IsNullOrWhiteSpace(search.Type))
                {
                    query = query.Where(p => p.Type.Contains(search.Type));
                }

                count = query.Count();
                return query.OrderBy(o => o.System_Plant_UID).GetPage(page);
            }
            else
            {
                //for export data
                var array = Array.ConvertAll(search.ExportUIds.Split(','), s => int.Parse(s));
                query = query.Where(p => array.Contains(p.System_Plant_UID));

                count = 0;
                return query.OrderBy(o => o.Plant);
            }
        }

        public IQueryable<System_Plant> GetValidPlantsByUserUId(int accountUId)
        {
            var now = DateTime.Now.Date;
            var query = from plant in DataContext.System_Plant
                        join user_plant in DataContext.System_User_Plant on plant.System_Plant_UID equals user_plant.System_Plant_UID
                        where user_plant.Account_UID == accountUId
                            && ((user_plant.Begin_Date <= now && !user_plant.End_Date.HasValue)
                                || (user_plant.Begin_Date <= now && user_plant.End_Date >= now))
                        select plant;
            return query;
        }

        public System_Plant GetPlantByPlant(string Plant)
        {
            var entity = DataContext.System_Plant.FirstOrDefault(p => p.Plant == Plant);        
            return entity;
        }
    }
    public interface ISystemPlantRepository : IRepository<System_Plant>
    {
        /// <summary>
        /// Query system plant and order by Plant asc
        /// </summary>
        /// <param name="search">search model</param>
        /// <param name="page">page info</param>
        /// <param name="count">number of total records</param>
        /// <returns>paged records</returns>
        IQueryable<System_Plant> QueryPlants(PlantModelSearch search, Page page, out int count);    

        IQueryable<System_Plant> GetValidPlantsByUserUId(int accountUId);
        System_Plant GetPlantByPlant(string Plant);
    }
}