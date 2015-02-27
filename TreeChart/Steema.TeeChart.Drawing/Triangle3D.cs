namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle3D
    {
        public Point3D p0;
        public Point3D p1;
        public Point3D p2;
    }
}

