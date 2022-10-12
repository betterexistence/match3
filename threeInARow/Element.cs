using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threeInARow
{
    public class Element
    {
        int x, y;
        public Element()
        {
            x = 0;
            y = 0;
        }

        public Element(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public Element(Element element)
        {
            x = element.x;
            y = element.y;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
    }
}
