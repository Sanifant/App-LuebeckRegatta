using System;
using System.Collections.Generic;
using System.Text;

namespace de.openelp.regatta.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PageItem(string header, Func<object> factory, string? iconData = null)
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public string Header { get; } = header;

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        public Func<object> Factory { get; } = factory;

        /// <summary>
        /// Gets the icon data.
        /// </summary>
        /// <value>
        /// The icon data.
        /// </value>
        public string? IconData { get; } = iconData;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PageItem"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="PageItem"/> is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; } = true;
    }
}
