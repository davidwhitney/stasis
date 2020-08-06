﻿using System.Collections.Generic;
using System.Text;
using Stasis.ContentModel;

namespace Stasis.TemplateEngines
{
    public interface ITemplateEngine
    {
        List<string> SupportedExtensions { get; }
        ProcessingResultBase Process(Item item, Template template);
    }

    public abstract class ProcessingResultBase
    {
        public string OutputPath { get; set; }
        public abstract byte[] ContentBytes { get; set; }

        protected ProcessingResultBase(string outputPath)
        {
            OutputPath = outputPath;
        }
    }

    public class HtmlResult : ProcessingResultBase
    {
        public string Content { set; get; }

        public override byte[] ContentBytes
        {
            get => Encoding.UTF8.GetBytes(Content);
            set => Content = Encoding.UTF8.GetString(value);
        }

        public HtmlResult(string content, string outputPath) 
            : base(outputPath)
        {
            Content = content;
        }
    }

    public sealed class BinaryResult : ProcessingResultBase
    {
        public override byte[] ContentBytes { get; set; }

        public BinaryResult(byte[] contentBytes, string outputPath)
            : base(outputPath)
        {
            ContentBytes = contentBytes;
        }
    }
}