
namespace Monopoly {
    class StreetTradable : ITradable {
        public StreetTradable(Property property) {
            Property = property;
            Money = new Optional<int>();
        }
        public Property Property { get; }
        public Optional<int> Money { get; }
    }
}
