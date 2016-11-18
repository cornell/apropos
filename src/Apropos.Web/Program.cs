﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using Apropos.Domain;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Apropos.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Apropos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.ObjectPool;
using ImageSharp;

namespace Apropos.Web
{
    public class Program
    {
        private static ILogger _logger;

        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            ArticleService service = host.Services.GetService(typeof(ArticleService)) as ArticleService;
            IHostingEnvironment env = host.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            ILoggerFactory loggerFactory = host.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            _logger = loggerFactory.CreateLogger<Program>();
            StartupDomainApplication(service, env);
            //Process.Start("gulp", "htmltopdf");

            host.Run();
        }

        private static void StartupDomainApplication(ArticleService service, IHostingEnvironment env)
        {
            var services = new ServiceCollection();
            services.AddTransient<RazorViewToStringRenderer>();

            var articles = service.GetArticles(Axe.Formation);
            int i = 1;
            int nombreArticles = articles.Count;
            articles.ForEach(article =>
            {
                DirectoryInfo repertoireDest = CreerRepertoireArticle(article, env);
                CreerFormationsPdf(article, repertoireDest, env);
                _logger.LogInformation($"{i++}/{nombreArticles}: {article.Titre}");
                CopierImages(article, repertoireDest);
            });            
        }

        private static void CopierImages(Article article, DirectoryInfo repertoireDest)
        {
            string repertoireOrigineImages = $"{article.Repertoire}/{article.NomFichierSansExtension}";
            FileInfo[] files = new FileInfo[0];            
            if (Directory.Exists(repertoireOrigineImages))
            {
                DirectoryInfo di = new DirectoryInfo(repertoireOrigineImages);
                files = di.GetFiles();
            }

            if (files.Length > 0)
            {
                DirectoryInfo repertoireDestImages = repertoireDest.CreateSubdirectory("images");
                foreach (FileInfo file in files)
                {
                    Image image;
                    using (FileStream stream = File.OpenRead(file.FullName))
                    {
                        image = new Image(stream);
                    }
                    Image image2 = new Image(image);

                    using (FileStream output = File.OpenWrite($"{repertoireDestImages.FullName}/{file.Name}"))
                    {
                        image
                            .Resize(1024, 0, new BicubicResampler(), false)
                            .Save(output);
                    }
                }
            }
        }
        
        /// <summary>
        /// Retourne le répertoire où ont été créé les pdfs.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static void CreerFormationsPdf(Article article, DirectoryInfo repertoire, IHostingEnvironment env)
        {            
            IServiceScopeFactory serviceScopeFactory = InitializeServices();
            ContratFormationView viewModel;
            if (article.HasFinancementDpc)
            {
                viewModel = ContratFormationView.Create(article, Financement.Dpc);
                string cheminFichierCree = CreerContratFormationPdf(serviceScopeFactory, repertoire, viewModel, "contrat-formation-dpc", "Contrats/ContratFormationDpcHorsDpc");
                _logger.LogInformation("création fichier:" + Path.GetFileName(cheminFichierCree));
            }
            if (article.HasFinancementHorsDpc)
            {
                viewModel = ContratFormationView.Create(article, Financement.HorsDpc);
                string cheminFichierCree = CreerContratFormationPdf(serviceScopeFactory, repertoire, viewModel, "contrat-formation-hors-dpc", "Contrats/ContratFormationDpcHorsDpc");
                _logger.LogInformation("création fichier:" + Path.GetFileName(cheminFichierCree));
            }
            if (article.HasFinancementSalarie)
            {
                viewModel = ContratFormationView.Create(article, Financement.Salarie);
                string cheminFichierCree = CreerContratFormationPdf(serviceScopeFactory, repertoire, viewModel, "contrat-formation-salarie", "Contrats/ContratFormationSalarie");
                _logger.LogInformation("création fichier:" + Path.GetFileName(cheminFichierCree));
            }
        }

        private static string CreerContratFormationPdf(IServiceScopeFactory serviceScopeFactory, DirectoryInfo repertoire, ContratFormationView viewModel, string filename, string template)
        {
            var modeleHtml = RenderViewAsync(serviceScopeFactory, viewModel, template).Result;
            string cheminModeleHtml = $"{repertoire.FullName}/{filename}.html";
            using (StreamWriter DestinationWriter = File.CreateText(cheminModeleHtml))
            {
                try
                {
                    DestinationWriter.Write(modeleHtml);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erreur sur la création du template PDF: {cheminModeleHtml}", ex);
                }
            }
            return cheminModeleHtml;
        }

        private static DirectoryInfo CreerRepertoireArticle(Article article, IHostingEnvironment env)
        {
            string path = "";
            DirectoryInfo result = null;
            try
            {
                string sAppPath = env.ContentRootPath;
                string swwwRootPath = env.WebRootPath;
                string lastDirectory = new DirectoryInfo(article.Repertoire).Name;
                path = $"{env.WebRootPath}/formation/{lastDirectory}/{article.Url}";
                result = Directory.CreateDirectory(path);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erreur sur la création du chemin: '{path}'", ex);
            }
            return result;
        }

        public static Task<string> RenderViewAsync<T>(IServiceScopeFactory scopeFactory, T viewModel, string template)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var helper = serviceScope.ServiceProvider.GetRequiredService<RazorViewToStringRenderer>();
                return helper.RenderViewToStringAsync(template, viewModel);
            }
        }

        public static IServiceScopeFactory InitializeServices(string customApplicationBasePath = null)
        {
            // Initialize the necessary services
            var services = new ServiceCollection();
            ConfigureDefaultServices(services, customApplicationBasePath);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        private static void ConfigureDefaultServices(IServiceCollection services, string customApplicationBasePath)
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            string applicationName;
            IFileProvider fileProvider;
            if (!string.IsNullOrEmpty(customApplicationBasePath))
            {
                applicationName = Path.GetFileName(customApplicationBasePath);
                fileProvider = new PhysicalFileProvider(customApplicationBasePath);
            }
            else
            {
                applicationName = applicationEnvironment.ApplicationName;
                fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            }

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);
            });

            // for use razorview engine
            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddLogging();
            services.AddMvc();
            services.AddTransient<RazorViewToStringRenderer>();
        }
    }
}