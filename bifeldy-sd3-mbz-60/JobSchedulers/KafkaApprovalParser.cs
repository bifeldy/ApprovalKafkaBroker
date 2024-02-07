using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Quartz;

using bifeldy_sd3_lib_60.Databases;
using bifeldy_sd3_lib_60.Models;
using bifeldy_sd3_lib_60.Repositories;
using bifeldy_sd3_lib_60.Services;
using bifeldy_sd3_lib_60.Tables;

using bifeldy_sd3_mbz_60.Models;
using bifeldy_sd3_mbz_60.Tables;

namespace bifeldy_sd3_mbz_60.JobSchedulers {

    [DisallowConcurrentExecution]
    public sealed class KafkaApprovalParser : IJob {

        private readonly ILogger<KafkaApprovalParser> _logger;
        private readonly IOraPg _orapg;
        private readonly IConverterService _converter;
        private readonly IGeneralRepository _generalRepo;

        public KafkaApprovalParser(
            ILogger<KafkaApprovalParser> logger,
            IOptions<EnvVar> envVar,
            IOraPg orapg,
            IConverterService converter,
            IGeneralRepository generalRepo
        ) {
            _logger = logger;
            _orapg = orapg;
            _generalRepo = generalRepo;
            _converter = converter;
        }

        public async Task Execute(IJobExecutionContext _context) {
            try {
                string kodeDc = await _generalRepo.GetKodeDc();
                string topicName = "web_approval_" + kodeDc;
                List<KAFKA_CONSUMER_AUTO_LOG> ls = await _orapg.Set<KAFKA_CONSUMER_AUTO_LOG>().Where(
                    kcal => kcal.TPC == topicName
                ).OrderBy(kcal => kcal.OFFS).ToListAsync();
                foreach (KAFKA_CONSUMER_AUTO_LOG l in ls) {
                    KafkaApprovalMessageHeader kamh = _converter.JsonToObject<KafkaApprovalMessageHeader>(l.VAL);
                    KAFKA_APPROVAL_T kat = new KAFKA_APPROVAL_T {
                        KODE_DC = kodeDc,
                        // --
                        HEADER_NO_DOC = kamh.NO_DOC,
                        HEADER_NO_REF_DOC = kamh.NO_REF_DOC,
                        HEADER_TGL_DOC = kamh.TGL_DOC,
                        HEADER_TGL_REF_DOC = kamh.TGL_REF_DOC,
                        HEADER_TGL_REQ = kamh.TGL_REQ,
                        HEADER_NAMA_PROGRAM = kamh.NAMA_PROGRAM,
                        HEADER_NAMA_FITUR = kamh.NAMA_FITUR,
                        HEADER_STATUS_APPROVAL = kamh.STATUS_APPROVAL,
                        HEADER_TGL_APPROVAL = kamh.TGL_APPROVAL,
                        HEADER_TGL_EXP = kamh.TGL_EXP,
                        // --
                        DETAIL_LEVEL = kamh.DETAILS.LEVEL,
                        DETAIL_STATUS_APPROVAL = kamh.DETAILS.STATUS_APPROVAL,
                        DETAIL_TGL_APPROVAL = kamh.DETAILS.TGL_APPROVAL,
                        DETAIL_TYPE_APPROVAL = kamh.DETAILS.TYPE_APPROVAL,
                        // --
                        USER_USERNAME = kamh.DETAILS.USER.USERNAME,
                        USER_EMAIL = kamh.DETAILS.USER.EMAIL,
                        USER_STATUS_APPROVAL = kamh.DETAILS.USER.STATUS_APPROVAL,
                        USER_TGL_APPROVAL = kamh.DETAILS.USER.TGL_APPROVAL,
                        USER_NOTE_APPROVAL = kamh.DETAILS.USER.NOTE_APPROVAL,
                    };
                    try {
                        await _orapg.Set<KAFKA_APPROVAL_T>().AddAsync(kat);
                        await _orapg.SaveChangesAsync();
                    }
                    catch (Exception e) {
                        _orapg.Set<KAFKA_APPROVAL_T>().Remove(kat);
                        _logger.LogError($"[{_context.Scheduler.SchedulerName}_DATA_FAIL] ⌚ {e.Message}");
                    }
                    try {
                        _orapg.Set<KAFKA_CONSUMER_AUTO_LOG>().Remove(l);
                        await _orapg.SaveChangesAsync();
                    }
                    catch (Exception e) {
                        _logger.LogError($"[{_context.Scheduler.SchedulerName}_LOG_FAIL] ⌚ {e.Message}");
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError($"[{_context.Scheduler.SchedulerName}_ERROR] ⌚ {ex.Message}");
            }
        }

    }

}
