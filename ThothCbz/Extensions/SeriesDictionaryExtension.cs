using ThothCbz.Entities;

namespace ThothCbz.Extensions
{
    internal static class SeriesDictionaryExtension
    {
        internal static List<List<FileEntity>> AllFilesGroupedByVolume(
                this Dictionary<string, List<FileEntity>>? value,
                string? SeriesName = null
            )
        {
            var result = new List<List<FileEntity>>();

            if (value == null || value.Count == 0)
                return result;

            var series = value
                    .SelectMany(s => s.Value)
                    .Select(s => s.Serie)
                    .Where(w => !string.IsNullOrWhiteSpace(w) && (string.IsNullOrWhiteSpace(SeriesName) || w.Equals(SeriesName, StringComparison.OrdinalIgnoreCase)))
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();

            if (!series.Any())
                return result;

            foreach(var serie in series)
            {
                var volumes = value
                        .SelectMany(s => s.Value)
                        .Where(w => w.Serie == serie)
                        .Select(s => s.Volume)
                        .Distinct()
                        .OrderBy(o => o)
                        .ToList();

                foreach (var volume in volumes)
                {
                    result.Add(
                            value
                                .SelectMany(s => s.Value)
                                .Where(w => w.Serie == serie && w.Volume == volume)
                                .OrderBy(o => o.FilePath)
                                .ToList()
                        );
                }
            }

            return result;
        }
    }
}
