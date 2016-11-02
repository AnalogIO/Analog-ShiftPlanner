﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IInstitutionRepository
    {
        bool HasApiKey(string apiKey);
        Institution Read(int id);
        Institution Read(string name);
    }
}
