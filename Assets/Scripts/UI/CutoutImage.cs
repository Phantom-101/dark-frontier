using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class CutoutImage : Image {
        private static readonly int iStencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering {
            get {
                var lMaterial = new Material(base.materialForRendering);
                lMaterial.SetInt(iStencilComp, (int)CompareFunction.NotEqual);
                return lMaterial;
            }
        }
    }
}