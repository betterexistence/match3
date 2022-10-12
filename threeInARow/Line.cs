using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threeInARow
{
    public class Line
    {
        public Line()
        {
            m_start = new Element();
            m_finish = new Element();
        }

        public Line(Element start, Element finish)
        {
            m_start = new Element(start);
            m_finish = new Element(finish);
        }

        public Element Start { get { return m_start; } set { m_start = new Element(value); } }
        public Element Finish { get { return m_finish; } set { m_finish = new Element(value); } }

        Element m_start;
        Element m_finish;
    }
}
