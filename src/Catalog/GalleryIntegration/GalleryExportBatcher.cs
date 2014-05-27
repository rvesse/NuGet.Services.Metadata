﻿using Newtonsoft.Json.Linq;
using NuGet.Services.Metadata.Catalog.Maintenance;
using NuGet.Services.Metadata.Catalog.Persistence;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.Services.Metadata.Catalog.GalleryIntegration
{
    public class GalleryExportBatcher
    {
        int _batchSize;
        List<GalleryExportPackage> _currentBatch;
        CatalogWriter _writer;

        public GalleryExportBatcher(int batchSize, CatalogWriter writer)
        {
            _batchSize = batchSize;
            _writer = writer;
            _currentBatch = new List<GalleryExportPackage>();
            Total = 0;
        }

        public void Process(JObject package, string registration, List<JObject> dependencies, List<string> targetFrameworks)
        {
            GalleryExportPackage export = new GalleryExportPackage
            {
                Package = package,
                Id = registration,
                Dependencies = dependencies,
                TargetFrameworks = targetFrameworks
            };

            _currentBatch.Add(export);

            if (_currentBatch.Count == _batchSize)
            {
                SubmitCurrentBatch();
                _currentBatch.Clear();
            }
        }

        public void Complete()
        {
            if (_currentBatch.Count > 0)
            {
                SubmitCurrentBatch();
                _currentBatch.Clear();
            }
        }

        public int Total
        {
            get;
            private set;
        }

        void SubmitCurrentBatch()
        {
            Total += _currentBatch.Count;

            foreach (GalleryExportPackage export in _currentBatch)
            {
                _writer.Add(new GalleryExportCatalogItem(export));
            }

            _writer.Commit().Wait();
        }
    }
}
