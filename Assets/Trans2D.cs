using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDG
{
    class Trans2D
    {
        private V2 _position;

        public V2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Ratio _rotation;

        public Ratio Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
    }
}
