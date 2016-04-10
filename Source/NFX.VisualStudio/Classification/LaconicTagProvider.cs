﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace NFX.VisualStudio
{
  [ContentType("Laconic")]
  [TagType(typeof(IClassificationTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicTagProvider : ITaggerProvider
  {
    [Export]
    [BaseDefinition("code")]
    [Name("Laconic")]
    internal static ContentTypeDefinition NfxContentType { get; set; }

    [Export]
    [FileExtension(".laconf")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfFileType { get; set; }

    [Export]
    [FileExtension(".rschema")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition RschemaFileType { get; set; }

    [Export]
    [FileExtension(".acmb")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition AcmbFileType { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      var taskManager = new TaskManager(ServiceProvider);
      return new LaconicClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, taskManager) as ITagger<T>;
    }
  }

  [ContentType("Laconic")]
  [TagType(typeof(IErrorTag))]
  [Export(typeof(ITaggerProvider))]
  internal sealed class LaconicErrorTagProvider : ITaggerProvider
  { 
    [Export]
    [FileExtension(".laconf")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition LaconfFileType { get; set; }

    [Export]
    [FileExtension(".rschema")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition RschemaFileType { get; set; }

    [Export]
    [FileExtension(".acmb")]
    [ContentType("Laconic")]
    internal static FileExtensionToContentTypeDefinition AcmbFileType { get; set; }

    [Import]
    internal SVsServiceProvider ServiceProvider { get; set; }

    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    [Import]
    internal IBufferTagAggregatorFactoryService TagAggregatorFactoryService { get; set; }

    [Import]
    internal ITextBufferFactoryService BufferFactory { get; set; }

    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
      var taskManager = new TaskManager(ServiceProvider);
      return new LaconicErrorClassifier(ClassificationTypeRegistry, BufferFactory, TagAggregatorFactoryService, taskManager) as ITagger<T>;
    }
  }
}