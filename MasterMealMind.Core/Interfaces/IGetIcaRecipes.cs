﻿using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces
{
    public interface IGetIcaRecipes
    {
        public Task GetIcaAsync();
    }
}
