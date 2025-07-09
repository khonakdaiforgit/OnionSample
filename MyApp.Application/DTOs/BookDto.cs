using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get;  set; }
        public string Title { get;  set; }
        public string Publisher { get;  set; }
        public Guid UserId { get;  set; } // ارتباط با کاربر
    }
}
