﻿using EddiDataDefinitions;
using System;
using System.Collections.Generic;
using Utilities;

namespace EddiEvents
{
    public class StarScannedEvent : Event
    {
        public const string NAME = "Star scanned";
        public const string DESCRIPTION = "Triggered when you complete a scan of a stellar body";
        public static string SAMPLE = "{ \"timestamp\":\"2018-12-01T08:04:24Z\", \"event\":\"Scan\", \"ScanType\":\"AutoScan\", \"BodyName\":\"Arietis Sector UJ-Q b5-2\", \"BodyID\":0, \"DistanceFromArrivalLS\":0.000000, \"StarType\":\"L\", \"StellarMass\":0.218750, \"Radius\":249075072.000000, \"AbsoluteMagnitude\":11.808075, \"Age_MY\":10020, \"SurfaceTemperature\":1937.000000, \"Luminosity\":\"V\", \"RotationPeriod\":119097.164063, \"AxialTilt\":0.000000 }";

        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static StarScannedEvent()
        {
            VARIABLES.Add("absolutemagnitude", "The absolute (bolometric) magnitude of the star that has been scanned");
            VARIABLES.Add("absolutemagnitudeprobability", "The cumulative probability describing the star's age, relative to other stars of the same stellar class");
            VARIABLES.Add("age", "The age of the star that has been scanned, in millions of years");
            VARIABLES.Add("ageprobability", "The probablility of finding a star of this class with this age");
            VARIABLES.Add("alreadydiscovered", "Whether this star's scan data has already been registered with Universal Cartographics");
            VARIABLES.Add("bodyname", "The name of the star that has been scanned");
            VARIABLES.Add("chromaticity", "The apparent colour of the star that has been scanned");
            VARIABLES.Add("density", "The average density of the star, in kg per cubic meter");
            VARIABLES.Add("densityprobability", "The cumulative probability describing the star's density, relative to other stars of the same stellarclass");
            VARIABLES.Add("distance", "The distance in LS from the main star");
            VARIABLES.Add("eccentricity", "The orbital eccentricity of the star");
            VARIABLES.Add("eccentricityprobability", "The cumulative probability describing the star's orbital eccentricity, relative to other stars of the same stellar class");
            VARIABLES.Add("estimatedhabzoneinner", "The estimated inner radius of the habitable zone of the scanned star, in light seconds, not considering other stars in the system");
            VARIABLES.Add("estimatedhabzoneouter", "The estimated outer radius of the habitable zone of the scanned star, in light seconds, not considering other stars in the system");
            VARIABLES.Add("estimatedvalue", "The estimated value of the current scan");
            VARIABLES.Add("inclination", "The orbital inclination of the star, in degrees");
            VARIABLES.Add("inclinationprobability", "The cumulative probability describing the star's orbital inclination, relative to other stars of the same stellar class");
            VARIABLES.Add("luminosity", "The luminosity of the star that has been scanned");
            VARIABLES.Add("luminosityclass", "The luminosity class of the star that has been scanned");
            VARIABLES.Add("mainstar", "True if the star is the main / primary star in the star system");
            VARIABLES.Add("massprobability", "The probablility of finding a star of this class with this mass");
            VARIABLES.Add("orbitalperiod", "The number of seconds taken for a full orbit of the main star");
            VARIABLES.Add("orbitalperiodprobability", "The cumulative probability describing the star's orbital period about the main star, relative to other stars of the same stellar class");
            VARIABLES.Add("periapsis", "The argument of periapsis of the star, in degrees");
            VARIABLES.Add("periapsisprobability", "The cumulative probability describing the stars's argument of periapsis, relative to other stars of the same stellar class");
            VARIABLES.Add("radius", "The radius of the star that has been scanned, in metres");
            VARIABLES.Add("radiusprobability", "The probablility of finding a star of this class with this radius");
            VARIABLES.Add("rings", "The star's rings");
            VARIABLES.Add("rotationalperiod", "The number of seconds taken for a full rotation");
            VARIABLES.Add("rotationalperiodprobability", "The cumulative probability describing the stars's rotational period, relative to other stars of the same stellar class");
            VARIABLES.Add("scantype", "The type of scan event (AutoScan, Basic, Detailed, NavBeacon, NavBeaconDetail)");
            VARIABLES.Add("scoopable", "True if the star is scoopable (K, G, B, F, O, A, M)");
            VARIABLES.Add("semimajoraxis", "The semi major axis of the star's orbit, in light seconds");
            VARIABLES.Add("semimajoraxisprobability", "The cumulative probability describing the semi-major axis of the orbit of the star, relative to other stars of the same stellar class");
            VARIABLES.Add("solarmass", "The mass of the star that has been scanned, relative to Sol's mass");
            VARIABLES.Add("solarradius", "The radius of the star that has been scanned, compared to Sol");
            VARIABLES.Add("stellarclass", "The stellar class of the star that has been scanned (O, G, etc)");
            VARIABLES.Add("stellarsubclass", "The stellar sub class of the star that has been scanned (0 - 9, with 0 being hotter and 9 being cooler)");
            VARIABLES.Add("temperature", "The temperature of the star that has been scanned");
            VARIABLES.Add("tempprobability", "The probablility of finding a star of this class with this temperature");
            VARIABLES.Add("tilt", "Axial tilt for the star, in degrees (only available if DSS equipped)");
            VARIABLES.Add("tiltprobability", "The cumulative probability describing the star's orbital tilt, relative to other stars of the same stellar class");
        }

        // Variable names for this event should match the class property names for maximum compatibility with the BodyDetails() function in Cottle

        [PublicAPI]
        public decimal? absolutemagnitude => star.absolutemagnitude;

        [PublicAPI]
        public decimal? absolutemagnitudeprobability => star.absolutemagnitudeprobability;

        [PublicAPI]
        public long? age => star.age;

        [PublicAPI]
        public decimal? ageprobability => star.ageprobability;

        [PublicAPI]
        public bool? alreadydiscovered => star.alreadydiscovered;

        [PublicAPI]
        public string bodyname => star.bodyname;

        [PublicAPI]
        public string chromaticity => star.chromaticity;

        [PublicAPI]
        public decimal? density => star.density;

        [PublicAPI]
        public decimal? densityprobability => star.densityprobability;

        [PublicAPI]
        public decimal? distance => star.distance;

        [PublicAPI]
        public decimal? eccentricity => star.eccentricity;

        [PublicAPI]
        public decimal? eccentricityprobability => star.eccentricityprobability;

        [PublicAPI]
        public decimal? estimatedhabzoneinner => star.estimatedhabzoneinner;

        [PublicAPI]
        public decimal? estimatedhabzoneouter => star.estimatedhabzoneouter;

        [PublicAPI]
        public long? estimatedvalue => star.estimatedvalue;

        [PublicAPI]
        public decimal? inclination => star.inclination;

        [PublicAPI]
        public decimal? inclinationprobability => star.inclinationprobability;

        [PublicAPI]
        public decimal? luminosity => star.luminosity;

        [PublicAPI]
        public string luminosityclass => star.luminosityclass;

        [PublicAPI]
        public bool? mainstar => star.mainstar;

        [PublicAPI]
        public decimal? massprobability => star.massprobability;

        [PublicAPI]
        public decimal? orbitalperiod => star.orbitalperiod;

        [PublicAPI]
        public decimal? orbitalperiodprobability => star.orbitalperiodprobability;

        [PublicAPI]
        public decimal? periapsis => star.periapsis;

        [PublicAPI]
        public decimal? periapsisprobability => star.periapsisprobability;

        [PublicAPI]
        public decimal? radius => star.radius;

        [PublicAPI]
        public decimal? radiusprobability => star.radiusprobability;

        [PublicAPI]
        public List<Ring> rings => star.rings;

        [PublicAPI]
        public decimal? rotationalperiod => star.rotationalperiod;

        [PublicAPI]
        public decimal? rotationalperiodprobability => star.rotationalperiodprobability;

        [PublicAPI]
        public string scantype { get; private set; } // One of AutoScan, Basic, Detailed, NavBeacon, NavBeaconDetail
                                                     // AutoScan events are detailed scans triggered via proximity. 

        [PublicAPI]
        public bool scoopable => star.scoopable;

        [PublicAPI]
        public decimal? semimajoraxis => star.semimajoraxis;

        [PublicAPI]
        public decimal? semimajoraxisprobability => star.semimajoraxisprobability;

        [PublicAPI]
        public decimal? solarmass => star.solarmass;

        [PublicAPI]
        public decimal? solarradius => star.solarradius;

        [PublicAPI]
        public string stellarclass => star.stellarclass;

        [PublicAPI]
        public int? stellarsubclass => star.stellarsubclass;

        [PublicAPI]
        public decimal? temperature => star.temperature;

        [PublicAPI]
        public decimal? tempprobability => star.tempprobability;

        [PublicAPI]
        public decimal? tilt => star.tilt;

        [PublicAPI]
        public decimal? tiltprobability => star.tiltprobability;

        // Deprecated, maintained for compatibility with user scripts

        [Obsolete("Use distance instead")]
        public decimal? distancefromarrival => distance;

        [Obsolete("Use bodyname instead")]
        public string name => bodyname;

        [Obsolete("Use inclination instead")]
        public decimal? orbitalinclination => inclination;

        [Obsolete("Use rotationalperiod instead")]
        public decimal? rotationperiod => rotationalperiod;

        // Variables below are not intended to be user facing

        public bool? alreadymapped => star.alreadymapped;

        public long? bodyId => star.bodyId;

        public DateTime? mapped => star.mapped;

        public List<IDictionary<string, object>> parents => star.parents;

        public DateTime? scanned => star.scanned;

        public Body star { get; private set; }

        public StarScannedEvent(DateTime timestamp, string scantype, Body star) : base(timestamp, NAME)
        {
            this.star = star;
            this.scantype = scantype;
        }
    }
}
