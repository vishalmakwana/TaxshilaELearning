using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TaxshilaMobile.Helpers
{
    public static class PageName
    {
        internal const string NavigationPage = "NavigationPage";
        internal const string MainPage = "MainPage";
        internal const string HomePage = "HomePage";
        internal const string AppMasterPage = "AppMasterPage";
        internal const string WalkthroughPage = "WalkthroughPage";
        internal const string TeacherDetailPage = "TeacherDetailPage";
        internal static string BaseMasterDetailPage = $"/{PageName.AppMasterPage}/{PageName.NavigationPage}";

        public const string NoInterNetConnectionPage = "NoInterNetConnectionPage";
        public const string IsReachableOrNotPage = "IsReachableOrNotPage";



        public static class LoginProcess
        {
            internal const string LoginPage = "LoginPage";
            internal const string RegisterPage = "RegisterPage";
        }
        public static class DoubtSolvingMaterial
        {
            internal const string DoubtSolvingMaterialPage = "DoubtSolvingMaterialPage";
            internal const string ShowDoubtSolvingMaterialPage = "ShowDoubtSolvingMaterialPage";
        }
        public static class Popup
        {
            internal const string UnitUpsertPopupPage = "UnitUpsertPopupPage";
            internal const string DefaultPickerPopup = "DefaultPickerPopup";
            internal const string ReadMoreDescriptionPopupPage = "ReadMoreDescriptionPopupPage";
        }

        public static class VideoLecture
        {
            internal const string VideoLectureListPage = "VideoLectureListPage";
            internal const string PlayVideoLecturePage = "PlayVideoLecturePage";
            internal const string VideoLectureTabbedPage = "VideoLectureTabbedPage";
            internal const string TodayVideoLecturePage = "TodayVideoLecturePage";

            internal const string FoundationVideoLectureTabbedPage = "FoundationVideoLectureTabbedPage";
            internal const string FoundationTodayLecturePage = "FoundationTodayLecturePage";
            internal const string FoundationHistoryLecturePage = "FoundationTodayLecturePage";
            internal const string PlayFoundationVideoLecturePage = "PlayFoundationVideoLecturePage";

        }
        public static class Notice
        {
            internal const string NoticeTabbedPage = "NoticeTabbedPage";
            internal const string NoticeBoardPage = "NoticeBoardPage";
            internal const string PublicEventNoticePage = "PublicEventNoticePage";
        }

        public static class UnitsInMeasure
        {
            internal const string UnitsPage = "UnitsPage";
            internal const string CreateUnitsPage = "CreateUnitsPage";
            internal const string UpdateUnitsPage = "UpdateUnitsPage";
        }

        public static class HomeworkAndStudyMatireal
        {
            internal const string HomeworkAndStudyMatirealTabbedPage = "HomeworkAndStudyMatirealTabbedPage";
            internal const string HomeWorkPage = "HomeWorkPage";
            internal const string StudyMatirealPage = "StudyMatirealPage";
        }


        public static class Categories
        {
            internal const string CategoriesPage = "CategoriesPage";
            internal const string CreateCategoryPage = "CreateCategoryPage";
            internal const string UpdateCategoryPage = "UpdateCategoryPage";
            internal const string CategoryStockReportPage = "CategoryStockReportPage";
        }
        public static class ManageStock
        {
            internal const string AddStockInPage = "AddStockIn";
            internal const string StockManagementPage = "StockManagementPage";
            internal const string GenerateStockReportPage = "GenerateStockReportPage";
        }
        public static class Products
        {
            internal const string ProductsPage = "ProductsPage";
            internal const string AddProductPage = "AddProductPage";
            internal const string UpdateProductPage = "UpdateProductPage";
        }
    }
    public static class Resources
    {
        public static string MaterialFontFamily
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return "Material Icons";

                    case Device.Android:
                        return "MaterialIcons-Regular.ttf#Material Icons";

                    case Device.UWP:
                        return "Assets/Fonts/MaterialIcons-Regular.ttf#Material Icons";

                    default:
                        return string.Empty;
                }
            }
        }

        public static string FontAwesomeFontFamily
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        return "Font Awesome 5 Free";

                    case Device.Android:
                        return "fa-solid-900.ttf#Font Awesome 5 Free";

                    case Device.UWP:
                        return "Assets/fa-solid-900.ttf#Font Awesome 5 Free";

                    default:
                        return string.Empty;
                }
            }
        }

    }

    public static class AlertMessages
    {

        public static string GoOnline = "Please Go online to use this functionality!";
        public static string NoRecordsOffline = "No Record Found.\nPlease go online to view full list.";
        public static string NoRecordsOnline = "No Record Found.";
        public static string UnderConstruction = "This Feature has been Currently Under Construction. Thank you for your patience";
        public static string DeleteSuccess = "Record delete successfully. Thank you ";
        public static string SaveSuccess = "Record Save successfully. Thank you ";

        public static string CategoryWarning = "This product already in used by one or more shade in list ! Please check again ";
        //public static string ProductWarning = "This Shade already in used by one or more product in list ! Please check again ";

        public static string UnitsWarning = "This unit already in used by one or more product in list ! Please check again ";
        public static string CategoryNotAvailabe = "There is not product created. Please create product first.";
        public static string NoInternet = "Currently, we do not have access to the INTERNET. Please connect to the Internet and try again or switch to off-line mode to continue.";
        public static string CanNotPlayBeforStarttimeVideoAlert = "Sorry!! Video can be played only on give time.You can watch it Later in History Tab";
        public static string YouAreNotEligible = "Sorry!! You are not eligible for foundation video lectures. Please contact to administrator";

        internal static class EmptyState
        {
            public static string DefaultTitle = "No results found!";
            public static string ConnectivityTitle = "Connectivity issue!";
            public static string DefaultSubtitle = "Try adjusting your search or filter to find what you're looking for";
            public static string ConnectivitySubtitle = "We can't connect to the server, please contact your system admin or try again later";
            public static string OfflineNotAvailableForResource = "Currently, we do not have access to the INTERNET. Please connect to the Internet and try again or switch to off-line mode to continue.";
        }
    }

    public static class Endpoint
    {
        public const string GetAllTeachers = "/apiv1/ApiAccount/GetAllTeachers";

        internal static class LoginEndpoint
        {
            public const string Authenticate = "/apiv1/ApiAccount/Authenticate";
            public const string Register = "/apiv1/ApiAccount/Register";
            public const string CreateToken = "/apiv1/ApiAccount/CreateToken";
            public const string LogOut = "/apiv1/ApiAccount/LogOut";
            public const string ValidateUser = "/apiv1/ApiAccount/ValidateUser";
            public const string CheckUserIsValid = "/apiv1/ApiAccount/CheckUserIsValid";
            public const string ValidateUserInfo = "/apiv1/ApiAccount/ValidateUserInfo";
        }
        internal static class VideoLectureEndPoint
        {
            public const string GetAllVideoLectures = "/apiv1/ApiVideoLecture/GetAllVideoLectures";
            public const string GetUserFoundationVideoLecture = "/apiv1/ApiFoundationClass/GetUserFoundationVideoLecture";


        }

        internal static class SubjectEndPoint
        {
            public const string GetStudentSubject = "/apiv1/ApiSubject/GetStudentSubject";

        }

        internal static class NoticeEndPoint
        {
            public const string GetStudentNotice = "/apiv1/ApiUserNotice/GetStudentNotice";
            public const string GetStudentPublicEvents = "/apiv1/ApiPublicEventNotice/GetStudentPublicEvents";
        }

        internal static class HomeworkAndStudyMaterialEndPoint
        {
            public const string GetStudentHomeWork = "/apiv1/ApiHomeWork/GetStudentHomeWork";
            public const string GetStudentStudyMaterials = "/apiv1/ApiStudyMaterial/GetStudentStudyMaterials";
        }
        internal static class CategoysEndpoint
        {
            public const string GetCategory = "/apiv2/HkEasyStock/GetCategory";
            public const string GetCategoryById = "/apiv2/HkEasyStock/GetCategoryById";
            public const string SaveCategory = "/apiv2/HkEasyStock/SaveCategory";
            public const string RemoveCategorys = "/apiv2/HkEasyStock/RemoveCategorys";
            public const string GetCategoryByIds = "/apiv2/HkEasyStock/GetCategoryByIds";
            public const string GetCategoryIds = "/apiv2/HkEasyStock/GetCategoryIds";

        }
        internal static class ProductsEndpoint
        {
            public const string GetProducts = "/apiv2/HkEasyStock/GetProducts";
            public const string GetProductById = "/apiv2/HkEasyStock/GetProductById";
            public const string SaveProduct = "/apiv2/HkEasyStock/SaveProduct";
            public const string RemoveProducts = "/apiv2/HkEasyStock/RemoveProducts";
            public const string GetProductsByIds = "/apiv2/HkEasyStock/GetProductsByIds";
            public const string GetProductIds = "/apiv2/HkEasyStock/GetProductIds";

        }

        internal static class UnitsEndpoint
        {
            public const string GetUnits = "/apiv2/HkEasyStock/GetUnits";
            public const string GetUnitById = "/apiv2/HkEasyStock/GetUnitById";
            public const string SaveUnit = "/apiv2/HkEasyStock/SaveUnit";
            public const string RemoveUnit = "/apiv2/HkEasyStock/RemoveUnit";
            public const string GetUnitByIds = "/apiv2/HkEasyStock/GetUnitByIds";
            public const string GetUnitIds = "/apiv2/HkEasyStock/GetUnitIds";
        }


        internal static class Sync
        {
            public const string GetUnitsSync = "/apiv2/Sync/GetUnitsSync";

        }


    }
}
