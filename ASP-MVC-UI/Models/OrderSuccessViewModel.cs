using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_MVC_UI.Models
{
    public class OrderSuccessViewModel
    {
        public IEnumerable<Address> Addresses { get; set; }
        public int CustomerRating { get; set; }
    }

}