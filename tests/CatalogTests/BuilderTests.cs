﻿using NuGet.Services.Metadata.Catalog.Maintenance;
using NuGet.Services.Metadata.Catalog.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CatalogTests
{
    class BuilderTests
    {
        public static async Task Test0Async()
        {
            string nuspecs = @"c:\data\nuspecs";

            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\pub",
                Container = "pub",
                BaseAddress = "http://localhost:8000/"
            };

            //Storage storage = new AzureStorage
            //{
            //    AccountName = "nuget3",
            //    AccountKey = "",
            //    Container = "pub",
            //    BaseAddress = "http://nuget3.blob.core.windows.net"
            //};

            CatalogContext context = new CatalogContext();

            CatalogWriter writer = new CatalogWriter(storage, context, 1000);

            const int BatchSize = 1000;
            int i = 0;

            int commitCount = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(nuspecs);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.xml"))
            {
                writer.Add(new NuspecPackageCatalogItem(fileInfo));

                if (++i % BatchSize == 0)
                {
                    await writer.Commit(DateTime.Now);

                    Console.WriteLine("commit number {0}", commitCount++);
                }
            }

            await writer.Commit(DateTime.Now);

            Console.WriteLine("commit number {0}", commitCount++);
        }

        public static void Test0()
        {
            Console.WriteLine("BuilderTests.Test0");

            Test0Async().Wait();
        }

        public static async Task Test1Async()
        {
            string nuspecs = @"c:\data\nuspecs";

            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\full",
                Container = "full",
                BaseAddress = "http://localhost:8000/"
            };

            CatalogContext context = new CatalogContext();

            CatalogWriter writer = new CatalogWriter(storage, context, 150);

            int total = 0;

            int[] commitSize = { 50, 40, 25, 50, 10, 30, 40, 5, 400, 30, 10 };
            int i = 0;

            int commitCount = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(nuspecs);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.xml"))
            {
                if (commitCount == commitSize.Length)
                {
                    break;
                }

                writer.Add(new NuspecPackageCatalogItem(fileInfo));
                total++;

                if (++i == commitSize[commitCount])
                {
                    await writer.Commit(DateTime.Now);

                    Console.WriteLine("commit number {0}", commitCount);

                    commitCount++;
                    i = 0;
                }
            }

            Console.WriteLine("total: {0}", total);
        }

        public static void Test1()
        {
            Console.WriteLine("BuilderTests.Test1");

            Test1Async().Wait();
        }

        public static async Task Test2Async()
        {
            string nuspecs = @"c:\data\test_nuspecs";

            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\test",
                Container = "test",
                BaseAddress = "http://localhost:8000/"
            };

            //Storage storage = new AzureStorage
            //{
            //    AccountName = "nuget3",
            //    AccountKey = "",
            //    Container = "pub",
            //    BaseAddress = "http://nuget3.blob.core.windows.net"
            //};

            CatalogContext context = new CatalogContext();

            CatalogWriter writer = new CatalogWriter(storage, context, 1000);

            const int BatchSize = 1000;
            int i = 0;

            int commitCount = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(nuspecs);
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles("*.xml"))
            {
                writer.Add(new NuspecPackageCatalogItem(fileInfo));

                if (++i % BatchSize == 0)
                {
                    await writer.Commit(DateTime.Now);

                    Console.WriteLine("commit number {0}", commitCount++);
                }
            }

            await writer.Commit(DateTime.Now);

            Console.WriteLine("commit number {0}", commitCount++);
        }

        public static void Test2()
        {
            Console.WriteLine("BuilderTests.Test2");

            Test2Async().Wait();
        }

        public static async Task Test3Async()
        {
            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\test",
                Container = "test",
                BaseAddress = "http://localhost:8000/"
            };

            CatalogContext context = new CatalogContext();

            CatalogWriter writer = new CatalogWriter(storage, context, 1000);

            string[] names1 = { "a", "b", "c", "d", "e" };
            string[] names2 = { "f", "g", "h" };
            string[] names3 = { "i", "j", "k" };

            foreach (string name in names1)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(new Dictionary<string, string> { { "prop1", "value1.1" }, { "prop2", "value2.1" } });

            Console.WriteLine("commit user data #1");

            foreach (KeyValuePair<string, string> items in await CatalogWriter.GetCommitUserData(storage))
            {
                Console.WriteLine("{0} {1}", items.Key, items.Value);
            }

            foreach (string name in names2)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(new Dictionary<string, string> { { "prop1", "value1.2" }, { "prop2", "value2.2" } });

            Console.WriteLine("commit user data #2");

            foreach (KeyValuePair<string, string> items in await CatalogWriter.GetCommitUserData(storage))
            {
                Console.WriteLine("{0} {1}", items.Key, items.Value);
            }

            foreach (string name in names3)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(new Dictionary<string, string> { { "prop1", "value1.3" }, { "prop2", "value2.3" } });

            Console.WriteLine("commit user data #3");

            foreach (KeyValuePair<string, string> items in await CatalogWriter.GetCommitUserData(storage))
            {
                Console.WriteLine("{0} {1}", items.Key, items.Value);
            }
        }

        public static void Test3()
        {
            Console.WriteLine("BuilderTests.Test3 - User Data");

            Test3Async().Wait();
        }

        public static async Task Test4Async()
        {
            Storage storage = new FileStorage
            {
                Path = @"c:\data\site\test",
                Container = "test",
                BaseAddress = "http://localhost:8000/"
            };

            CatalogContext context = new CatalogContext();

            CatalogWriter writer = new CatalogWriter(storage, context, 4);

            string[] names1 = { "a", "b", "c", "d", "e" };
            string[] names2 = { "f", "g", "h" };
            string[] names3 = { "i", "j", "k" };

            DateTime timeStamp = DateTime.UtcNow;

            foreach (string name in names1)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(timeStamp);

            Console.WriteLine("commit #1 timeStamp {0}", await CatalogWriter.GetLastCommitTimeStamp(storage));
            Console.WriteLine("commit #1 count {0}", await CatalogWriter.GetCount(storage));

            timeStamp = timeStamp.AddHours(1);

            foreach (string name in names2)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(timeStamp);

            Console.WriteLine("commit #2 timeStamp {0}", await CatalogWriter.GetLastCommitTimeStamp(storage));
            Console.WriteLine("commit #2 count {0}", await CatalogWriter.GetCount(storage));

            timeStamp = timeStamp.AddHours(1);

            foreach (string name in names3)
            {
                writer.Add(new TestCatalogItem(name));
            }
            await writer.Commit(timeStamp);

            Console.WriteLine("commit #3 timeStamp {0}", await CatalogWriter.GetLastCommitTimeStamp(storage));
            Console.WriteLine("commit #3 count {0}", await CatalogWriter.GetCount(storage));
        }

        public static void Test4()
        {
            Console.WriteLine("BuilderTests.Test4 - commit TimeStamp");

            Test4Async().Wait();
        }
    }
}
