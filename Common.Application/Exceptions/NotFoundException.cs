﻿using System.Text.Json;

namespace Common.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(object filter) : base("Not found by filter " + JsonSerializer.Serialize(filter))
        {

        }
    }
}
