using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactForm.Sample.Postgres.Models
{
    public class ContactModelData: ContactModel
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }

        public ContactModelData()
        {
        }

        public ContactModelData(ContactModel c)
        {
            Id = Guid.NewGuid();
            ContactName = c.ContactName;
            Category = c.Category;
            Email = c.Email;
            Message = c.Message;
            Phone = c.Phone;
            Subject = c.Subject;
        }
    }
}
