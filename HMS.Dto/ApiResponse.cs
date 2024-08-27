﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Dto
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class GenericApiRequest<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public T Param { get; set; }
    }

}

