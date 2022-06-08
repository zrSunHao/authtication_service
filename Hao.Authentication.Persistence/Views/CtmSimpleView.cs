using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Persistence.Views
{
    public class CtmSimpleView
    {
        public string Id { get; set; }

        public string? Avatar { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string? Intro { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
