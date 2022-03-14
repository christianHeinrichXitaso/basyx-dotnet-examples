/*******************************************************************************
* Copyright (c) 2020, 2021 Robert Bosch GmbH
* Author: Constantin Ziesche (constantin.ziesche@bosch.com)
*
* This program and the accompanying materials are made available under the
* terms of the Eclipse Distribution License 1.0 which is available at
* https://www.eclipse.org/org/documents/edl-v10.html
*
* 
*******************************************************************************/
using BaSyx.AAS.Server.Http;
using BaSyx.API.Components;
using BaSyx.Common.UI;
using BaSyx.Common.UI.Swagger;
using BaSyx.Models.Core.AssetAdministrationShell.Generics;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using BaSyx.Submodel.Server.Http;
using BaSyx.Utils.Settings.Types;
using NLog.Web;
using System;

namespace SimpleAssetAdministrationShell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            AssetAdministrationShell aas = SimpleAssetAdministrationShell.GetAssetAdministrationShell();
            ISubmodel testSubmodel = aas.Submodels["TestSubmodel"];

            ServerSettings submodelServerSettings = ServerSettings.CreateSettings();
            submodelServerSettings.ServerConfig.Hosting.ContentPath = "Content";
            submodelServerSettings.ServerConfig.Hosting.Environment = "Development";
            submodelServerSettings.ServerConfig.Hosting.Urls.Add("http://+:5040");
            submodelServerSettings.ServerConfig.Hosting.Urls.Add("https://+:5440");

            SubmodelHttpServer submodelServer = new SubmodelHttpServer(submodelServerSettings);
            submodelServer.WebHostBuilder.UseNLog();
            ISubmodelServiceProvider submodelServiceProvider = testSubmodel.CreateServiceProvider();
            submodelServer.SetServiceProvider(submodelServiceProvider);
            submodelServiceProvider.UseAutoEndpointRegistration(submodelServerSettings.ServerConfig);
            submodelServer.AddBaSyxUI(PageNames.SubmodelServer);
            submodelServer.AddSwagger(Interface.Submodel);
            submodelServer.Run();
        }     
    }
}
