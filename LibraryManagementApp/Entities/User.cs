﻿using LibraryManagementApp.Entities;

namespace LibraryManagementApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string AadharNo { get; set; }
        public List<BookAllocation> BooksAllocated { get; set; }
    }
}
