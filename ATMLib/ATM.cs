using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMLib
{
    public class ATM
    {
        public long Id { get; set; }

        public decimal ATMBalance { get; set; }

        public string Adress { get; set; }

        public Bank Bank { get; set; }

        public long BankId { get; set; }

    }
}
