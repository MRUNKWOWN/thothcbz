using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThothCbz.Entities;
using ThothCbz.Enumerators;

namespace ThothCbz.Extensions
{
    internal static class SeriesDictionaryExtension
    {
        internal static List<List<FileEntity>> AllFilesGroupedByVolume(
                this Dictionary<string, List<FileEntity>>? value,
                string? SeriesName = null
            )
        {
            if (value == null || value.Count == 0)
            {
                return new List<List<FileEntity>>();
            }

            return value
                    .SelectMany(s => s.Value)
                    .OrderBy(o => o.SeriePath)
                    .ThenBy(t => t.Volume)
                    .GroupBy(g => g.Volume)
                    .Select(s => s.Select(m => m).ToList())
                    .ToList();
        }
    }
}
