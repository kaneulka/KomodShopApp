using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PaymentMethodModels
{
    public class PaymentMethodsViewModel
    {
        public IEnumerable<PaymentMethodViewModel> PaymentMethods { get; set; }
        public PageViewModel PageViewModel { get; set; }

    }
}
