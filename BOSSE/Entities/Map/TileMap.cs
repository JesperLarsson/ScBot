﻿/*
    BOSSE - Starcraft 2 Bot
    Copyright (C) 2020 Jesper Larsson

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
    using System.Runtime.CompilerServices;
    using System.Diagnostics;

    using SC2APIProtocol;
    using Action = SC2APIProtocol.Action;
    using static CurrentGameState;
    using static UnitConstants;

    /// <summary>
    /// Stores information about each tile of the ingame map
    /// </summary>
    [Serializable]
    public class TileMap<TileType>
    {
        public readonly int Width;
        public readonly int Height;
        private readonly TileType[,] Map;

        public TileMap()
        {
            Size2DI size = CurrentGameState.GameInformation.StartRaw.MapSize;

            this.Width = size.X;
            this.Height = size.Y;
            this.Map = new TileType[this.Width, this.Height];
        }

        public TileMap(int xSize, int ySize)
        {
            this.Width = xSize;
            this.Height = ySize;
            this.Map = new TileType[this.Width, this.Height];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TileType GetTile(int x, int y)
        {
            return this.Map[x, y];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTile(int x, int y, TileType value)
        {
            this.Map[x, y] = value;
        }
    }
}
