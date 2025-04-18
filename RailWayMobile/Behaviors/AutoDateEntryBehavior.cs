using System.Globalization;
namespace RailWayMobile.Behaviors
{

    public class AutoDateEntryBehavior : Behavior<Entry>
    {
        private bool _isFormatting;
        private string _previousText = "";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (_isFormatting) return;

            var entry = (Entry)sender;
            var newText = args.NewTextValue;
            var oldText = args.OldTextValue;

            // Handle backspace
            if (newText.Length < oldText?.Length)
            {
                _previousText = newText;
                return;
            }

            _isFormatting = true;

            try
            {
                // Remove all non-digit characters
                var digitsOnly = new string(newText.Where(char.IsDigit).ToArray());

                // Auto-insert dots at appropriate positions
                string formattedText = digitsOnly.Length switch
                {
                    > 0 and <= 2 => digitsOnly, // Just day
                    > 2 and <= 4 => $"{digitsOnly[..2]}.{digitsOnly[2..]}", // Day.Month
                    > 4 => $"{digitsOnly[..2]}.{digitsOnly.Substring(2, 2)}.{digitsOnly[4..]}", // Day.Month.Year
                    _ => digitsOnly
                };

                // Limit year to 4 digits
                if (digitsOnly.Length > 8)
                {
                    formattedText = formattedText[..10]; // dd.mm.yyyy is 10 chars
                }

                // Only update if changed to prevent cursor jumping
                if (formattedText != newText)
                {
                    entry.Text = formattedText;
                }

                // Validate the date
                bool isValid = DateTime.TryParseExact(formattedText, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

                entry.TextColor = isValid ? Colors.Black : Colors.Red;
                _previousText = formattedText;
            }
            finally
            {
                _isFormatting = false;
            }
        }
    }
}