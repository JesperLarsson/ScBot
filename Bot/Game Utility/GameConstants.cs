﻿/*
 * Copyright Jesper Larsson 2019, Linköping, Sweden
 */
namespace Bot
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

    /// <summary>
    /// Game constants not available in the wrapper layer
    /// </summary>
    public static class GameConstants
    {
        public const double FRAMES_PER_SECOND = 22.4;

    }
}
