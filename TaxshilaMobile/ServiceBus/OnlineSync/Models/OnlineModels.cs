using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OnlineSync.Models
{
    public class ClassModel
    {

        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }

    }

    public class StandardModel
    {
        
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }
        public ICollection<VideoLecturesMaster> VideoLectures { get; set; }      

    }
    public class SubjectMaster
    {
       
        public int SubjectId { get; set; }
        public string Subjectname { get; set; }
        public string OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public string SubjectCode { get; set; }
        public string Descriptions { get; set; }
        public ICollection<VideoLecturesMaster> VideoLectures { get; set; }
       

    }

    public class VideoLecturesMaster
    {
        
        public int VideoLectureId { get; set; }
        public string VideoLectureName { get; set; }

        public int StandardMasterId { get; set; }

        public int SubjectMasterId { get; set; }

        public DateTime PublishDate { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }
        public string link3 { get; set; }
        public string link4 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public StandardModel standardMaster { get; set; }
        public SubjectMaster subjectMaster { get; set; }

    }
}
