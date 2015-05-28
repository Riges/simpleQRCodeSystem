using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQRCodeSystem.Model
{
    class Badge
    {
        public int Id { get; set; }

        public string code { get; set; }
        public bool Used { get; set; }

        public Badge()
        {
            this.Used = false;
            this.code = "";
        }
    }
}
