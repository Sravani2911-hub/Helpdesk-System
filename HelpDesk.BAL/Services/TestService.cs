using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDesk.DAL.Models;

namespace HelpDesk.BAL.Services
{
    public class TestService
    {
        public Sample GetSample()
        {
            return new Sample
            {
                Id = 1
            };
        }
    }
}
