namespace RailWayMobile.ClassesDTO
{
    public class UserDTO : ICloneable
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Lastname { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? Patronymic { get; set; }

        public string? Email { get; set; }

        public string? NumberPhone { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Gender { get; set; }

        public string? SeriesPassport { get; set; }

        public string? NumberPassport { get; set; }

        public string? Token { get; set; }

        public byte[]? Image { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone(); // поверхностное копирование
        }
    }
}
