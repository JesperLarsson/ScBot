﻿/*
 * Copyright Jesper Larsson 2019, Linköping, Sweden
 */
namespace BOSSE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading;

    using SC2APIProtocol;
    using Action = SC2APIProtocol.Action;
    using static CurrentGameState;
    using static UnitConstants;
    using static AbilityConstants;

    /// <summary>
    /// Holds all of our sensors
    /// </summary>
    public class SensorManager
    {
        /// <summary>
        /// Name => Sensor instance mapping
        /// </summary>
        private Dictionary<Sensor.SensorId, Sensor> ActiveSensors = new Dictionary<Sensor.SensorId, Sensor>();

        public void Initialize()
        {
            AddSensor(new OwnStructureWasCompletedSensor());
        }

        public void AddSensor(Sensor newSensor)
        {
            if (ActiveSensors.ContainsKey(newSensor.Id))
            {
                Log.Warning("Already have a sensor with name " + newSensor.ToString());
            }

            ActiveSensors[newSensor.Id] = newSensor;
            Log.Info("Added new sensor: " + newSensor.ToString());
        }

        public Sensor GetSensor(Sensor.SensorId id)
        {
            if (!ActiveSensors.ContainsKey(id))
            {
                return null;
            }

            return ActiveSensors[id];
        }

        /// <summary>
        /// Updates all sensors
        /// </summary>
        public void Tick()
        {
            foreach (var iter in ActiveSensors)
            {
                iter.Value.Tick();
            }
        }
    }
}
