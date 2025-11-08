using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ThothCbz.Properties;

namespace ThothCbz.Entities
{
    public class ThothNotifyablePropertiesEntity : INotifyPropertyChanged
    {
        private bool _adjustFilesActive = false;
        private bool _unifyPagesActive = false;
        private bool _splitPagesActive = false;
        private bool _generateCbzActive = false;
        private bool _analysisExecuted = false;
        private bool _generationProcessRunning = false;
        private bool _cancelGenerationProcessQueued = false;
        private bool _existOddUnifyablePagesFiles = false;
        private int _filesCount = 0;
        private int _splittableFilesCount = 0;
        private int _unifyableFilesCount = 0;
        private int _unifyableFilesNeedingConversionCount = 0;
        private int _splittableFilesNeedingConversionCount = 0;
        private int _adjustableFilesCount = 0;
        private int _unknownFilesCount = 0;
        private int _volumesCount = 0;
        private int _seriesCount = 0;
        private string _directoryPathToAnalyze = string.Empty;
        private Dictionary<string, List<FileEntity>> _seriesDictionary = new Dictionary<string, List<FileEntity>>();
        private ConcurrentBag<int> _splittableFilesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _unifyableFilesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _adjustableFilesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _volumesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _seriesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _filesSettedCount = new ConcurrentBag<int>();
        private ConcurrentBag<int> _cbzFilesSettedCount = new ConcurrentBag<int>();

        public static ThothNotifyablePropertiesEntity Default { get; } = new ThothNotifyablePropertiesEntity();

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(
                [CallerMemberName] string propertyName = ""
            )
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool AdjustFilesActive 
        { 
            get
            {
                return _adjustFilesActive;
            }

            set 
            {
                if (value != _adjustFilesActive)
                {
                    _adjustFilesActive = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistWarnings));
                    NotifyPropertyChanged(nameof(EnableAdjustControls));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            } 
        }

        public bool UnifyPagesActive
        {
            get
            {
                return _unifyPagesActive && (UnifyableFilesNeedingConversionCount == 0 || AdjustFilesActive);
            }

            set
            {
                if (value != _unifyPagesActive)
                {
                    _unifyPagesActive = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistWarnings));
                }
            }
        }

        public bool SplitPagesActive
        {
            get
            {
                return _splitPagesActive && (SplittableFilesNeedingConversionCount == 0 || AdjustFilesActive);
            }

            set
            {
                if (value != _splitPagesActive)
                {
                    _splitPagesActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool GenerateCbzActive
        {
            get
            {
                return _generateCbzActive;
            }

            set
            {
                if (value != _generateCbzActive)
                {
                    _generateCbzActive = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(EnableCbzControls));
                }
            }
        }

        public bool AnalysisExecuted
        {
            get
            {
                return _analysisExecuted;
            }

            set
            {
                if (value != _analysisExecuted)
                {
                    _analysisExecuted = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(EnableOptionControls));
                    NotifyPropertyChanged(nameof(EnableAdjustControls));
                    NotifyPropertyChanged(nameof(EnableCbzControls));
                    NotifyPropertyChanged(nameof(EnabledAdjustFiles));
                    NotifyPropertyChanged(nameof(EnabledUnifyPages));
                    NotifyPropertyChanged(nameof(EnabledSplitPages));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            }
        }

        public bool GenerationProcessRunning
        {
            get
            {
                return _generationProcessRunning;
            }

            set
            {
                if (value != _generationProcessRunning)
                {
                    _generationProcessRunning = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(EnableControls));
                    NotifyPropertyChanged(nameof(EnableOptionControls));
                    NotifyPropertyChanged(nameof(EnableAdjustControls));
                    NotifyPropertyChanged(nameof(EnableCbzControls));
                    NotifyPropertyChanged(nameof(EnabledCancelGenerationProcess));
                    NotifyPropertyChanged(nameof(EnabledAdjustFiles));
                    NotifyPropertyChanged(nameof(EnabledUnifyPages));
                    NotifyPropertyChanged(nameof(EnabledSplitPages));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            }
        }

        public bool KeepUserChoicesBetweenFileAnalyses { get; set; } = false;

        public bool CancelGenerationProcessQueued
        {
            get
            {
                return _cancelGenerationProcessQueued;
            }

            set
            {
                if (value != _cancelGenerationProcessQueued)
                {
                    _cancelGenerationProcessQueued = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(EnabledCancelGenerationProcess));
                }
            }
        }

        public int FilesCount
        {
            get
            {
                return _filesCount;
            }
            private set
            {
                if (value != _filesCount)
                {
                    _filesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistFiles));
                    NotifyPropertyChanged(nameof(EnabledAdjustFiles));
                    NotifyPropertyChanged(nameof(EnabledUnifyPages));
                    NotifyPropertyChanged(nameof(EnabledSplitPages));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            }
        }

        public int SplittableFilesCount
        {
            get
            {
                return _splittableFilesCount;
            }
            private set
            {
                if (value != _splittableFilesCount)
                {
                    _splittableFilesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistSplittableFiles));
                    NotifyPropertyChanged(nameof(EnabledSplitPages));
                }
            }
        }

        public int UnifyableFilesCount
        {
            get
            {
                return _unifyableFilesCount;
            }
            private set
            {
                if (value != _unifyableFilesCount)
                {
                    _unifyableFilesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistUnifyableFiles));
                    NotifyPropertyChanged(nameof(EnabledUnifyPages));
                }
            }
        }

        public int AdjustableFilesCount
        {
            get
            {
                return _adjustableFilesCount;
            }
            private set
            {
                if (value != _adjustableFilesCount)
                {
                    _adjustableFilesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistAdjustableFiles));
                    NotifyPropertyChanged(nameof(ExistWarnings));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            }
        }

        public int SplittableFilesNeedingConversionCount
        {
            get
            {
                return _splittableFilesNeedingConversionCount;
            }
            private set
            {
                if (value != _splittableFilesNeedingConversionCount)
                {
                    _splittableFilesNeedingConversionCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(SplitPagesActive));
                    NotifyPropertyChanged(nameof(ExistWarnings));
                }
            }
        }

        public int UnifyableFilesNeedingConversionCount
        {
            get
            {
                return _unifyableFilesNeedingConversionCount;
            }
            private set
            {
                if (value != _unifyableFilesNeedingConversionCount)
                {
                    _unifyableFilesNeedingConversionCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(UnifyPagesActive));
                    NotifyPropertyChanged(nameof(ExistWarnings));
                }
            }
        }

        public int UnknownFilesCount
        {
            get
            {
                return _unknownFilesCount;
            }
            private set
            {
                if (value != _unknownFilesCount)
                {
                    _unknownFilesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistUnknownFiles));
                    NotifyPropertyChanged(nameof(EnabledAdjustFiles));
                    NotifyPropertyChanged(nameof(EnabledUnifyPages));
                    NotifyPropertyChanged(nameof(EnabledSplitPages));
                    NotifyPropertyChanged(nameof(EnabledGenerateCbz));
                }
            }
        }

        public int VolumesCount
        {
            get
            {
                return _volumesCount;
            }
            private set
            {
                if (value != _volumesCount)
                {
                    _volumesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistVolumesFiles));
                }
            }
        }

        public int SeriesCount
        {
            get
            {
                return _seriesCount;
            }
            private set
            {
                if (value != _seriesCount)
                {
                    _seriesCount = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistSeriesFiles));
                }
            }
        }

        public string DirectoryPathToAnalyze
        {
            get
            {
                return string.IsNullOrWhiteSpace(_directoryPathToAnalyze) 
                        ? Resources.txtDirectoryDefaultText
                        : _directoryPathToAnalyze;
            }

            set
            {
                if (value != _directoryPathToAnalyze)
                {
                    _directoryPathToAnalyze = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Dictionary<string, List<FileEntity>> SeriesDictionary
        {
            get
            {
                return _seriesDictionary;
            }

            set
            {
                if (value != _seriesDictionary)
                {
                    _seriesDictionary = value;
                    NotifyPropertyChanged();

                    var filesAnalyzed = value.SelectMany(s => s.Value);

                    SeriesCount = value.Keys.Count();
                    VolumesCount = value.SelectMany(s => s.Value.Where(w => !string.IsNullOrWhiteSpace(w?.Volume)).GroupBy(g => g.Volume).Select(s2 => s2.Key).Distinct()).Count();
                    SplittableFilesCount = filesAnalyzed.Where(w => w is { } && w.IsSplit).ToList().Count();
                    UnifyableFilesCount = filesAnalyzed.Where(w => w is { } && w.IsUnify).ToList().Count();
                    UnifyableFilesNeedingConversionCount = filesAnalyzed.Where(w => w is { } && w.IsUnify && w.NeedConversion).ToList().Count();
                    SplittableFilesNeedingConversionCount = filesAnalyzed.Where(w => w is { } && w.IsSplit && w.NeedConversion).ToList().Count();
                    UnknownFilesCount = filesAnalyzed.Where(w => w is { } && !w.IsAcceptableFileType).ToList().Count();
                    AdjustableFilesCount = filesAnalyzed.Where(w => w is { } && w.NeedConversion).ToList().Count();
                    FilesCount = filesAnalyzed.Count();
                    ExistOddUnifyablePagesFiles = filesAnalyzed
                                                    .Where(w => w.IsUnify)
                                                    .GroupBy(g => new { g.Serie, g.Volume, g.Chapter })
                                                    .Select(s => s.Count() % 2)
                                                    .Where(w2 => w2 != 0)
                                                    .Any();
                }
            }
        }

        public ConcurrentBag<int> SplittableFilesSetted
        {
            get
            {
                return _splittableFilesSettedCount;
            }

            set
            {
                if (value != _splittableFilesSettedCount)
                {
                    _splittableFilesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> UnifyableFilesSetted
        {
            get
            {
                return _unifyableFilesSettedCount;
            }

            set
            {
                if (value != _unifyableFilesSettedCount)
                {
                    _unifyableFilesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> AdjustableFilesSetted
        {
            get
            {
                return _adjustableFilesSettedCount;
            }

            set
            {
                if (value != _adjustableFilesSettedCount)
                {
                    _adjustableFilesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> VolumesSetted
        {
            get
            {
                return _volumesSettedCount;
            }

            set
            {
                if (value != _volumesSettedCount)
                {
                    _volumesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> SeriesSetted
        {
            get
            {
                return _seriesSettedCount;
            }

            set
            {
                if (value != _seriesSettedCount)
                {
                    _seriesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> FilesSetted
        {
            get
            {
                return _filesSettedCount;
            }

            set
            {
                if (value != _filesSettedCount)
                {
                    _filesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ConcurrentBag<int> CbzFilesSetted
        {
            get
            {
                return _cbzFilesSettedCount;
            }

            set
            {
                if (value != _cbzFilesSettedCount)
                {
                    _cbzFilesSettedCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ExistOddUnifyablePagesFiles
        {
            get
            {
                return _existOddUnifyablePagesFiles;
            }

            private set
            {
                if (value != _existOddUnifyablePagesFiles)
                {
                    _existOddUnifyablePagesFiles = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(ExistWarnings));
                }
            }
        }

        public bool ExistUnknownFiles => UnknownFilesCount > 0;

        public bool ExistAdjustableFiles => AdjustableFilesCount > 0 && AdjustableFilesCount > AdjustableFilesSetted.Sum(s => s);

        public bool ExistSplittableFiles => SplittableFilesCount > 0 && SplittableFilesCount > SplittableFilesSetted.Sum(s => s);

        public bool ExistUnifyableFiles => UnifyableFilesCount > 0 && UnifyableFilesCount > UnifyableFilesSetted.Sum(s => s);

        public bool ExistFiles => FilesCount > 0;

        public bool ExistVolumesFiles => VolumesCount > 0;

        public bool ExistSeriesFiles => SeriesCount > 0;

        public bool ExistWarnings => AnalysisExecuted && (
                                        (ExistAdjustableFiles && !AdjustFilesActive) || 
                                            ExistUnknownFiles || (
                                                (
                                                    (ExistUnifyableFiles && UnifyableFilesNeedingConversionCount > 0 && UnifyPagesActive) || 
                                                    (ExistSplittableFiles && SplittableFilesNeedingConversionCount > 0 && SplitPagesActive)
                                                ) && 
                                            !AdjustFilesActive)
                                        ) ||
                                        (ExistUnifyableFiles && UnifyPagesActive && ExistOddUnifyablePagesFiles);

        public bool EnabledAdjustFiles => !GenerationProcessRunning && AnalysisExecuted && ExistFiles && FilesCount > FilesSetted.Sum(s => s) && !ExistUnknownFiles;

        public bool EnabledUnifyPages => EnabledAdjustFiles && ExistUnifyableFiles;

        public bool EnabledSplitPages => EnabledAdjustFiles && ExistSplittableFiles;

        public bool EnabledGenerateCbz => (!ExistAdjustableFiles || AdjustFilesActive) && AnalysisExecuted && !GenerationProcessRunning && FilesCount > CbzFilesSetted.Sum(s => s);

        public bool EnabledCancelGenerationProcess => GenerationProcessRunning && !CancelGenerationProcessQueued;

        public bool EnableControls => !GenerationProcessRunning;

        public bool EnableOptionControls => EnableControls && AnalysisExecuted;

        public bool EnableAdjustControls => EnableOptionControls && EnabledAdjustFiles && AdjustFilesActive;

        public bool EnableCbzControls => EnableOptionControls && EnabledGenerateCbz && GenerateCbzActive;

        public bool EnableUnifyableControls => EnableOptionControls && EnabledUnifyPages && UnifyPagesActive;

        public bool EnableGenerationProcess => EnableOptionControls && EnabledGenerateCbz && !ExistWarnings;

        public void Reset()
        {
            if(!ThothNotifyablePropertiesEntity.Default.KeepUserChoicesBetweenFileAnalyses)
            {
                AdjustFilesActive = false;
                UnifyPagesActive = false;
                SplitPagesActive = false;
                GenerateCbzActive = false;
            }
            
            AnalysisExecuted = false;
            GenerationProcessRunning = false;
            CancelGenerationProcessQueued = false;
            FilesSetted = new ConcurrentBag<int>();
            SeriesSetted = new ConcurrentBag<int>();
            SplittableFilesSetted = new ConcurrentBag<int>();
            UnifyableFilesSetted = new ConcurrentBag<int>();
            CbzFilesSetted = new ConcurrentBag<int>();
            VolumesSetted = new ConcurrentBag<int>();
            AdjustableFilesSetted = new ConcurrentBag<int>();
            SeriesDictionary = new Dictionary<string, List<FileEntity>>();
        }

        public void ForceNotification(string propertyName)
        {
            NotifyPropertyChanged(propertyName);
        }
    }
}
