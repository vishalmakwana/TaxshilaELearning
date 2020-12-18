using TaxshilaMobile.DataTypesApp.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class User : IUser
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string UserId { get;  set; }
        public string SchoolName {  get;  set; }
        public string FatherName {  get;  set; }
        public string District {  get;  set; }
        public string Medium {  get;  set; }
        public string Std {  get;  set; }
        public string Group {  get;  set; }
        public int StandardId {  get;  set; }
        public string StandardName {  get;  set; }
        public int ClassId {  get;  set; }
        public string ClassName {  get;  set; }

        public bool AccessFoundationFeature { get; set; }
    }
}
