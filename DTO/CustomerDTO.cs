using System.Runtime.Serialization;
using System.Xml.Linq;

namespace DCEBackEnd.DTO
{
    public class CustomerDTO
    {
        //[DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "LastName")]
        public string LastName { get; set; }


        [DataMember(Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
}
