﻿using NuGet.Services.Metadata.Catalog.Collecting;
using NuGet.Services.Metadata.Catalog.Collecting.Test;
using NuGet.Services.Metadata.Catalog.Persistence;
using System;
using System.Threading.Tasks;

namespace CatalogTests
{
    class CollectorTests
    {

        public static async Task Test0Async()
        {
            //Storage storage = new AzureStorage
            //{
            //    AccountName = "nuget3",
            //    AccountKey = "",
            //    Container = "feed",
            //    BaseAddress = "http://nuget3.blob.core.windows.net"
            //};

            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\export2",
                Container = "export2",
                BaseAddress = "http://localhost:8000/"
            };

            ResolverCollector collector = new ResolverCollector(storage, 200);

            await collector.Run(new Uri("http://localhost:8000/export/catalog/index.json"), DateTime.MinValue);
            Console.WriteLine("http requests: {0} batch count: {1}", collector.RequestCount, collector.BatchCount);
        }

        public static void Test0()
        {
            Console.WriteLine("CollectorTests.Test0");

            Test0Async().Wait();
        }

        public static async Task Test1Async()
        {
            CountCollector collector = new CountCollector();
            await collector.Run(new Uri("http://localhost:8000/export/catalog/index.json"), DateTime.MinValue);
            Console.WriteLine("total: {0}", collector.Total);
            Console.WriteLine("http requests: {0}", collector.RequestCount);
        }

        public static void Test1()
        {
            Console.WriteLine("CollectorTests.Test1");

            Test1Async().Wait();
        }

        public static async Task Test2Async()
        {
            Collector collector = new CheckLinksCollector();
            await collector.Run(new Uri("http://nuget3.blob.core.windows.net/pub/catalog/index.json"), DateTime.MinValue);
        }

        public static void Test2()
        {
            Console.WriteLine("CollectorTests.Test2");

            Test2Async().Wait();
        }

        public static async Task Test3Async()
        {
            DistinctPackageIdCollector collector = new DistinctPackageIdCollector(200);
            await collector.Run(new Uri("http://nuget3.blob.core.windows.net/pub/catalog/index.json"), DateTime.MinValue);

            foreach (string s in collector.Result)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.WriteLine("count = {0}", collector.Result.Count);
            Console.WriteLine("http requests: {0}", collector.RequestCount);
        }

        public static void Test3()
        {
            Console.WriteLine("CollectorTests.Test3");

            Test3Async().Wait();
        }

        public static async Task Test4Async()
        {
            //Storage storage = new AzureStorage
            //{
            //    AccountName = "nuget3",
            //    AccountKey = "",
            //    Container = "feed",
            //    BaseAddress = "http://nuget3.blob.core.windows.net"
            //};

            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\test",
                Container = "test",
                BaseAddress = "http://localhost:8000/"
            };

            ResolverCollector collector = new ResolverCollector(storage, 200);

            await collector.Run(new Uri("http://localhost:8000/test/catalog/index.json"), DateTime.MinValue);
            Console.WriteLine("http requests: {0} batch count: {1}", collector.RequestCount, collector.BatchCount);
        }

        public static void Test4()
        {
            Console.WriteLine("CollectorTests.Test4");

            Test4Async().Wait();
        }
    }
}
