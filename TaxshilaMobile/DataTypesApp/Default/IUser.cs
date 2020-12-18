using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.DataTypesApp.Default
{
    public interface IUser
    {
        string FullName { get; set; }
        string Password { get; set; }
        string Username { get; set; }
        string CellPhone { get; set; }
        string Email { get; set; }
        string City { get; set; }
        string State { get; set; }
        string UserId { get; set; }
       
        string SchoolName { get; set; }
        string FatherName { get; set; }
        string District { get; set; }
        string Medium { get; set; }
        string Std { get; set; }
        string Group { get; set; }
        int StandardId { get; set; }
        string StandardName { get; set; }
        int ClassId { get; set; }
        string ClassName { get; set; }
         bool AccessFoundationFeature { get; set; }

    }
}
