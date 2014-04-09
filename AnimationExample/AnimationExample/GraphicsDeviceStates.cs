using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectSneakyGame
{
    /// <summary>
    /// Static class of pre-made GraphicsDevice states so memory does not have to be wasted when creating new GraphicsDevice states.
    /// </summary>
    public class GraphicsDeviceStates
    {
        static public BlendState Blend3DNormal = BlendState.AlphaBlend;                     // Default 3D GraphicsDevice blend state.
        static public RasterizerState Rasterizer3DNormal = RasterizerState.CullNone;        // Default 3D GraphicsDevice rasterizer state.
        static public SamplerState Sampler3DNormal = SamplerState.LinearWrap;               // Default 3D GraphicsDevice sampler state.
        static public DepthStencilState DepthStencil3DNormal = DepthStencilState.Default;   // Default 3D GraphicsDevice depth stencil state.
    }
}
