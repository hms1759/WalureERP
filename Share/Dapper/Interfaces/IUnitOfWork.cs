﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Dapper.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Dispose(bool disposing);
        IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Commit();
        void Rollback();
        IDbTransaction BeginTransaction();
    }
}
