using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monopoly {
    class Street : Property {
        public Street( 
            string name,
            ConsoleColor color,
            int price, 
            int rent, 
            int pledge, 
            int buyout,
            int increaseRent,
            int housePurchase,
            int houseSale
            ) : base(name, color, price, rent, pledge, buyout) {

            IncreaseRent  = increaseRent;
            HousePurchase = housePurchase;
            HouseSale = houseSale;
            NumberOfHouses = 0;
            IsBeSold = true;
        }
        public int NumberOfHouses { get; set; }
        public int HousePurchase  { get; }
        public int HouseSale { get; }
        public override int IncreaseRent  { get; }
        public override bool IsBeSold { get; set; }

        public void BuyHouse() {
            Owner.Money -= HousePurchase;
            NumberOfHouses++;
            Rent += IncreaseRent ;
        }
        public void SellHouse() {
            Owner.Money += HouseSale;
            NumberOfHouses--;
            Rent -= IncreaseRent ;
        }


        public static Street FromXElement(XElement element) {
            return new Street(
                (string)element.Element("Name"),
                (ConsoleColor)((int)element.Element("Color")),
                (int)element.Element("Price"),
                (int)element.Element("Rent"),
                (int)element.Element("Pledge"),
                (int)element.Element("Buyout"),
                (int)element.Element("IncreaseRent"),
                (int)element.Element("HousePurchase"),
                (int)element.Element("HouseSale")
                );
        }

        
    }
}
