﻿/*
    BOSSE - Starcraft 2 Bot
    Copyright (C) 2022 Jesper Larsson

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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

    /// <summary>
    /// Runs background tasks while the main bot is running to offload the work on another CPU core
    /// </summary>
    public class BackgroundWorkerThread
    {
        private Thread ThreadInstance;
        
        public void StartThread()
        {
            ThreadInstance = new Thread(new ThreadStart(MainLoop));
            ThreadInstance.Name = "BotBackgroundThread";
            ThreadInstance.Start();
        }

        private void MainLoop()
        {
            while (true)
            {
                try
                {
                    StrategicMapSet.CalculateNewFromCurrentMapState();
                }
                catch (Exception ex)
                {
                    Log.Error("PERIODICAL THREAD EXCEPTION: " + ex);
                }

                Thread.Sleep(1000);
            }
        }
    }
}