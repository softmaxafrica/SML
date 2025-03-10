using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class CompanyCustomer
    {
        [Key]
        public int TXNID { get; set; }

        public DateTime CreationDate { get; set; }
        public string CustomerId { get; set; }   
        public string CompanyId { get; set; }    
        

         public Customer Customer { get; set; }  
         public Company Company { get; set; }
    }
}
