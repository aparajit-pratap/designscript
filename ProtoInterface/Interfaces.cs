﻿using System;
using System.ComponentModel;
using Autodesk.DesignScript.Runtime;
using System.Collections.Generic;

namespace Autodesk.DesignScript.Interfaces
{
    /// <summary>
    /// This interface is implemented by the various host of GraphEditor to
    /// pass some configuration parameters as well as to receive some event
    /// notifications required by the host application to update itself.
    /// </summary>
    public interface IGraphEditorHostApplication
    {
        /// <summary>
        /// Name value pair for configuration parameters. Mostly the 
        /// configuration keys are used from ConfigurationKeys class.
        /// </summary>
        Dictionary<string, object> Configurations { get; }

        /// <summary>
        /// This method is called by the graph controller to notify host app
        /// that the graph execution has finished and graph is updated. Host
        /// app would like to refresh it's graphics or perform some cleanup.
        /// </summary>
        void PostGraphUpdate();

        /// <summary>
        /// When the current graph is suppressed and a new graph is activated
        /// this method is called by the graph controller, so that host app
        /// can setup or activate a document corresponding to given graph id.
        /// </summary>
        /// <param name="graphId"></param>
        void GraphActivated(uint graphId);
    }

    /// <summary>
    /// Provides application configuration
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Provides the path of main executing script
        /// </summary>
        string RootModulePath { get; }

        /// <summary>
        /// Provides list of include directories.
        /// </summary>
        string[] IncludeDirectories { get; }

        /// <summary>
        /// Gets application defined configuration value.
        /// </summary>
        /// <param name="config">Configuration name</param>
        /// <returns>Configuration value</returns>
        object GetConfigValue(string config);

        /// <summary>
        /// Set application defined configuration value.
        /// </summary>
        /// <param name="config">Configuration name</param>
        /// <param name="value">Configuration value</param>
        void SetConfigValue(string config, object value);
    }

    public class ConfigurationKeys
    {
        /// <summary>
        /// This key is used to configure the library filename, which implements 
        /// IGeometryFactory interface.
        /// </summary>
        public static readonly string GeometryFactory = "GeometryFactoryFileName";

        /// <summary>
        /// This key is used to configure the library filename, which implements 
        /// IPersistenceManager interface.
        /// </summary>
        public static readonly string PersistentManager = "PersistentManagerFileName";

        /// <summary>
        /// This key is used to set/get IContextDataProvider implementation
        /// by host application.
        /// </summary>
        public static readonly string GeometryProvider = "GeometryProvider";

        /// <summary>
        /// This key is used to set the session database object for the host
        /// application. THIS IS NOW DEPRECATED (use "SessionKey" instead).
        /// </summary>
        public static readonly string SessionDatabase = "SessionDatabase";

        /// <summary>
        /// This key is used to set the session key (by the host application) 
        /// which is understood by the corresponding IPersistenceManager when 
        /// it comes to telling one session from another. An example of 
        /// session would be the documents in the host application, a host 
        /// document can be identified with a session key, and each host 
        /// document has a unique session key that IPersistenceManager can use
        /// to differentiate between two documents. The corresponding value 
        /// for SessionKey is a value of "string" type.
        /// </summary>
        public static readonly string SessionKey = "SessionKey";

        /// <summary>
        /// This key is used to request explicit lock on database by the host
        /// application before execution of the script. The corresponding value
        /// is bool.
        /// </summary>
        public static readonly string RequestExplicitLock = "RequestExplicitLock";

        /// <summary>
        /// This key is used to check if the application is recording user actions.
        /// The corresponding value is bool.
        /// </summary>
        public static readonly string RecordingUserActions = "RecordingUserActions";

        /// <summary>
        /// This key is used to determine if the persistent objects should be cleared on 
        /// screen before each run. In live execution scenarios like DesignScript Studio,
        /// there is no clear distinction between runs. For more information, please see 
        /// "AsmExtensionApplication::OnBeginExecution" in "AsmExtensionApplication.cpp".
        /// </summary>
        public static readonly string ClearPersistedObjects = "ClearPersistedObjects";

        /// <summary>
        /// This key is being referenced in "DesignScriptStudio.Graph.Ui.GraphControl"
        /// as a way to determine if the underlying "RenderService" should be enabled.
        /// The default behaviour (if this flag is not specified) is to enable geometric 
        /// preview. The "object" value being passed for this configuration key is 
        /// expected to be a "bool" object rather than a "string" object.
        /// </summary>
        public static readonly string GeometricPreviewEnabled = "GeometricPreviewEnabled";

        /// <summary>
        /// This key is being referenced in "DesignScriptStudio.Graph.Ui.GraphControl"
        /// as a way to filtered out classes that is currently not supported in ProtoGL.
        /// The "object" value being passed for this configuration key is 
        /// expected to be a "string" object which specified the assembly and class that
        /// should be hidden. The location of the config file is "\installtools\Bundle\
        /// DesignScript.bundle\Contents\Win64" The format of the string should be 
        /// "%assemblyName%;%className&;..;%assemblyName%;%className&;", 
        /// The CoreCoponent will look for "%assemblyName%;%className&;" (with two ';' at
        /// end of assembly name and class name) and filter the fully matched item out
        /// </summary>
        public static readonly string FilteredClasses = "FilteredClasses";

        /// <summary>
        /// This key is referenced in the geometry test framework.
        /// The value type for this key is bool.
        /// If the value is true, that means the core will generate XML properties.
        /// </summary>
        public static readonly string GeometryXmlProperties = "GeometryXmlProperties";
    }

    /// <summary>
    /// Represents a session object for current execution.
    /// </summary>
    public interface IExecutionSession
    {
        /// <summary>
        /// Gets the configuration object for this execution session.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Searches the given file and returns full path, if found.
        /// </summary>
        /// <param name="filename">File name to search.</param>
        /// <returns>Full path of given filename or empty string if file not 
        /// found.</returns>
        string SearchFile(string filename);
    }

    /// <summary>
    /// An FFI library can implement this interface to get some notifications
    /// from DesignScript application.
    /// </summary>
    public interface IExtensionApplication
    {
        /// <summary>
        /// Called when first time this application is loaded.
        /// </summary>
        void StartUp();

        /// <summary>
        /// Called when script execution starts.
        /// </summary>
        /// <param name="session">Execution session object with which script
        /// execution starts.</param>
        void OnBeginExecution(IExecutionSession session);

        /// <summary>
        /// Called when script execution is suspended for debugging/inspection
        /// </summary>
        /// <param name="session">Execution session object with which script
        /// execution starts.</param>
        void OnSuspendExecution(IExecutionSession session);

        /// <summary>
        /// Called when script execution is resumed after debugging/inspection
        /// </summary>
        /// <param name="session">Execution session object with which script
        /// execution starts.</param>
        void OnResumeExecution(IExecutionSession session);

        /// <summary>
        /// Called when script execution has ended.
        /// </summary>
        /// <param name="session">Execution session object with which script
        /// execution had started.</param>
        void OnEndExecution(IExecutionSession session);

        /// <summary>
        /// Called when designscript application is shutting down.
        /// </summary>
        void ShutDown();
    }

    /// <summary>
    /// Represents an external data to be used as context for execution.
    /// </summary>
    public interface IContextData
    {
        /// <summary>
        /// Gets name of the data. This context data can be identified with
        /// name in designscript world.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The context data as represented in DesignScript
        /// </summary>
        Object Data { get; }

        /// <summary>
        /// Event notifier to notify when it's data changes.
        /// </summary>
        event EventHandler DataChanged;

        /// <summary>
        /// Gets the context provider for interpretation of data in designscript
        /// world.
        /// </summary>
        IContextDataProvider ContextProvider { get; }
    }

    /// <summary>
    /// Represents a connector to external data source to provide context 
    /// speicific data. This interface provide import/export feature for any 
    /// context specific data. It also provides a mechanism to capture data 
    /// interactively.
    /// </summary>
    public interface IContextDataProvider
    {
        /// <summary>
        /// Gets the name of this data provider to identify it uniquely.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Imports specific the context data using the given identifier string.
        /// </summary>
        /// <param name="connectionParameters">Input dictionary of connection parameters 
        /// to connect to the data source to import the data. Each context data in 
        /// the list contains pair of connection parameter name and value</param>
        /// <returns></returns>
        IContextData[] ImportData(Dictionary<string, Object> connectionParameters);

        /// <summary>
        /// Exports data to specified file. This context provider determines the
        /// format for data store and returns the connection string for the given
        /// file using which this data can be imported back again.
        /// </summary>
        /// <param name="data">Collection of data that needs to be exported.</param>
        /// <param name="filePath">Path for the file where this data can be 
        /// exported and saved.</param>
        /// <returns>The connection parameters using which the exported data can be 
        /// imported in future.Each context data in the list contains 
        /// pair of connection parameter name and value</returns>
        Dictionary<string, Object> ExportData(IContextData[] data, string filePath);

        /// <summary>
        /// Begins data capture interaction in the specific context and returns 
        /// collection of captured data.
        /// </summary>
        /// <returns>Dictionary of connection parameters to import the data 
        /// captured by interaction. Each context data in the list contains 
        /// pair of connection parameter name and value</returns>
        Dictionary<string, Object> CaptureData();

        /// <summary>
        /// Returns DesignScript expression for given parameters assigned to input
        /// variable.
        /// </summary>
        /// <param name="parameters">Captured parameters to be converted to
        /// DesignScript expression assigned to the input variable.</param>
        /// <param name="variable">Variable name to which imported data to be 
        /// assigned.</param>
        /// <returns>DesignScript expression string</returns>
        string GetExpression(Dictionary<string, Object> parameters, string variable);
    }
}