using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLib
{
    public class Bank
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<ATM> ATMList { get; set; } = new List<ATM>();

        public string SupportPhone { get; set; }
    }
}
