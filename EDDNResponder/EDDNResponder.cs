﻿using EddiCompanionAppService;
using EddiCore;
using EddiDataDefinitions;
using EddiDataProviderService;
using EddiEddnResponder.Properties;
using EddiEvents;
using EddiStatusService;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Utilities;

namespace EddiEddnResponder
{
    /// <summary>
    /// A responder for EDDI to provide information to EDDN.
    /// </summary>
    public class EDDNResponder : EDDIResponder
    {
        // Schema reference: https://github.com/EDCD/EDDN/tree/master/schemas

        public readonly EDDNState eddnState;
        private readonly List<ISchema> schemas = new List<ISchema>();
        private readonly List<ICapiSchema> capiSchemas = new List<ICapiSchema>();

        public string ResponderName()
        {
            return "EDDN responder";
        }

        public string LocalizedResponderName()
        {
            return EddnResources.name;
        }

        public string ResponderDescription()
        {
            return EddnResources.desc;
        }

        [UsedImplicitly]
        public EDDNResponder() : this(StarSystemSqLiteRepository.Instance)
        { }

        public EDDNResponder(StarSystemRepository starSystemRepository)
        {
            // Configure our state tracking toolkit
            eddnState = new EDDNState(starSystemRepository);

            // Populate our schemas list
            GetSchemas();

            // Handle status changes
            StatusService.StatusUpdatedEvent += StatusServiceOnStatusUpdatedEvent;

            // Handle Companion App station data
            CompanionAppService.Instance.CombinedStationEndpoints.StationUpdatedEvent += FrontierApiOnStationUpdatedEvent;
            
            Logging.Info($"Initialized {ResponderName()}");
        }

        private void FrontierApiOnStationUpdatedEvent(object sender, CompanionApiEndpointEventArgs e)
        {
            foreach (var schema in capiSchemas)
            {
                try
                {
                    var handledData = schema.Handle(e.profileJson, e.marketJson, e.shipyardJson, e.fleetCarrierJson, eddnState, out bool handled);
                    if (handled)
                    {
                        schema.SendCapi(handledData);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Schema", schema?.GetType().Name);
                    ex.Data.Add("FrontierApiData", e);
                    Logging.Error($"{schema?.GetType().Name ?? "Schema"} data handler failed.", ex);
                }

            }
        }

        private void StatusServiceOnStatusUpdatedEvent(object sender, EventArgs e)
        {
            if (sender is Status status)
            {
                eddnState.Location.GetLocationInfo(status);
            }
        }

        public void Handle(Event theEvent)
        {
            if (EDDI.Instance.inTelepresence)
            {
                // We don't do anything whilst in Telepresence
                return;
            }

            if (string.IsNullOrEmpty(theEvent.raw)) 
            {
                // A null value may indicate a synthetic event used to pass data within EDDI
                // (which should always be ignored)
                return; 
            }

            Logging.Debug("Received event " + theEvent.raw);

            var data = Deserializtion.DeserializeData(theEvent.raw);
            var edType = JsonParsing.getString(data, "event");

            // Attempt to obtain available game version data from the active event 
            eddnState.GameVersion.GetVersionInfo(edType, data);

            // Except as noted above, always attempt to obtain available location data from the active event 
            eddnState.Location.GetLocationInfo(edType, data);

            if (theEvent.fromLoad)
            {
                // Don't do anything further with data acquired during log loading,
                // just update our internal state and move on
                return;
            }

            // Handle events
            foreach (var schema in schemas)
            {
                try
                {
                    var handledData = schema.Handle(edType, data, eddnState, out bool handled);
                    if (handled)
                    {
                        schema.Send(handledData);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Schema", schema?.GetType().Name);
                    data.ToList().ForEach(kv => ex.Data.Add(kv.Key, kv.Value));
                    Logging.Error($"{schema?.GetType().Name ?? "Schema"} data handler failed.", ex);
                }
            }
        }

        public bool Start()
        {
            return true;
        }

        public void Stop()
        { }

        public void Reload()
        { }

        void GetSchemas()
        {
            foreach (var type in typeof(ISchema).Assembly.GetTypes())
            {
                if (!type.IsInterface && !type.IsAbstract)
                {
                    if (type.GetInterfaces().Contains(typeof(ISchema)))
                    {
                        // Ensure that the static constructor of the class has been run
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

                        var schema = type.InvokeMember(null,
                            BindingFlags.CreateInstance,
                            null, null, null) as ISchema;
                        schemas.Add(schema);
                    }
                    if (type.GetInterfaces().Contains(typeof(ICapiSchema)))
                    {
                        // Ensure that the static constructor of the class has been run
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

                        var capiSchema = type.InvokeMember(null,
                            BindingFlags.CreateInstance,
                            null, null, null) as ICapiSchema;
                        capiSchemas.Add(capiSchema);
                    }
                }
            }
        }

        public UserControl ConfigurationTabItem()
        {
            return null;
        }
    }
}
