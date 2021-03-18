using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Models
{
    public interface IDatabaseItem<T>
    {
        T Id { get; set; }
    }
}
