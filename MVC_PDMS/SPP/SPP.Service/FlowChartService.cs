using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPP.Data;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Model;
using SPP.Model.ViewModels;

namespace SPP.Service
{
    public interface IFlowChartService
    {
        PagedListModel<FlowChartModelGet> QueryFlowCharts(FlowChartModelSearch searchModel, Page page);
        string CheckFlowChart(FlowChartExcelImportParas parasItem);
        List<SystemFunctionPlantDTO> QueryAllFunctionPlants();
        void ImportFlowChart(FlowChartImport importItem);
        void ImportFlowUpdateChart(FlowChartImport importItem);
        FlowChartGet QueryFlowChart(int FlowChart_Master_UID);
        FlowChartExcelExport ExportFlowChart(int FlowChart_Master_UID, bool isTemp);
        List<FlowChartHistoryGet> QueryHistoryFlowChart(int FlowChart_Master_UID);
        PagedListModel<FlowChartDetailGet> QueryFLDetailList(int id, int Version);
        List<FlowChartDetailDTO> QueryFLDetailList(int id);
        bool ClosedFL(int id, bool isClosed);
        PagedListModel<FlowChartDetailGet> QueryFLDetailTempList(int id, int Version);
        FlowChartDetailGet QueryFLDetailByID(int id);
        FlowChartDetailGetByMasterInfo QueryFunPlant(int id);
        void SaveFLDetailInfo(FlowChartDetailDTO dto, int AccountID);
        void SaveAllDetailInfo(List<FlowChartDetailAndMGDataInputDTO> dto, int AccountID);
        void DeleteFLTemp(int id);
        void ImportFlowChartMGData(List<FlowChartMgDataDTO> mgDataList, int FlowChart_Master_UID);

        #region ----- add by Destiny Zhang  

        PagedListModel<PrjectListVM> QueryFlowChartMasterDatas(int user_account_uid);
        ProcessDataSearch QueryFlowChartDataByMasterUid(int flowChartMaster_uid);
        List<FlowChartMasterDTO> QueryProjectTypes();
        List<string> QueryProcess(int flowchartmasterUid);

        #endregion

        #region ---------add by justin

        PagedListModel<FlPlanManagerVM> QueryProcessMGData(int masterUID,DateTime date);
        FlowChartPlanManagerVM QueryProcessMGDataSingle(int uid , DateTime date);
        string FlowChartPlan(FlowChartPlanManagerVM ent);
        #endregion
    }

    public class FlowChartService:IFlowChartService
    {
        #region Private interfaces properties
        private readonly IUnitOfWork unitOfWork;
        private readonly IFlowChartMasterRepository flowChartMasterRepository;
        private readonly IFlowChartDetailRepository flowChartDetailRepository;
        private readonly IFlowChartMgDataRepository flowChartMgDataRepository;
        private readonly ISystemBUDRepository systemBUDRepository;
        private readonly ISystemProjectRepository systemProjectRepository;
        private readonly ISystemFunctionPlantRepository systemFunctionPlantRepository;
        private readonly IFlowChartDetailTempRepository flowChartDetailTempRepository;
        private readonly IFlowChartMgDataTempRepository flowChartMgDataTempRepository;
        private readonly ISystemUserRepository systemUserRepository;

        #endregion //Private interfaces properties

        #region Service constructor
        public FlowChartService(
            IFlowChartMasterRepository flowChartMasterRepository,
            IFlowChartDetailRepository flowChartDetailRepository,
            IFlowChartMgDataRepository flowChartMgDataRepository,
            ISystemBUDRepository systemBUDRepository,
            ISystemProjectRepository systemProjectRepository,
            ISystemFunctionPlantRepository systemFunctionPlantRepository,
            IFlowChartDetailTempRepository flowChartDetailTempRepository,
            IFlowChartMgDataTempRepository flowChartMgDataTempRepository,
            ISystemUserRepository systemUserRepository,
        IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.flowChartMasterRepository = flowChartMasterRepository;
            this.flowChartDetailRepository = flowChartDetailRepository;
            this.flowChartMgDataRepository = flowChartMgDataRepository;
            this.systemProjectRepository = systemProjectRepository;
            this.systemBUDRepository = systemBUDRepository;
            this.systemFunctionPlantRepository = systemFunctionPlantRepository;
            this.flowChartDetailTempRepository = flowChartDetailTempRepository;
            this.flowChartMgDataTempRepository = flowChartMgDataTempRepository;
            this.systemUserRepository = systemUserRepository;
        }
        #endregion //Service constructor

        public PagedListModel<FlowChartModelGet> QueryFlowCharts(FlowChartModelSearch searchModel, Page page)
        {
            var totalCount = 0;
            var flChartList = flowChartMasterRepository.QueryFlowCharts(searchModel, page, out totalCount);

            return new PagedListModel<FlowChartModelGet>(totalCount, flChartList.ToList());
        }

        private FlowChartGet SetAutoMapFlChart(FlowChart_Master item)
        {
            FlowChartGet model = new FlowChartGet();
            model.FlowChartMasterDTO = AutoMapper.Mapper.Map<FlowChartMasterDTO>(item);
            model.SystemProjectDTO = AutoMapper.Mapper.Map<SystemProjectDTO>(item.System_Project);
            model.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(item.System_Users);
            model.BU_D_Name = item.System_Project.System_BU_D.BU_D_Name;
            return model;
        }

        public List<FlowChartMasterDTO> QueryProjectTypes()
        {
            var EnumEntity = flowChartMasterRepository.QueryProjectTypes();
            var result = AutoMapper.Mapper.Map<List<FlowChartMasterDTO>>(EnumEntity);
            return result;
        }



        public List<string> QueryProcess(int flowchartmasterUid)
        {
            return flowChartDetailRepository.QueryProcess(flowchartmasterUid).ToList();
        }

        public string CheckFlowChart(FlowChartExcelImportParas parasItem)
        {
            string isValid = string.Empty;
            var budItem = systemBUDRepository.GetMany(m => m.BU_D_Name == parasItem.BU_D_Name).FirstOrDefault();
            //如果budItem没有，说明不存在这个客户
            if (budItem != null)
            {
                if (parasItem.isEdit)
                {
                    var masterItem = flowChartMasterRepository.GetById(parasItem.FlowChart_Master_UID);
                    if (masterItem.System_Project.Project_Name == parasItem.Project_Name && masterItem.System_Project.System_BU_D.BU_D_Name == parasItem.BU_D_Name
                        && masterItem.System_Project.Product_Phase == parasItem.Product_Phase && masterItem.Part_Types == parasItem.Part_Types)
                    {
                        isValid = string.Format("{0}_{1}_{2}", masterItem.FlowChart_Master_UID, masterItem.Project_UID, masterItem.FlowChart_Version);
                    }
                    else
                    {
                        isValid = "客户，专案名称，部件，阶段不匹配，不能更新";
                    }
                }
                else
                {
                    var projetItem = systemProjectRepository.GetMany(m => m.BU_D_UID == budItem.BU_D_UID && m.Project_Name == parasItem.Project_Name && m.Product_Phase == parasItem.Product_Phase).FirstOrDefault();
                    //如果projectUIDList没有，说明该数据不存在
                    if (projetItem != null)
                    {
                        //如果flItem为空，说明可以新导入这条数据
                        var flItem = flowChartMasterRepository.GetMany(m => m.Project_UID == projetItem.Project_UID && m.Part_Types == parasItem.Part_Types).FirstOrDefault();
                        if (flItem == null)
                        {
                            isValid = projetItem.Project_UID.ToString();
                        }
                        else
                        {
                            isValid = "导入的专案已经存在，不能新增专案";
                        }
                    }
                    else
                    {
                        isValid = "客户或专案名称或阶段不存在，不能导入";
                    }
                }
            }
            else
            {
                isValid = "客户名称不存在";
            }
            return isValid;
        }

        public List<SystemFunctionPlantDTO> QueryAllFunctionPlants()
        {
            var functionPlants = systemFunctionPlantRepository.GetAll();
            List<SystemFunctionPlantDTO> dtoList = new List<SystemFunctionPlantDTO>();
            foreach (var item in functionPlants)
            {
                dtoList.Add(AutoMapper.Mapper.Map<SystemFunctionPlantDTO>(item));
            }
            return dtoList;
        }

        public void ImportFlowChart(FlowChartImport importItem)
        {
            FlowChart_Master flMasterItem = new FlowChart_Master();
            flMasterItem = AutoMapper.Mapper.Map<FlowChart_Master>(importItem.FlowChartMasterDTO);
            flowChartMasterRepository.Add(flMasterItem);

            foreach (var detailDTOItem in importItem.FlowChartImportDetailDTOList)
            {
                var detailItem = AutoMapper.Mapper.Map<FlowChart_Detail>(detailDTOItem.FlowChartDetailDTO);
                flMasterItem.FlowChart_Detail.Add(detailItem);

                var mgDataItem = AutoMapper.Mapper.Map<FlowChart_MgData>(detailDTOItem.FlowChartMgDataDTO);
                detailItem.FlowChart_MgData.Add(mgDataItem);
            }
            unitOfWork.Commit();
        }

        public void ImportFlowUpdateChart(FlowChartImport importItem)
        {
            //进行更新操作，全删全插
            //var oldMasterFLItem = flowChartMasterRepository.GetById(importItem.FlowChartMasterDTO.FlowChart_Master_UID);

            //全删操作
            //var tempDetailList = flowChartDetailTempRepository.GetMany(m => m.FlowChart_Master_UID == oldMasterFLItem.FlowChart_Master_UID).ToList();
            //foreach (var tempDetailItem in tempDetailList)
            //{
            //    flowChartDetailTempRepository.Delete(tempDetailItem);
            //}
            //var tempIdList = tempDetailList.Select(m => m.FlowChart_DT_UID).ToList();
            //var tempMgDataList = flowChartMgDataTempRepository.GetMany(m => tempIdList.Contains(m.FlowChart_DT_UID)).ToList();
            //foreach (var tempMgDataItem in tempMgDataList)
            //{
            //    flowChartMgDataTempRepository.Delete(tempMgDataItem);
            //}

            //全插操作
            //oldMasterFLItem.FlowChart_Version_Comment = importItem.FlowChartMasterDTO.FlowChart_Version_Comment;
            //List<FlowChart_Detail_Temp> flDetailTempList = new List<FlowChart_Detail_Temp>();
            //List<FlowChart_MgData_Temp> flMgTempList = new List<FlowChart_MgData_Temp>();
            //StringBuilder insertSql = new StringBuilder();
            //foreach (var detailDTOItem in importItem.FlowChartImportDetailDTOList)
            //{
            //    ////第二层表
            //    //var detailItem = AutoMapper.Mapper.Map<FlowChart_Detail_Temp>(detailDTOItem.FlowChartDetailDTO);
            //    //detailItem.Rise_Version_Flag = true;

            //    //oldMasterFLItem.FlowChart_Detail_Temp.Add(detailItem);

            //    ////第三层表
            //    //FlowChart_MgData_Temp newMgData = new FlowChart_MgData_Temp();
            //    //newMgData.FlowChart_DT_UID = detailItem.FlowChart_DT_UID;
            //    //newMgData.Product_Plan = detailDTOItem.FlowChartMgDataDTO.Product_Plan;
            //    //newMgData.Target_Yield = detailDTOItem.FlowChartMgDataDTO.Target_Yield;
            //    //newMgData.Modified_UID = detailDTOItem.FlowChartMgDataDTO.Modified_UID;
            //    //newMgData.Modified_Date = detailDTOItem.FlowChartMgDataDTO.Modified_Date;
            //    //detailItem.FlowChart_MgData_Temp.Add(newMgData);


            //}


            //flowChartMasterRepository.Update(oldMasterFLItem);

            //unitOfWork.Commit();

            flowChartMasterRepository.UpdateFolowCharts(importItem);
        }

        public FlowChartGet QueryFlowChart(int FlowChart_Master_UID)
        {
            var flItem = flowChartMasterRepository.GetById(FlowChart_Master_UID);
            var item = SetAutoMapFlChart(flItem);
            return item;
        }

        public PagedListModel<FlowChartDetailGet> QueryFLDetailList(int id, int Version)
        {
            var totalCount = 0;
            var masterItem = flowChartMasterRepository.GetById(id);
            if (masterItem != null)
            {
                IList<FlowChartDetailGet> importList = new List<FlowChartDetailGet>();

                var flChartList = flowChartMasterRepository.QueryFLDetailList(id, Version, out totalCount);
                //importItem.FlowChartMasterDTO = AutoMapper.Mapper.Map<FlowChartMasterDTO>(masterItem);
                foreach (var flChartItem in flChartList)
                {
                    FlowChartDetailGet importDetailItem = new FlowChartDetailGet();
                    importDetailItem.FlowChartDetailDTO = AutoMapper.Mapper.Map<FlowChartDetailDTO>(flChartItem);
                    importDetailItem.FlowChartMgDataDTO = AutoMapper.Mapper.Map<FlowChartMgDataDTO>(flChartItem.FlowChart_MgData.FirstOrDefault());
                    importDetailItem.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(flChartItem.System_Users);
                    importDetailItem.FunPlant = flChartItem.System_Function_Plant.FunPlant;
                    importList.Add(importDetailItem);
                }
                return new PagedListModel<FlowChartDetailGet>(0, importList);
            }
            else
            {
                return null;
            }
        }

        public List<FlowChartDetailDTO> QueryFLDetailList(int id)
        {
            var maxVersion = flowChartDetailRepository.GetMany(m => m.FlowChart_Master_UID == id).Max(m => m.FlowChart_Version);
            var list = flowChartDetailRepository.GetMany(m => m.FlowChart_Master_UID == id && m.FlowChart_Version == maxVersion).OrderBy(m => m.Process_Seq).ToList();
            var dto = AutoMapper.Mapper.Map<List<FlowChartDetailDTO>>(list);
            return dto;
        }


        public PagedListModel<FlowChartDetailGet> QueryFLDetailTempList(int id, int Version)
        {
            var totalCount = 0;
            var masterItem = flowChartMasterRepository.GetById(id);
            IList<FlowChartDetailGet> importList = new List<FlowChartDetailGet>();
            if (masterItem != null)
            {
                var flChartList = flowChartMasterRepository.QueryFLDetailTempList(id, Version, out totalCount);
                //importItem.FlowChartMasterDTO = AutoMapper.Mapper.Map<FlowChartMasterDTO>(masterItem);
                foreach (var flChartItem in flChartList)
                {
                    FlowChartDetailGet importDetailItem = new FlowChartDetailGet();
                    importDetailItem.FlowChartDetailTempDTO = AutoMapper.Mapper.Map<FlowChartDetailTempDTO>(flChartItem);
                    importDetailItem.FlowChartMgDataTempDTO = AutoMapper.Mapper.Map<FlowChartMgDataTempDTO>(flChartItem.FlowChart_MgData_Temp.FirstOrDefault());

                    importDetailItem.FlowChartDetailDTO = AutoMapper.Mapper.Map<FlowChartDetailDTO>(importDetailItem.FlowChartDetailTempDTO);
                    importDetailItem.FlowChartDetailDTO.FlowChart_Detail_UID = importDetailItem.FlowChartDetailTempDTO.FlowChart_DT_UID;

                    //importDetailItem.FlowChartMgDataDTO = AutoMapper.Mapper.Map<FlowChartMgDataDTO>(importDetailItem.FlowChartMgDataTempDTO);
                    //importDetailItem.FlowChartMgDataDTO.FlowChart_MgData_UID = importDetailItem.FlowChartMgDataTempDTO.FlowChart_MgDataT_UID;

                    importDetailItem.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(flChartItem.System_Users);
                    importDetailItem.FunPlant = flChartItem.System_Function_Plant.FunPlant;
                    importList.Add(importDetailItem);
                }
            }
            return new PagedListModel<FlowChartDetailGet>(0, importList);
        }

        public List<FlowChartHistoryGet> QueryHistoryFlowChart(int FlowChart_Master_UID)
        {
            List<FlowChartHistoryGet> historyList = new List<FlowChartHistoryGet>();
            var userInfoList = systemUserRepository.GetAll().ToList();
            var masterItem = flowChartMasterRepository.GetById(FlowChart_Master_UID);
            var detailVersionList = flowChartDetailRepository.GetMany(m => m.FlowChart_Master_UID == FlowChart_Master_UID).OrderBy(m => m.FlowChart_Version).Select(m => new { m.FlowChart_Version, m.FlowChart_Version_Comment, m.Modified_Date }).Distinct().ToList();
            //var flChartList = flowChartMasterRepository.QueryFLList(FlowChart_Master_UID);
            foreach (var detailVersionItem in detailVersionList)
            {
                var projectItem = masterItem.System_Project;
                var userItem = masterItem.System_Users; //userInfoList.Where(m => m.Account_UID == detailVersionItem).First();
                FlowChartHistoryGet historyItem = new FlowChartHistoryGet();
                historyItem.FlowChart_Master_UID = masterItem.FlowChart_Master_UID;
                historyItem.BU_D_Name = projectItem.System_BU_D.BU_D_Name;
                historyItem.FlowChart_Version = detailVersionItem.FlowChart_Version;
                historyItem.FlowChart_Version_Comment = detailVersionItem.FlowChart_Version_Comment;
                historyItem.Project_Name = projectItem.Project_Name;
                historyItem.Product_Phase = projectItem.Product_Phase;
                historyItem.Part_Types = masterItem.Part_Types;
                historyItem.User_Name = userItem.User_Name;
                historyItem.Modified_Date = detailVersionItem.Modified_Date;
                historyList.Add(historyItem);
            }
            return historyList;
        }

        public FlowChartExcelExport ExportFlowChart(int FlowChart_Master_UID, bool isTemp)
        {
            FlowChartExcelExport exportItem = new FlowChartExcelExport();

            var flMasterItem = flowChartMasterRepository.GetById(FlowChart_Master_UID);
            List<FlowChartDetailAndMGDataDTO> detailList = new List<FlowChartDetailAndMGDataDTO>();

            if (flMasterItem != null)
            {
                exportItem.BU_D_Name = flMasterItem.System_Project.System_BU_D.BU_D_Name;
                exportItem.Project_Name = flMasterItem.System_Project.Project_Name;
                exportItem.Part_Types = flMasterItem.Part_Types;
                exportItem.Product_Phase = flMasterItem.System_Project.Product_Phase;
                
                exportItem.SystemUserDTO = AutoMapper.Mapper.Map<SystemUserDTO>(flMasterItem.System_Users);
                exportItem.FlowChartDetailAndMGDataDTOList = detailList;
                if (isTemp)
                {
                    foreach (var item in flMasterItem.FlowChart_Detail_Temp)
                    {
                        FlowChartDetailAndMGDataDTO dtoItem = new FlowChartDetailAndMGDataDTO();
                        dtoItem = AutoMapper.Mapper.Map<FlowChartDetailAndMGDataDTO>(item);
                        dtoItem.PlantName = item.System_Function_Plant.FunPlant;

                        var mgDataTempItem = item.FlowChart_MgData_Temp.First();
                        dtoItem.Product_Plan = mgDataTempItem.Product_Plan;
                        dtoItem.Target_Yield = mgDataTempItem.Target_Yield;
                        detailList.Add(dtoItem);

                    }
                }
                else
                {
                    foreach (var item in flMasterItem.FlowChart_Detail)
                    {
                        FlowChartDetailAndMGDataDTO dtoItem = new FlowChartDetailAndMGDataDTO();
                        dtoItem = AutoMapper.Mapper.Map<FlowChartDetailAndMGDataDTO>(item);
                        dtoItem.PlantName = item.System_Function_Plant.FunPlant;

                        //var mgDataItem = item.FlowChart_MgData.First();
                        //dtoItem.Product_Plan = mgDataItem.Product_Plan;
                        //dtoItem.Target_Yield = mgDataItem.Target_Yield;
                        detailList.Add(dtoItem);
                    }
                }
            }

            return exportItem;
        }


        public PagedListModel<PrjectListVM> QueryFlowChartMasterDatas(int user_account_uid)
        {
            int totalCount = 0;
            var flowChartListDatas = flowChartMasterRepository.QueryFlowChartMasterDatas(user_account_uid, out totalCount);
            return new PagedListModel<PrjectListVM>(totalCount, flowChartListDatas);
        }

        public ProcessDataSearch QueryFlowChartDataByMasterUid(int flowChartMaster_uid)
        {

            var tempDataList = flowChartMasterRepository.QueryFlowChartDataByMasterUid(flowChartMaster_uid);
            ProcessDataSearch result = new ProcessDataSearch();
            foreach (var item in tempDataList)
            {
                result.Customer = item.Customer;
                result.Part_Types = item.Part_Types;
                result.Product_Phase = item.Product_Phase;
                result.Project = item.Project;
                result.Func_Plant = item.Func_Plant;
            }
            return result;
        }

        public bool ClosedFL(int id, bool isClosed)
        {
            var item = flowChartMasterRepository.GetById(id);
            if (isClosed)
            {
                item.Is_Closed = true;
            }
            else
            {
                item.Is_Closed = false;
            }
            flowChartMasterRepository.Update(item);
            unitOfWork.Commit();
            return true;
        }

        public FlowChartDetailGet QueryFLDetailByID(int id)
        {
            FlowChartDetailGet detailItem = new FlowChartDetailGet();
            var item = flowChartDetailRepository.GetById(id);
            detailItem.FlowChartDetailDTO = AutoMapper.Mapper.Map<FlowChartDetailDTO>(item);
            detailItem.FunPlant = item.System_Function_Plant.FunPlant;
            return detailItem;
        }

        public FlowChartDetailGetByMasterInfo QueryFunPlant(int id)
        {
            var masterItem = flowChartMasterRepository.GetById(id);

            FlowChartDetailGetByMasterInfo detailInfo = new FlowChartDetailGetByMasterInfo();
            detailInfo.BU_D_Name = masterItem.System_Project.System_BU_D.BU_D_Name;
            detailInfo.Project_Name = masterItem.System_Project.Project_Name;
            detailInfo.Part_Types = masterItem.Part_Types;
            detailInfo.Product_Phase = masterItem.System_Project.Product_Phase;

            List<SystemFunctionPlantDTO> dtoList = new List<SystemFunctionPlantDTO>();
            var list = systemFunctionPlantRepository.GetAll().OrderBy(m => m.FunPlant).ToList();
            dtoList = AutoMapper.Mapper.Map<List<SystemFunctionPlantDTO>>(list);
            detailInfo.SystemFunctionPlantDTOList = dtoList;
            return detailInfo;
        }

        public void SaveFLDetailInfo(FlowChartDetailDTO dto, int AccountID)
        {
            var item = flowChartDetailRepository.GetById(dto.FlowChart_Detail_UID);
            item.DRI = dto.DRI;
            item.Place = dto.Place;
            item.System_FunPlant_UID = dto.System_FunPlant_UID;
            //item.Product_Stage = dto.Product_Stage;
            item.Process_Desc = dto.Process_Desc;
            //item.Color = dto.Color;
            //item.Material_No = dto.Material_No;
            item.Modified_UID = AccountID;
            item.Modified_Date = DateTime.Now;
            flowChartDetailRepository.Update(item);
            unitOfWork.Commit();
        }

        public void SaveAllDetailInfo(List<FlowChartDetailAndMGDataInputDTO> list, int AccountID)
        {
            var idList = list.Select(m => m.FlowChart_Detail_UID).ToList();
            var mgDataList = flowChartMgDataRepository.GetMany(m => idList.Contains(m.FlowChart_Detail_UID)).ToList();

            foreach (var item in list)
            {
                var mgDataItem = mgDataList.Where(m => m.FlowChart_Detail_UID == item.FlowChart_Detail_UID).FirstOrDefault();
                if (mgDataItem != null)
                {
                    mgDataItem.Product_Plan = item.Product_Plan;
                    mgDataItem.Target_Yield = double.Parse(item.Target_Yield.Substring(0, item.Target_Yield.Length - 1)) / 100;
                    mgDataItem.Modified_Date = DateTime.Now;
                    mgDataItem.Modified_UID = AccountID;
                    flowChartMgDataRepository.Update(mgDataItem);
                }
            }
            unitOfWork.Commit();
        }

        public void DeleteFLTemp(int id)
        {
            var item = flowChartMasterRepository.GetById(id);
            if (item != null)
            {
                var detailList = flowChartDetailTempRepository.GetMany(m => m.FlowChart_Master_UID == item.FlowChart_Master_UID).ToList();
                var detailIDList = detailList.Select(m => m.FlowChart_DT_UID).ToList();
                var mgTempList = flowChartMgDataTempRepository.GetMany(m => detailIDList.Contains(m.FlowChart_DT_UID)).ToList();
                foreach (var mgTempItem in mgTempList)
                {
                    flowChartMgDataTempRepository.Delete(mgTempItem);
                }
                foreach (var detailItem in detailList)
                {
                    flowChartDetailTempRepository.Delete(detailItem);
                }
                unitOfWork.Commit();
            }
        }

        public void ImportFlowChartMGData(List<FlowChartMgDataDTO> mgDataList, int FlowChart_Master_UID)
        {
            //var maxVersion = flowChartDetailRepository.GetMany(m => m.FlowChart_Master_UID == FlowChart_Master_UID).Max(m => m.FlowChart_Version);
            //var detailIDList = flowChartDetailRepository.GetMany(m => m.FlowChart_Master_UID == FlowChart_Master_UID && m.FlowChart_Version == maxVersion).Select(m => m.FlowChart_Detail_UID).ToList();

            //flowChartMgDataRepository.Delete(q=> detailIDList.Contains(q.FlowChart_Detail_UID));

            //var dtoList = AutoMapper.Mapper.Map<IEnumerable<FlowChart_MgData>>(mgDataList);
            //foreach (var mgDataItem in dtoList)
            //{
            //    flowChartMgDataRepository.Add(mgDataItem);
            //}
            //unitOfWork.Commit();

            flowChartMasterRepository.BatchImportPlan(mgDataList, FlowChart_Master_UID);
        }

        /// <summary>
        /// 根据flowchart masteruid 查询计划数据
        /// </summary>
        /// <param name="masterUID"></param>
        /// <returns></returns>
        public  PagedListModel<FlPlanManagerVM> QueryProcessMGData(int masterUID,DateTime date)
        {
            var totalCount = 0;
          
            var flChartList = flowChartMasterRepository.QueryFlowMGData(masterUID, date, out totalCount);
            var result = new List<FlPlanManagerVM>();
           foreach (var item in flChartList)
            {
                var returnItem = new FlPlanManagerVM();
                returnItem.Detail_UID = item.Detail_UID;
                returnItem.Process_seq = item.Process_seq;
                returnItem.Process = item.Process;
                returnItem.date = item.date;
                returnItem.Color = item.Color;
                returnItem.MondayProduct_Plan = item.MondayProduct_Plan;
                if (item.MondayTarget_Yield!=null)
                {
                    returnItem.MondayTarget_Yield = item.MondayTarget_Yield * 100 + "%";
                }

                returnItem.TuesdayProduct_Plan = item.TuesdayProduct_Plan;
                if (item.TuesdayTarget_Yield != null)
                {
                    returnItem.TuesdayTarget_Yield = item.TuesdayTarget_Yield * 100 + "%";
                }

                returnItem.WednesdayProduct_Plan = item.WednesdayProduct_Plan;
                if (item.WednesdayTarget_Yield != null)
                {
                    returnItem.WednesdayTarget_Yield = item.WednesdayTarget_Yield * 100 + "%";
                }

                returnItem.ThursdayProduct_Plan = item.ThursdayProduct_Plan;
                if (item.ThursdayTarget_Yield != null)
                {
                    returnItem.ThursdayTarget_Yield = item.ThursdayTarget_Yield * 100 + "%";
                }

                returnItem.FridayProduct_Plan = item.FridayProduct_Plan;
                if (item.FridayTarget_Yield != null)
                {
                    returnItem.FridayTarget_Yield = item.FridayTarget_Yield * 100 + "%";
                }

                returnItem.SaterdayProduct_Plan = item.SaterdayProduct_Plan;
                if (item.SaterdayTarget_Yield != null)
                {
                    returnItem.SaterdayTarget_Yield = item.SaterdayTarget_Yield * 100 + "%";
                }

                returnItem.SundayProduct_Plan = item.SundayProduct_Plan;
                if (item.SundayTarget_Yield != null)
                {
                    returnItem.SundayTarget_Yield = item.SundayTarget_Yield * 100 + "%";
                }
                result.Add(returnItem);
           }

            return new PagedListModel<FlPlanManagerVM>(totalCount, result);
        }

        public  FlowChartPlanManagerVM QueryProcessMGDataSingle(int uid ,DateTime date)
        {
            return flowChartMasterRepository.QueryFlowMGDataSingle(uid,  date);
        }
        public string FlowChartPlan(FlowChartPlanManagerVM ent)
        {
            var items= flowChartMasterRepository.UpdatePlan(ent.Detail_UID,ent.date);
            int i = 0;

            foreach(var item in items)
            {
                i++;
                if(i==1&& ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.MondayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.MondayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 2 && ent.TuesdayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.TuesdayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.TuesdayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 3 && ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.WednesdayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.WednesdayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 4 && ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.ThursdayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.ThursdayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 5 && ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.FridayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.FridayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 6 && ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.SaterdayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.SaterdayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
                if (i == 7 && ent.MondayProduct_Plan != null)
                {
                    item.Product_Plan = int.Parse(ent.SundayProduct_Plan.ToString());
                    item.Target_Yield = double.Parse(ent.SundayTarget_Yield.ToString());
                    flowChartMgDataRepository.Update(item);
                }
               
               
            }
            unitOfWork.Commit();
            return "SUCCESS";
        }
    
    }
}
