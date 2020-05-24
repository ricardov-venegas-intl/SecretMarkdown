// <copyright file="NativeMethods.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace VenegasIntl.SecretMarkdown.UI
{
    /// <summary>
    /// PInvoke methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The LockWindowUpdate function disables or enables drawing in the specified window. Only one window can be locked at a time.
        /// </summary>
        /// <param name="hWndLock">Target Windpows Handle.</param>
        /// <returns>True if the function succeed.</returns>
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);
    }
}
