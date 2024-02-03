using System.ComponentModel.DataAnnotations;

using bifeldy_sd3_lib_60.Abstractions;

namespace bifeldy_sd3_mbz_60.Tables {

    public sealed class KAFKA_APPROVAL_T : EntityTable {
        public string KODE_DC { get; set; }
        // --
        [Key] public string HEADER_NO_DOC { get; set; }
        public DateTime? HEADER_TGL_DOC { get; set; }
        public DateTime? HEADER_TGL_REQ { get; set; }
        [Key] public string HEADER_NAMA_PROGRAM { get; set; }
        [Key] public string HEADER_NAMA_FITUR { get; set; }
        public string HEADER_STATUS_APPROVAL { get; set; }
        public DateTime? HEADER_TGL_APPROVAL { get; set; }
        public DateTime? HEADER_TGL_EXP { get; set; }
        // --
        public int? DETAIL_LEVEL { get; set; }
        public string DETAIL_STATUS_APPROVAL { get; set; }
        public DateTime? DETAIL_TGL_APPROVAL { get; set; }
        public string DETAIL_TYPE_APPROVAL { get; set; }
        // --
        public string USER_USERNAME { get; set; }
        [Key] public string USER_EMAIL { get; set; }
        public string USER_STATUS_APPROVAL { get; set; }
        public DateTime? USER_TGL_APPROVAL { get; set; }
        public string USER_NOTE_APPROVAL { get; set; }
    }

}
