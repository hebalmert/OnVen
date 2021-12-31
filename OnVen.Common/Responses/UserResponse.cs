using OnVen.Common.Entities;
using OnVen.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnVen.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ImageId { get; set; }

        public string ImageFullPath => ImageId == string.Empty
            ? $"http://onven.linkonext.com/Images/noimage.png"
            : $"http://onven.linkonext.com/Users/{ImageId}";

        public UserType UserType { get; set; }

        public City City { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }

}
