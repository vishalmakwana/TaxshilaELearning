using System;
using System.Collections.Generic;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models
{
    public class UnitModelDTO : ModelDTOBase
    {
        public string Title { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int MeasurementTypeId { get; set; }
        public List<ProductModelDTO> ProductsDTO { get; set; }
    }

    public class ProductModelDTO : ModelDTOBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? Status { get; set; }
        public string ItemCode { get; set; }
        public int CategorysId { get; set; }
        public int UnitsId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
    }

    public class CategoryModelDTO : ModelDTOBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public List<ProductModelDTO> ProductsDTO { get; set; }
    }

    public class StockInOutDTO : ModelDTOBase
    {
        public int StockInOutCategorysId { get; set; }
        public int StockInOutUnitId { get; set; }
        public int StockInOutProductId { get; set; }
        public int StockInOutOption { get; set; }
        public int StockCount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class UserInfoDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public DateTime? ValidTo { get; set; }
        public string SchoolName { get; set; }
        public string FatherName { get; set; }
        public string District { get; set; }
        public string Medium { get; set; }
        public string Std { get; set; }
        public string Group { get; set; }
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public bool IsLogin { get; set; }
        public string FullInfo { get; set; }
        public bool AccessFoundationFeature { get; set; }


    }

    public class MeasurementTypeDTO : ModelDTOBase
    {
        public string MeasurementTypeName { get; set; }
        public string Descriptions { get; set; }
    }

    public class VideoLectureDTO
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
        public string SubjectName { get; set; }
        public string StandardName { get; set; }
    }

    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string Subjectname { get; set; }
        public string OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string SubjectCode { get; set; }
        public string Descriptions { get; set; }
    }
    public class UserNoticeDTO
    {
        public int UserNoticeId { get; set; }
        public string NoticeTitle { get; set; }
        public DateTime NoticeDate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }
        public string NoticeFile { get; set; }
        public string NoticeImageUrl { get; set; }

    }
    public class PublicEventDTO
    {
        public int PublicEventId { get; set; }
        public string PublicEventTitle { get; set; }
        public DateTime PublicEventDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }
        public string PublicEventFile { get; set; }
        public string PublicEventImageUrl { get; set; }

    }

    public class HomeworkDTO
    {
        public int HomeworkId { get; set; }
        public string HomeWorkTitle { get; set; }
        public string OrganizationId { get; set; }
        public DateTime HomeWorkDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int StandardMasterId { get; set; }
        public int SubjectMasterId { get; set; }
        public int ClassId { get; set; }
        public string SubjectName { get; set; }
        public string StandardName { get; set; }
        public string studentClass { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }
        public string HomeWorkGroup { get; set; }
        public string HomeWorkFile { get; set; }
        public string HomeWorkFileURL { get; set; }
    }

    public class StudyMaterialsDTO
    {
        public int StudyMaterialId { get; set; }
        public string StudyMaterialTitle { get; set; }
        public DateTime StudyMaterialDate { get; set; }
        public string SubjectName { get; set; }
        public string StandardName { get; set; }
        public int StandardMasterId { get; set; }
        public int SubjectMasterId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
        public string Descriptions { get; set; }
        public string StudyMaterialFile { get; set; }
        public string StudyMaterialFileURL { get; set; }

    }

    public class FoundationVideoLectureDTO
    {
        public int FoundationVideoLectureId { get; set; }
        public string VideoLectureTitle { get; set; }
        public string OrganizationId { get; set; }
        public int StandardMasterId { get; set; }
        public string StandardName { get; set; }
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
    }
}