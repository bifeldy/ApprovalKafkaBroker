using Microsoft.Extensions.Options;

using Quartz;

using bifeldy_sd3_lib_60.Databases;
using bifeldy_sd3_lib_60.Models;

namespace bifeldy_sd3_mbz_60.JobSchedulers {

    public sealed class JobExample : IJob {

        private readonly ILogger<JobExample> _logger;
        private readonly EnvVar _envVar;
        private readonly IOraPg _orapg;

        public JobExample(
            ILogger<JobExample> logger,
            IOptions<EnvVar> envVar,
            IOraPg orapg
        ) {
            _logger = logger;
            _envVar = envVar.Value;
            _orapg = orapg;
        }

        public async Task Execute(IJobExecutionContext _context) {
            try {
                DateTime dt = await _orapg.ExecScalarAsync<DateTime>($@"SELECT {(_envVar.IS_USING_POSTGRES ? "NOW()" : "SYSDATE FROM DUAL")}");
                _logger.LogInformation($"Tanggal & Waktu Database => {dt}");
                Console.WriteLine(dt.ToString());
            }
            catch (Exception ex) {
                _logger.LogError($"{_context.Scheduler.SchedulerName} {ex.Message}");
            }
        }

    }

}
