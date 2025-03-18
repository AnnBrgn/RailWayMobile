using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWayMobile.ClassesDTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Lastname { get; set; }

        public string? Patronymic { get; set; }

        public string? Email { get; set; }

        public string? NumberPhone { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Gender { get; set; }

        public string? SeriesPassport { get; set; }

        public string? NumberPassport { get; set; }

    }
}
