// <copyright file="SimpleMarkdownColorParser.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace VenegasIntl.SecretMarkdown.Backend.ColorParser
{
    /// <summary>
    /// Implements a simple markdown color parser.
    /// </summary>
    public class SimpleMarkdownColorParser : IColorParser
    {
        private const string TokensRegularExpression = @"^#\s.*$|^##\s.*$|^###\s.*$|^\s*([\*\+\-])\s.*$|^\s*(\d+\.?)\s.*$";

        private Regex markdownTokensRegularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMarkdownColorParser"/> class.
        /// </summary>
        public SimpleMarkdownColorParser()
        {
            this.markdownTokensRegularExpression = new Regex(TokensRegularExpression, RegexOptions.Multiline);
        }

        /// <summary>
        /// Parse the markdown text into color regions.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>Enumeration of the Style regions.</returns>
        public IEnumerable<StyleRegion> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            MatchCollection tokenMatches = this.markdownTokensRegularExpression.Matches(text);
            int currentLocation = 0;

            foreach (Match match in tokenMatches)
            {
                int selectionStartIndex = match.Index;
                int selectionLenght = match.Length;

                // Check if we have a normal region before the current token.
                if (selectionStartIndex > currentLocation)
                {
                    yield return new StyleRegion
                    {
                        ColorStyle = ColorStyle.Normal,
                        StartPosition = currentLocation,
                        Length = selectionStartIndex - currentLocation,
                    };
                }

                if (match.Value.StartsWith("###", StringComparison.OrdinalIgnoreCase) == true)
                {
                    yield return new StyleRegion
                    {
                        ColorStyle = ColorStyle.Header3,
                        StartPosition = selectionStartIndex,
                        Length = selectionLenght,
                    };
                }
                else if (match.Value.StartsWith("##", StringComparison.OrdinalIgnoreCase) == true)
                {
                    yield return new StyleRegion
                    {
                        ColorStyle = ColorStyle.Header2,
                        StartPosition = selectionStartIndex,
                        Length = selectionLenght,
                    };
                }
                else if (match.Value.StartsWith("#", StringComparison.OrdinalIgnoreCase) == true)
                {
                    yield return new StyleRegion
                    {
                        ColorStyle = ColorStyle.Header1,
                        StartPosition = selectionStartIndex,
                        Length = selectionLenght,
                    };
                }
                else
                {
                    if (match.Groups[1].Length > 0)
                    {
                        selectionStartIndex = match.Groups[1].Index;
                        selectionLenght = match.Groups[1].Length;
                    }
                    else
                    {
                        selectionStartIndex = match.Groups[2].Index;
                        selectionLenght = match.Groups[2].Length;
                    }

                    yield return new StyleRegion
                    {
                        ColorStyle = ColorStyle.ListPrefix,
                        StartPosition = selectionStartIndex,
                        Length = selectionLenght,
                    };
                }

                // Update current position to the end of the region|* He
                currentLocation = selectionStartIndex + selectionLenght;
            }

            // Check if we have a normal region after the last token.
            if (currentLocation < text.Length)
            {
                yield return new StyleRegion
                {
                    ColorStyle = ColorStyle.Normal,
                    StartPosition = currentLocation,
                    Length = text.Length - currentLocation,
                };
            }
        }
    }
}
