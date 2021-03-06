using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCGrid3DAction : CCGridAction
    {

        #region Constructors

        protected CCGrid3DAction (float duration) : base (duration)
        {
        }

        protected CCGrid3DAction (float duration, CCGridSize gridSize) : this (duration, gridSize, 0)
        {
        }

        protected CCGrid3DAction (float duration, CCGridSize gridSize, float amplitude)
            : base (duration, gridSize, amplitude)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCGrid3DActionState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCGrid3DActionState : CCGridActionState
    {
        CCRenderTexture gridRenderTexture;
        CCGrid3D grid3D;
        CCSize gridTextureSizeInPixels;

        public override CCGridBase Grid 
        {
            get 
            {
                if (Target != null && Target.GameView.DesignResolution != CCSize.Zero)
                {
                    var texelToContentSizeRatios = CCSize.One;
                    gridTextureSizeInPixels = Target.GameView.DesignResolution;
                }
                else
                {
                    // This probably serves no purpose but it keeps us from NRE's out
                    gridTextureSizeInPixels = new CCSize(GridSize.X, GridSize.Y);
                }

                var gridTexture = new CCRenderTexture(
                    gridTextureSizeInPixels,
                    gridTextureSizeInPixels,
                    CCSurfaceFormat.Color);
                grid3D = new CCGrid3D(GridSize, gridTexture);

                grid3D.Scene = Scene;

                return grid3D;
            }

            protected set 
            {
                Debug.Assert (value is CCGrid3D);
                grid3D = (CCGrid3D)value;
            }
        }

        public CCGrid3DActionState (CCGrid3DAction action, CCNodeGrid target) : base (action, target)
        {
        }

        #region Grid Vertex manipulation

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public CCVertex3F Vertex (CCGridSize pos)
        {
            return grid3D [pos];
        }

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public CCVertex3F Vertex (int x, int y)
        {
            return grid3D [x, y];
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public CCVertex3F OriginalVertex (CCGridSize pos)
        {
            return grid3D.OriginalVertex (pos);
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public CCVertex3F OriginalVertex (int x, int y)
        {
            return grid3D.OriginalVertex (x, y);
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex (CCGridSize pos, ref CCVertex3F vertex)
        {
            grid3D [pos] = vertex;
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex (int x, int y, ref CCVertex3F vertex)
        {
            grid3D [x, y] = vertex;
        }

        #endregion Grid Vertex manipulation
    }

    #endregion Action state
}