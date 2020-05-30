// <copyright file="Program.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VenegasIntl.SecretMarkdown.Backend.ColorParser;
using VenegasIntl.SecretMarkdown.Backend.Encryptor;
using VenegasIntl.SecretMarkdown.Backend.Repositories;
using VenegasIntl.SecretMarkdown.UI.Forms;

namespace VenegasIntl.SecretMarkdown.UI
{
    /// <summary>
    /// Program Entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var servicesCollection = new ServiceCollection();
            var serviceProvider = servicesCollection
                .AddScoped<IColorParser, SimpleMarkdownColorParser>()
                .AddScoped<ITextEncryptor, AesTextEncryptor>()
                .AddScoped<INotesRepository, NotesRepository>()
                .AddScoped<MainForm>()
                .AddSingleton<IConfiguration>(configuration)
                .BuildServiceProvider();

            using (var mainForm = serviceProvider.GetService<MainForm>())
            {
                Application.Run(mainForm);
            }
        }
    }
}
