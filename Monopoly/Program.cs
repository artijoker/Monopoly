using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monopoly {
    class Program {
        static void Main() {
            Console.OutputEncoding = Encoding.GetEncoding(1251);
            Console.InputEncoding = Encoding.GetEncoding(1251);

            Game monopoly = new Game(
                new Field(
                    XElement.Load(@"../../XML/Map.xml"),
                    XElement.Load(@"../../XML/Property.xml"),
                    XElement.Load(@"../../XML/Cards.xml")
                    )
                );
            monopoly.RunGame();
           
        }
    }
}
