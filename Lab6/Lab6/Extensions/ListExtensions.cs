using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            var random = new Random();
            return list.OrderBy(_ => random.Next()).ToList();
        }
    }
}
