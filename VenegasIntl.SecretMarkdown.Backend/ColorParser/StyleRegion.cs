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

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if the objects are equals.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            StyleRegion other = obj as StyleRegion;
            return this.ColorStyle == other.ColorStyle
                && this.StartPosition == other.StartPosition
                && this.Length == other.Length;
        }

        /// <summary>
        /// Return the hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return this.ColorStyle.GetHashCode() ^ this.StartPosition.GetHashCode() ^ this.Length.GetHashCode();
        }
    }
}
