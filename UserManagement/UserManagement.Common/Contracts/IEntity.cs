﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Common.Contracts
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
