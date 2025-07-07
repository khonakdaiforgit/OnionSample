using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Publisher { get; private set; }
        public Guid UserId { get; private set; } // ارتباط با کاربر

        public Book(string title, string publisher, Guid userId)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            UserId = userId;
        }
    }
}
