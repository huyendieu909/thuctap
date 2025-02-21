using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HoangXuanQuy.OnlinePainting.Data.Models
{
    public class Customer : IdentityUser
    {
        //Kế thừa Identity thì Key là String Guid

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        //Identity có sẵn Email và PhoneNumber

        public string? Address { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
