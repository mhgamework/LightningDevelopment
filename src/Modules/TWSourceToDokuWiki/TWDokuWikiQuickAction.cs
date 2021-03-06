﻿using System;
using DocumentationHelper;
using LightningDevelopment;

namespace Modules.TWSourceToDokuWiki
{
    public class TWDokuWikiQuickAction : IQuickAction
    {
        public string Command
        {
            get { return "tw push docs"; }
        }

        public void Execute(string[] arguments)
        {
            var uploader = new DocUploader();
            uploader.UploadDocs();
        }
    }

}
