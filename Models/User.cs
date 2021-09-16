using System;

namespace Holism.Accounts.Models
{
    public class User : Holism.Models.IEntity
    {
        public User()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public Guid KeycloakGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid? ProfilePictureGuid { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
