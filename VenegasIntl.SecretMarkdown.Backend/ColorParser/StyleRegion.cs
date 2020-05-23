// <copyright file="StyleRegion.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace VenegasIntl.SecretMarkdown.Backend.ColorParser
{
    /// <summary>
    /// Data about a style region.
    /// </summary>
    public class StyleRegion
    {
        /// <summary>
        /// Gets or sets the color style of the region.
        /// </summary>
        public ColorStyle ColorStyle { get; set; }

        /// <summary>
        /// Gets or sets the start position of the region.
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// Gets or sets the length of the region.
        /// </summary>
        public int Length { get; set; }
    }
}
