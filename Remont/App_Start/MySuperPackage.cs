using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(Remont.App_Start.MySuperPackage), "PreStart")]

namespace Remont.App_Start {
    public static class MySuperPackage {
        public static void PreStart() {
            MVCControlsToolkit.Core.Extensions.Register();
        }
    }
}