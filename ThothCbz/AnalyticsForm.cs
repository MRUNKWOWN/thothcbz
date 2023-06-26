using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;

using ThothCbz.Constants;
using ThothCbz.Entities;
using ThothCbz.Extensions;
using ThothCbz.Properties;

namespace ThothCbz
{
    public partial class AnalyticsForm : Form
    {
        private string _directory = string.Empty;

        public Dictionary<string, List<FileEntity>> SeriesDictionary = new Dictionary<string, List<FileEntity>>();

        public AnalyticsForm(
                string directory
            )
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                this.Close();
            }

            InitializeComponent();

            _directory = directory;

            txtAnalysis.BackColor = GlobalConstants.DEFAULT_BACKGROUND_COLOR;
            txtAnalysis.ForeColor = GlobalConstants.DEFAULT_ENABLED_TEXT_COLOR;
        }

        public void AnalyseDirectories()
        {
            try
            {
                var filesFounded = Directory.GetFiles(_directory, "*.*", SearchOption.AllDirectories).ToList();

                if (filesFounded?.Any() != true)
                {
                    return;
                }

                filesFounded
                    .Where(w => w.Contains(GlobalConstants.DEFAULT_FILES_TO_GRAYSCALE_FILE_NAME) || w.Contains(GlobalConstants.DEFAULT_BLANK_FILE_NAME))
                    .ToList()
                    .ForEach(f =>
                    {
                        filesFounded.Remove(f);
                    });

                var filesBag = new ConcurrentBag<FileEntity>();

                Invoke(delegate
                {
                    txtAnalysis.Text = string.Format(filesFounded.Count > 1
                                                       ? Resources.LblAnalysingMultiplesDescriptionText
                                                       : Resources.LblAnalysingOneDescriptionText
                                                    , filesFounded.Count.ToString("N0"));
                    progressBarAnalysis.Maximum = filesFounded.Count;
                });

                Parallel.ForEach(filesFounded, file =>
                    {
                        filesBag.Add(new FileEntity(
                                filePath: file,
                                selectedFolderPath: _directory,
                                useSelectedFolderAsLevel: Settings.Default.UseSelectedFolderAsPartOfTheFileStructure,
                                splitFolderDefaultName: Settings.Default.DefaultSplitFolderName,
                                unifyFolderDefaultName: Settings.Default.DefaultUnifyFolderName
                            ));

                        Invoke(delegate
                        {
                            progressBarAnalysis.Value = filesBag.Count;
                        });
                    });

                Invoke(delegate
                {
                    ThothNotifyablePropertiesEntity.Default.SeriesDictionary = filesBag
                                                                                .Where(w => !string.IsNullOrWhiteSpace(w?.Serie))
                                                                                .GroupBy(g => g.Serie)
                                                                                .Select(s => new { s.Key, Items = s.Select(m => m).ToList() })
                                                                                .ToDictionary(d => d.Key, d => d.Items);

                    if (!ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses)
                    {
                        ThothNotifyablePropertiesEntity.Default.SeriesDictionary.AsParallel().ForAll(s =>
                        {

                            var filePath = s.Value.FilesToGrayScaleFilePath();

                            if (string.IsNullOrWhiteSpace(filePath) || File.Exists(filePath))
                            {
                                return;
                            }

                            var stb = new StringBuilder();

                            File.WriteAllLines(
                                    filePath,
                                    s.Value.Select(s => s.FilePath).OrderBy(o => o)
                                );
                        });
                    }
                });
            }

            catch (Exception ex)
            {
                ex.InformAndSaveLog();

                backgroundWorkerAnalizer_RunWorkerCompleted(this, null);
            }
        }

        private void AnalyticsForm_Shown(object sender, EventArgs e)
        {
            backgroundWorkerAnalizer.RunWorkerAsync();
        }

        private void backgroundWorkerAnalizer_DoWork(object sender, DoWorkEventArgs e)
        {
            AnalyseDirectories();
        }

        private void backgroundWorkerAnalizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs? e)
        {
            Close();
        }
    }
}
