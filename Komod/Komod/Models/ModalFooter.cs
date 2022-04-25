using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Принять";
        public string CancelButtonText { get; set; } = "Отмена";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
        public bool OnlySubmitButton { get; set; }
    }
}
