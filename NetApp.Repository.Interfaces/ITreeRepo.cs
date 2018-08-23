﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetApp.Models.Interfaces;

namespace NetApp.Repository.Interfaces
{
    public interface ITreeRepo<T> : IListRepo<T> where T : ITreeNode<T>
    {
        Task<IList<T>> GetRoot();
        
        Task<IList<T>> GetAllChildren(string id);
    }
}