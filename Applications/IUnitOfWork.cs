﻿using Applications.IRepositories;
using Applications.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications
{
    public interface IUnitOfWork
    {
        public IClassRepository ClassRepository { get; }
        public IQuizzRepository QuizzRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
