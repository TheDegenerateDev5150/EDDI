﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EddiEdsmResponder.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EDSMResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EDSMResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EddiEdsmResponder.Properties.EDSMResources", typeof(EDSMResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EDSM API key:.
        /// </summary>
        public static string api_key_label {
            get {
                return ResourceManager.GetString("api_key_label", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EDSM Commander name:.
        /// </summary>
        public static string cmd_name_label {
            get {
                return ResourceManager.GetString("cmd_name_label", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Send details of your travels to EDSM. EDSM is a third-party tool that provides information on the locations of star systems and keeps a log of the star systems you have visited. It uses the data provided to crowd-source a map of the galaxy.
        /// </summary>
        public static string desc {
            get {
                return ResourceManager.GetString("desc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obtain EDSM log.
        /// </summary>
        public static string log_button {
            get {
                return ResourceManager.GetString("log_button", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EDSM API not configured; cannot obtain logs..
        /// </summary>
        public static string log_button_companion_unconfigured {
            get {
                return ResourceManager.GetString("log_button_companion_unconfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter EDSM API key to obtain log.
        /// </summary>
        public static string log_button_empty_api_key {
            get {
                return ResourceManager.GetString("log_button_empty_api_key", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EDSM error received: .
        /// </summary>
        public static string log_button_error_received {
            get {
                return ResourceManager.GetString("log_button_error_received", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obtained log.
        /// </summary>
        public static string log_button_fetched {
            get {
                return ResourceManager.GetString("log_button_fetched", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obtaining log....
        /// </summary>
        public static string log_button_fetching {
            get {
                return ResourceManager.GetString("log_button_fetching", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Obtaining log .
        /// </summary>
        public static string log_button_fetching_progress {
            get {
                return ResourceManager.GetString("log_button_fetching_progress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EDSM Responder.
        /// </summary>
        public static string name {
            get {
                return ResourceManager.GetString("name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To connect to EDSM you need to have registered an account with them. Once you have done this you can obtain your API key by clicking on your portrait in the top-right corner of the screen and selecting &apos;My API Key&apos;.
        /// </summary>
        public static string p1 {
            get {
                return ResourceManager.GetString("p1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If you registered a different commander name from your actual one, please enter it below.
        /// </summary>
        public static string p2 {
            get {
                return ResourceManager.GetString("p2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Once you have entered your data above you can pull your existing logs from EDSM so that EDDI knows how many times you have been to each system. You only need to do this the first time you set up EDSM. Note that this can take a while to run.
        /// </summary>
        public static string p3 {
            get {
                return ResourceManager.GetString("p3", resourceCulture);
            }
        }
    }
}
