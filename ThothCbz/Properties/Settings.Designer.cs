﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThothCbz.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool UseSelectedFolderAsPartOfTheFileStructure {
            get {
                return ((bool)(this["UseSelectedFolderAsPartOfTheFileStructure"]));
            }
            set {
                this["UseSelectedFolderAsPartOfTheFileStructure"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("split")]
        public string DefaultSplitFolderName {
            get {
                return ((string)(this["DefaultSplitFolderName"]));
            }
            set {
                this["DefaultSplitFolderName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("unify")]
        public string DefaultUnifyFolderName {
            get {
                return ((string)(this["DefaultUnifyFolderName"]));
            }
            set {
                this["DefaultUnifyFolderName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EnableUpscale {
            get {
                return ((bool)(this["EnableUpscale"]));
            }
            set {
                this["EnableUpscale"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7016")]
        public int MinimalImageHeight {
            get {
                return ((int)(this["MinimalImageHeight"]));
            }
            set {
                this["MinimalImageHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ReadOrder {
            get {
                return ((int)(this["ReadOrder"]));
            }
            set {
                this["ReadOrder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool EnableSpaceInUnifyablePages {
            get {
                return ((bool)(this["EnableSpaceInUnifyablePages"]));
            }
            set {
                this["EnableSpaceInUnifyablePages"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DisableGbzGeneration {
            get {
                return ((bool)(this["DisableGbzGeneration"]));
            }
            set {
                this["DisableGbzGeneration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EnableBrightnessAndContrastAdjustments {
            get {
                return ((bool)(this["EnableBrightnessAndContrastAdjustments"]));
            }
            set {
                this["EnableBrightnessAndContrastAdjustments"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EnableBlankPageBetweenChapters {
            get {
                return ((bool)(this["EnableBlankPageBetweenChapters"]));
            }
            set {
                this["EnableBlankPageBetweenChapters"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int BlackPageType {
            get {
                return ((int)(this["BlackPageType"]));
            }
            set {
                this["BlackPageType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UnifySplittedChaptersFolder {
            get {
                return ((bool)(this["UnifySplittedChaptersFolder"]));
            }
            set {
                this["UnifySplittedChaptersFolder"] = value;
            }
        }
    }
}
