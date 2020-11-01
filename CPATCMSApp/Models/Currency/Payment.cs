using CPATCMSApp.Models.CnFs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CPATCMSApp.Models.Currency
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public int CnFProfileId { get; set; }
        public CnFProfile CnFProfile { get; set; }

        public double Amount { get; set; }
        public string VerificationCode { get; set; }
        public string Name { get; set; }
        public string ReferenceCode { get; set; }

        [NotMapped]
        public string CnFProfileName { get; set; }

        [NotMapped]
        public string PaymentMethodName { get; set; }

        [NotMapped]
        public string PaymentTypeName { get; set; }
    }
}
