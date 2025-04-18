using System.Text.RegularExpressions;

namespace RailWayMobile.Behaviors
{
    public class PhoneNumberEntryBehavior : Behavior<Entry>
    {
        private string _previousText = string.Empty;
        private bool _isFormatting;

        // Russian phone number regex (adjust for your needs)
        public const string PhoneRegexPattern = @"^(\+7|8)?[\s-]?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";
        public static readonly Regex PhoneRegex = new Regex(PhoneRegexPattern, RegexOptions.Compiled);

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            entry.Unfocused += OnEntryUnfocused;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            entry.Unfocused -= OnEntryUnfocused;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormatting) return;

            var entry = (Entry)sender;
            var newText = e.NewTextValue ?? string.Empty;
            var oldText = e.OldTextValue ?? string.Empty;

            // Handle deletion
            if (newText.Length < oldText.Length)
            {
                _previousText = newText;
                return;
            }

            _isFormatting = true;

            try
            {
                // Remove all non-digit characters
                var digitsOnly = new string(newText.Where(char.IsDigit).ToArray());

                // Format based on input length
                string formattedText = digitsOnly.Length switch
                {
                    0 => string.Empty,
                    1 when digitsOnly[0] == '7' => "+7 (",
                    1 when digitsOnly[0] == '8' => "8 (",
                    1 => "+7 (" + digitsOnly,
                    <= 4 => $"+7 ({digitsOnly[1..Math.Min(digitsOnly.Length, 4)]}",
                    <= 7 => $"+7 ({digitsOnly.Substring(1, 3)}) {digitsOnly[4..Math.Min(digitsOnly.Length, 7)]}",
                    <= 9 => $"+7 ({digitsOnly.Substring(1, 3)}) {digitsOnly.Substring(4, 3)}-{digitsOnly[7..Math.Min(digitsOnly.Length, 9)]}",
                    _ => $"+7 ({digitsOnly.Substring(1, 3)}) {digitsOnly.Substring(4, 3)}-{digitsOnly.Substring(7, 2)}-{digitsOnly.Substring(9, Math.Min(digitsOnly.Length - 9, 2))}"
                };

                // Update text if changed
                if (formattedText != newText)
                {
                    entry.Text = formattedText;
                }

                // Validate
                bool isValid = PhoneRegex.IsMatch(formattedText);
                entry.TextColor = isValid ? Colors.Black : Colors.Red;

                _previousText = formattedText;
            }
            finally
            {
                _isFormatting = false;
            }
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            if (!PhoneRegex.IsMatch(entry.Text ?? string.Empty))
            {
                entry.TextColor = Colors.Red;
            }
        }
    }
}