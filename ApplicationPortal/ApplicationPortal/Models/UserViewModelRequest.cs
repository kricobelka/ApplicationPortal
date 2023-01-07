﻿namespace ApplicationPortal.Models
{
    public class UserViewModelRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}
