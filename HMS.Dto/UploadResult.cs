﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Dto
{
    public class UploadResult
    {
        public bool IsUploaded { get; set; }
        public string FileLocation { get; set; }
        public string Message { get; set; }
    }
}
