using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Domain.DataContracts {
    public class NewBankDetailDataContract {
        public string Name { get; set; }   
        
        public long AccountNo { get; set; }     

        public string Address { get; set; }      

        public string SwiftBic { get; set; }       

        public string Iban { get; set; }    

        public string VatNumber { get; set; }
    }
}
