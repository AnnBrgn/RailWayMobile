using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace RailWayMobile.Behaviors
{
    public class EmailEntryBehavior : Behavior<Entry>
    {
        public static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
            var entry = (Entry)sender;
            ValidateEmail(entry, e.NewTextValue);
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            ValidateEmail(entry, entry.Text);
        }

        private void ValidateEmail(Entry entry, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                entry.TextColor = Colors.Black;
                return;
            }

            bool isValid = EmailRegex.IsMatch(text);
            entry.TextColor = isValid ? Colors.Black : Colors.Red;
        }
    }
}
