using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LANMaker.Components
{
    public interface IMenuComponent
    {
        void Draw(float deltaSeconds, Vector2 parentPosition);
    }
}
