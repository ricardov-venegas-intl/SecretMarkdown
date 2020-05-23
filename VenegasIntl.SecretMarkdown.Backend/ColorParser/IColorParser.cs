// <copyright file="IColorParser.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace VenegasIntl.SecretMarkdown.Backend.ColorParser
{
    /// <summary>
    /// Color Parser Interface.
    /// </summary>
    public interface IColorParser
    {
        /// <summary>
        /// Parse the text into color regions.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>Enumeration of the Style regions.</returns>
        IEnumerable<StyleRegion> Parse(string text);
    }
}
