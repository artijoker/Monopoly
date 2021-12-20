using System;
using System.Xml.Linq;

namespace Monopoly {
    class Railway : Property{
        public Railway(
            string name,
            ConsoleColor color,
            int price,
            int rent,
            int pledge,
            int buyout,
            int increaseRent
            ) : base(name, color, price, rent, pledge, buyout) {
            IncreaseRent  = increaseRent;
            IsBeSold = true;
        }
        public override int IncreaseRent  { get; }
        public override bool IsBeSold { get; set; }

        public static Railway FromXElement(XElement element) {
            return new Railway(
                (string)element.Element("Name"),
                (ConsoleColor)((int)element.Element("Color")),
                (int)element.Element("Price"),
                (int)element.Element("Rent"),
                (int)element.Element("Pledge"),
                (int)element.Element("Buyout"),
                (int)element.Element("IncreaseRent")
                );

        }
    }
}
