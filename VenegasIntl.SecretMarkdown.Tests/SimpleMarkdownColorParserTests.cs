// <copyright file="SimpleMarkdownColorParserTests.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenegasIntl.SecretMarkdown.Backend.ColorParser;

namespace VenegasIntl.SecretMarkdown.Tests
{
    /// <summary>
    /// Simple Markdown Color Parser Tests.
    /// </summary>
    [TestClass]
    public class SimpleMarkdownColorParserTests
    {
        /// <summary>
        /// Parse a text with only header 1.
        /// </summary>
        [TestMethod]
        public void Header1Only()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "# Header 1";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(ColorStyle.Header1, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(text.Length, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only header 2.
        /// </summary>
        [TestMethod]
        public void Header2Only()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "## Header 2";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(ColorStyle.Header2, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(text.Length, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only header 3.
        /// </summary>
        [TestMethod]
        public void Header3Only()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "### Header 3";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(ColorStyle.Header3, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(text.Length, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only a list bullet.
        /// </summary>
        [TestMethod]
        public void ListOnly()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "* ";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(ColorStyle.ListPrefix, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(1, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only a list number.
        /// </summary>
        [TestMethod]
        public void ListNoPeriodOnly()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "1 ";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(ColorStyle.ListPrefix, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(1, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only a list number with period.
        /// </summary>
        [TestMethod]
        public void ListWithPeriodOnly()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "1. ";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(ColorStyle.ListPrefix, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(2, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with only normal text.
        /// </summary>
        [TestMethod]
        public void NormalOnly()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = "Hello World";
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(ColorStyle.Normal, tokens[0].ColorStyle);
            Assert.AreEqual(0, tokens[0].StartPosition);
            Assert.AreEqual(text.Length, tokens[0].Length);
        }

        /// <summary>
        /// Parse a text with multiple styles.
        /// </summary>
        [TestMethod]
        public void Complex()
        {
            IColorParser parser = new SimpleMarkdownColorParser();
            var textComponents = new string[]
            {
                "# Header",
                "\nthis is normal text\n",
                "## header 2",
                "\n",
                "*",
                " list 1\n",
                "1",
                " list 2\n",
                "### header 3",
                "\n",
            };

            var styles = new ColorStyle[]
            {
                ColorStyle.Header1,
                ColorStyle.Normal,
                ColorStyle.Header2,
                ColorStyle.Normal,
                ColorStyle.ListPrefix,
                ColorStyle.Normal,
                ColorStyle.ListPrefix,
                ColorStyle.Normal,
                ColorStyle.Header3,
                ColorStyle.Normal,
            };

            var text = string.Concat(textComponents);
            var tokens = parser.Parse(text).ToList();

            // Test number of tokens
            Assert.AreEqual(textComponents.Length, tokens.Count);

            // Test each token
            int currentPosition = 0;
            for (int i = 0; i < textComponents.Length; i++)
            {
                Assert.AreEqual(styles[i], tokens[i].ColorStyle);
                Assert.AreEqual(currentPosition, tokens[i].StartPosition);
                Assert.AreEqual(textComponents[i].Length, tokens[i].Length);
                currentPosition += textComponents[i].Length;
            }

            // Verify currentPosition final value
            Assert.AreEqual(text.Length, currentPosition);
        }

        /// <summary>
        /// Parse a text with only header 1.
        /// </summary>
        [TestMethod]
        public void Empty()
        {
            IColorParser parser = new SimpleMarkdownColorParser();

            var text = string.Empty;
            var tokens = parser.Parse(text).ToList();
            Assert.AreEqual(0, tokens.Count);
        }

        /// <summary>
        /// Verify argument validation.
        /// </summary>
        [TestMethod]
        public void VerifyValidation()
        {
            IColorParser parser = new SimpleMarkdownColorParser();
            Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    _ = parser.Parse(null).ToList();
                });
        }
    }
}
