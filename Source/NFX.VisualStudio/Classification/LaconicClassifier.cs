﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace NFX.VisualStudio
{
  internal sealed class LaconicClassifier : Classifier, ITagger<IClassificationTag>
  {
    internal LaconicClassifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    private List<ITagSpan<IClassificationTag>> m_Oldtags;

    public IEnumerable<ITagSpan<IClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      var tags = new List<ITagSpan<IClassificationTag>>();

      if (spans.Count < 1)
        return tags;
      var newSpanshot = spans[0].Snapshot;
      if (m_Snapshot == newSpanshot)
        return m_Oldtags;

      m_Snapshot = newSpanshot;

      var text = new SnapshotSpan(m_Snapshot, new Span(0, m_Snapshot.Length)).GetText();
      GetLaconicTags(ref tags, text);
      SynchronousUpdate(m_Snapshot);

      m_Oldtags = tags;
      return tags;
    }

    void SynchronousUpdate(ITextSnapshot snapshotSpan)
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          TagsChanged.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshotSpan, 0, snapshotSpan.Length)));
      }
    }
  }

  internal sealed class LaconicErrorClassifier : Classifier, ITagger<IErrorTag>
  {
    internal LaconicErrorClassifier(
      IClassificationTypeRegistryService typeService,
      ITextBufferFactoryService bufferFactory,
      IBufferTagAggregatorFactoryService tagAggregatorFactoryService,
      TaskManager taskManager)
      : base(typeService, bufferFactory, tagAggregatorFactoryService, taskManager)
    {
    }
    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

    private List<ITagSpan<IErrorTag>> m_ErrorTags = new List<ITagSpan<IErrorTag>>();

    public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      var tags = new List<ITagSpan<IErrorTag>>();

      if (spans.Count < 1)
        return tags;
      var newSpanshot = spans[0].Snapshot;
      if (m_Snapshot == newSpanshot)
        return m_ErrorTags;

      m_Snapshot = newSpanshot;

      var text = new SnapshotSpan(m_Snapshot, new Span(0, m_Snapshot.Length)).GetText();
      GetErrorLaconicTags(ref tags, text);
      SynchronousUpdate(m_Snapshot);

      m_ErrorTags = tags;
      return tags;
    }

    void SynchronousUpdate(ITextSnapshot snapshotSpan)
    {
      lock (ts_LockObject)
      {
        var t = TagsChanged;
        if (t != null)
          t.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshotSpan, 0, snapshotSpan.Length)));
      }
    }
  }
}