using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Helper
{
    public class ContactDal :Query<Contact> , IContactDal
    {
    }
}