namespace bifeldy_sd3_mbz_60.Models {

    public class KafkaApprovalMessageHeader {
        public string NO_DOC { get; set; }
        public int NO_REF_DOC { get; set; }
        public DateTime? TGL_DOC { get; set; }
        public DateTime? TGL_REF_DOC { get; set; }
        public DateTime? TGL_REQ { get; set; }
        public string NAMA_PROGRAM { get; set; }
        public string NAMA_FITUR { get; set; }
        public string STATUS_APPROVAL { get; set; }
        public DateTime? TGL_APPROVAL { get; set; }
        public DateTime? TGL_EXP { get; set; }
        public KafkaApprovalMessageDetail DETAILS { get; set; }
    }

    public class KafkaApprovalMessageDetail {
        public int LEVEL { get; set; }
        public string STATUS_APPROVAL { get; set; }
        public DateTime? TGL_APPROVAL { get; set; }
        public string TYPE_APPROVAL { get; set; }
        public KafkaApprovalMessageDetailUser USER { get; set; }
    }

    public class KafkaApprovalMessageDetailUser {
        public string USERNAME { get; set; }
        public string EMAIL { get; set; }
        public string STATUS_APPROVAL { get; set; }
        public DateTime? TGL_APPROVAL { get; set; }
        public string NOTE_APPROVAL { get; set; }
    }

}
