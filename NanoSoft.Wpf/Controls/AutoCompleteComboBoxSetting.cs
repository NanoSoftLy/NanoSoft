using System;

namespace NanoSoft.Wpf.Controls
{
    public class AutoCompleteComboBoxSetting : DotNetKit.Windows.Controls.AutoCompleteComboBoxSetting
    {
        private readonly Action<string> _action;

        public AutoCompleteComboBoxSetting(Action<string> action)
        {
            _action = action;
        }

        /// <summary>
        /// Gets a filter function which determines whether items should be suggested or not
        /// for the specified query.
        /// Default: Gets the filter which maps an item to <c>true</c>
        /// if its text contains the query (case insensitive).
        /// </summary>
        /// <param name="query">
        /// The string input by user.
        /// </param>
        /// <param name="stringFromItem">
        /// The function to get a string which identifies the specified item.
        /// </param>
        /// <returns></returns>

        public override Func<object, bool> GetFilter(string query, Func<object, string> stringFromItem)
        {
            _action(query);
            return
                item =>
                    stringFromItem(item)
                    .IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }


        /// <summary>
        /// Gets an integer.
        /// The combobox opens the drop down
        /// if the number of suggested items is less than the value.
        /// Note that the value is larger, it's heavier to open the drop down.
        /// Default: 100.
        /// </summary>
        public override int MaxSuggestionCount => 100;


        /// <summary>
        /// Gets the duration to delay updating the suggestion list.
        /// Returns <c>Zero</c> if no delay.
        /// Default: 300ms.
        /// </summary>
        public override TimeSpan Delay => TimeSpan.FromMilliseconds(300.0);
    }
}